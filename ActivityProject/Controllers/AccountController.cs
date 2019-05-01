using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ActivityProject.Models;
using ActivityProject.Services;
using ActivityProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActivityProject.Controllers
{
    public class AccountController : Controller
    {
        private IProfessorData _professorData;

        public AccountController(IProfessorData professorData)
        {
            _professorData = professorData;
        }

        public IActionResult Index()
        {
            Console.WriteLine("Sandra");
            return View("Login");
        }

        public async Task<IActionResult> OnPostAsync(AccountLogin model)
        {
            Console.WriteLine("Sandra");
            if (ModelState.IsValid)
            {
                var isValid = _professorData.CheckProfessor(model.Username, model.Password); //proveris iz baze
                if (!isValid)
                {
                    ModelState.AddModelError("error", "Ne postoji nastavnik sa unetim korisničkim imenom i/ili lozinkom.");
                    return View("login");
                }
                Professor professor = _professorData.FindByUsername(model.Username);
                // Create the identity from the user info
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.Username));
                identity.AddClaim(new Claim(ClaimTypes.Name, model.Username));
                HttpContext.Session.SetString("professor", professor.FirstName + " " + professor.LastName);
                Console.WriteLine(HttpContext.Session.GetString("professor"));
                // Authenticate using the identity
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberMe });

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }


    }
}