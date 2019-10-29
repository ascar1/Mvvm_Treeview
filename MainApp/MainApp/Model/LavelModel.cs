using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    public class LavelModel
    {
        public LavelModel()
        {
            this.ParemtId = -1;
        }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Id { get; set; }
        public int ParemtId { get; set; }
    }
}
