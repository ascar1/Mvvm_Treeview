using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MvvmLight1.Model;

namespace MvvmLight1.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem("Welcome to MVVM Light [design]");
            callback(item, null);
        }

        public void GetDataLevel(Action<List<LavelModel>, Exception> callback)
        {
            var item = new List<LavelModel>();
            item.Add(new LavelModel() { id = 0, name = "name 1 ", comment = "# 1" });
            item.Add(new LavelModel() { id = 1, name = "name 2 ", comment = "# 1" });
            item.Add(new LavelModel() { id = 2, name = "name 3 ", comment = "# 1" });
            item.Add(new LavelModel() { id = 3, paremtId = 0, name = "name 4", comment = "# 1" });
            item.Add(new LavelModel() { id = 4, paremtId = 1, name = "name 5", comment = "# 1" });
            item.Add(new LavelModel() { id = 5, paremtId = 2, name = "name 6", comment = "# 1" });
            callback(item, null);
        }
        public void GetParam(Action<List<ParamModel>, Exception> callback)
        {
            var item = new List<ParamModel>();
            item.Add(new ParamModel() { id = 0, ParamID = 3, name = "param 1", type = DataType.String, val = "val 1", comment = "#1" });
            item.Add(new ParamModel() { id = 0, ParamID = 3, name = "param 2", type = DataType.String, val = "val 2", comment = "#2" });
            item.Add(new ParamModel() { id = 0, ParamID = 3, name = "param 3", type = DataType.String, val = "val 3", comment = "#3" });
            item.Add(new ParamModel() { id = 0, ParamID = 4, name = "param 4", type = DataType.String, val = "val 4", comment = "#4" });
            item.Add(new ParamModel() { id = 0, ParamID = 4, name = "param 5", type = DataType.String, val = "val 5", comment = "#5" });
            item.Add(new ParamModel() { id = 0, ParamID = 4, name = "param 6", type = DataType.String, val = "val 6", comment = "#6" });
            item.Add(new ParamModel() { id = 0, ParamID = 6, name = "param 7", type = DataType.String, val = "val 7", comment = "#7" });
            item.Add(new ParamModel() { id = 0, ParamID = 6, name = "param 8", type = DataType.String, val = "val 8", comment = "#8" });
            callback(item, null);
        }

        public void LoadData(Action<List<ParamModel>, Exception> callback, Action<List<LavelModel>, Exception> callback1)
        {
            XDocument xdoc = XDocument.Load("C:\\newparam1.xml");
            var LavelItem = new List<LavelModel>();
            var ParamItem = new List<ParamModel>();
            foreach (var tmp in xdoc.Element("ProgramParam").Elements("level"))
            {

                LavelItem.Add(new LavelModel()
                {
                    id = Convert.ToInt16(tmp.Attribute("id").Value),
                    paremtId = Convert.ToInt16(tmp.Attribute("paremtId").Value),
                    name = tmp.Attribute("name").Value,
                    comment = tmp.Attribute("comment").Value,
                });

                foreach (var tmp2 in tmp.Elements("Param"))
                {
                    ParamItem.Add(new ParamModel()
                    {
                        id = Convert.ToInt16(tmp.Attribute("id").Value),
                        ParamID = Convert.ToInt16(tmp.Attribute("ParamID").Value),
                        name = tmp.Attribute("name").Value,
                       // type = tmp.Attribute("type").Value,
                        val = tmp.Attribute("val").Value,
                        comment = tmp.Attribute("comment").Value
                    });
                }
            }
            callback(ParamItem, null);
            callback1(LavelItem, null);
        }
        public void SaveParam(ParamModel param)
        {
        }
        public void DeleteParam(int id)
        {
            throw new NotImplementedException();
        }

        public void AddLavel(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteLavel(int i)
        {
            throw new NotImplementedException();
        }
 
        public void SaveLavel(LavelModel lavel)
        {
            throw new NotImplementedException();
        }

        public int getNewIndexLavel()
        {
            throw new NotImplementedException();
        }

        public void InsertLavel(LavelModel lavel)
        {
            throw new NotImplementedException();
        }
    }
}