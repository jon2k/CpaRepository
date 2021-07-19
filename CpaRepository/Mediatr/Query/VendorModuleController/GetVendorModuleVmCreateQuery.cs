using Core.Interfaces.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.ViewModel.VendorModule;

namespace Web.Mediatr.Query.VendorModuleController
{
    public class GetVmForVendorModuleCreateQuery : IRequest<VendorModuleViewModel>
    {
        public int Id { get; set; }

        public class GetVmForVendorModuleCreateQueryHandler : IRequestHandler<GetVmForVendorModuleCreateQuery, VendorModuleViewModel>
        {
            private readonly IVendorModuleRepo _repo;
            public GetVmForVendorModuleCreateQueryHandler(IVendorModuleRepo repo)
            {
                _repo = repo;
            }
            public async Task<VendorModuleViewModel> Handle(GetVmForVendorModuleCreateQuery request, CancellationToken cancellationToken)
            {
                var modules = _repo.GetAllCpaModules().ToList();
                var cpaModule = modules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                var vendorModule = new VendorModuleViewModel()
                {
                    VendorName = _repo.GetNameVendor(request.Id),
                    VendorId = request.Id,
                    CpaModulesSelectListItem = cpaModule,
                    CpaModulesId = modules.Select(n => n.Id).ToList()
                };
                return vendorModule;
            }
        }
    }
}
