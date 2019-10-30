using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MainApp.Command;
using MainApp.Model;
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
            
            //this.UserRoleColumns.Add(new DataGridTextColumn { Header = "First Name", Binding = new Binding("col") });
            //this.UserRoleColumns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("col1") });
            LoadData();
        }
        private void HandleMessage(NotificationMessage notificationMessage)
        {
            
            if (notificationMessage.Notification == "Show")
            {
                MessageBox.Show("! " + notificationMessage.Notification.ToString());
                
                
            }
        }

        public ObservableCollection<DataGridColumn> DataGridColumns { get; private set; }

        private ObservableCollection<string> listComboBoxItems = new ObservableCollection<string>();
        public ObservableCollection<string> ListComboBoxItems
        {
            get { return listComboBoxItems; }
            set { listComboBoxItems = value; /*OnPropertyChanged();*/ }
        }
        private void GetListComboBoxItems()
        {
            //iterator.WorkPoints[0].
            
        }
        public DataSet Store { get; set; }
        public DataTable Table { get; set; }
        public void LoadData()
        {
            GetTestArrData();
            InitializeRolesColumns();
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

            DataRow row = Table.NewRow();
            row.ItemArray = new object[] { null, "Война и мир", 200 };
            Table.Rows.Add(row); // добавляем первую строку
            Table.Rows.Add(new object[] { null, "Отцы и дети", 170 }); // добавляем вторую строку
        }
        private void InitializeRolesColumns()
        {            
            foreach (DataColumn role in Store.Tables[0].Columns)
            {
                this.AddColumn(role);               
            }
        }
        private void AddColumn(DataColumn role /*UserRoleDataSet.RoleRow role*/)
        {
            //var resourceDictionary = ResourceDictionaryResolver.GetResourceDictionary("Styles.xaml");
            //var userRoleValueConverter = resourceDictionary["UserRoleValueConverter"] as IValueConverter;
            //var checkBoxColumnStyle = resourceDictionary["CheckBoxColumnStyle"] as Style;
            var binding = new Binding(role.ColumnName);
            //{
            //    //Converter = userRoleValueConverter,
            //    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridCell), 1),
            //    Path = new PropertyPath("."),
            //    Mode = BindingMode.TwoWay
            //};

            var dataGridCheckBoxColumn = new DataGridTextColumn
            {
                Header = role.Caption,
                Binding = binding,
                //IsThreeState = false,
                CanUserSort = false,
                
            };

            //ObjectTag.SetTag(dataGridCheckBoxColumn, role);
            this.DataGridColumns.Add(dataGridCheckBoxColumn);
        }
        private void GetListAnalysis()
        {

        }
        public DataView Rows
        {
            get
            {
                return Store.Tables[0].DefaultView;
            }
        }
        #region Обработка команд
        private MyCommand _TestCommand;
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
        private MyCommand _ShowCommand;
        public MyCommand ShowCommand
        {
            get
            {
                return _ShowCommand ?? (_ShowCommand = new MyCommand(obj =>
                {
                    MessageBox.Show("Команда ShowCommand в TabAnalizResult");
                }));
            }
        }
        #endregion


    }

    public class TabOrder : TabVM2
    {
        public TabOrder()
            : base("Список выставленных ордеров")
        {
        }
    }

    public class TabDeal : TabVM2
    {
        public TabDeal()
            : base("Список сделок ")
        {
        }
    }

}
