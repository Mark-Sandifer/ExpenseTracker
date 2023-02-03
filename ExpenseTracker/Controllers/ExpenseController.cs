using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly AppDBContext _context;

        public ExpenseController(AppDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public List<object> GetExpenseData()
        {
            List<object> data = new List<object>();
            double total = 0;
            List<string> labels = new List<string>();
            List<double> amount = new List<double>();
            IEnumerable <Expense> expenseList = _context.Expenses.ToList();

            var result = expenseList.GroupBy(e => e.Category);

            foreach(var group in result)
            {
                labels.Add(group.Key.ToString());
                total = 0;
                foreach(Expense exp in group)
                {
                    total = total + exp.Amount;
                    Debug.WriteLine(exp.Date);
                }
                amount.Add(total);
            }

            data.Add(labels);
            data.Add(amount);

            return data;
        }

        [HttpPost]
        public List<object> GetPastWeekData()
        {
            List<object> data = new List<object>();
            double total = 0;
            List<string> labels = new List<string>();
            List<double> amount = new List<double>();
            IEnumerable<Expense> expenseList = _context.Expenses.ToList();

            var result = expenseList.Where(x => x.Date > DateTime.Today.AddDays(-7)).GroupBy(e => e.Category);

            foreach (var group in result)
            {
                labels.Add(group.Key.ToString());
                total = 0;
                foreach (Expense exp in group)
                {
                    total = total + exp.Amount;
                    Debug.WriteLine(exp.Date);
                }
                amount.Add(total);
            }

            data.Add(labels);
            data.Add(amount);

            return data;
        }

        // GET: Expense
        public async Task<IActionResult> Index()
        {
              return _context.Expenses != null ? 
                          View(await _context.Expenses.ToListAsync()) :
                          Problem("Entity set 'AppDBContext.Expenses'  is null.");
        }

        // GET: Expense/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expense/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Category,Amount,Note,Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expense/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,Amount,Note,Date")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expense/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Expenses == null)
            {
                return Problem("Entity set 'AppDBContext.Expenses'  is null.");
            }
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
          return (_context.Expenses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
