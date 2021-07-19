using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Interfaces.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModel.Letter;

namespace Web.Mediatr.Query.LetterController
{
    public class GetVmForLetterEditQuery : IRequest<LetterViewModel>
    {
        public int Id { get; set; }

        public class GetVmForLetterEditQueryHandler : IRequestHandler<GetVmForLetterEditQuery, LetterViewModel>
        {
            private readonly ILetterRepo _repo;
            private readonly IMapper _mapper;
            public GetVmForLetterEditQueryHandler(ILetterRepo repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<LetterViewModel> Handle(GetVmForLetterEditQuery request, CancellationToken cancellationToken)
            {
                var model = _repo.GetById(request.Id);
                var vm = _mapper.Map<LetterViewModel>(model);
                var vendor = _repo.GetAllVendors();
                vm.Vendors = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
               
                return vm;
            }
        }
    }
}
