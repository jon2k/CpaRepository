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
        // GET: VendorModuleController/VendorModule
        public ActionResult VendorModule(int id)
        {
            
            int selectedIndex = id==0 ? 1 : id;
            IEnumerable<Vendor> vendors = _db.Vendors.ToList();
            ViewBag.data = vendors;
            ViewBag.IdVendor = selectedIndex;
            ViewBag.NameVendor = _db.Vendors.Where(n => n.Id == selectedIndex).FirstOrDefault().Name;
            var model = _db.VendorModules.Where(n => n.VendorId == selectedIndex).ToList();
            return View(model);
        }

        // GET: VendorModuleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VendorModuleController/Create
        public ActionResult Create(int id)
        {
            ViewData["Vendor"] = _db.Vendors.Where(n => n.Id == id).FirstOrDefault().Name;
            ViewBag.VendorId = id;
            return View();
        }

        // POST: VendorModuleController/Create
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(VendorModule module)
        {
            //ToDo костыль
            module.Id = 0;    
            _db.Add(module);
            _db.SaveChanges();

            try
            {
                return RedirectToAction(nameof(VendorModule),new { id=module.VendorId});
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {
            
            var model=_db.VendorModules.Where(n => n.Id == id).FirstOrDefault();
            ViewData["Vendor"] = _db.Vendors.Where(n => n.Id == model.VendorId).FirstOrDefault().Name;
           
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(VendorModule module)
        {          
            _db.VendorModules.Update(module);       
            _db.SaveChanges();
            try
            {
                return RedirectToAction(nameof(VendorModule), new { id = module.VendorId });
            }
            catch
            {
                return View();
            }
        }      

        // GET: VendorModuleController/Delete/5
        public ActionResult Delete(int id)
        {
           var module= _db.VendorModules.Where(n => n.Id == id).FirstOrDefault();
            ViewData["Vendor"] = _db.Vendors.Where(n => n.Id == module.VendorId).FirstOrDefault().Name;
            return View(module);
        }

        // POST: VendorModuleController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var module=_db.VendorModules.Find(id);
            _db.VendorModules.Remove(module);
            _db.SaveChanges();
            try
            {
                return RedirectToAction(nameof(VendorModule), new { id = module.VendorId });
            }
            catch
            {
                return View();
            }
        }
    }
}
