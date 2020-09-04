using System;
using System.Collections.Generic;
using System.Text;

namespace Kryptoteket.Bot.Models
{
    public class ChartData
    {
        public List<double> Data { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
    }
}
