using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class ExportListenerLogDTO
    {
        public string filename { get; set; }
        public string folderpath { get; set; }
         public string FullFileName {get;set;}
         public string ExportType  {get;set;} 
         public string ArchiveFullFileName  {get;set;} 
        public string Status  {get;set;}
        public DateTime? insert_date  {get;set;} 
        public Guid? GroupID  {get;set;}
        public int? seq  {get;set;} 
        public string eventcode  {get;set;} 
        public string remark   {get;set;}
        public Guid? Setting_PK { get; set; }
        public Guid? ID { get; set; }
       public string EVENT_NAME { get; set; } 
    }
}
