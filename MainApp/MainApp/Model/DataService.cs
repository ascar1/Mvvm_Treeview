using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Windows;

namespace MainApp.Model
{
    public enum DataType { String, Int, Bool };
    public class DataService : IDataService
    {

        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
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
    }
}