using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class BaoHongRepository : BaseRepository<PhieuBaoHong>, IBaoHongRepository
    {
        public BaoHongRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<TrangThaiPhieu>> GetTrangThai()
        {
            return await this.dbContext.QueryAsync<TrangThaiPhieu>(sql: "SELECT * FROM TRANGTHAIPHIEU WHERE DAXOA = 0");
        }

        public async Task<IEnumerable<DichVu>> GetDichVuByKhachHangId(int idKhachHang)
        {
            return await this.dbContext.QueryAsync<DichVu>(sql: @"SELECT DV.* FROM DICHVU DV
                                JOIN DICHVUKHACHHANG DVKH ON DVKH.IDDICHVU = DV.ID
                                WHERE DVKH.IDKHACHHANG = @idKhachHang AND DV.DAXOA = 0",
                                new { 
                                    idKhachHang
                                });
        }

        public async Task<DichVuKhachHang> GetDichVuKhachHang(int idKhachHang, int idDichVu)
        {
            return await this.dbContext.QueryFirstOrDefaultAsync<DichVuKhachHang>(sql: @"SELECT * FROM DichVuKhachHang 
                                WHERE IdKhachHang = @idKhachHang AND IdDichVu = @idDichVu AND DaXoa = 0",
                                new
                                {
                                    idKhachHang,
                                    idDichVu
                                });
        }

        public async Task<DichVuKhachHang> GetDichVuKhachHangByIdAsync(int idDichVuKhachHang)
        {
            return await this.dbContext.QueryFirstOrDefaultAsync<DichVuKhachHang>(sql: @"SELECT * FROM DichVuKhachHang 
                                WHERE Id = @idDichVuKhachHang",
                                new
                                {
                                    idDichVuKhachHang,
                                });
        }

        public async Task<IEnumerable<ChiTietPhieuBaoHong>> GetChiTietBaoHong(int id)
        {
            var sql = "SELECT * FROM ChiTietPhieuBaoHong WHERE IdPhieuBaoHong = @id ORDER BY ThoiGian";
            return await this.dbContext.QueryAsync<ChiTietPhieuBaoHong>(
                sql: sql,
                param: new
                {
                    id
                }
                );
        }


        public async Task<IEnumerable<BaoHongInfo>> SearchAsync(BaoHongSearch search)
        {
            var whereClause = new List<string>();
            DateTime? tuNgay = null;
            if (!string.IsNullOrEmpty(search.TuNgay))
            {
                tuNgay = DateTime.ParseExact(search.TuNgay, Constants.FormatDate, CultureInfo.InvariantCulture);
                whereClause.Add("pbh.NgayKhoiTao >= @TuNgay");
            }

            DateTime? denNgay = null;
            if (!string.IsNullOrEmpty(search.DenNgay))
            {
                denNgay = DateTime.ParseExact(search.DenNgay, Constants.FormatDate, CultureInfo.InvariantCulture).AddDays(1);
                whereClause.Add("pbh.NgayKhoiTao < @DenNgay");
            }

            if (search.TrangThai > 0)
            {
                whereClause.Add("pbh.IdTrangThaiPhieu = @TrangThai");
            }

            if (search.IdKhachHang.HasValue)
            {
                whereClause.Add("dvkh.IdKhachHang = @IdKhachHang");
            }

            whereClause.Add("pbh.daxoa = 0");
            var whereString = string.Join(" AND ", whereClause);
            var list = await this.dbContext.QueryAsync<BaoHongInfo>(
                sql: string.Format(@"SELECT PBH.ID, DV.TEN DICHVU, DVKH.IDDICHVU, PBH.NGAYKHOITAO, PBH.IDTRANGTHAIPHIEU, TTP.TEN TRANGTHAI, PBH.DIEMDANHGIA, DVKH.IDKHACHHANG FROM PHIEUBAOHONG PBH
                                    JOIN DICHVUKHACHHANG DVKH ON DVKH.ID = PBH.IDDICHVUKHACHHANG
                                    JOIN DICHVU DV ON DVKH.IDDICHVU = DV.ID
                                    JOIN TRANGTHAIPHIEU TTP ON TTP.ID = PBH.IDTRANGTHAIPHIEU
                                    WHERE {0} ORDER BY NGAYKHOITAO DESC", whereString),
                param: new { 
                    search.IdKhachHang,
                    TuNgay = tuNgay,
                    DenNgay = denNgay,
                    search.TrangThai
                });

            return list;
        }
    }
}
