using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Query
{
    public class GetAllVendorsQuery : IRequest<IEnumerable<Vendor>>
    {
        public class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQuery, IEnumerable<Vendor>>
        {
            private readonly IAgreedModulesRepo _repo;
            public GetAllVendorsQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<Vendor>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetAllVendors();
            }
        }
    }
}
