using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Models;

namespace AtHomeProject.Domain.Interfaces
{
    public interface IShippingService
    {
        Task<(double Total, string Company)> GetRateAsync(
            string contract,
            string destination,
            IEnumerable<PackageDimensionModel> packages
        );
    }
}
