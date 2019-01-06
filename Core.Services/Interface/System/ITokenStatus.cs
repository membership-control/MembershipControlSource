using Core.Services.DTO;
using Core.Services.DTO.SystemPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data.Model;

namespace Core.Services.Interface
{
    public interface ITokenStatus:IDatatable
    {
        List<TokenStatusDTO> GetAllDTO();

        Core.Data.Model.Result DevResetToken(Guid[] keys, LogModel logmodel = null);
    }
}
