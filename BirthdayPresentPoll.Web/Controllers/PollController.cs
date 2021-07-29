using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BirthdayPresentPoll.Web.Data;
using Microsoft.AspNet.Identity;
using BirthdayPresentPoll.Web.Models;
using System.Web.Http;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using ActionNameAttribute = Microsoft.AspNetCore.Mvc.ActionNameAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace BirthdayPresentPoll.Web.Controllers
{
    [Authorize]
    public class PollController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PollController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string currentUserId = User.Identity.GetUserId();

            var applicationDbContext = _context.Polls
                .Where(x => x.CelebrantId != currentUserId)
                .Include(p => p.Celebrant)
                .Include(p => p.Initiator);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .Include(p => p.Celebrant)
                .Include(p => p.Initiator)
                .Include(p => p.Votes)
                .ThenInclude(p => p.Present)
                .FirstOrDefaultAsync(m => m.Id == id);

            string currentUserId = User.Identity.GetUserId();
            var hasVoteCast = poll.Votes.FirstOrDefault(v => v.VoterId == currentUserId) != null;

            if (poll == null)
            {
                return NotFound();
            }

            var votes = poll.Votes
                .GroupBy(p => p.Present.Name)
                .ToDictionary(g => g.Key, g => g.ToList());

            var presents = await _context.Presents.ToListAsync();

            //// -1 because the person for whom the poll is cannot vote for himself
            var totalVotesCount = await _context.Users.CountAsync() - 1;

            var model = new PollModel()
            {
                Id = poll.Id,
                IsActive = poll.IsActive,
                HasVoteCast = hasVoteCast,
                CelebrantName = poll.Celebrant.UserName,
                InitiatorName = poll.Initiator.UserName,
                VotesNotCastCount = totalVotesCount - votes.Count,
                Votes = votes
            };

            ViewData["Presents"] = new SelectList(presents, "Id", "Name");

            return View(model);
        }

        public IActionResult Create()
        {
            string currentUserId = User.Identity.GetUserId();
            var users = _context.Users.Where(u => u.Id != currentUserId);

            ViewData["Celebrant"] = new SelectList(users, "Id", "UserName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CelebrantId")] Poll poll)
        {
            var celebrant = await _context.Users.FirstOrDefaultAsync(u => u.Id == poll.CelebrantId);
            var existingPoll = await _context.Polls
                .Include(p => p.Celebrant)
                .Where(p => p.IsActive == true)
                .FirstOrDefaultAsync(p => p.Celebrant.Birthday == celebrant.Birthday);

            if (existingPoll is not null)
            {
                return BadRequest(new HttpError("There is an ongoing vote for this date or person."));
            }

            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                poll.InitiatorId = currentUserId;
                poll.IsActive = true;

                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CelebrantId"] = new SelectList(_context.Users, "Id", "UserName", poll.CelebrantId);

            return View(poll);
        }

        [HttpPost]
        public async Task<IActionResult> Vote([Bind("PollId, PresentId")]Vote vote)
        {
            var poll = await _context.Polls
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.Id == vote.PollId);

            if (poll is null)
            {
                return NotFound("Poll was not found");
            }

            if (!poll.IsActive)
            {
                return BadRequest("Cannot cast vote for inactive poll");
            }

            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();

                if (poll.Votes.FirstOrDefault(v => v.VoterId == currentUserId) is not null)
                {
                    return BadRequest(new HttpError("This user has already voted for this poll"));
                }

                vote.VoterId = currentUserId;
                poll.Votes.Add(vote);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Stop(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var poll = await _context.Polls.FirstOrDefaultAsync(m => m.Id == id);
            poll.IsActive = false;

            _context.Update(poll);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .Include(p => p.Celebrant)
                .Include(p => p.Initiator)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (poll is null)
            {
                return NotFound();
            }

            return View(poll);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poll = await _context.Polls.FindAsync(id);

            if (poll.IsActive)
            {
                return BadRequest(new HttpError("Cannot delete an ongoing poll. You must stop it first."));
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.Id == id);
        }
    }
}
