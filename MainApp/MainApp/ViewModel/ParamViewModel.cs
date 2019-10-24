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

        public int Id
        {
            get => _param.Id;
            set => _param.Id = value;
        }
        public int PerentID
        {
            get => _param.ParamID;
            set => _param.ParamID = value;
        }
        public string Name
        {
            get { return _param.Name; }
            set { _param.Name = value; }
        }
        public DataType Type
        {
            get { return _param.Type; }
            set
            {
                _param.Type = value;
            }
        }
        public string Comment
        {
            get { return _param.Comment; }
            set
            {
                _param.Comment = value;
            }
        }
        public string Val
        {
            get { return _param.Val; }
            set
            {
                _param.Val = value;
            }
        }
        public void BeginEdit()
        {
            if (inEdit) return;
            if (_param.Id == 0) isNew = true;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as ParamViewModel;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            this.Name = backupCopy.Name;
            this.Type = backupCopy.Type;
            this.Val = backupCopy.Val;
            this.Comment = backupCopy.Comment;
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
