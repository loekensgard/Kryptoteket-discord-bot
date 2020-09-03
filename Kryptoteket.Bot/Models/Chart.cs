using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class Chart
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
    public partial class Data
    {
        [JsonPropertyName("datasets")]
        public List<Dataset> Datasets { get; set; }
    }
    public partial class Dataset
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("data")]
        public List<long> Data { get; set; }

        [JsonPropertyName("fill")]
        public bool Fill { get; set; }

        [JsonPropertyName("borderColor")]
        public string BorderColor { get; set; }
    }
}
