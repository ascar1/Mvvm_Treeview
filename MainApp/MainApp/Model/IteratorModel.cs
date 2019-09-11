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
        private List<FileArrModel> fileArrs;
        public List<WorkPointModel> WorkPoints;

        public IteratorModel(List<MasterPointModel> MasterChartPoint,List<FileArrModel> files)
        {
            MasterPoint = MasterChartPoint;
            fileArrs = files;
        }

        public bool next()
        {
            return true;
        }
    }
}
