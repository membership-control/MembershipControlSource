using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebMembership.Areas.AdministrationPage.Models
{
    public class RegisterAdminViewModel
    {
        public string[] ServicesSource { get; set; }
        public string imageClass { get; set; }
    }
}