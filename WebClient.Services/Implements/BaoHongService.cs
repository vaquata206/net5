using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class BaoHongService : IBaoHongService
    {
        private readonly IUnitOfWork unitOfWork;

        public BaoHongService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrangThaiPhieu>> GetTrangThai()
        {
            return await this.unitOfWork.BaoHongRepository.GetTrangThai();
        }
        public async Task<IEnumerable<BaoHongInfo>> SearchAsync(BaoHongSearch search)
        {
            return await this.unitOfWork.BaoHongRepository.SearchAsync(search);
        }
        public async Task<IEnumerable<DichVu>> GetDichVuByKhachHangId(int idKhachHang)
        {
            return await this.unitOfWork.BaoHongRepository.GetDichVuByKhachHangId(idKhachHang);
        }
        public async Task<PhieuBaoHong> GetByIdAsync(int id)
        {
            return await this.unitOfWork.BaoHongRepository.GetByIdAsync(id);
        }

        public async Task<DichVuKhachHang> GetDichVuKhachHangByIdAsync(int idDichVuKhachHang)
        {
            return await this.unitOfWork.BaoHongRepository.GetDichVuKhachHangByIdAsync(idDichVuKhachHang);
        }

        public async Task<IEnumerable<NhanVien>> GetNhanVienKyThuat()
        {
            return await this.unitOfWork.NhanVienRepository.GetNhanVienKyThuat();
        }

        public async Task SaveAsync(BaoHongVM viewModal, int idKhachHang)
        {
            var dichVuKhachHang = await this.unitOfWork.BaoHongRepository.GetDichVuKhachHang(idKhachHang, viewModal.IdDichVu);
            var trangThai = await this.unitOfWork.BaoHongRepository.GetTrangThai();
            if (dichVuKhachHang == null)
            {
                throw new Exception("Khách hàng không sử dụng dịch vụ này");
            }

            if (viewModal.Id == 0)
            {
                var entiry = await this.unitOfWork.BaoHongRepository.AddAsync(new PhieuBaoHong
                {
                    IdDichVuKhachHang = dichVuKhachHang.Id,
                    IdTrangThaiPhieu = trangThai.ElementAt(0).Id,
                    MoTa = viewModal.MoTa,
                    TinhTrang = 0,
                    DaXoa = false,
                    NgayKhoiTao = DateTime.Now
                });
            }
            else
            {
                var entity = await this.unitOfWork.BaoHongRepository.GetByIdAsync(viewModal.Id);
                entity.IdDichVuKhachHang = dichVuKhachHang.Id;
                entity.MoTa = viewModal.MoTa;
                await this.unitOfWork.BaoHongRepository.UpdateAsync(entity);
            }

            this.unitOfWork.Commit();
        }

        public async Task TiepNhan(int id, int idNhanVien)
        {
            var baohong = await this.unitOfWork.BaoHongRepository.GetByIdAsync(id);
            baohong.IdTrangThaiPhieu = 2;
            await this.unitOfWork.BaoHongRepository.UpdateAsync(baohong);
            var chitiet = new ChiTietPhieuBaoHong
            {
                IdNhanVien = idNhanVien,
                IdPhieuBaoHong = id,
                IdTrangThaiPhieu = 2,
                ThoiGian = DateTime.Now
            };
            await this.unitOfWork.BaoHongRepository.AddAsync(chitiet);
            this.unitOfWork.Commit();
        }

        public async Task ChuyenKyThuat(ChuyenKyThuatVM viewModal)
        {
            var baohong = await this.unitOfWork.BaoHongRepository.GetByIdAsync(viewModal.Id);
            baohong.IdTrangThaiPhieu = 3;
            await this.unitOfWork.BaoHongRepository.UpdateAsync(baohong);
            var chitiet = new ChiTietPhieuBaoHong
            {
                IdNhanVien = viewModal.IdNVKyThuat,
                IdPhieuBaoHong = viewModal.Id,
                IdTrangThaiPhieu = 3,
                ThoiGian = DateTime.Now
            };
            await this.unitOfWork.BaoHongRepository.AddAsync(chitiet);
            this.unitOfWork.Commit();
        }

        public async Task<IEnumerable<ChiTietPhieuBaoHong>> GetChiTietBaoHong(int id)
        {
            return await this.unitOfWork.BaoHongRepository.GetChiTietBaoHong(id);
        }

        public async Task HoanThanh(int id, int idNhanVien)
        {
            var baohong = await this.unitOfWork.BaoHongRepository.GetByIdAsync(id);
            var chitietphieu = await this.GetChiTietBaoHong(id);
            var ctkt = chitietphieu.LastOrDefault(x => x.IdTrangThaiPhieu == 3);
            baohong.IdTrangThaiPhieu = 4;
            baohong.TinhTrang = (DateTime.Now - ctkt.ThoiGian) > TimeSpan.FromHours(2) ? 2 : 1;
            await this.unitOfWork.BaoHongRepository.UpdateAsync(baohong);
            var chitiet = new ChiTietPhieuBaoHong
            {
                IdNhanVien = idNhanVien,
                IdPhieuBaoHong = id,
                IdTrangThaiPhieu = 4,
                ThoiGian = DateTime.Now
            };
            await this.unitOfWork.BaoHongRepository.AddAsync(chitiet);
            this.unitOfWork.Commit();
        }

        public async Task GuiDanhGia(DanhGiaVM danhGiaVM)
        {
            var baohong = await this.unitOfWork.BaoHongRepository.GetByIdAsync(danhGiaVM.Id);
            baohong.DiemDanhGia = danhGiaVM.DiemDanhGia;
            baohong.NoiDungDanhGia = danhGiaVM.NoiDungDanhGia;
            await this.unitOfWork.BaoHongRepository.UpdateAsync(baohong);
            this.unitOfWork.Commit();
        }
    }
}
