namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Category Category { get; set; }
        public double Amount { get; set; }
        public string? Note { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public enum Category
    {
        Bill,
        Travel,
        Shopping,
        EatingOut,
        Health,
        Holiday,
        Other
    }
}
