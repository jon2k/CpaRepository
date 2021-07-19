using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Mediatr.Command;
using Web.Mediatr.Query;
using Web.Mediatr.Query.AgreedModuleController;
using Web.Mediatr.Query.LetterController;
using Web.Mediatr.Query.ModulesController;
using Web.ViewModel.AgreedModules;

namespace Web.Controllers.Admin
{
    public class AgreedModulesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<VendorModuleController> _logger;

        public AgreedModulesController(IMapper mapper, IMediator mediator, ILogger<VendorModuleController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ActionResult> AgreedModules()
        {
            try
            {
                var agreedModules = await _mediator.Send(new GetAgreedModulesQuery());
                var agreedModulesVM = _mapper.Map<IEnumerable<AgreedModuleViewModel>>(agreedModules);
                return View(agreedModulesVM);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "AgreedModuleController");
            }
        }
        
        public async Task<ActionResult> Create()
        {
            try
            {
                var vm = await _mediator.Send(new GetVmForAgreedModuleCreateQuery());
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }
       
        [HttpPost]
        public async Task<ActionResult> Create(AgreedModuleViewModel vm)
        {
            try
            {
                if (vm.FileModule != null)
                {                 
                    var module = await _mediator.Send(new CreateAgreedModuleCommand()
                    {
                        AgreedModule = _mapper.Map<AgreedModule>(vm),
                        FileModule = vm.FileModule
                    });
                    if (module != null)
                    {
                        _logger.LogInformation($"Добавлен согласованный модуль. " +
                           $"Вендор - {module.VendorModule.Vendor.Name}, " +
                           $"Модуль - {module.VendorModule.NameModule}, " +
                           $"Версия - {module.Version}, " +
                           $"CRC - {module.CRC}, " +
                           $"Время - {DateTime.Now}");
                        return RedirectToAction(nameof(AgreedModules));
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    _logger.LogError("Отсуствует файл.");
                    return View();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
        
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var vm = await _mediator.Send(new GetVmForAgreedModuleEditQuery() { Id = id });
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AgreedModuleViewModel vm)
        {
            try
            {
                var res= await _mediator.Send(new EditAgreedModuleCommand()
                {
                    AgreedModule = _mapper.Map<AgreedModule>(vm),
                    FileModule = vm.FileModule
                });
               
                return RedirectToAction(nameof(AgreedModules));

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(vm.Id);
            }
        }
        
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var model = await _mediator.Send(new GetAgreedModuleByIdQuery() { Id = id });
                var vm = _mapper.Map<AgreedModuleViewModel>(model);

                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _mediator.Send(new DeleteAgreedModuleCommand() { Id = id });              
                return RedirectToAction(nameof(AgreedModules));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }

        public async Task<ActionResult> GetVendorModules(int id)
        {
            try
            {
                var vm = await _mediator.Send(new GetVendorModuleOneVendorQuery() { Id=id});
                return PartialView(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return PartialView();
            }
        }
        
        public async Task<ActionResult> GetLetters(int id)
        {
            try
            {
                var vm = await _mediator.Send(new GetLettersOneVendorQuery() { Id = id });
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
                if (file != null)
                {
                    return file;
                }
                else
                {
                    _logger.LogError("Отсутствует файл письма.");
                    return RedirectToAction(nameof(AgreedModules));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(AgreedModules));
            }
        }
       
    }
}
