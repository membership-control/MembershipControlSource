using AutoMapper;
using Core.Data.Model;
using Core.Data.EF;
using Core.Services.DTO;
using Core.Services.DTO.SystemPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Configuration
{
    public class SystemViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TokenStatusDTO, TokenStatusModel>();
            CreateMap<TokenStatusModel, TokenStatusDTO>();

            CreateMap<RoleDTO, AspNetRole>()
                .ForSourceMember(sourceMember => sourceMember.Users, opt => opt.Ignore());
            CreateMap<AspNetRole, RoleDTO>()
                .ForMember(desMember => desMember.Users, opt => opt.Ignore());
        }

    }
}
