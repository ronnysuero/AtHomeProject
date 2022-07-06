using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Pagination;

namespace AtHomeProject.Domain.Interfaces
{
    public interface IDeviceService
    {
        Task InsertRangeAsync(IEnumerable<DeviceModel> models);

        Task<DeviceModel> GetBySerialNumberAsync(string serialNumber);

        Task<PagedResult<DeviceListModel>> GetPagedAsync(PaginationRequest request);

        Task<IEnumerable<DeviceModel>> GetAsync();

        Task<DeviceModel> InsertAsync(DeviceModel model);
    }
}
