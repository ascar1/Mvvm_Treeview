using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    class IteratorModel
    {
        private List<MasterPointModel> MasterPoint;

        public IteratorModel(List<MasterPointModel> MasterChartPoint)
        {
            MasterPoint = MasterChartPoint;
        }

        public bool next()
        {
            return true;
        }
    }
}
