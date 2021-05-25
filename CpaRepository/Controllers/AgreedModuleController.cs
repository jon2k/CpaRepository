using CpaRepository.EF;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.ViewModel.AgreedModules;
using CpaRepository.ViewModel.VendorModule;
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
    public class AgreedModuleController : Controller
    {
        private readonly ILogger<VendorModuleController> _logger;
        private AgreedModulesRepo _repo;
        public AgreedModuleController(AgreedModulesRepo context, ILogger<VendorModuleController> logger)
        {
            _repo = context;
            _logger = logger;
        }
        public ActionResult AgreedModules()
        {
            try
            {
                var model = _repo.GetAll().Select(n => new AgreedModuleViewModel
                {
                    Id = n.Id,
                    VendorId = n.VendorModule.VendorId,
                    Vendor = n.VendorModule.Vendor,
                    VendorModuleId = n.VendorModuleId,
                    VendorModule = n.VendorModule,
                    CRC = n.CRC,
                    Changes = n.Changes,
                    DateOfAgreement = n.DateOfAgreement,
                    PatchLetter = n.PatchLetter,
                    PatchVendorModule = n.PatchVendorModule,
                    Version = n.Version
                });

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "AgreedModuleController");
            }
        }
        public ActionResult Create()
        {
            try
            {
                var vendor = _repo.GetAllVendors();
                ViewBag.VendorId = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
                var vendorModules= _repo.GetVendorModulesOneVendor(vendor.FirstOrDefault().Id);
                ViewBag.VendorModuleId = vendorModules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AgreedModuleViewModel module)
        {
            try
            {
                var agreedModule = new AgreedModule()
                {
                    VendorModuleId=module.VendorModuleId,
                    Changes=module.Changes,
                    CRC=module.CRC,
                    PatchLetter=module.PatchLetter,
                    DateOfAgreement=module.DateOfAgreement,
                    PatchVendorModule=module.PatchVendorModule,
                    Version=module.Version                 
                };
                  await _repo.AddAsync(agreedModule);
                return RedirectToAction(nameof(AgreedModules));
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
                var vendor = _repo.GetAllVendors();
                //var modules = _repo.GetAllCpaModules().ToList();
                //var cpaModule = modules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                ViewBag.VendorId = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
                var vendorModules = _repo.GetVendorModulesOneVendor(model.VendorModule.VendorId);
                ViewBag.VendorModuleId = vendorModules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                var vm = new AgreedModuleViewModel
                {
                    Id = model.Id,
                    Changes=model.Changes,
                    CRC=model.CRC,
                    DateOfAgreement=model.DateOfAgreement,
                    PatchLetter=model.PatchLetter,
                    PatchVendorModule=model.PatchVendorModule,
                    VendorId=model.VendorModule.VendorId,
                    VendorModuleId=model.VendorModuleId,
                    Version=model.Version
                    
                };

                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModule));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VendorModuleViewModel module)
        {
            try
            {
                var cpaModules = _repo.GetAllCpaModules().Where(p => module.CpaModulesId.Any(l => p.Id == l)).ToList();
                var vendoeModule = _repo.GetById(module.Id);
                //vendoeModule.CpaModules = cpaModules;
                //vendoeModule.NameModule = module.NameModule;
                //vendoeModule.Description = module.Description;

                await _repo.UpdateAsync(vendoeModule);
                return RedirectToAction(nameof(VendorModule), new { id = module.VendorId });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(module.Id);
            }
        }

        public ActionResult GetVendorModules(int id)
        {
            try
            {
                ViewBag.VendorModules = _repo.GetVendorModulesOneVendor(id);
                return PartialView();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
            
        }
       
    }
}
