using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace Project_HR.Controllers
{
    public class SessionController : Controller
    {
        public SessionController(IOptions<AzureAdB2COptions> b2cOptions)
        {
            AzureAdB2COptions = b2cOptions.Value;
        }

        public AzureAdB2COptions AzureAdB2COptions { get; set; }

        [HttpGet]
        public IActionResult SignIn()
        {
            var redirectUrl = Url.Action(nameof(SessionController.postUserClaims), "Session");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        [HttpGet]
        public IActionResult postUserClaims()
        {
            //ApplicationUser user = new ApplicationUser();
            //foreach (var v in User.Claims)
            //{
            //    if (v.Type.Contains("email"))
            //        user.Email = v.Value;
            //    if (v.Type.Contains("givenname"))
            //        user.Name = v.Value;
            //    if (v.Type.Contains("surname"))
            //        user.Lastname = v.Value;
            //    if (v.Type.Contains("nameidentifier"))
            //        user.NameIdentifier = v.Value;
            //    user.RoleId = (int)Roles.HRUser;
            //}

            (User.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            //var us = User;
            //user.GetRole = Roles.User;

            //if (_userService.Add(user) != null) _userService.Save();

            return Redirect(Url.Action(nameof(ApiController.LogIn), "Home"));
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            var redirectUrl = Url.Action(nameof(ApiController.LogIn), "Home");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            properties.Items[AzureAdB2COptions.PolicyAuthenticationProperty] = AzureAdB2COptions.ResetPasswordPolicyId;
            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var redirectUrl = Url.Action(nameof(ApiController.LogIn), "Home");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            properties.Items[AzureAdB2COptions.PolicyAuthenticationProperty] = AzureAdB2COptions.EditProfilePolicyId;
            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Session", values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(ApiController.LogIn), "Home");
            }

            return View();
        }
    }
}