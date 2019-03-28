using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLight1.Model
{
    public class LavelModel{
        public LavelModel()
        {
            this.paremtId = -1;            
        }
        public string name { get; set; }
        public string comment { get; set; }
        public int id { get; set; }
        public int paremtId { get; set; }        
    }
}
