using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class ExportListenerLogDetailDTO
    {
        public string EVENT_NAME { get; set; }
        public string Status { get; set; }
        public DateTime? insert_Date{ get; set; }        
        public string Remark{ get; set; }
    }
}
