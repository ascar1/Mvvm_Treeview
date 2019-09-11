using MainApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.ViewModel
{
    public class FileViewModel
    {
        private readonly FileArr _fileArr;
        public FileViewModel(FileArr fArr)
        {
            _fileArr = fArr;
        }
        public string Tiker 
        {
            get => _fileArr.Tiker;
            set => _fileArr.Tiker = value;
        }
        public DateTime sDate
        {
            get => _fileArr.sDate;
            set => _fileArr.sDate = value;
        }
        public DateTime eDate
        {
            get => _fileArr.eDate;
            set => _fileArr.eDate = value;
        }
        public bool Work
        {
            get => _fileArr.Work;
            set => _fileArr.Work = value;
        }

    }
}
