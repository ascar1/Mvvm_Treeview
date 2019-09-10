using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    class ChartPointModel
    {
        public string Tiker { get; set; }
        public List<DateModel> Data { get; set; }
    }

    class DateModel
    {
        public string Scale { get; set; }
        public List<PointModel> Points { get; set; }
    }
    class PointModel
    {
        public DateTime Date { get; set; }
        public Double Open { get; set; }
        public Double High { get; set; }
        public Double Low { get; set; }
        public Double Close { get; set; }
        public Double Vol { get; set; }
        public Double OpenInt { get; set; }
        public List<IndexModel> IndexPoint { get; set; }

    }
    class IndexModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
    }
}
