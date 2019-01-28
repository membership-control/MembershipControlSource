using AutoMapper;
using Core.Data.EF;
using Core.Data.Model;
using Core.Services.DTO.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Services.Configuration
{
    public class AdministrationViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ActivityDTO, MEM_Activity>();
            CreateMap<MEM_Activity, ActivityDTO>();

            CreateMap<RegisterActivityMasterDTO, MEM_Activity>()
                .ForMember(des => des.ACT_Name, opt => opt.MapFrom(src => src.text))
                .ForMember(des => des.ACT_FromDate, opt => opt.MapFrom(src => src.startDate))
                .ForMember(des => des.ACT_ToDate, opt => opt.MapFrom(src => src.endDate))
                .ForMember(src => src.ACT_Address, opt => opt.Ignore())
                .ForMember(src => src.ACT_Category, opt => opt.Ignore())
                .ForMember(src => src.ACT_DOC_ID, opt => opt.Ignore())
                .ForMember(src => src.ACT_Flex1, opt => opt.Ignore())
                .ForMember(src => src.ACT_Flex2, opt => opt.Ignore())
                .ForMember(src => src.ACT_Flex3, opt => opt.Ignore())
                .ForMember(src => src.ACT_Flex4, opt => opt.Ignore())
                .ForMember(src => src.ACT_Flex5, opt => opt.Ignore())
                .ForMember(src => src.ACT_ID, opt => opt.Ignore())
                .ForMember(src => src.ACT_InsertDate, opt => opt.Ignore())
                .ForMember(src => src.ACT_InsertUser, opt => opt.Ignore())
                .ForMember(src => src.ACT_Location, opt => opt.Ignore())
                .ForMember(src => src.ACT_MaxSeat, opt => opt.Ignore())
                .ForMember(src => src.ACT_MemberTypeReq, opt => opt.Ignore())
                .ForMember(src => src.ACT_UpdateDate, opt => opt.Ignore())
                .ForMember(src => src.ACT_UpdateUser, opt => opt.Ignore())
                .ForSourceMember(src => src.MBR_PK, opt => opt.Ignore())
                .ForSourceMember(src => src.MBR_Name, opt => opt.Ignore())
                .ForSourceMember(src => src.MBR_Phone1, opt => opt.Ignore())
                .ForSourceMember(src => src.MBR_Type, opt => opt.Ignore())
                .ForSourceMember(src => src.MBR_Email, opt => opt.Ignore())
                .ForSourceMember(src => src.UAC_AttDate, opt => opt.Ignore())
                .ForSourceMember(src => src.UAC_RegDate, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Remarks, opt => opt.Ignore())
                .ForSourceMember(src => src.Flex1, opt => opt.Ignore())
                .ForSourceMember(src => src.Flex2, opt => opt.Ignore());
            CreateMap<MEM_Activity, RegisterActivityMasterDTO>()
                .ForMember(des => des.text, opt => opt.MapFrom(src => src.ACT_Name))
                .ForMember(des => des.startDate, opt => opt.MapFrom(src => src.ACT_FromDate))
                .ForMember(des => des.endDate, opt => opt.MapFrom(src => src.ACT_ToDate))
                .ForSourceMember(src => src.ACT_Address, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Category, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_DOC_ID, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Flex1, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Flex2, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Flex3, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Flex4, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Flex5, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_ID, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_InsertDate, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_InsertUser, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_Location, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_MaxSeat, opt => opt.Ignore())
                //.ForSourceMember(src => src.ACT_MemberTypeReq, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_UpdateDate, opt => opt.Ignore())
                .ForSourceMember(src => src.ACT_UpdateUser, opt => opt.Ignore())
                .ForMember(src => src.MBR_PK, opt => opt.Ignore())
                .ForMember(src => src.MBR_Name, opt => opt.Ignore())
                .ForMember(src => src.MBR_Phone1, opt => opt.Ignore())
                .ForMember(src => src.MBR_Type, opt => opt.Ignore())
                .ForMember(src => src.MBR_Email, opt => opt.Ignore())
                .ForMember(src => src.UAC_AttDate, opt => opt.Ignore())
                .ForMember(src => src.UAC_RegDate, opt => opt.Ignore())
                .ForMember(src => src.ACT_Remarks, opt => opt.Ignore())
                .ForMember(src => src.Flex1, opt => opt.Ignore())
                .ForMember(src => src.Flex2, opt => opt.Ignore());

            CreateMap<MemberDTO, MEM_Membership>()
                .ForMember(des => des.MBR_Photo, opt => opt.Ignore())
                .ForMember(des => des.MBR_ID, opt => opt.MapFrom(src => Convert.ToInt32(src.MBR_ID)));
            CreateMap<MEM_Membership, MemberDTO>()
                .ForSourceMember(sourceMember => sourceMember.MBR_Photo, opt => opt.Ignore())
                .ForMember(des => des.MBR_ID, opt => opt.MapFrom(src => String.Format("{0:D6}", src.MBR_ID)));

            CreateMap<UserActivityDTO, MEM_UserActivity>()
                .ForSourceMember(src => src.UAC_MBR_Phone, opt => opt.Ignore())
                .ForSourceMember(src => src.UAC_ACT_ID, opt => opt.Ignore());
            CreateMap<MEM_UserActivity, UserActivityDTO>()
                .ForMember(des => des.UAC_MBR_Phone, opt => opt.Ignore())
                .ForMember(des => des.UAC_ACT_ID, opt => opt.Ignore());
        }
    }
}
