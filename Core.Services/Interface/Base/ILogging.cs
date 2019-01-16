using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data.Model;
using Core.Infrastructure.Dev;

namespace Core.Services.Interface
{
    public interface ILogging : IDisposable
    {
        DevResponse LogDB(LogModel db_model);
    }
}
