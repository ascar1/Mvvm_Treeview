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
    public class LavelViewModel :ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly LavelModel _lavelModel;
        private LavelViewModel _parent;

        #region constructor
        public LavelViewModel()
        {

        }
        public LavelViewModel (LavelModel model)
        {
            _lavelModel = model;
            Children = new ObservableCollection<LavelViewModel>();
        }
        #endregion
        #region обработка выбора         
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                if (!_IsSelected) IsEditMode = false; 
/*                if (value)
                {*/
                   OnPropertyChanged("IsSelected");                   
                //}
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
                    OnPropertyChanged();
                }
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { _isEditMode = value; }
        }
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
    }
}

