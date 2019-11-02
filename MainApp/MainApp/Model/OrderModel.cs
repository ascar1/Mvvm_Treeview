using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    public class OrderModel
    {
        public string Tiker { get; set; }
        public string Type { get; set; }// Long/Short
        public int Vol { get; set;}
        public double Price { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsStop { get; set; }
        public bool IsExecute { get; set; }
        public DateTime ExecuteDate { get; set; }
        public DateTime CanceledData { get; set; }
    }
}
