using System;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace WebClient.Services.Helpers
{
    public class ExcelHelper
    {
        /// <summary>
        /// Chỉ set giá trị, và copy format từ row khác
        /// </summary>
        /// <param name="rowSource">Lấy format từ 1 row nào đó gán vào</param>
        /// <param name="rowTarget">Row được gán vào</param>
        /// <param name="column">row</param>
        /// <param name="data">data gán vào</param>
        public void SetCellValueExcel(IRow rowSource, IRow rowTarget, int column, string data)
        {
            var cell = rowTarget.CreateCell(column);
            cell.CellStyle = rowSource.GetCell(column).CellStyle;
            cell.Row.Height = (short)-1;
            cell.SetCellValue(data);
        }
        public void SetCellValueExcel(IRow rowSource, IRow rowTarget, int column, double data)
        {
            var cell = rowTarget.CreateCell(column);
            cell.CellStyle = rowSource.GetCell(column).CellStyle;
            cell.SetCellValue(data);
        }
        public void SetCellValueExcel(IRow rowSource, IRow rowTarget, int column, decimal data)
        {
            var cell = rowTarget.CreateCell(column);
            cell.CellStyle = rowSource.GetCell(column).CellStyle;
            cell.SetCellValue((double)data);
        }
        public void SetCellValueExcel(IRow rowSource, IRow rowTarget, int row, bool data, bool spaceOnNULL = true)
        {
            var cell = rowTarget.CreateCell(row);
            cell.CellStyle = rowSource.GetCell(row).CellStyle;
            if (spaceOnNULL && data == false)
            {
                cell.SetCellValue(string.Empty);
            }
            else cell.SetCellValue(data);
        }
        public void SetCellValueExcel(IRow rowSource, IRow rowTarget, int row, DateTime data, bool spaceOnNULL = true)
        {
            var cell = rowTarget.CreateCell(row);
            cell.CellStyle = rowSource.GetCell(row).CellStyle;
            if (spaceOnNULL && data == DateTime.MinValue)
            {
                cell.SetCellValue(string.Empty);
            }
            else cell.SetCellValue(data);
        }
        public void SetCellValueExcel(IRow rowSource, IRow rowTarget, int row, double data, bool spaceOnNULL = true)
        {
            var cell = rowTarget.CreateCell(row);
            cell.CellStyle = rowSource.GetCell(row).CellStyle;
            if (spaceOnNULL && data == 0d)
            {
                cell.SetCellValue(string.Empty);
            }
            else cell.SetCellValue(data);
        }
        public void SetCellValueExcel(IRow rowSource, IRow rowTarget, int row, int data, bool spaceOnNULL = true)
        {
            var cell = rowTarget.CreateCell(row);
            cell.CellStyle = rowSource.GetCell(row).CellStyle;
            if (spaceOnNULL && data == 0)
            {
                cell.SetCellValue(string.Empty);
            }
            else cell.SetCellValue(data);
        }

        public void SetCellValueExcel(IRow row, int column, decimal param)
        {
            var cell = row.GetCell(column);
            if (cell == null)
            {
                cell = row.CreateCell(column);
            }

            cell.SetCellValue(Convert.ToDouble(param));
        }
        public void SetCellFormulaExcel(IRow rowSource, IRow rowTarget, int row, string data)
        {
            var cell = rowTarget.CreateCell(row);
            cell.CellStyle = rowSource.GetCell(row).CellStyle;
            cell.SetCellFormula(data);
        }

        /// <summary>
        /// Format Cell
        /// </summary>
        /// <param name="sheet">tên sheet</param>
        /// <param name="row">dòng thứ (bắt đầu từ 0)</param>
        /// <param name="column">cột</param>
        /// <param name="param">Giá trị ô</param>
        public void FormatCellValueExcel(ISheet sheet, int row, int column, string param)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                cell = sheet.GetRow(row).CreateCell(column);
            }

            cell.SetCellValue(param);
        }

        public void FormatCellValueExcel(ISheet sheet, int row, int column, bool param)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                cell = sheet.GetRow(row).CreateCell(column);
            }

            cell.SetCellValue(param);
        }
        public void FormatCellValueExcel(ISheet sheet, int row, int column, int param)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                cell = sheet.GetRow(row).CreateCell(column);
            }

            cell.SetCellValue(Convert.ToDouble(param));
        }
        public void FormatCellValueExcel(ISheet sheet, int row, int column, DateTime param)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                cell = sheet.GetRow(row).CreateCell(column);
            }

            cell.SetCellValue(param);
        }
        public void FormatCellValueExcel(ISheet sheet, int row, int column, double param)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                cell = sheet.GetRow(row).CreateCell(column);
            }

            cell.SetCellValue(param);
        }
        public void FormatCellFormulaExcel(ISheet sheet, int row, int column, string param)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                cell = sheet.GetRow(row).CreateCell(column);
            }

            cell.SetCellFormula(param);
        }
        public void FormatCellFormulaExcel(IRow rowSource, IRow rowTarget, int column, string param)
        {
            var cell = rowTarget.CreateCell(column);
            cell.CellStyle = rowSource.GetCell(column).CellStyle;
            cell.SetCellFormula(param);
        }
        public void FormatCellValueExcel(ISheet sheet, int row, int column, params string[] param)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                cell = sheet.GetRow(row).CreateCell(column);
            }

            cell.SetCellValue(string.Format(cell.StringCellValue, param));
        }
        public void FormatCellValueExcel(IRow row, int column, params string[] param)
        {
            var cell = row.GetCell(column);
            if (cell == null)
            {
                cell = row.CreateCell(column);
            }

            cell.SetCellValue(string.Format(cell.StringCellValue, param));
        }

        public string GetValueCell(ISheet sheet, int row, int column)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
            }

            return null;
        }

        /// <summary>
        /// set value cho 1 cell
        /// </summary>
        /// <param name="sheet">sheet</param>
        /// <param name="row">hàng</param>
        /// <param name="column">cột</param>
        /// <param name="data">giá trị</param>
        /// <returns></returns>
        public ICell SetCellValueExcel(ISheet sheet, int row, int column, string data)
        {
            var cell = sheet.GetRow(row).CreateCell(column);
            cell.SetCellValue(data);
            return cell;
        }

        public void CopyRow(XSSFWorkbook workbook, ISheet sourceWorksheet, int sourceRowNum, ISheet desWorksheet, int destinationRowNum)
        {
            // Get the source / new row
            IRow newRow = desWorksheet.GetRow(destinationRowNum);
            IRow sourceRow = sourceWorksheet.GetRow(sourceRowNum);

            newRow = desWorksheet.CreateRow(destinationRowNum);

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);
                ICell newCell = newRow.CreateCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }

                // Copy style from old cell and apply to new cell
                ICellStyle newCellStyle = workbook.CreateCellStyle();
                newCellStyle.CloneStyleFrom(oldCell.CellStyle); ;
                newCell.CellStyle = newCellStyle;

                // If there is a cell comment, copy
                if (newCell.CellComment != null) newCell.CellComment = oldCell.CellComment;

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null) newCell.Hyperlink = oldCell.Hyperlink;

                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);

                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case CellType.Blank:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    case CellType.Boolean:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    case CellType.Error:
                        newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                        break;
                    case CellType.Formula:
                        newCell.SetCellFormula(oldCell.CellFormula);
                        break;
                    case CellType.Numeric:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    case CellType.String:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;
                    case CellType.Unknown:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                }
            }

            // If there are are any merged regions in the source row, copy to new row
            for (int i = 0; i < sourceWorksheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = sourceWorksheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.RowNum
                        , (newRow.RowNum + (cellRangeAddress.FirstRow - cellRangeAddress.LastRow))
                        , cellRangeAddress.FirstColumn
                        , cellRangeAddress.LastColumn);
                    desWorksheet.AddMergedRegion(newCellRangeAddress);
                }
            }

        }
    }
}
