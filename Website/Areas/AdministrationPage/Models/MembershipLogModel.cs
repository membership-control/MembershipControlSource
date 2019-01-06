using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Services.DTO.Visibility;

namespace WebMembership.Areas.AdministrationPage.Models
{
    public class MembershipLogModel
    {
        public List<ImportListenerLogDTO> Details { get; set; }
        public List<ImportListenerLogDetailDTO> DetailsGrid { get; set; }

    }
}