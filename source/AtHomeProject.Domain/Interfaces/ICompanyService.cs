using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Models;

namespace AtHomeProject.Domain.Interfaces
{
    public interface ICompanyService
    {
        Task<double> CalculateShipmentAsync(
            string contract,
            string destination,
            IEnumerable<PackageDimensionModel> packages
        );
    }
}
