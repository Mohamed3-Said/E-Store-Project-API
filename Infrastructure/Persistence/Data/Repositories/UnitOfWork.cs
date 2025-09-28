using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUnitOfWork
    {
        //Dictionary<string, object> _repositories 
         private readonly Dictionary<string, object> _repositories = [];
        public IGenericRepository<Tentity, Tkey> GetRepository<Tentity, Tkey>() where Tentity : BaseEntity<Tkey>
        {
            //Get the type name of the entity
            var typeName = typeof(Tentity).Name;

            //Dic<string,object> => string = TypeName, object = GenericRepository
            if (_repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepository<Tentity, Tkey>) value;

            //if (_repositories.ContainsKey(typeName))
            //    return (IGenericRepository<Tentity,Tkey>) _repositories[typeName];

            else
            {
                //Create object of GenericRepository<Tentity,Tkey>
                var Repo = new GenericRepository<Tentity, Tkey>(_dbContext);
                //store it to the dictionary
                _repositories.Add(typeName, Repo);
                //Return the GenericRepository<Tentity,Tkey>
                return Repo;
            }
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }


}
