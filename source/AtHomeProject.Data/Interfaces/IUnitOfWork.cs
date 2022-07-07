using System;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;

namespace AtHomeProject.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Users> Users { get; }
        IRepository<UsersClaims> UsersClaims { get; }
        Task<int> SaveAsync();
    }
}
