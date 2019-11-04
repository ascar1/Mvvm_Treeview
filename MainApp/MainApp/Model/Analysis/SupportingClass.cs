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
            LoadSign();
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
                Result.Add(Math.Round(tmp,1));
            }
            return Result;
        }
        #endregion

        #region Результат по методу сигнатур 
        private List<List<double>> Signature = new List<List<double>>();
        private List<List<double>> SignatureEMAUp = new List<List<double>>();
        private List<List<double>> SignatureMACDUp = new List<List<double>>();
        private List<List<double>> SignatureEMADown = new List<List<double>>();
        private List<List<double>> SignatureMACDDown = new List<List<double>>();

        private void LoadSign()
        {
            Signature.Add(new List<double>() { 0, 0.2, 0.6, 1 });
            Signature.Add(new List<double>() { 0, 0.3, 0.6, 1 });
            Signature.Add(new List<double>() { 0, 0.3, 0.7, 1 });
            Signature.Add(new List<double>() { 0, 0.4, 0.7, 1 });
            Signature.Add(new List<double>() { 0, 0.4, 0.8, 1 });

            SignatureEMAUp.Add(new List<double>() { 0 });
            SignatureEMAUp.Add(new List<double>() { 0.3, 0.4, 0.5 });
            SignatureEMAUp.Add(new List<double>() { 0.4, 0.5, 0.6 });
            SignatureEMAUp.Add(new List<double>() { 1 });

            SignatureMACDUp.Add(new List<double>() { 1 });
            SignatureMACDUp.Add(new List<double>() { 0.5, 0.6, 0.7 });
            SignatureMACDUp.Add(new List<double>() { 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 });
            SignatureMACDUp.Add(new List<double>() { 0 });

            SignatureEMADown.Add(new List<double>() { 1 });
            SignatureEMADown.Add(new List<double>() { 0.9, 0.8, 0.7, 0.6 });
            SignatureEMADown.Add(new List<double>() { 0.5, 0.4, 0.3, 0.2 });
            SignatureEMADown.Add(new List<double>() { 0 });

            SignatureMACDDown.Add(new List<double>() { 1 });
            SignatureMACDDown.Add(new List<double>() { 0.5, 0.6, 0.7 });
            SignatureMACDDown.Add(new List<double>() { 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 });
            SignatureMACDDown.Add(new List<double>() { 0 });


        }
        public string ChekSignature(string str,string Name)
        {
            //LoadSign();
            List<double> arr = new List<double>();
            arr = str.Split(';').Select(n=> Math.Round(Convert.ToDouble(n),1)).ToList();
            //arr = new List<double>() { 0, 0.3, 0.6, 1 };

            bool flag = true; string result = "";
            switch(Name)
            {
                case "EMA8":
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (SignatureEMAUp[i].FindIndex(item => item == arr[i]) == -1)
                        {
                            flag = false;
                        }
                    }
                    if (flag) { result = "Up"; }
                    
                    break;
                case "EMA16":
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (SignatureEMAUp[i].FindIndex(item => item == arr[i]) == -1)
                        {
                            flag = false;
                        }
                    }
                    if (flag) { result = "Up"; }
                    break;
                case "MACD":
                    for (int i = 0; i < arr.Count; i++)
                    {
                        if (SignatureMACDUp[i].FindIndex(item => item == arr[i]) == -1)
                        {
                            flag = false;
                        }
                    }
                    if (flag) { result = "Up"; }
                    break;
            }
            return result;

            //int result = Signature.FindIndex(i => i.SequenceEqual(arr));
            //if (result != -1)
            //{
            //    return "Up";
            //}
        }
        #endregion
    }
}
