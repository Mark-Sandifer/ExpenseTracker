using ExpenseTracker.Extensions;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _dbContext;

        public HomeController(ILogger<HomeController> logger, AppDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost]
        public List<object> GetExpenseData(string type, int? length)
        {
            List<object> data = new List<object>();
            List<string> labels = new List<string>();
            List<double> amount = new List<double>();
            double total = 0;
            IEnumerable<Expense> expenseList = _dbContext.Expenses.ToList();

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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}