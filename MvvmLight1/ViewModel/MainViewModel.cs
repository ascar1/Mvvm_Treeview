﻿using GalaSoft.MvvmLight;
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
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IDataService _dataService;
        public ObservableCollection<LavelViewModel> LavelList { get; set; }
        public ObservableCollection<ParamViewModel> ParamList { get; private set; }
        public ObservableCollection<string> type { get; private set; }
        public LavelViewModel SelectedLavel { get; private set; }
        public ParamViewModel SelectedROW { get; set; }
        public int parentSelected;
        public int idSelected;

        #region constructor
        public MainViewModel(IDataService dataService)
        {
            LavelList = new ObservableCollection<LavelViewModel>();
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

            Edit = new DelegateCommand<object>(arg =>
           {
               LavelViewModel tmp = LavelList.FirstOrDefault(i => i.IsSelected);
               if (tmp != null)
               {
                   tmp.IsEditMode = true;
               }
           });
        }
        #endregion
        public DelegateCommand<object> Edit { get; private set; }

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs("propertyName"));
        }
        
        private int Find (int id, ObservableCollection<LavelViewModel> tmp)
        {
            if (tmp == null)
                return -1;
            
            foreach (var tmp1 in tmp)
            {
                if (tmp1.ID == id)
                {
                    LavelModel _tmp = new LavelModel();
                    _tmp.name = "new";                    
                    _tmp.id = _dataService.getNewIndexLavel();
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

        // TODO: добовление нового элемента
        private void NewLavel(int id)
        {            
            // --- > Найти и добавить 
            Find(id, LavelList);
        }

        #region Command
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
                                                            _dataService.DeleteParam(id);
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

        private int FindLavel (int id, ObservableCollection<LavelViewModel> tmp)
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
}