using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaRepository.Controllers
{
    public class ActualVendorModuleController : Controller
    {
        public IActionResult ActualModules()
        {

            return View();
        }
    }
}
