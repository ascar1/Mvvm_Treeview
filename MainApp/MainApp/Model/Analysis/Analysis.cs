using System;
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
            List<double> tmp = supporting.GetNormData(EMAPoint);
            ResultArr ResultArr = new ResultArr() { Name = name, ValStr = String.Join(";", tmp) };
            AnalysisResults.ResultArr.Add(ResultArr);
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
                    }
                //}                
            }
            #endregion
            #region Проанализировать данные 
            List<string> resultArr = new List<string>();
            //string result = "";
            foreach(var tmp in AnalysisResults.ResultArr)
            {
                resultArr.Add(supporting.ChekSignature(tmp.ValStr,tmp.Name));
            }
            int ResultCount = 0; Result = "";
            foreach(string tmp in resultArr)
            {
                switch(tmp)
                {
                    case "Up":
                        //AnalysisResults.Result = tmp;
                        //Result = tmp;
                        if ( resultArr.FindAll(i => i == "Up").Count() == 3)
                        {
                            AnalysisResults.Result = "Up";
                            Result = "Up";
                        }

                        ResultCount++;
                        break;
                    case "Down":
                        AnalysisResults.Result = tmp;
                        Result = tmp;
                        break;
                }
            }
            //if (ResultCount == resultArr.Count)
            //{
            //    AnalysisResults.Result = "Up";
            //    Result = "Up";
            //}
            //else if (ResultCount == 1)
            //{
            //    MessageBox.Show("!");
            //}
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

        private void GetOrder()
        {
            
            OrderModels = new OrderModel() {
                Tiker = Tiker,
                Type = A1Result,
                Vol = 1,
                Price = DateModel.Points.Last().Close,
                BeginDate = DateModel.Points.Last().Date,
                EndDate = DateModel.Points.Last().Date.AddDays(1),
                IsActive = true
            };

        }

        public void GetAnalysis()
        {
            switch (A1Result)
            {
                case "Up":
                    GetOrder();
                    break;
                case "Down":
                    break;
            }


        }

        public string GetResult()
        {
            throw new NotImplementedException();
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

        private void GetStopOrder ()
        {
            if (DateModel.Points.Last().Date.Hour == 18)
            {
                StopOrder = DateModel.Points.Last().Close;
            }
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
            }
        }
    }
}
