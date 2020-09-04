using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Kryptoteket.Bot.Models
{
    public class ChartBuilder
    {
        [JsonPropertyName("chart")]
        public Chart Chart { get; set; }
    }

    public partial class Chart
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
        [JsonPropertyName("options")]
        public Options Options { get; set; }
    }
    public partial class Data
    {
        [JsonPropertyName("datasets")]
        public List<Dataset> Datasets { get; set; }
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; }
    }
    public partial class Dataset
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("data")]
        public List<double> Data { get; set; }

        [JsonPropertyName("fill")]
        public bool Fill { get; set; }

        [JsonPropertyName("borderColor")]
        public string BorderColor { get; set; }
    }
    public partial class Options
    {
        [JsonPropertyName("scales")]
        public Scales Scales { get; set; }
    }

    public partial class Scales
    {
        [JsonPropertyName("yAxes")]
        public List<YAx> YAxes { get; set; }
    }

    public partial class YAx
    {
        [JsonPropertyName("ticks")]
        public Ticks Ticks { get; set; }
    }

    public partial class Ticks
    {
        [JsonPropertyName("suggestedMin")]
        public double SuggestedMin { get; set; }

        [JsonPropertyName("suggestedMax")]
        public double SuggestedMax { get; set; }
    }

}
