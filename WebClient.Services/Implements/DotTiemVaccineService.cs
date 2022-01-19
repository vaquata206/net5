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
using WebClient.Services.Helpers;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class DotTiemVaccineService : IDotTiemVaccineService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly DateTime now;
        private IDangKyTiemVaccineService dangKyTiemVaccineService;

        public DotTiemVaccineService(IDangKyTiemVaccineService dangKyTiemVaccineService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            now = DateTime.Now;
            this.dangKyTiemVaccineService = dangKyTiemVaccineService;
        }

        /// <summary>
        /// Tìm kiếm đợt tiêm
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đợt tiêm</returns>
        public async Task<PagingResponse<DotTiemVaccineInfo>> SearchAsync(PagingRequest<DotTiemVaccineFilterVM> pagingRequest, int idDonVi)
        {
            return await this.unitOfWork.DotTiemVaccineRepository.SearchAsync(pagingRequest, idDonVi);
        }

        /// <summary>
        /// Lay danh sach doi tuong dang ky tiem cua dot tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>DotTiem_VaccineVM</returns>
        public async Task<DotTiem_VaccineVM> LayThongTinDangKyTiemTheoIdDotTiem(int id_DotTiem)
        {
            var dotTiem = await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(id_DotTiem);
            var donVi = await this.unitOfWork.DepartmentRepository.GetByIdAsync(dotTiem.Id_DonVi);

            DotTiem_VaccineVM result = mapper.Map<DotTiem_VaccineVM>(dotTiem);
            result.Ten_DonVi = donVi.Ten_DonVi;

            var dsDoiTuong = await this.unitOfWork.DotTiemVaccineRepository.GetAllDoiTuongDangKyTiemTheoIdDotTiem(id_DotTiem);

            foreach(var doiTuong in dsDoiTuong)
            {
                doiTuong.Ten_GioiTinh = doiTuong.Gioi_Tinh == Constants.GioiTinh.Nam.GetHashCode() ? "Nam" : "Nữ";
                doiTuong.Nam_Sinh = doiTuong.Ngay_Sinh.ToString(Constants.FormatDate);
                if (doiTuong.TinhTrang_TiemChung == 1)
                {
                    doiTuong.LichSuTiem = await this.unitOfWork.DangKyTiemVaccineRepository.LayLichSuTiemVaccine(doiTuong.TinhTrang_TiemChung, doiTuong.Id_ThongTin);
                }
            }
            result.DsDoiTuongDangKy = dsDoiTuong;
            return result;
        }

        /// <summary>
        /// Nhap du lieu ket qua sau khi tiem chung
        /// </summary>
        /// <param name="id_DotTiem">id DotTiemVaccine</param>
        /// <param name="file">file ket qua tiem</param>
        /// <param name="account">user</param>
        /// <returns></returns>
        public async Task NhapKetQuaTiem(int id_DotTiem, IFormFile file, AccountInfo account)
        {
            if (file != null)
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);
                ms.Position = 0;
                ISheet sheet;
                if (file.FileName.Split('.')[1].Equals("xls"))
                {
                    var wb = new HSSFWorkbook(ms);
                    sheet = wb.GetSheetAt(0);
                }
                else
                {
                    var wb = new XSSFWorkbook(ms);
                    sheet = wb.GetSheetAt(0);
                }

                var listCMND = (await this.unitOfWork.DangKyTiemVaccineRepository.LayDsCMNDCuaDoiTuongDangKyTheoIdDotTiem(id_DotTiem)).ToList();
                List<LichSuTiemVaccineVM> listKetQuaTiem = new();
                var dotTiem = await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(id_DotTiem);
                int rowIndex = 5;

                // Lay thong tin dot tiem vaccine
                var ten_KeHoach = sheet.GetRow(rowIndex).GetCell(3).StringCellValue;
                var ngay_HenTiem = sheet.GetRow(rowIndex++).GetCell(8).StringCellValue;
                var ten_DoiTiem = sheet.GetRow(rowIndex).GetCell(3).StringCellValue;
                var diaDiem_Tiem = sheet.GetRow(rowIndex++).GetCell(8).StringCellValue;
                var lo_Vaccine = sheet.GetRow(rowIndex).GetCell(3).StringCellValue;
                var loai_Vaccine = sheet.GetRow(rowIndex).GetCell(8).StringCellValue;

                // Lay thong tin danh sach ket qua tiem
                rowIndex = 15;
                while (!string.IsNullOrEmpty(sheet.GetRow(rowIndex).GetCell(Constants.SttHangCotExcelKetQuaTiem.CMND).StringCellValue))
                {
                    // lấy row hiện tại
                    var nowRow = sheet.GetRow(rowIndex);
                    if (!string.IsNullOrEmpty(sheet.GetRow(rowIndex).GetCell(Constants.SttHangCotExcelKetQuaTiem.NgayTiem).StringCellValue))
                    {
                        var cmnd = nowRow.GetCell(Constants.SttHangCotExcelKetQuaTiem.CMND).StringCellValue;
                        var thongTinCMND = listCMND.Find(e => e.CMND.Trim().Equals(cmnd.Trim()));
                        var ngayTiemStr = nowRow.GetCell(Constants.SttHangCotExcelKetQuaTiem.NgayTiem)?.StringCellValue;

                        if (thongTinCMND != null && DateTime.TryParseExact(ngayTiemStr, format: Constants.FormatDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngayTiem))
                        {
                            
                            var ngayNhapStr = nowRow.GetCell(Constants.SttHangCotExcelKetQuaTiem.NgayNhapThongTin)?.StringCellValue;
                            LichSuTiemVaccineVM item = new()
                            {
                                Id_DangKyTiem = thongTinCMND.Id_DangKy,
                                Ngay_Tiem = ngayTiem,
                                Ten_KeHoach = ten_KeHoach,
                                Ten_DoiTiem = ten_DoiTiem,
                                DiaDiem_Tiem = diaDiem_Tiem,
                                Lo_Vaccine = lo_Vaccine,
                                Loai_Vaccine = loai_Vaccine,
                                SoMui_DaTiem = int.Parse(nowRow.GetCell(Constants.SttHangCotExcelKetQuaTiem.SoMuiDaTiem).NumericCellValue.ToString()),
                                Ngay_KhoiTao = now,
                                Id_NV_KhoiTao = account.UserId,
                                Id_DonVi = thongTinCMND.Id_DonVi
                            };

                            if (DateTime.TryParseExact(ngayNhapStr, format: Constants.FormatDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngayNhap)) {
                                item.NgayNhap_ThongTin = ngayNhap;
                            };


                            listKetQuaTiem.Add(item);
                            listCMND.Remove(thongTinCMND);
                        }
                    }
                    
                    // tăng index khi lấy xong
                    rowIndex++;
                }

                // Cap nhat thong tin dot tiem vaccine
                dotTiem.SoLuong_DaTiem = listKetQuaTiem.Count;
                await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(dotTiem);

                var listDotTiem_Phuong = (await this.unitOfWork.DotTiemVaccineRepository.LayDsDotTiemTheoIdCha(id_DotTiem)).ToList();
                var listGroup = from item in listKetQuaTiem
                                    group item by item.Id_DonVi;
                foreach (var item in listGroup)
                {
                    var dotTiem_Phuong = listDotTiem_Phuong.First(e => e.Id_DonVi == item.Key);
                    dotTiem_Phuong.SoLuong_DaTiem = item.Count();

                    await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(dotTiem_Phuong);
                }

                if (listKetQuaTiem.Count > 0)
                {
                    await this.unitOfWork.LichSuTiemVaccineRepository.LuuDanhSachKetQuaTiem(this.mapper.Map<IEnumerable<LichSu_Tiem_Vaccine>>(listKetQuaTiem).ToList());
                }

                this.unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Xuất file danh sách đăng ký tiêm
        /// </summary>
        /// <param name="id">id dottiem</param>
        /// <returns>file excel</returns>
        public async Task<MemoryStream> ExportDsDangKyTiem(int id)
        {
            var data = await this.LayThongTinDangKyTiemTheoIdDotTiem(id);
            var export = new ExportDsDangKyTiem(data);
            return export.RenderExcel();
        }

        /// Get dot tiem vaccine theo id
        /// </summary>
        /// <param name="id">id dot tiem vaccine</param>
        /// <returns>dot tiem vaccine</returns>
        public async Task<DotTiemVaccine> GetByIdAsync(int id)
        {
            return await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(id);
        }

        public async Task LuuDanhSachTiemVaccine(int id, IEnumerable<int> listDangKy, int dangKy, int userId)
        {
            var entity = await this.GetByIdAsync(id);
            if (entity.SoLuong_DangKy < listDangKy.Count())
            {
                throw new Exception("Số lượng đăng ký nhiều hơn cho phép");      
            }

            if (entity.Trang_Thai == Constants.States.Disabed.GetHashCode())
            {
                throw new Exception("Đợt đăng ký đã đóng không được cập nhật");
            }

            var oldlist = await this.unitOfWork.DotTiemVaccineRepository.GetThongTin_DangKy_Tiem_Vaccine(id);
            var newls = listDangKy.Where(x => !oldlist.Any(y => y.Id_ThongTin_NguoiDan == x));
            var removels = oldlist.Where(x => !listDangKy.Any(y => x.Id_ThongTin_NguoiDan == y));
            var isChanged = false;
            foreach (var i in newls.Distinct())
            {
                await this.unitOfWork.DotTiemVaccineRepository.AddObjectAsync(new ThongTin_DangKy_Tiem_Vaccine
                {
                    Id_DotTiem_Vaccine = id,
                    Id_ThongTin_NguoiDan = i,
                    Tinh_Trang = Constants.States.Actived.GetHashCode(),
                    Id_NV_KhoiTao = userId,
                    Ngay_KhoiTao = now
                });

                isChanged = true;
            }

            foreach (var i in removels)
            {
                i.Tinh_Trang = Constants.States.Disabed.GetHashCode();
                i.Id_NV_CapNhat = userId;
                i.Ngay_CapNhat = now;
                await this.unitOfWork.DotTiemVaccineRepository.UpdateObjectAsync(i);

                isChanged = true;
            }

            var parent = await this.GetByIdAsync(entity.Id_Cha);
            if (entity.TinhTrang_DangKy != dangKy)
            {
                if (dangKy == Constants.States.Actived.GetHashCode())
                {
                    entity.Ngay_DangKy = now;
                }

                entity.TinhTrang_DangKy = dangKy;
                entity.Ngay_CapNhat = now;
                entity.Id_NV_CapNhat = userId;
                await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(entity);
                isChanged = true;
            }

            if (isChanged)
            {
                this.unitOfWork.Commit();
            }
        }

        public async Task HuyDangKyDotTiemVaccine(int id, AccountInfo account)
        {
            var entity = await this.GetByIdAsync(id);
            if (entity.Trang_Thai == Constants.States.Disabed.GetHashCode())
            {
                throw new Exception("Quá thời hạn hủy đăng ký tiêm vaccine");
            }

            entity.TinhTrang_DangKy = Constants.States.Disabed.GetHashCode();
            entity.Ngay_CapNhat = now;
            entity.Id_NV_CapNhat = account.UserId;
            await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(entity);
            this.unitOfWork.Commit();
        }

        /// Lay danh sach doi tuong dang ky tiem cua dot tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>DotTiem_VaccineVM</returns>
        public async Task<DotTiemVaccineVM> LayThongTinDotTiemTheoId(int id_DotTiem)
        {
            var dotTiem = await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(id_DotTiem);
            var donVi = await this.unitOfWork.DepartmentRepository.GetByIdAsync(dotTiem.Id_DonVi);

            DotTiemVaccineVM result = mapper.Map<DotTiemVaccineVM>(dotTiem);
            result.Ten_DonVi = donVi.Ten_DonVi;

            return result;
        }

        /// <summary>
        /// Lay danh sach dot tiem Vaccine
        /// </summary>
        /// <returns>List dot tiem Vaccine</returns>
        public async Task<PagingResponse<DotTiemVaccineInfo>> LayDanhSachDotTiemVaccineTheoIdCha(int id_Cha, int id_DonVi, int userId)
        {
            PagingResponse<DotTiemVaccineInfo> dsDotTiem;
            PagingRequest<DotTiemVaccineFilterVM> request = new PagingRequest<DotTiemVaccineFilterVM>()
            {
                Draw = 1,
                Length = -1,
                Filter = new DotTiemVaccineFilterVM
                {
                    Id_DotTiem = id_Cha
                }
            };
            if (id_Cha > 0)
            {
                dsDotTiem = await this.unitOfWork.DotTiemVaccineRepository.LayDanhSachDotTiemVaccineTheoIdCha(request);
            }
            else
            {
                dsDotTiem = new PagingResponse<DotTiemVaccineInfo>
                {
                    Draw = request.Draw
                };
                var dsPhuong = await this.unitOfWork.DepartmentRepository.GetDepartmentsByIdParent(id_DonVi, userId);
                var dsDotTiemMoi = new List<DotTiemVaccineInfo>();
                foreach (var phuong in dsPhuong)
                {
                    var dotTiemMoi = new DotTiemVaccineInfo
                    {
                        Id_DotTiem = 0,
                        Ten_DonVi = phuong.Ten_DonVi,
                        Id_Cha = 0,
                        Id_DonVi = phuong.Id_DonVi,
                        SoLuong_DangKy = 0
                    };
                    dsDotTiemMoi.Add(dotTiemMoi);
                }
                dsDotTiem.Data = dsDotTiemMoi;
                dsDotTiem.RecordsTotal = dsDotTiemMoi.Count;
                dsDotTiem.RecordsFiltered = dsDotTiemMoi.Count;
            }
            return dsDotTiem;
        }

        /// <summary>
        /// Luu dot tiem Vaccine
        /// <param name="viewModel">viewModel</param>
        /// </summary>
        /// <returns>result</returns>
        public async Task SaveAsync(DotTiemVaccineVM viewModel, int userId)
        {
            DotTiemVaccine entity = this.mapper.Map<DotTiemVaccine>(viewModel);
            if (viewModel.Id_DotTiem > 0)
            {
                // cap nhat
                var dotTiemVaccine = await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(viewModel.Id_DotTiem);
                var dsDotTiemPhuong = await this.unitOfWork.DotTiemVaccineRepository.LayDsDotTiemTheoIdCha(viewModel.Id_DotTiem);

                dotTiemVaccine.Ten_KeHoach = viewModel.Ten_KeHoach;
                dotTiemVaccine.SoLuong_DangKy = viewModel.SoLuong_DangKy;
                dotTiemVaccine.SoLuong_DaTiem = viewModel.SoLuong_DaTiem;

                dotTiemVaccine.NgayTiem_BatDau = viewModel.NgayTiem_BatDau;
                dotTiemVaccine.NgayTiem_KetThuc = viewModel.NgayTiem_KetThuc;

                dotTiemVaccine.Ghi_Chu = viewModel.Ghi_Chu;

                dotTiemVaccine.Ngay_CapNhat = DateTime.Now;
                dotTiemVaccine.Id_NV_CapNhat = userId;
                await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(dotTiemVaccine);

                // cap nhat SoLuong_DangKy cho DotTiem Phuong
                var tongSoVaccinDangKy = viewModel.ds_SoLuong_DangKy.Select(x => x).Sum();
                if (tongSoVaccinDangKy > dotTiemVaccine.SoLuong_DangKy)
                {
                    throw new Exception("Số lượng vaccine đăng ký lớn hơn đợt tiêm cha");
                }
                for (var i = 0; i < viewModel.ds_Id_DonVi.Length; i++)
                {
                    var dotTiemPhuong = dsDotTiemPhuong.ToList().Where(x => x.Id_DonVi == viewModel.ds_Id_DonVi[i]).First();
                    dotTiemPhuong.SoLuong_DangKy = viewModel.ds_SoLuong_DangKy[i];
                    dotTiemPhuong.Id_NV_CapNhat = userId;
                    dotTiemPhuong.Ngay_CapNhat = DateTime.Now;
                    await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(dotTiemPhuong);
                }
            }
            else
            {
                // them moi DotTiem Quan
                var tongSoVaccinDangKy = viewModel.ds_SoLuong_DangKy.Select(x => x).Sum();
                if (tongSoVaccinDangKy > viewModel.SoLuong_DangKy)
                {
                    throw new Exception("Số lượng vaccine đăng ký lớn hơn đợt tiêm cha");
                }
                entity.Ma_DotTiem = "DT" + DateTime.Now.Ticks;
                entity.Ten_KeHoach = viewModel.Ten_KeHoach;
                entity.Id_DonVi = viewModel.Id_DonVi;

                entity.SoLuong_DangKy = viewModel.SoLuong_DangKy;
                entity.SoLuong_DaTiem = 0;

                entity.NgayTiem_BatDau = viewModel.NgayTiem_BatDau;
                entity.NgayTiem_KetThuc = viewModel.NgayTiem_KetThuc;

                entity.Ghi_Chu = viewModel.Ghi_Chu;

                entity.Id_NV_KhoiTao = userId;
                entity.Ngay_KhoiTao = DateTime.Now;
                entity.TinhTrang_DangKy = Constants.States.Disabed.GetHashCode();
                entity.Tinh_Trang = Constants.States.Actived.GetHashCode();
                entity.Trang_Thai = Constants.States.Actived.GetHashCode();
                var dotTiemQuan = await this.unitOfWork.DotTiemVaccineRepository.AddAsync(entity);

                // Tao dottiem Phuong
                for (var i = 0 ; i < viewModel.ds_Id_DonVi.Length; i++)
                {
                    var phuong = await this.unitOfWork.DepartmentRepository.GetByIdAsync(viewModel.ds_Id_DonVi[i]);
                    var dotTiemPhuong = new DotTiemVaccine
                    {
                        Ma_DotTiem = "DTP" + DateTime.Now.Ticks,
                        Ten_KeHoach = entity.Ten_KeHoach,
                        Id_DonVi = phuong.Id_DonVi,
                        Id_Cha = dotTiemQuan.Id_DotTiem,

                        SoLuong_DangKy = viewModel.ds_SoLuong_DangKy[i],
                        SoLuong_DaTiem = 0,
                        NgayTiem_BatDau = viewModel.NgayTiem_BatDau,
                        NgayTiem_KetThuc = viewModel.NgayTiem_KetThuc,

                        Id_NV_KhoiTao = userId,
                        Ngay_KhoiTao = DateTime.Now,
                        Trang_Thai = entity.Trang_Thai,
                        Tinh_Trang = entity.Tinh_Trang,
                        TinhTrang_DangKy = entity.TinhTrang_DangKy
                    };
                    await this.unitOfWork.DotTiemVaccineRepository.AddAsync(dotTiemPhuong);
                }
            }
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Edit ds dot tiem Vaccine cua Phuong
        /// <param name="dsDotTiem">viewModel</param>
        /// <param name="userId">id current user</param>
        /// </summary>
        /// <returns>result</returns>
        public async Task EditDsDotTiemPhuongAsync(List<DotTiemVaccineVM> dsDotTiem, int userId)
        {
            if (dsDotTiem.Count() > 0)
            {
                var dotTiemCha = await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(dsDotTiem[0].Id_Cha);

                var tongSoVaccinDangKy = dsDotTiem.Select(x => x.SoLuong_DangKy).Sum();
                if (tongSoVaccinDangKy > dotTiemCha.SoLuong_DangKy)
                {
                    throw new Exception("Số lượng vaccine đăng ký lớn hơn đợt tiêm cha");
                }
                foreach (var dotTiem in dsDotTiem)
                {
                    var entity = await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(dotTiem.Id_DotTiem);
                    entity.SoLuong_DangKy = dotTiem.SoLuong_DangKy;
                    entity.Id_NV_CapNhat = userId;
                    entity.Ngay_CapNhat = DateTime.Now;
                    await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(entity);
                }
                this.unitOfWork.Commit();
            }
        }

        /// <summary>
        /// delete Dottiem by id
        /// </summary>
        /// <param name="id">Dottiem id</param>
        /// <param name="userId">Current user id</param>
        /// <returns>the Task</returns>
        public async Task DeleteByIdAsync(int id, int userId)
        {
            var dotTiemVaccine = await this.unitOfWork.DotTiemVaccineRepository.GetByIdAsync(id);
            dotTiemVaccine.Id_NV_CapNhat = userId;
            dotTiemVaccine.Ngay_CapNhat = DateTime.Now;
            dotTiemVaccine.Tinh_Trang = Constants.States.Disabed.GetHashCode();
            await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(dotTiemVaccine);
            var listDotTiem = await this.unitOfWork.DotTiemVaccineRepository.LayDsDotTiemTheoIdCha(dotTiemVaccine.Id_DotTiem);
            foreach (var i in listDotTiem)
            {
                i.Tinh_Trang = dotTiemVaccine.Tinh_Trang;
                i.Id_NV_CapNhat = userId;
                i.Ngay_CapNhat = DateTime.Now;
                await this.unitOfWork.DotTiemVaccineRepository.UpdateAsync(i);
            }

            this.unitOfWork.Commit();
        }

        /// <summary>
        /// Chốt danh sách đăng ký của đợt tiêm và đợt tiêm con
        /// </summary>
        /// <param name="id">id đợt tiêm</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        public async Task ChotDanhSachDotTiem(int id, AccountInfo account)
        {
            await this.unitOfWork.DotTiemVaccineRepository.ChotDanhSachDotTiem(id, account);
            this.unitOfWork.Commit();
        }

        public async Task DangKyExcel(int idDotTiem, IFormFile fileDSDKTiem, AccountInfo account)
        {
            var list = await this.dangKyTiemVaccineService.DongBoDSDKTiemFromExcel(fileDSDKTiem, account, commit : false);
            await LuuDanhSachTiemVaccine(idDotTiem, list.Select(x => x.Id_ThongTin), 0, account.UserId);
        }
    }
}
