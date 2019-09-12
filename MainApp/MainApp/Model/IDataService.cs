using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainApp.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
        void LoadData(Action<List<MasterPointModel>, Exception> callback);
        List<FileArrModel> GetFileArrs();

        List<PointModel> GetMasterPoint(string scale, string tiker);
    }
}
