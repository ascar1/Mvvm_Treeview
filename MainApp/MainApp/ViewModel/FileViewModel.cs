using MainApp.Command;
using MainApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainApp.ViewModel
{
    public class FileViewModel
    {
        private readonly FileArrModel _fileArr;
        public FileViewModel(FileArrModel fArr)
        {
            _fileArr = fArr;
        }
        public string Tiker 
        {
            get => _fileArr.Tiker;
            set => _fileArr.Tiker = value;
        }
        public string Fname
        {
            get => _fileArr.Fname;
            set => _fileArr.Fname = value;
        }
        public DateTime SDate
        {
            get => _fileArr.SDate;
            set => _fileArr.SDate = value;
        }
        public DateTime EDate
        {
            get => _fileArr.EDate;
            set => _fileArr.EDate = value;
        }
        public bool Work
        {
            get => _fileArr.Work;
            set => _fileArr.Work = value;
        }

        private MyCommand _TestCommand1;
        public MyCommand TestCommand1
        {
            get
            {
                return _TestCommand1 ?? (_TestCommand1 = new MyCommand(obj =>
                {
                    MessageBox.Show("123");
                }));
            }
        }
    }
}
