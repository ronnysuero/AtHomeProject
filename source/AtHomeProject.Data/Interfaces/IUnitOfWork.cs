using System;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;

namespace AtHomeProject.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Device> Device { get; }

        IRepository<SensorInput> SensorInput { get; }

        IRepository<SensorAlert> SensorAlert { get; }

        Task<int> SaveAsync();
    }
}
