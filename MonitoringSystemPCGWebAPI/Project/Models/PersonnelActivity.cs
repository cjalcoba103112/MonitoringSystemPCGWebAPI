
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class PersonnelActivity
    {
		[Key]
		public int? PersonnelActivityId {get;set;}
		public int? PersonnelId {get;set;}

		[ForeignKey("PersonnelId")]
        [JsonIgnore]
        public Personnel? Personnel {get;set;}
		public int? ActivityTypeId {get;set;}
        [ForeignKey("ActivityTypeId")]	
        public ActivityType? ActivityType {get;set;}
		public string? Title {get;set;}
		public DateTime? StartDate {get;set;}
		public DateTime? EndDate {get;set;}
		public string? Status {get;set;}
		public string? Result {get;set;}
		public string? Remarks {get;set;}
		public decimal? Days { get; set; }

		public string? Reason { get; set; }

		public bool? IsWarningSent { get; set; }
		public bool? IsFullyApproved { get; set; }
    }
}
