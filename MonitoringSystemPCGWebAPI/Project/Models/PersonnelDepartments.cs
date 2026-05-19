using System.ComponentModel.DataAnnotations.Schema;
using Models;

namespace Models
{
    public class PersonnelDepartments
    {
        public int? Id { get; set; }
        public int? PersonnelId { get; set; }
        [ForeignKey("PersonnelId")]
        public Personnel? Personnel { get; set; }
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
        public bool? IsPrimary { get; set; }
    }
}
