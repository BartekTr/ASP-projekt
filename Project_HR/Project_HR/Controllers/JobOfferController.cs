﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_HR.Models;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Project_HR.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            _context = context;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCompany(int id = 1)
        {
            if (id != 0)
                return Json(_context.Company.FirstOrDefault(x => x.Id == id).Name);
            else
                return Json("nazwa");
        }


        [HttpGet]
        public PagingViewModel GetData(int pageSize = 4, string search = "", int pageNo = 1, string option = "JobTitle", string offers = "All")
        {
            int totalPage, totalRecord;
            List<JobOffer> res;
            if (search != null)
                res = _context.JobOffer.Include(x => x.JobApplication).Where(x => x.JobTitle.Contains(search)).ToList();
            else
                res = _context.JobOffer.Include(x => x.JobApplication).ToList();

            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                string userEmail = User.Claims.FirstOrDefault(x => x.Type == "emails").Value;
                User user = _context.User.FirstOrDefault(x => x.EmailAdress == userEmail);
                switch (offers)
                {
                    case "All":
                        break;
                    case "Applied":
                        res = res.Where(x => x.JobApplication.Where(y => y.UserId == user.Id).ToList().Count >= 1).ToList();
                        break;
                    case "NotApplied":
                        res = res.Where(x => x.JobApplication.Where(y => y.UserId == user.Id).ToList().Count < 1).ToList();
                        break;
                    default:
                        break;
                }
            }
            totalRecord = res.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            List<JobOffer> record;
            switch (option)
            {
                case "JobTitle":
                    record = (from u in res
                              orderby u.JobTitle
                              select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Location":
                    record = (from u in res
                              orderby u.Location
                              select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Created":
                    record = (from u in res
                              orderby u.Created
                              select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Company":
                    record = (from u in res
                              orderby u.CompanyId
                              select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    record = (from u in res
                              orderby u.SalaryFrom, u.SalaryTo
                              select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            PagingViewModel empData = new PagingViewModel
            {
                JobOffers = record,
                TotalPage = totalPage
            };

            return empData;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var offer = await _context.JobOffer.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (offer == null)
            {
                return NotFound($"offer not found in DB");
            }

            return View(offer);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Applicate(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var offer = await _context.JobOffer.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (offer == null)
            {
                return NotFound($"offer not found in DB");
            }

            return RedirectToAction("Create", "JobApplications", new { offerId = id });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> EditApplicate(int? id2)
        {
            if (id2 == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            return RedirectToAction("Edit", "JobApplications", new { id = id2 });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var offer = await _context.JobOffer.FirstOrDefaultAsync(x => x.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id should not be null");
            }
            var offer = _context.JobOffer.FirstOrDefault(x => x.Id== id.Value);
            if (offer.Hrid != null)
            {
                var user = _context.User.FirstOrDefault(x => x.Id == offer.Hrid);
                if (_context.JobOffer.Where(x => x.Hrid == user.Id).Count() == 1)
                {
                    user.RoleId = 3; //user
                    _context.Update(user);
                }
            }
            var apps = _context.JobApplication.Where(x => x.OfferId == id.Value);
            _context.JobApplication.RemoveRange(apps);
            _context.JobOffer.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await _context.Company.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = await _context.Company.ToListAsync();
                return View(model);
            }

            JobOffer jo = new JobOffer
            {
                Id = model.Id,
                CompanyId = model.CompanyId,
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };

            await _context.JobOffer.AddAsync(jo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffer.Include(j => j.JobApplication).Include(j => j.Hr)
                .FirstOrDefaultAsync(x => x.Id == id && x.Company != null);
            ViewBag.LinkableId = -1;
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Claims.FirstOrDefault(x => x.Type == "emails").Value;
                var user = _context.User.FirstOrDefault(x => x.EmailAdress == userEmail);
                var a = offer.JobApplication.FirstOrDefault(x => x.UserId == user.Id);
                if (a != null)
                    ViewBag.LinkableId = a.Id;
            }
            offer.Company = await _context.Company.FirstOrDefaultAsync(x => x.Id == offer.CompanyId);
            return View(offer);
        }
    }
}
