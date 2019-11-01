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
                if (_ParamDS.GetParamValue(items.Id,"Type") == "EMA")
                {
                    string type = _ParamDS.GetParamValue(items.Id, "Type");
                    switch (type)
                    {
                        case "EMA":
                            AnalysisEMA(_ParamDS.GetParamValue(items.Id, "Name"));
                        break;
                    }
                }                
            }
            #endregion
            #region Проанализировать данные 
            List<string> resultArr = new List<string>();
            string result = "";
            foreach(var tmp in AnalysisResults.ResultArr)
            {
                resultArr.Add(supporting.ChekSignature(tmp.ValStr));
            }
            foreach(string tmp in resultArr)
            {
                switch(tmp)
                {
                    case "Up":
                        AnalysisResults.Result = tmp;
                        result = tmp;
                        break;
                    case "Down":
                        AnalysisResults.Result = tmp;
                        result = tmp;
                        break;
                }
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

        public string result()
        {
            throw new NotImplementedException();
        }

        public List<ResultArr> ResultArr()
        {
            return AnalysisResults.ResultArr;            
        }
        #endregion
    }

    class Analysis2 : IAnalysis
    {
        public Analysis2(DateModel dateModel, string name)
        {
        }

        public void GetAnalysis()
        {
            throw new NotImplementedException();
        }

        public string result()
        {
            throw new NotImplementedException();
        }
    }
}
