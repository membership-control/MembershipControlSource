﻿using Core.Services.DTO;
using Core.Services.DTO.SystemPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMembership.Areas.SystemPage.Models
{
    public class ServiceStatusPageModel
    {
        public List<ServiceDetailDTO> Details { get; set; }
    }
}