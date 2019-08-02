using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspCore.Data
{
    public partial class Departments
    {
        public Departments()
        {
            Employees = new HashSet<Employees>();
        }

        [Key]
        public int DepartmentId { get; set; }
        public string DepartmenCode { get; set; }
        public string DepartmenName { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
