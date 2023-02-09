using ExpenseTracker.Models;
using System.Data;

namespace ExpenseTracker.Extensions
{
    public static class EnumberableExtension
    {
        public static IEnumerable<Expense> GetRecentData(this IEnumerable<Expense>dataSet, string type, int length) =>
                                                            type.ToLowerInvariant() switch
                                                            {
                                                                "day" => dataSet.FilterDays(length),
                                                                "week" => dataSet.FilterDays(length*7),
                                                                "month" => dataSet.FilterMonths(length),
                                                                "year" => dataSet.FilterYears(length),
                                                                _=>throw new ArgumentOutOfRangeException(nameof(type))
                                                            };
        
        private static IEnumerable<Expense> FilterDays(this IEnumerable<Expense> dataSet, int length) => 
                                                            dataSet.Where(ex => ex.Date > DateTime.UtcNow.AddDays(-length));

        private static IEnumerable<Expense> FilterMonths(this IEnumerable<Expense> dataSet, int length) => 
                                                            dataSet.Where(ex => ex.Date > DateTime.UtcNow.AddMonths(-length));

        private static IEnumerable<Expense> FilterYears(this IEnumerable<Expense> dataSet, int length) => 
                                                            dataSet.Where(ex => ex.Date > DateTime.UtcNow.AddYears(-length));
    }
}
