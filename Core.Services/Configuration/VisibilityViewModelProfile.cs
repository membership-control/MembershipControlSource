using AutoMapper;
using Core.Data.EF;
using Core.Data.Model;
using Core.Services.DTO;
using Core.Services.DTO.Visibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Core.Services.Configuration
{
    public class VisibilityViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ExportListenerLogShortDTO, ExportListenerLogShort>();
            CreateMap<ExportListenerLogShort, ExportListenerLogShortDTO>();

            CreateMap<POControlTableMasterDTO, TGF_PO_CONTROL_TABLE>()
                .ForSourceMember(sourceMember => sourceMember.FTP_VS_RECEIVE, opt => opt.Ignore())
                .ForMember(des => des.BATCH_ID, opt => opt.Ignore())
                .ForMember(des => des.ORDER_LINENO, opt => opt.Ignore())
                .ForMember(des => des.PRODUCT, opt => opt.Ignore())
                .ForMember(des => des.PRE_PROCESS_STATUS, opt => opt.Ignore())
                .ForMember(des => des.STAGING_STATUS, opt => opt.Ignore());
            CreateMap<TGF_PO_CONTROL_TABLE, POControlTableMasterDTO>()
                .ForMember(desMember => desMember.FTP_RECEIVE_DATE, opt => opt.Ignore())
                .ForSourceMember(src => src.BATCH_ID, opt => opt.Ignore())
                .ForSourceMember(src => src.ORDER_LINENO, opt => opt.Ignore())
                .ForSourceMember(src => src.PRODUCT, opt => opt.Ignore())
                .ForSourceMember(src => src.PRE_PROCESS_STATUS, opt => opt.Ignore())
                .ForSourceMember(src => src.STAGING_STATUS, opt => opt.Ignore());
        }
    }
}
