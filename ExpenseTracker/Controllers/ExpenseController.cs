using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : BaseController<ExpenseController>
    {
        public async Task<IActionResult> Index() => dbContext.Expenses != null ? 
                                                    View(await dbContext.Expenses.ToListAsync()) :
                                                    Problem("Entity set 'AppDBContext.Expenses'  is null.");
        public async Task<IActionResult> Details(int? id) => await checkForNullAsync(id);
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Category,Amount,Note,Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(expense);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }
        public async Task<IActionResult> Edit(int? id) =>  await checkForNullAsync(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,Amount,Note,Date")] Expense expense)
        {
            if (id != expense.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(expense);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id)) return NotFound() ?? throw new ApplicationException("Edit could not be completed");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }
        public async Task<IActionResult> Delete(int? id) => await checkForNullAsync(id);

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Expenses == null) return Problem("Entity set 'AppDBContext.Expenses'  is null.");
            var expense = await dbContext.Expenses.FindAsync(id);
            if (expense != null) dbContext.Expenses.Remove(expense);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ExpenseExists(int id) => (dbContext.Expenses?.Any(e => e.Id == id)).GetValueOrDefault(); 
    }
}