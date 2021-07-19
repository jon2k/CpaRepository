using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModel.Letter;

namespace Web.Mediatr.Query.LetterController
{
    public class GetVmForLetterCreateQuery : IRequest<LetterViewModel>
    {


        public class GetVmForLetterCreateQueryHandler : IRequestHandler<GetVmForLetterCreateQuery, LetterViewModel>
        {
            private readonly IAgreedModulesRepo _repo;
            public GetVmForLetterCreateQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<LetterViewModel> Handle(GetVmForLetterCreateQuery request, CancellationToken cancellationToken)
            {
                var vm = new LetterViewModel() { DateOfLetter=System.DateTime.Now};
                var vendor = _repo.GetAllVendors();
                vm.Vendors = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
                return vm;
            }
        }
    }
}
