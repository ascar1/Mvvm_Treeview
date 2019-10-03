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
        public List<WorkPointModel> WorkPoints;

        private List<FileArrModel> fileArrs;
        public bool isClear;

        public IteratorModel(List<MasterPointModel> MasterChartPoint,List<FileArrModel> files)
        {
            isClear = true;
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
            GetSeed();
        }

        public bool next()
        {
            bool flag = false;
            foreach (MasterPointModel i in MasterPoint )
            {
                int index = WorkPoints.FindIndex(ii => ii.Tiker == i.Tiker);
                int tmp1 = WorkPoints[index].Data.Find(i1 => i1.Scale == "60").Points.Count;
                if (tmp1 < i.Data[0].Points.Count)
                {
                    flag = true;
                    var tmp = i.Data[0].Points[WorkPoints[index].Data.Find(i1 => i1.Scale == "60").Points.Count];
                    WorkPoints[index].Data.Find(i1 => i1.Scale == "60").Points.Add(tmp);
                    GetScale(WorkPoints[index].Data);
                    GetIndex(WorkPoints[index].Data);
                }
            }
            return flag;
        }

        public void GetSeed ()
        {
            for (int i = 0; i <= Seed; i++)
            {
                next();
            }
        }

        public void GetScale(List<DateModel> dateModels)
        {
            DateTime CurrDate;
            DateTime Date1;
            
            if (dateModels.Count == 1)
            {
                dateModels.Add(new DateModel()
                {
                    Points = new List<PointModel>(),
                    Scale = "D"                
                });
                dateModels.Find(i => i.Scale == "D").Points.Add(dateModels[0].Points[0]);
            }
            else
            {
                CurrDate = dateModels.Find(i => i.Scale == "60").Points.Last().Date;
                Date1 = dateModels.Find(i => i.Scale == "D").Points.Last().Date;
                if (CurrDate.Date == Date1.Date)
                {
                    PointModel pointD = dateModels.Find(i => i.Scale == "D").Points.Last();
                    PointModel point60 = dateModels.Find(i => i.Scale == "60").Points.Last();
                    pointD.High = Math.Max(pointD.High, point60.High);
                    pointD.Low = Math.Min(pointD.High, point60.High);
                    pointD.Close = point60.Close;
                    pointD.Vol = pointD.Vol + point60.Vol;
                }
                else
                {
                    PointModel point60 = dateModels.Find(i => i.Scale == "60").Points.Last();
                    dateModels.Find(i => i.Scale == "D").Points.Add(new PointModel()
                    {
                        Close = point60.Close,
                        Vol = point60.Vol,
                        Date = point60.Date,
                        High = point60.High,
                        Low = point60.Low,
                        Open = point60.Open
                    });
                }
            }
        }
        public void GetIndex(List<DateModel> dateModels)
        {
            IndexModel indexModel = new IndexModel();
            List<LavelModel> lavelModels = new List<LavelModel>();            
            ParamDataService paramDataService = new ParamDataService();           
            paramDataService.GetDataLevel((item, error) => { if (error != null) { return; } lavelModels = item; });
            foreach(var tmp in lavelModels.FindAll(i=> i.paremtId == lavelModels.Find(i1 => i1.name == "Index").id))
            {                
                List<ParamModel> tmp2 = paramDataService.GetParam(tmp.id);
                string type = tmp2.Find(i => i.name.Trim() == "Type").val;                
                switch (type)
                {
                    case "EMA":
                        //MessageBox.Show("EMA");
                        indexModel.GetEMA(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                        indexModel.GetEMA(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                        break;
                    case "MACD":
                        //MessageBox.Show("MACD");
                        //indexModel.GetMACD(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                        //indexModel.GetMACD(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                        break;
                    case "FI":
                        //MessageBox.Show("FI");
                        //indexModel.GetForceIndex(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                        //indexModel.GetForceIndex(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                        break;
                }
            }             
        }
        public void All()
        {
            while(next())
            {

            }
          //  MessageBox.Show("All");
        }
    }
}
