using Core.Data.EF;
using Core.Data.Repository;
using Core.Services.DTO.SystemPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Dev;
using Core.Data.Model;

namespace Core.Services.Interface
{
    public interface IServerStatus
    {
        List<ServerDetailDTO> GetAllData();
    }
}
