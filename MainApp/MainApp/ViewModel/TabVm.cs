using GalaSoft.MvvmLight;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
using System.Windows.Controls;
using System.Windows.Media;

namespace MainApp.ViewModel
{
    public abstract class TabVm : ViewModelBase, INotifyPropertyChanged
    {
        public string Header { get; set; }
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
                    MessageBox.Show("Команда TestCommand в abstract class TabVm" );
                    //Event2?.Invoke("Команда TestCommand в abstract class TabVm");
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
                    Event1?.Invoke(this, new PropertyChangedEventArgs("propertyName"));
                    Event2?.Invoke(this, new PropertyChangedEventArgs("propertyName"));
                }));
            }
        }
        public void Test()
        {
            MessageBox.Show("тест");
            Event2?.Invoke(this, new PropertyChangedEventArgs("propertyName"));
        }
        #endregion
       // public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler Event1;
        public event PropertyChangedEventHandler Event2;

        /*    public event AccountHandler Event2;
            protected bool EventIsNull()
            {
                return Event2 == null;
            }*/
    }

    public class Tab1Vm : TabVm
    {
        private readonly IParamDataService _paramDataService;
        public ObservableCollection<LavelViewModel> LavelList { get; set; }
        public ObservableCollection<ParamViewModel> ParamList { get; private set; }
        public ObservableCollection<string> Type { get; private set; }
        public LavelViewModel SelectedLavel { get; private set; }
        public ParamViewModel SelectedROW { get; set; }
        public int parentSelected;
        public int idSelected;
        public Tab1Vm()
            : base("Параметры")
        {
            LavelList = new ObservableCollection<LavelViewModel>();
            ParamList = new ObservableCollection<ParamViewModel>();
            Type = new ObservableCollection<string>();

            _paramDataService = new ParamDataService();
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
            var rootElement = list.Where(c => c.ParemtId == -1);
            foreach (var rootCategory in rootElement)
            {
                LavelViewModel tmp = new LavelViewModel(rootCategory);
                tmp.PropertyChanged += ItemsOnCollectionChanged1;
                LavelList.Add(tmp);
                SetChild(tmp, list);
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
        public void SetChild(LavelViewModel root, IList<LavelModel> source)
        {
            for (var i = 0; i < source.Count; i++)
            {
                if (root.ID != source[i].Id && root.ID == source[i].ParemtId)
                {
                    if (source[i].ParemtId != -1)
                    {
                        LavelViewModel tmp = new LavelViewModel(source[i]);
                        tmp.PropertyChanged += ItemsOnCollectionChanged1;
                        root.Children.Add(tmp);
                        SetChild(tmp, source);
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
                    LavelModel _tmp = new LavelModel
                    {
                        Name = "new",
                        Id = _paramDataService.NewIndexLavel,
                        ParemtId = id
                    };

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
#pragma warning disable CS0108 // "Tab1Vm.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // "Tab1Vm.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("propertyName"));
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
                    //MessageBox.Show("Команда " + " Button !!! " + LavelList.Count());
                    //NotifyPropertyChanged("111");
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
          //  MessageBox.Show("Команда MouseCommand: " /* +  SelectedROW.name.ToString()*/);
        }));

        private MyCommand newItem;
        public MyCommand NewItem => newItem ?? (newItem = new MyCommand(obj =>
        {
            ParamList[ParamList.Count - 1].PerentID = parentSelected;
        }));
        private MyCommand deleteRow;
        public MyCommand DeleteRow => deleteRow ?? (deleteRow = new MyCommand(obj =>
        {
            if ((MessageBox.Show("Delete selected element?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
            {
                int id = SelectedROW.Id;
                int ind = ParamList.IndexOf(ParamList.Where(i => i.Id == id).FirstOrDefault());
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
                       // MessageBox.Show("1");
                        SelectedLavel.IsEditMode = true;
                        SelectedLavel.IsExpanded = true;
                        SelectedLavel.CNGName = SelectedLavel.Name;
                        //NotifyPropertyChanged("LavelList");
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
                    if (SelectedLavel.Name != SelectedLavel.CNGName)
                    {
                        if (MessageBox.Show("Save changes?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            SelectedLavel.Save();
                        }
                        else
                        {
                            if (SelectedLavel.CNGName != null)
                            {
                                SelectedLavel.Name = SelectedLavel.CNGName;
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
                            //NotifyPropertyChanged("DeleteLavel");
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

    public class TabData: TabVm
    {
        private readonly IDataService _dataService;
        public ObservableCollection<FileViewModel> Files { get; set; }
        public ObservableCollection<PointModel> Points { get; set; }
        private bool FlagMaster;
        private IteratorModel iterator;
        public TabData(IDataService data, bool flagMaster )
            :base("Данные")
        {
            FlagMaster = flagMaster;
            if (flagMaster)
            {
                base.Header = "Master Data";
                Flag = Visibility.Collapsed;
            }
            else
            {
                base.Header = "Work Data";
                Flag = Visibility.Visible;
            }
            Files = new ObservableCollection<FileViewModel>();
            Points = new ObservableCollection<PointModel>();
            _dataService = data;
            List<FileArrModel> f = data.GetFileArrs();
            foreach (var i in f)
            {
                if (i.Work)
                {
                    Files.Add(new FileViewModel(i));
                    ListComboBoxItems.Add(i.Tiker);
                }                
            }
            listScaleItems.Add("60");
            listScaleItems.Add("D");
            iterator = IteratorModel.GetInstance(data.GetMasterPoints(), data.GetFileArrs());
        }
        private void LoadData(string tiker)
        {
            Points.Clear();
            foreach (PointModel i in _dataService.GetMasterPoint("60", Value))
            {
                Points.Add(i);
            };
        }

        private void LoadData1(string tiker, string skale)
        {
            Points.Clear();
            foreach (PointModel i in iterator.WorkPoints.Find(i => i.Tiker == tiker).Data.Find(i1=>i1.Scale==skale).Points)
            {
                Points.Add(i);
            };
        }

        private ObservableCollection<string> listComboBoxItems = new ObservableCollection<string>();
        public ObservableCollection<string> ListComboBoxItems
        {
            get { return listComboBoxItems; }
            set { listComboBoxItems = value; /*OnPropertyChanged();*/ }
        }

        string v;
        public string Value
        {
            get => v;
            set
            {
                Set(ref v, value);
                if (FlagMaster)
                {
                    LoadData(value);
                }               
            }
        }
        private ObservableCollection<string> listScaleItems = new ObservableCollection<string>();
        public ObservableCollection<string> ListScaleItems
        {
            get { return listScaleItems; }
            set { listScaleItems = value; /*OnPropertyChanged();*/ }
        }
        string scale;
        public string Scale
        {
            get => scale;
            set => Set(ref scale, value);
        }

        public Visibility Flag { get; set; }
        private MyCommand _NextCommand;
        public MyCommand NextCommand
        {
            get
            {
                return _NextCommand ?? (_NextCommand = new MyCommand(obj =>
                {
                   // MessageBox.Show("TabData NextCommand");
                    if (FlagMaster)
                    {
                        LoadData(Value);
                    }    
                    else
                    {

                        iterator.NextN(100);
                        LoadData1(v,scale);
                    }
                }));
            }
        }
        private MyCommand _AllCommand;
        public MyCommand AllCommand
        {
            get
            {
                return _AllCommand ?? (_AllCommand = new MyCommand(obj =>
                {
                    //MessageBox.Show("TabData AllCommand");
                    if (FlagMaster)
                    {
                        LoadData(Value);
                    }
                    else
                    {
                        //iterator.GetSeed();
                        iterator.All();
                    }
                }));
            }
        }
        private MyCommand showCommand;
        public MyCommand ShowCommand
        {
            get
            {
                return showCommand ?? (showCommand = new MyCommand(obj =>
                {
                    //MessageBox.Show("ShowCommand");
                    LoadData1(Value, Scale);
                }));
            }
        }
    }

    public class TabDataParam: TabVm
    {
        private readonly IDataService _dataService;
        public ObservableCollection<FileViewModel> Files { get; set; }

        public TabDataParam(IDataService data)
            : base("Настройка данных")
        {
            Files = new ObservableCollection<FileViewModel>();
            _dataService = data;
            List<FileArrModel> f = data.GetFileArrs();
            foreach(var i in f)
            {                
                Files.Add(new FileViewModel( i));
            }
            
        }
        private MyCommand _TestCommand1;
        public MyCommand TestCommand1
        {
            get
            {
                return _TestCommand1 ?? (_TestCommand1 = new MyCommand(obj =>
                {
                    MessageBox.Show("Команда TestCommand1 в TabDataParam");
                    MessageBox.Show(Files.Count().ToString());
                }));
            }
        }
    }

    public class ChartData : TabVm
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> NLabels { get; set; }
        public List<string> NLabels1 { get; set; }
        public ChartValues<OhlcPoint> OhlcPoints { get; set; }
        public ChartValues<double> EMALine { get; set; }

        private IteratorModel iterator;
        private readonly IDataService _dataService;
        private ParamDataService paramDataService = new ParamDataService();
        public ObservableCollection<FileViewModel> Files { get; set; }

        private ObservableCollection<string> listComboBoxItems = new ObservableCollection<string>();
        public ObservableCollection<string> ListComboBoxItems
        {
            get { return listComboBoxItems; }
            set { listComboBoxItems = value; /*OnPropertyChanged();*/ }
        }

        string v;
        public string Value
        {
            get => v;
            set
            {
                Set(ref v, value);
            }
        }
        string scale;
        public string Scale
        {
            get => scale;
            set => Set(ref scale, value);            
        }
        private ObservableCollection<string> listScaleItems = new ObservableCollection<string>();
        public ObservableCollection<string> ListScaleItems
        {
            get { return listScaleItems; }
            set { listScaleItems = value; /*OnPropertyChanged();*/ }
        }

        public ChartData(IDataService data)
            : base("График")
        {            
             From = 1;
             To = 3;
            ScaleSer = 1;
            _dataService = data;
            iterator = IteratorModel.GetInstance (data.GetMasterPoints(), data.GetFileArrs());
            NLabels = new List<string>();
            NLabels1 = new List<string>();
            Files = new ObservableCollection<FileViewModel>();
            List<FileArrModel> f = data.GetFileArrs();
            foreach (var i in f)
            {
                if (i.Work)
                {
                    Files.Add(new FileViewModel(i));
                    ListComboBoxItems.Add(i.Tiker);
                }
            }
            listScaleItems.Add("60");
            listScaleItems.Add("D");

            TypeViewChart = paramDataService.GetQuantity("Index", "ChartArea");
            //TypeViewChart = 1;
        }


        #region Обработка комманд
        private MyCommand _NextCommand;
        public MyCommand NextCommand
        {
            get
            {
                return _NextCommand ?? (_NextCommand = new MyCommand(obj =>
                {
                    iterator.Next();
                    //LoadData1(v, scale);
                    //To = 4;
                    CVM1.To = 10; CVM1.From = 1;

                }));
            }
        }
        private MyCommand _Next100Command;
        public MyCommand Next100Command
        {
            get
            {
                return _Next100Command ?? (_Next100Command = new MyCommand(obj =>
                {
                    iterator.NextN(200);

                    //LoadData1(v, scale);

                }));
            }
        }
        private MyCommand _AllCommand;
        public MyCommand AllCommand
        {
            get
            {
                return _AllCommand ?? (_AllCommand = new MyCommand(obj =>
                {
                    iterator.All();                    
                    //LoadData1(v, scale);
                }));
            }
        }
        private MyCommand showCommand;
        public MyCommand ShowCommand
        {
            get
            {
                return showCommand ?? (showCommand = new MyCommand(obj =>
                {                    
                    NewGETChart(Value, Scale);

                    base.Test();
                    //Event2?.Invoke("propertyName");


                    //LoadData1(Value, Scale);
                }));
            }
        }
        #endregion

        #region Старый код для работы с графикой 
        private double _from;
        public double From
        {
            get { return _from; }
            set {Set<double>(() => this.From, ref _from, value);}
        }
        private double _to;
        public double To
        {
            get { return _to; }
            set {Set<double>(() => this.To, ref _to, value);}
        }
        public int ScaleSer
        {
            get { return _scaleSer; }
            set
            {
                Set<int>(() => this.ScaleSer, ref _scaleSer, value);
                To = From + value;
            }
        }
        private int _kolPoint;
        public int KolPoint
            {
            get { return _kolPoint; }
            set
            {

                Set<int>(() => this.KolPoint, ref _kolPoint, value);
            }
            }
        public int IndexChart
        {
            get { return _indexChart; }
            set {Set<int>(() => this.IndexChart, ref _indexChart, value);}
        }
        private void LoadData1(string tiker, string skale)
        {
            OhlcPoints.Clear();
            EMALine.Clear();
            NLabels.Clear();
            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            foreach (PointModel i in iterator.WorkPoints.Find(i => i.Tiker == tiker).Data.Find(i1 => i1.Scale == skale).Points)
            {
                OhlcPoints.Add(new OhlcPoint
                {
                    Close = i.Close,
                    High = i.High,
                    Low = i.Low,
                    Open = i.Open
                });
                NLabels.Add(i.Date.ToString("dd MMM"));
                EMALine.Add(i.IndexPoint[0].Value[0].Value);
                SeriesCollection[0].Values.Add(new OhlcPoint
                {
                    Close = i.Close,
                    High = i.High,
                    Low = i.Low,
                    Open = i.Open
                });
                SeriesCollection[1].Values.Add(i.IndexPoint[0].Value[0].Value);
                //points.Add(i);
            };
            GetScaleAverage(tiker, skale, 50);
        }
        private void GetScaleAverage(string tiker, string skale, int KolPoint)
        {
            EMALine.Clear();
            NLabels.Clear();
            List<PointModel> tmp = iterator.WorkPoints.Find(i => i.Tiker == tiker).Data.Find(i1 => i1.Scale == skale).Points;
            
            int scale1 = tmp.Count / this.KolPoint;
            IndexChart = scale1;
            ScaleSer = scale1;         
            for (int i = 0; i < tmp.Count; i = i + scale1)
            {
                EMALine.Add(tmp[i].Close);
                NLabels.Add(tmp[i].Date.ToString("dd MMM"));
            }                     
        }
        private void GetChart(string tiker, string skale)
        {
            if (tiker == null) return;
            if (scale == null) return;

            OhlcPoints.Clear();
            //EMALine.Clear();
            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            NLabels1.Clear();

            List<PointModel> tmp = iterator.WorkPoints.Find(i => i.Tiker == tiker).Data.Find(i1 => i1.Scale == skale).Points;

            for (int i = Convert.ToInt32(From); i<To*IndexChart;  i++)
            {                
                OhlcPoints.Add(new OhlcPoint
                {
                    Close =  tmp[i].Close,
                    High = tmp[i].High,
                    Low = tmp[i].Low,
                    Open = tmp[i].Open
                });
                NLabels1.Add(tmp[i].Date.ToString("dd MMM"));
                //EMALine.Add(tmp[i].IndexPoint[0].Value[0].Value);
                
                SeriesCollection[0].Values.Add(new OhlcPoint
                {
                    Close = tmp[i].Close,
                    High = tmp[i].High,
                    Low = tmp[i].Low,
                    Open = tmp[i].Open
                });
                SeriesCollection[1].Values.Add(tmp[i].IndexPoint[0].Value[0].Value);               
            }
           // MessageBox.Show("!");
        }

        #endregion

        #region Новый код для работы с графиком 

        private int _scaleSer;        
        private int _indexChart;
        private int TypeViewChart;
        
        private ChartViewModel cvm1;
        public ChartViewModel CVM1
        {
            set
            {
                Set<ChartViewModel>(() => this.CVM1, ref cvm1, value);
            }
            get
            {
                return cvm1;
            }
        }
        private void AddLabelData(string tiker, string skale)
        {
            if (CVM1.Labels != null) { CVM1.Labels.Clear(); }
            if (CVM1.LabelsScale != null) { CVM1.LabelsScale.Clear(); }
            
            CVM1.Labels = iterator.GetArrDate(tiker, skale, CVM1.From, CVM1.To,CVM1.IndexChart);
            CVM1.LabelsScale = iterator.GetScaleArrDate(tiker, skale, 50);
        }
        private void LoadChartData(string tiker, string skale, bool flag)
        {
            CVM1.KolPoint = iterator.GetListPoint(tiker, skale).Count;

            CVM1.SeriesCollection = iterator.GetSeriesCollection(tiker, skale, "0", CVM1.From, CVM1.To,CVM1.IndexChart);
            CVM1.SeriesCollection1 = iterator.GetScaleSeriesCollection(tiker, skale, "0", CVM1.KolSkale);
                        
            if (flag) { CVM1.PropertyChanged += CVM1_PropertyChanged; }            
        }
        private void ClearSeries ()
        {
            CVM1.SeriesCollection.Clear();
            CVM1.SeriesCollection1.Clear();
            if (CVM1.SeriesCollection2 != null) { CVM1.SeriesCollection2.Clear(); }
            if (CVM1.SeriesCollection3 != null) { CVM1.SeriesCollection3.Clear(); }
            CVM1.Labels.Clear();
            CVM1.LabelsScale.Clear();
        }
        private void NewGETChart(string tiker, string skale)
        {
            switch (TypeViewChart)
            {
                case 0:                    
                    CVM1 = new ChartViewModel0();
                    LoadChartData(tiker, skale,true);
                    AddLabelData(tiker, skale);                    
                    break;
                case 1:
                    CVM1 = new ChartViewModel3(iterator);                    
                    LoadChartData(tiker, skale, true);
                    CVM1.SeriesCollection2 = iterator.GetSeriesCollection(tiker, skale, "1", CVM1.From, CVM1.To, CVM1.IndexChart);
                    AddLabelData(tiker, skale);
                    break;
                case 2:
                    CVM1 = new ChartViewModel4(iterator);                    
                    LoadChartData(tiker, skale, true);
                    CVM1.SeriesCollection2 = iterator.GetSeriesCollection(tiker, skale, "1", CVM1.From, CVM1.To, CVM1.IndexChart);
                    CVM1.SeriesCollection3 = iterator.GetSeriesCollection(tiker, skale, "2", CVM1.From, CVM1.To, CVM1.IndexChart);
                    AddLabelData(tiker, skale);
                    break;
            }
        }
        private void GetDataChart(string tiker, string skale)
        {
            switch (TypeViewChart)
            {
                case 0:
                    ClearSeries();
                    LoadChartData(tiker, skale, false);
                    AddLabelData(tiker, skale);
                    break;
                case 1:
                    ClearSeries();
                    LoadChartData(tiker, skale, false);
                    CVM1.SeriesCollection2 = iterator.GetSeriesCollection(tiker, skale, "1", CVM1.From, CVM1.To, CVM1.IndexChart);
                    AddLabelData(tiker, skale);
                    break;
                case 2:
                    ClearSeries();
                    LoadChartData(tiker, skale, false);
                    CVM1.SeriesCollection2 = iterator.GetSeriesCollection(tiker, skale, "1", CVM1.From, CVM1.To, CVM1.IndexChart);
                    CVM1.SeriesCollection3 = iterator.GetSeriesCollection(tiker, skale, "2", CVM1.From, CVM1.To, CVM1.IndexChart);
                    AddLabelData(tiker, skale);
                    break;
            }
        }
        private void CVM1_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "From")
            {
                GetDataChart(Value, Scale);
                // MessageBox.Show("!");
            }
        }
        #endregion
    }

    #region Тестовый класс для создания сетки и создания элементов графика диномически
    public class TestViewModel :TabVm
    {
        private IteratorModel iterator;
        private readonly IDataService _dataService;
        // TODO: попробовать по этой ссылке  https://stackoverflow.com/questions/4493445/wpf-binding-how-to-databind-to-grid
        // TODO: ПОСМОТРЕТЬ СПОСОБ С ПОЛЬЗОАТЕЛЬСКОЙ ПАНЕЛЬЮ https://docs.microsoft.com/ru-ru/dotnet/framework/wpf/controls/panels-overview
        private ObservableCollection<ChartViewModel0> _CVM;
        public ObservableCollection<ChartViewModel0> CVM
        {
            get
            {
                return _CVM ?? (_CVM = new ObservableCollection<ChartViewModel0>());
            }
        }
        private ChartViewModel cvm1;
        public ChartViewModel CVM1
        {
            get{ return cvm1; }
        }

        public TestViewModel(IDataService data)
            :base ("Тестовый элимет")
        {
            ParamDataService paramDataService = new ParamDataService();
            _dataService = data;
            iterator = IteratorModel.GetInstance(data.GetMasterPoints(), data.GetFileArrs());
            iterator.NextN(50);
            int i = paramDataService.GetQuantity("Index", "ChartArea");            
            switch (i)
            {
                case 0:
                    MessageBox.Show("0");
                    cvm1 = new ChartViewModel0
                    {
                        From = 3,
                        SeriesCollection = iterator.GetSeriesCollection("BANE", "60", "0", 1, 30, 2),
                        SeriesCollection1 = iterator.GetScaleSeriesCollection("BANE", "60", "0", 30)
                    };

                    break;
                case 1:
                    cvm1 = new ChartViewModel3(iterator)
                    {
                        From = 3,
                        SeriesCollection = iterator.GetSeriesCollection("BANE", "60", "0", 1, 30, 2),
                        SeriesCollection1 = iterator.GetScaleSeriesCollection("BANE", "60", "0", 30),
                        SeriesCollection2 = iterator.GetSeriesCollection("BANE", "60", "1", 1, 30, 2)
                    };

                    break;
                case 2:
                    cvm1 = new ChartViewModel4(iterator)
                    {
                        From = 3,
                        SeriesCollection = iterator.GetSeriesCollection("BANE", "60", "0", 1, 30, 2),
                        SeriesCollection1 = iterator.GetScaleSeriesCollection("BANE", "60", "0", 30),
                        SeriesCollection2 = iterator.GetSeriesCollection("BANE", "60", "1", 1, 30, 2),
                        SeriesCollection3 = iterator.GetSeriesCollection("BANE", "60", "2", 1, 30, 2)
                    };
                    break;
            }
            //CVM.Add(new ChartViewModel0());
            //CVM.Add(new ChartViewModel0());
            //CVM.Add(new ChartViewModel0());
        }
    }
    #endregion
}
