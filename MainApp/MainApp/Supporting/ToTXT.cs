using MainApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Supporting
{
    class ToTXT
    {
        //private readonly string FileDir = "C:\\TXT";
        public ToTXT()
        {

        }
        private void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
        public void UpLoadOrder()
        {
            string path = @"C:\TXT\Order.txt";
            IteratorModel iterator = IteratorModel.GetInstance(null, null);
            Type type = Type.GetType("MainApp.Model.OrderModel");
            using (FileStream fstream = new FileStream(path, FileMode.Create))
            {
                string str = "";
                foreach (var tmp in type.GetProperties())
                {
                    if (str != "")
                    {
                        str = str + "^" + tmp.Name.ToString();
                    }
                    else
                    {
                        str = tmp.Name.ToString();
                    }
                    
                }
                AddText(fstream, str + "\r\n");
                
                foreach (var tmp in iterator.orderModels)
                {
                    str = "";
                    foreach (var tmp1 in type.GetProperties())
                    {
                        PropertyInfo fi = typeof(OrderModel).GetProperty(tmp1.Name.ToString());
                        if (str != "")
                        {
                            str = str + "^" + fi.GetValue(tmp).ToString();
                        }
                        else
                        {
                            str = fi.GetValue(tmp).ToString();
                        }
                    }
                    AddText(fstream, str + "\r\n");
                }
                Console.WriteLine("Текст записан в файл");
            }
        }

        public void UpLoadDeal()
        {
            string path = @"C:\TXT\Deal.txt";
            IteratorModel iterator = IteratorModel.GetInstance(null, null);
            Type type = Type.GetType("MainApp.Model.DealModel");
            using (FileStream fstream = new FileStream(path, FileMode.Create))
            {
                string str = "";
                foreach (var tmp in type.GetProperties())
                {
                    if (str != "")
                    {
                        str = str + "^" + tmp.Name.ToString();
                    }
                    else
                    {
                        str = tmp.Name.ToString();
                    }

                }
                AddText(fstream, str + "\r\n");

                foreach (var tmp in iterator.dealModels)
                {
                    str = "";
                    foreach (var tmp1 in type.GetProperties())
                    {
                        PropertyInfo fi = typeof(DealModel).GetProperty(tmp1.Name.ToString());
                        if (str != "")
                        {
                            str = str + "^" + fi.GetValue(tmp).ToString();
                        }
                        else
                        {
                            str = fi.GetValue(tmp).ToString();
                        }
                    }
                    AddText(fstream, str + "\r\n");
                }
                Console.WriteLine("Текст записан в файл");
            }

        }

        public void UpLoadAnalysis()
        {
            string path = @"C:\TXT\Analysis.txt";
            IteratorModel iterator = IteratorModel.GetInstance(null, null);

            Type type = Type.GetType("MainApp.Model.DealModel");

            using (FileStream fstream = new FileStream(path, FileMode.Create))
            {
                string str = "";
                #region Шапка 
                foreach (var tmp in type.GetProperties())
                {
                    if (str != "")
                    {
                        str = str + "^" + tmp.Name.ToString();
                    }
                    else
                    {
                        str = tmp.Name.ToString();
                    }

                }
                AddText(fstream, str + "\r\n");
                #endregion
                #region Тело отчета 
                List<string> tmplist = new List<string>();
                foreach (var tmp in iterator.WorkPoints)
                {
                    
                    DateModel Points = tmp.Data.Find(i => i.Scale == "D");
                    foreach (var tmp1 in Points.Points)
                    {
                        tmplist.Add(tmp.Tiker.ToString());
                        tmplist.Add(tmp1.Date.ToString());
                        foreach(var results in tmp1.AnalysisResults)
                        {
                            tmplist.Add(results.Name);
                            foreach(var result in results.ResultArr)
                            {
                                tmplist.Add(result.Name);
                                List<string> AtrStr = result.ValStr.Split(';').ToList();
                                AtrStr.ForEach(i => { tmplist.Add(i); });
                                
                            }
                            tmplist.Add(results.Result);
                        }
                        int index = Points.Points.FindIndex(i => i.Date == tmp1.Date);
                        if ((index != -1) && (index+1 < Points.Points.Count()))
                        {
                            tmplist.Add(Points.Points[index + 1].Date.ToString());
                            tmplist.Add(Points.Points[index + 1].Open.ToString());
                            tmplist.Add(Points.Points[index + 1].Close.ToString());
                        }

                        str = "";
                        foreach (var tmpstr in tmplist)
                        {
                            if (str != "")
                            {
                                str = str + "^" + tmpstr;
                            }
                            else
                            {
                                str = tmpstr;
                            }                            
                        }

                        tmplist.Clear();
                        if (tmp1.AnalysisResults.Count!=0)
                        {
                            AddText(fstream, str + "\r\n");
                        }
                        
                    }

                    
                }
                #endregion
                Console.WriteLine("Текст записан в файл");
            }
        }

    }
}
