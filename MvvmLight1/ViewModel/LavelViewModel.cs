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
        public LavelViewModel()
        {

        }
        public LavelViewModel (LavelModel model)
        {
            _lavelModel = model;

            Children = new ObservableCollection<LavelViewModel>();
        }
        #region обработка выбора         
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                if (value)
                {
                    OnPropertyChanged();                   
                }
            }
        }
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
            }
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

