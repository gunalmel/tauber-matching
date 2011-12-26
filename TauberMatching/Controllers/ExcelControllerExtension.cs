using System;
using System.IO;
using System.IO.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauberMatching.Controllers.Results; 
using DocumentFormat.OpenXml; 
using DocumentFormat.OpenXml.Packaging; 
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Extensions;
using System.Collections; 

namespace TauberMatching.Controllers
{
    /// <summary> 
    /// Excel controller extensions class. 
    /// </summary> 
    public static class ExcelControllerExtensions
    {
        /// <summary> 
        /// Controller Extension: Returns an Excel
        /// result constructor for returning values from rows. 
        /// </summary> 
        /// <param name="controller">This controller.</param> 
        /// <param name="fileName">Excel file name.</param> 
        /// <param name="excelWorkSheetName">Excel worksheet name: 
        /// default: sheet1.</param> 
        /// <param name="rows">Excel row values.</param> 
        /// <returns>Action result.</returns> 
        public static ActionResult Excel(this Controller controller,
               string fileName, string excelWorkSheetName, IList rows)
        {
            return new ExcelResult(fileName, excelWorkSheetName, rows);
        }

        /// <summary> 
        /// Controller Extension: Excel result constructor
        /// for returning values from rows and headers. 
        /// </summary> 
        /// <param name="controller">This controller.</param> 
        /// <param name="fileName">Excel file name.</param> 
        /// <param name="excelWorkSheetName">
        /// Excel worksheet name: default: sheet1.</param> 
        /// <param name="rows">Excel row values.</param> 
        /// <param name="headers">Excel header values.</param> 
        /// <returns>Action result.</returns> 
        public static ActionResult Excel(this Controller controller,
               string fileName, string excelWorkSheetName,
               IList rows, string[] headers)
        {
            return new ExcelResult(fileName, excelWorkSheetName, rows, headers);
        }

        /// <summary> 
        /// Controller Extension: Excel result constructor for returning
        /// values from rows and headers and row keys. 
        /// </summary> 
        /// <param name="controller">This controller.</param> 
        /// <param name="fileName">Excel file name.</param> 
        /// <param name="excelWorkSheetName">
        /// Excel worksheet name: default: sheet1.</param> 
        /// <param name="rows">Excel row values.</param> 
        /// <param name="headers">Excel header values.</param> 
        /// <param name="rowKeys">Key values for the rows collection.</param> 
        /// <returns>Action result.</returns> 
        public static ActionResult Excel(this Controller controller, string fileName,
               string excelWorkSheetName, IList rows,
        string[] headers, string[] rowKeys)
        {
            return new ExcelResult(fileName, excelWorkSheetName, rows, headers, rowKeys);
        }
    }
}

namespace TauberMatching.Controllers.Results
{
     /// <summary> 
    /// Excel result class 
    /// </summary> 
    public class ExcelResult : ActionResult
    {
        /// <summary> 
        /// File Name. 
        /// </summary> 
        private string excelFileName;
        /// <summary> 
        /// Sheet Name. 
        /// </summary> 
        private string excelWorkSheetName;
        /// <summary> 
        /// Excel Row data. 
        /// </summary> 
        private IList rowData;
        /// <summary> 
        /// Excel Header Data. 
        /// </summary> 
        private string[] headerData = null;
        /// <summary> 
        /// Row Data Keys. 
        /// </summary> 
        private string[] rowPointers = null;
        /// <summary> 
        /// Action Result: Excel result constructor for returning values from rows. 
        /// </summary> 
        /// <param name="fileName">Excel file name.</param> 
        /// <param name="workSheetName">Excel worksheet name: default: sheet1.</param> 
        /// <param name="rows">Excel row values.</param> 
        public ExcelResult(string fileName, string workSheetName, IList rows)
            : this(fileName, workSheetName, rows, null, null)
        {
        }

        /// <summary> 
        /// Action Result: Excel result constructor
        /// for returning values from rows and headers. 
        /// </summary> 
        /// <param name="fileName">Excel file name.</param> 
        /// <param name="workSheetName">Excel worksheet name: default: sheet1.</param> 
        /// <param name="rows">Excel row values.</param> 
        /// <param name="headers">Excel header values.</param> 
        public ExcelResult(string fileName, string workSheetName,
               IList rows, string[] headers)
            : this(fileName, workSheetName, rows, headers, null)
        {
        }
        /// <summary> 
        /// Action Result: Excel result constructor for returning
        /// values from rows and headers and row keys. 
        /// </summary> 
        /// <param name="fileName">Excel file name.</param> 
        /// <param name="workSheetName">Excel worksheet name: default: sheet1.</param> 
        /// <param name="rows">Excel row values.</param> 
        /// <param name="headers">Excel header values.</param> 
        /// <param name="rowKeys">Key values for the rows collection.</param> 
        public ExcelResult(string fileName, string workSheetName,
               IList rows, string[] headers, string[] rowKeys)
        {
            this.rowData = rows;
            this.excelFileName = fileName;
            this.excelWorkSheetName = workSheetName;
            this.headerData = headers;
            this.rowPointers = rowKeys;
        }

        /// <summary> 
        /// Gets a value for file name. 
        /// </summary> 
        public string ExcelFileName
        {
            get { return this.excelFileName; }
        }
        /// <summary> 
        /// Gets a value for file name. 
        /// </summary> 
        public string ExcelWorkSheetName
        {
            get { return this.excelWorkSheetName; }
        }
        /// <summary> 
        /// Gets a value for rows. 
        /// </summary> 
        public IList ExcelRowData
        {
            get { return this.rowData; }
        }
        /// <summary> 
        /// Execute the Excel Result. 
        /// </summary> 
        /// <param name="context">Controller context.</param> 
        public override void ExecuteResult(ControllerContext context)
        {
            MemoryStream stream = ExcelDocument.Create(this.excelFileName,
                                  this.excelWorkSheetName, this.rowData,
                                  this.headerData, this.rowPointers);
            WriteStream(stream, this.excelFileName);
        }
        /// <summary> 
        /// Writes the memory stream to the browser. 
        /// </summary> 
        /// <param name="memoryStream">Memory stream.</param> 
        /// <param name="excelFileName">Excel file name.</param> 
        private static void WriteStream(MemoryStream memoryStream, string excelFileName)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.AddHeader("content-disposition",
              String.Format("attachment;filename={0}", excelFileName));
            memoryStream.WriteTo(context.Response.OutputStream);
            memoryStream.Close();
            context.Response.End();
        }
    }
}

namespace TauberMatching.Controllers 
{ 
    /// <summary> 
    /// Excel document. 
    /// </summary> 
    public static class ExcelDocument
    {
        /// <summary> 
        /// Default spread sheet name. 
        /// </summary> 
        private const string DefaultSheetName = "Sheet1";

        /// <summary> 
        /// Create the exel document for streaming. 
        /// </summary> 
        /// <param name="documentName">Excel file name.</param> 
        /// <param name="excelWorkSheetName">
        /// Excel worksheet name: default: sheet1.</param> 
        /// <param name="rowData">Row data to write.</param> 
        /// <param name="headerData">Header data.</param> 
        /// <param name="rowPointers">Row pointers.</param> 
        /// <returns>Memory stream.</returns> 
        public static MemoryStream Create(string documentName,
               string excelWorkSheetName, IList rowData,
               string[] headerData, string[] rowPointers)
        {
            return CreateSpreadSheet(documentName, excelWorkSheetName,
                   rowData, headerData, rowPointers, null);
        }

        /// <summary> 
        /// Create the spreadsheet. 
        /// </summary> 
        /// <param name="documentName">Excel file name.</param> 
        /// <param name="excelWorkSheetName">
        /// Excel worksheet name: default: sheet1.</param> 
        /// <param name="rowData">Row data to write.</param> 
        /// <param name="headerData">Header data.</param> 
        /// <param name="rowPointers">Row pointers.</param> 
        /// <param name="styleSheet">Style sheet.</param> 
        /// <returns>Memory stream.</returns> 
        private static MemoryStream CreateSpreadSheet(string documentName,
                string excelWorkSheetName, IList rowData,
                string[] headerData, string[] rowPointers, Stylesheet styleSheet)
        {
            int rowNum = 0;
            int colNum = 0;
            int maxWidth = 0;
            int minCol = 1;
            int maxCol = rowPointers == null ? minCol : rowPointers.Length;
            maxCol = maxCol == 1 && headerData == null ? 1 : headerData.Length;
            MemoryStream xmlStream = SpreadsheetReader.Create();
            SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(xmlStream, true);
            SetSheetName(excelWorkSheetName, spreadSheet);
            if (styleSheet == null)
            {
                SetStyleSheet(spreadSheet);
            }
            else
            {
                spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet = styleSheet;
                spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.Save();
            }
            WorksheetPart worksheetPart =
              SpreadsheetReader.GetWorksheetPartByName(spreadSheet,
                                excelWorkSheetName);
            WriteHeaders(headerData, out rowNum, out colNum,
                         out maxWidth, spreadSheet, worksheetPart);
            AddCellWidthStyles(Convert.ToUInt32(minCol),
                Convert.ToUInt32(maxCol), maxWidth, spreadSheet, worksheetPart);
            if (rowPointers == null || rowPointers.Length == 0)
            {
                WriteRowsFromHeaders(rowData, headerData, rowNum,
                                     out maxWidth, spreadSheet, worksheetPart);
            }
            else
            {
                WriteRowsFromKeys(rowData, rowPointers, rowNum,
                         out maxWidth, spreadSheet, worksheetPart);
            }

            // Save to the memory stream 
            SpreadsheetWriter.Save(spreadSheet);
            spreadSheet.Close();
            spreadSheet.Dispose();
            return xmlStream;
        }

        /// <summary> 
        /// Set the name of the spreadsheet. 
        /// </summary> 
        /// <param name="excelSpreadSheetName">Spread sheet name.</param> 
        /// <param name="spreadSheet">Spread sheet.</param> 
        private static void SetSheetName(string excelSpreadSheetName,
                            SpreadsheetDocument spreadSheet)
        {
            excelSpreadSheetName = excelSpreadSheetName ?? DefaultSheetName;
            Sheet ss = spreadSheet.WorkbookPart.Workbook.Descendants<Sheet>().Where(
                         s => s.Name == DefaultSheetName).SingleOrDefault<Sheet>();
            ss.Name = excelSpreadSheetName;
        }

        /// <summary> 
        /// Add cell width styles. 
        /// </summary> 
        /// <param name="minCol">Minimum column index.</param> 
        /// <param name="maxCol">Maximum column index.</param> 
        /// <param name="maxWidth">Maximum column width.</param> 
        /// <param name="spreadSheet">Spread sheet.</param> 
        /// <param name="workSheetPart">Work sheet.</param> 
        private static void AddCellWidthStyles(uint minCol, uint maxCol,
                int maxWidth, SpreadsheetDocument spreadSheet,
        WorksheetPart workSheetPart)
        {
            Columns cols = new Columns(new Column()
            {
                CustomWidth = true,
                Min = minCol,
                Max = maxCol,
                Width = maxWidth,
                BestFit = false
            });
            workSheetPart.Worksheet.InsertBefore<Columns>(cols,
               workSheetPart.Worksheet.GetFirstChild<SheetData>());
        }

        /// <summary> 
        /// Set the style sheet. 
        // Note: Setting the style here rather than passing it in ensures
        // that all worksheets will have a common user interface design. 
        /// </summary> 
        /// <param name="spreadSheet">Spread sheet to change.</param> 
        private static void SetStyleSheet(SpreadsheetDocument spreadSheet)
        {
            // Note: Setting the style here rather than passing it in
            // ensures that all worksheets will have a common user interface design. 
            Stylesheet styleSheet =
        spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet;
            styleSheet.Fonts.AppendChild(
              new Font(new FontSize() { Val = 11 },
              new Color() { Rgb = "FFFFFF" }, new FontName() { Val = "Arial" }));
            styleSheet.Fills.AppendChild(new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    BackgroundColor = new BackgroundColor() { Rgb = "D8D8D8" }
                }
            });
            spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.Save();
        }

        /// <summary> 
        /// Save the styl for worksheet headers. 
        /// </summary> 
        /// <param name="cellLocation">Cell location.</param> 
        /// <param name="spreadSheet">Spreadsheet to change.</param> 
        /// <param name="workSheetPart">Worksheet to change.</param> 
        private static void SeatHeaderStyle(string cellLocation,
                SpreadsheetDocument spreadSheet, WorksheetPart workSheetPart)
        {
            Stylesheet styleSheet =
              spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet;
            Cell cell = workSheetPart.Worksheet.Descendants<Cell>().Where(
                            c => c.CellReference == cellLocation).FirstOrDefault();
            if (cell == null)
            {
                throw new ArgumentNullException("Cell not found");
            }

            cell.SetAttribute(new OpenXmlAttribute("", "s", "", "1"));
            OpenXmlAttribute cellStyleAttribute = cell.GetAttribute("s", "");
            CellFormats cellFormats =
              spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
            // pick the first cell format. 
            CellFormat cellFormat = (CellFormat)cellFormats.ElementAt(0);
            CellFormat cf = new CellFormat(cellFormat.OuterXml);
            cf.FontId = styleSheet.Fonts.Count;
            cf.FillId = styleSheet.Fills.Count;
            cellFormats.AppendChild(cf);
            int a = (int)styleSheet.CellFormats.Count.Value;
            cell.SetAttribute(cellStyleAttribute);
            cell.StyleIndex = styleSheet.CellFormats.Count;
            workSheetPart.Worksheet.Save();
        }

        /// <summary> 
        /// Replace special characters. 
        /// </summary> 
        /// <param name="value">Value to input.</param> 
        /// <returns>Value with special characters replaced.</returns> 
        private static string ReplaceSpecialCharacters(string value)
        {
            value = value.Replace("’", "'");
            value = value.Replace("“", "\"");
            value = value.Replace("”", "\"");
            value = value.Replace("–", "-");
            value = value.Replace("…", "...");
            return value;
        }

        /// <summary> 
        /// Write values to the spreadsheet. 
        /// </summary> 
        /// <param name="cellLocation">Row Column Value.</param> 
        /// <param name="strValue">Value to write.</param> 
        /// <param name="spreadSheet">Spreadsheet to write to. </param> 
        /// <param name="workSheet">Worksheet to write to. </param> 
        private static void WriteValues(string cellLocation,
                string strValue, SpreadsheetDocument spreadSheet,
                WorksheetPart workSheet)
        {
            WorksheetWriter workSheetWriter =
                    new WorksheetWriter(spreadSheet, workSheet);
            int intValue = 0;
            if (strValue.Contains("$"))
            {
                strValue = strValue.Replace("$", "");
                strValue = strValue.Replace(",", "");
                workSheetWriter.PasteValue(cellLocation, strValue, CellValues.Number);
            }
            else if (int.TryParse(strValue, out intValue))
            {
                workSheetWriter.PasteValue(cellLocation, strValue, CellValues.Number);
            }
            else if (string.IsNullOrEmpty(strValue))
            {
                workSheetWriter.PasteText(cellLocation, strValue);
            }
            else
            {
                workSheetWriter.PasteText(cellLocation, strValue);
            }
        }

        /// <summary> 
        /// Write the excel rows for the spreadsheet. 
        /// </summary> 
        /// <param name="rowData">Excel row values.</param> 
        /// <param name="rowDataKeys">Excel row-key values.</param> 
        /// <param name="rowNum">Row number.</param> 
        /// <param name="maxWidth">Max width.</param> 
        /// <param name="spreadSheet">Spreadsheet to write to. </param> 
        /// <param name="workSheet">Worksheet to write to. </param> 
        private static void WriteRowsFromKeys(IList rowData,
                string[] rowDataKeys, int rowNum, out int maxWidth,
                SpreadsheetDocument spreadSheet, WorksheetPart workSheet)
        {
            maxWidth = 0;
            foreach (object row in rowData)
            {
                int colNum = 0;
                foreach (string rowKey in rowDataKeys)
                {
                    var rowValue = row.GetType().GetProperty(rowKey).GetValue(row, null);
                    string strValue = rowValue == null ? "" : rowValue.ToString();
                      //row.GetType().GetProperty(rowKey).GetValue(row, null).ToString();
                    strValue = ReplaceSpecialCharacters(strValue);
                    maxWidth = strValue.Length > maxWidth ? strValue.Length : maxWidth;
                    string cellLocation = string.Format("{0}{1}",
                           GetColumnLetter(colNum.ToString()), rowNum);
                    ExcelDocument.WriteValues(cellLocation, strValue,
                                              spreadSheet, workSheet);
                    colNum++;
                }
                rowNum++;
            }
        }

        /// <summary> 
        /// Convert column number to alpha numeric value. 
        /// </summary> 
        /// <param name="colNumber">Column number.</param> 
        /// <returns>ASCII value for number.</returns> 
        private static string GetColumnLetter(string colNumber)
        {
            if (string.IsNullOrEmpty(colNumber))
            {
                throw new ArgumentNullException(colNumber);
            }
            string colName = null;
            try
            {
                for (int i = 0; i < colNumber.Length; i++)
                {
                    string colValue = colNumber.Substring(i, 1);
                    int asc = Convert.ToInt16(colValue) + 65;
                    colName += Convert.ToChar(asc);
                }
            }
            finally
            {
                colName = colName ?? "A";
            }
            return colName;
        }

        /// <summary> 
        /// Write the values for the rows from headers. 
        /// </summary> 
        /// <param name="rowData">Excel row values.</param> 
        /// <param name="headerData">Excel header values.</param> 
        /// <param name="rowNum">Row number.</param> 
        /// <param name="maxWidth">Max width.</param> 
        /// <param name="spreadSheet">Spreadsheet to write to. </param> 
        /// <param name="workSheet">Worksheet to write to. </param> 
        private static void WriteRowsFromHeaders(IList rowData,
                string[] headerData, int rowNum, out int maxWidth,
                SpreadsheetDocument spreadSheet, WorksheetPart workSheet)
        {
            WorksheetWriter workSheetWriter = new WorksheetWriter(spreadSheet, workSheet);
            maxWidth = 0;
            foreach (object row in rowData)
            {
                int colNum = 0;
                foreach (string header in headerData)
                {
                    string strValue =
                      row.GetType().GetProperty(header).GetValue(row, null).ToString();
                    strValue = ReplaceSpecialCharacters(strValue);
                    maxWidth = strValue.Length > maxWidth ? strValue.Length : maxWidth;
                    string cellLocation = string.Format("{0}{1}",
                           GetColumnLetter(colNum.ToString()), rowNum);
                    ExcelDocument.WriteValues
                (cellLocation, strValue, spreadSheet, workSheet);
                    colNum++;
                }
                rowNum++;
            }
        }

        /// <summary> 
        /// Write the excel headers for the spreadsheet. 
        /// </summary> 
        /// <param name="headerData">Excel header values.</param> 
        /// <param name="rowNum">Row number.</param> 
        /// <param name="colNum">Column Number.</param> 
        /// <param name="maxWidth">Max column width</param> 
        /// <param name="spreadSheet">Maximum Column Width to write to. </param> 
        /// <param name="workSheet">Worksheet to write to. </param> 
        private static void WriteHeaders(string[] headerData, out int rowNum,
                out int colNum, out int maxWidth,
                SpreadsheetDocument spreadSheet, WorksheetPart workSheet)
        {
            rowNum = 1;
            colNum = 0;
            maxWidth = 0;
            foreach (string header in headerData)
            {
                string strValue = ReplaceSpecialCharacters(header);
                string cellLocation = string.Format("{0}{1}",
                       GetColumnLetter(colNum.ToString()), rowNum);
                maxWidth = strValue.Length > maxWidth ? strValue.Length : maxWidth;
                ExcelDocument.WriteValues(cellLocation, strValue, spreadSheet, workSheet);
                SeatHeaderStyle(cellLocation, spreadSheet, workSheet);
                colNum++;
            }
            rowNum++;
        }
    }
}