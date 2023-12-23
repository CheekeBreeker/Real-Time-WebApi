using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTWebApi.Data;
using RTWebApi.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace RTWebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChatDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ChatDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.UserName == name);
            if(user == null)
            {
                user = new User() { UserName = name }; 

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }

            var claims = new List<Claim>
            {
                new Claim("Id", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var messages = _dbContext.Messages
                .Include(p => p.User)
                .OrderByDescending(x => x.Id).Take(50).ToList();

            return View(messages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}