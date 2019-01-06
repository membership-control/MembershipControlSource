using Core.Data.EF;
using Core.Data.Repository;
using Core.Infrastructure.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interface
{
   public interface IDatatable
    {
       /// <summary>
       /// datatable CURD
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
       DtResponse DTData(System.Collections.Specialized.NameValueCollection request);
    }
}
