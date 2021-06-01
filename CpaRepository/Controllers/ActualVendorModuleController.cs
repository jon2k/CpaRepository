using CpaRepository.Repository;
using CpaRepository.ViewModel.ActualVendorModule;
using CpaRepository.ViewModel.AgreedModules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class ActualVendorModuleController : Controller
    {
        private readonly ILogger<ActualVendorModuleController> _logger;
        private AgreedModulesRepo _repo;

        public ActualVendorModuleController(AgreedModulesRepo context, ILogger<ActualVendorModuleController> logger)
        {
            _repo = context;
            _logger = logger;
        }
        public IActionResult ActualModules()
        {
            var vendor = _repo.GetAllVendors();
            var vendors = new List<SelectListItem>();
            vendors.Add(new SelectListItem() { Value = "0", Text = "Все вендоры" });
            vendors.AddRange(vendor.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList());
            ViewBag.VendorId = vendors;
            
            var cpaModules = _repo.GetAllCpaModules();
            var cpaModulesList = new List<SelectListItem>();
            cpaModulesList.Add(new SelectListItem() { Value = "0", Text = "Все ТПР модули" });
            cpaModulesList.AddRange( cpaModules.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.NameModule
            }).ToList());          
            ViewBag.CpaModuleId = cpaModulesList;
          

            return View(new List<AgreedModuleViewModel>());
        }
        [HttpPost]
        public ActionResult GetActualModules(DataForFiltr test)
        {
            try
            {
               // ViewBag.VendorModules = _repo.GetVendorModulesOneVendor(id);
                return PartialView(new List<AgreedModuleViewModel>());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
        }
    }
}
