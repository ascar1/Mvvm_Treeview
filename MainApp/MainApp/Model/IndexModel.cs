using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainApp.Model
{
    class IndexModel
    {
        public IndexModel()
        {

        }
        private string GetStringParam(List<ParamModel> @params, string NameParam) => @params.Find(tmp => tmp.Name == NameParam).Val;
        private int GetIntParam (List<ParamModel> @params, string NameParam)
        {
            return Convert.ToInt32(@params.Find(tmp => tmp.Name == NameParam).Val); ;
        }
        private void AddVal (PointModel point, string name, string type, string namePoint, double val)
        {
            int ii = point.IndexPoint.FindIndex(I1 => I1.Name == name);
            if (ii == -1)
            {
                List<IndexPoint> indexPoints = new List<IndexPoint>
                {
                    new IndexPoint
                    {
                        Name = namePoint,
                        Value = val
                    }
                };
                point.IndexPoint.Add(new Index()
                {
                    Name = name,
                    Type = type,
                    Value = indexPoints

                });
            }
            else
            {
                int i = point.IndexPoint[ii].Value.FindIndex(i1 => i1.Name == namePoint);
                if (i == -1)
                {
                    point.IndexPoint[ii].Value.Add(new IndexPoint()
                    {
                        Name = namePoint,
                        Value = val
                    });
                }
                else
                {
                    point.IndexPoint[ii].Value[i].Value = (double)val;
                }
                
            }
        }
        private double GetValIndex(PointModel point, List<ParamModel> @params) => point.IndexPoint.Find(i => i.Name == GetStringParam(@params, "Name")).Value[0].Value;
        private double GetValIndex(PointModel point, string name, string namePoint) => point.IndexPoint.Find(i => i.Name == name).Value.Find(i1 => i1.Name == namePoint).Value;
        private double GetEMA(List<PointModel> points, int N, string name , string namePoint)
        {
            // Расчитать и вернуть значение EMA 
            Decimal EMA0 = 0; Decimal EMA = 0; Decimal k1; Decimal k2;

            k2 = N + 1;
            k1 = (Decimal)2 / (Decimal)k2;

            if (points.Count <= N)
            {
                N = points.Count;
                EMA0 = (Decimal)points[0].Close;
            }
            else
            {
                EMA0 = (Decimal)GetValIndex(points[points.Count - (N + 1)], name, namePoint);
            }


            for (int I = points.Count - N; I < points.Count;I++)
            {
                if (I == 0)
                {
                    EMA0 = (Decimal)points[0].Close;
                }                
                EMA = (Decimal)points[I].Close * (Decimal)k1 + (Decimal)EMA0 * (1 - (Decimal)k1);
                EMA0 = EMA;
            }
            return (double)EMA;
        }
        private double GetEMA (List<PointModel> points, int N, string name, string type, string namePoint)
        {

           Decimal EMA0 = 0; Decimal EMA = 0; Decimal k1; Decimal k2;

            k2 = N + 1;
            k1 = (Decimal)2 / (Decimal)k2;

            if (points.Count <= N)
            {
                N = points.Count;
                EMA0 = (Decimal)points[0].Close;
            }
            else
            {
                EMA0 = (Decimal)GetValIndex(points[points.Count - (N + 1)], name, namePoint);
            }

            for (int I = points.Count - N; I < points.Count; I++)
            {
                decimal tmpVal = Convert.ToDecimal(points[I].IndexPoint.Find(i1 => i1.Name == name).Value.Find(i2 => i2.Name == namePoint).Value);
                EMA = tmpVal * (Decimal)k1 + (Decimal)EMA0 * (1 - (Decimal)k1);
                EMA0 = EMA;                
            }

            return (double)EMA;            
        }
        public void GetEMA(List<PointModel> points, List<ParamModel> @params)
                {
                    // Расчитать и записать EMA по параметрам из файла настроек 
                    int N; string name;
                    N = GetIntParam(@params, "Period");
                    name = GetStringParam(@params, "Name");

                    /*int i = 0;*/ Decimal EMA0 = 0; Decimal EMA; Decimal k1; Decimal k2;

                    k2 = N + 1;
                    k1 = (Decimal)2 / (Decimal)k2;
                    if (points.Count <= N)
                    {
                        N = points.Count;
                        EMA0 = (Decimal)points[0].Close;
                    }
                    else
                    {
                        EMA0 = (Decimal)GetValIndex(points[points.Count - (N + 1)], @params);
                    }
                    for (int i = points.Count-N;i<points.Count;i++)
                    {
                        EMA = (Decimal)points[i].Close * (Decimal)k1 + (Decimal)EMA0 * (1 - (Decimal)k1);
                        AddVal(points[i], name, "EMA", "EMA", (double)EMA);
                        EMA0 = EMA;
                    }
                }
        public void GetMACD(List<PointModel> points, List<ParamModel> @params)
        {
            string name = GetStringParam(@params, "Name");
            int NEMA26 = GetIntParam(@params, "EMAl"); //26
            int NEMA12 = GetIntParam(@params, "EMAs"); //12
            int nEMAa = GetIntParam(@params, "EMAa");

            double EMA12 = GetEMA(points, NEMA12, name, "_EMA12");
            AddVal(points.Last(), "MACD", "MACD", "_EMA12", EMA12);

            double EMA26 = GetEMA(points, NEMA26, name ,"_EMA26");
            AddVal(points.Last(), "MACD", "MACD", "_EMA26", EMA26);

            double Fast = EMA26 - EMA12;
            AddVal(points.Last(), "MACD", "MACD", "_Fast", Fast);

            double Slow = GetEMA(points, nEMAa, "MACD", "MACD", "_Fast");
            AddVal(points.Last(), "MACD", "MACD", "_Slow", Slow);

            double Bar_Graph = Fast - Slow;
            AddVal(points.Last(), "MACD", "MACD", "Bar_Graph", Bar_Graph);

        }
        public void GetForceIndex (List<PointModel> points, List<ParamModel> @params)
        {
            string name = GetStringParam(@params, "Name");
            int n = GetIntParam(@params, "Period");
            double RawFI;
            
            if (points.Count != 1)
            {
                RawFI = points.Last().Vol * (points.Last().Close - points[points.Count - 2].Close);
            }
            else
            {
                RawFI = points.Last().Vol;
            }
            
            AddVal(points.Last(), name, "FI", "_RawFI", RawFI);
            double FI = GetEMA(points, n, name, "FI", "_RawFI");
            AddVal(points.Last(), name, "FI", "FI", FI);
            //
        }
        // TODO: Добавить каналы 
        public void GetEnvelopes (List<PointModel> points, List<ParamModel> @params)
        {

        }
        // TODO: Добавить ATR или  другой показатель волатитильности 
    }
}
