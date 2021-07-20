using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Admin
{
    public class CpaModulesController : Controller
    {
        private readonly ILogger<CpaModulesController> _logger;
        private IRepository<CpaModule> _repo;
        public CpaModulesController(IRepository<CpaModule> repo, ILogger<CpaModulesController> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
