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
                //MessageBox.Show(i.ToString());
                //var tmp = DateModel.Points[i].IndexPoint.Find(i1 => i1.Name == nameIndex);
                Result.Add(DateModel.Points[i].IndexPoint.Find(i1 => i1.Name == nameIndex).Value.Find(j => j.Name == nameVal).Value);
            }
            Result.Reverse();
            return Result;
        }

        private void AnalysisEMA (string name)
        {
            //MessageBox.Show(name);
            //DateModel.Points.Last().AnalysisResults.Add(new AnalysisResult(Name) { Result = "test" });
            
            List<double> EMAPoint = GetListIndexValue(name, "EMA", PerAnalysis);
            List<double> tmp = supporting.GetNormData(EMAPoint);
            ResultArr ResultArr = new ResultArr() { Name = name, ValStr = String.Join(";", tmp) };
            AnalysisResults.ResultArr.Add(ResultArr);
            //MessageBox.Show(String.Join(";",tmp));
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

            int ind = DateModel.Points.Last().AnalysisResults.FindIndex(i => i.Name == Name);
            if (ind == -1)
            {
                DateModel.Points.Last().AnalysisResults.Add(AnalysisResults);
            }
            else
            {
                DateModel.Points.Last().AnalysisResults[ind] = AnalysisResults;
            }

            
            //AnalysisEMA("EMA");            
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
}
