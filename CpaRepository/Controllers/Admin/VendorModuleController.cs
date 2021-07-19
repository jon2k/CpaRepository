using AutoMapper;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Web.Mediatr.Command;
using Web.Mediatr.Query.VendorModuleController;
using Web.ViewModel.VendorModule;

namespace Web.Controllers.Admin
{
    public class VendorModuleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<VendorModuleController> _logger;
        public VendorModuleController(IMapper mapper, IMediator mediator, ILogger<VendorModuleController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));         
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ActionResult> VendorModule(int id)
        {
            try
            {
                var vm = await _mediator.Send(new GetVendorModuleQuery() { Id = id });
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index), "HomeController");
            }
        }

        public async Task<ActionResult> Create(int id)
        {
            try
            {
                var vm = await _mediator.Send(new GetVmForVendorModuleCreateQuery() { Id = id });
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = id });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(VendorModuleViewModel vm)
        {
            try
            {
                vm.Id = 0;
                var vendorModule = await _mediator.Send(new CreateVendorModuleCommand()
                {
                    VendorModule = _mapper.Map<VendorModule>(vm)
                });

                return RedirectToAction(nameof(VendorModule), new { id = vm.VendorId });
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
                var vm = await _mediator.Send(new GetVmForVendorModuleEditQuery() { Id = id });

                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = 1 });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VendorModuleViewModel vm)
        {
            try
            {
                var vendorModule = await _mediator.Send(new EditVendorModuleCommand()
                {
                    VendorModule = _mapper.Map<VendorModule>(vm)
                });
               
                return RedirectToAction(nameof(VendorModule), new { id = vm.VendorId });
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
                var module = await _mediator.Send(new GetVendorModuleByIdQuery() { Id = id });
                return View(module);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(VendorModule), new { id = 1 });
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var res = await _mediator.Send(new DeleteVendorModuleCommand() { Id = id });
                return RedirectToAction(nameof(VendorModule), new { id = res.Item2 });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return View();
            }
        }
    }
}
