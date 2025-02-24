﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Product
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<Package?> GetPackageByIdAsync(int id);
        Task<Package> AddPackageAsync(Package package);
        Task<Package?> UpdatePackageAsync(Package package);
        Task<bool> DeletePackageAsync(int id);
    }
}
