using AutoMapper;
using System.Linq;
using Core.Models;
using Web.ViewModel.AgreedModules;
using Web.ViewModel.Letter;
using Web.ViewModel.Module;
using Web.ViewModel.VendorModule;
using System.Collections.Generic;

namespace Web.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {          
            CreateMap<VendorModuleViewModel, VendorModule>()
               // .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x=>x.CpaModules, opt=>opt.MapFrom(src=>CreateCpaModulesType(src.CpaModulesId)));                 
            
            CreateMap<VendorModule, VendorModuleViewModel>()
                .ForMember(x => x.CpaModulesId, opt => opt.MapFrom(src => src.CpaModules.Select(n => n.Id).ToList()))
                .ForMember(x => x.VendorName, opt => opt.MapFrom(src => src.Vendor));
            
            CreateMap<Letter, LetterViewModel>()
                .ForMember(nameof(LetterViewModel.ExistLetter), opt => opt.MapFrom(src => System.IO.File.Exists(src.PathLetter)));
            
            CreateMap<LetterViewModel, Letter>();
            
            CreateMap<AgreedModule, AgreedModuleViewModel>()
                .ForMember(x => x.NumberLetter, opt => opt.MapFrom(src => src.Letter.NumberLetter))
                  .ForMember(x => x.VendorId, opt => opt.MapFrom(src => src.VendorModule.VendorId))
                  .ForMember(x => x.Vendor, opt => opt.MapFrom(src => src.VendorModule.Vendor))
                  .ForMember(nameof(AgreedModuleViewModel.ExistModule), opt => opt.MapFrom(src => System.IO.File.Exists(src.PathVendorModule)))
                  .ForMember(nameof(AgreedModuleViewModel.DateOfLetter), opt => opt.MapFrom(src => src.Letter.DateOfLetter));
            
            CreateMap<AgreedModuleViewModel, AgreedModule>()
                  .ForPath(x=>x.VendorModule.VendorId, opt => opt.MapFrom(src => src.VendorId));               
            
            CreateMap<AgreedModule, ModuleViewModel>()
                  .ForMember(nameof(ModuleViewModel.ExistModule), opt => opt.MapFrom(src => System.IO.File.Exists(src.PathVendorModule)))
                  .ForMember(nameof(ModuleViewModel.DateOfLetter), opt => opt.MapFrom(src => src.Letter.DateOfLetter))
                  .ForMember(nameof(ModuleViewModel.NumberLetter), opt => opt.MapFrom(src => src.Letter.NumberLetter))
                  .ForMember(nameof(ModuleViewModel.VendorId), opt => opt.MapFrom(src => src.VendorModule.VendorId))
                  .ForMember(nameof(ModuleViewModel.Vendor), opt => opt.MapFrom(src => src.VendorModule.Vendor))
                  .ForMember(nameof(ModuleViewModel.CpaModules), opt => opt.MapFrom(src => src.VendorModule.CpaModules));
        }
        private List<CpaModule> CreateCpaModulesType(List<int> cpaModulesId)
        {
            return Enumerable.Range(1, cpaModulesId.Count)
                .Select((n, i) => new CpaModule() { Id = cpaModulesId[i] }).ToList();
        }
    }
}
