using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
    public interface IRoleBL
    {

        public Task<Tuple<string>> AddRole(Tuple<string> role,  int empId);

        public Task<Tuple<string>> ViewRole(int empId);
    }
}
