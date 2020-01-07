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
    [Route("[controller]/[action]")]
    public class JobApplicationsController : Controller
    {
        private readonly DataContext _context;

        public JobApplicationsController(DataContext context)
        {
            _context = context;
        }

        // GET: JobApplications
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.JobApplication.Include(j => j.Offer).Include(j => j.User).Include(j => j.State);
            return View(await dataContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            List<JobApplication> searchResult = new List<JobApplication>();
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Claims.FirstOrDefault(x => x.Type == "emails").Value;
                if (User.IsInRole("User"))
                {
                    if (string.IsNullOrEmpty(searchString))
                        return View(await _context.JobApplication.Include(j => j.Offer).Include(j => j.User).Include(j => j.State).
                            Where(j => j.User.EmailAdress == userEmail).ToListAsync());

                    searchResult = await _context.JobApplication.Include(j => j.Offer).Include(j => j.User).Include(j => j.State)
                        .Where(j => j.Offer.JobTitle.Contains(searchString) && j.User.EmailAdress == userEmail).ToListAsync();
                }
                else if (User.IsInRole("HR"))
                {
                    var user = _context.User.FirstOrDefault(x => x.EmailAdress == userEmail);
                    if (string.IsNullOrEmpty(searchString))
                        return View(await _context.JobApplication.Include(j => j.Offer).Include(j => j.User).Include(j => j.State).
                            Where(j => j.Offer.Hrid == user.Id).ToListAsync());

                    searchResult = await _context.JobApplication.Include(j => j.Offer).Include(j => j.User).Include(j => j.State)
                        .Where(j => j.Offer.Hrid != null)
                        .Where(j => j.Offer.JobTitle.Contains(searchString) && j.Offer.Hrid == user.Id).ToListAsync();
                }
                else
                {
                    if (string.IsNullOrEmpty(searchString))
                        return View(await _context.JobApplication.Include(j => j.Offer).Include(j => j.State)
                            .Include(j => j.User).ToListAsync());

                    searchResult = await _context.JobApplication.Include(j => j.Offer).Include(j => j.User).Include(j => j.State)
                        .Where(j => j.Offer.JobTitle.Contains(searchString)).ToListAsync();
                }
            }
            return View(searchResult);
        }
        // GET: JobApplications/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplication
                .Include(j => j.Offer)
                .Include(j => j.User)
                .Include(j => j.State)
                .Include(j => j.Offer.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            //auth
            string userEmail = User.Claims.FirstOrDefault(x => x.Type == "emails").Value;
            var user = _context.User.FirstOrDefault(x => x.EmailAdress == userEmail);
            if (!((user.Id == jobApplication.UserId && User.IsInRole("User")) ||(user.Id == jobApplication.Offer.Hrid && User.IsInRole("HR"))))
                return RedirectToAction("Index", "Home");
            return View(jobApplication);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeState(int? appId, int? option)
        {
            if (!User.IsInRole("HR"))
                return RedirectToAction("Index", "Home");
            if (option == null || appId == null)
            {
                return BadRequest($"id should not be null");
            }
            var app = await _context.JobApplication.FirstOrDefaultAsync(x => x.Id == appId.Value);
            app.StateId = option.Value;
            _context.Update(app);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "JobApplications", new { id = appId });
        }



        [ApiExplorerSettings(IgnoreApi = true)]
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
            string userEmail = User.Claims.FirstOrDefault(x => x.Type == "emails").Value;
            var user = _context.User.FirstOrDefault(x => x.EmailAdress == userEmail);
            JobApplication ja = new JobApplication
            {
                Id = model.Id,
                OfferId = model.OfferId,
                UserId = user.Id,
                CvUrl = model.CvUrl,
                ContactAgreement = model.ContactAgreement,
                StateId = 1 //pending
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var app = await _context.JobApplication.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (app == null)
            {
                return NotFound($"offer not found in DB");
            }

            return View(app);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobApplication model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var app = await _context.JobApplication.FirstOrDefaultAsync(x => x.Id == model.Id);
            app.ContactAgreement = model.ContactAgreement;
            app.CvUrl = model.CvUrl;
            _context.Update(app);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // GET: JobApplications/Delete/5
        [ApiExplorerSettings(IgnoreApi = true)]
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
