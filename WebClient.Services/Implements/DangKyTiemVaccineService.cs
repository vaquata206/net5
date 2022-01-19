using AutoMapper;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class DangKyTiemVaccineService : IDangKyTiemVaccineService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly DateTime now;
        private readonly IMapper mapper;

        public DangKyTiemVaccineService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            now = DateTime.Now;
            this.mapper = mapper;
        }

        /// <summary>
        /// Tìm kiếm đối tượng đăng ký tiêm vaccine
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đối tượng đăng ký tiêm</returns>
        public async Task<PagingResponse<DoiTuongDangKyTiemInfo>> SearchAsync(PagingRequest<DoiTuongDangKyTiemFilterVM> pagingRequest)
        {
            return await this.unitOfWork.DangKyTiemVaccineRepository.SearchAsync(pagingRequest);
        }

        /// <summary>
        /// Lấy thông tin người đăng ký tiêm vaccine theo id người đăng ký
        /// </summary>
        /// <param name="id">id người đăng ký</param>
        /// <returns>Thông tin chi tiết người đăng ký</returns>
        public async Task<ThongTin_NguoiDan> GetByIdASync(int id)
        {
            var thongTin = await this.unitOfWork.DangKyTiemVaccineRepository.GetByIdAsync(id);
            if (thongTin == null)
            {
                throw new System.Exception("Không tìm thấy thông tin chi tiết. Vui lòng kiểm tra lại.");
            }

            thongTin.SoMuiDaTiem = await this.unitOfWork.DangKyTiemVaccineRepository.GetTongSoMuiDaTiemVaccine(id);
            return thongTin;
        }

        /// <summary>
        /// Cập nhật thông tin người đăng ký tiêm vaccine
        /// </summary>
        /// <param name="thongTinNguoiDanVM">thông tin cập nhật</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        public async Task SaveAsync(ThongTinNguoiDanVM thongTinNguoiDanVM, AccountInfo account)
        {
            var thongTinCu = await this.unitOfWork.DangKyTiemVaccineRepository.GetByIdAsync(thongTinNguoiDanVM.Id_ThongTin);
            var thongTinCapNhat = mapper.Map<ThongTinNguoiDanVM, ThongTin_NguoiDan>(thongTinNguoiDanVM);
            thongTinCapNhat.Id_NV_CapNhat = account.UserId;
            thongTinCapNhat.Ngay_CapNhat = now;
            thongTinCapNhat.Id_NV_KhoiTao = thongTinCu.Id_NV_KhoiTao;
            thongTinCapNhat.Ngay_KhoiTao = thongTinCu.Ngay_KhoiTao;
            thongTinCapNhat.Ghi_Chu = thongTinCu.Ghi_Chu;
            thongTinCapNhat.Tinh_Trang = thongTinCu.Tinh_Trang;

            await this.unitOfWork.DangKyTiemVaccineRepository.UpdateAsync(thongTinCapNhat);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Xóa: Cập nhật tình trạng của thông tin người đăng ký tiêm vaccine theo id
        /// </summary>
        /// <param name="id">id người đăng ký</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        public async Task DeleteAsync(int id, AccountInfo account)
        {
            var thongTin = await this.GetByIdASync(id);
            thongTin.Tinh_Trang = Constants.States.Disabed.GetHashCode();
            thongTin.Id_NV_CapNhat = account.UserId;
            thongTin.Ngay_CapNhat = now;

            await this.unitOfWork.DangKyTiemVaccineRepository.UpdateAsync(thongTin);
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Thong ke tong quat so luong lieu vaccine, so nguoi dang ky, so nguoi da tiem  mui 1, 2
        /// </summary>
        /// <param name="id_DonVi">id don vi thong ke</param>
        /// <returns></returns>
        public async Task<ThongKeTongQuatVM> ThongKeTongQuatSoLuong(int id_DonVi)
        {
            return await this.unitOfWork.DangKyTiemVaccineRepository.ThongKeTongQuatSoLuong(id_DonVi);
        }

        /// <summary>
        /// Import danh sách đăng ký tiêm vaccine
        /// </summary>
        /// <param name="FileDSDKTiem">File excel danh sách đăng ký tiêm vaccine</param>
        /// <param name="account">User</param>
        /// <returns></returns>
        public async Task<IEnumerable<ThongTin_NguoiDan>> DongBoDSDKTiemFromExcel(IFormFile FileDSDKTiem, AccountInfo account, bool commit = true)
        {
            if (FileDSDKTiem != null)
            {
                var ms = new MemoryStream();
                FileDSDKTiem.CopyTo(ms);
                ms.Position = 0;
                ISheet sheet;
                if (FileDSDKTiem.FileName.Split('.')[1].Equals("xls"))
                {
                    var wb = new HSSFWorkbook(ms);
                    sheet = wb.GetSheetAt(0);
                }
                else
                {
                    var wb = new XSSFWorkbook(ms);
                    sheet = wb.GetSheetAt(0);
                }

                var listNguoiDaDangKy = (await this.unitOfWork.DangKyTiemVaccineRepository.GetAllAsync()).ToList();
                int rowIndex = 3;
                var now = DateTime.Now;

                var listDanToc = await this.unitOfWork.DmDanTocRepository.GetAllAsync();
                var listDonVi = await this.unitOfWork.DepartmentRepository.GetDepartmentsWithTerm(account.UserId, account.DepartmentId);
                var listDoiTuongUuTien = await this.unitOfWork.DmDoiTuongUuTienRepository.GetAllAsync();

                var listUpdated = new List<ThongTin_NguoiDan>();
                var listAdded = new List<ThongTin_NguoiDan>();
                var list = new List<ThongTin_NguoiDan>();

                while (true)
                {
                    var nowRow = sheet.GetRow(rowIndex);
                    var cmnd = nowRow?.GetCell(Constants.STTHangCotExcelDSDKTiem.CMND)?.ToString();
                    if (string.IsNullOrEmpty(cmnd))
                    {
                        break;
                    }

                    cmnd = cmnd.Trim();

                    bool error = false;

                    var stt = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.STT)?.ToString();
                    var ho_Ten = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Ho_Ten).ToString();
                    var gioiTinhStr = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Gioi_Tinh).ToString();
                    var ngaySinhStr = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Ngay_Sinh)?.ToString();
                    var email = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Email).ToString();
                    var nghe_Nghiep = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Nghe_Nghiep).ToString();
                    var donVi_CongTac = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.DonVi_CongTac).ToString();
                    var so_DienThoai = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.So_DienThoai).ToString();
                    var soThe_BHYT = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.SoThe_BHYT).ToString();
                    var ma_DoiTuong_UuTien = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Ma_DoiTuong_UuTien).ToString();
                    var ma_DanToc = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Ma_DanToc).ToString();
                    var quoc_Tich = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Quoc_Tich).ToString();
                    var ma_DonVi = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.Ma_DonVi).ToString();
                    var diaChi_HienTai = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.DiaChi_HienTai).ToString();
                    var soMuiDKTiem = nowRow.GetCell(Constants.STTHangCotExcelDSDKTiem.SoMuiDKTiem).ToString();

                    if (string.IsNullOrEmpty(ho_Ten))
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Họ tên không đúng", stt));
                    }

                    if (string.IsNullOrEmpty(gioiTinhStr) || !int.TryParse(gioiTinhStr, out int gioi_Tinh))
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Giới tính không đúng", stt));
                    }

                    DateTime ngay_Sinh = DateTime.MinValue;
                    if (string.IsNullOrEmpty(ngaySinhStr) || !DateTime.TryParseExact(ngaySinhStr, Constants.FormatDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out ngay_Sinh))
                    {
                        if (!DateTime.TryParseExact(ngaySinhStr, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngay_Sinh))
                        {
                            error = true;
                            throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Ngày sinh không đúng", stt));
                        }
                    }

                    var id_DoiTuong_UuTien = listDoiTuongUuTien.FirstOrDefault(x => x.Ma_DoiTuong == ma_DoiTuong_UuTien)?.Id_DoiTuong;
                    if (!id_DoiTuong_UuTien.HasValue)
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Mã nhóm đối tượng ưu tiên không đúng", stt));
                    }

                    var id_DonVi = listDonVi.FirstOrDefault(x => x.Ma_DonVi == ma_DonVi)?.Id_DonVi;
                    if (!id_DonVi.HasValue)
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Mã xã/phường không đúng", stt));
                    }

                    if (string.IsNullOrEmpty(so_DienThoai))
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Số điện thoại không đúng", stt));
                    }

                    if (string.IsNullOrEmpty(cmnd))
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Số CMND/CCCD/Hộ chiếu không đúng", stt));
                    }

                    var id_DanToc = listDanToc.FirstOrDefault(x => x.Ma_DanToc == ma_DanToc)?.Id_DanToc;
                    if (!id_DanToc.HasValue)
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu dân tộc không đúng", stt));
                    }

                    if (string.IsNullOrEmpty(quoc_Tich))
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Quốc tịch không đúng", stt));
                    }

                    if (string.IsNullOrEmpty(soMuiDKTiem) || !int.TryParse(soMuiDKTiem, out int soMuiDaTiem))
                    {
                        error = true;
                        throw new Exception(string.Format("STT {0} chưa nhập hoặc dữ liệu Số mũi đăng ký tiêm không đúng", stt));
                    }

                    if (!error)
                    {
                        var thongTinCMND = listNguoiDaDangKy.FirstOrDefault(e => e.CMND.Trim().Equals(cmnd));

                        if (thongTinCMND != null)
                        {
                            ThongTin_NguoiDan item = new()
                            {
                                Id_ThongTin = thongTinCMND.Id_ThongTin,
                                Ho_Ten = ho_Ten,
                                Gioi_Tinh = gioi_Tinh,
                                Ngay_Sinh = ngay_Sinh,
                                Email = email,
                                Nghe_Nghiep = nghe_Nghiep,
                                DonVi_CongTac = donVi_CongTac,
                                So_DienThoai = so_DienThoai,
                                CMND = cmnd,
                                SoThe_BHYT = soThe_BHYT,
                                Id_DoiTuong_UuTien = id_DoiTuong_UuTien ?? 0,
                                Id_DanToc = id_DanToc ?? 0,
                                Quoc_Tich = quoc_Tich,
                                Id_DonVi = id_DonVi ?? 0,
                                DiaChi_HienTai = diaChi_HienTai,
                                SoMuiDaTiem = soMuiDaTiem > 0? soMuiDaTiem - 1: 0,
                                Ngay_CapNhat = now,
                                Id_NV_CapNhat = account.UserId,
                                Ngay_KhoiTao = thongTinCMND.Ngay_KhoiTao,
                                Id_NV_KhoiTao = thongTinCMND.Id_NV_KhoiTao,
                                Ghi_Chu = thongTinCMND.Ghi_Chu,
                                Tinh_Trang = thongTinCMND.Tinh_Trang
                            };

                            listUpdated.Add(item);
                            listNguoiDaDangKy.Remove(thongTinCMND);
                        }
                        else
                        {
                            ThongTin_NguoiDan item = new()
                            {
                                Ho_Ten = ho_Ten,
                                Gioi_Tinh = gioi_Tinh,
                                Ngay_Sinh = ngay_Sinh,
                                Email = email,
                                Nghe_Nghiep = nghe_Nghiep,
                                DonVi_CongTac = donVi_CongTac,
                                So_DienThoai = so_DienThoai,
                                CMND = cmnd,
                                SoThe_BHYT = soThe_BHYT,
                                Id_DoiTuong_UuTien = id_DoiTuong_UuTien ?? 0,
                                Id_DanToc = id_DanToc ?? 0,
                                Quoc_Tich = quoc_Tich,
                                Id_DonVi = id_DonVi ?? 0,
                                DiaChi_HienTai = diaChi_HienTai,
                                SoMuiDaTiem = soMuiDaTiem > 0 ? soMuiDaTiem - 1 : 0,
                                Ngay_CapNhat = null,
                                Id_NV_CapNhat = null,
                                Ngay_KhoiTao = now,
                                Id_NV_KhoiTao = account.UserId,
                                Ghi_Chu = null,
                                Tinh_Trang = Constants.States.Actived.GetHashCode()
                            };
                            listAdded.Add(item);
                        }
                    }

                    rowIndex++;
                }

                // Cap nhat
                foreach (var i in listUpdated)
                {
                    await this.unitOfWork.DangKyTiemVaccineRepository.UpdateAsync(i);
                    list.Add(i);
                }

                // Them moi
                foreach (var i in listAdded)
                {
                    await this.unitOfWork.DangKyTiemVaccineRepository.AddAsync(i);
                    list.Add(i);
                }

                if (list.Count > 0 && commit)
                {
                    this.unitOfWork.Commit();
                }

                return list;
            }

            return new List<ThongTin_NguoiDan>();
        }
    }
}
