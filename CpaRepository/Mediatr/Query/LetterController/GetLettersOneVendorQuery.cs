using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;

namespace Web.Mediatr.Query.LetterController
{
    public class GetLettersOneVendorQuery : IRequest<IEnumerable<Letter>>
    {
        public int Id { get; set; }

        public class GetLettersOneVendorQueryHandler : IRequestHandler<GetLettersOneVendorQuery, IEnumerable<Letter>>
        {
            private readonly IAgreedModulesRepo _repo;

            public GetLettersOneVendorQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<Letter>> Handle(GetLettersOneVendorQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetLettersOneVendor(request.Id);
            }
        }
    }
}
