﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainApp.Model.Analysis
{
    

    /// <summary>
    /// Анализ 1 по тредовый 
    /// </summary>
    class Analysis1 :  IAnalysis
    {
        //double [][] Signature = new double[5][];
        
        public Analysis1(DateModel dateModel, string name )
        {
            this.Name = name;
            this.DateModel = dateModel;
            PerAnalysis = 4;
            AnalysisResults = new AnalysisResult(name);
        }

        public string Name { get => name; set => name = value; }
        private string name;
        
        private AnalysisResult AnalysisResults;
        private DateModel DateModel;
        private readonly int PerAnalysis;
        private ParamDataService _ParamDS = new ParamDataService();
        private SupportingClass  supporting = SupportingClass.GetInstance();
        private string Result = "";
        private List<double> GetListIndexValue(string nameIndex, string nameVal, int n)
        {
            List<double> Result = new List<double>();            
            for (int i = DateModel.Points.Count-1; i >= DateModel.Points.Count() - n; i--)
            {
                Result.Add(DateModel.Points[i].IndexPoint.Find(i1 => i1.Name == nameIndex).Value.Find(j => j.Name == nameVal).Value);                
            }
            Result.Reverse();            
            return Result;
        }
        private double GetIndexValue (string nameIndex, string nameVal)
        {
            return DateModel.Points.Last().IndexPoint.Find(i1 => i1.Name == nameIndex).Value.Find(j => j.Name == nameVal).Value;
        }
        private string GetDirection ()
        {
            bool FlagUp = false; bool FlagDown = false;
            List<double> EMAPoint8 = GetListIndexValue("EMA8", "EMA", PerAnalysis);
            List<double> EMAPoint16 = GetListIndexValue("EMA16", "EMA", PerAnalysis);
            for (int i=0;EMAPoint8.Count() > i; i++)
            {
                if (EMAPoint8[i] > EMAPoint16[i])
                {
                    FlagUp = true;
                }
                else
                {
                    FlagDown = true;
                }
            }
            if ((!FlagUp) && (!FlagDown))
            {
                return "";
            }
            else if ((FlagUp) && (!FlagDown))
            {
                return "Up";
            }
            else if ((!FlagUp) && (FlagDown))
            {
                return "Down";
            }
            else
            {
                return "";
            }
        }
        private string GetDirectionMACD()
        {
            bool FlagUp = false; bool FlagDown = false;
            List<double> EMAPoint = GetListIndexValue("MACD", "Bar_Graph", PerAnalysis); 
            List<double> tmp = new List<double>();

            for (int i = 0; EMAPoint.Count() > i; i++)
            {
                if (EMAPoint[i] >= 0)
                {
                    FlagUp = true;
                }
                else
                {
                    FlagDown = true;
                }
            }
            if ((!FlagUp) && (!FlagDown))
            {
                return "";
            }
            else if ((FlagUp) && (!FlagDown))
            {
                return "Up";
            }
            else if ((!FlagUp) && (FlagDown))
            {
                return "Down";
            }
            else
            {
                return "";
            }
        }
        private void AnalysisEMA (string name)
        {          
            List<double> EMAPoint = GetListIndexValue(name, "EMA", PerAnalysis);
            for (int i = 0; i < EMAPoint.Count;i++)
            {
                EMAPoint[i] = Math.Round(EMAPoint[i], 2);
            }
            
            ResultArr ResultArr = new ResultArr() { Name = name + "Val", ValStr = String.Join(";", EMAPoint) };
            AnalysisResults.ResultArr.Add(ResultArr);
            
            List<double> tmp = supporting.GetNormData(EMAPoint);
            ResultArr = new ResultArr() { Name = name, ValStr = String.Join(";", tmp) };
            AnalysisResults.ResultArr.Add(ResultArr);
        }
        private void AnalysisMACD(string name)
        {
            List<double> EMAPoint = GetListIndexValue(name, "Bar_Graph", PerAnalysis);
            List<double> tmp = new List<double>();
            foreach (double item in EMAPoint)
            {
                if (item > 0) { tmp.Add(1); }
                else if (item < 0) { tmp.Add(-1);}
                else { tmp.Add(0); }
            }
            ResultArr ResultArr = new ResultArr() { Name = name+"Val", ValStr = String.Join(";", tmp) };
            AnalysisResults.ResultArr.Add(ResultArr);

            tmp = supporting.GetNormData(EMAPoint);
            ResultArr = new ResultArr() { Name = name, ValStr = String.Join(";", tmp) };
            AnalysisResults.ResultArr.Add(ResultArr);
        }
        private void GetIndex(string name)
        {
            ResultArr ResultArr;
            switch (name)
            {
                case "ADX":
                    ResultArr = new ResultArr() { Name = name + "Val", ValStr = GetIndexValue("ADX", "ADX").ToString() };
                    AnalysisResults.ResultArr.Add(ResultArr);
                    break;
                case "CCI":
                    ResultArr = new ResultArr() { Name = name + "Val", ValStr = GetIndexValue("CCI", "CCI").ToString() };
                    AnalysisResults.ResultArr.Add(ResultArr);
                    break;
            }
        }
        private bool GetAnalysisMACD(string Direction)
        {
            switch (Direction)
            {
                case "Up":
                    bool flag = false;
                    List<double> EMAPoint = GetListIndexValue("MACD", "Bar_Graph", PerAnalysis);
                    List<double> tmp = new List<double>();
                    for (int i=0;EMAPoint.Count() > i;i++)
                    {
                        if (EMAPoint[i] < 0.1)
                        {
                            flag = true;
                        }
                    }
                    if (flag) return false;
                    return true;
                    //tmp = supporting.GetNormData(EMAPoint);
                    //if ((tmp.First() == 0) && (tmp.Last() == 1))
                    //{
                    //    return true;
                    //}                    
                    //break;
                case "Down":
                    break;
                default:
                    return false;                    
            }
            

            return false;
        }
        private bool GetAnalysisPC(string Direction)
        {
            switch (Direction)
            {
                case "Up":                    
                    List<double> PC50 = GetListIndexValue("PC50", "Hiht", 50);
                    List<double> PC20 = GetListIndexValue("PC20", "Hiht", 50);
                    List<double> _PC50 = GetListIndexValue("PC50", "Low", 50);
                    List<double> _PC20 = GetListIndexValue("PC20", "Low", 50);
                    int kol1 = 0; int kol2 = 0;
                    for (int i = 0; PC50.Count() > i; i++)
                    {
                        if ((PC50[i] - PC20[i]) == 0)
                        {
                            kol1++;
                        }
                        if ((_PC20[i] - _PC50[i]) > 0)
                        {
                            kol2++;
                        }                       
                    }
                    if (/*(kol1 > 25) && */(kol2 > 25))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "Down":
                    break;
                default:
                    return false;
            }
            return false;
        }
        #region Публичные методы 
        public void GetAnalysis()
        {
            AnalysisResults = new AnalysisResult(Name);
            List<LavelModel> lavels = _ParamDS.GetLavelModels("Index");
            if (DateModel.Points.Count() < PerAnalysis)
            {
                return;
            }
            #region  Расчитать данные по индексам
            foreach (var items in lavels)
            {
                //if (_ParamDS.GetParamValue(items.Id,"Type") == "EMA")
                //{
                string type = _ParamDS.GetParamValue(items.Id, "Type");
                switch (type)
                {
                    case "EMA":
                        AnalysisEMA(_ParamDS.GetParamValue(items.Id, "Name"));
                        break;
                    case "MACD":
                        AnalysisMACD("MACD");
                        break;
                    case "CCI":
                        GetIndex("CCI");
                        break;
                    case "ADX":
                        GetIndex("ADX");
                        break;

                }
                //}                
            }
            #endregion
            #region Проанализировать данные 
            List<string> resultArr = new List<string>();
            //string Direction = GetDirection();
            string Direction = GetDirectionMACD();
            switch (Direction)
            {
                case "Up":
                    if (GetAnalysisPC("Up"))
                    {
                        Result = Direction;
                        AnalysisResults.Result = "Up";
                    }
                    break;
                case "Down":
                    break;
                default:
                    Result = "";
                    break;
            }
            #endregion
            #region Записать данные в коллекцию 
            int ind = DateModel.Points.Last().AnalysisResults.FindIndex(i => i.Name == Name);
            if (ind == -1)
            {
                DateModel.Points.Last().AnalysisResults.Add(AnalysisResults);
            }
            else
            {
                DateModel.Points.Last().AnalysisResults[ind] = AnalysisResults;
            }
            #endregion                  
        }

        public string GetResult()
        {
            return Result;
        }

        public List<ResultArr> ResultArr()
        {
            return AnalysisResults.ResultArr;            
        }
        #endregion
    }
    /// <summary>
    /// Анализ 2 Принимаем решение об открытии позиции 
    /// </summary>
    class Analysis2 
    {
        private readonly int PerAnalysis;
        private DateModel DateModel;
        private ParamDataService _ParamDS = new ParamDataService();
        private AnalysisResult AnalysisResults;
        private string A1Result;
        private double GetIndexValue(string nameIndex, string nameVal)
        {
            return DateModel.Points.Last().IndexPoint.Find(i1 => i1.Name == nameIndex).Value.Find(j => j.Name == nameVal).Value;
        }
        private List<double> GetListIndexValue(string nameIndex, string nameVal, int n)
        {
            List<double> Result = new List<double>();
            for (int i = DateModel.Points.Count - 1; i >= DateModel.Points.Count() - n; i--)
            {
                Result.Add(DateModel.Points[i].IndexPoint.Find(i1 => i1.Name == nameIndex).Value.Find(j => j.Name == nameVal).Value);
            }
            Result.Reverse();
            return Result;
        }
        private string GetDirectionMACD()
        {
            bool FlagUp = false; bool FlagDown = false;
            List<double> EMAPoint = GetListIndexValue("MACD", "Bar_Graph", PerAnalysis);
            List<double> tmp = new List<double>();

            for (int i = 0; EMAPoint.Count() > i; i++)
            {
                if (EMAPoint[i] >= 0)
                {
                    FlagUp = true;
                }
                else
                {
                    FlagDown = true;
                }
            }
            if ((!FlagUp) && (!FlagDown))
            {
                return "";
            }
            else if ((FlagUp) && (!FlagDown))
            {
                return "Up";
            }
            else if ((!FlagUp) && (FlagDown))
            {
                return "Down";
            }
            else
            {
                return "";
            }
        }
        public bool HaveOrder
        {
            get
            {
                if (OrderModels == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }            
        }
        public OrderModel OrderModels { get; set; }
        public string Name { get => name; set => name = value; }
        private string name;
        public string Tiker { get; set; }
        public Analysis2(DateModel dateModel, string name, string tiker, string A1Result)
        {
            this.Tiker = tiker;
            this.Name = name;
            this.DateModel = dateModel;
            this.A1Result = A1Result;
            PerAnalysis = 4;
            AnalysisResults = new AnalysisResult(name);
        }
        private double GetMax (int n)
        {
            int N = DateModel.Points.Count();
            if (N >= n) { N = DateModel.Points.Count() - n; }
            else { N = 0; }
            double Val = DateModel.Points[N].High;

            for (int i = N; i < DateModel.Points.Count()-1; i++ )
            {
                if (Val < DateModel.Points[i].High)
                {
                    Val = DateModel.Points[i].High;
                }
            }
            return Val;
        }
        private double GetStopOrder()
        {
            return 0;
        }
        private void GetOrder()
        {

            OrderModels = new OrderModel() {
                Tiker = Tiker,
                Type = A1Result,
                Vol = 1,
                Price = GetMax(20) + (GetIndexValue("ATR", "ATR")/3), //DateModel.Points.Last().IndexPoint.Find(i => i.Name == "EMA8").Value[0].Value, //GetMax(20)
                BeginDate = DateModel.Points.Last().Date,                
                IsActive = true
            };

        }

        public void GetAnalysis()
        {
            //double CCIVal = GetIndexValue("CCI", "CCI");
            if (DateModel.Points.Last().Date.Hour != 11) { return;  }
            switch (A1Result)
            {
                case "Up":
                    //string str = GetDirectionMACD();
                    //if (str == "Up")
                    //{
                        GetOrder();
                    //}                    
                    break;
                case "Down":
                    break;
            }


        }

        public string GetResult()
        {
            return A1Result;            
        }
    }
    class Analysis3
    {
        private string A1Result;
        public string Name { get; set; }        
        public string Tiker { get; set; }
        public double StopOrder { get; set; }
        private DateModel DateModel;
        public Analysis3(DateModel dateModel, string name,string tiker, string A1Result )
        {
            this.Tiker = tiker;
            this.Name = name;
            this.A1Result = A1Result;
            this.StopOrder = 0;
            this.DateModel = dateModel;
        }

        private double GetChandlerExitLong()
        {
            IndexModel indexModel = new IndexModel();
            double tmp = DateModel.Points.Last().IndexPoint.Find(i => i.Name == "ATR").Value.Find(i1 => i1.Name == "ATR").Value;
            double stop = (indexModel.GetMax(DateModel.Points, 22) - (tmp * 3));
            if (stop > 0)
            {
                return stop;
            }

            return 0;
        }


        private void GetStopOrder ()
        {
            StopOrder = GetChandlerExitLong();
        }

        public void GetAnalysis()
        {
            switch (A1Result)
            {
                case "Up":
                    GetStopOrder();
                    break;
                case "Down":
                    break;
                case "":
                    // Закрываем ели нет Результата первого анализа 
                    StopOrder = DateModel.Points.Last().Close;
                    break;
            }
        }
    }
}
