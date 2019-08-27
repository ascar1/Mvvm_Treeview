using GalaSoft.MvvmLight;
using MainApp.Command;
using MainApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainApp.ViewModel
{
    public abstract class TabVm : ViewModelBase, INotifyPropertyChanged
    {
        public string Header { get; }
        delegate void GetName();
        public TabVm(string header)
        {
            Header = header;
        }
        #region обработка команд
        private MyCommand _TestCommand;
        public MyCommand TestCommand
        {
            get
            {
                return _TestCommand ?? (_TestCommand = new MyCommand(obj =>
                {
                    MessageBox.Show("Команда " + " Button !!! ");
                }));
            }
        }
        private MyCommand _CloseCommand;
        public MyCommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new MyCommand(obj =>
                {
                    event1.Invoke(this, new PropertyChangedEventArgs("propertyName"));
                }));
            }
        }

        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler event1;
    }

    public class Tab1Vm : TabVm
    {
        private readonly IParamDataService _paramDataService;
        public ObservableCollection<LavelViewModel> LavelList { get; set; }
        public ObservableCollection<ParamViewModel> ParamList { get; private set; }
        public ObservableCollection<string> type { get; private set; }
        public LavelViewModel SelectedLavel { get; private set; }
        public ParamViewModel SelectedROW { get; set; }
        public int parentSelected;
        public int idSelected;
        public Tab1Vm()
            : base("Параметры")
        {
            LavelList = new ObservableCollection<LavelViewModel>();
            ParamList = new ObservableCollection<ParamViewModel>();
            type = new ObservableCollection<string>();

            _paramDataService = new ParamDataService();
            _paramDataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        return;
                    }
                });
            _paramDataService.GetDataLevel((item, error) => { if (error != null) { return; } LoadLavel(item); });

            Edit = new DelegateCommand<object>(arg =>
            {
                LavelViewModel tmp = LavelList.FirstOrDefault(i => i.IsSelected);
                if (tmp != null)
                {
                    tmp.IsEditMode = true;
                }
            });
        }
        private void LoadLavel(List<LavelModel> list)
        {
            var rootElement = list.Where(c => c.paremtId == -1);
            foreach (var rootCategory in rootElement)
            {
                LavelViewModel tmp = new LavelViewModel(rootCategory);
                tmp.PropertyChanged += ItemsOnCollectionChanged1;
                LavelList.Add(tmp);
                setChild(tmp, list);
            }
        }
        private void ItemsOnCollectionChanged1(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                var tmp = (LavelViewModel)sender;
                if (tmp.IsSelected)
                {
                    SelectedLavel = (LavelViewModel)sender;
                }

                if (SelectedLavel.IsSelected == false)
                {
                    SelectedLavel.IsEditMode = false;
                }
                var Lavel = (LavelViewModel)sender;
                ParamList.Clear();
                _paramDataService.GetParam(
                    (item, error) =>
                    {
                        if (error != null)
                        {
                            return;
                        }
                        parentSelected = Lavel.ID;
                        LoadParam(item, Lavel.ID);
                    });
            }
        }
        public void setChild(LavelViewModel root, IList<LavelModel> source)
        {
            for (var i = 0; i < source.Count; i++)
            {
                if (root.ID != source[i].id && root.ID == source[i].paremtId)
                {
                    if (source[i].paremtId != -1)
                    {
                        LavelViewModel tmp = new LavelViewModel(source[i]);
                        tmp.PropertyChanged += ItemsOnCollectionChanged1;
                        root.Children.Add(tmp);
                        setChild(tmp, source);
                    }
                }
            }
        }
        private void LoadParam(List<ParamModel> list, int id)
        {
            var rootElement = list.Where(c => c.ParamID == id);
            foreach (var root in rootElement)
            {
                ParamList.Add(new ParamViewModel(root));
            }
        }
        private int Find(int id, ObservableCollection<LavelViewModel> tmp)
        {
            if (tmp == null)
                return -1;

            foreach (var tmp1 in tmp)
            {
                if (tmp1.ID == id)
                {
                    LavelModel _tmp = new LavelModel();
                    _tmp.name = "new";
                    _tmp.id = _paramDataService.getNewIndexLavel();
                    _tmp.paremtId = id;

                    LavelViewModel _tmp_ = new LavelViewModel(_tmp);
                    _tmp_.PropertyChanged += ItemsOnCollectionChanged1;
                    _tmp_.IsEditMode = true;
                    _tmp_.IsSelected = true;
                    _tmp_.IsExpanded = true;
                    _tmp_.IsNew = true;
                    tmp1.Children.Add(_tmp_);
                    return tmp1.ID;
                }
                else
                {
                    int i = Find(id, tmp1.Children);
                    if (i != -1)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
        private void NewLavel(int id)
        {
            // --- > Найти и добавить 
            Find(id, LavelList);
        }

        #region Command
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs("propertyName"));
        }
        public DelegateCommand<object> Edit { get; private set; }
        private MyCommand myCommand;
        public MyCommand MyCommand
        {
            // тестовая комманда 
            get
            {
                return myCommand ?? (myCommand = new MyCommand(obj =>
                {
                    MessageBox.Show("Команда " + " Button !!! " + LavelList.Count());
                    LavelList[1].Children.Remove(SelectedLavel);


                }));
            }
        }
        private MyCommand relayCommand;
        public MyCommand RelayCommand
        {
            get
            {
                return relayCommand ?? (relayCommand = new MyCommand(obj =>
                {
                    //MessageBox.Show("Команда RelayCommand: " /*+ SelectedROW.isNew.ToString()*/);                    
                }));
            }
        }

        private MyCommand mouseCommand;
        public MyCommand MouseCommand => mouseCommand ?? (mouseCommand = new MyCommand(obj =>
        {
            MessageBox.Show("Команда MouseCommand: " /* +  SelectedROW.name.ToString()*/);
        }));

        private MyCommand newItem;
        public MyCommand NewItem => newItem ?? (newItem = new MyCommand(obj =>
        {
            ParamList[ParamList.Count - 1].perentID = parentSelected;
        }));
        private MyCommand deleteRow;
        public MyCommand DeleteRow => deleteRow ?? (deleteRow = new MyCommand(obj =>
        {
            if ((MessageBox.Show("Delete selected element?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
            {
                int id = SelectedROW.id;
                int ind = ParamList.IndexOf(ParamList.Where(i => i.id == id).FirstOrDefault());
                ParamList.RemoveAt(ind);
                _paramDataService.DeleteParam(id);
            }
        }));
        private MyCommand addLavel;
        public MyCommand AddLavel => addLavel ?? (addLavel = new MyCommand(obj =>
        {
            NewLavel(SelectedLavel.ID);
        }));

        private MyCommand editLavel;
        public MyCommand EditLavel
        {
            get
            {
                return editLavel ?? (editLavel = new MyCommand(obj =>
                {
                    if (SelectedLavel != null)
                    {
                        MessageBox.Show("1");
                        SelectedLavel.IsEditMode = true;
                        SelectedLavel.IsExpanded = true;
                        SelectedLavel.CNGName = SelectedLavel.name;
                        NotifyPropertyChanged("LavelList");
                    }
                }));
            }
        }

        private MyCommand commitLavel;
        public MyCommand CommitLavel
        {
            get
            {
                return commitLavel ?? (commitLavel = new MyCommand(obj =>
                {
                    if (SelectedLavel.name != SelectedLavel.CNGName)
                    {
                        if (MessageBox.Show("Save changes?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            SelectedLavel.Save();
                        }
                        else
                        {
                            if (SelectedLavel.CNGName != null)
                            {
                                SelectedLavel.name = SelectedLavel.CNGName;
                            }
                            else
                            {

                            }
                        }
                    }
                    if (SelectedLavel != null)
                    {
                        SelectedLavel.IsEditMode = false;
                    }

                }));
            }
        }

        private int FindLavel(int id, ObservableCollection<LavelViewModel> tmp)
        {
            if (tmp == null)
                return -1;

            foreach (var tmp1 in tmp)
            {
                if (tmp1.ID == id)
                {
                    tmp.Remove(SelectedLavel);
                    return id;
                }
                else
                {
                    int i = FindLavel(id, tmp1.Children);
                    if (i != -1)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private MyCommand deletelavel;
        public MyCommand DeleteLavel
        {
            get
            {
                return deletelavel ?? (deletelavel = new MyCommand(obj =>
                {
                    if (SelectedLavel != null)
                    {
                        if (MessageBox.Show("Delete lavel?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            SelectedLavel.Delete();
                            FindLavel(SelectedLavel.ID, LavelList);
                            NotifyPropertyChanged("DeleteLavel");
                        }
                    }

                }));
            }
        }
        #endregion
    }

    public class Tab2Vm : TabVm
    {
        public Tab2Vm()
            : base("Tab 2 ________________")
        {
        }
    }
}
