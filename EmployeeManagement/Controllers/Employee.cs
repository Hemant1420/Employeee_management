using Azure;
using BussinessLayer.InterfaceBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;

namespace EmployeeManagement.Controllers
{
    [Route("api/Employees")]
    [ApiController]
    public class Employee : ControllerBase
    {
        private readonly IEmployeeBL _employeeBL;


        public Employee(IEmployeeBL employeeBL)
        {
            _employeeBL = employeeBL;
        }

        [HttpPost]


        public async Task<ResponseModel<Tuple<string, string, string, long, string>>> Register(Tuple<string, string, string, long, string> employee)
        {
            var response = new ResponseModel<Tuple<string, string, string, long, string>>();

            try
            {
                var result = await  _employeeBL.AddEmployee(employee);
               


                if (result != null)
                {

                    response.Success = true;
                    response.Message = "Employee Registered Successfully";
                    response.Data = result;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Employee Already registered";

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;


        }

        [HttpPost("Login")]

        public async Task<ResponseModel<string>> Login(Tuple<string, string> login)
        {
            var response = new ResponseModel<string>();

            try
            {

                var result = await _employeeBL.Login(login);

                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Login Successful";
                    response.Data = result;

                }
                else
                {
                    response.Success = false;
                    response.Message = "Login failed";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;

        }

        [HttpGet]
        [Authorize]
        public async  Task<ResponseModel<Tuple<string, string, string, long, string>>> Viewdetail()
        {
            var response = new ResponseModel<Tuple<string, string, string, long, string>>();
            

            try
            {

                string empId = User.FindFirstValue("EmployeeId");
                int _empId = Convert.ToInt32(empId);

                var result = await _employeeBL.ViewDetail(_empId);

                response.Success = true;
                response.Message = "Data retrieved Successfully";
                response.Data = result;




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;

        }

        [HttpPut]
        [Authorize]

        public async Task<ResponseModel<Tuple<string, string, string, long, string>>> EditDetail(Tuple<string, string, string, long, string> employeeEntity)
        {

            var response = new ResponseModel<Tuple<string, string, string, long, string>>();
            try
            {
                string empId = User.FindFirstValue("EmployeeId");
                int _empId = Convert.ToInt32(empId);

                var result = await _employeeBL.Editdetail(employeeEntity, _empId);

                response.Success = true;
                response.Message = "Data retrieved Successfully";
                response.Data = result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;


        }

        [HttpDelete]
        [Authorize]

        public async Task<ResponseModel<bool>> DeleteUser()
        {
            var response = new ResponseModel<bool>();

            try
            {

                string empId = User.FindFirstValue("EmployeeId");
                int _empId = Convert.ToInt32(empId);

                var result = await _employeeBL.RemoveUser(_empId);

                response.Success = true;
                response.Message = "User removed Successfully";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
            

        }

        [HttpPost("Upload")]
        public async Task<ResponseModel<bool>> UploadImage(IFormFile file, string bucketName, string objectKey) 
        {
            var response = new ResponseModel<bool>();

            try
            {

                var result = await _employeeBL.UploadPhoto(file, bucketName, objectKey);
                if (result)
                {
                    response.Success = true;
                    response.Message = "User photo uploaded Successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = " photo upload UnSuccessfully";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        } 

        



    }
}
