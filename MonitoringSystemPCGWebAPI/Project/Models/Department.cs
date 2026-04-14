
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Department
    {
		[Key]
		public int? DepartmentId {get;set;}
		public string? DepartmentName {get;set;}
		public string? Location {get;set;}

    }
}
