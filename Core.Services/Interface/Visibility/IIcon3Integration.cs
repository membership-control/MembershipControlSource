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
    public interface IIcon3Integration : IFileIO
    {
        DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, string filter, LogModel logmodel);
        DevResponse DevChartDataset(string filter, LogModel logmodel);

        string ReprocessFile(string file, string unc, LogModel logmodel = null);
    }
}
