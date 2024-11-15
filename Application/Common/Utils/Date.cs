namespace Application.Common.Utils
{
    public static class Date
    {
        public static DateTime UtcToLocal(DateTime date)
        {
            return date.AddHours(-3);
        }
        public static DateTime? UtcToLocal(DateTime? manifestResolvedAt)
        {
            return manifestResolvedAt?.AddHours(-3);
        }
    }
}
