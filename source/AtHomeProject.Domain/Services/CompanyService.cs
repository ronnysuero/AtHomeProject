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
            _fedexService = fedexService;
            _upsService = upsService;
            _uspsService = uspsService;
        }

        public async Task<double> CalculateShipmentAsync(
            string contract,
            string destination,
            IEnumerable<PackageDimensionModel> packages
        )
        {
            var results = await Task.WhenAll(
                _fedexService.GetRateAsync(contract, destination, packages),
                _upsService.GetRateAsync(contract, destination, packages),
                _uspsService.GetRateAsync(contract, destination, packages)
            );

            return Math.Round(results.Select(s => s.Total).Min(), 2);
        }
    }
}
