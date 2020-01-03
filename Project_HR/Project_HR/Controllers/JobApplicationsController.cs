using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_HR.Models;

namespace Project_HR.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly DataContext _context;

        public JobApplicationsController(DataContext context)
        {
            _context = context;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.JobApplication.Include(j => j.Offer).Include(j => j.User);
            return View(await dataContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.JobApplication.Include(j => j.Offer).Include(j => j.User).ToListAsync());

            List<JobApplication> searchResult = await _context.JobApplication.Include(j => j.Offer).Include(j => j.User)
                .Where(j => j.Offer.JobTitle.Contains(searchString)).ToListAsync();
            return View(searchResult);
        }
        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplication
                .Include(j => j.Offer)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        public async Task<IActionResult> Create(int? offerId)
        {
            if (offerId == null)
            {
                return BadRequest($"id should not be null");
            }
            var model = new JobApplication();
            model.Offer = await _context.JobOffer.FirstOrDefaultAsync(x => x.Id == offerId);
            model.OfferId = offerId ?? default(int);
            model.Offer.Company = await _context.Company.FirstOrDefaultAsync(x => x.Id == model.Offer.CompanyId);
            return View(model);
        }


        // POST: JobApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobApplication model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            JobApplication ja = new JobApplication
            {
                Id = model.Id,
                OfferId = model.OfferId,
                UserId = 2,
                CvUrl = model.CvUrl,
                ContactAgreement = model.ContactAgreement
            };
            ja.User = await _context.User.FirstOrDefaultAsync(x => x.Id == ja.UserId);
            ja.Offer = await _context.JobOffer.FirstOrDefaultAsync(x => x.Id == ja.OfferId);
            ja.Offer.Company = await _context.Company.FirstOrDefaultAsync(x => x.Id == ja.Offer.CompanyId);
            await _context.JobApplication.AddAsync(ja);
            ja.Offer.JobApplication.Add(ja);
            _context.Update(ja.Offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: JobApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplication.FindAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.JobOffer, "Id", "Description", jobApplication.OfferId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "EmailAdress", jobApplication.UserId);
            return View(jobApplication);
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OfferId,ContactAgreement,CvUrl,UserId")] JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.Id))
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
            ViewData["OfferId"] = new SelectList(_context.JobOffer, "Id", "Description", jobApplication.OfferId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "EmailAdress", jobApplication.UserId);
            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplication
                .Include(j => j.Offer)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplication.FindAsync(id);
            _context.JobApplication.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplication.Any(e => e.Id == id);
        }
    }
}
