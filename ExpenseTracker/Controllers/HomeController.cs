using ExpenseTracker.Extensions;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        [HttpPost]
        public List<object> GetExpenseData(string type, int? length)
        {
            List<object> data = new List<object>();
            List<string> labels = new List<string>();
            List<double> amount = new List<double>();
            double total = 0;
            IEnumerable<Expense> expenseList = dbContext.Expenses.ToList();

            if (length.HasValue && !string.IsNullOrEmpty(type))
            {
                expenseList = expenseList.GetRecentData(type, length.Value);
            }
            var result = expenseList.GroupBy(e => e.Category);

            foreach (var group in result)
            {
                labels.Add(group.Key.ToString());
                total = 0;
                foreach (Expense exp in group)
                {
                    total += exp.Amount;
                }
                amount.Add(total);
            }
            data.AddRange(new object[] { labels, amount });
            return data;
        }

        [HttpPost]
        public List<object> GetCategoryAmount()
        {
            List<object> data = new List<object>();
            List<string> labels = new List<string>();
            List<double> amount = new List<double>();
            IEnumerable<Expense> expenseList = dbContext.Expenses.ToList();

            var query = expenseList.GroupBy(x => x.Category).Select(y => (y.Key, y.Count()));
            foreach (var group in query)
            {
                labels.Add(group.Key.ToString());
                amount.Add(group.Item2);
            }

            data.AddRange(new object[] { labels, amount });
            return data;
        }

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}