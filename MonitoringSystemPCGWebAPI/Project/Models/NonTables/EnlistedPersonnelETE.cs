using Models;

namespace Models.NonTables
{
    public class EnlistedPersonnelETE : Personnel
    {
        public int? YearsInService { get; set; }
        public DateTime? DateOfLatestReEnlistment { get; set; }
        public DateTime? NextETE { get; set; }
        public double? ETEDaysRemaining { get; set; }
        public string? Remarks { get; set; }

        public string? EmailCategory { get; set; }
        public string? SupportingDocument { get; set; }
    }
}
