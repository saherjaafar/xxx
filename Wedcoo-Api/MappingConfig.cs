using AutoMapper;
using Core.Dto;
using Core.Models;

namespace Wedcoo_Api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Country,CountryDto>().ReverseMap();
        }
    }
}
