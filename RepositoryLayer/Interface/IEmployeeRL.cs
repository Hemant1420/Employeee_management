using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IEmployeeRL
    {

        public  Task<Tuple<string, string, string, long, string>> AddEmployee(Tuple<string, string, string, long, string> employee);

        public Task<string> Login(Tuple<string, string> login);

        public Task<Tuple<string, string, string, long, string>> ViewDetail(int employeeId);

        public Task<Tuple<string, string, string, long, string>> Editdetail(Tuple<string, string, string, long, string> employee, int empId);

        public Task<bool> RemoveUser(int empId);

        public Task<bool> UploadPhoto(IFormFile formFile, string bucketName, string objectKey);






    }
}
