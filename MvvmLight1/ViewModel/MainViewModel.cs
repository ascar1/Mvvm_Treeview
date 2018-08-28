using GalaSoft.MvvmLight;
using MvvmLight1.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public ObservableCollection <LavelViewModel> LavelList { get; private set; }
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
            LavelList = new ObservableCollection<LavelViewModel>();
            var rootElement = list.Where(c => c.paremtId == -1);
            foreach (var rootCategory in rootElement)
            {
                LavelViewModel tmp = new LavelViewModel(rootCategory);
                LavelList.Add(tmp);
                LavelViewModel.setChild(tmp, list);                    
            }
        }

        private void LoadParam(List<ParamModel> list, int id)
        {
            ParamList = new ObservableCollection<ParamViewModel>();
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
            _dataService.GetParam(
                (item,error) =>
                {
                    if (error!= null)
                    {
                        return;
                    }
                    LoadParam(item, 6);
                }
                );
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}