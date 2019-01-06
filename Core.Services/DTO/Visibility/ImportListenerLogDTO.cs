using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
   public  class ImportListenerLogDTO
    {
       public string Filename { get; set; }
       public string FolderPath { get; set; }
       public string FullFileName { get; set; }
       public Guid? Setting_PK { get; set; }
       public string DIJobName { get; set; }
       public string ArchiveFullFileName { get; set; }
       public string Status { get; set; }
       public string insert_user { get; set; }
       public DateTime? insert_date { get; set; }
       public int? seq { get; set; }
       public string SendToFullName { get; set; }
       public string SendToStatus { get; set; }
       public Guid? GroupID { get; set; }
       public string Remark { get; set; }
       public string EventCode { get; set; }
       public string EVENT_NAME { get; set; }
    }
}
