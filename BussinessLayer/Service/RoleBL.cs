using BussinessLayer.Interface;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class RoleBL : IRoleBL
    {

        private readonly IRoleRL _roleRL;
        private readonly IDistributedCache _cache;

        public RoleBL(IRoleRL roleRL, IDistributedCache cache)
        {
            _roleRL = roleRL;
            _cache = cache;
        }

        public Task<Tuple<string>> AddRole(Tuple<string> role,  int empId)
        {
            return _roleRL.AddRole(role, empId);
        }


        public async Task<Tuple<string>> ViewRole(int empId)
        {
           string key = "Role:" + Convert.ToString(empId);
           var cacheResult =  _cache.GetString(Convert.ToString(key));

           if(cacheResult != null)
           {
                var finalResult = JsonSerializer.Deserialize<Tuple<string>>(cacheResult);
                return finalResult;

           }
           else
           {
               var result =  await _roleRL.ViewRole(empId);

              
               _cache.SetString(Convert.ToString(key), JsonSerializer.Serialize(result));
                return result;
           }


        }

    }
}
