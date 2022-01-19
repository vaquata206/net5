using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class LichSuTiemVaccineRepository : BaseRepository<LichSu_Tiem_Vaccine>, ILichSuTiemVaccineRepository
    {
        public LichSuTiemVaccineRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Luu danh sach ket qua tiem vao Db
        /// </summary>
        /// <param name="listKetQuaTiem">danh sach thong tin ket qua tiem</param>
        /// <returns></returns>
        public async Task LuuDanhSachKetQuaTiem(List<LichSu_Tiem_Vaccine> listKetQuaTiem)
        {
            try
            {
                int soLuong = listKetQuaTiem.Count;
                int[] dsIDDangKyTiem = new int[soLuong];
                DateTime?[] dsNgayTiem = new DateTime?[soLuong];
                string[] dsTenKeHoach = new string[soLuong];
                string[] dsTenDoiTiem = new string[soLuong];
                string[] dsDiaDiemTiem = new string[soLuong];
                string[] dsLoVaccine = new string[soLuong];
                string[] dsLoaiVaccine = new string[soLuong];
                int[] dsSoMuiDaTiem = new int[soLuong];
                DateTime?[] dsNgayNhapThongTin = new DateTime?[soLuong];
                DateTime?[] dsNgayKhoiTao = new DateTime?[soLuong];
                int[] dsIdNvKhoiTao = new int[soLuong];

                for (var i = 0; i < soLuong; i++)
                {
                    dsIDDangKyTiem[i] = listKetQuaTiem[i].Id_DangKyTiem;
                    dsNgayTiem[i] = listKetQuaTiem[i].Ngay_Tiem;
                    dsTenKeHoach[i] = listKetQuaTiem[i].Ten_KeHoach;
                    dsTenDoiTiem[i] = listKetQuaTiem[i].Ten_DoiTiem;
                    dsDiaDiemTiem[i] = listKetQuaTiem[i].DiaDiem_Tiem;
                    dsLoVaccine[i] = listKetQuaTiem[i].Lo_Vaccine;
                    dsLoaiVaccine[i] = listKetQuaTiem[i].Loai_Vaccine;
                    dsSoMuiDaTiem[i] = listKetQuaTiem[i].SoMui_DaTiem;
                    dsNgayNhapThongTin[i] = listKetQuaTiem[i].NgayNhap_ThongTin;
                    dsNgayKhoiTao[i] = listKetQuaTiem[i].Ngay_KhoiTao;
                    dsIdNvKhoiTao[i] = listKetQuaTiem[i].Id_NV_KhoiTao;
                }

                OracleParameter prIdDangKyTiem = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsIDDangKyTiem
                };

                OracleParameter prNgayTiem = new OracleParameter
                {
                    OracleDbType = OracleDbType.Date,
                    Value = dsNgayTiem
                };

                OracleParameter prTenKeHoach = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsTenKeHoach
                };

                OracleParameter prTenDoiTiem = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsTenDoiTiem
                };

                OracleParameter prDiaDiemTiem = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsDiaDiemTiem
                };

                OracleParameter prLoVaccine = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsLoVaccine
                };

                OracleParameter prLoaiVaccine = new OracleParameter
                {
                    OracleDbType = OracleDbType.Varchar2,
                    Value = dsLoaiVaccine
                };

                OracleParameter prSoMuiDaTiem = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsSoMuiDaTiem
                };

                OracleParameter prNgayNhapThongTin = new OracleParameter
                {
                    OracleDbType = OracleDbType.Date,
                    Value = dsNgayNhapThongTin
                };

                OracleParameter prIdNvKhoiTao = new OracleParameter
                {
                    OracleDbType = OracleDbType.Int32,
                    Value = dsIdNvKhoiTao
                };

                OracleParameter prNgayKhoiTao = new OracleParameter
                {
                    OracleDbType = OracleDbType.Date,
                    Value = dsNgayKhoiTao
                };

                OracleCommand cmd = (OracleCommand)this.dbContext.DbTransaction.Connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO LICHSU_TIEM_VACCINE ( 
                                        ID_LICHSU, ID_DANGKYTIEM, NGAY_TIEM, TEN_KEHOACH, TEN_DOITIEM, DIADIEM_TIEM, LO_VACCINE, LOAI_VACCINE, SOMUI_DATIEM, NGAYNHAP_THONGTIN, NGAY_KHOITAO, ID_NV_KHOITAO) 
                                    VALUES ( LICHSU_TIEM_VACCINE_SEQ.NEXTVAL, :1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11) ";
                cmd.ArrayBindCount = soLuong;
                cmd.Parameters.Add(prIdDangKyTiem);
                cmd.Parameters.Add(prNgayTiem);
                cmd.Parameters.Add(prTenKeHoach);
                cmd.Parameters.Add(prTenDoiTiem);
                cmd.Parameters.Add(prDiaDiemTiem);
                cmd.Parameters.Add(prLoVaccine);
                cmd.Parameters.Add(prLoaiVaccine);
                cmd.Parameters.Add(prSoMuiDaTiem);
                cmd.Parameters.Add(prNgayNhapThongTin);
                cmd.Parameters.Add(prNgayKhoiTao);
                cmd.Parameters.Add(prIdNvKhoiTao);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Lấy danh sách lịch sử tiêm vaccine theo id người đăng ký
        /// </summary>
        /// <param name="idNguoiDangKy">id người đăng ký tiêm</param>
        /// <returns>danh sách lịch sử tiêm</returns>
        public async Task<IEnumerable<LichSu_Tiem_Vaccine>> LayDsLichSuTiemTheoIdNguoiDangKy(int idNguoiDangKy)
        {
            string sql = "SELECT LS.* " +
                         "FROM LICHSU_TIEM_VACCINE LS " +
                                "JOIN THONGTIN_DANGKY_TIEM_VACCINE TTDK ON LS.ID_DANGKYTIEM = TTDK.ID_DANGKY AND TTDK.TINH_TRANG = :TinhTrang " +
                                "JOIN THONGTIN_NGUOIDAN TTND ON TTDK.ID_THONGTIN_NGUOIDAN = TTND.ID_THONGTIN AND TTND.TINH_TRANG = :TinhTrang " +
                         "WHERE LS.TINH_TRANG = :TinhTrang AND TTND.ID_THONGTIN = :IdThongTin ORDER BY LS.NGAY_TIEM DESC";
            var parameters = new
            {
                IdThongTin = idNguoiDangKy,
                TinhTrang = Constants.States.Actived.GetHashCode(),
            };
            var list = await this.dbContext.QueryAsync<LichSu_Tiem_Vaccine>(
                sql: sql,
                param: parameters);
            return list;
        }
    }
}
