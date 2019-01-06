using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interface
{
    public interface IFileIO
    {
        string readUNC(string request, string serverpath, string unc);
    }
}
