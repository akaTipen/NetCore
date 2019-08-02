using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace AspCore.Data
{
    public class EmployeesViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [DisplayName("Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Join Date")]
        public string JoinDate { get; set; }
        public byte[] Photo { get; set; }
        public IFormFile SendPhoto { get; set; }
        public decimal? Height { get; set; }
        public double? Weight { get; set; }
        public int DepartmentId { get; set; }
        [DisplayName("Department")]
        public string DepartmentName { get; set; }
    }
}
