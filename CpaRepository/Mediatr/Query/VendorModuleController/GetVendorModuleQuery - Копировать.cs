using Core.Interfaces.EF;
using Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Query.VendorModuleController
{
    public class GetVendorModuleByIdQuery : IRequest<VendorModule>
    {
        public int Id { get; set; }
        public class GetVendorModuleByIdQueryHandler : IRequestHandler<GetVendorModuleByIdQuery,VendorModule>
        {
            private readonly IVendorModuleRepo _repo;
            public GetVendorModuleByIdQueryHandler(IVendorModuleRepo repo)
            {
                _repo = repo;
            }
            public async Task<VendorModule> Handle(GetVendorModuleByIdQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetById(request.Id);
            }
        }
    }
}
