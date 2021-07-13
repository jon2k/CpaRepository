using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class CpaModulesController : Controller
    {
        private readonly ILogger<CpaModulesController> _logger;
        private Repository<CpaModule> _repo;
        public CpaModulesController(Repository<CpaModule> context, ILogger<CpaModulesController> logger)
        {
            _repo = context;
            _logger = logger;
        }
        public ActionResult CpaModule()
        {
            try
            {
                IEnumerable<CpaModule> module = _repo.GetAll();
                return View(module);
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
        public async Task<ActionResult> Create(CpaModule module)
        {
           
            try
            {
                await _repo.AddAsync(module);
                return RedirectToAction(nameof(CpaModule));
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
                var module = _repo.GetById(id);
                return View(module);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(CpaModule));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CpaModule module)
        {
            try
            {
                await _repo.UpdateAsync(module);
                return RedirectToAction(nameof(CpaModule));
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
                return RedirectToAction(nameof(CpaModule));
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var module = _repo.GetById(id);
                await _repo.DeleteAsync(module);
                return RedirectToAction(nameof(CpaModule));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
    }
}
