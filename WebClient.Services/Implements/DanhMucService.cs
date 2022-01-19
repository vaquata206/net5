using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Models;
using WebClient.Repositories;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class DanhMucService : IDanhMucService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DanhMucService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Lấy tất cả danh sách đối tượng ưu tiên
        /// </summary>
        /// <returns>danh sách đối tượng ưu tiên</returns>
        public async Task<IEnumerable<DmDoiTuongUuTienInfo>> GetAllDmDoiTuongUuTienAsync()
        {
            try
            {
                var dsDoiTuongUuTien = await this.unitOfWork.DmDoiTuongUuTienRepository.GetAllAsync();
                var ketQua = mapper.Map<IEnumerable<DmDoiTuongUuTien>, IEnumerable<DmDoiTuongUuTienInfo>>(dsDoiTuongUuTien);
                return ketQua;
            }
            catch
            {
                throw new Exception("Lấy danh sách đối tượng không thành công");
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách dân tộc
        /// </summary>
        /// <returns>danh sách dân tộc</returns>
        public async Task<IEnumerable<DmDanTocInfo>> GetAllDmDanTocAsync()
        {
            try
            {
                var dsDanToc = await this.unitOfWork.DmDanTocRepository.GetAllAsync();
                var ketQua = mapper.Map<IEnumerable<DmDanToc>, IEnumerable<DmDanTocInfo>>(dsDanToc);
                return ketQua;
            }
            catch
            {
                throw new Exception("Lấy danh sách đối tượng không thành công");
            }
        }
    }
}
