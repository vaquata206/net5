using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using WebClient.Core.Entities;
using WebClient.Core.Helpers;
using WebClient.Core.Models;
using WebClient.Core.ViewModels;

namespace WebClient.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // department
            this.CreateMap<Department, DepartmentVM>();
            this.CreateMap<DepartmentVM, Department>();

            //employee
            this.CreateMap<Employee, EmployeeVM>()
               .ForMember(dest => dest.MaNhanVien, opts => opts.MapFrom(src => src.Ma_NhanVien))
               .ForMember(dest => dest.HoTen, opts => opts.MapFrom(src => src.Ho_Ten))
               .ForMember(dest => dest.DiaChi, opts => opts.MapFrom(src => src.Dia_Chi))
               .ForMember(dest => dest.DienThoai, opts => opts.MapFrom(src => src.Dien_Thoai))
               .ForMember(dest => dest.NamSinh, opts => opts.MapFrom(src => src.Nam_Sinh.HasValue ? src.Nam_Sinh.Value.ToString(Constants.FormatDate) : string.Empty))
               .ForMember(dest => dest.SoCMND, opts => opts.MapFrom(src => src.So_CMND))
               .ForMember(dest => dest.NgayCapCMND, opts => opts.MapFrom(src => src.NgayCap_CMND.HasValue ? src.NgayCap_CMND.Value.ToString(Constants.FormatDate) : string.Empty))
               .ForMember(dest => dest.NoiCapCMND, opts => opts.MapFrom(src => src.NoiCap_CMND))
               .ForMember(dest => dest.IdDonVi, opts => opts.MapFrom(src => src.Id_DonVi))
               .ForMember(dest => dest.GhiChu, opts => opts.MapFrom(src => src.Ghi_Chu));

            this.CreateMap<EmployeeVM, Employee>()
                .ForMember(dest => dest.Ma_NhanVien, opts => opts.MapFrom(src => src.MaNhanVien))
                .ForMember(dest => dest.Ho_Ten, opts => opts.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.Dia_Chi, opts => opts.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.Dien_Thoai, opts => opts.MapFrom(src => src.DienThoai))
                .ForMember(dest => dest.Nam_Sinh, opts => opts.MapFrom(src => DateTime.ParseExact(src.NamSinh, Constants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.So_CMND, opts => opts.MapFrom(src => src.SoCMND))
                .ForMember(dest => dest.NgayCap_CMND, opts => opts.MapFrom(src => DateTime.ParseExact(src.NgayCapCMND, Constants.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.NoiCap_CMND, opts => opts.MapFrom(src => src.NoiCapCMND))
                .ForMember(dest => dest.Id_DonVi, opts => opts.MapFrom(src => src.IdDonVi))
                .ForMember(dest => dest.Ghi_Chu, opts => opts.MapFrom(src => src.GhiChu));

            // danh mục đối tượng ưu tiên
            this.CreateMap<DmDoiTuongUuTien, DmDoiTuongUuTienInfo>();
            this.CreateMap<DmDoiTuongUuTienInfo, DmDoiTuongUuTien>();

            // Đợt tiêm vaccine
            this.CreateMap<DotTiemVaccine, DotTiem_VaccineVM>();
            this.CreateMap<DotTiemVaccine, DotTiemVaccineVM>();
            this.CreateMap<DotTiemVaccineVM, DotTiemVaccine>();

            // danh mục dân tộc
            this.CreateMap<DmDanToc, DmDanTocInfo>();
            this.CreateMap<DmDanTocInfo, DmDanToc>();

            // thông tin người dân
            this.CreateMap<ThongTin_NguoiDan, ThongTinNguoiDanVM>();
            this.CreateMap<ThongTinNguoiDanVM, ThongTin_NguoiDan>()
                .ForMember(dest => dest.Ngay_Sinh, opt => opt.MapFrom(src => DateTime.ParseExact(src.Ngay_Sinh, Constants.FormatDate, CultureInfo.InvariantCulture)));

            this.CreateMap<LichSuTiemVaccineVM, LichSu_Tiem_Vaccine>();
        }
    }
}
