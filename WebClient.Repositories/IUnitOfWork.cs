using System;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IFeatureRepository FeatureRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPermissionFeatureRepository PermissionFeatureRepository { get; }
        IEmployeePermissionRepository EmployeePermissionRepository { get; }
        IDangKyTiemVaccineRepository DangKyTiemVaccineRepository { get; }
        IDotTiemVaccineRepository DotTiemVaccineRepository { get; }
        ILichSuTiemVaccineRepository LichSuTiemVaccineRepository { get; }
        IDmDoiTuongUuTienRepository DmDoiTuongUuTienRepository { get; }
        IDmDanTocRepository DmDanTocRepository { get; }
        void Commit();
        void Rollback();
    }
}
