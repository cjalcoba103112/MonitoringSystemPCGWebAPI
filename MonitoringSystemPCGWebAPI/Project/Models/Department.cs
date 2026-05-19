
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Department
    {
		[Key]
		public int? DepartmentId {get;set;}
		public string? DepartmentName {get;set;}
		public int? OicId {get;set;}


		[ForeignKey("OicId")]
		public Personnel? Oic {get;set; }
        public string? Location {get;set;}

    }
}
