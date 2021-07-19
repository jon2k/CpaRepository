using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;

namespace Web.Mediatr.Query.ModulesController
{
    public class GetVendorModuleOneVendorQuery : IRequest<IEnumerable< VendorModule>>
    {
        public int Id { get; set; }

        public class GetVendorModuleOneVendorQueryHandler : IRequestHandler<GetVendorModuleOneVendorQuery, IEnumerable<VendorModule>>
        {
            private readonly IAgreedModulesRepo _repo;

            public GetVendorModuleOneVendorQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<VendorModule>> Handle(GetVendorModuleOneVendorQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetVendorModulesOneVendor(request.Id);
            }
        }
    }
}
