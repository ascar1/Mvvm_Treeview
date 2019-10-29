using GalaSoft.MvvmLight;
using MainApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModel
{
    public class LavelViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly LavelModel _lavelModel;
        private ParamDataService _dataService = _dataService.getin //= new ParamDataService();
        private LavelViewModel _parent;
        #region constructor
        public LavelViewModel(LavelModel model)
        {
            _lavelModel = model;
            Children = new ObservableCollection<LavelViewModel>();
        }
        public LavelViewModel(LavelModel model, bool SelectFlag)
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
            get
            {

                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
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
            set
            {

                _isEditMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEditMode"));
            }
        }
#pragma warning disable CS0108 // "LavelViewModel.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // "LavelViewModel.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.

        #endregion
        public int ID
        {
            get => _lavelModel.Id;
            set => _lavelModel.Id = value;
        }
        public int ParentID
        {
            get
            {
                return _lavelModel.ParemtId;
            }
            set
            {
                _lavelModel.ParemtId = value;
            }
        }
        public string Name
        {
            get
            {
                return _lavelModel.Name;
            }
            set
            {
                _lavelModel.Name = value;
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
        public bool Test
        {
            get
            {
                /*
                if (_lavelModel.name == "Analiz1") return _IsSelected; 
                else return false;*/
                return _IsSelected;
            }
        }
        public bool IsRoot
        {
            get { return _lavelModel.ParemtId == -1; }
        }
        private bool _IsNew;
        public bool IsNew
        {
            get => _IsNew;
            set => _IsNew = value;
        }
        public LavelViewModel Parent
        {
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
            if (!IsNew)
                //Обновить запись
                _dataService.SaveLavel(this._lavelModel);
            else
                // Добавить новую запись 
                _dataService.InsertLavel(this._lavelModel);
                
        }
        public void Delete()
        {
            _dataService.DeleteLavel(this.ID);
        }
    }
}
