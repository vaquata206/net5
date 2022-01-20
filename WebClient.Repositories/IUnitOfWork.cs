using System;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }
        IDonViRepository DepartmentRepository { get; }
        IFeatureRepository FeatureRepository { get; }
        INhanVienRepository NhanVienRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPermissionFeatureRepository PermissionFeatureRepository { get; }
        IEmployeePermissionRepository EmployeePermissionRepository { get; }
        IBaoHongRepository BaoHongRepository { get; }
        void Commit();
        void Rollback();
    }
}
