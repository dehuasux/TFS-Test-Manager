using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TFSCycleQuery
{
    internal class MyExcel
    {
        public string mFilename;
        public Microsoft.Office.Interop.Excel.Application app;
        public Microsoft.Office.Interop.Excel.Workbooks wbs;
        public Microsoft.Office.Interop.Excel.Workbook wb;
        public Microsoft.Office.Interop.Excel.Worksheets wss;
        public Microsoft.Office.Interop.Excel.Worksheet ws;

        public MyExcel()
        {
            //
            // 
            //
        }

        public void Create()
        {
            app = new Microsoft.Office.Interop.Excel.Application();
            //app.Visible = true;
            wbs = app.Workbooks;
            wb = wbs.Add(true);
            ws = (Excel.Worksheet) wb.Sheets[1];
        }

        public void Open(string FileName)
        {
            app = new Microsoft.Office.Interop.Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(FileName);
            //wb = wbs.Open(FileName, 0, true, 5,"", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "t", false, false, 0, true,Type.Missing,Type.Missing);
            //wb = wbs.Open(FileName,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Microsoft.Office.Interop.Excel.XlPlatform.xlWindows,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
            mFilename = FileName;
        }

        public Microsoft.Office.Interop.Excel.Worksheet GetSheet(string SheetName)
        {
            Microsoft.Office.Interop.Excel.Worksheet s =
                (Microsoft.Office.Interop.Excel.Worksheet) wb.Worksheets[SheetName];
            return s;
        }

        public Microsoft.Office.Interop.Excel.Worksheet AddSheet(string SheetName)
        {
            Microsoft.Office.Interop.Excel.Worksheet s =
                (Microsoft.Office.Interop.Excel.Worksheet)
                    wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = SheetName;
            return s;
        }

        public void DelSheet(string SheetName)
        {
            ((Microsoft.Office.Interop.Excel.Worksheet) wb.Worksheets[SheetName]).Delete();
        }

        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(string OldSheetName, string NewSheetName)
        {
            Microsoft.Office.Interop.Excel.Worksheet s =
                (Microsoft.Office.Interop.Excel.Worksheet) wb.Worksheets[OldSheetName];
            s.Name = NewSheetName;
            return s;
        }

        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(Microsoft.Office.Interop.Excel.Worksheet Sheet,
            string NewSheetName)
        {

            Sheet.Name = NewSheetName;

            return Sheet;
        }

        public void SetCellValue(Microsoft.Office.Interop.Excel.Worksheet ws, int x, int y, object value)
        {
            ws.Cells[x, y] = value;
        }

        public void SetCellValue(string ws, int x, int y, object value)
        {

            GetSheet(ws).Cells[x, y] = value;
        }

        public void SetCellProperty(Microsoft.Office.Interop.Excel.Worksheet ws, int Startx, int Starty, int Endx,
            int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color,
            Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        {
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }

        public void SetCellProperty(string wsn, int Startx, int Starty, int Endx, int Endy, int size, string name,
            Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        {

            Microsoft.Office.Interop.Excel.Worksheet ws = GetSheet(wsn);
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;

            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }


        public void UniteCells(Microsoft.Office.Interop.Excel.Worksheet ws, int x1, int y1, int x2, int y2)
        {
            ws.get_Range(ws.Cells[x1, y1], ws.Cells[x2, y2]).Merge(Type.Missing);
        }

        public void UniteCells(string ws, int x1, int y1, int x2, int y2)
        {
            GetSheet(ws).get_Range(GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]).Merge(Type.Missing);

        }


        public void InsertTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(ws).Cells[startX + i, j + startY] = dt.Rows[i][j].ToString();

                }

            }

        }

        public void InsertTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX,
            int startY)
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[startX + i, j + startY] = dt.Rows[i][j];

                }

            }

        }


        public void AddTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    GetSheet(ws).Cells[i + startX, j + startY] = dt.Rows[i][j];

                }

            }

        }

        public void AddTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX,
            int startY)
        {


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[i + startX, j + startY] = dt.Rows[i][j];

                }
            }

        }

/*        public void InsertPictures(string Filename, string ws)
        {
            GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        }

        //public void InsertPictures(string Filename, string ws, int Height, int Width)
        //{
        //    GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Height = Height;
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Width = Width;
        //}
        //public void InsertPictures(string Filename, string ws, int left, int top, int Height, int Width)
        //{

        //    GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).IncrementLeft(left);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).IncrementTop(top);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Height = Height;
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Width = Width;
        //}*/

        public void InsertActiveChart(Microsoft.Office.Interop.Excel.XlChartType ChartType, string ws, int DataSourcesX1,
            int DataSourcesY1, int DataSourcesX2, int DataSourcesY2,
            Microsoft.Office.Interop.Excel.XlRowCol ChartDataType)
        {
            ChartDataType = Microsoft.Office.Interop.Excel.XlRowCol.xlColumns;
            wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                wb.ActiveChart.ChartType = ChartType;
                wb.ActiveChart.SetSourceData(
                    GetSheet(ws)
                        .get_Range(GetSheet(ws).Cells[DataSourcesX1, DataSourcesY1],
                            GetSheet(ws).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
                wb.ActiveChart.Location(Microsoft.Office.Interop.Excel.XlChartLocation.xlLocationAsObject, ws);
            }
        }

        public bool Save()
        {
            if (mFilename == "")
            {
                return false;
            }
            else
            {
                try
                {
                    wb.Save();
                    return true;
                }

                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool SaveAs(object FileName)
            //文档另存为
        {
            try
            {
                wb.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing);
                return true;

            }

            catch (Exception ex)
            {
                return false;

            }
        }

        public void Close()
        {
            //wb.Save();
            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            wbs.Close();
            app.Quit();
            wb = null;
            wbs = null;
            app = null;
            GC.Collect();
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public void KillSpecialExcel()
        {
            try
            {
                if (app != null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId(new IntPtr(app.Hwnd), out lpdwProcessId);
                    System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete Excel Process Error:" + ex.Message);
            }
        }
    }
}
