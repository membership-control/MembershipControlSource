using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Services.DTO.Visibility;

namespace WebMembership.Areas.AdministrationPage.Models
{
    public class ActivityLogModel
    {
        public List<ExportListenerLogDTO> Details { get; set; }
        public List<ExportListenerLogDetailDTO> DetailsGrid { get; set; }
    }
}