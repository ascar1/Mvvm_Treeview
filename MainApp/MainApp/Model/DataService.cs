﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Windows;
using System.IO;
using System.Text;

namespace MainApp.Model
{
    public enum DataType { String, Int, Bool };
    public class DataService : IDataService
    {
        private List<MasterPointModel> CPModel_;
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }

        public void LoadData(Action<List<MasterPointModel>, Exception> callback)
        {
            string dirName = @"C:\Tiker\";
            CPModel_ = new List<MasterPointModel>();
            /* string FileName = "BANE_140303_150331.txt";
             FileName = @"C:\Tiker\" + FileName;*/
            if (Directory.Exists(dirName))
            {
                string[] files = Directory.GetFiles(dirName, "*.txt");
                foreach (string s in files)
                {
                    LoadFile(s, Path.GetFileName(s).Substring(0, 4), "60");
                }
            }
            callback(CPModel_, null);
        }

        public List<FileArrModel> GetFileArrs()
        {
            List<FileArrModel> f = new List<FileArrModel>();
            foreach (var i in CPModel_)
            {
                f.Add(new FileArrModel(i, true));
            }
            return f;
        }

        private void LoadFile(string FName, string Tiker, string Scale)
        {
            string[] _data;
            MasterPointModel CPModel = new MasterPointModel
            {
                Tiker = Tiker,
                FName = Path.GetFileName(FName),
                Data = new List<DateModel>()
            };
            CPModel.Data.Add(new DateModel());
            CPModel.Data[0].Scale = Scale;
            CPModel.Data[0].Points = new List<PointModel>();


            if (!File.Exists(FName))
            {
                throw new FileNotFoundException();
            }

            StreamReader reader = new StreamReader(FName, Encoding.Default);
            while (!reader.EndOfStream)
            {
                _data = reader.ReadLine().Split(';');
                if (Double.TryParse(_data[6]/*.Replace(".", ",")*/, out double i) == true)
                {
                    CPModel.Data[0].Points.Add(new PointModel()
                    {
                        Open = Convert.ToDouble(_data[4]/*.Replace(".", ",")*/),
                        High = Convert.ToDouble(_data[5]/*.Replace(".", ",")*/),
                        Low = Convert.ToDouble(_data[6]/*.Replace(".", ",")*/),
                        Close = Convert.ToDouble(_data[7]/*.Replace(".", ",")*/),
                        Vol = Convert.ToDouble(_data[8]),
                        Date = Convert.ToDateTime(_data[2] + " " + _data[3])
                    });
                }
                else
                {
                    //MessageBox.Show("!");
                }
            }
            CPModel.SDate = CPModel.Data[0].Points.First().Date;
            CPModel.EDate = CPModel.Data[0].Points.Last().Date;

            CPModel_.Add(CPModel);
        }

        private DataType GetType(string type)
        {
            switch (type)
            {
                case "String": return DataType.String;
                case "Int": return DataType.Int;
                case "Bool": return DataType.Bool;
                default: return DataType.String;
            }
        }

        public List<PointModel> GetMasterPoint(string scale, string tiker)
        {
            MasterPointModel tmp = CPModel_.Find(i => i.Tiker == tiker);
            return tmp.Data[0].Points;
        }

        public List<MasterPointModel> GetMasterPoints()
        {
            return CPModel_;
        }
        /*
        public ParamDataService ParamDS ()
        {
            return ParamDataService.GetInstance();
        }
        */
    }
}