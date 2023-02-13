using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.Models;
using AutoMapper;

namespace Api.Server.ChuBao.Maps
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Contact, ContactDto>().ReverseMap() ;
            CreateMap<Contact, CreateContactDto>().ReverseMap();
        }
    }
}
