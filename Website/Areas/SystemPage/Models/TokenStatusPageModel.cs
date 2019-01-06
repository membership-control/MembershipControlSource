using Core.Services.DTO;
using Core.Services.DTO.SystemPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMembership.Areas.SystemPage.Models
{
    public class TokenStatusPageModel
    {
        public List<TokenStatusDTO> Details { get; set; }

        //public List<ImportListenerSettingDTO> DashBoards { get; set; }
        public List<ServiceDetailDTO> DashBoards { get; set; }
    }
}