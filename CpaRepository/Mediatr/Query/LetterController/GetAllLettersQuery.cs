using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;

namespace Web.Mediatr.Query.LetterController
{
    public class GetAllLettersQuery : IRequest<IEnumerable<Letter>>
    {
        public class GetAllLettersQueryHandler : IRequestHandler<GetAllLettersQuery, IEnumerable<Letter>>
        {
            private readonly ILetterRepo _repo;
            public GetAllLettersQueryHandler(ILetterRepo repo)
            {
                _repo = repo;
            }
            public async Task<IEnumerable<Letter>> Handle(GetAllLettersQuery request, CancellationToken cancellationToken)
            {
                return _repo.GetAll();
            }
        }
    }
}
