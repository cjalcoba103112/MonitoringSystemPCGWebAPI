
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class PersonnelDutyLogs
    {
		[Key]
		public int? Id {get;set;}
		public int? PersonnelId {get;set;}
		[ForeignKey("PersonnelId")]
        [JsonIgnore]
        public Personnel? Personnel { get; set; }
        public string? Status {get;set;}
		public DateTime? StartDate {get;set;}
		public DateTime? EndDate {get;set;}
		public string? Remarks {get;set;}
		public bool? IsActive {get;set;}

    }
}
