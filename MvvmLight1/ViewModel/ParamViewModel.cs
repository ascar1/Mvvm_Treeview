using GalaSoft.MvvmLight;
using MvvmLight1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLight1.ViewModel
{
    public class ParamViewModel: ViewModelBase
    {
        private readonly ParamModel _param;

        public ObservableCollection<ParamViewModel> Param;

        public ParamViewModel (ParamModel param)
        {
            _param = param;
        }
        public int id
        {
            get => _param.id;
            set => _param.id = value;
        }
        public int perentID
        {
            get => _param.ParamID;
            set => _param.ParamID = value;
        }
        public string name 
        {
            get { return _param.name; }
            set
            {
                _param.name = value;
            }
        }
        public string type
        {
            get { return _param.type; }
            set
            {
                _param.type = value;
            }
        }
        public string comment
        {
            get { return _param.comment; }
            set
            {
                _param.comment = value;
            }
        }
        public string val
        {
            get { return _param.val; }
            set
            {
                _param.val = value;
            }
        }
    }
}
