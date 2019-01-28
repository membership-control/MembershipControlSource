using Core.Data.EF;
using Core.Data.Repository;
using Core.Infrastructure.Dev;
using Core.Data.Model;
using Core.Services.DTO.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interface
{
    public interface IMember : IRepository<MEM_Membership, DI_WK_TEMPEntities>, IDevGrid
    {
        DevResponse CheckID(string ID);
        DevResponse ExportMembers();
        DevResponse ImportMembers(byte[] filebinary, string filename = null, LogModel logmodel = null);
    }
}
