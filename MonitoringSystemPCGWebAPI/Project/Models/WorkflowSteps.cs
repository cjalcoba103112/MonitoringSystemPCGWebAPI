
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class WorkflowSteps
    {
		[Key]
		public int? Id {get;set;}
		public int? RankCategoryId {get;set;}
		[ForeignKey("RankCategoryId")]
		public RankCategory? RankCategory { get; set; }
		public int? StepNumber {get;set;}
		public int? RoleId {get;set;}
        [ForeignKey("RoleId")]
        public Role? Role{ get; set; }

    }
}
