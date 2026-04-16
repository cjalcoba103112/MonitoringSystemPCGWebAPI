
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class EmailEteCommunication
    {
		[Key]
		public int? Id {get;set;}
		public int? PersonnelId {get;set;}
		[ForeignKey("PersonnelId")]
		public Personnel? Personnel { get; set; }
		public string? EmailCategory {get;set;}
		public DateTime? NextEte {get;set;}
		public int? RemainingDays { get; set; }
		public string? CommunicationToken {get;set;}
		public DateTime? SentDateTime {get;set;}
		public DateTime? ExpiryDateTime {get;set;}
		public bool? IsAccessed {get;set;}
		public DateTime? AccessedDateTime {get;set;}
		public string? Response {get;set;}
		public DateTime? ResponseDateTime {get;set;}

		public string? Remarks { get;set;}
		public string? SupportingDocument { get;set;}
    }
}
