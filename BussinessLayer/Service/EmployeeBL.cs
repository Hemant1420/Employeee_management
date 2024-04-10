using BussinessLayer.InterfaceBL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeRL _employeeRL;
        private readonly IDistributedCache _cache;

        public EmployeeBL(IEmployeeRL employeeRL, IDistributedCache cache)
        {
            _employeeRL = employeeRL;
            _cache = cache;
        }

        public Task<Tuple<string, string, string, long, string>> AddEmployee(Tuple<string, string, string, long, string> employee)
        {
            return _employeeRL.AddEmployee(employee);
        }

        public Task<string> Login(Tuple<string, string> login)
        {
            return _employeeRL.Login(login);
        }

        public async Task<Tuple<string, string, string, long, string>> ViewDetail(int employeeId)
        {
            /*var resultCache = _cache.GetString(Convert.ToString(employeeId));

            if (resultCache != null)
            {
                var finalResult = JsonSerializer.Deserialize<Tuple<string, string, string, long, string>>(resultCache);
                var employeeTuple = Tuple.Create(finalResult.Item1, finalResult.Item2, finalResult.Item3, finalResult.Item4, finalResult.Item5);

                return employeeTuple;
            }
            else
            {
                var result = await _employeeRL.ViewDetail(employeeId);

                _cache.SetString(Convert.ToString(employeeId), JsonSerializer.Serialize(result));
                return result;

            }*/

            var result = await _employeeRL.ViewDetail(employeeId);

            return result;
        }

        public Task<Tuple<string, string, string, long, string>> Editdetail(Tuple<string, string, string, long, string> employeeEntity, int empId)
        {
            return _employeeRL.Editdetail(employeeEntity, empId);

            
        }

        public Task<bool> RemoveUser(int empId)
        {
            return _employeeRL.RemoveUser(empId);  
        }

        public Task<bool> UploadPhoto(IFormFile formFile, string bucketName, string objectKey)
        {
              return _employeeRL.UploadPhoto(formFile, bucketName, objectKey ); 
        }


    }
}
