using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface IGenericRepository<Tentity,Tkey> where Tentity : BaseEntity<Tkey>
    {
        //Async Methods => GetAll, GetById, Add
        //Sync Methods => Update, Delete
        Task<IEnumerable<Tentity>> GetAllAsync();
        Task<Tentity?> GetByIdAsync(Tkey id);
        Task AddAsync(Tentity entity);
        void Update(Tentity entity);
        void Remove(Tentity entity);

        #region With Specifications:
        Task<IEnumerable<Tentity>> GetAllAsync(ISpecifications<Tentity,Tkey> specifications);
        Task<Tentity?> GetByIdAsync(ISpecifications<Tentity, Tkey> specifications);

        public Task<int> CountAsync(ISpecifications<Tentity, Tkey> specifications);
        #endregion
    }
}
