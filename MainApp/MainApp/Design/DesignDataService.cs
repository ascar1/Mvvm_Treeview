using System;
using System.Collections.Generic;
using MainApp.Model;

namespace MainApp.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem("Welcome to MVVM Light [design]");
            callback(item, null);
        }

        public List<FileArr> GetFileArrs()
        {
            throw new NotImplementedException();
        }

        public void LoadData(Action<List<MasterPointModel>, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}