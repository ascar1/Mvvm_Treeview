using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    public class DealModel
    {
        public string Tiker { get; set; }
        public string Type { get; set; }
        public DateTime OpenDate { get; set; }       
        public double OpenPrice { get; set; }
        public double StopPrice { get; set; }
        public int Vol { get; set; }
        public double ClosePrice { get; set; }
        public DateTime CloseDate { get; set; }
        public bool InMarket { get; set; }
        //public List<HistoryDeal> History { get; set; }
    }

    public class HistoryDeal
    {
        public DateTime Date { get; set; }
        public Double Open { get; set; }
        public Double High { get; set; }
        public Double Low { get; set; }
        public Double Close { get; set; }
        public double StopPrice { get; set; }
    }

}
