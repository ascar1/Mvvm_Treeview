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
        public int Vol { get; set; }
        public double OpenPrice { get; set; }
        public double StopPrice { get; set; }
        public DateTime OpenDate { get; set; }
        public double ClosePrice { get; set; }
        public DateTime CloseDate { get; set; }
        public bool InMarket { get; set; }
    }
}
