using BussinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Entity;
using System.Security.Claims;
using System.Text.Json;

namespace EmployeeManagement.Controllers
{
    [Route("api/EmpRole")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleBL _roleBL;


        public RoleController(IRoleBL roleBL)
        {
            _roleBL = roleBL;
        }

        [HttpPost]
        [ Authorize]

        public async Task<ResponseModel<Tuple<string>>> AddRole(Tuple<string> role)
        {
            var response = new ResponseModel<Tuple<string>>();
            try
            {
                string empId = User.FindFirstValue("EmployeeId");
                int _empId = Convert.ToInt32(empId);

                var result = await _roleBL.AddRole(role,_empId );

                response.Success = true;
                response.Message = $"Role Added Successfully";
                response.Data = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return response;
        }

        [HttpGet]
        [Authorize]

        public async Task<ResponseModel<Tuple<string>>> ViewRole()
        {
            var response = new ResponseModel<Tuple<string>>();
            try
            {
                string empId = User.FindFirstValue("EmployeeId");
                int _empId = Convert.ToInt32(empId);

               

                var result = await _roleBL.ViewRole(_empId);

                response.Success = true;
                response.Message = $"The following employee is {result}";
                response.Data = result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message ); 

            }

            return response;
        }



    }
}
