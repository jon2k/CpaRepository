using AutoMapper;
using CpaRepository.ViewModel.ActualVendorModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Mediatr.ModulesController;
using Web.Mediatr.Query;

namespace CpaRepository.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ILogger<ModulesController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ModulesController(IMapper mapper, IMediator mediator, ILogger<ModulesController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IActionResult> ActualModules(int id = 0)
        {
            try
            {
                ViewData["Title"] = "Актуальные модули";                           
                var agreedModules = await _mediator.Send(new GetActualModulesQuery() { Id=id});
                agreedModules.IsArchive = false;             
                return View("Modules", agreedModules);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }

        }
        public async Task<IActionResult> ArchiveModules()
        {
            try
            {
                ViewData["Title"] = "Архивные модули";              
                var agreedModules = await _mediator.Send(new GetArchiveModulesQuery());
                agreedModules.IsArchive = true;
                return View("Modules", agreedModules);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetModules(DataForFiltr filtr)
        {
            try
            {              
                var agreedModules = await _mediator.Send(new GetModulesQuery() { Filtr=filtr});
                var vm = _mapper.Map<List<ModuleViewModel>>(agreedModules).OrderByDescending(m => m.DateOfLetter);

                return PartialView(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var file = await _mediator.Send(new GetFileQuery() { Id = id });
                if (file!=null)
                {
                    return file;
                }
                else
                {
                    _logger.LogError("Отсутствует файл письма.");
                    return RedirectToAction(nameof(ActualModules));
                }              
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(ActualModules));
            }
        }      
    }
}
