using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Models;

namespace Models
{
    public class ApprovalProccess
    {
        [Key]
        public int? Id { get; set; }

        // Stage 1: CMAA
        public int? CmaaId { get; set; }
        public string? CmaaRemarks { get; set; }
        public bool? CmaaIsApprove { get; set; }

        [ForeignKey("CmaaId")]
        public Personnel? Cmaa { get; set; }

        // Stage 2: OIC
        public int? OicId { get; set; }
        public string? OicRemarks { get; set; }
        public bool? OicIsApprove { get; set; }
        [ForeignKey("OicId")]
        public Personnel? Oic { get; set; }

        // Stage 3: CSG
        public int? CsgId { get; set; }
        public string? CsgRemarks { get; set; }
        public bool? CsgIsApprove { get; set; }
        [ForeignKey("CsgId")]
        public Personnel? Csg { get; set; }

        // Stage 4: Final CO
        public int? CoId { get; set; }
        public string? CoRemarks { get; set; }
        public bool? CoIsApprove { get; set; }
        [ForeignKey("CoId")]
        public Personnel? Co { get; set; }

        public int? CurrentStage { get; set; } = 1;
    }
}
