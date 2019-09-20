using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainApp.Model
{
    class IteratorModel
    {
        private int Seed = 10;
        private List<MasterPointModel> MasterPoint;
        private List<FileArrModel> fileArrs;
        public List<WorkPointModel> WorkPoints;

        public IteratorModel(List<MasterPointModel> MasterChartPoint,List<FileArrModel> files)
        {
            MasterPoint = MasterChartPoint;
            WorkPoints = new List<WorkPointModel>();
            fileArrs = files;
            foreach (var i in MasterPoint)
            {
                List<DateModel> dateModels = new List<DateModel>();
                dateModels.Add(new DateModel()
                {
                    Scale = "60",
                    Points = new List<PointModel>()
                });
                WorkPoints.Add(new WorkPointModel() {
                    Tiker = i.Tiker,
                    CurrDate = i.sDate,
                    sDate = i.sDate,
                    Data = dateModels
                });
            }
        }

        public bool next()
        {
            foreach (var i in MasterPoint )
            {
                int index;
                // WorkPoints.Find(ii => ii.Tiker == i.Tiker).Data[0].Points.Add(new PointModel());
                index = WorkPoints.FindIndex(ii => ii.Tiker == i.Tiker);
                int intdex1 = WorkPoints[index].Data[0].Points.Count();
                MessageBox.Show("TabData AllCommand");
            }
            return true;
        }

        public void GetSeed ()
        {
            for (int i = 0; i <= Seed; i++)
            {
                next();
            }
        }
    }
}
