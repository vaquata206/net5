using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using WebClient.Core.ViewModels;
using WebClient.Core.Helper;
using WebClient.Core.Helpers;

namespace WebClient.Services.Helpers
{
    public class ExportDsDangKyTiem
    {
        private readonly string sExcelFileName = "MauBc_DanhSachDangKyTiemVaccine.xlsx";
        private readonly DotTiem_VaccineVM data;
        private readonly ExcelHelper excelHelper = new();
        public ExportDsDangKyTiem(DotTiem_VaccineVM data)
        {
            this.data = data;
        }

        public MemoryStream RenderExcel()
        {
            var memory = new MemoryStream();

            XSSFWorkbook workBook;
            using (var stream = new FileStream("./wwwroot/templates/" + sExcelFileName, FileMode.Open, FileAccess.Read))
            {
                workBook = new XSSFWorkbook(stream);
                stream.Close();
            }

            ISheet sheet = workBook.GetSheetAt(0);
            ISheet sheetMau = workBook.GetSheetAt(1);
            var rowMau = sheetMau.GetRow(0);
            var rowHienTai = 4;
            IRow row;
            
            this.excelHelper.FormatCellValueExcel(sheet, 0, 0, string.Format("DANH SÁCH ĐỐI TƯỢNG ĐĂNG KÝ TIÊM VẮC XIN COVID 19").ToUpper());

            var dsDoiTuongDangKy = data.DsDoiTuongDangKy;
            if (dsDoiTuongDangKy != null && dsDoiTuongDangKy.Any())
            {
                for (var i = 0; i < dsDoiTuongDangKy.Count(); i++)
                {
                    var item = dsDoiTuongDangKy.ElementAt(i);
                    row = sheet.CreateRow(rowHienTai);
                    row.RowStyle = rowMau.RowStyle;
                    this.excelHelper.SetCellValueExcel(rowMau, row, 0, (i + 1));
                    this.excelHelper.SetCellValueExcel(rowMau, row, 1, item.Ho_Ten);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 2, item.Ten_GioiTinh);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 3, item.Ngay_Sinh.ToString(Constants.FormatDate));
                    this.excelHelper.SetCellValueExcel(rowMau, row, 4, item.Email);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 5, item.Id_DoiTuong_UuTien.ToString());
                    this.excelHelper.SetCellValueExcel(rowMau, row, 6, item.Nghe_Nghiep);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 7, item.DonVi_CongTac);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 8, item.So_DienThoai);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 9, item.CMND);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 10, item.SoThe_BHYT);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 11, item.Id_DanToc);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 12, item.Quoc_Tich);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 13, Constants.DmTinhThanh.Ten_Tinh);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 14, Constants.DmTinhThanh.Ma_Tinh);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 15, item.Ten_QuanHuyen);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 16, item.Ma_QuanHuyen);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 17, item.Ten_PhuongXa);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 18, item.Ma_PhuongXa);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 19, item.DiaChi_HienTai);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 20, data.NgayTiem_BatDau);
                    this.excelHelper.SetCellValueExcel(rowMau, row, 21, item.TinhTrang_TiemChung + (item.Trang_Thai == Constants.States.Actived.GetHashCode() ? 0 : 1));
                    if (item.TinhTrang_TiemChung == 1 && item.LichSuTiem != null)
                    {
                        this.excelHelper.SetCellValueExcel(rowMau, row, 22, item.LichSuTiem.Loai_Vaccine);
                        this.excelHelper.SetCellValueExcel(rowMau, row, 23, item.LichSuTiem.Ngay_Tiem.ToString("dd/MM/yyyy HH:mm"));
                        this.excelHelper.SetCellValueExcel(rowMau, row, 24, item.LichSuTiem.Lo_Vaccine);
                        this.excelHelper.SetCellValueExcel(rowMau, row, 25, item.LichSuTiem.DiaDiem_Tiem);
                        this.excelHelper.SetCellValueExcel(rowMau, row, 26, item.LichSuTiem.PhanUng_SauTiem);
                    }
                    else
                    {
                        this.excelHelper.SetCellValueExcel(rowMau, row, 22, string.Empty);
                        this.excelHelper.SetCellValueExcel(rowMau, row, 23, string.Empty);
                        this.excelHelper.SetCellValueExcel(rowMau, row, 24, string.Empty);
                        this.excelHelper.SetCellValueExcel(rowMau, row, 25, string.Empty);
                        this.excelHelper.SetCellValueExcel(rowMau, row, 26, string.Empty);
                    }
                    if (data.Trang_Thai == Constants.States.Disabed.GetHashCode())
                    {
                        this.excelHelper.SetCellValueExcel(rowMau, row, 27, item.Trang_Thai == 1 ? "Đã tiêm" : "Chưa tiêm");
                    }
                    else
                    {
                        this.excelHelper.SetCellValueExcel(rowMau, row, 27, item.Ghi_Chu_DangKyTiem);
                    }
                    
                    ++rowHienTai;
                }
            }


            // Xóa sheet mẫu
            workBook.RemoveSheetAt(1);
            workBook.Write(memory);
            memory.Position = 0;
            return memory;
        }
        }
}
