using Core.Interfaces.EF;
using Core.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Command
{
    public class CreateVendorModuleCommand : IRequest<VendorModule>
    {
        public VendorModule VendorModule;

        public class CreateVendorModuleCommandHandler : IRequestHandler<CreateVendorModuleCommand, VendorModule>
        {
            private readonly IVendorModuleRepo _repo;

            public CreateVendorModuleCommandHandler(IVendorModuleRepo repo)
            {
                _repo = repo ?? throw new ArgumentNullException(nameof(repo));              
            }

            public async Task<VendorModule> Handle(CreateVendorModuleCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var cpaModules = _repo.GetAllCpaModules().Where(p =>request.VendorModule.CpaModules.Any(l => p.Id == l.Id)).ToList();
                    request.VendorModule.CpaModules = cpaModules;

                    return await _repo.AddAsync(request.VendorModule);
                   
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
