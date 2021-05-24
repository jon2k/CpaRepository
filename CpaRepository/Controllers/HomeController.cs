using CpaRepository.EF;
using CpaRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _db;
        public HomeController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
           // var res = _db.Vendors.ToList();
            return View();
        }
        public IActionResult AddModule()
        {
            int selectedIndex = 1;
            SelectList states = new SelectList(_db.Vendors, "Id", "Name", selectedIndex);
            ViewBag.States = states;
            SelectList cities = new SelectList(_db.VendorModules.Where(c => c.VendorId == selectedIndex), "Id", "NameModule");
            ViewBag.Cities = cities;
            return View();
            
        }
        public ActionResult GetItems(int id)
        {
            return PartialView(_db.VendorModules.Where(c => c.VendorId == id).ToList());
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
