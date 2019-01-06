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
    public interface IUser 
    {
        DevResponse GetAllUsers(System.Collections.Specialized.NameValueCollection request, LogModel logmodel = null);
        DevResponse DevUserPermissions(string id, LogModel logmodel = null);
        DevResponse DevDBFilter(string id, LogModel logmodel = null);
        bool DeleteUser(string id, LogModel logmodel = null);
        DevResponse RoleLookup(LogModel logmodel = null);
    }
}
