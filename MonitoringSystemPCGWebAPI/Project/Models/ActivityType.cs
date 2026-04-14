
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ActivityType
    {
		[Key]
		public int? ActivityTypeId {get;set;}
		public string? ActivityTypeName {get;set;}
        public int? MaxCredits { get; set; } 
        public int? ResetMonths { get; set; }
        public bool? IsMandatoryLeave { get; set; } 

    }
}
