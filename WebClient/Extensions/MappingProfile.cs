using System;
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
            this.CreateMap<DonVi, DepartmentVM>();
            this.CreateMap<DepartmentVM, DonVi>();

            //account
            this.CreateMap<Account, AccountInfo>();
            this.CreateMap<AccountInfo, Account>();

            //TimKiemMarkerMobile
            this.CreateMap<TimKiemMarkerInFo, TimKiemMarkerMobile>();
        }
    }
}
