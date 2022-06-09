using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.SharePoint;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class ExcelHandler
    {
        #region Variable Members
        private readonly IExceptionService exceptionService;
        SPFile file;
        MemoryStream memStr;
        WorkbookPart wbPart = null;
        SpreadsheetDocument document = null;
        Dictionary<string, Worksheet> worksheetList;
        Dictionary<string, Dictionary<string, string>> columnHeaders;
        Dictionary<string, uint> columnHeaderRowIds;
        bool ignoreNotFoundHeaders;
        #endregion

        public ExcelHandler(IExceptionService exceptionService, bool ignoreNotFoundHeaders)
        {
            this.exceptionService = exceptionService;
            SPFolder projectFolder;
            string folderUrl;
            byte[] fileContent;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spSite.OpenWeb())
                    {
                        try
                        {
                            //Get the document from the library using SPQuery
                            SPList list = spSite.RootWeb.Lists[GlobalConstants.DOCLIBRARY_CompassTemplatesName];
                            folderUrl = list.RootFolder.ServerRelativeUrl;
                            if (spweb.GetFolder(folderUrl).Exists)
                            {
                                projectFolder = spweb.GetFolder(folderUrl);
                                //Get the first document returned. There should be one only.
                                file = projectFolder.Files.OfType<SPFile>().Where(x => x.Name.Equals(GlobalConstants.EXP_SYNC_TEMPLATE_NAME)).ToList().First();
                                if (file == null)
                                    throw new Exception("Copy of template Sync file was not found");
                                fileContent = file.OpenBinary();
                                memStr = new MemoryStream();
                                memStr.Write(fileContent, 0, fileContent.Length);
                                document = SpreadsheetDocument.Open(memStr, true);
                                wbPart = document.WorkbookPart;
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "ExcelHandler", "constructor");
                        }
                    }
                }
            });
            worksheetList = new Dictionary<string, Worksheet>();
            columnHeaders = new Dictionary<string, Dictionary<string, string>>();
            columnHeaderRowIds = new Dictionary<string, uint>();
            this.ignoreNotFoundHeaders = ignoreNotFoundHeaders;
        }

        public MemoryStream SaveDocument()
        {
            document.Close();
            return memStr;
        }
        public bool updateRowByHeaders(string sheetName, Dictionary<string, string> values, uint valuesRowId, ref string message, uint headerRowId)
        {
            string[] headers;
            int w = 0;
            if (!columnHeaders.ContainsKey(sheetName))
            {
                headers = new string[values.Count];
                foreach (string header in values.Keys)
                {
                    headers[w] = header;
                    w++;
                }
                SetColumnHeaders(sheetName, headerRowId, headers);
            }
            return updateRowByHeaders(sheetName, values, valuesRowId, ref message);
        }
        public bool updateRowByHeaders(string sheetName, Dictionary<string, string> values, uint valuesRowId, ref string message)
        {
            Worksheet ws;
            SheetData sheet;
            bool isString;
            double numero;
            Dictionary<string, string> columns;
            string header, value, excelColumn, withoutCommas;
            uint headerRowId = 0;
            Row row;
            if (!columnHeaderRowIds.TryGetValue(sheetName, out headerRowId))
                { message = "columnHeaderRowId not found"; return false; }
            if (!columnHeaders.TryGetValue(sheetName, out columns))
                { message = "columnHeaders not found"; return false; }
            ws = GetWorksheet(sheetName);
            if (ws == null)
                { message = "Sheet not found"; return false; }
            sheet = ws.GetFirstChild<SheetData>();
            row = GetRow(sheet, valuesRowId);
            foreach (KeyValuePair<string, string> headerValue in values)
            {
                header = headerValue.Key;
                value = headerValue.Value;
                if (string.IsNullOrEmpty(value))
                    continue;
                if (!columns.TryGetValue(header, out excelColumn))
                {
                    if (!ignoreNotFoundHeaders)
                    { message = "Header " + header + " not found"; return false; }
                    continue;
                }
                withoutCommas = value.Replace(",", "");
                isString = !Double.TryParse(withoutCommas, out numero);
                if (isString)
                {
                    if (value.Length > 0 && value[0] == '\'')
                        UpdateValueByRowCol(row, excelColumn, valuesRowId, value.Substring(1), true);
                    else
                        UpdateValueByRowCol(row, excelColumn, valuesRowId, value, true);
                }
                else
                    UpdateValueByRowCol(row, excelColumn, valuesRowId, withoutCommas, false);
            }
            ws.Save();
            return true;
        }

        private string CopyFile(string source, string dest)
        {
            string result = "Copied file";
            try
            {
                // Overwrites existing files
                File.Copy(source, dest, true);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private Worksheet GetWorksheet(string sheetName)
        {
            Sheet sheet;
            Worksheet ws = null;

            worksheetList.TryGetValue(sheetName, out ws);
            if (ws == null)
            {
                sheet = wbPart.Workbook.Descendants<Sheet>().Where((s) => s.Name == sheetName).FirstOrDefault();
                if (sheet == null)
                    return null;
                ws = ((WorksheetPart)(wbPart.GetPartById(sheet.Id))).Worksheet;
                if (ws != null)
                    worksheetList.Add(sheetName, ws);
            }
            return ws;
        }

        public Dictionary<string, string> SetColumnHeaders(string sheetName, uint headerRowId, string[] headers)
        {
            Dictionary<string, string> columns = null;
            Worksheet ws;
            SheetData sheetData;
            Cell cell;
            Row row;
            string column = "";
            uint rowId = 0;
            int index;
            if (columnHeaders.TryGetValue(sheetName, out columns))
                return columns;
            ws = GetWorksheet(sheetName);
            if (ws == null)
                return null;
            sheetData = ws.GetFirstChild<SheetData>();
            row = GetRow(sheetData, headerRowId);
            columns = new Dictionary<string, string>();
            foreach (string header in headers)
            {
                index = GetSharedStringItem(header);
                if (index != -1)
                {
                    cell = row.Elements<Cell>().Where(c => c.CellValue != null && c.CellValue.Text == index.ToString()).FirstOrDefault();
                    if (cell != null)
                    {
                        if (SplitAddres(cell.CellReference, ref column, ref rowId))
                            columns.Add(header, column);
                    }
                }
            }
            columnHeaders.Add(sheetName, columns);
            columnHeaderRowIds.Add(sheetName, headerRowId);
            return columns;
        }

        private bool UpdateByHeader(string sheetName, uint headerRowId, string header, uint newValueRowId, string newValue, bool isString)
        {
            Row row;
            string column = "";
            int index;
            uint rowId = 0;
            Worksheet ws;
            SheetData sheetData;
            Cell cell;
            index = GetSharedStringItem(header);
            if (index == -1)
                return false;
            ws = GetWorksheet(sheetName);
            if (ws == null)
                return false;
            sheetData = ws.GetFirstChild<SheetData>();
            row = GetRow(sheetData, headerRowId);
            cell = row.Elements<Cell>().Where(c => c.CellValue.Text == index.ToString()).FirstOrDefault();
            if (cell == null)
                return false;
            if (!SplitAddres(cell.CellReference, ref column, ref rowId))
                return false;
            return UpdateValueByRowCol(ws, column, newValueRowId, newValue, isString);
        }

        // Given a Worksheet and an address (like "AZ254"), either return a cell reference, or 
        // create the cell reference and return it.
        private Cell InsertCellInWorksheet(Worksheet ws, string column, uint rowId)
        {
            string addressName;
            SheetData sheetData = ws.GetFirstChild<SheetData>();
            Cell cell = null;

            addressName = column + rowId;
            Row row = GetRow(sheetData, rowId);

            // If the cell you need already exists, return it.
            // If there is not a cell with the specified column name, insert one.  
            Cell refCell = row.Elements<Cell>().
                Where(c => c.CellReference.Value == addressName).FirstOrDefault();
            if (refCell != null)
            {
                cell = refCell;
            }
            else
            {
                cell = CreateCell(row, addressName);
            }
            return cell;
        }
        private Cell InsertCellInWorksheet(Row row, string column, uint rowId)
        {
            string addressName;
            Cell cell = null;
            addressName = column + rowId;
            // If the cell you need already exists, return it.
            // If there is not a cell with the specified column name, insert one.  
            Cell refCell = row.Elements<Cell>().
                Where(c => c.CellReference.Value == addressName).FirstOrDefault();
            if (refCell != null)
            {
                cell = refCell;
            }
            else
            {
                cell = CreateCell(row, addressName);
            }
            return cell;
        }
        private Cell CreateCell(Row row, String address)
        {
            Cell cellResult;
            Cell refCell = null;

            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, address, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            cellResult = new Cell();
            cellResult.CellReference = address;

            row.InsertBefore(cellResult, refCell);
            return cellResult;
        }

        private Row GetRow(SheetData wsData, UInt32 rowIndex)
        {
            var row = wsData.Elements<Row>().
            Where(r => r.RowIndex.Value == rowIndex).FirstOrDefault();
            if (row == null)
            {
                row = new Row();
                row.RowIndex = rowIndex;
                wsData.Append(row);
            }
            return row;
        }

        private UInt32 GetRowIndex(string address)
        {
            string rowPart;
            UInt32 l;
            UInt32 result = 0;

            for (int i = 0; i < address.Length; i++)
            {
                if (UInt32.TryParse(address.Substring(i, 1), out l))
                {
                    rowPart = address.Substring(i, address.Length - i);
                    if (UInt32.TryParse(rowPart, out l))
                    {
                        result = l;
                        break;
                    }
                }
            }
            return result;
        }

        private bool SplitAddres(string address, ref string column, ref UInt32 rowId)
        {
            bool success = false;
            int w;
            for (w = 1; w < address.Length; w++)
            {
                if ((int)address[w] < 65)
                {
                    column = address.Substring(0, w);
                    rowId = Convert.ToUInt32(address.Substring(w));
                    success = true;
                    break;
                }
            }
            return success;
        }

        public bool UpdateValueByAddress(string sheetName, string addressName, string value, bool isString)
        {
            Worksheet ws = null;
            string column = "";
            uint rowId = 0;
            ws = GetWorksheet(sheetName);
            if (ws == null)
                return false;
            if (!SplitAddres(addressName, ref column, ref rowId))
                return false;
            return UpdateValueByRowCol(ws, column, rowId, value, isString);
        }
        private bool UpdateValueByRowCol(Worksheet ws, string column, uint rowId, string value, bool isString)
        {
            int stringIndex;
            UInt32Value styleIndex = 0;
            Cell cell;
            cell = InsertCellInWorksheet(ws, column, rowId);
            if (isString)
            {
                // Either retrieve the index of an existing string,
                // or insert the string into the shared string table
                // and get the index of the new item.
                stringIndex = InsertSharedStringItem(value);

                cell.CellValue = new CellValue(stringIndex.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            }
            else
            {
                cell.CellValue = new CellValue(value);
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }
            if (styleIndex > 0)
                cell.StyleIndex = styleIndex;

            // Save the worksheet.
            ws.Save();
            return true;
        }
        private bool UpdateValueByRowCol(Row row, string column, uint rowId, string value, bool isString)
        {
            int stringIndex;
            UInt32Value styleIndex = 0;
            Cell cell;

            cell = InsertCellInWorksheet(row, column, rowId);
            if (isString)
            {
                // Either retrieve the index of an existing string,
                // or insert the string into the shared string table
                // and get the index of the new item.
                stringIndex = InsertSharedStringItem(value);

                cell.CellValue = new CellValue(stringIndex.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            }
            else
            {
                cell.CellValue = new CellValue(value);
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }
            if (styleIndex > 0)
                cell.StyleIndex = styleIndex;

            return true;
        }
        // Given the main workbook part, and a text value, insert the text into the shared
        // string table. Create the table if necessary. If the value already exists, return
        // its index. If it doesn't exist, insert it and return its new index.
        private int InsertSharedStringItem(string value)
        {
            int index = 0;
            bool found = false;
            var stringTablePart = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

            // If the shared string table is missing, something's wrong.
            // Just return the index that you found in the cell.
            // Otherwise, look up the correct text in the table.
            if (stringTablePart == null)
            {
                // Create it.
                stringTablePart = wbPart.AddNewPart<SharedStringTablePart>();
            }

            var stringTable = stringTablePart.SharedStringTable;
            if (stringTable == null)
            {
                stringTable = new SharedStringTable();
            }

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in stringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == value)
                {
                    found = true;
                    break;
                }
                index += 1;
            }

            if (!found)
            {
                stringTable.AppendChild(new SharedStringItem(new Text(value)));
                stringTable.Save();
            }

            return index;
        }

        private int GetSharedStringItem(string value)
        {
            int index = 0;
            bool found = false;
            var stringTablePart = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

            if (stringTablePart == null)
                return -1;

            var stringTable = stringTablePart.SharedStringTable;
            if (stringTable == null)
                return -1;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in stringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == value)
                {
                    found = true;
                    break;
                }
                index += 1;
            }

            if (found)
                return index;
            else
                return -1;
        }

        // Used to force a recalc of cells containing formulas. The
        // CellValue has a cached value of the evaluated formula. This
        // will prevent Excel from recalculating the cell even if 
        // calculation is set to automatic.
        private bool RemoveCellValue(string sheetName, string column, uint rowId)
        {
            bool returnValue = false;

            Sheet sheet = wbPart.Workbook.Descendants<Sheet>().
                Where(s => s.Name == sheetName).FirstOrDefault();
            if (sheet != null)
            {
                Worksheet ws = ((WorksheetPart)(wbPart.GetPartById(sheet.Id))).Worksheet;
                Cell cell = InsertCellInWorksheet(ws, column, rowId);

                // If there is a cell value, remove it to force a recalc
                // on this cell.
                if (cell.CellValue != null)
                {
                    cell.CellValue.Remove();
                }

                // Save the worksheet.
                ws.Save();
                returnValue = true;
            }

            return returnValue;
        }
    }
}
