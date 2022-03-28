using Bookshop.Data;
using Bookshop.Models;
using Bookshop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bookshop.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homeVM = new HomeVM()
            {
                Books = await _context.Books.Include(b=>b.Author).ToListAsync()
                //Authors = await _context.Authors.ToListAsync()
            };


            return View(homeVM);
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