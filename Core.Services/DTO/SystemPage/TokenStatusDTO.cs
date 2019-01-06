using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.SystemPage
{
    public class TokenStatusDTO
    {
        public Guid? Setting_PK { get; set; }
        public Guid? PK { get; set; }
        public Guid? batch_id { get; set; }
        public string Status { get; set; }
        public string insert_user { get; set; }
        public string update_user { get; set; }
        public string FileFullName { get; set; }
        public DateTime? update_date { get; set; }
        public DateTime? insert_date { get; set; }
    }

}
