﻿using Core.Data.EF;
using Core.Data.Repository;
using Core.Infrastructure.Dev;
using Core.Services.DTO.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interface
{
    public interface IActivity : IRepository<MEM_Activity, DI_WK_TEMPEntities>, IDevGrid
    {
        DevResponse CheckID(string ID);
    }
}
