using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interface
{
    public interface IServerStatusMobile
    {
        bool uploadFile(string _uploadFile);
        bool WriteToLogFile(string filename, string content);
    }
}
