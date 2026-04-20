using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Models;

namespace Models
{
    public class ApprovalProccess
    {
        public int? Id { get; set; }

        public int? ActivityId { get; set; }
        // Stage 1: CMAA
        public int? CmaaId { get; set; }
        public string? CmaaRemarks { get; set; }
        public bool? CmaaIsApprove { get; set; }

        // Stage 2: OIC
        public int? OicId { get; set; }
        public string? OicRemarks { get; set; }
        public bool? OicIsApprove { get; set; }

        // Stage 3: CSG
        public int? CsgId { get; set; }
        public string? CsgRemarks { get; set; }
        public bool? CsgIsApprove { get; set; }

        // Stage 4: Final CO
        public int? CoId { get; set; }
        public string? CoRemarks { get; set; }
        public bool? CoIsApprove { get; set; }

        public int? CurrentStage { get; set; } = 1;
    }
}
