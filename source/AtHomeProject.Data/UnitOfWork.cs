using System;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using AtHomeProject.Data.Interfaces;

namespace AtHomeProject.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ApplicationDbContext db) => _db = db;

        private readonly ApplicationDbContext _db;
        private IRepository<Device> _deviceRepository;
        private IRepository<SensorInput> _sensorInputRepository;
        private IRepository<SensorAlert> _sensorAlertRepository;

        public IRepository<Device> Device
        {
            get
            {
                _deviceRepository ??= new Repository<Device>(_db);

                return _deviceRepository;
            }
        }

        public IRepository<SensorInput> SensorInput
        {
            get
            {
                _sensorInputRepository ??= new Repository<SensorInput>(_db);

                return _sensorInputRepository;
            }
        }

        public IRepository<SensorAlert> SensorAlert
        {
            get
            {
                _sensorAlertRepository ??= new Repository<SensorAlert>(_db);

                return _sensorAlertRepository;
            }
        }

        public async Task<int> SaveAsync() => await _db.SaveChangesAsync();

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _db.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
