using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class VendorsController : Controller
    {
        private readonly ILogger<VendorsController> _logger;
        private Repository<Vendor> _repo;
        public VendorsController(Repository<Vendor> context, ILogger<VendorsController> logger)
        {
            _repo = context;
            _logger = logger;
        }
        public ActionResult Vendor()
        {
            try
            {
                IEnumerable<Vendor> vendors = _repo.GetAll();             
                return View(vendors);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }
        }
        
        public ActionResult Create()
        {
                     
             return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Vendor vendor)
        {
            try
            {
                await _repo.AddAsync(vendor);
                return RedirectToAction(nameof(Vendor));
            }
            catch (Exception e)
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
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Vendor));
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(Vendor vendor)
        {
            try
            {
                await _repo.UpdateAsync(vendor);
                return RedirectToAction(nameof(Vendor));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
        
        public ActionResult Delete(int id)
        {
            try
            {
                var vendor = _repo.GetById(id);
              //  ViewBag.Vendor = _db.GetNameVendor(module.VendorId);
                return View(vendor);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Vendor));
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
                return RedirectToAction(nameof(Vendor));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
    }
}
