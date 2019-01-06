using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class ImportListenerLogDetailDTO
    {
        public string Mode { get; set; }
        public string Status { get; set; }
        public string EVENT_name { get; set; }
        public string Remark { get; set; }
        public DateTime? insert_date { get; set; }
    }
}
