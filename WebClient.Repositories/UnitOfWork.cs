using System;
using WebClient.Core;
using WebClient.Repositories.Implements;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private IAccountRepository accountRepository;
        private IDepartmentRepository departmentRepository;
        private IFeatureRepository featureRepository;
        private IEmployeeRepository employeeRepository;
        private IPermissionRepository permissionRepository;
        private IPermissionFeatureRepository permissionFeatureRepository;
        private IEmployeePermissionRepository employeePermissionRepository;
        private IDangKyTiemVaccineRepository dangKyTiemVaccineRepository;
        private IDotTiemVaccineRepository dotTiemVaccineRepository;
        private ILichSuTiemVaccineRepository lichSuTiemVaccineRepository;
        private IDmDoiTuongUuTienRepository dmDoiTuongUuTienRepository;
        private IDmDanTocRepository dmDanTocRepository;

        public UnitOfWork(AppSetting appSetting)
        {
            this.dbContext = new DbContext(appSetting.ConnectionString);
        }

        public IAccountRepository AccountRepository => this.accountRepository ??= new AccountRepository(dbContext);
        public IDepartmentRepository DepartmentRepository => this.departmentRepository ??= new DepartmentRepository(dbContext);
        public IFeatureRepository FeatureRepository => this.featureRepository ??= new FeatureRepository(dbContext);
        public IEmployeeRepository EmployeeRepository => this.employeeRepository ??= new EmployeeRepository(dbContext);
        public IPermissionRepository PermissionRepository => this.permissionRepository ??= new PermissionRepository(dbContext);
        public IPermissionFeatureRepository PermissionFeatureRepository => this.permissionFeatureRepository ??= new PermissionFeatureRepository(dbContext);
        public IEmployeePermissionRepository EmployeePermissionRepository => this.employeePermissionRepository ??= new EmployeePermissionRepository(dbContext);
        public IDangKyTiemVaccineRepository DangKyTiemVaccineRepository => this.dangKyTiemVaccineRepository ??= new DangKyTiemVaccineRepository(dbContext);
        public IDotTiemVaccineRepository DotTiemVaccineRepository => this.dotTiemVaccineRepository ??= new DotTiemVaccineRepository(dbContext);
        public ILichSuTiemVaccineRepository LichSuTiemVaccineRepository => this.lichSuTiemVaccineRepository ??= new LichSuTiemVaccineRepository(dbContext);
        public IDmDoiTuongUuTienRepository DmDoiTuongUuTienRepository => this.dmDoiTuongUuTienRepository ??= new DmDoiTuongUuTienRepository(dbContext);
        public IDmDanTocRepository DmDanTocRepository => this.dmDanTocRepository ??= new DmDanTocRepository(dbContext);
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
