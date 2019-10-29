using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using static MainApp.Model.DataService;

namespace MainApp.Model
{
    // TODO: Изменить использование данного класса на Сиглтон Нужно что бы был только один энкземпляр класса 
    class ParamDataService : IParamDataService
    {
        private readonly string NameFile = "C:\\newparam_.xml";
        private List<LavelModel> LavelItem = new List<LavelModel>();
        private List<ParamModel> ParamItem = new List<ParamModel>();
        private List<string> TypeDataList = new List<string>();
        public ParamDataService()
        {
            LoadData();
        }
        private void Clear()
        {
            LavelItem.Clear();
            ParamItem.Clear();
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
        private int GetNewIndexParam()
        {
            int i = ParamItem.Max(I => I.Id);
            return ++i;
        }
        public int NewIndexLavel
        {
            get
            {
                int i = LavelItem.Max(I => I.Id);
                return ++i;
            }
        }
        private void LoadData()
        {
            Clear();
            XDocument xdoc = XDocument.Load(NameFile);
            foreach (var tmp in xdoc.Element("ProgramParam").Elements("level"))
            {
                LavelItem.Add(new LavelModel()
                {
                    Id = ChekInt(tmp.Attribute("Id")),
                    ParemtId = ChekInt(tmp.Attribute("paremtId")),
                    Name = tmp.Attribute("Name").Value,
                    Comment = tmp.Attribute("Comment").Value,
                });
                foreach (var tmp1 in tmp.Elements("level"))
                {
                    LavelItem.Add(new LavelModel()
                    {
                        Id = ChekInt(tmp1.Attribute("Id")),
                        ParemtId = ChekInt(tmp1.Attribute("paremtId")),
                        Name = tmp1.Attribute("Name").Value,
                        Comment = tmp1.Attribute("Comment").Value,
                    });

                    foreach (var tmp2 in tmp1.Elements("Param"))
                    {
                        ParamItem.Add(new ParamModel()
                        {
                            Id = ChekInt(tmp2.Attribute("Id")),
                            ParamID = ChekInt(tmp2.Attribute("ParamID")),
                            Name = tmp2.Attribute("Name").Value,
                            Type = GetType(tmp2.Attribute("Type").Value),
                            Val = tmp2.Attribute("Val").Value,
                            Comment = tmp2.Attribute("Comment").Value,
                        });
                    }
                }
            }
        }
        private int ChekInt(XAttribute str)
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
           // var item = new DataItem("Welcome to MVVM Light");
           // callback(item, null);
        }
        #region функции для работы с уровнями
        public void GetDataLevel(Action<List<LavelModel>, Exception> callback)
        {
            LoadData();
            callback(LavelItem, null);
        }
        public void SaveLavel(LavelModel lavel)
        {
            // Обновить уровень
            XDocument xDoc = XDocument.Load(NameFile);
            // записать                 
            foreach (XElement tmp in xDoc.Element("ProgramParam").Elements("level"))
            {
                foreach (XElement tmp1 in tmp.Elements("level"))
                {
                    if (lavel.Id != 0)
                    {
                        if (tmp1.Attribute("Id").Value.ToString() == lavel.Id.ToString())
                        {
                            tmp1.Attribute("Name").Value = lavel.Name;
                            tmp1.Attribute("Comment").Value = lavel.Comment;
                            break;
                        }
                    }
                }
            }
            xDoc.Save(NameFile);
        }
        public void InsertLavel(LavelModel lavel)
        {
            // Вставить новый уровень
            XDocument xDoc = XDocument.Load(NameFile);
            var tmp = xDoc.Element("ProgramParam").Elements("level").First(i => i.Attribute("Id").Value.ToString() == lavel.ParemtId.ToString());
            tmp.Add(new XElement("level",
                        new XAttribute("Id", lavel.Id),
                        new XAttribute("paremtId", lavel.ParemtId),
                        new XAttribute("Name", lavel.Name),
                        new XAttribute("Comment", "")
                ));
            xDoc.Save(NameFile);
        }
        public void AddLavel(int id)
        {
            XDocument xDoc = XDocument.Load(NameFile);
            var tmp = xDoc.Element("ProgramParam").Elements("level").First(i => i.Attribute("Id").Value.ToString() == id.ToString());
            tmp.Add(new XElement("level",
                        new XAttribute("Id", NewIndexLavel),
                        new XAttribute("paremtId", id),
                        new XAttribute("Name", "New Lavel"),
                        new XAttribute("Comment", "")
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
        public void GetParam(Action<List<ParamModel>, Exception> callback)
        {
            LoadData();
            callback(ParamItem, null);
        }
        public List<LavelModel> GetLavelModels(string NameLavel) => LavelItem.FindAll(i => i.ParemtId == LavelItem.Find(i1 => i1.Name == NameLavel).Id);
        public string GetValParam(string lavel, string nameParam)
        {

            return null;
        }
        public int GetQuantity (string lavel, string nameParam)
        {
            HashSet<string> arr = new HashSet<string>();
            List<LavelModel> ListTMP = GetLavelModels(lavel);
            foreach(var tmp in ListTMP)
            {
                List<ParamModel> paramModels = GetParam(tmp.Id);
                arr.Add(paramModels.Find(i => i.Name == nameParam).Val);                
            }            
            return arr.Count-1;
        }

        public List<ParamModel> GetParam(int id) => ParamItem.FindAll(i => i.ParamID == id);
        public string GetParamValue(int id, string name) => GetParam(id).Find(i1 => i1.Name.Trim() == name).Val;
        private void WriteParam(XElement tmp, ParamModel param)
        {
            tmp.Add(new XElement("Param",
                      new XAttribute("Id", GetNewIndexParam()),
                      new XAttribute("ParamID", param.ParamID),
                      new XAttribute("Name", param.Name),
                      new XAttribute("Type", param.Type),
                      new XAttribute("Val", param.Val),
                      new XAttribute("Comment", param.Comment)
                      ));
        }
        private void UpdateParam(XElement tmp, ParamModel param)
        {
            tmp.Attribute("Name").Value = param.Name;
            tmp.Attribute("Type").Value = param.Type.ToString();
            tmp.Attribute("Val").Value = param.Val;
            tmp.Attribute("Comment").Value = param.Comment;
        }
        public void SaveParam(ParamModel param)
        {
            XDocument xDoc = XDocument.Load(NameFile);            
            // записать                 
            foreach (XElement tmp in xDoc.Element("ProgramParam").Elements("level"))
            {
                foreach (XElement tmp1 in tmp.Elements("level"))
                {
                    if ((tmp1.Attribute("Id").Value.ToString() == param.ParamID.ToString()) && (param.Id == 0))
                    {
                        WriteParam(tmp1, param);
                        break;
                    }
                    foreach (XElement tmp2 in tmp1.Elements("Param"))
                    {
                        if (param.Id != 0)
                        {
                            if (tmp2.Attribute("Id").Value.ToString() == param.Id.ToString())
                            {
                                UpdateParam(tmp2, param);
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
