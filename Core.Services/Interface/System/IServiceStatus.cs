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
    public interface IServiceStatus
    {
        Infrastructure.Dev.DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, LogModel logmodel = null);
        List<ServiceDetailDTO> GetAllData();
        bool StartService(string key, bool action, LogModel logmodel = null);
    }
  
}

