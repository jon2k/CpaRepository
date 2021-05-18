using CpaRepository.EF;
using CpaRepository.ModelsDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class VendorModuleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _db;
        public VendorModuleController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }
        // GET: SiemensController1
        public ActionResult VendorModule(int id)
        {
            
            int selectedIndex = id==0 ? 1 : id;
            IEnumerable<Vendor> vendors = _db.Vendors.ToList();
            ViewBag.data = vendors;
            var model = _db.VendorModules.Where(n => n.VendorId == selectedIndex).ToList();
            return View(model);
        }

        // GET: SiemensController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SiemensController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiemensController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SiemensController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SiemensController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SiemensController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SiemensController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
