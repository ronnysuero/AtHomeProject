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
        private IRepository<Users> _usersRepository;
        private IRepository<UsersClaims> _usersClaimRepository;

        public IRepository<Users> Users
        {
            get
            {
                _usersRepository ??= new Repository<Users>(_db);

                return _usersRepository;
            }
        }

        public IRepository<UsersClaims> UsersClaims
        {
            get
            {
                _usersClaimRepository ??= new Repository<UsersClaims>(_db);

                return _usersClaimRepository;
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
