using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class ExportListenerLogShortDTO
    {
        public System.Guid ID { get; set; }
        public string FullFileName { get; set; }
        public string Status { get; set; }
    }
}
