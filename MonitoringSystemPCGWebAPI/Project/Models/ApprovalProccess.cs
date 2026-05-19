
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ApprovalProccess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? CurrentStage { get; set; }
        public int? StageOneId { get; set; }
        [ForeignKey("StageOneId")]
        public Personnel? ApproverOne { get; set; }
        public string? StageOneRemarks { get; set; }
        public bool? StageOneIsApprove { get; set; }
        public int? StageTwoId { get; set; }
        [ForeignKey("StageTwoId")]
        public Personnel? ApproverTwo { get; set; }
        public string? StageTwoRemarks { get; set; }
        public bool? StageTwoIsApprove { get; set; }
        public int? StageThreeId { get; set; }
        public string? StageThreeRemarks { get; set; }
        public bool? StageThreeIsApprove { get; set; }
        [ForeignKey("StageThreeId")]
        public Personnel? ApproverThree { get; set; }
        public int? StageFourId { get; set; }
        [ForeignKey("StageFourId")]
        public Personnel? ApproverFour { get; set; }
        public string? StageFourRemarks { get; set; }
        public bool? StageFourIsApprove { get; set; }

    }
}
