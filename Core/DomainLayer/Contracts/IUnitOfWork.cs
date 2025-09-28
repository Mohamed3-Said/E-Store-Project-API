using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface IUnitOfWork
    {

        // Generic Repository => Dictionary<string, object>  //Method way Implemmentation
        IGenericRepository<Tentity,Tkey> GetRepository<Tentity, Tkey>() where Tentity : BaseEntity<Tkey>;   
        //IGenericRepository<Product,int> GetProductRepo { get; } **** 
        Task<int> SaveChangesAsync();
    }
}
