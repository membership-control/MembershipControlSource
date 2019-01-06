using Core.Data.EF;
using Core.Data.Repository;
using Core.Infrastructure.DataTables;
using Core.Data.Model;
using Core.Infrastructure.Dev;
using Core.Services.DTO.Visibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interface
{
    public interface IPOControl : IRepository<TGF_PO_CONTROL_TABLE, TGF_IntegrationEntities>, IDevGrid, IFileIO
 //   public interface IPOControl : IRepository<UserSetting, DCT_MangementEntities>, IDevGrid, IFileIO
    {
        DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, string filter, LogModel logmodel);
        DevResponse DevPageDetails(string range, string CustomerID, string OrderNo, int OrderSplit, string SupplierCode, LogModel logmodel = null);
        DevResponse DevChartDataByCustomer(string filter, LogModel logmodel);
        DevResponse DevChartDataFTPVsProcessing(string filter, LogModel logmodel);
        DevResponse DevChartDataOrderStatus(string filter, LogModel logmodel);
        DevResponse DevSelectBoxClientData(LogModel logmodel = null);

        bool ReprocessFile(string file, string destinationPath, string unc, LogModel logmodel = null);
    }
}
