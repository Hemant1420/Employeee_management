using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class RoleRL : IRoleRL
    {
        private readonly EmployeeContext _context;
        private readonly IDistributedCache _cache;

        public RoleRL(EmployeeContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Tuple<string>> AddRole(Tuple<string> role, int empId)
        {
            RoleEntity roleEntity = new RoleEntity();

            roleEntity.RoleName = role.Item1;
            roleEntity.EmployeeId = empId;


            await _context.RoleEntities.AddAsync(roleEntity);
            await  _context.SaveChangesAsync();

            var roleTuple = Tuple.Create(roleEntity.RoleName);

            string key = "Role:" + Convert.ToString(empId);
            await _cache.SetStringAsync(Convert.ToString(key), JsonSerializer.Serialize(roleTuple));
            return roleTuple;
        }
        public async Task<Tuple<string>> ViewRole(int empId)
        {
            var check = await  _context.RoleEntities.FirstOrDefaultAsync(e => e.EmployeeId == empId);
            var roleTuple = Tuple.Create(check.RoleName);


            return roleTuple;
        }

        

    }
}
