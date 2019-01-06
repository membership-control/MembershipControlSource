using Core.Infrastructure.Dev;
using Core.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interface
{
    public interface IDevGrid
    {
        DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, LogModel logmodel = null);
    }
}
