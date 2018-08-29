using GalaSoft.MvvmLight;
using MvvmLight1.Command;
using MvvmLight1.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

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

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        private MyCommand myCommand;
        public MyCommand MyCommand
        {
            get
            {
                return myCommand ?? (myCommand = new MyCommand(obj =>
                {
                    MessageBox.Show("Команда");
                    ParamList.Clear();
                    _dataService.GetParam(
                        (item, error) =>
                        {
                            if (error != null)
                            {
                                return;
                            }
                            LoadParam(item, 4);
                        }
                        );
                }));
            }
        }
        public BindingList<LavelViewModel> LavelList { get; private set; }
        public ObservableCollection <ParamViewModel> ParamList { get; private set; }
 
        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        ///  Загрузка Lavel List 
        /// </summary>
        /// <param name="list"></param>
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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            LavelList = new BindingList<LavelViewModel>();
            ParamList = new ObservableCollection<ParamViewModel>();
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            _dataService.GetDataLevel(
                (item,error) =>
                {
                    if (error!= null)
                    {
                        return;
                    }
                    LoadLavel(item);
                }
                );
        }

        private void ItemsOnCollectionChanged1(object sender, PropertyChangedEventArgs e)
        {          
            if (e.PropertyName == "IsSelected")
            {
                var Lavel = (LavelViewModel)sender;
                ParamList.Clear();
                _dataService.GetParam(
                    (item, error) =>
                    {
                        if (error != null)
                        {
                            return;
                        }
                        LoadParam(item, Lavel.ID);
                    }
                    );
            }
        }
    }
}