using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmLight1.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
        void GetDataLevel (Action<List<LavelModel>, Exception> callback);
        void GetParam(Action<List<ParamModel>, Exception> callback);
        //void LoadData(Action<List<ParamModel>, Exception> callback, Action<List<LavelModel>, Exception> callback1);
    }
}
