using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Model
{
    public class MasterPointModel
    {
        public string Tiker { get; set; }
        public string FName { get; set; }
        public DateTime sDate { get; set; }
        public DateTime eDate { get; set; }
        public List<DateModel> Data { get; set; }         
    }
    public class WorkPointModel
    {
        public string Tiker { get; set; }
        public DateTime sDate { get; set; }
        public DateTime CurrDate { get; set;}
        public List<DateModel> Data { get; set; }
    }
    public class DateModel
    {
        public string Scale { get; set; }
        public List<PointModel> Points { get; set; }
    }
    public class PointModel
    {
        public DateTime Date { get; set; }
        public Double Open { get; set; }
        public Double High { get; set; }
        public Double Low { get; set; }
        public Double Close { get; set; }
        public Double Vol { get; set; }
        public Double OpenInt { get; set; }
        public List<IndexModel> IndexPoint { get; set; }

    }
    public class IndexModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
    }

    public class FileArrModel
    {
        public string Tiker { get; set; }
        public string Fname { get; set; }
        public DateTime sDate { get; set; }
        public DateTime eDate { get; set; }
        public bool Work { get; set; }
        public FileArrModel()
        {

        }
        public FileArrModel (MasterPointModel master, bool flag)
        {
            Tiker = master.Tiker;
            Fname = master.FName;
            sDate = master.sDate;
            eDate = master.eDate;
            Work = flag;
        }
    }
}
