using Amazon.SQS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.HashPassword;
using RepositoryLayer.Interface;
using RepositoryLayer.JwtToken;
using System.Text.Json;

namespace RepositoryLayer.Service
{
    public class EmployeeRL : IEmployeeRL
    {
        private readonly EmployeeContext _context;
        private readonly Hashpassword _hash;
        private readonly IConfiguration _config;
        private readonly IDistributedCache _cache;
        private readonly IAWS _awsRL;
        private readonly IS3 _s3;
        public EmployeeRL(EmployeeContext context, Hashpassword hash, IConfiguration config, IDistributedCache cache, IAWS awsRL, IS3 s3)
        {
            _context = context;
            _hash = hash;
            _config = config;
            _cache = cache;
            _awsRL = awsRL;
            _s3 = s3;
        }



        public async Task<Tuple<string, string, string, long, string>>  AddEmployee(Tuple<string, string, string, long, string> employee)
        {
            var check =  await _context.EmployeeDetail.FirstOrDefaultAsync(e => e.Email == employee.Item3);
            if (check == null)
            {

                var employeeDetails = new EmployeeEntity
                {
                    EmployeeName = employee.Item1,
                    EmployeeLastName = employee.Item2,
                    Email = employee.Item3,
                    ContactNo = employee.Item4,
                    password = _hash.HashPassword(employee.Item5)
                };

               await  _context.EmployeeDetail.AddAsync(employeeDetails);
                await _context.SaveChangesAsync();

                var employeeTuple = Tuple.Create(employeeDetails.EmployeeName, employeeDetails.EmployeeLastName, employeeDetails.Email, employeeDetails.ContactNo, employeeDetails.password);


                await _cache.SetStringAsync(Convert.ToString(employeeDetails.EmployeeId), System.Text.Json.JsonSerializer.Serialize(employeeTuple));

              await  _awsRL.SendMessageAsync(employeeTuple);


                return employee;
            }
            return null;
        }
        
        public async Task<string> Login(Tuple<string, string> login)
        {
            var checkUser = await _context.EmployeeDetail.FirstOrDefaultAsync(e => e.Email == login.Item1);
            


            if (checkUser != null)
            {
                bool HashPassword = _hash.VerifyPassword(login.Item2, checkUser.password);

                if(HashPassword)
                {
                    Generatetoken token = new Generatetoken(_config);

                    return token.GenerateToken(checkUser);
                }

            }

            return null;
        }

        public async Task<Tuple<string, string, string, long, string>> ViewDetail(int employeeId)
        {
            var detail = await _context.EmployeeDetail.FirstOrDefaultAsync(e => e.EmployeeId ==  employeeId);  
            
            if (detail != null)
            {

                List<Message> message = await _awsRL.RecieveMessageAsync();
                List<AllMessage> allMessage = new List<AllMessage>();

                
                allMessage = message.Select(c => new AllMessage{ 
               MessageId = c.MessageId, 
               ReceiptHandle = c.ReceiptHandle,  
               Employee = JsonConvert.DeserializeObject<Tuple<string, string, string, long, string>>(c.Body)
               }).ToList();


                var employeeTuple = allMessage.FirstOrDefault()?.Employee;
                string reciept = allMessage.FirstOrDefault()?.ReceiptHandle;

               await  _awsRL.DeleteMessageAsync(reciept);

                return employeeTuple;
                // Create a tuple with the employee details and return it
                //var employeeTuple = Tuple.Create(detail.EmployeeName, detail.EmployeeLastName, detail.Email, detail.ContactNo, detail.password);

            }


            return null;
        }

       public T Update<T>(T input1, T input2)
       {
            if(input1.Equals("") )
            {
                return input2;
            }
            return input1;
       }
       public async Task<Tuple<string, string, string, long, string>> Editdetail(Tuple<string, string, string, long, string> employee,int empId) 
       {
            var Userdetail = await _context.EmployeeDetail.FirstOrDefaultAsync(e => e.EmployeeId == empId);

            Userdetail.EmployeeName = Update(employee.Item1, Userdetail.EmployeeName);
            Userdetail.EmployeeLastName = Update(employee.Item2, Userdetail.EmployeeLastName);
            Userdetail.Email = Update(employee.Item3, Userdetail.Email);
            Userdetail.ContactNo = Update(employee.Item4, Userdetail.ContactNo);
            Userdetail.password = Update(employee.Item5, Userdetail.password);

           await  _context.SaveChangesAsync();

            var employeeTuple = Tuple.Create(Userdetail.EmployeeName, Userdetail.EmployeeLastName, Userdetail.Email, Userdetail.ContactNo, Userdetail.password);

          await  _cache.SetStringAsync(Convert.ToString(empId), System.Text.Json.JsonSerializer.Serialize(employeeTuple));


            return employeeTuple;


       }

       public async Task<bool> RemoveUser (int empId)
        {
            var Userdetail = await _context.EmployeeDetail.FirstOrDefaultAsync(e => e.EmployeeId == empId);

            _context.EmployeeDetail.Remove(Userdetail);
            await _context.SaveChangesAsync();

            _cache.Remove(Convert.ToString(empId));
            return true;
        }

        public async Task<bool> UploadPhoto(IFormFile formFile, string bucketName, string objectKey)
        {
            bool upload = await _s3.UploadFileAsync(formFile, bucketName, objectKey);

            if(upload)
            {
                return true;
            }
            return false;
        }
       

    }
}
