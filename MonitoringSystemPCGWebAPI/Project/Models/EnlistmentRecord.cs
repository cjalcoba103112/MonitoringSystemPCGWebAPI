using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class EnlistmentRecord
    {
        [Key]
        public int? EnlistmentId { get; set; }

        public int? PersonnelId { get; set; }   

        public DateTime? EnlistmentStart { get; set; }
        public DateTime? EnlistmentEnd { get; set; }

        public int? ContractYears { get; set; }

        public string? Status { get; set; }
        // ACTIVE, COMPLETED, REENLISTED, SEPARATED, ALREADY SUBMITTED

        public bool? ReenlistmentFlag { get; set; } = false;

        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
