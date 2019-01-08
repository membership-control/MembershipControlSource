using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data.EF;
using Core.Data.Repository;
using Core.Infrastructure.Dev;

namespace Core.Services.Interface
{
    public interface IRegister : IRepository<MEM_UserActivity, DI_WK_TEMPEntities>, IDevGrid//, IEmail
    {
        Task<DevResponse> UploadForm(byte[] filebinary, bool isEmail = true, object emailModel = null);
        DevResponse LoadDetailGrid(string act_id);
    }
}
