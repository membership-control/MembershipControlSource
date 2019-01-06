using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Visibility
{
    public class ArchiveFolderLogFileDTO
    {
        public string Archived_FilePath { get; set; }
        public Nullable<System.DateTime> insert_date { get; set; }
    }
}
