using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Project_HR.Controllers
{
    public class ApiController : Controller
    {
        public AzureAdB2COptions AzureAdB2COptions { get; set; }

        public ApiController(IOptions<AzureAdB2COptions> b2cOptions)
        {
            AzureAdB2COptions = b2cOptions.Value;
        }

        public IActionResult LogIn()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "Api");
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "JobOffer");
            else if (User.IsInRole("HR"))
                return RedirectToAction("Index", "JobOffer");
            else if (User.IsInRole("User"))
                return RedirectToAction("Index", "JobOffer");
            else return SignOut();
        }
 
        [HttpGet]
        public IActionResult SignIn()
        {
            var redirectUrl = Url.Action(nameof(ApiController.LogIn), "Api");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }


        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(ApiController.LogIn), "Api", values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}