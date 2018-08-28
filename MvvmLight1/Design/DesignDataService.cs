using System;
using System.Collections.Generic;
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
            item.Add(new ParamModel() { id = 0, ParamID = 3, name = "param 1", type = "str", val = "val 1", comment = "#1" });
            item.Add(new ParamModel() { id = 0, ParamID = 3, name = "param 2", type = "str", val = "val 2", comment = "#2" });
            item.Add(new ParamModel() { id = 0, ParamID = 3, name = "param 3", type = "str", val = "val 3", comment = "#3" });
            item.Add(new ParamModel() { id = 0, ParamID = 4, name = "param 4", type = "str", val = "val 4", comment = "#4" });
            item.Add(new ParamModel() { id = 0, ParamID = 4, name = "param 5", type = "str", val = "val 5", comment = "#5" });
            item.Add(new ParamModel() { id = 0, ParamID = 4, name = "param 6", type = "str", val = "val 6", comment = "#6" });
            item.Add(new ParamModel() { id = 0, ParamID = 6, name = "param 7", type = "str", val = "val 7", comment = "#7" });
            item.Add(new ParamModel() { id = 0, ParamID = 6, name = "param 8", type = "str", val = "val 8", comment = "#8" });
            callback(item, null);
        }

    }
}