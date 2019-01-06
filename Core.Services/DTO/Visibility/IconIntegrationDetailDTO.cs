using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class IconIntegrationDetailDTO
    {
        public string JobName { get; set; }
        public string process_PK { get; set; }
        public Nullable<System.DateTime> Listener_insert_date { get; set; }
        public string Type { get; set; }
        public string Keyreference { get; set; }
        public Nullable<System.DateTime> DI_Process_insert_date { get; set; }
        public string BATCH_ID { get; set; }
        public Nullable<bool> LISTENER_TRIGGERED { get; set; }
        public Nullable<bool> DI_CALLED { get; set; }
        public string Status { get; set; }
        public string Last_Remarks { get; set; }
        public string DI_Trace { get; set; }
        public string DI_Error { get; set; }
        public Nullable<bool> FILE_EXPORTED { get; set; }
        public string Output_file_name { get; set; }
        public Nullable<bool> CW1_Received { get; set; }
    }
}
