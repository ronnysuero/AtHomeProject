using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;

namespace AtHomeProject.Domain.Services
{
    public class FedexFakeService : IFedexService
    {
        //TODO: implement real calculation consuming fedex http api: https://www.fedex.com/en-us/developer.html
        public Task<(double Total, string Company)> GetRateAsync(
            string contract,
            string destination,
            IEnumerable<PackageDimensionModel> packages
        ) => Task.FromResult((new Random().NextDouble() * (9999 - 35) + 10, "Fedex"));
    }
}
