using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class EmployeeModel
    {
        public string EmployeeName { get; set; }

        public string EmployeeLastName { get; set; }

   
        public string Email { get; set; }


        public long ContactNo { get; set; }

        public string password {  get; set; }

        public IFormFile image { get; set; }


    }
}
