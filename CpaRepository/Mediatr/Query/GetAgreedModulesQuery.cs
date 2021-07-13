using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Query
{
    public class GetAgreedModulesQuery : IRequest<IEnumerable<AgreedModule>>
    {
        //
        public int CountElement { get; set; }

        public class GetAgreedModuleQueryHandler : IRequestHandler<GetAgreedModulesQuery, IEnumerable<AgreedModule>>
        {
            private readonly IAgreedModulesRepo _repo;
            public GetAgreedModuleQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<AgreedModule>> Handle(GetAgreedModulesQuery request, CancellationToken cancellationToken)
            {
                if (request.CountElement!=0)
                {
                    return _repo.GetAll().OrderByDescending(d => d.Letter.DateOfLetter).Take(request.CountElement);
                }
                else
                    return _repo.GetAll().OrderByDescending(d => d.Letter.DateOfLetter);

            }
        }
    }
}
