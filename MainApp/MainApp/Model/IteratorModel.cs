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
        private static IteratorModel instance;
        public static IteratorModel GetInstance(List<MasterPointModel> MasterChartPoint, List<FileArrModel> files)
        {
            if (instance == null)
                instance = new IteratorModel(MasterChartPoint, files);
            return instance;
        }

        private readonly int Seed = 480;
        private List<MasterPointModel> MasterPoint;        
        public List<WorkPointModel> WorkPoints;
        private List<FileArrModel> fileArrs;
        public List<OrderModel> orderModels { get; set; }
        public List<DealModel> dealModels { get; set; }
        public bool isClear;

        protected IteratorModel(List<MasterPointModel> MasterChartPoint,List<FileArrModel> files)
        {
            isClear = true;
            MasterPoint = MasterChartPoint;
            WorkPoints = new List<WorkPointModel>();
            orderModels = new List<OrderModel>();
            dealModels = new List<DealModel>();
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
            for (int item = 0; item <= Seed; item++)
            {
                foreach (MasterPointModel i in MasterPoint)
                {
                    int index = WorkPoints.FindIndex(ii => ii.Tiker == i.Tiker);
                    int tmp1 = WorkPoints[index].Data.Find(i1 => i1.Scale == "60").Points.Count;
                    if (tmp1 < i.Data[0].Points.Count)
                    {
                        var tmp = i.Data[0].Points[WorkPoints[index].Data.Find(i1 => i1.Scale == "60").Points.Count];
                        WorkPoints[index].Data.Find(i1 => i1.Scale == "60").Points.Add(tmp);
                        GetScale(WorkPoints[index].Data);
                        GetIndex(WorkPoints[index].Data);
                    }
                }
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
            foreach(var tmp in lavelModels.FindAll(i=> i.ParemtId == lavelModels.Find(i1 => i1.Name == "Index").Id))
            {                
                List<ParamModel> tmp2 = paramDataService.GetParam(tmp.Id);
                if (tmp2.Count != 0)
                {
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
                            //DateTime date = new DateTime(2019, 11, 12, 19, 00, 00);
                            //if (dateModels.Find(i => i.Scale == "60").Points.Last().Date == date) { MessageBox.Show("!"); }
                            indexModel.GetMACD(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                            break;
                        case "FI":
                            //MessageBox.Show("FI");
                            indexModel.GetForceIndex(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                            indexModel.GetForceIndex(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                            break;
                        case "ATR":
                            indexModel.GetATR(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                            indexModel.GetATR(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                            break;
                        case "ADX":
                            indexModel.GetFinamADX(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                            indexModel.GetFinamADX(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                            break;
                        case "CCI":
                            indexModel.GetCCI(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                            indexModel.GetCCI(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                            break;
                        case "PC":
                            indexModel.GetPraceChanel(dateModels.Find(i => i.Scale == "60").Points, tmp2);
                            indexModel.GetPraceChanel(dateModels.Find(i => i.Scale == "D").Points, tmp2);
                            break;
                    }
                }
            }             
        }
        private void GetAnalysis(List<DateModel> dateModels, string tiker)
        {
            IndexModel indexModel = new IndexModel();
            if (dateModels.Find(i=> i.Scale == "60").Points.Last().Date.Hour == 19)
            {
                IAnalysis analysis1 = new Analysis1(dateModels.Find(i => i.Scale == "D"), "Analysis1");
                analysis1.GetAnalysis();
            }



            int item = dateModels.FindIndex(i => i.Scale == "D");
            int itemAnalysis1 = dateModels[item].Points.Last().AnalysisResults.FindIndex(i => i.Name == "Analysis1");
            string A1Result = "";
            if (itemAnalysis1 != -1)
            {
                A1Result = dateModels[item].Points.Last().AnalysisResults[itemAnalysis1].Result;
            }
            else
            {
                int count = dateModels[item].Points.Count();
                itemAnalysis1 = dateModels[item].Points[count - 2].AnalysisResults.FindIndex(i => i.Name == "Analysis1");
                if (itemAnalysis1 != -1)
                {
                    A1Result = dateModels[item].Points[count - 2].AnalysisResults[itemAnalysis1].Result;
                }
                else
                {
                    A1Result = "";
                }
                
            }
            
            //if (item >= 0)
            //{
            //    item = dateModels[item].Points.Last().AnalysisResults.FindIndex(i => i.Name == "Analysis1");
            //    if (item >= 0)
            //    {
                    //A1Result = dateModels.Find(i => i.Scale == "D").Points.Last().AnalysisResults[item].Result;
                    //if (tiker == "GAZP") { }
                    Analysis2 analysis2 = new Analysis2(dateModels.Find(i => i.Scale == "60"), "Analysis2", tiker, A1Result);
                    analysis2.GetAnalysis();
                    // Удаляем ордер если сигнала нет 
                    int itemDeal = 0;
                    if (analysis2.GetResult() == "")
                    {
                        itemDeal = orderModels.FindIndex(i => i.Tiker == tiker & i.IsActive == true);
                        if (itemDeal != -1)
                        {
                            orderModels[itemDeal].IsActive = false;
                            orderModels[itemDeal].EndDate = dateModels[0].Points.Last().Date;
                        }                        
                    }
                    // проверяем на наличие копий ордера И открытых позиций                      
                    if (analysis2.HaveOrder)
                    {
                        itemDeal = dealModels.FindIndex(i => i.Tiker == tiker & i.InMarket == true);
                        if (itemDeal == -1)
                        {
                            itemDeal = orderModels.FindIndex(i => i.Tiker == tiker & i.IsActive == true);
                            if (itemDeal == -1)
                            {
                                orderModels.Add(analysis2.OrderModels);
                            }
                            else
                            {
                                orderModels[itemDeal].Price = analysis2.OrderModels.Price;
                                //orderModels.Add(analysis2.OrderModels);
                            }
                        }
                    }

                    itemDeal = dealModels.FindIndex(i => i.Tiker == tiker & i.InMarket == true);
                    if (itemDeal != -1)
                    {
                        if (A1Result != "")
                        {
                            if (dateModels.Find(i => i.Scale == "60").Points.Last().Date.Hour == 19)
                            {
                                //dealModels[itemDeal].StopPrice = dateModels.Find(i => i.Scale == "D").Points.Last().IndexPoint.Find(i => i.Name == "EMA16").Value[0].Value;
                                double tmp = dateModels.Find(i => i.Scale == "60").Points.Last().IndexPoint.Find(i => i.Name == "ATR").Value.Find(i1 => i1.Name == "ATR").Value;
                                double stop = (indexModel.GetMax(dateModels.Find(i => i.Scale == "60").Points, 22) - (tmp*2.5) );
                                if (stop > 0)
                                {
                                    dealModels[itemDeal].StopPrice = stop;
                                }
                                else
                                {
                                    dealModels[itemDeal].StopPrice = (indexModel.GetMax(dateModels.Find(i => i.Scale == "60").Points, 22) - (tmp * 2.5));
                                }
                            }
                        }
                        else
                        {
                            //double tmp = dateModels.Find(i => i.Scale == "D").Points.Last().IndexPoint.Find(i => i.Name == "ATR").Value.Find(i1 => i1.Name == "ATR").Value;
                            if (dateModels.Find(i => i.Scale == "60").Points.Last().Date.Hour == 19)
                            {
                                //dealModels[itemDeal].StopPrice = (indexModel.GetMax(dateModels.Find(i => i.Scale == "60").Points, 22) - (tmp * 3));
                                dealModels[itemDeal].StopPrice = dateModels.Find(i => i.Scale == "60").Points.Last().Close; //IndexPoint.Find(i => i.Name == "EMA8").Value[0].Value;
                            }
                        }
                        //Analysis3 analysis3 = new Analysis3(dateModels.Find(i => i.Scale == "60"), "Analysis3", tiker, A1Result);
                        //analysis3.GetAnalysis();
                        //if (analysis3.StopOrder != 0)
                        //{
                        //    dealModels.Find(i => i.Tiker == tiker & i.InMarket == true).StopPrice = analysis3.StopOrder;
                        //}
                //    }
                //}
            }
        }
        private string GetResult(DateModel data, string name)
        {
            for (int i = data.Points.Count-1; i > 0; i--)
            {
                int item = data.Points[i].AnalysisResults.FindIndex(ii => ii.Name == name);
                if (item != -1)
                {
                    return data.Points[i].AnalysisResults[item].Result;
                }
            }

            return "";
        }
        private void _GetAnalysis(List<DateModel> dateModels, string tiker)
        {
            IndexModel indexModel = new IndexModel();
            DateTime date = new DateTime(2019, 1, 10, 19, 00, 00);
            if ((dateModels.Find(i => i.Scale == "60").Points.Last().Date == date) && (tiker == "YNDX")) { MessageBox.Show("!"); }

            if (dateModels.Find(i => i.Scale == "60").Points.Last().Date.Hour == 19)
            {
                IAnalysis analysis1 = new Analysis1(dateModels.Find(i => i.Scale == "D"), "Analysis1");
                analysis1.GetAnalysis();
            }


            string A1Result = GetResult(dateModels.Find(i => i.Scale == "D"), "Analysis1");

            Analysis2 analysis2 = new Analysis2(dateModels.Find(i => i.Scale == "D"), "Analysis2", tiker, A1Result);
            analysis2.GetAnalysis();

            // Удаляем ордер если сигнала нет             
            if (A1Result == "")
            {
                int itemDeal = orderModels.FindIndex(i => i.Tiker == tiker & i.IsActive == true);
                if (itemDeal != -1)
                {
                    orderModels[itemDeal].IsActive = false;
                    orderModels[itemDeal].EndDate = dateModels[0].Points.Last().Date;
                }
            }
            // Проверяем на наличие копий ордера и открытых позиций
            if (analysis2.HaveOrder)
            {
                int itemDeal = dealModels.FindIndex(i => i.Tiker == tiker & i.InMarket == true);
                if (itemDeal == -1)
                {
                    itemDeal = orderModels.FindIndex(i => i.Tiker == tiker & i.IsActive == true);
                    if (itemDeal == -1)
                    {
                        orderModels.Add(analysis2.OrderModels);
                    }
                    else
                    {
                        orderModels[itemDeal].Price = analysis2.OrderModels.Price;
                    }
                }

            }
            // Обработка открытой позиции 
            Analysis3 analysis3 = new Analysis3(dateModels.Find(i => i.Scale == "D"), "Analysis3", tiker, A1Result);
            analysis3.GetAnalysis();
            int indexDeal = dealModels.FindIndex(i => i.Tiker == tiker & i.InMarket == true);
            if (indexDeal != -1)
            {
                dealModels[indexDeal].StopPrice = analysis3.StopOrder;
            }

        }

        private void ChekDeal(List<DateModel> dateModels, string tiker)
        {
            DateTime CurrDate = dateModels.Find(i => i.Scale == "60").Points.Last().Date;
            #region Проверить и удалить просроченные ордера 
            int item = orderModels.FindIndex(i => i.Tiker == tiker & i.IsActive == true & i.IsExecute == false);
            if (item >= 0)
            { 
                if ((orderModels[item].BeginDate < CurrDate) )
                {
                    var TmpPoint = dateModels.Find(i => i.Scale == "60").Points.Last();                
                    if ((orderModels[item].Price < TmpPoint.High ) && (orderModels[item].Price > TmpPoint.Low  ) && (CurrDate.Hour != 11))
                    {
                            orderModels[item].IsExecute = true;
                            orderModels[item].IsActive = false;
                            orderModels[item].ExecuteDate = CurrDate;
                            dealModels.Add(new DealModel()
                            {
                                Tiker = orderModels[item].Tiker,
                                Type = orderModels[item].Type,
                                Vol = orderModels[item].Vol,
                                InMarket = true,
                                OpenDate = CurrDate,
                                
                                StopPrice = dateModels.Find(i => i.Scale == "D").Points.Last().IndexPoint.Find(i => i.Name == "EMA16").Value[0].Value,
                                OpenPrice = orderModels[item].Price,
                            });                       
                    }
                }
                else if (CurrDate > orderModels[item].EndDate)
                {
                    //orderModels[item].IsActive = false;
                }

            }
            #endregion
            #region Закрыть по Стоп ордеру 
            item = dealModels.FindIndex(i => i.Tiker == tiker & i.InMarket == true);
            if (item >= 0)
            {
                var TmpPoint = dateModels.Find(i => i.Scale == "60").Points.Last();
                if ((dealModels[item].StopPrice < TmpPoint.High) && (dealModels[item].StopPrice > TmpPoint.Low))
                {
                    dealModels[item].ClosePrice = dealModels[item].StopPrice;
                    dealModels[item].CloseDate = TmpPoint.Date;
                    dealModels[item].InMarket = false;
                }
            }
            #endregion
        }
        public void All()
        {
            while(Next())
            {

            }            
        }
        public bool Next()
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
                    _GetAnalysis(WorkPoints[index].Data, WorkPoints[index].Tiker);
                    ChekDeal(WorkPoints[index].Data,WorkPoints[index].Tiker);
                }
            }
            return flag;
        }
        public void NextN(int n)
        {
            for(int i=0; i<n;i++)
            {
               
                if (!Next())
                {
                    break;
                }
            }            
        }
        public List<PointModel> GetListPoint(string tiker, string skale) => WorkPoints.Find(i => i.Tiker == tiker).Data.Find(i1 => i1.Scale == skale).Points;

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
                    string type = paramDataService.GetParamValue(tmp.Id, "ChartArea");
                    string name = paramDataService.GetParamValue(tmp.Id, "Name");
                    
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
