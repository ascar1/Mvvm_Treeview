using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    interface IParamDataService
    {
        void GetData(Action<DataItem, Exception> callback);
        void GetDataLevel(Action<List<LavelModel>, Exception> callback);
        void GetParam(Action<List<ParamModel>, Exception> callback);
        void SaveParam(ParamModel param);
        void DeleteParam(int id);
        void AddLavel(int id);
        void DeleteLavel(int i);
        void SaveLavel(LavelModel lavel);
        void InsertLavel(LavelModel lavel);
        int getNewIndexLavel();
    }
}
