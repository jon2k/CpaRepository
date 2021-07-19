using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Admin
{
    public class VendorsController : Controller
    {
        private readonly ILogger<VendorsController> _logger;
        private IRepository<Vendor> _repo;
        public VendorsController(IRepository<Vendor> context, ILogger<VendorsController> logger)
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
                return View(vendor);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Vendor));
            }
        }

        [HttpPost, ActionName("Delete")]
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
