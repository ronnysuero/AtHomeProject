﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;

namespace AtHomeProject.Domain.Services
{
    public class UspsFakeService : IUspsService
    {
        //TODO: implement real calculation consuming usps http api: https://www.usps.com/business/web-tools-apis/#developers
        public Task<(double Total, string Company)> GetRateAsync(
            string contract,
            string destination,
            IEnumerable<PackageDimensionModel> packages
        ) => Task.FromResult((new Random().NextDouble() * (9999 - 35) + 10, "USPS"));
    }
}