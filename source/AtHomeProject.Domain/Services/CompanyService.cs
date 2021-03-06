using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;

namespace AtHomeProject.Domain.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IFedexService _fedexService;
        private readonly IUpsService _upsService;
        private readonly IUspsService _uspsService;

        public CompanyService(IFedexService fedexService, IUpsService upsService, IUspsService uspsService)
        {
            _fedexService = fedexService ?? throw new ArgumentNullException(nameof(fedexService));
            _upsService = upsService ?? throw new ArgumentNullException(nameof(upsService));
            _uspsService = uspsService ?? throw new ArgumentNullException(nameof(uspsService));
        }

        public async Task<double> CalculateShipmentAsync(
            string contract,
            string destination,
            IEnumerable<PackageDimension> packages
        )
        {
            var results = await Task.WhenAll(
                _fedexService.GetRateAsync(contract, destination, packages),
                _upsService.GetRateAsync(contract, destination, packages),
                _uspsService.GetRateAsync(contract, destination, packages)
            );

            return Math.Round(results.Select(s => s.Item1).Min(), 2);
        }
    }
}
