using System;
using WebClient.Core;
using WebClient.Core.Entities;
using WebClient.Repositories.Implements;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private IAccountRepository accountRepository;
        private IDonViRepository departmentRepository;
        private IFeatureRepository featureRepository;
        private INhanVienRepository nhanVienRepository;
        private IPermissionRepository permissionRepository;
        private IPermissionFeatureRepository permissionFeatureRepository;
        private IEmployeePermissionRepository employeePermissionRepository;
        private IBaoHongRepository baoHongRepository;
        public UnitOfWork(AppSetting appSetting)
        {
            this.dbContext = new DbContext(appSetting.ConnectionString);
        }

        public IAccountRepository AccountRepository => this.accountRepository ??= new AccountRepository(dbContext);
        public IDonViRepository DepartmentRepository => this.departmentRepository ??= new DonViRepository(dbContext);
        public IFeatureRepository FeatureRepository => this.featureRepository ??= new FeatureRepository(dbContext);
        public INhanVienRepository NhanVienRepository => this.nhanVienRepository ??= new NhanVienRepository(dbContext);
        public IPermissionRepository PermissionRepository => this.permissionRepository ??= new PermissionRepository(dbContext);
        public IPermissionFeatureRepository PermissionFeatureRepository => this.permissionFeatureRepository ??= new PermissionFeatureRepository(dbContext);
        public IEmployeePermissionRepository EmployeePermissionRepository => this.employeePermissionRepository ??= new EmployeePermissionRepository(dbContext);
        public IBaoHongRepository BaoHongRepository => this.baoHongRepository ??= new BaoHongRepository(dbContext);

        public void Commit()
        {
            this.dbContext.Commit();
            Dispose();
        }

        public void Rollback()
        {
            this.dbContext.Rollback();
            Dispose();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork() // the finalizer
        {
            Dispose(false);
        }
    }
}
