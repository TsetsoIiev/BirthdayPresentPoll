using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BirthdayPresentPoll.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace BirthdayPresentPoll.Web.Controllers
{
    [Authorize]
    public class PresentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PresentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Present
        public async Task<IActionResult> Index()
        {
            return View(await _context.Presents.ToListAsync());
        }

        // GET: Present/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var present = await _context.Presents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (present == null)
            {
                return NotFound();
            }

            return View(present);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Present present)
        {
            if (ModelState.IsValid)
            {
                _context.Add(present);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(present);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var present = await _context.Presents.FindAsync(id);
            if (present == null)
            {
                return NotFound();
            }
            return View(present);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Present present)
        {
            if (id != present.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(present);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresentExists(present.Id))
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
            return View(present);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var present = await _context.Presents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (present == null)
            {
                return NotFound();
            }

            return View(present);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var present = await _context.Presents.FindAsync(id);
            _context.Presents.Remove(present);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresentExists(int id)
        {
            return _context.Presents.Any(e => e.Id == id);
        }
    }
}
