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
        private double GetValIndex(PointModel point, string name, string namePoint)
        {
            var tmp = point.IndexPoint.Find(i => i.Name == name).Value;
            var tmp1 = tmp.Find(i1 => i1.Name == namePoint).Value;
            return point.IndexPoint.Find(i => i.Name == name).Value.Find(i1 => i1.Name == namePoint).Value;
        }
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
                EMA0 = (Decimal)GetValIndex(points[0], name, namePoint);
            }
            else
            {
                EMA0 = (Decimal)GetValIndex(points[points.Count - (N + 1)], name, namePoint);
            }
            int II = 1;
            for (int I = points.Count - N; I < points.Count; I++)
            {
                decimal tmpVal = Convert.ToDecimal(GetValIndex(points[I], name, namePoint));
                EMA = tmpVal * (Decimal)k1 + (Decimal)EMA0 * (1 - (Decimal)k1);
                EMA0 = EMA;
                II++;
            }
            return (double)EMA;            
        }
        private double GetSMA (List<PointModel> points, int N, string name, string type, string namePoint)
        {
            Decimal EMA = 0; Decimal k1; Decimal k2;

            k2 = N + 1;
            k1 = (Decimal)2 / (Decimal)k2;

            if (points.Count <= N)
            {
                N = points.Count;
                EMA = (Decimal)GetValIndex(points[0], name, namePoint);
            }
            else
            {
                //EMA = (Decimal)GetValIndex(points[points.Count - (N + 1)], name, namePoint);
                EMA = 0;
            }
            int ii = 0;
            for (int I = points.Count - N; I < points.Count; I++)
            {
                decimal tmpVal = Convert.ToDecimal(GetValIndex(points[I], name, namePoint));
                EMA = EMA + tmpVal;
                ii++;
            }
            EMA = EMA / N;
            return (double)EMA;
        }
        private double GetSumIndex (List<PointModel> points, int N, string name, string type, string namePoint)
        {
            Decimal EMA = 0; 
            if (points.Count <= N)
            {
                N = points.Count;
                EMA = (Decimal)GetValIndex(points[0], name, namePoint);
            }
            else
            {
                EMA = (Decimal)GetValIndex(points[points.Count - (N + 1)], name, namePoint);
            }

            for (int I = points.Count - N; I < points.Count; I++)
            {                
                EMA = EMA + Convert.ToDecimal(GetValIndex(points[I], name, namePoint));
            }            
            return (double)EMA;
        }
        public void GetEMA(List<PointModel> points, List<ParamModel> @params)
        {
            //Расчитать и записать EMA по параметрам из файла настроек
            int N; string name;
            N = GetIntParam(@params, "Period");
            name = GetStringParam(@params, "Name");

            Decimal EMA0 = 0; Decimal EMA; Decimal k1; Decimal k2;

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
            for (int i = points.Count - N; i < points.Count; i++)
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

            //double Fast = EMA12 - EMA26;
            //AddVal(points.Last(), "MACD", "MACD", "_Fast", Fast);
            //double Slow = GetEMA(points, nEMAa, "MACD", "MACD", "_Fast");
            //AddVal(points.Last(), "MACD", "MACD", "_Slow", Slow);

            //double Bar_Graph = Fast - Slow;
            double Bar_Graph = EMA12 - EMA26;
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
        
        public void GetATR (List<PointModel> points, List<ParamModel> @params)
        {
            string name = GetStringParam(@params, "Name");
            int n = GetIntParam(@params, "Period");
            double TR;
            if (points.Count != 1)
            {
                List<double> items = new List<double>();
                items.Add( points.Last().High - points.Last().Low);
                items.Add(points.Last().High - points[points.Count() - 2].Close);
                items.Add(points.Last().Low - points[points.Count() - 2].Close);
                TR = items.Max();
            }
            else
            {
                TR = Math.Abs( points.Last().High - points.Last().Low);
            }
            AddVal(points.Last(), name, "ATR", "TR", TR);
            double ATR = GetEMA(points, n, name, "ATR", "TR");
            AddVal(points.Last(), name, "ATR", "ATR", ATR);
        }
        public double GetMax(List<PointModel> points, int n)
        {
            int N = points.Count();
            if (N >= n) { N = points.Count() - n; }
            else { N = 0; }
            double Val = points[N].High;

            for (int i = N; i < points.Count(); i++)
            {
                if (Val < points[i].High)
                {
                    Val = points[i].High;
                }
            }
            return Val;            
        }
        public double GetMin(List<PointModel> points, int n)
        {
            int N = points.Count();
            if (N >= n) { N = points.Count() - n; }
            else { N = 0; }
            double Val = points[N].High;

            for (int i = N; i < points.Count(); i++)
            {
                if (Val > points[i].High)
                {
                    Val = points[i].High;
                }
            }
            return Val;
        }
        // TODO: Добавить ATX 
        public void GetADX(List<PointModel> points, List<ParamModel> @params)
        {
            string name = GetStringParam(@params, "Name");
            int n = GetIntParam(@params, "Period");
            string type = "ADX";
            if (points.Count < 2)
            {
                AddVal(points.Last(), name, type, "+DM", 0);
                AddVal(points.Last(), name, type, "-DM", 0);
                AddVal(points.Last(), name, type, "TR", 0);
                AddVal(points.Last(), name, type, "+DI", 0);
                AddVal(points.Last(), name, type, "-DI", 0);
                AddVal(points.Last(), name, type, "_ADX", 0);
                AddVal(points.Last(), name, type, "ADX", 0);
                return;
            }

            double Plus_M = points.Last().High - points[points.Count - 2].High;
            double Minus_M = points.Last().Low - points[points.Count - 2].Low;

            double Plus_DM = 0;            
            if ((Plus_M > Minus_M) && (Plus_M > 0))
            {
                Plus_DM = Plus_M;
            }
            else if ((Plus_M < Minus_M) || (Plus_M < 0))
            {
                Plus_DM = 0;
            }
            double Minus_DM = 0;
            if (( Minus_M < Plus_M) || (Minus_M < 0))
            {
                Minus_DM = 0;
            }
            else if ((Minus_M > Plus_M ) && (Minus_M > 0))
            {
                Minus_DM = Minus_M;                
            }
            double TR = Math.Max(points.Last().High, points[points.Count - 2].Close) - Math.Min(points.Last().Low, points[points.Count - 2].Close);
            if (TR == 0) TR = 1;
            AddVal(points.Last(), name, type, "+DM", Plus_DM);
            AddVal(points.Last(), name, type, "-DM", Minus_DM);
            AddVal(points.Last(), name, type, "TR", TR);

            AddVal(points.Last(), name, type, "+DI", Plus_DM/TR);
            AddVal(points.Last(), name, type, "-DI", Minus_DM/TR);
            double PlusDI = GetSMA(points, n, name, type, "+DI");
            double MinusDI = GetSMA(points, n, name, type, "-DI");
            
            double ADX = Convert.ToDouble(Math.Abs((PlusDI - MinusDI)) / (PlusDI + MinusDI));
            //if ((PlusDI - MinusDI) == 0) { ATX = 0; }
            if (Double.IsNaN(ADX)) { ADX = 0; }

            AddVal(points.Last(), name, type, "_ADX", ADX);
            AddVal(points.Last(), name, type, "ADX", GetSMA(points, n, name, type, "_ADX")*100);

        }
        // TODO: Добавить CCI 
        public void GetCCI(List<PointModel> points, List<ParamModel> @params)
        {
            string name = GetStringParam(@params, "Name");
            int n = GetIntParam(@params, "Period");

            double TP = (points.Last().High + points.Last().Low + points.Last().Close) / 3;
            AddVal(points.Last(), name, "CCI", "TP", TP);

            double TPSMA = GetSMA(points, n, name, "CCI", "TP");
            AddVal(points.Last(), name, "CCI", "TPSMA", TPSMA);
                        
            Decimal Sum = 0;
            int N = n;
            if (points.Count <= N)
            {
                N = points.Count;
                Sum = (Decimal)Math.Abs(TPSMA - GetValIndex(points[0], name, "TP"));
            }
            else
            {
                //Sum = (Decimal)Math.Abs(TPSMA - GetValIndex(points[points.Count - (N + 1)], name, "TP"));
            }
            int ii = 0;
            for (int i = points.Count - N; i < points.Count; i++)
            {
                Sum = Sum + (Decimal)Math.Abs(TPSMA - GetValIndex(points[i], name, "TP")) ;
                ii++;
            }
            Sum = Sum / N;
            AddVal(points.Last(), name, "CCI", "MD", (double)Sum);

            double CCI = (TP - TPSMA)/(0.015*(double)Sum);
            AddVal(points.Last(), name, "CCI", "CCI", CCI);
        }
    }
}
