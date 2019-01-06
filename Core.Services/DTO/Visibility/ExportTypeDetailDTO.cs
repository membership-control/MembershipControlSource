using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class ExportTypeDetailDTO
    {
       public string FolderPath  {get;set;} 
       public string ExportType  {get;set;} 
       public string ExportLocation  {get;set;} 
       public string LoginName  {get;set;} 
       public string Password  {get;set;}
       public string flex1 { get; set; }
       public string flex2 { get; set; }
       public string flex3 { get; set; } 
       public int? DelayTime  {get;set;} 
       public string FileNamePattern  {get;set;} 
       public int? FileNamePatternSection  {get;set;} 
       public string FileExtension  {get;set;}
       public Guid? Setting_PK { get; set; } 
    }
}
