using AutoMapper;
using CpaRepository.EF;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
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
    public class VendorModuleController : Controller
    {
        private readonly ILogger<VendorModuleController> _logger;
        private VendorModuleRepo _repo;
        public VendorModuleController(VendorModuleRepo context, ILogger<VendorModuleController> logger)
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
                var modules = _repo.GetAllCpaModules().ToList();
                var cpaModule = modules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();          
                var vendorModule = new VendorModuleViewModel() {
                    CpaModules = cpaModule, 
                    CpaModulesId=modules.Select(n=>n.Id).ToArray() };           
                return View(vendorModule);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VendorModuleViewModel module)
        {
            try
            {
                var cpaModules = _repo.GetAllCpaModules().Where(p => module.CpaModulesId.Any(l => p.Id == l)).ToList();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<VendorModuleViewModel, VendorModule>()
                  .ForMember(nameof(CpaRepository.ModelsDb.VendorModule.CpaModules), opt => opt.MapFrom(src => cpaModules))
                  .ForMember(nameof(CpaRepository.ModelsDb.VendorModule.Id), opt => opt.MapFrom(src => 0)));
                var mapper = new Mapper(config);
                var vendorModule = mapper.Map<VendorModule>(module);

                await _repo.AddAsync(vendorModule);
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
                var modules = _repo.GetAllCpaModules().ToList();
                var cpaModule = modules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                ViewBag.Vendor = _repo.GetNameVendor(model.VendorId);
                
                var config = new MapperConfiguration(cfg => cfg.CreateMap<VendorModule, VendorModuleViewModel>()
                   .ForMember(nameof(VendorModuleViewModel.CpaModulesId), opt => opt.MapFrom(src => src.CpaModules.Select(n=>n.Id).ToArray()))
                   .ForMember(nameof(VendorModuleViewModel.CpaModules), opt => opt.MapFrom(src => cpaModule)));
                var mapper = new Mapper(config);
                var vm = mapper.Map<VendorModuleViewModel>(model);

                return View(vm);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = 1 });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VendorModuleViewModel module)
        {          
            try
            {
                var cpaModules = _repo.GetAllCpaModules().Where(p => module.CpaModulesId.Any(l => p.Id == l)).ToList();
                var vendoeModule = _repo.GetById(module.Id);
                vendoeModule.CpaModules = cpaModules;
                vendoeModule.NameModule = module.NameModule;
                vendoeModule.Description = module.Description;
                
                await _repo.UpdateAsync(vendoeModule);
                return RedirectToAction(nameof(VendorModule), new { id = module.VendorId });
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return View(module.Id);
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
