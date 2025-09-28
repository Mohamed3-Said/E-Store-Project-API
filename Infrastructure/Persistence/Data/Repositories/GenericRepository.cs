using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Persistence.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories
{
    public class GenericRepository<Tentity, Tkey>(StoreDbContext _dbContext) : IGenericRepository<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
    {
        public async Task AddAsync(Tentity entity) => await _dbContext.Set<Tentity>().AddAsync(entity);

        public async Task<IEnumerable<Tentity>> GetAllAsync() => await _dbContext.Set<Tentity>().ToListAsync();

        public async Task<Tentity?> GetByIdAsync(Tkey id) => await _dbContext.Set<Tentity>().FindAsync(id);

        public void Remove(Tentity entity) => _dbContext.Set<Tentity>().Remove(entity);

        public void Update(Tentity entity) => _dbContext.Set<Tentity>().Update(entity);



        #region With Specifications:
        public async Task<IEnumerable<Tentity>> GetAllAsync(ISpecifications<Tentity, Tkey> specifications)
        {
          return await SpecificationEvaluator.CreateQuery(_dbContext.Set<Tentity>(),specifications).ToListAsync();
        }

        public async Task<Tentity?> GetByIdAsync(ISpecifications<Tentity, Tkey> specifications)
        {
          return await SpecificationEvaluator.CreateQuery(_dbContext.Set<Tentity>(),specifications).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecifications<Tentity, Tkey> specifications)
         => await SpecificationEvaluator.CreateQuery(_dbContext.Set<Tentity>(),specifications).CountAsync();
        #endregion


    }
}
