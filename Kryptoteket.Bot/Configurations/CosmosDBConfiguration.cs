using System;
using System.Collections.Generic;
using System.Text;

namespace Kryptoteket.Bot.Configurations
{
    public class CosmosDBConfiguration
    {
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set; }
    }
}
