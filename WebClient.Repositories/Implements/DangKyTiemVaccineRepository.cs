using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DangKyTiemVaccineRepository : BaseRepository<ThongTin_NguoiDan>, IDangKyTiemVaccineRepository
    {
        public DangKyTiemVaccineRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Tìm kiếm đối tượng đăng ký tiêm vaccine
        /// </summary>
        /// <param name="pagingRequest">Paging request</param>
        /// <returns>Danh sách đối tượng đăng ký tiêm</returns>
        public async Task<PagingResponse<DoiTuongDangKyTiemInfo>> SearchAsync(PagingRequest<DoiTuongDangKyTiemFilterVM> pagingRequest)
        {
            var response = new PagingResponse<DoiTuongDangKyTiemInfo>
            {
                Draw = pagingRequest.Draw
            };

            var whereClause = new List<string>();
            whereClause.Add("ID_DONVI IN (select dvi.id_donvi from don_vi dvi start with dvi.id_donvi = :IdDonVi connect by dvi.tinh_trang = 1 and prior dvi.id_donvi = dvi.id_dv_cha)");

            if (!string.IsNullOrEmpty(pagingRequest.Filter.HoTen))
            {
                whereClause.Add("UPPER(HO_TEN) like '%'||:HoTen||'%'");
            }

            if (!string.IsNullOrEmpty(pagingRequest.Filter.Cmnd))
            {
                whereClause.Add("UPPER(CMND) = :Cmnd");
            }

            if (!string.IsNullOrEmpty(pagingRequest.Filter.NgaySinh))
            {
                whereClause.Add("TO_CHAR(NGAY_SINH, 'DD/MM/YYYY') = :NgaySinh");
            }

            if (pagingRequest.Filter.DsDoiTuongUuTien != null && pagingRequest.Filter.DsDoiTuongUuTien.Length > 0)
            {
                whereClause.Add(string.Format("ID_DOITUONG_UUTIEN IN ({0})", string.Join(',', pagingRequest.Filter.DsDoiTuongUuTien)));
            }

            if (!string.IsNullOrEmpty(pagingRequest.Filter.SoDienThoai))
            {
                whereClause.Add("UPPER(SO_DIENTHOAI) = :SoDienThoai");
            }

            if (pagingRequest.Filter.SoMuiDaTiem != -1)
            {
                whereClause.Add("SOMUIDATIEM = :SoMuiDaTiem");
            }

            if (pagingRequest.Filter.GioiTinh != 0)
            {
                whereClause.Add("Gioi_Tinh = :GioiTinh");
            }

            var pagingString = string.Empty;
            if (pagingRequest.Length > 0)
            {
                pagingString = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pagingRequest.Start, pagingRequest.Length);
            }

            var parameters = new
            {
                IdDonVi = pagingRequest.Filter.IdDonVi,
                HoTen = pagingRequest.Filter.HoTen?.Trim()?.ToUpper(),
                Cmnd = pagingRequest.Filter.Cmnd?.Trim()?.ToUpper(),
                SoDienThoai = pagingRequest.Filter.SoDienThoai?.Trim()?.ToUpper(),
                SoMuiDaTiem = pagingRequest.Filter.SoMuiDaTiem,
                GioiTinh = pagingRequest.Filter.GioiTinh,
                NgaySinh = pagingRequest.Filter.NgaySinh?.Trim()
            };

            var whereString = string.Join(" and ", whereClause);
            var sql = string.Format(@"SELECT *
                    FROM (SELECT THONGTIN_NGUOIDAN.ID_DONVI, ID_THONGTIN, HO_TEN, GIOI_TINH, NGAY_SINH, SO_DIENTHOAI, CMND, DIACHI_HIENTAI, ID_DOITUONG_UUTIEN, DTUT.MA_DOITUONG MA_DOITUONG_UUTIEN, DTUT.NGHE_NGHIEP NGHENGHIEPUUTIEN,
                    NVL((SELECT MAX(lst.somui_datiem) FROM lichsu_tiem_vaccine lst JOIN thongtin_dangky_tiem_vaccine ttdk ON lst.id_dangkytiem = ttdk.id_dangky
                    WHERE ttdk.tinh_trang = 1 and lst.tinh_trang = 1 and ttdk.id_thongtin_nguoidan = thongtin_nguoidan.id_thongtin), THONGTIN_NGUOIDAN.SOMUIDATIEM) SoMuiDaTiem, DV.TEN_DONVI PHUONGXA
                    FROM THONGTIN_NGUOIDAN LEFT JOIN DM_DOITUONG_UUTIEN DTUT ON THONGTIN_NGUOIDAN.ID_DOITUONG_UUTIEN = DTUT.ID_DOITUONG
                    LEFT JOIN DON_VI DV ON DV.ID_DONVI = THONGTIN_NGUOIDAN.ID_DONVI
                    WHERE THONGTIN_NGUOIDAN.TINH_TRANG = 1) WHERE {0} ORDER BY PHUONGXA, HO_TEN DESC {1}", whereString, pagingString);
            var list = await this.dbContext.QueryAsync<DoiTuongDangKyTiemInfo>(
                sql: sql,
                param: parameters);

            var total = await this.dbContext.QueryFirstOrDefaultAsync<int>(
                sql: string.Format("SELECT count(1) FROM THONGTIN_NGUOIDAN " +
                                    "WHERE {0}", whereString),
                param: parameters
                );

            response.Data = list;
            response.RecordsTotal = total;
            response.RecordsFiltered = total;

            return response;
        }

        /// <summary>
        /// Lay danh sach doi tuong dang ky tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>List ThongTin_NguoiDan</returns>
        public async Task<IEnumerable<ThongTin_NguoiDanVM>> LayDsDoiTuongDangKyTiemTheoIdDotTiem(int id_DotTiem)
        {
            var query = @"SELECT
                            TTND.*,
                            (SELECT COUNT(1)
                                FROM LICHSU_TIEM_VACCINE
                                WHERE LICHSU_TIEM_VACCINE.ID_DANGKYTIEM = TTDKTVC.ID_DANGKY
                                    AND LICHSU_TIEM_VACCINE.TINH_TRANG = 1) AS TRANG_THAI,
                            (SELECT *
                            FROM
                                (SELECT LICHSU_TIEM_VACCINE.SOMUI_DATIEM
                                FROM LICHSU_TIEM_VACCINE
                                WHERE LICHSU_TIEM_VACCINE.ID_DANGKYTIEM = TTDKTVC.ID_DANGKY
                                    AND LICHSU_TIEM_VACCINE.TINH_TRANG = 1
                                ORDER BY LICHSU_TIEM_VACCINE.ID_LICHSU)
                            WHERE ROWNUM = 1) AS TINHTRANG_TIEMCHUNG
                        FROM THONGTIN_NGUOIDAN TTND
                        JOIN THONGTIN_DANGKY_TIEM_VACCINE TTDKTVC ON TTND.ID_THONGTIN = TTDKTVC.ID_THONGTIN_NGUOIDAN
                        JOIN DOTTIEM_VACCINE DTVC ON TTDKTVC.ID_DOTTIEM_VACCINE = DTVC.ID_DOTTIEM
                        WHERE TTND.TINH_TRANG = 1
                            AND TTDKTVC.TINH_TRANG = 1
                            AND DTVC.TINH_TRANG = 1
                            AND (DTVC.ID_DOTTIEM = :id_dottiem OR DTVC.ID_CHA = :id_dottiem)";

            var response = await this.dbContext.QueryAsync<ThongTin_NguoiDanVM>(
                query,
                param: new { id_dottiem = id_DotTiem },
                commandType: System.Data.CommandType.Text
            );

            return response;
        }

        /// <summary>
        /// Lay danh sach CMND cua doi tuong dang ky tiem vaccine theo id_dottiem
        /// </summary>
        /// <param name="id_DotTiem">id DotTiem_Vaccine</param>
        /// <returns>List ThongTinDangKyTiemVM</returns>
        public async Task<IEnumerable<ThongTinDangKyTiemVM>> LayDsCMNDCuaDoiTuongDangKyTheoIdDotTiem(int id_DotTiem)
        {
            var query = @"SELECT 
                            DKTV.ID_DANGKY, 
                            TTND.CMND,
                            TTND.ID_DONVI
                        FROM THONGTIN_NGUOIDAN TTND
                        JOIN THONGTIN_DANGKY_TIEM_VACCINE DKTV
                            ON TTND.ID_THONGTIN = DKTV.ID_THONGTIN_NGUOIDAN
                        JOIN DOTTIEM_VACCINE DTV
                            ON DKTV.ID_DOTTIEM_VACCINE = DTV.ID_DOTTIEM
                        WHERE TTND.TINH_TRANG = 1
                            AND DKTV.TINH_TRANG = 1
                            AND DTV.TINH_TRANG = 1
                            AND (DTV.ID_DOTTIEM = :id_dottiem OR DTV.ID_CHA = :id_dottiem)
                            AND DKTV.ID_DANGKY NOT IN (
                                SELECT LICHSU_TIEM_VACCINE.ID_DANGKYTIEM 
                                FROM LICHSU_TIEM_VACCINE 
                                WHERE LICHSU_TIEM_VACCINE.TINH_TRANG = 1)";

            var response = await this.dbContext.QueryAsync<ThongTinDangKyTiemVM>(
                query,
                param: new { id_dottiem = id_DotTiem },
                commandType: System.Data.CommandType.Text
            );

            return response;
        }

        /// <summary>
        /// Lay lich su tiem vaccine
        /// </summary>
        /// <param name="muitiem">mui tiem</param>
        /// <param name="idThongTin">Id thong tin nguoi dang ky tiem</param>
        /// <returns>LichSuTiemVaccineVM</returns>
        public async Task<LichSuTiemVaccineVM> LayLichSuTiemVaccine(int muiTiem, int idThongTin)
        {
            var query = @"SELECT LST.*
                        FROM LichSu_Tiem_Vaccine LST
                        JOIN THONGTIN_DANGKY_TIEM_VACCINE TTDK ON TTDK.ID_DANGKY = LST.ID_DANGKYTIEM
                        WHERE LST.Tinh_Trang = 1 AND TTDK.TINH_TRANG = 1 AND LST.SOMUI_DATIEM = :muiTiem AND TTDK.ID_THONGTIN_NGUOIDAN = :idThongTin";

            var response = await this.dbContext.QueryFirstOrDefaultAsync<LichSuTiemVaccineVM>(
                query,
                param: new { muiTiem, idThongTin },
                commandType: System.Data.CommandType.Text
            );

            return response;
        }

        /// <summary>
        /// Thong ke tong quat so luong lieu vaccine, so nguoi dang ky, so nguoi da tiem  mui 1, 2
        /// </summary>
        /// <param name="id_DonVi">id don vi thong ke</param>
        /// <returns></returns>
        public async Task<ThongKeTongQuatVM> ThongKeTongQuatSoLuong(int id_DonVi)
        {
            var query = @"SELECT 
                            TONGSOLIEUVACCINE,
                            TONGSONGUOIDANGKY,
                            TONGSONGUOIDATIEMMUI1,
                            TONGSONGUOIDATIEMMUI2
                        FROM 
                            (SELECT SUM(SOLUONG_DANGKY) AS TONGSOLIEUVACCINE
                            FROM DOTTIEM_VACCINE 
                            WHERE TINH_TRANG = 1
                            AND ID_DONVI = :id_donvi
                            ),
                            (SELECT COUNT(1) AS TONGSONGUOIDANGKY
                            FROM THONGTIN_NGUOIDAN TTND
                            JOIN DON_VI DV
                            ON TTND.ID_DONVI = DV.ID_DONVI
                            WHERE TTND.TINH_TRANG = 1
                            AND DV.TINH_TRANG = 1 
                            AND (TTND.ID_DONVI = :id_donvi OR DV.ID_DV_CHA = :id_donvi)
                            ),
                            (SELECT 
                                SUM(CASE WHEN SOMUIDATIEM = 1 THEN 1 ELSE 0 END) AS TONGSONGUOIDATIEMMUI1,
                                SUM(CASE WHEN SOMUIDATIEM = 2 THEN 1 ELSE 0 END) AS TONGSONGUOIDATIEMMUI2
                            FROM (SELECT 
                                    TTND.ID_THONGTIN, 
                                    TTND.ID_DONVI,
                                    NVL(MAX(LSTV.SOMUI_DATIEM), TTND.SOMUIDATIEM) AS SOMUIDATIEM
                                FROM THONGTIN_NGUOIDAN TTND
                                JOIN DON_VI DV
                                    ON TTND.ID_DONVI = DV.ID_DONVI
                                    AND DV.TINH_TRANG = 1
                                LEFT OUTER JOIN THONGTIN_DANGKY_TIEM_VACCINE DKTV
                                    ON TTND.ID_THONGTIN = DKTV.ID_THONGTIN_NGUOIDAN
                                    AND DKTV.TINH_TRANG = 1
                                LEFT OUTER JOIN LICHSU_TIEM_VACCINE LSTV
                                    ON DKTV.ID_DANGKY = LSTV.ID_DANGKYTIEM
                                    AND LSTV.TINH_TRANG = 1
                                WHERE TTND.TINH_TRANG = 1
                                AND (DV.ID_DONVI = :id_donvi OR DV.ID_DV_CHA = :id_donvi)
                                GROUP BY TTND.ID_THONGTIN, TTND.SOMUIDATIEM, TTND.ID_DONVI)
                            ) ";

            var response = await this.dbContext.QueryFirstOrDefaultAsync<ThongKeTongQuatVM>(
                query,
                param: new { id_DonVi = id_DonVi },
                commandType: System.Data.CommandType.Text
            );

            return response;
        }

        public async Task<int> GetTongSoMuiDaTiemVaccine(int id)
        {
            var query = @"SELECT 
                            NVL(MAX(ls.somui_datiem), tt.somuidatiem)
                        FROM thongtin_nguoidan TT
                        LEFT JOIN thongtin_dangky_tiem_vaccine DK ON dk.id_thongtin_nguoidan = tt.id_thongtin AND dk.tinh_trang = 1
                        LEFT JOIN lichsu_tiem_vaccine LS ON ls.id_dangkytiem = dk.id_dangky AND ls.tinh_trang = 1
                        WHERE TT.id_thongtin = :id 
                        AND TT.Tinh_Trang = 1
                        GROUP BY TT.id_thongtin, tt.somuidatiem";

            return await this.dbContext.QueryFirstOrDefaultAsync<int>(
                query,
                param: new { id = id },
                commandType: System.Data.CommandType.Text
            );
        }

        /// <summary>
        /// Lấy danh sách người dân
        /// </summary>
        /// <returns>Danh sách người dân</returns>
        public async Task<IEnumerable<ThongTin_NguoiDan>> GetAllAsync()
        {
            var list = await this.dbContext.QueryAsync<ThongTin_NguoiDan>(
             sql: string.Format("SELECT * FROM THONGTIN_NGUOIDAN WHERE TINH_TRANG = 1 ORDER BY THONGTIN_NGUOIDAN.NGAY_KHOITAO DESC"));
            return list;
        }

        /// <summary>
        /// Lưu danh sách đăng ký tiêm vaccine
        /// </summary>
        /// <param name="listThongTin_NguoiDan">Danh sách đăng ký tiêm vaccine</param>
        /// <returns></returns>
        public async Task DongBoDSDKTiemFromExcel(List<ThongTin_NguoiDan> listThongTin_NguoiDan)
        {
            try
            {
                int soLuong = listThongTin_NguoiDan.Count;
                string[] dsHo_Ten = new string[soLuong];
                int[] dsGioi_Tinh = new int[soLuong];
                DateTime?[] dsNgay_Sinh = new DateTime?[soLuong];
                string[] dsEmail = new string[soLuong];
                string[] dsNghe_Nghiep = new string[soLuong];
                string[] dsDonVi_CongTac = new string[soLuong];
                string[] dsSo_DienThoai = new string[soLuong];
                string[] dsCMND = new string[soLuong];
                string[] dsSoThe_BHYT = new string[soLuong];
                int[] dsId_DoiTuong_UuTien = new int[soLuong];
                int[] dsId_DanToc = new int[soLuong];
                string[] dsQuoc_Tich = new string[soLuong];
                int[] dsId_DonVi = new int[soLuong];
                string[] dsDiaChi_HienTai = new string[soLuong];
                int[] dsSoMuiDaTiem = new int[soLuong];
                DateTime?[] dsNgay_CapNhat = new DateTime?[soLuong];
                int?[] dsId_NV_CapNhat = new int?[soLuong];
                DateTime?[] dsNgay_KhoiTao = new DateTime?[soLuong];
                int[] dsId_NV_KhoiTao = new int[soLuong];
                string[] dsGhi_Chu = new string[soLuong];
                int[] dsTinh_Trang = new int[soLuong];

                for (var i = 0; i < soLuong; i++)
                {
                    dsHo_Ten[i] = listThongTin_NguoiDan[i].Ho_Ten;
                    dsGioi_Tinh[i] = listThongTin_NguoiDan[i].Gioi_Tinh;
                    dsNgay_Sinh[i] = listThongTin_NguoiDan[i].Ngay_Sinh;
                    dsEmail[i] = listThongTin_NguoiDan[i].Email;
                    dsNghe_Nghiep[i] = listThongTin_NguoiDan[i].Nghe_Nghiep;
                    dsDonVi_CongTac[i] = listThongTin_NguoiDan[i].DonVi_CongTac;
                    dsSo_DienThoai[i] = listThongTin_NguoiDan[i].So_DienThoai;
                    dsCMND[i] = listThongTin_NguoiDan[i].CMND;
                    dsSoThe_BHYT[i] = listThongTin_NguoiDan[i].SoThe_BHYT;
                    dsId_DoiTuong_UuTien[i] = listThongTin_NguoiDan[i].Id_DoiTuong_UuTien;
                    dsId_DanToc[i] = listThongTin_NguoiDan[i].Id_DanToc;
                    dsQuoc_Tich[i] = listThongTin_NguoiDan[i].Quoc_Tich;
                    dsId_DonVi[i] = listThongTin_NguoiDan[i].Id_DonVi;
                    dsDiaChi_HienTai[i] = listThongTin_NguoiDan[i].DiaChi_HienTai;
                    dsSoMuiDaTiem[i] = listThongTin_NguoiDan[i].SoMuiDaTiem;
                    dsNgay_CapNhat[i] = listThongTin_NguoiDan[i].Ngay_CapNhat;
                    dsId_NV_CapNhat[i] = listThongTin_NguoiDan[i].Id_NV_CapNhat;
                    dsNgay_KhoiTao[i] = listThongTin_NguoiDan[i].Ngay_KhoiTao;
                    dsId_NV_KhoiTao[i] = listThongTin_NguoiDan[i].Id_NV_KhoiTao;
                    dsGhi_Chu[i] = listThongTin_NguoiDan[i].Ghi_Chu;
                    dsTinh_Trang[i] = listThongTin_NguoiDan[i].Tinh_Trang;
                }

                OracleParameter prHo_Ten = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsHo_Ten
                };

                OracleParameter prGioi_Tinh = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsGioi_Tinh
                };

                OracleParameter prNgay_Sinh = new OracleParameter
                {
                    OracleDbType = OracleDbType.Date,
                    Value = dsNgay_Sinh
                };

                OracleParameter prEmail = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsEmail
                };

                OracleParameter prNghe_Nghiep = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsNghe_Nghiep
                };

                OracleParameter prDonVi_CongTac = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsDonVi_CongTac
                };

                OracleParameter prSo_DienThoai = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsSo_DienThoai
                };

                OracleParameter prCMND = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsCMND
                };

                OracleParameter prSoThe_BHYT = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsSoThe_BHYT
                };

                OracleParameter prId_DoiTuong_UuTien = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsId_DoiTuong_UuTien
                };

                OracleParameter prId_DanToc = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsId_DanToc
                };

                OracleParameter prQuoc_Tich = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsQuoc_Tich
                };

                OracleParameter prId_DonVi = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsId_DonVi
                };

                OracleParameter prDiaChi_HienTai = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsDiaChi_HienTai
                };

                OracleParameter prSoMuiDaTiem = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsSoMuiDaTiem
                };

                OracleParameter prNgay_CapNhat = new OracleParameter
                {
                    OracleDbType = OracleDbType.Date,
                    Value = dsNgay_CapNhat
                };

                OracleParameter prId_NV_CapNhat = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsId_NV_CapNhat
                };

                OracleParameter prNgay_KhoiTao = new OracleParameter
                {
                    OracleDbType = OracleDbType.Date,
                    Value = dsNgay_KhoiTao
                };

                OracleParameter prId_NV_KhoiTao = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsId_NV_KhoiTao
                };

                OracleParameter prGhi_Chu = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsGhi_Chu
                };

                OracleParameter prTinh_Trang = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsTinh_Trang
                };

                OracleCommand cmd = (OracleCommand)this.dbContext.DbTransaction.Connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO THONGTIN_NGUOIDAN ( 
                                        ID_THONGTIN, HO_TEN, GIOI_TINH, NGAY_SINH, EMAIL, NGHE_NGHIEP, DONVI_CONGTAC, SO_DIENTHOAI, CMND, SOTHE_BHYT, ID_DOITUONG_UUTIEN, ID_DANTOC, QUOC_TICH, ID_DONVI, DIACHI_HIENTAI,  SOMUIDATIEM, NGAY_CAPNHAT, ID_NV_CAPNHAT, NGAY_KHOITAO, ID_NV_KHOITAO, GHI_CHU, TINH_TRANG) 
                                    VALUES ( THONGTIN_NGUOIDAN_SEQ.NEXTVAL, :1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14, :15, :16, :17, :18, :19, :20, :21) ";
                cmd.ArrayBindCount = soLuong;
                cmd.Parameters.Add(prHo_Ten);
                cmd.Parameters.Add(prGioi_Tinh);
                cmd.Parameters.Add(prNgay_Sinh);
                cmd.Parameters.Add(prEmail);
                cmd.Parameters.Add(prNghe_Nghiep);
                cmd.Parameters.Add(prDonVi_CongTac);
                cmd.Parameters.Add(prSo_DienThoai);
                cmd.Parameters.Add(prCMND);
                cmd.Parameters.Add(prSoThe_BHYT);
                cmd.Parameters.Add(prId_DoiTuong_UuTien);
                cmd.Parameters.Add(prId_DanToc);
                cmd.Parameters.Add(prQuoc_Tich);
                cmd.Parameters.Add(prId_DonVi);
                cmd.Parameters.Add(prDiaChi_HienTai);
                cmd.Parameters.Add(prSoMuiDaTiem);
                cmd.Parameters.Add(prNgay_CapNhat);
                cmd.Parameters.Add(prId_NV_CapNhat);
                cmd.Parameters.Add(prNgay_KhoiTao);
                cmd.Parameters.Add(prId_NV_KhoiTao);
                cmd.Parameters.Add(prGhi_Chu);
                cmd.Parameters.Add(prTinh_Trang);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
