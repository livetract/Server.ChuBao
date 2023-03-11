using AutoMapper;
using Core.Server.ChuBao.DTOs;
using Data.Server.Chubao.Entities;
using Data.Server.ChuBao.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Server.ChuBao.Commons

{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<Contact, CreateContactDto>().ReverseMap();

            CreateMap<IdentityUser, UserDto>().ReverseMap();
            CreateMap<IdentityUser, RegisterUserDto>().ReverseMap();

            CreateMap<Record, RecordDto>().ReverseMap();
            CreateMap<Record, CreateRecordDto>().ReverseMap();
        }
    }
}
