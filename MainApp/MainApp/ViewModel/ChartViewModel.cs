using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using GalaSoft.MvvmLight;
using MainApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MainApp.ViewModel
{
    public abstract class ChartViewModel: ViewModelBase, INotifyPropertyChanged
    {
        private SeriesCollection _SeriesCollection;
        public SeriesCollection SeriesCollection
        {
            get
            {
                return _SeriesCollection;
            }
            set
            {
                _SeriesCollection = value;
                //UpdateIndex();
                NotifyPropertyChanged();
            }
        }
        private SeriesCollection _SeriesCollection1;
        public SeriesCollection SeriesCollection1
        {
            get
            {
                return _SeriesCollection1;
            } 
            set
            {
                _SeriesCollection1 = value;                
                NotifyPropertyChanged();
            }
        }
        private SeriesCollection _SeriesCollection2;
        public SeriesCollection SeriesCollection2
        {   get { return _SeriesCollection2; }
            set
            {
                _SeriesCollection2 = value;
                NotifyPropertyChanged();
            }
        }
        private SeriesCollection _SeriesCollection3;
        public SeriesCollection SeriesCollection3
        {
            get { return _SeriesCollection3; }
            set
            {
                _SeriesCollection3 = value;
                NotifyPropertyChanged();
            }
        }

        public IteratorModel Iterator;
        private int _From;
        public int From
        {
            get
            {
                return _From;
            }
            set
            {
                _From = value;                
                NotifyPropertyChanged();               
            }
        }
        private int _To;
        public int To
        {
            get
            {
                return _To;
            }
            set
            {
                _To = value;
                NotifyPropertyChanged();
            }
        }

        // Длина маштабируемой серии 
        private int _scaleSer;
        public int ScaleSer
        {
            get { return _scaleSer; }
            set
            {
                _scaleSer = value;
                NotifyPropertyChanged();
            }
        }
        // Количество всего знаков 
        private int _kolPoint;
        public int KolPoint
        {
            get { return _kolPoint; }
            set
            {
                _kolPoint = value;
                UpdateIndex();
                NotifyPropertyChanged();
            }
        }

        private int _kolScale;
        public int KolSkale
        {
            get { return _kolScale; }
            set
            {
                _kolScale = value;
                NotifyPropertyChanged();
            }

        }

        // Коофициент маштабирования 
        private int _indexChart;
        public int IndexChart
        {
            get
            {
                if (_indexChart == 0) { return 1; }
                return _indexChart;
            }
            set
            {
                _indexChart = value;
                NotifyPropertyChanged();
            }
        }
        

        private List<string> _labels;
        public List<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                NotifyPropertyChanged();
            }
        }
        private List<string> _labelsScale;
        public List<string> LabelsScale
        {
            get { return _labelsScale; }
            set
            {
                _labelsScale = value;
                NotifyPropertyChanged();
            }
        }

        private void UpdateIndex()
        {
            IndexChart = KolPoint / KolSkale;
        }
        public abstract void Update();

        public abstract void Update1();

#pragma warning disable CS0108 // "ChartViewModel.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // "ChartViewModel.PropertyChanged" скрывает наследуемый член "ObservableObject.PropertyChanged". Если скрытие было намеренным, используйте ключевое слово new.

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
    public class ChartViewModel0: ChartViewModel
    {
        
        public override void Update()
        {
            MessageBox.Show("class ChartViewModel0");
        }

        public override void Update1()
        {
            MessageBox.Show("class ChartViewModel0");
            //base.Update1();
        }


        public ChartViewModel0()
        {
            //Iterator = _iterator;
            //iterator = new IteratorModel(data.GetMasterPoints(), data.GetFileArrs());
            From = 1;
            To = 10;
            Labels = new List<string>()
            {
                DateTime.Now.ToString("dd MMM"),
                DateTime.Now.AddDays(1).ToString("dd MMM"),
                DateTime.Now.AddDays(2).ToString("dd MMM"),
                DateTime.Now.AddDays(3).ToString("dd MMM"),
                DateTime.Now.AddDays(4).ToString("dd MMM"),
            };
            SeriesCollection1 = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { 30, 32, 35, 30, 28 },
                    Fill= Brushes.LightGray,
                    Stroke=Brushes.Gray,
                    PointGeometry = Geometry.Empty,
                    //AreaLimit=0
                }
            };
            KolSkale = 50;
        }
    }
    public class ChartViewModel3: ChartViewModel
    {
        
        public override void Update()
        {
            MessageBox.Show("class ChartViewModel3");
        }

        public override void Update1()
        {
            throw new NotImplementedException();
        }

        public ChartViewModel3(IteratorModel _iterator)
        {
            From = 1;
            To = 10;
            KolSkale = 50;
             
            Iterator = _iterator;
            SeriesCollection = new SeriesCollection
            {
                new OhlcSeries()
                {
                    Values = new ChartValues<OhlcPoint>
                    {
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33)
                    }
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {30, 32, 35, 30, 28},
                    Fill = Brushes.Transparent
                }
            };

            Labels = new List<string>()
            {
                DateTime.Now.ToString("dd MMM"),
                DateTime.Now.AddDays(1).ToString("dd MMM"),
                DateTime.Now.AddDays(2).ToString("dd MMM"),
                DateTime.Now.AddDays(3).ToString("dd MMM"),
                DateTime.Now.AddDays(4).ToString("dd MMM"),
            };
        }
    }
    public class ChartViewModel4: ChartViewModel
    {
        public override void Update()
        {
            MessageBox.Show("class ChartViewModel4");
        }
        public ChartViewModel4(IteratorModel _iterator)
        {
            From = 1;
            To = 10;
            KolSkale = 50;

            Iterator = _iterator;

            SeriesCollection = new SeriesCollection
            {
                new OhlcSeries()
                {
                    Values = new ChartValues<OhlcPoint>
                    {
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33)
                    }
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {30, 32, 35, 30, 28},
                    Fill = Brushes.Transparent
                }
            };

            SeriesCollection2 = new SeriesCollection
            {
                new LineSeries
                {
                    Stroke = Brushes.Gray,
                    Values = new ChartValues<double> {30, 32, 35, 30, 28},
                    Fill = Brushes.Transparent
                }
            };

            SeriesCollection3 = new SeriesCollection
            {
                new LineSeries
                {
                    Stroke = Brushes.Green,
                    Values = new ChartValues<double> {30, 32, 35, 30, 28},
                    Fill = Brushes.Transparent
                }
            };
            SeriesCollection1 = new SeriesCollection
            {
                new LineSeries
                {
                    Stroke = Brushes.Black,
                    Values = new ChartValues<double> {30, 32, 35, 30, 28},
                    Fill = Brushes.Transparent
                }
            };

            Labels = new List<string>()
            {
                DateTime.Now.ToString("dd MMM"),
                DateTime.Now.AddDays(1).ToString("dd MMM"),
                DateTime.Now.AddDays(2).ToString("dd MMM"),
                DateTime.Now.AddDays(3).ToString("dd MMM"),
                DateTime.Now.AddDays(4).ToString("dd MMM"),
            };
        }
        


        public override void Update1()
        {
           /* SeriesCollection[0].Values.Add(new OhlcPoint(32, 35, 30, 32));
            SeriesCollection[0].Values.Add(new OhlcPoint(32, 35, 30, 32));
            SeriesCollection[0].Values.Add(new OhlcPoint(32, 35, 30, 32));
            SeriesCollection[0].Values.Add(new OhlcPoint(32, 35, 30, 32));
            double i = 30;
            SeriesCollection[1].Values.Add(i);
            SeriesCollection[1].Values.Add(i);
            SeriesCollection[1].Values.Add(i);
            SeriesCollection[1].Values.Add(i);
            SeriesCollection1[0].Values.Add(i);
            SeriesCollection1[0].Values.Add(i);
            SeriesCollection1[0].Values.Add(i);
            SeriesCollection1[0].Values.Add(i);
            SeriesCollection2[0].Values.Add(i);
            SeriesCollection2[0].Values.Add(i);
            SeriesCollection2[0].Values.Add(i);
            SeriesCollection2[0].Values.Add(i);
            SeriesCollection3[0].Values.Add(i);
            SeriesCollection3[0].Values.Add(i);
            SeriesCollection3[0].Values.Add(i);
            SeriesCollection3[0].Values.Add(i);*/
        }
    }
}
