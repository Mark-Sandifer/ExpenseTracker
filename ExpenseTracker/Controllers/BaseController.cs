using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class BaseController<T> : Controller where T : BaseController<T>
    {
        private ILogger<T>? _logger;
        private AppDBContext? _dBContext;

        protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
        protected AppDBContext dbContext => _dBContext = (AppDBContext)HttpContext.RequestServices.GetRequiredService(typeof(AppDBContext));
        public async Task<IActionResult> checkForNullAsync(int? id)
        {
            if (id == null || dbContext.Expenses == null)
            {
                return NotFound();
            }

            var expense = await dbContext.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }
    }
}