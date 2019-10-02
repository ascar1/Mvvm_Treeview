using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    class IndexModel
    {
        public IndexModel()
        {

        }

        private string getStringParam (List<ParamModel> @params, string NameParam)
        {
            return @params.Find(tmp => tmp.name == NameParam).val; 
        }
        private int getIntParam (List<ParamModel> @params, string NameParam)
        {
            return Convert.ToInt32(@params.Find(tmp => tmp.name == NameParam).val); ;
        }
        private void addVal (PointModel point, string name, string type, string namePoint, double val)
        {
            int ii = point.IndexPoint.FindIndex(I1 => I1.Name == name);
            if (ii == -1)
            {
                List<IndexPoint> indexPoints = new List<IndexPoint>();
                indexPoints.Add(new IndexPoint
                {
                    Name = namePoint,
                    Value = val
                });
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
        public void GetEMA(List<PointModel> points, List<ParamModel> @params)
        {
            int N; string name;
            N = getIntParam(@params, "Period");
            name = getStringParam(@params, "Name");

            int i = 0; Decimal EMA0 = 0; Decimal EMA; Decimal k1; Decimal k2;

            k2 = N + 1;
            k1 = (Decimal)2 / (Decimal)k2;

            foreach (PointModel tmp in points)
            {
                if (i==0)
                {
                    EMA0 = (Decimal)tmp.Close;
                }
                i++;
                EMA = (Decimal)tmp.Close * (Decimal)k1 + (Decimal)EMA0 * (1 - (Decimal)k1);
                addVal(tmp, name, "EMA", "EMA", (double)EMA);
                EMA0 = EMA;
            }
        }
        public double GetEMA(List<PointModel> points, int N)
        {
            int i = 0; Decimal EMA0 = 0; Decimal EMA = 0; Decimal k1; Decimal k2;

            k2 = N + 1;
            k1 = (Decimal)2 / (Decimal)k2;

            foreach (PointModel tmp in points)
            {
                if (i == 0)
                {
                    EMA0 = (Decimal)tmp.Close;
                }
                i++;
                EMA = (Decimal)tmp.Close * (Decimal)k1 + (Decimal)EMA0 * (1 - (Decimal)k1);
                EMA0 = EMA;
            }
            return (double)EMA;
        }
        public double GetEMA (List<PointModel> points, int N, string name, string type, string namePoint)
        {
            int i = 0; Decimal EMA0 = 0; Decimal EMA = 0; Decimal k1; Decimal k2;

            k2 = N + 1;
            k1 = (Decimal)2 / (Decimal)k2;

            foreach (PointModel tmp in points)
            { 
                if (i == 0)
                {
                    EMA0 = Convert.ToDecimal(tmp.IndexPoint.Find(i1 => i1.Name == name).Value.Find(i2 => i2.Name == namePoint).Value);
                }
                i++;
                decimal tmpVal = Convert.ToDecimal(tmp.IndexPoint.Find(i1 => i1.Name == name).Value.Find(i2 => i2.Name == namePoint).Value);
                EMA = tmpVal * (Decimal)k1 + (Decimal)EMA0 * (1 - (Decimal)k1);
                EMA0 = EMA;
            }
            return (double)EMA;            
        }

        public void GetMACD(List<PointModel> points, List<ParamModel> @params)
        {
            string name = getStringParam(@params, "Name");
            int nEMAi = getIntParam(@params, "EMAl"); 
            int nEMAs = getIntParam(@params, "EMAs"); 
            int nEMAa = getIntParam(@params, "EMAa");

            double EMAi = GetEMA(points, nEMAi);
            addVal(points.Last(), "MACD", "MACD", "EMAi", EMAi);
            double EMAs = GetEMA(points, nEMAs);
            addVal(points.Last(), "MACD", "MACD", "EMAs", EMAs);
            double _EMAa = EMAs - EMAi;
            addVal(points.Last(), "MACD", "MACD", "_EMAa", _EMAa);
            double EMAa = GetEMA(points, nEMAa, "MACD", "MACD", "_EMAa");
            addVal(points.Last(), "MACD", "MACD", "EMAa", EMAa);
        }

        public void GetForceIndex (List<PointModel> points, List<ParamModel> @params)
        {
            string name = getStringParam(@params, "Name");
            int n = getIntParam(@params, "Period");

            double RawFI = points.Last().Vol * (points.Last().Close - points[points.Count-1].Close) ;
            addVal(points.Last(), name, "FI", "RawFI", RawFI);
            double FI = GetEMA(points, n, name, "FI", "RawFI");
            addVal(points.Last(), name, "FI", "FI", FI);
            //
        }
    }
}
