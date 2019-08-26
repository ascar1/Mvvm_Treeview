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
    public abstract class TabVm : INotifyPropertyChanged
    {
        public string Header { get; }

        public TabVm(string header)
        {
            Header = header;
        }

        private MyCommand _TestCommand;
        public MyCommand TestCommand
        {
            get
            {
                return _TestCommand ?? (_TestCommand = new MyCommand(obj =>
                {
                    MessageBox.Show("Команда " + " Button !!! ");
                }));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Tab1Vm : TabVm
    {
        public Tab1Vm()
            : base("Tab 1")
        {
        }
    }

    public class Tab2Vm : TabVm
    {
        public Tab2Vm()
            : base("Tab 2")
        {
        }
    }
}
