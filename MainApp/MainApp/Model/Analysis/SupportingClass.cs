using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainApp.Model.Analysis
{
    /// <summary>
    /// Класс где реализованны вспомогательные функции 
    /// </summary>
    
    public class SupportingClass
    {       
        #region Реализация патерна Singletone
        private static SupportingClass instance;
        private SupportingClass()
        {

        }
        public static SupportingClass GetInstance()
        {
            if (instance == null)
                instance = new SupportingClass();
            return instance;
        }
        #endregion

        #region Нормализация данных от 0 до 1
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
        #endregion

        #region Результат по методу сигнатур 
        private List<List<double>> Signature = new List<List<double>>();
        private void LoadSign()
        {
            Signature.Add(new List<double>() { 0, 0.2, 0.6, 1 });
            Signature.Add(new List<double>() { 0, 0.3, 0.6, 1 });
            Signature.Add(new List<double>() { 0, 0.3, 0.7, 1 });
            Signature.Add(new List<double>() { 0, 0.4, 0.7, 1 });
            Signature.Add(new List<double>() { 0, 0.4, 0.8, 1 });
        }
        public string ChekSignature(string str)
        {
            LoadSign();
            List<double> arr = new List<double>();
            arr = str.Split(';').Select(n=> Math.Round(Convert.ToDouble(n),1)).ToList();
            //arr = new List<double>() { 0, 0.3, 0.6, 1 };
            int result = Signature.FindIndex(i => i.SequenceEqual(arr));

            if (result != -1)
            {
                return "Up";
            }

            return "";
        }
        #endregion
    }
}
