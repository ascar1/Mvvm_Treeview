using GalaSoft.MvvmLight;
using MainApp.Command;
using MainApp.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace MainApp.ViewModel
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

        public List<MasterPointModel> MasterChartPoint { get; set; }
        public List<FileArrModel> FileArr { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
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
            _dataService.LoadData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        return;
                    }
                    MasterChartPoint = item;
                });
            FileArr = _dataService.GetFileArrs();

            Tabs.Add(new ChartData(_dataService));
            Tabs.Last().Event1 += MainViewModel_event1;
            Tabs.Last().Event2 += MainViewModel_Event2;

            
            //Tabs.Last().PropertyChanged += MainViewModel_PropertyChanged;
            Tabs.Add(new Tab1Vm());
            Tabs.Last().Event1 += MainViewModel_event1;
            
            Tabs2.Add(new TabAnalizResult(_dataService));
            Tabs2.Last().Event1 += MainViewModel_event1;
            SelectedTab2 = Tabs2.FirstOrDefault();

            SelectedTab = Tabs.FirstOrDefault();
            Str = "Строка 2 Столбец 1";
        }

        private void MainViewModel_Event2(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MessageBox.Show("!!!");
            throw new NotImplementedException();
        }

        private string _Str;
        public string Str
        {
            get { return _Str; }
            set {
                Set<string>(() => this.Str, ref _Str, value);
                //_Str = value;
            }
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainViewModel_event1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           // MessageBox.Show("!!!");
            Tabs.Remove(_selectedTab);
            
        }

        private ObservableCollection<TabVm> _tabs;
        public ObservableCollection<TabVm> Tabs
        {
            get {
                
                return _tabs ?? (_tabs = new ObservableCollection<TabVm>());
                }
            
        }
        private TabVm _selectedTab;
        public TabVm SelectedTab
        {
            get { return _selectedTab; }
            set { _selectedTab = value; }
        }

        private ObservableCollection<TabVM2> _Tabs2;
        public ObservableCollection<TabVM2> Tabs2
        {
            get
            {
                return _Tabs2 ?? (_Tabs2 = new ObservableCollection<TabVM2>());
            }
        }
        private TabVM2 _selectedTab2;
        public TabVM2 SelectedTab2
        {
            get { return _selectedTab2; }
            set { _selectedTab2 = value; }
        }


        #region Обработка команд
        private MyCommand _TestCommand;
        public MyCommand TestCommand
        {
            get
            {
                return _TestCommand ?? (_TestCommand = new MyCommand(obj =>
                {
                    //Tabs.Add(new Tab2Vm());
                    //Tabs.Last().event1 += MainViewModel_event1;
                    //SelectedTab = Tabs.Last();
                    //Str = "!!!";
                    //nPropertyChanged("Str");
                    Tabs.Add(new TestViewModel(_dataService));
                    Tabs.Last().Event1 += MainViewModel_event1;

                }));
            }
        }

#pragma warning disable CS0108 // "MainViewModel.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // "MainViewModel.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.
        protected void OnPropertyChanged(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private MyCommand _DeleteItemCommand;
        public MyCommand DeleteItemCommand
        {
            get
            {
                return _DeleteItemCommand ?? (_DeleteItemCommand = new MyCommand(obj =>
                {                    
                    Tabs.Remove(_selectedTab);                    
                }));
            }
        }
        private MyCommand _OpenParamCommand;
        public MyCommand OpenParamCommand
        {
            get
            {
                return _OpenParamCommand ?? (_OpenParamCommand = new MyCommand(obj =>
                {
                    Tabs.Add(new Tab1Vm());
                    Tabs.Last().Event1 += MainViewModel_event1;
                    SelectedTab = Tabs.Last();
                }));
            }
        }
        private MyCommand _OpenDataCommand;
        public MyCommand OpenDataCommand
        {
            get
            {
                return _OpenDataCommand ?? (_OpenDataCommand = new MyCommand(obj =>
                {
                    Tabs.Add(new TabDataParam(_dataService));
                    Tabs.Last().Event1 += MainViewModel_event1;
                    SelectedTab = Tabs.Last();
                }));
            }
        }
        private MyCommand _OpenViewDataCommand;
        public MyCommand OpenViewDataCommand
        {
            get
            {
                return _OpenViewDataCommand ?? (_OpenViewDataCommand = new MyCommand(obj =>
                {
                    Tabs.Add(new TabData(_dataService, true));
                    Tabs.Last().Event1 += MainViewModel_event1;
                    SelectedTab = Tabs.Last();
                }));
            }
        }
        private MyCommand _OpenViewWorkDataCommand;
        public MyCommand OpenViewWorkDataCommand
        {
            get
            {
                return _OpenViewWorkDataCommand ?? (_OpenViewWorkDataCommand = new MyCommand(obj =>
                {
                    Tabs.Add(new TabData(_dataService, false));
                    Tabs.Last().Event1 += MainViewModel_event1;
                    SelectedTab = Tabs.Last();
                }));
            }
        }
        private MyCommand _OpenChartViewWorkDataCommand;
        public MyCommand OpenChartViewWorkDataCommand
        {
            get
            {
                return _OpenChartViewWorkDataCommand ?? (_OpenChartViewWorkDataCommand = new MyCommand(obj =>
                {
                    Tabs.Add(new ChartData(_dataService));
                    Tabs.Last().Event1 += MainViewModel_event1;
                    //Tabs.Last().Event2 += MainViewModel_event2;
                    SelectedTab = Tabs.Last();
                }));
            }
        }
        #endregion

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}