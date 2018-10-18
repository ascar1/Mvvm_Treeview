using GalaSoft.MvvmLight;
using System.Windows.Interactivity;
using MvvmLight1.Command;
using MvvmLight1.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>    
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        public BindingList<LavelViewModel> LavelList { get; set; }
        public ObservableCollection<ParamViewModel> ParamList { get; private set; }
        public ObservableCollection<string> type { get; private set; }
        public LavelViewModel SelectedLavel { get; private set; }
        public ParamViewModel SelectedROW { get; set; }
        public int parentSelected;
        public int idSelected;

        public bool testparam
        {
            get
            {
                return true;
            }
        }

        #region constructor
        public MainViewModel(IDataService dataService)
        {
            LavelList = new BindingList<LavelViewModel>();
            ParamList = new ObservableCollection<ParamViewModel>();
            type = new ObservableCollection<string>();
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                });
            _dataService.GetDataLevel(
                (item, error) =>
                {
                    if (error != null)
                    {
                        return;
                    }
                    LoadLavel(item);
                });
        }
        #endregion

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
        private void ItemsOnCollectionChanged1(object sender, PropertyChangedEventArgs e)
        {
            
            if (e.PropertyName == "IsExpanded")
            {
                MessageBox.Show("2");
            }
            if (e.PropertyName == "IsSelected")
            {
                SelectedLavel = (LavelViewModel)sender;
                var Lavel = (LavelViewModel)sender;
                ParamList.Clear();
                _dataService.GetParam(
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

        #region Command
        private MyCommand myCommand;
        public MyCommand MyCommand
        {
            get
            {
                return myCommand ?? (myCommand = new MyCommand(obj =>
                {
                    //AddNewParam();
                    MessageBox.Show("Команда " + " Test " + LavelList.Count());
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
                                                            _dataService.DeleteParam(id);
                                                        }
                                                    }));
        private MyCommand addLavel;
        public MyCommand AddLavel => addLavel ?? (addLavel = new MyCommand(obj =>
                                                   {
                                                       MessageBox.Show("Add Lavel!!! " + parentSelected.ToString());
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
                        SelectedLavel.IsEditMode = true;
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
                    if (SelectedLavel != null)
                    {
                        MessageBox.Show("CommitLavel");
                        SelectedLavel.IsEditMode = false;
                    }
                        
                }));
            }
        }




        #endregion
    }
}