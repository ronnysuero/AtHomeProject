using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Pagination;

namespace AtHomeProject.Domain.Interfaces
{
    public interface ISensorInputService
    {
        Task InsertRangeAsync(IEnumerable<SensorInputModel> models);

        Task<PagedResult<SensorInputListModel>> GetPagedAsync(SensorInputRequest request);
    }
}
