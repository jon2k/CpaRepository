using Core.Interfaces.EF;
using Core.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Command.VendorModuleController
{
    public class EditVendorModuleCommand : IRequest<VendorModule>
    {
        public VendorModule VendorModule;


        public class EditVendorModuleCommandHandler : IRequestHandler<EditVendorModuleCommand, VendorModule>
        {
            private readonly IVendorModuleRepo _repo;

            public EditVendorModuleCommandHandler(IVendorModuleRepo repo)
            {
                _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            }

            public async Task<VendorModule> Handle(EditVendorModuleCommand request, CancellationToken cancellationToken)
            {
                var cpaModules = _repo.GetAllCpaModules().Where(p => request.VendorModule.CpaModules.Any(l => p.Id == l.Id)).ToList();
                var vendorModule = _repo.GetById(request.VendorModule.Id);
                vendorModule.CpaModules = cpaModules;
                vendorModule.NameModule = request.VendorModule.NameModule;
                vendorModule.Description = request.VendorModule.Description;

                return await _repo.UpdateAsync(vendorModule);
            }
        }
    }
}