using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Services
{
    public class HttpResponseService
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();
            }

            return content;
        }

        public async Task<T> DeserializeJsonFromStream<T>(HttpResponseMessage response)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<T>(contentStream, _options);
            return result;
        }
    }
}
