
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class LeaveTypes
    {
		[Key]
		public int? LeaveTypeID {get;set;}
		public string? LeaveName {get;set;}
		public int? AccrualPerMonth {get;set;}
		public int? MaxPerPeriod {get;set;}
		public string? ResetType {get;set;}
		public bool? Accumulation {get;set;}

    }
}
