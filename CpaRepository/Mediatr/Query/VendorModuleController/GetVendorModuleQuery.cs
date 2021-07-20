using Core.Interfaces.EF;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.ViewModel.VendorModule;

namespace Web.Mediatr.Query.VendorModuleController
{
    public class GetVendorModuleQuery : IRequest<VendorModuleVM>
    {
        public int Id { get; set; }
        public class GetVendorModuleQueryHandler : IRequestHandler<GetVendorModuleQuery,VendorModuleVM>
        {
            private readonly IVendorModuleRepo _repo;
            public GetVendorModuleQueryHandler(IVendorModuleRepo repo)
            {
                _repo = repo;
            }
            public async Task<VendorModuleVM> Handle(GetVendorModuleQuery request, CancellationToken cancellationToken)
            {
                if (request.Id == 0) request.Id = 1;               
                return new VendorModuleVM()
                {
                    VendorId = request.Id,
                    VendorName = _repo.GetNameVendor(request.Id),
                    VendorModules = _repo.GetVendorModulesOneVendor(request.Id).ToList()
                };
            }
        }
    }
}
