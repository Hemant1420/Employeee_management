using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions options) : base(options)
        {


        }

       public  DbSet<EmployeeEntity> EmployeeDetail { get; set; }

        public DbSet<RoleEntity> RoleEntities { get; set; }


    }
}
