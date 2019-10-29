using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model.Analysis
{
    /// <summary>
    /// Класс где реализованны вспомогательные функции 
    /// </summary>
    
    public class SupportingClass
    {
        // TODO: Реализовать нормализацию данных

        private static SupportingClass instance;
        private SupportingClass()
        {

        }
        public List<double> GetNormData (List<double> InputArr)
        {
            List<double> Result = new List<double>();
            double Max = InputArr.Max();
            double Min = InputArr.Min();
            double Denominator = (Max - Min);
            foreach (var item in InputArr)
            {
                double tmp = (item - Min) / Denominator;
                Result.Add(tmp);
            }
            return Result;
        }
        public static SupportingClass GetInstance()
        {
            if (instance == null)
                instance = new SupportingClass();
            return instance;
        }
    }
}
