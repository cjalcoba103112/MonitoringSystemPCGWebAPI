using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Personnel
    {
        [Key]
        public int? PersonnelId { get; set; }
        public string? Profile { get; set; }
        public string? SerialNumber { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public int? RankId { get; set; }

        [ForeignKey("RankId")]
        public Rank? Rank { get; set; }

        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }  
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public Usertbl? User {  get; set; }

        public string? EmploymentStatus { get; set; }
        public DateTime? DateEnlisted { get; set; }

        public DateTime? DateEnteredService { get; set; }

        public string? Email { get; set; }
        public DateTime? DateOfLastPromotion { get; set;  }
        public ICollection<PersonnelActivity>? PersonnelActivities { get; set; }
        public ICollection<EnlistmentRecord>? EnlistmentRecords { get; set; }
        public ICollection<PersonnelPromotion>? PersonnelPromotions { get;set;  }
    }
}