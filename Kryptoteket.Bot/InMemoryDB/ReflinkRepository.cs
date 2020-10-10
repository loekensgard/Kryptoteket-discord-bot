using Kryptoteket.Bot.Configurations;
using Kryptoteket.Bot.Interfaces;
using Kryptoteket.Bot.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.InMemoryDB
{
    public class ReflinkRepository : IReflinkRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseId = "Reflinks-db";
        private readonly string _containerId = "Reflinks-container";
        private Database _database;
        private Container _container;

        private readonly CosmosDBConfiguration _options;

        public ReflinkRepository(IOptions<CosmosDBConfiguration> options)
        {
            _options = options.Value;
            _cosmosClient = new CosmosClient(_options.EndpointUri, _options.PrimaryKey);
        }

        public async Task<bool> AddReflink(string name, string reflink)
        {
            var id = reflink.Substring(reflink.Length - 8);
            var item = new Reflinks { id = id, name = name, reflink = reflink };
            var created = false;
            try
            {
                if (string.IsNullOrEmpty(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
                if (string.IsNullOrEmpty(reflink)) throw new ArgumentException($"'{nameof(reflink)}' cannot be null or empty", nameof(reflink));

                await CreateDatabaseAsync();
                await CreateContainerAsync();

                var exists = await CheckIfExists(item);

                if (!exists)
                {
                    var reflinkResponse = await _container.CreateItemAsync<Reflinks>(item, new PartitionKey(item.name));
                    Log.Information("{string} added to database | Operation consumed {consumed} RUs.\n", reflinkResponse.Resource, reflinkResponse.RequestCharge);
                    created = true;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed adding {reflink} to db");
            }

            return created;
        }

        //Cosmos make you do some ugly shit
        private async Task<bool> CheckIfExists(Reflinks item)
        {
            var check = true;
            try
            {
                var exists = await _container.ReadItemAsync<Reflinks>(item.id, new PartitionKey(item.name));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound) 
            {
                check = false;
            }
            return check;
        }

        public async Task<List<string>> GetReflinks()
        {
            await CreateDatabaseAsync();
            await CreateContainerAsync();

            var sqlQuery = "SELECT * FROM c";
            var queryDefinition = new QueryDefinition(sqlQuery);
            var queryResultSetIterator = _container.GetItemQueryIterator<Reflinks>(queryDefinition);

            var reflinks = new List<string>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Reflinks reflink in currentResultSet)
                {
                    reflinks.Add(reflink.reflink);
                }
            }

            return reflinks;
        }

        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
        }

        private async Task CreateContainerAsync()
        {
            // Create a new container
            _container = await _database.CreateContainerIfNotExistsAsync(_containerId, "/name");
        }


    }
}
