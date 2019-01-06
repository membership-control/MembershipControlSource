using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.Services.DTO.Generic
{
    public class HTMLEmailViewModel
    {
        [EmailAddress]
        public string Destination { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
