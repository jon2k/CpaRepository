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
        private Repository<AgreedModule> _repo;
        public AgreedModuleController(Repository<AgreedModule> context, ILogger<VendorModuleController> logger)
        {
            _repo = context;
            _logger = logger;
        }
        public ActionResult AgreedModules()
        {
            try
            {
                
                //var modules = _repo.GetAll();
                var model = _repo.GetAll().Select(n => new AgreedModuleViewModel
                {
                    Id = n.Id,
                    VendorId=n.VendorModule.VendorId,
                    Vendor=n.VendorModule.Vendor,
                    VendorModuleId=n.VendorModuleId,
                    VendorModule=n.VendorModule,
                    CRC=n.CRC,
                    Changes=n.Changes,
                    DateOfAgreement=n.DateOfAgreement,
                    PatchLetter=n.PatchLetter,
                    PatchVendorModule=n.PatchVendorModule,
                    Version=n.Version
                });
                //ViewBag.data = vendors;
                //ViewBag.IdVendor = id;
                //ViewBag.NameVendor = _repo.GetNameVendor(id);
                //var model = _repo.GetVendorModulesOneVendor(id);
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }
        }
    }
}
