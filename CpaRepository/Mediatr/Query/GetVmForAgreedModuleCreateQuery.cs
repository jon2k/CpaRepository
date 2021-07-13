using CpaRepository.ModelsDb;
using CpaRepository.Repository;
using CpaRepository.ViewModel.AgreedModules;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mediatr.Query
{
    public class GetVmForAgreedModuleCreateQuery : IRequest<AgreedModuleViewModel>
    {


        public class GetVmForAgreedModuleCreateQueryHandler : IRequestHandler<GetVmForAgreedModuleCreateQuery, AgreedModuleViewModel>
        {
            private readonly IAgreedModulesRepo _repo;
            public GetVmForAgreedModuleCreateQueryHandler(IAgreedModulesRepo repo)
            {
                _repo = repo;
            }
            public async Task<AgreedModuleViewModel> Handle(GetVmForAgreedModuleCreateQuery request, CancellationToken cancellationToken)
            {
                var vm = new AgreedModuleViewModel();
                var vendor = _repo.GetAllVendors();
                vm.VendorsId = vendor.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();
                var vendorModules = _repo.GetVendorModulesOneVendor(vendor.FirstOrDefault().Id);
                vm.VendorModulesId = vendorModules.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.NameModule
                }).ToList();
                var letters = _repo.GetLettersOneVendor(vendor.FirstOrDefault().Id);
                vm.LettersId = letters.Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.NumberLetter
                }).ToList();
                return vm;
            }
        }
    }
}
