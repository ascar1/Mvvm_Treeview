using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLight1.Model
{
    public class ParamModel
    {
        public int id { get; set; }
        public int ParamID
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }
        public string val
        {
            get;
            set;
        }
        public string comment
        {
            get;
            set;
        }
    }
}
