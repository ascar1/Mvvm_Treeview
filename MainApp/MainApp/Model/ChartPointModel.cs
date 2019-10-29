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
        public DateTime SDate { get; set; }
        public DateTime EDate { get; set; }
        public List<DateModel> Data { get; set; }         
    }
    public class WorkPointModel
    {
        public string Tiker { get; set; }
        public DateTime SDate { get; set; }
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
        public List<Index> IndexPoint { get; set; }
        public List<AnalysisResult> AnalysisResults { get; set; }
        public PointModel()
        {
            IndexPoint = new List<Index>();
            AnalysisResults = new List<AnalysisResult>();
        }
    }
    public class Index
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<IndexPoint> Value { get; set; }
        public Index()
        {
            Value = new List<IndexPoint>();
        }
    }
    public class IndexPoint
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
    public class FileArrModel
    {
        public string Tiker { get; set; }
        public string Fname { get; set; }
        public DateTime SDate { get; set; }
        public DateTime EDate { get; set; }
        public bool Work { get; set; }
        public FileArrModel()
        {

        }
        public FileArrModel (MasterPointModel master, bool flag)
        {
            Tiker = master.Tiker;
            Fname = master.FName;
            SDate = master.SDate;
            EDate = master.EDate;
            Work = flag;
        }
    }
    public class AnalysisResult
    {
        public AnalysisResult(string name)
        {
            this.Name = name;
            ResultArr = new List<ResultArr>();
        }
        public string Name { get; set; }
        public string Result { get; set; }
        public List<ResultArr> ResultArr { get; set; }
    }
    public class ResultArr
    {
        public string Name { get; set; }
        public double Val { get; set; }
        public string ValStr { get; set; }
    }
}
