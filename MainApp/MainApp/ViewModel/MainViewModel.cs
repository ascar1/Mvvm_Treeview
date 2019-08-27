using GalaSoft.MvvmLight;
using MainApp.Command;
using MainApp.Model;
using System;
using System.Collections.ObjectModel;
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

            Tabs.Add(new Tab1Vm());
            Tabs.Add(new Tab2Vm());
            
            SelectedTab = Tabs.FirstOrDefault();
        }

        public ObservableCollection<TabVm> Tabs
        {
            get { return _tabs ?? (_tabs = new ObservableCollection<TabVm>()); }
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
                    //Tabs.Add(new Tab1Vm());
                    Tabs.Remove(_selectedTab);
                    MessageBox.Show("Команда " + " Button !!! ");
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

        #endregion

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}