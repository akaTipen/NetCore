using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AspCore.Data
{
    public class Employees
    {
        [Key]
        public int EmployeeId { get; set; }
        [DisplayName("Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Join Date")]
        public DateTime? JoinDate { get; set; }
        public byte[] Photo { get; set; }
        public decimal? Height { get; set; }
        public double? Weight { get; set; }
        [DisplayName("Department")]
        public int DepartmentId { get; set; }
    }
}
