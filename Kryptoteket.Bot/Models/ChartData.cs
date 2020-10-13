using System.Collections.Generic;

namespace Kryptoteket.Bot.Models
{
    public class ChartData
    {
        public List<double> Data { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
    }
}
