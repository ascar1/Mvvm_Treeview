using GalaSoft.MvvmLight;
using MainApp.Command;
using MainApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MainApp.View
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
        public List<FileArrModel> fileArr { get; set; }

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
            fileArr = _dataService.GetFileArrs();

            Tabs.Add(new ChartData(_dataService));
            Tabs.Last().event1 += MainViewModel_event1;
            Tabs.Add(new Tab1Vm());
            Tabs.Last().event1 += MainViewModel_event1;


            SelectedTab = Tabs.FirstOrDefault();
        }

        private void MainViewModel_event1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           // MessageBox.Show("!!!");
            Tabs.Remove(_selectedTab);
            
        }

        public ObservableCollection<TabVm> Tabs
        {
            get {
                
                return _tabs ?? (_tabs = new ObservableCollection<TabVm>());
                }
            
        }
        private ObservableCollection<TabVm> _tabs;

        public TabVm SelectedTab
        {
            get { return _selectedTab; }
            set { _selectedTab = value; }
        }
        
        private TabVm _selectedTab;

        #region Обработка команд
        private MyCommand _TestCommand;
        public MyCommand TestCommand
        {
            get
            {
                return _TestCommand ?? (_TestCommand = new MyCommand(obj =>
                {                   
                    Tabs.Add(new Tab2Vm());
                    Tabs.Last().event1 += MainViewModel_event1;
                    SelectedTab = Tabs.Last();
                }));
            }
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
                    Tabs.Last().event1 += MainViewModel_event1;
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
                    Tabs.Last().event1 += MainViewModel_event1;
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
                    Tabs.Last().event1 += MainViewModel_event1;
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
                    Tabs.Last().event1 += MainViewModel_event1;
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
                    Tabs.Last().event1 += MainViewModel_event1;
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