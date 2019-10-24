using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MainApp.Model.Analysis;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MainApp.Model
{
    public class IteratorModel
    {
        private readonly int Seed = 10;
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
                List<DateModel> dateModels = new List<DateModel>
                {
                    new DateModel()
                    {
                        Scale = "60",
                        Points = new List<PointModel>()
                    }
                };
                WorkPoints.Add(new WorkPointModel() {
                    Tiker = i.Tiker,
                    CurrDate = i.SDate,
                    SDate = i.SDate,
                    Data = dateModels
                });
            }
            GetSeed();
        }
        
        private void GetSeed ()
        {
            for (int i = 0; i <= Seed; i++)
            {
                Next();
            }
        }
        private void GetScale(List<DateModel> dateModels)
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
                    pointD.Low = Math.Min(pointD.Low, point60.Low);
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
        private void GetIndex(List<DateModel> dateModels)
        {
            IndexModel indexModel = new IndexModel();
            List<LavelModel> lavelModels = new List<LavelModel>();            
            ParamDataService paramDataService = new ParamDataService();           
            paramDataService.GetDataLevel((item, error) => { if (error != null) { return; } lavelModels = item; });
            foreach(var tmp in lavelModels.FindAll(i=> i.paremtId == lavelModels.Find(i1 => i1.name == "Index").id))
            {                
                List<ParamModel> tmp2 = paramDataService.GetParam(tmp.id);
                string type = tmp2.Find(i => i.Name.Trim() == "Type").Val;                
                switch (type)
                {
                    case "EMA":
                        //MessageBox.Show("EMA");
                        indexModel.GetEMA(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                        indexModel.GetEMA(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                        break;
                    case "MACD":
                        //MessageBox.Show("MACD");
                        indexModel.GetMACD(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                        indexModel.GetMACD(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                        break;
                    case "FI":
                        //MessageBox.Show("FI");
                        indexModel.GetForceIndex(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                        indexModel.GetForceIndex(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                        break;
                }
            }             
        }
        private void GetAnalis(List<DateModel> dateModels)
        {
            IAnalysis1 analysis1 = new Analysis1(dateModels.Find(i => i.Scale == "D"));
        }
        public void All()
        {
            while(Next())
            {

            }
          //  MessageBox.Show("All");
        }
        public bool Next()
        {         
           bool 
            flag = false;
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
        public void NextN(int n)
        {
            for(int i=0; i<n;i++)
            {
                Next();
            }            
        }
        public List<PointModel> GetListPoint (string tiker, string skale)
        {
            return WorkPoints.Find(i => i.Tiker == tiker).Data.Find(i1 => i1.Scale == skale).Points;
        }
        #region Методы для работы с графиками
        private int GetIndexSeries(SeriesCollection series, string name)
        {
            int i = 0;
            foreach (var tmp in series)
            {
                if (tmp.Title == name)
                {
                    return i;
                }
                i++;
            }
            if (name.IndexOf("Bar_Graph") == -1)
            {
                series.Add(new LineSeries
                {
                    Title = name,
                    Name = name,
                    Values = new ChartValues<double>(),
                    Fill = Brushes.Transparent,
                    PointGeometry = Geometry.Empty

                });
            }
            else
            {
                series.Add(new ColumnSeries
                {
                    Title = name,
                    Name = name,
                    Values = new ChartValues<double>(),
                    
                    PointGeometry = Geometry.Empty

                });
            }

            return i;
        }
        private void AddSeries (SeriesCollection series, List<IndexPoint> points, string name)
        {
           // int i = GetIndexSeries(series, name);
            foreach(var tmp in points)
            {
                if (tmp.Name.Substring(0,1) != "_")
                {
                    int i = GetIndexSeries(series, name + "_" + tmp.Name);
                    if (tmp.Name.IndexOf("FI") == -1)
                    {
                        series[i].Values.Add(tmp.Value);
                    }
                    else
                    {
                        series[i].Values.Add(tmp.Value);
                    }
                        
                }
            }
        }
        public SeriesCollection GetSeriesCollection (string tiker, string skale,string ChartArea, int From, int To, int index)
        {
            SeriesCollection result;
            if (ChartArea == "0")
            {
                result = new SeriesCollection
                {
                    new OhlcSeries()
                    {
                        Title = tiker,
                         Name = tiker,
                         Values = new ChartValues<OhlcPoint>()
                    }
                };
            }
            else
            {
                result = new SeriesCollection
                {
                    new LineSeries()
                    {
                         Values = new ChartValues<double>(),
                         PointGeometry = Geometry.Empty
                    }
                };
            }
            
            ParamDataService paramDataService = new ParamDataService();
            List<PointModel> point = GetListPoint(tiker, skale);
            // Если To == 0 то выводим всю серию
            if (To == 0)
            {
                To = point.Count - 1;
            }

            for (int i = From*index; i <= To*index; i++)
            {
                int ind = 0;
                // По умолчанию для ChartArea0 выводм серию графика цен
                if (ChartArea == "0")
                {
                    result[ind].Values.Add(new OhlcPoint
                    {
                        Close = point[i].Close,
                        High = point[i].High,
                        Low = point[i].Low,
                        Open = point[i].Open
                    });
                    ind++;
                }
                // Выводим индексы на серию Области графика 
                foreach (var tmp in paramDataService.GetLavelModels("Index"))
                {
                    string type = paramDataService.GetParamValue(tmp.id, "ChartArea");
                    string name = paramDataService.GetParamValue(tmp.id, "Name");
                    
                    if (type == ChartArea)
                    {
                        //int tmpind = GetIndexSeries(result, name);
                        //double tmpval = point[i].IndexPoint.Find(i1 => i1.Name == name).Value[0].Value;

                        AddSeries(result, point[i].IndexPoint.Find(i1 => i1.Name == name).Value, name);
                        
                        //result[tmpind].Values.Add(tmpval);
                        ind++;
                    }
                }
            }           
            return result;
        }
        public List<string> GetArrDate(string tiker, string skale, int From, int To, int index)
        {
            List<string> result = new List<string>();
            List<PointModel> point = GetListPoint(tiker, skale);
            // Если To == 0 то выводим всю серию
            if (To == 0)
            {
                To = point.Count - 1;
            }

            for (int i = From*index; i < To*index; i++)
            {
                result.Add(point[i].Date.ToString("dd MMM"));
            }
            return result;
        }
        public List<string> GetScaleArrDate(string tiker, string skale, int KolPoint)
        {
            List<string> result = new List<string>();
            List<PointModel> point = GetListPoint(tiker, skale);

            int scale1 = point.Count / KolPoint;
            if (scale1 == 0) { scale1 = 1; }
            for (int i = 0; i < point.Count; i = i + scale1  )
            {
                result.Add(point[i].Date.ToString("dd MMM"));
            }
            return result;
        }
        public SeriesCollection GetScaleSeriesCollection(string tiker, string skale, string ChartArea, int KolPoint)
        {
            List<string> NLabels = new List<string>();
            SeriesCollection result;
            result = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> (),
                    Fill= Brushes.LightGray,
                    Stroke=Brushes.Gray,
                    PointGeometry = Geometry.Empty,
                    //AreaLimit=0
                }
            };

            List<PointModel> point = GetListPoint(tiker, skale);            
            
            int scale1 = point.Count / KolPoint;
            if (scale1 == 0) { scale1 = 1; }
            for (int i = 0; i < point.Count; i = i + scale1)
            {                
                if (result.Count == 0)
                {
                    result.Add(new LineSeries { });
                };
                result[0].Values.Add(point[i].Close);                
                NLabels.Add(point[i].Date.ToString("dd MMM"));
            }        
            return result;
        }
        #endregion

    }
}
