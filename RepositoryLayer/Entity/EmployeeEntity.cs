using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class EmployeeEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeLastName { get; set; }


        [EmailAddress]
        public string Email { get; set; }

        public long ContactNo { get; set; }

        public string password {  get; set; }

        public byte[] imageData { get; set; }

    }
}
