using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_HR.Models;

namespace Project_HR.Controllers
{
    [Route("[controller]/[action]")]
    public class HRController : Controller
    {
        private readonly DataContext _context;

        public HRController(DataContext context)
        {
            _context = context;
        }
        // GET: HR
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home");
            HRListViewModel model = new HRListViewModel
            {
                Users = _context.User.Where(x => x.Role.Name != "Admin").ToList(),
                Offer = _context.JobOffer.FirstOrDefault(x => x.Id == id)
            };

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Choose(int? offerId, int? userId)
        {
            if (offerId == null || userId == null)
            {
                return BadRequest($"id should not be null");
            }
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);
            if (user.RoleId == 3) //user role
            {
                var appsToRemove = await _context.JobApplication.Where(x => x.UserId == user.Id).ToListAsync();
                _context.JobApplication.RemoveRange(appsToRemove);
            }
            user.RoleId = 2//hr role;
            _context.Update(user);
            var offer = await _context.JobOffer.FirstOrDefaultAsync(x => x.Id == offerId);
            offer.Hrid = user.Id;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "JobOffer", new { id = offerId });
        }

        [HttpPost]
        public async Task<ActionResult> Remove(int? offerId, int? userId)
        {
            if (offerId == null || userId == null)
            {
                return BadRequest($"id should not be null");
            }
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);
            if (_context.JobOffer.Where(x => x.Hrid == userId).Count() <= 1)
                user.RoleId = 3; //user role
            _context.Update(user);
            var offer = await _context.JobOffer.FirstOrDefaultAsync(x => x.Id == offerId);
            offer.Hrid = null;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "JobOffer", new { id = offerId });
        }

    }
}

