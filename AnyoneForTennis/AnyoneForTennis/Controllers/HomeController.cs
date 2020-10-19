using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Authorization;
using AnyoneForTennis.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace AnyoneForTennis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDBContext context, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (this.User.IsInRole("Admin"))
            {
                HttpContext.Session.SetString("Role", "Admin");
            }
            else if (this.User.IsInRole("Coach"))
            {
                HttpContext.Session.SetString("Role", "Coach");

                foreach (var item in _context.Coaches)
                {
                    if (item.Username.CompareTo(this.User.Identity.Name) == 0)
                    {
                        HttpContext.Session.SetString("Id", item.CoachId.ToString());
                        break;
                    }
                }
            }
            else
            {
                HttpContext.Session.SetString("Role", "Member");
            }
            
            return Redirect("Events/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //Todo Remove later
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //-----------------------------------------------------------------------------------------------------------------------
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (user.UserName.CompareTo("Admin") == 0)
                {
                    var member = new Member
                    {
                        Username = model.Username,
                        Email = model.Email,
                        IsAdmin = true
                    };

                    _context.Members.Add(member);
                    _context.SaveChanges();

                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    if (model.IsCoach)
                    {
                        //is a coach
                        var member = new Coach
                        {
                            Name = model.Name,
                            Age = model.Age,
                            Biography = model.Biography,
                            Username = model.Username
                        };

                        _context.Coaches.Add(member);
                        _context.SaveChanges();

                        await _userManager.AddToRoleAsync(user, "Coach");
                    }
                    else
                    {
                        //is a member
                        var member = new Member
                        {
                            Username = model.Username,
                            Email = model.Email,
                            IsAdmin = false
                        };

                        _context.Members.Add(member);
                        _context.SaveChanges();

                        await _userManager.AddToRoleAsync(user, "Member");
                    }
                }
                

                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError("Login Failed");
                }

            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
