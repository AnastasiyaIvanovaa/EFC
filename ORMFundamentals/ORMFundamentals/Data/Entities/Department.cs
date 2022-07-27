using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ORMFundamentals.Data.Entities
{
    public class Department
    {

        public Department()
        {
             this.Employees=new HashSet<Employee>();
        }
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        [ForeignKey(nameof(Employee))]
        public int? ManagerID { get; set; }

        
    }
}
