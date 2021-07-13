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
    public class GetAllCpaModulesQuery : IRequest<IEnumerable<CpaModule>>
    {
        public class GetAllCpaModulesQueryHandler : IRequestHandler<GetAllCpaModulesQuery, IEnumerable<CpaModule>>
        {
            private readonly IAgreedModulesRepo _repo;
            public GetAllCpaModulesQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<CpaModule>> Handle(GetAllCpaModulesQuery request, CancellationToken cancellationToken)
            {               
                return _repo.GetAllCpaModules();
            }
        }
    }
}
