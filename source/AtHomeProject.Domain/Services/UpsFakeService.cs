using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;

namespace AtHomeProject.Domain.Services
{
    public class UpsFakeService : IUpsService
    {
        //TODO: implement real calculation consuming Ups http api: https://www.ups.com/upsdeveloperkit
        public Task<(double Total, string Company)> GetRateAsync(
            string contract,
            string destination,
            IEnumerable<PackageDimension> packages
        ) => Task.FromResult((new Random().NextDouble() * (9999 - 35) + 10, "UPS"));
    }
}
