namespace Utilities.Interfaces
{
    public interface IDayUtility
    {
        decimal CountDays(DateTime? start, DateTime? end, bool? isMandatory = false);
    }
}
