using AutoMapper;
using CpaRepository.Models;
using CpaRepository.ViewModel.ActualVendorModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Mediatr.Query;

namespace CpaRepository.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMapper mapper, IMediator mediator, ILogger<HomeController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var agreedModules = await _mediator.Send(new GetAgreedModulesQuery() { CountElement = 5 });
                var agreedModulesVm = _mapper.Map<IEnumerable<ModuleViewModel>>(agreedModules)
                    .OrderByDescending(m => m.DateOfLetter).ToList();
                return View(agreedModulesVm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(new List<ModuleViewModel>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
