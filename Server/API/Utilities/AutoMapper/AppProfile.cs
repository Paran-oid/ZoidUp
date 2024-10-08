﻿using API.Models;
using API.Models.DTOs;
using AutoMapper;

namespace API.Utilities.AutoMapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<CreateMessageDTO, Message>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<EditMessageDTO, Message>();

        }
    }
}
