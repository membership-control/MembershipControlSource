using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.DTO.Generic;

namespace Core.Services.Interface
{
    public interface IEmail
    {
        Task SendFormattedHTMLEmailAsync(HTMLEmailViewModel model);
    }
}
