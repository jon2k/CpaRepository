using CpaRepository.EF;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
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
        private VendorModuleRepo _db;
        public VendorModuleController(VendorModuleRepo context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }
        // GET: VendorModuleController/VendorModule
        public ActionResult VendorModule(int id)
        {
            try
            {
                if (id == 0) id = 1;
                IEnumerable<Vendor> vendors = _db.GetAllVendors();
                ViewBag.data = vendors;
                ViewBag.IdVendor = id;
                ViewBag.NameVendor = _db.GetNameVendor(id);
                var model = _db.GetVendorModulesOneVendor(id);
                return View(model);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }
        }      

        public ActionResult Create(int id)
        {
            try
            {
                ViewBag.Vendor = _db.GetNameVendor(id);
                ViewBag.VendorId = id;
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendorModule module)
        {
            //ToDo костыль
            module.Id = 0;               
            try
            {
                await _db.AddAsync(module);
                return RedirectToAction(nameof(VendorModule),new { id=module.VendorId});
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var model = _db.GetById(id);
                ViewBag.Vendor = _db.GetNameVendor(model.VendorId);

                return View(model);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = 1 });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VendorModule module)
        {          
            try
            {
                await _db.UpdateAsync(module);
                return RedirectToAction(nameof(VendorModule), new { id = module.VendorId });
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }      

        public ActionResult Delete(int id)
        {
            try
            {
                var module = _db.GetById(id);
                ViewBag.Vendor = _db.GetNameVendor(module.VendorId);
                return View(module);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = 1 });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {                     
            try
            {
                var module = _db.GetById(id);
                await _db.DeleteAsync(module);
                return RedirectToAction(nameof(VendorModule), new { id = module.VendorId });
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
    }
}
