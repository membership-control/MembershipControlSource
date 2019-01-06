using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Model
{
    public class LogModel
    {
        public System.Guid PK { get; set; }

        public string Insert_User { get; set; }

        public string Client { get; set; }

        public string SessionID { get; set; }

        public string Mode_ID { get; set; }

        public DateTime? Insert_Date { get; set; }

        public string Details { get; set; }

        public string Action { get; set; }

        public Nullable<System.Boolean> Status { get; set; }

        public string Remark { get; set; }

        public string UserID { get; set; }
    }
}
