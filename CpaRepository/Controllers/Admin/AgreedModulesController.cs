using AutoMapper;
using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.ViewModel.AgreedModules;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Mediatr.AgreedModulesController;
using Web.Mediatr.Command;
using Web.Mediatr.Query;

namespace CpaRepository.Controllers
{
    public class AgreedModulesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<VendorModuleController> _logger;

        public AgreedModulesController(IMapper mapper, IMediator mediator, IAgreedModulesRepo context, ILogger<VendorModuleController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
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
        public async Task<ActionResult> Create(AgreedModuleViewModel agreedModuleVM)
        {
            try
            {
                if (agreedModuleVM.FileModule != null)
                {
                    var agreedModule = _mapper.Map<AgreedModule>(agreedModuleVM);
                    var module = await _mediator.Send(new EditAgreedModuleCommand()
                    {
                        AgreedModule = agreedModule,
                        FileModule = agreedModuleVM.FileModule
                    });
                    if (module != null)
                    {
                        _logger.LogInformation($"Добавлен согласованный модуль. " +
                           $"Вендор - {agreedModule.VendorModule.Vendor.Name}, " +
                           $"Модуль - {agreedModule.VendorModule.NameModule}, " +
                           $"Версия - {agreedModule.Version}, " +
                           $"CRC - {agreedModule.CRC}, " +
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
        public async Task<ActionResult> Edit(AgreedModuleViewModel module)
        {
            try
            {
                var agreedModule = _mapper.Map<AgreedModule>(module);
                var res= await _mediator.Send(new EditAgreedModuleCommand()
                {
                    AgreedModule = agreedModule,
                    FileModule = module.FileModule
                });
               
                return RedirectToAction(nameof(AgreedModules));

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View(module.Id);
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
                var module = await _mediator.Send(new GetAgreedModuleByIdQuery() { Id = id });
                if (module.PathVendorModule != null)
                {
                    return PhysicalFile(module.PathVendorModule, "application/octet-stream", module.PathVendorModule.Split('\\').Last());
                }
                else
                {
                    _logger.LogError("Отсутствует полный путь к файлу письма.");
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
