using MainApp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MainApp.Supporting
{
    /// <summary>
    /// Класс для экспорта таблицы в Excel
    /// </summary>
    class ToExcel
    {
        private Excel.Application app = null;
        private Excel.Workbook workbook = null;
        private Excel.Worksheet worksheet = null;

        private DataView Data;
        private List<WorkPointModel> workPoints;
        public ToExcel(DataView data) 
        {
            Data = data;
        }
        public ToExcel(List<WorkPointModel> WorkPoints)
        {
            workPoints = WorkPoints;
        }

        public void UploadToExcel1()
        {
            CreateExcel();
            foreach (var tmp in workPoints)
            {
                
                foreach(var item in tmp.Data.Find(i=>i.Scale=="D").Points)
                {
                    int j = 1;
                    if (item.AnalysisResults.Count() != 0)
                    {
                        
                        worksheet.Cells[row, j] = tmp.Tiker.ToString();
                        j++;
                        worksheet.Cells[row, j] = item.Date.ToString();
                        j++;
                        
                        foreach (var item1 in item.AnalysisResults[0].ResultArr)
                        {
                            worksheet.Cells[row, j] = item1.ValStr.ToString();
                            j++;
                        }
                        row++;
                    }
                }
            }
            //app.Visible = true;
            //app.Interactive = true;
        }

        public void UploadToExcel()
        {
            CreateExcel();
            AddHead();
            //AddBody();
            app.Visible = true;
            //app.Interactive = true;
        }
        private void CreateExcel()
        {
            app = new Excel.Application();
            app.Interactive = false;
            app.Visible = false;
            workbook = app.Workbooks.Add(1);
            worksheet = (Excel.Worksheet)workbook.Sheets[1];
        }
        private void RangeCell(string Cell1, string Cell2)
        {
            Excel.Range _excelCells1 = (Excel.Range)worksheet.get_Range(Cell1, Cell2).Cells;
            _excelCells1.Merge(Type.Missing);
        }
        private int row = 1;
        private void AddHead()
        {
            Excel._Worksheet worksheet = app.ActiveSheet;
            //RangeCell("A1", "A3");
            int j = 1;
            //Data.Table.ToString();
            foreach(var tmp in Data.Table.Columns)
            {                
                worksheet.Cells[row, j] = tmp.ToString();
                 j++;
            }
            row++;


            #region Шапка отчета
            /*
            worksheet.Cells[1, 1] = "Номер счета второго порядка";

            worksheet.Cells[1, 2] = "Входящие остатки";
            worksheet.Cells[1, 5] = "Обороты за отчетный период";
            worksheet.Cells[1, 11] = "Исходящие остатки";

            worksheet.Cells[2, 5] = "по дебету";
            worksheet.Cells[2, 8] = "по кредиту";

            worksheet.Cells[3, 2] = "в рублях";
            worksheet.Cells[3, 3] = "ин.вал., драг. металлы";
            worksheet.Cells[3, 4] = "итого";

            worksheet.Cells[3, 5] = "в рублях";
            worksheet.Cells[3, 6] = "ин.вал., драг. металлы";
            worksheet.Cells[3, 7] = "итого";

            worksheet.Cells[3, 8] = "в рублях";
            worksheet.Cells[3, 9] = "ин.вал., драг. металлы";
            worksheet.Cells[3, 10] = "итого";

            worksheet.Cells[3, 11] = "в рублях";
            worksheet.Cells[3, 12] = "ин.вал., драг. металлы";
            worksheet.Cells[3, 13] = "итого";

            worksheet.Cells[4, "A"] = "1";
            worksheet.Cells[4, "B"] = "2";
            worksheet.Cells[4, "C"] = "3";
            worksheet.Cells[4, "D"] = "4";
            worksheet.Cells[4, "E"] = "5";
            worksheet.Cells[4, "F"] = "6";
            worksheet.Cells[4, "G"] = "7";
            worksheet.Cells[4, "H"] = "8";
            worksheet.Cells[4, "I"] = "9";
            worksheet.Cells[4, "J"] = "10";
            worksheet.Cells[4, "K"] = "11";
            worksheet.Cells[4, "L"] = "12";
            worksheet.Cells[4, "M"] = "13";
            */
            #endregion

        }
        private void AddBody()
        {
            int j;
            for (int i = 0; i < Data.Table.Rows.Count; i++)
            {
                j = 1;
                foreach (var item in Data.Table.Rows[i].ItemArray)
                {                    
                    worksheet.Cells[row, j] = item.ToString();
                    j++;                    
                }
                row++;
            }
        }

    }
}
