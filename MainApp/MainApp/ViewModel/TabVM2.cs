using GalaSoft.MvvmLight;
using MainApp.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainApp.ViewModel
{
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
                }));
            }
        }

        #endregion
        // public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler Event1;
    }

    public class TabAnalizResult : TabVM2
    {
        public TabAnalizResult()
            : base("Результаты анализа")
        {
        }
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
