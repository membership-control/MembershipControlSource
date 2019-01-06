using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class IconIntegrationMasterDTO
    {
        public Nullable<bool> REPROCESS { get; set; }
        public string Log_PK { get; set; }
        public string fullfilename { get; set; }
        public string e_booking_no { get; set; }
        public string summary_status { get; set; }
        public Nullable<System.DateTime> Icon_insert_date { get; set; }
        public IEnumerable<IconIntegrationDetailDTO> Details { get; set; }
    }
}
