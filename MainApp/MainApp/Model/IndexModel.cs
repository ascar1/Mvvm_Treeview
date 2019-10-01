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
        private void addVal (PointModel point, string name, string type, double val)
        {
            int ii = point.IndexPoint.FindIndex(I1 => I1.Name == name);
            if (ii == -1)
            {
                List<IndexPoint> indexPoints = new List<IndexPoint>();
                indexPoints.Add(new IndexPoint
                {
                    Name = name,
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
                point.IndexPoint[ii].Value[0].Value = (double)val;
            }
        }

        public void GetEMA(List<PointModel> points, List<ParamModel> @params)
        {
            int N; string name;
            N = getIntParam(@params, "Period");
            name = getStringParam(@params, "name");

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
                addVal(tmp, name, "EMA", (double)EMA);
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
        public double GetEMA (List<PointModel> points, int N, string name, string type)
        {
            int i = 0; Decimal EMA0 = 0; Decimal EMA = 0; Decimal k1; Decimal k2;

            k2 = N + 1;
            k1 = (Decimal)2 / (Decimal)k2;

            foreach (PointModel tmp in points)
            { 
                EMA0 = Convert.ToDecimal(tmp.IndexPoint.Find(i1 => i1.Name == name).Value.Find(i2 => i2.Name == type).Value);
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

        public void GetMACD(List<PointModel> points, List<ParamModel> @params)
        {
            string name = getStringParam(@params, "name");
            int nEMAi = getIntParam(@params, "EMAl"); 
            int nEMAs = getIntParam(@params, "EMAs"); 
            int nEMAa = getIntParam(@params, "EMAa");

            double EMAi = GetEMA(points, nEMAi);            
            double EMAs = GetEMA(points, nEMAs);

            foreach (PointModel tmp in points)
            {

            }
            //double 

        }
    }
}
