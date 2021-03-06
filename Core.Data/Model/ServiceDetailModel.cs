﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Model
{
    public class ServiceDetailModel
    {
        public Guid? TGF_GI_ControlTower_Server_Status_Setting_PK { get; set; }
        public string Server_Name { get; set; }
        public string Setting_Detail { get; set; }
        public string Type{ get; set; }
        public System.Boolean IsEnabled { get; set; }
        public string insert_user { get; set; }
        public DateTime insert_date { get; set; }
        public string update_user { get; set; }
        public DateTime? update_date { get; set; }
    }
}
