using GalaSoft.MvvmLight;
using MainApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static MainApp.Model.DataService;

namespace MainApp.ViewModel
{
    public class ParamViewModel : ViewModelBase, IEditableObject
    {
        private ParamModel _param;        
        private IParamDataService _dataService = new ParamDataService();
        private ParamViewModel backupCopy;
        public ObservableCollection<ParamViewModel> Param;
        private bool isNew;
        private bool inEdit;
        #region constructor
        public ParamViewModel()
        {
            _param = new ParamModel();
            isNew = true;
        }
        public ParamViewModel(ParamModel param)
        {
            _param = param;
            isNew = false;
        }
        #endregion

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
            set { _param.name = value; }
        }
        public DataType type
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
        public void BeginEdit()
        {
            if (inEdit) return;
            if (_param.id == 0) isNew = true;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as ParamViewModel;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            this.name = backupCopy.name;
            this.type = backupCopy.type;
            this.val = backupCopy.val;
            this.comment = backupCopy.comment;
        }

        public void EndEdit()
        {
            if ((!inEdit) && (MessageBox.Show("Save changes?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
            {
                _dataService.SaveParam(this._param);
                isNew = false;
                inEdit = false;
            }
            else
            {
                CancelEdit();
            }
        }
    }
}
