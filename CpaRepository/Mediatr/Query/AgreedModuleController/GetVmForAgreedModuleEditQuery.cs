using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Interfaces.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModel.AgreedModules;

namespace Web.Mediatr.Query.AgreedModuleController
{
    public class GetVmForAgreedModuleEditQuery : IRequest<AgreedModuleViewModel>
    {
        public int Id { get; set; }

        public class GetVmForAgreedModuleEditQueryHandler : IRequestHandler<GetVmForAgreedModuleEditQuery, AgreedModuleViewModel>
        {
            private readonly IAgreedModulesRepo _repo;
            private readonly IMapper _mapper;
            public GetVmForAgreedModuleEditQueryHandler(IAgreedModulesRepo repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<AgreedModuleViewModel> Handle(GetVmForAgreedModuleEditQuery request, CancellationToken cancellationToken)
            {
              
                var model = _repo.GetById(request.Id);
                var vm = _mapper.Map<AgreedModuleViewModel>(model);
                var vendor = _repo.GetAllVendors();
                vm.VendorsId = vendor.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Name }).ToList();
                var vendorModules = _repo.GetVendorModulesOneVendor(model.VendorModule.VendorId);
                vm.VendorModulesId = vendorModules.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NameModule }).ToList();
                var letters = _repo.GetLettersOneVendor(model.VendorModule.VendorId);
                vm.LettersId = letters.Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.NumberLetter }).ToList();

                return vm;
            }
        }
    }
}
