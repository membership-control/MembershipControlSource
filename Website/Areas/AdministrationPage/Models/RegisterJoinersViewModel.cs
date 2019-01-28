using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Services.DTO.Administration;

namespace WebMembership.Areas.AdministrationPage.Models
{

    public class RegisterJoinersViewModel
    {
        public string Data_Type { get; set; }
        public string Title_Name { get; set; }
        public List<RegisterActivityGridDTO> DataGrid { get; set; }
    }
}