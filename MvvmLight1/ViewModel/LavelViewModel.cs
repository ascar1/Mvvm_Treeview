using GalaSoft.MvvmLight;
using MvvmLight1.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MvvmLight1.Model
{
    public class LavelViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly LavelModel _lavelModel;
        private IDataService _dataService = new DataService();
        private LavelViewModel _parent;
        #region constructor
        public LavelViewModel (LavelModel model)
        {
            _lavelModel = model;            
            Children = new ObservableCollection<LavelViewModel>();            
        }
        public LavelViewModel (LavelModel model, bool SelectFlag)
        {
            _lavelModel = model;
            this.IsEditMode = SelectFlag;
            this.IsSelected = SelectFlag;            
            Children = new ObservableCollection<LavelViewModel>();
        }
        #endregion
        #region обработка выбора                
        private bool _IsSelected;
        public bool IsSelected
        {
            get {
                
                return _IsSelected;
            }
            set
            {              
                _IsSelected = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("IsSelected"));                                            
            }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set {
                _IsExpanded = value;
                if (value)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsExpanded"));
                }
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set {
                
                _isEditMode = value;
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs("IsEditMode"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        public int ID
        {
            get => _lavelModel.id;
            set => _lavelModel.id = value;
        }
        public int ParentID
        {
            get
            {
                return _lavelModel.paremtId;
            }
            set
            {
                _lavelModel.paremtId = value;
            }
        }
        public string name {
            get
            {
                return _lavelModel.name;
            }
            set
            {
                _lavelModel.name = value;
                //Вызвать событие обновление для интефейса  
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("name"));
            }
        }

        private string _CNGName;
        public string CNGName
        {
            get
            {
                return _CNGName;
            }
            set
            {
                _CNGName = value;
            }
        }
        public bool test
        {
            get
            {
                /*
                if (_lavelModel.name == "Analiz1") return _IsSelected; 
                else return false;*/
                return _IsSelected;
            }
        }       
        public bool isRoot
        {
            get { return _lavelModel.paremtId == -1; }
        }
        public LavelViewModel parent {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                if (value != null)
                {
                    ParentID = value.ID;
                }
            }
        }
        public ObservableCollection<LavelViewModel> Children { get; set; }
        public void Save()
        {
            _dataService.SaveLavel(this._lavelModel);
        }
    }
}

