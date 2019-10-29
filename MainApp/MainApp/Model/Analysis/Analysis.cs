using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model.Analysis
{
    

    /// <summary>
    /// Анализ 1 по тредовый 
    /// </summary>
    class Analysis1 : IAnalysis
    {
        public Analysis1(DateModel dateModel, string name )
        {
            this.Name = name;
            this.DateModel = dateModel;
            AnalysisResults = new AnalysisResult(name);
        }
        
        public string Name { get => name; set => name = value; }
        private string name;
        public AnalysisResult AnalysisResults { get => analysisResults; set => analysisResults = value; }
        private AnalysisResult analysisResults;
        private DateModel DateModel;

        private void AnalysisEMA (string name)
        {

        }

        #region Публичные методы 
        public void GetAnalysis()
        {
            AnalysisEMA("EMA");            
        }

        public string result()
        {
            throw new NotImplementedException();
        }

        public List<ResultAnalysis> ResultArr()
        {
            return AnalysisResults.ResultAnalyses;            
        }
        #endregion
    }
}
