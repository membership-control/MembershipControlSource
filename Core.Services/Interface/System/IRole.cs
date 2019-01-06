using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data.Model;
using Core.Services.DTO;
using Core.Services.DTO.SystemPage;
using Core.Infrastructure.Dev;

namespace Core.Services.Interface
{
    public interface IRole : IDevGrid
    {
        DevResponse DevRolePermissions(string id, LogModel logmodel = null);
    }
}
