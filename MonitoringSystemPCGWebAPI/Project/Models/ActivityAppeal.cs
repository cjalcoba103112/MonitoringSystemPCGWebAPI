
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ActivityAppeal
    {
		[Key]
		public int? Id {get;set;}
		public int? PersonnelActivityId {get;set;}
		[ForeignKey("PersonnelActivityId")]
		public PersonnelActivity? PersonnelActivity {get;set;}
		public string? AppealToken {get;set;}
		public bool? IsUsed {get;set;}
		public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedAt {get;set;} 
		public string? Remarks {get;set;}
		public string? DisapprovedRoleName { get; set; }
		public string? AppealTargetRoleName { get; set; }

    }
}
