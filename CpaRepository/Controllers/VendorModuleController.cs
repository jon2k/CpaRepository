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
        private VendorModuleRepo _repo;
        public VendorModuleController(VendorModuleRepo context, ILogger<HomeController> logger)
        {
            _repo = context;
            _logger = logger;
        }

        public ActionResult VendorModule(int id)
        {
            try
            {
                if (id == 0) id = 1;
                IEnumerable<Vendor> vendors = _repo.GetAllVendors();
                ViewBag.data = vendors;
                ViewBag.IdVendor = id;
                ViewBag.NameVendor = _repo.GetNameVendor(id);
                var model = _repo.GetVendorModulesOneVendor(id);
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
                ViewBag.Vendor = _repo.GetNameVendor(id);
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
                await _repo.AddAsync(module);
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
                var model = _repo.GetById(id);
                ViewBag.Vendor = _repo.GetNameVendor(model.VendorId);

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
                await _repo.UpdateAsync(module);
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
                var module = _repo.GetById(id);
                ViewBag.Vendor = _repo.GetNameVendor(module.VendorId);
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
                var module = _repo.GetById(id);
                await _repo.DeleteAsync(module);
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
