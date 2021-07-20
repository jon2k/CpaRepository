using AutoMapper;
using Core.Interfaces.EF;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.ViewModel.VendorModule;

namespace Web.Mediatr.Query.VendorModuleController
{
    public class GetVmForVendorModuleEditQuery : IRequest<VendorModuleViewModel>
    {
        public int Id { get; set; }

        public class GetVmForVendorModuleEditQueryHandler : IRequestHandler<GetVmForVendorModuleEditQuery, VendorModuleViewModel>
        {
            private readonly IVendorModuleRepo _repo;
            private readonly IMapper _mapper;
            public GetVmForVendorModuleEditQueryHandler(IVendorModuleRepo repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<VendorModuleViewModel> Handle(GetVmForVendorModuleEditQuery request, CancellationToken cancellationToken)
            {

                var model = _repo.GetById(request.Id);
                var modules = _repo.GetAllCpaModules().ToList();
                var cpaModule = modules.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.NameModule,
                    Selected = DefinitionSelectedModule(n, model)
                }).ToList();
                var vm = _mapper.Map<VendorModuleViewModel>(model);
                vm.CpaModulesSelectListItem = cpaModule;

                return vm;
            }
            private bool DefinitionSelectedModule(CpaModule cpaModule, VendorModule vendorModule)
            {
                foreach (var cpa in vendorModule.CpaModules)
                {
                    if (cpaModule.Id == cpa.Id)
                        return true;
                }

                return false;
            }
        }
    }
}
