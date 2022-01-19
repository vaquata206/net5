using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DotTiemVaccineRepository : BaseRepository<DotTiemVaccine>, IDotTiemVaccineRepository
    {
        public DotTiemVaccineRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Tìm kiếm đợt tiêm
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đợt tiêm</returns>
        public async Task<PagingResponse<DotTiemVaccineInfo>> SearchAsync(PagingRequest<DotTiemVaccineFilterVM> pagingRequest, int idDonVi)
        {
            var response = new PagingResponse<DotTiemVaccineInfo>
            {
                Draw = pagingRequest.Draw
            };

            var whereClause = new List<string>();

            if (!string.IsNullOrEmpty(pagingRequest.Filter.TenDotTiem))
            {
                whereClause.Add("UPPER(DTVC.Ten_KeHoach) like '%'||:TenDotTiem||'%'");
            }

            if (pagingRequest.Filter.TrangThai > 0)
            {
                whereClause.Add("DTVC.Trang_Thai = :TrangThai");
            }

            if (pagingRequest.Filter.TinhTrang > 0)
            {
                whereClause.Add("DTVC.TinhTrang_DangKy = :TinhTrang");
            }

            if (idDonVi > 0)
            {
                whereClause.Add("DTVC.Id_DonVi = :idDonVi");
            }

            if (pagingRequest.Filter.NgayTiem.HasValue)
            {
                whereClause.Add("DTVC.NgayTiem_BatDau <= :NgayTiem AND DTVC.NgayTiem_KetThuc >= :NgayTiem ");
            }

            whereClause.Add("DTVC.Tinh_Trang = 1");
            var pagingString = string.Empty;
            if (pagingRequest.Length > 0)
            {
                pagingString = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pagingRequest.Start, pagingRequest.Length);
            }

            var parameters = new
            {
                idDonVi,
                TenDotTiem = pagingRequest.Filter.TenDotTiem?.Trim()?.ToUpper(),
                TinhTrang = pagingRequest.Filter.TinhTrang - 1,
                TrangThai = pagingRequest.Filter.TrangThai - 1,
                NgayTiem = pagingRequest.Filter.NgayTiem?.Date
            };

            var whereString = string.Join(" and ", whereClause);
            var list = await this.dbContext.QueryAsync<DotTiemVaccineInfo>(
                sql: string.Format(@"SELECT DTVC.Id_DotTiem, DTVC.Ma_DotTiem, DTVC.Ten_KeHoach, DTVC.SoLuong_DaTiem, DTVC.NgayTiem_BatDau, DTVC.NgayTiem_KetThuc, DTVC.Trang_Thai ,
                                    DTVC.SoLuong_DangKy, DTVC.ID_CHA, DTVC.HANCUOI_DANGKY,
                                    CASE 
                                        WHEN DTVC.ID_CHA = 0 THEN 
                                            CASE WHEN (SELECT COUNT(1) FROM DotTiem_Vaccine WHERE ID_CHA = DTVC.ID_DOTTIEM AND TINH_TRANG = 1 AND TINHTRANG_DANGKY = 1) = (SELECT COUNT(1) FROM DotTiem_Vaccine WHERE ID_CHA = DTVC.ID_DOTTIEM AND TINH_TRANG = 1)
                                            THEN 1 ELSE 0 END
                                        ELSE DTVC.TinhTrang_DangKy
                                    END AS TinhTrang_DangKy
                                    FROM DotTiem_Vaccine DTVC
                                    LEFT JOIN Don_Vi DV  ON DTVC.Id_DonVi = DV.Id_DonVi AND DV.Tinh_Trang = 1
                                    WHERE {0} ORDER BY DTVC.Ngay_KhoiTao DESC {1}", whereString, pagingString),
                param: parameters);

            var total = await this.dbContext.QueryFirstOrDefaultAsync<int>(
                sql: string.Format("SELECT count(1) FROM DotTiem_Vaccine DTVC " +
                                    "LEFT JOIN Don_Vi DV  ON DTVC.Id_DonVi = DV.Id_DonVi AND DV.Tinh_Trang = 1 " +
                                    "WHERE {0}", whereString),
                param: parameters
                );

            response.Data = list;
            response.RecordsTotal = total;
            response.RecordsFiltered = total;
            return response;
        }

        /// <summary>
        /// Lay danh sach dot tiem Vaccine
        /// </summary>
        /// <returns>List dot tiem Vaccine</returns>
        public async Task<PagingResponse<DotTiemVaccineInfo>> LayDanhSachDotTiemVaccineTheoIdCha(PagingRequest<DotTiemVaccineFilterVM> pagingRequest)
        {
            var response = new PagingResponse<DotTiemVaccineInfo>
            {
                Draw = pagingRequest.Draw
            };

            var parameters = new
            {
                id_Cha = pagingRequest.Filter.Id_DotTiem
            };
            var pagingString = string.Empty;
            if (pagingRequest.Length > 0)
            {
                pagingString = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pagingRequest.Start, pagingRequest.Length);
            }

            var query = string.Format("SELECT DTVC.Id_DotTiem, DTVC.Id_Cha, DTVC.Ma_DotTiem, DTVC.Ten_KeHoach, DTVC.SoLuong_DaTiem, DTVC.NgayTiem_BatDau, DTVC.NgayTiem_KetThuc, DTVC.Trang_Thai , " +
                                " DTVC.TinhTrang_DangKy, DTVC.SoLuong_DangKy, DV.Id_DonVi ,DV.Ten_DonVi " +
                             "FROM DotTiem_Vaccine DTVC " +
                             "LEFT JOIN Don_Vi DV  ON DTVC.Id_DonVi = DV.Id_DonVi AND DV.Tinh_Trang = 1 " +
                             "WHERE DTVC.Id_Cha = :id_Cha and DTVC.Tinh_Trang = 1 ORDER BY DTVC.Ngay_KhoiTao DESC {0}", pagingString);

            var list = await this.dbContext.QueryAsync<DotTiemVaccineInfo>(sql: query, param: parameters);

            var totalSQL = "SELECT count(1) FROM DotTiem_Vaccine DTVC WHERE Id_Cha = :id_Cha and Tinh_Trang = 1";
            var total = await this.dbContext.QueryFirstOrDefaultAsync<int>(
               totalSQL, param: parameters, commandType: System.Data.CommandType.Text);

            response.Data = list;
            response.RecordsTotal = total;
            response.RecordsFiltered = total;
            return response;
        }

        /// Lay danh sach doi tuong dang ky tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>List ThongTin_NguoiDan</returns>
        public async Task<IEnumerable<ThongTin_NguoiDanVM>> GetAllDoiTuongDangKyTiemTheoIdDotTiem(int id_DotTiem)
        {
            var query = @"SELECT
                            TTND.*, DTUT.MA_DOITUONG Ma_DoiTuong_UuTien, DTUT.NGHE_NGHIEP NgheNghiepUuTien,
                            TTDKTVC.Ghi_Chu  Ghi_Chu_DangKyTiem, TTDKTVC.Id_DangKy, 
                            DV.MA_DONVI AS Ma_PhuongXa,
                            DV.TEN_DONVI AS Ten_PhuongXa,
                            QUAN.MA_DONVI AS Ma_QuanHuyen,
                            QUAN.TEN_DONVI AS Ten_QuanHuyen,
                            CASE WHEN
                                (SELECT COUNT(1)
                                    FROM LICHSU_TIEM_VACCINE
                                    WHERE LICHSU_TIEM_VACCINE.ID_DANGKYTIEM = TTDKTVC.ID_DANGKY AND LICHSU_TIEM_VACCINE.TINH_TRANG = 1) >= 1
                                THEN 1 ELSE 0 END AS TRANG_THAI,
                            NVL((SELECT MAX(LICHSU_TIEM_VACCINE.SOMUI_DATIEM)
                                    FROM LICHSU_TIEM_VACCINE
                                    WHERE LICHSU_TIEM_VACCINE.ID_DANGKYTIEM = TTDKTVC.ID_DANGKY AND LICHSU_TIEM_VACCINE.TINH_TRANG = 1), TTND.SOMUIDATIEM) AS TINHTRANG_TIEMCHUNG
                        FROM THONGTIN_NGUOIDAN TTND
                        JOIN THONGTIN_DANGKY_TIEM_VACCINE TTDKTVC ON TTND.ID_THONGTIN = TTDKTVC.ID_THONGTIN_NGUOIDAN
                        JOIN DOTTIEM_VACCINE DTVC ON TTDKTVC.ID_DOTTIEM_VACCINE = DTVC.ID_DOTTIEM
                        JOIN DON_VI DV ON DV.ID_DONVI = TTND.ID_DONVI
                        LEFT JOIN DON_VI QUAN ON QUAN.ID_DONVI = DV.ID_DV_CHA
                        LEFT JOIN DM_DOITUONG_UUTIEN DTUT ON DTUT.ID_DOITUONG = TTND.ID_DOITUONG_UUTIEN AND DTUT.TINH_TRANG = 1
                        WHERE TTND.TINH_TRANG = 1
                            AND TTDKTVC.TINH_TRANG = 1
                            AND DV.TINH_TRANG = 1
                            AND DTVC.TINH_TRANG = 1
                            AND (DTVC.ID_DOTTIEM = :id_dottiem OR DTVC.ID_CHA = :id_dottiem) ORDER BY Ma_PhuongXa, HO_TEN ";

            var response = await this.dbContext.QueryAsync<ThongTin_NguoiDanVM>(
                query,
                param: new { id_dottiem = id_DotTiem },
                commandType: System.Data.CommandType.Text
            );

            return response;
        }

        public async Task<IEnumerable<ThongTin_DangKy_Tiem_Vaccine>> GetThongTin_DangKy_Tiem_Vaccine(int id)
        {
            var query = @"SELECT * FROM THONGTIN_DANGKY_TIEM_VACCINE WHERE ID_DOTTIEM_VACCINE = :id AND TINH_TRANG = 1";

            return await this.dbContext.QueryAsync<ThongTin_DangKy_Tiem_Vaccine>(
                query,
                param: new { id = id },
                commandType: System.Data.CommandType.Text
            );
        }

        /// <summary>
        /// Lay danh sach cac dot tiem cua phuong theo id dot tiem cua Quan
        /// </summary>
        /// <param name="id_DotTiem">id dot tiem cua Quan</param>
        /// <returns></returns>
        public async Task<IEnumerable<DotTiemVaccine>> LayDsDotTiemTheoIdCha(int id_DotTiem)
        {
            var query = @"SELECT *
                        FROM DOTTIEM_VACCINE DTV
                        WHERE DTV.TINH_TRANG = 1
                        AND DTV.ID_CHA = :id_dottiem";

            var response = await this.dbContext.QueryAsync<DotTiemVaccine>(
                query,
                param: new { id_dottiem = id_DotTiem },
                commandType: System.Data.CommandType.Text
            );

            return response;
        }

        /// <summary>
        /// Chốt danh sách đăng ký của đợt tiêm và đợt tiêm con
        /// </summary>
        /// <param name="idDotTiem">id đợt tiêm</param>
        /// <param name="account">thông tin người cập nhật</param>
        /// <returns>không trả về</returns>
        public async Task ChotDanhSachDotTiem(int idDotTiem, AccountInfo account)
        {
            var query = string.Format(@"UPDATE DOTTIEM_VACCINE SET 
                                            TRANG_THAI = {0}, NGAY_CAPNHAT = SYSDATE, ID_NV_CAPNHAT = {1} 
                                            WHERE ID_DOTTIEM IN (
                                                SELECT DTVC.ID_DOTTIEM 
                                                FROM DOTTIEM_VACCINE DTVC START WITH DTVC.ID_DOTTIEM = :IdDotTiem CONNECT BY DTVC.TINH_TRANG = :TinhTrang AND PRIOR DTVC.ID_DOTTIEM = DTVC.ID_CHA)"
                                            , Constants.TrangThai_DotTiem.DongDangKy.GetHashCode(),
                                            account.UserId);
            await this.dbContext.ExecuteAsync(
                query,
                param: new { IdDotTiem = idDotTiem, TinhTrang = Constants.States.Actived.GetHashCode() },
                commandType: System.Data.CommandType.Text
            );
        }
    }
}

