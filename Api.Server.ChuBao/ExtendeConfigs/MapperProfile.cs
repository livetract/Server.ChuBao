using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Api.Server.ChuBao.ExtendeConfigs

{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Contact, ContactDto>().ReverseMap() ;
            CreateMap<Contact, CreateContactDto>().ReverseMap();

            CreateMap<IdentityUser, UserDto>().ReverseMap();
            CreateMap<IdentityUser, RegisterUserDto>().ReverseMap();
        }
    }
}
