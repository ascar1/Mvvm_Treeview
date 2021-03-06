﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Windows;

namespace MvvmLight1.Model
{
    public enum DataType { String, Int, Bool };
    public class DataService : IDataService
    {
        private string NameFile = "C:\\newparam_.xml";
        private List<LavelModel> LavelItem = new List<LavelModel>();
        private List<ParamModel> ParamItem = new List<ParamModel>();
        private List<string> TypeDataList = new List<string>();
        public DataService()
        {
            LoadData();
        }
        private void Clear()
        {
            LavelItem.Clear();
            ParamItem.Clear();
        }
        private DataType getType(string type)
        {
            switch (type)
            {
                case "String": return DataType.String;
                case "Int": return DataType.Int;
                case "Bool": return DataType.Bool;
                default: return DataType.String;
            }
        }
        private int getNewIndexParam()
        {
            int i = ParamItem.Max(I=> I.id);            
            return ++i;
        }
        public int getNewIndexLavel()
        {
            int i = LavelItem.Max(I => I.id);
            return ++i;
        }
        private void LoadData()
        {
            Clear();
            XDocument xdoc = XDocument.Load(NameFile);
            foreach (var tmp in xdoc.Element("ProgramParam").Elements("level"))
            {                
                LavelItem.Add(new LavelModel()
                {
                    id = chekInt(tmp.Attribute("Id")),
                    paremtId = chekInt(tmp.Attribute("paremtId")),
                    name = tmp.Attribute("Name").Value,
                    comment = tmp.Attribute("Comment").Value,
                });
                foreach (var tmp1 in tmp.Elements("level"))
                {
                    LavelItem.Add(new LavelModel()
                    {
                        id = chekInt(tmp1.Attribute("Id")),
                        paremtId = chekInt(tmp1.Attribute("paremtId")),
                        name = tmp1.Attribute("Name").Value,
                        comment = tmp1.Attribute("Comment").Value,
                    });

                    foreach (var tmp2 in tmp1.Elements("Param"))
                    {
                        ParamItem.Add(new ParamModel()
                        {
                            id = chekInt(tmp2.Attribute("Id")),
                            ParamID = chekInt(tmp2.Attribute("ParamID")),
                            name = tmp2.Attribute("Name").Value,
                            type = getType(tmp2.Attribute("Type").Value),
                            val = tmp2.Attribute("Val").Value,
                            comment = tmp2.Attribute("Comment").Value,                            
                        });
                    }
                }
            }
        }
        private int chekInt(XAttribute str)
        {
            if (str == null)
            {
                return -1;
            }

            int val = Convert.ToInt16(str.Value);
            return val;
        }
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service
            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }
        #region функции для работы с уровнями
        public void GetDataLevel(Action<List<LavelModel>, Exception> callback)
        {
            LoadData();
            callback(LavelItem, null);
        }
        public void SaveLavel (LavelModel lavel)
        {
            // Обновить уровень
            XDocument xDoc = XDocument.Load(NameFile);
            // записать                 
            foreach (XElement tmp in xDoc.Element("ProgramParam").Elements("level"))
            {
                foreach (XElement tmp1 in tmp.Elements("level"))
                {                    
                    if (lavel.id != 0)
                    {
                        if (tmp1.Attribute("Id").Value.ToString() == lavel.id.ToString())
                        {                            
                            tmp1.Attribute("Name").Value = lavel.name;
                            tmp1.Attribute("Comment").Value = lavel.comment;                            
                            break;
                        }
                    }
                }
            }
            xDoc.Save(NameFile);
        }
        public void InsertLavel (LavelModel lavel)
        {
            // Вставить новый уровень
            XDocument xDoc = XDocument.Load(NameFile);
            var tmp = xDoc.Element("ProgramParam").Elements("level").First(i => i.Attribute("Id").Value.ToString() == lavel.paremtId.ToString());
            tmp.Add(new XElement("level",
                        new XAttribute("Id", lavel.id),
                        new XAttribute("paremtId", lavel.paremtId),
                        new XAttribute("Name", lavel.name),
                        new XAttribute("Comment", "")
                ));
            xDoc.Save(NameFile);
        }
        public void AddLavel(int id)
        {            
            XDocument xDoc = XDocument.Load(NameFile);
            var tmp = xDoc.Element("ProgramParam").Elements("level").First(i => i.Attribute("Id").Value.ToString() == id.ToString());
            tmp.Add(new XElement("level", 
                        new XAttribute("Id", getNewIndexLavel()),
                        new XAttribute("paremtId", id),
                        new XAttribute("Name", "New Lavel"),
                        new XAttribute("Comment","")
                ));
            xDoc.Save(NameFile);
        }
        public void DeleteLavel(int id)
        {
            // Удалить уровень
            XDocument xDoc = XDocument.Load(NameFile);
            //var tmp = xDoc.Element("ProgramParam").Elements("level").First(i => i.Attribute("Id").Value.ToString() == id.ToString());
                            
            foreach (XElement tmp in xDoc.Element("ProgramParam").Elements("level"))
            {
                foreach (XElement tmp1 in tmp.Elements("level"))
                {
                    if (id != 0)
                    {
                        if (tmp1.Attribute("Id").Value.ToString() == id.ToString())
                        {                           
                            tmp1.Remove();
                            break;
                        }
                    }
                }
            }
            xDoc.Save(NameFile);
            //tmp.

        }
        #endregion
        #region Функции для работы с параметрами 
        public void GetParam (Action<List<ParamModel>, Exception> callback)
        {
            LoadData();
            callback(ParamItem, null);
        }
        private void writeParam(XElement tmp, ParamModel param)
        {
            tmp.Add(new XElement("Param",
                      new XAttribute("Id", getNewIndexParam()),
                      new XAttribute("ParamID", param.ParamID),
                      new XAttribute("Name", param.name),
                      new XAttribute("Type", param.type),
                      new XAttribute("Val", param.val),
                      new XAttribute("Comment", param.comment)
                      ));           
        }
        private void updateParam(XElement tmp, ParamModel param)
        {
            tmp.Attribute("Name").Value = param.name;
            tmp.Attribute("Type").Value = param.type.ToString();
            tmp.Attribute("Val").Value = param.val;
            tmp.Attribute("Comment").Value = param.comment;
        }
        public void SaveParam(ParamModel param)
        {
            XDocument xDoc = XDocument.Load(NameFile);
            bool flag = false;
            // записать                 
            foreach (XElement tmp in xDoc.Element("ProgramParam").Elements("level"))
            {
                foreach (XElement tmp1 in tmp.Elements("level"))
                {
                    if ((tmp1.Attribute("Id").Value.ToString() == param.ParamID.ToString()) && (param.id == 0))
                    {
                        writeParam(tmp1, param);
                        break;
                    }
                    foreach (XElement tmp2 in tmp1.Elements("Param"))
                    {
                        if (param.id != 0)
                        {
                            if (tmp2.Attribute("Id").Value.ToString() == param.id.ToString())
                            {
                                updateParam(tmp2, param);
                                break;
                            }
                        }
                    }
                }
            }
            xDoc.Save(NameFile);
            LoadData();
        }
        public void DeleteParam(int id)
        {
            XDocument xDoc = XDocument.Load(NameFile);            
            // записать                 
            foreach (XElement tmp in xDoc.Element("ProgramParam").Elements("level"))
            {
                foreach (XElement tmp1 in tmp.Elements("level"))
                {
                    foreach (XElement tmp2 in tmp1.Elements("Param"))
                    {
                        if (tmp2.Attribute("Id").Value.ToString() == id.ToString())
                        {
                            tmp2.Remove();
                            break;
                        }
                    }
                }
            }
            xDoc.Save(NameFile);
            LoadData();
        }
        #endregion

    }
}