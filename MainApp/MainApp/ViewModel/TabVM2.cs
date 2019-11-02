using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MainApp.Command;
using MainApp.Model;
using MainApp.Supporting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MainApp.ViewModel
{
    public class TMPList
    {
        public string col { get; set; }
        public string col1 { get; set; }
    }


    public abstract class TabVM2 : ViewModelBase, INotifyPropertyChanged
    {
        public string Header { get; set; }
        delegate void GetName();
        public TabVM2(string header)
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
                    MessageBox.Show("Команда TestCommand в abstract class TabVm");
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
                    Event1.Invoke(this, new PropertyChangedEventArgs("propertyName"));
                    //Event2.Invoke(this, new PropertyChangedEventArgs("propertyName"));
                }));
            }
        }

        #endregion
        // public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler Event1;
        //public event PropertyChangedEventHandler Event2;
    }



    public class TabAnalizResult : TabVM2
    {
        private string Tiker;
        private string Scale;


        public TabAnalizResult(IDataService data)
            : base("Результаты анализа")
        {
            Messenger.Default.Register<NotificationMessage>(
                this,
                ChartData.Token,
                HandleMessage);
            Messenger.Default.Register<PropertyChangedMessage<string>>(
                this,
                message => {
                    
                    Debug.WriteLine(message.PropertyName.ToString());
                    Debug.WriteLine(message.OldValue + " --> " + message.NewValue);

                    string str = message.PropertyName;
                    switch (str)
                    {
                        case "Tiker":
                            Tiker = message.NewValue;
                            break;
                        case "Scale":
                            Scale = message.NewValue;
                            break;
                    }
                });
                
            DataGridColumns = new ObservableCollection<DataGridColumn>();
            Rows = null;
            //this.UserRoleColumns.Add(new DataGridTextColumn { Header = "First Name", Binding = new Binding("col") });
            //this.UserRoleColumns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("col1") });
            //LoadData();
            
        }

        private string _SelectAvalysis;
        public string SelectAvalysis
        {
            set
            {
                _SelectAvalysis = value;
            }
            get
            {
                return _SelectAvalysis;
            }
        }
        private void HandleMessage(NotificationMessage notificationMessage)
        {
            
            if (notificationMessage.Notification == "Show")
            {
                MessageBox.Show("! " + notificationMessage.Notification.ToString());
                GetListComboBoxItems();
                LoadData("");
                
                Rows = Store.Tables[0].DefaultView;
            }
        }
        private ObservableCollection<DataGridColumn> _DataGridColumns;
        public ObservableCollection<DataGridColumn> DataGridColumns
        {
            get
            {
                return _DataGridColumns;
            }
            set
            {
                Set(ref _DataGridColumns, value);
                //_DataGridColumns = value;
            }
        }

        private ObservableCollection<string> listComboBoxItems = new ObservableCollection<string>();
        public ObservableCollection<string> ListComboBoxItems
        {
            get { return listComboBoxItems; }
            set { listComboBoxItems = value; /*OnPropertyChanged();*/ }
        }
        private void GetListComboBoxItems()
        {
            IteratorModel iterator = IteratorModel.GetInstance(null, null);
            List<PointModel> points = iterator.GetListPoint(Tiker, Scale);            
            
            foreach(var tmp in points.Last().AnalysisResults)
            {
                ListComboBoxItems.Add(tmp.Name);
            }
            
        }
        private DataSet _Store;
        public DataSet Store
        {
            get
            {
                return _Store;
            }
            set
            {
                _Store = value;
            }
        }
        private DataTable _Table;
        public DataTable Table
        {
            get
            {
                return _Table;
            }
            set
            {
                _Table = value;
            }
        }
        public void LoadData(string Name)
        {
            if (Name != "")
            {
                GetArrData(Name);
            }
            else
            {
                GetTestArrData();
            }
            InitializeRolesColumns();
        }
        private void GetTable(List<string> Names)
        {
            Store = new DataSet("BookStore");
            Table = new DataTable("Books");

            Store.Clear();
            // добавляем таблицу в dataset
            Store.Tables.Add(Table);

            // создаем столбцы для таблицы
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true; // столбец будет иметь уникальное значение
            idColumn.AllowDBNull = false; // не может принимать null
            idColumn.AutoIncrement = true; // будет автоинкрементироваться
            idColumn.AutoIncrementSeed = 1; // начальное значение
            idColumn.AutoIncrementStep = 1; // приращении при добавлении новой строки
            
            DataColumn DateTimeColumn = new DataColumn("Data", Type.GetType("System.DateTime"));
            Table.Columns.Add(idColumn);
            Table.Columns.Add(DateTimeColumn);
            foreach (var tmp in Names)
            {
                DataColumn Column = new DataColumn(tmp, Type.GetType("System.String"));
                Table.Columns.Add(Column);
            }
            // определяем первичный ключ таблицы books
            Table.PrimaryKey = new DataColumn[] { Table.Columns["Id"] };
        }
        private void GetRows(DateTime date, List<ResultArr> result)
        {
            DataRow row = Table.NewRow();
            // row.ItemArray = new object[] { null, "Война и мир", 200 };
            List<object> list = new List<object>();
            list.Add(null);
            list.Add(date);
            foreach (var tmp in result)
            {
                list.Add(tmp.ValStr);
            }          
            row.ItemArray = list.ToArray();
            Table.Rows.Add(row); // добавляем первую строку            

        }
        private void GetArrData(string name)
        {
            IteratorModel iterator = IteratorModel.GetInstance(null, null);
            List<PointModel> points = iterator.GetListPoint(Tiker, Scale);
            List<string> items = new List<string>();
            List<ResultArr> resultArrs = new List<ResultArr>();

            foreach (var item in  points.Last().AnalysisResults.Find(i => i.Name == name).ResultArr)
            {
                items.Add(item.Name);                
            }
            GetTable(items);
            foreach (var tmp in points)
            {                
                var tmpItem = tmp.AnalysisResults.Find(i => i.Name == name);
                if (tmpItem != null)
                {
                    resultArrs.Clear();
                    foreach(var item in tmpItem.ResultArr)
                    {
                        resultArrs.Add(item);
                    }
                    GetRows(tmp.Date, resultArrs);
                }
            }  
            
        }
        private void GetTestArrData()
        {
            Store = new DataSet("BookStore");
            Table = new DataTable("Books");

            // добавляем таблицу в dataset
            Store.Tables.Add(Table);

            // создаем столбцы для таблицы Books
            DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            idColumn.Unique = true; // столбец будет иметь уникальное значение
            idColumn.AllowDBNull = false; // не может принимать null
            idColumn.AutoIncrement = true; // будет автоинкрементироваться
            idColumn.AutoIncrementSeed = 1; // начальное значение
            idColumn.AutoIncrementStep = 1; // приращении при добавлении новой строки

            DataColumn nameColumn = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn priceColumn = new DataColumn("Price", Type.GetType("System.Decimal"));
            priceColumn.DefaultValue = 100; // значение по умолчанию
            DataColumn discountColumn = new DataColumn("Discount", Type.GetType("System.Decimal"));
            discountColumn.Expression = "Price * 0.2";

            Table.Columns.Add(idColumn);
            Table.Columns.Add(nameColumn);
            Table.Columns.Add(priceColumn);
            Table.Columns.Add(discountColumn);
            // определяем первичный ключ таблицы books
            Table.PrimaryKey = new DataColumn[] { Table.Columns["Id"] };

            // Заполняем данные 
            DataRow row = Table.NewRow();
           // row.ItemArray = new object[] { null, "Война и мир", 200 };
            List<object> list = new List<object>();
            list.Add(null);
            list.Add("Война и мир");
            list.Add(200);
            row.ItemArray = list.ToArray();

            Table.Rows.Add(row); // добавляем первую строку
            Table.Rows.Add(new object[] { null, "Отцы и дети", 170 }); // добавляем вторую строку
            
            
        }
        private void InitializeRolesColumns()
        {
            int i = DataGridColumns.Count - 1;
            while (DataGridColumns.Count !=0)
            {
                DataGridColumns.RemoveAt(i);
                i--;
            }

            foreach (DataColumn role in Store.Tables[0].Columns)
            {
                this.AddColumn(role);               
            }
        }
        private void AddColumn(DataColumn role /*UserRoleDataSet.RoleRow role*/)
        {
            var binding = new Binding(role.ColumnName);
            var dataGridCheckBoxColumn = new DataGridTextColumn
            {
                Header = role.Caption,
                Binding = binding,
                CanUserSort = false,
                
            };
            DataGridColumns.Add(dataGridCheckBoxColumn);           
        }
        private DataView _Rows;
        public DataView Rows
        {
            set
            {
                Set(ref _Rows, value);
            }
            get
            {                

                return _Rows;
            }
        }
        #region Обработка команд        
        public MyCommand TestCommand
        {
            get
            {
                return _TestCommand ?? (_TestCommand = new MyCommand(obj =>
                {
                    MessageBox.Show("Команда TestCommand в TabAnalizResult");
                }));
            }
        }
        private MyCommand _TestCommand;
        public MyCommand ShowCommand
        {
            get
            {
                return _ShowCommand ?? (_ShowCommand = new MyCommand(obj =>
                {                                        
                    LoadData(SelectAvalysis);
                    Rows = Store.Tables[0].DefaultView;
                }));
            }
        }
        private MyCommand _ShowCommand;
        public MyCommand ToExcelCommand
        {
            get
            {
                return toExcelCommand ?? (toExcelCommand = new MyCommand(obj =>
                {
                    MessageBox.Show("To Execel Command " + Rows.ToString());
                    
                    ToExcel excel = new ToExcel(Rows);
                    excel.UploadToExcel();
                }));
            }            
        }
        private MyCommand toExcelCommand;
        #endregion
    }

    public class TabOrder : TabVM2
    {
        public TabOrder(IDataService data)
            : base("Список выставленных ордеров")
        {
        }
    }

    public class TabDeal : TabVM2
    {
        public TabDeal(IDataService data)
            : base("Список сделок ")
        {
        }
    }

}
