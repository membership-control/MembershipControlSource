using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.EF;
using Core.Data.Repository;
using Core.Infrastructure.DataTables;
using Core.Infrastructure.Specification;
using Core.Infrastruture.Specification;
using Core.Infrastruture.Specification.Implementation;
using Core.Services.DTO.Visibility;
using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Core.Infrastructure.Utility;
using Core.Data.UnitOfWork;
using Core.Infrastructure.Dev;
using Core.Data.Model;
using System.Configuration;

namespace Core.Services.Service
{
    internal class OrderStatusChart
    {
        public string STATUS { get; set; }
        public int ORDERNO { get; set; }
    }

    internal class ClientNameObject
    {
        public string Display { get; set; }
        public string Folderpath { get; set; }
    }

    public class POControlService : Repository<TGF_PO_CONTROL_TABLE, TGF_IntegrationEntities>, IPOControl
    {
        private IMapper _Imapper;
        private IntegrationUnitOfWork _unitofWork;
        //private DWUnitOfWork _dwUnitofWork;
        private DCTUnitOfWork _dctUnitofwork;
        private string _DCTdbserver, _CW1dbserver;


        public POControlService(IntegrationUnitOfWork work, IMapper iImapper, DCTUnitOfWork dct_work) : base(work)
        {
            this._Imapper = iImapper;
            this._dctUnitofwork = dct_work;
            this._unitofWork = work;
            //this._dwUnitofWork = dw_work;
            CustomerIDFilter = new List<string>();


            this._DCTdbserver = System.Configuration.ConfigurationManager.AppSettings["DTCserver"];
            this._CW1dbserver = System.Configuration.ConfigurationManager.AppSettings["CW1server"];

        }

        #region Properties
        private int FilterRange { get; set; }
        
        private List<string> CustomerIDFilter { get; set; }
        #endregion

        #region CRUD methods

        private DevResponse PageData(DevRequest request,string userid)
        {
            DevResponse response = new DevResponse();
            IQueryable<POControlTableMasterDTO> data =
this.GetAll().UseAsDataSource(this._Imapper).For<POControlTableMasterDTO>();

            Specification<POControlTableMasterDTO> specs = base.DevGridFilters<POControlTableMasterDTO>(request.Filters);
            //var filteredData = data.Where(specs.SatisfiedBy());
            //var filterDate = DateTime.Now.AddDays(FilterRange * -1);
            //filteredData = filteredData.Where(po => po.RECEIVE_DATE.HasValue && po.RECEIVE_DATE.Value > filterDate);
            
            //if (request.Sorts.Count > 0)
            //{
            //    foreach (var sort in request.Sorts)
            //    {
            //        filteredData = filteredData.OrderBy<POControlTableDTO>(sort.fieldname, sort.desc);
            //    }
            //}
            //else
            //{
            //    filteredData = filteredData.OrderByDescending(orderby => orderby.BATCH_ID);
            //}

            //var dataPage = filteredData.Skip(request.Skip).Take(request.Take);

//            var dataPageNew = dataPage.Select(p => new
//            {
//                ORDERNO = p.ORDERNO,
//                ORDER_LINENO = p.ORDER_LINENO,
//                ORDER_SPLIT = p.ORDER_SPLIT,
//                BATCH_ID = p.BATCH_ID,
//                SOURCE_FILE_NAME = p.SOURCE_FILE_NAME,
//                SUPPLIER_CODE = p.SUPPLIER_CODE,
//                PRODUCT = p.PRODUCT,
//                CUSTOMER_ID = p.CUSTOMER_ID,
//                RECEIVE_DATE = p.RECEIVE_DATE,
//                PRE_PROCESS_STATUS = p.PRE_PROCESS_STATUS,
//                STAGING_STATUS = p.STAGING_STATUS,
//                FILE_GENERATED = p.FILE_GENERATED,
//                TRANSFER_STATUS = p.TRANSFER_STATUS,
//                IN_EDIENTERPRISE = p.IN_EDIENTERPRISE,
//                EDI_ORD_LINE_PK = p.EDI_ORD_LINE_PK,
//                EDI_LAST_EDIT_TIME = p.EDI_LAST_EDIT_TIME,
//                COMMENT = p.COMMENT,
//                FTP_RECEIVE_DATE = p.FTP_RECEIVE_DATE,
//                FTP_VS_RECEIVE = !p.FTP_RECEIVE_DATE.HasValue ? "Unable to calculate" : (
//                (p.RECEIVE_DATE.Value - p.FTP_RECEIVE_DATE.Value).TotalHours >= 72 ? "Over 72" :
//                                ((p.RECEIVE_DATE.Value - p.FTP_RECEIVE_DATE.Value).TotalHours >= 48 ? "Over 48" :
//                                    (
//                                    (p.RECEIVE_DATE.Value - p.FTP_RECEIVE_DATE.Value).TotalHours >= 24 ? "Over 24" : "< 24"
//                                    )
//                                )
//                                )
//            });

//            response.data = dataPageNew.ToList();

            //if (request.RequireTotalCount)
            //    response.totalCount = filteredData.Count();

            //// linq entities joining method
            //try
            //{
            //    var dataPage = filteredData.ToList();
                
            //    var cw1_orders = this._enterpriseUnitofWork.CreateSet<JobOrderHeader>()
            //                            .Where(p => p.JD_SystemLastEditTimeUtc.HasValue && DbFunctions.DiffDays(p.JD_SystemLastEditTimeUtc.Value, DateTime.UtcNow).Value < 365)
            //                        .GroupBy(x => new { x.JD_OrderNumber, x.JD_OrderNumberSplit })
            //                        .Select(g => new
            //                        {
            //                            JD_OrderNumber = g.Key.JD_OrderNumber,
            //                            JD_OrderNumberSplit = (int)g.Key.JD_OrderNumberSplit,
            //                            JD_SystemLastEditTimeUtc = g.Select(o => o.JD_SystemLastEditTimeUtc).Max().Value
            //                        }).ToList();

            //var final_data =
            //    from p in dataPage
            //    join o in cw1_orders on new { OrderNo = p.ORDERNO, OrderSplit = p.ORDER_SPLIT } equals new { OrderNo = o.JD_OrderNumber, OrderSplit = (int)o.JD_OrderNumberSplit } into ps
            //    from o in ps.DefaultIfEmpty()
            //    select new
            //    {
            //        ORDERNO = p.ORDERNO,
            //        ORDER_LINENO = p.ORDER_LINENO,
            //        ORDER_SPLIT = p.ORDER_SPLIT,
            //        BATCH_ID = p.BATCH_ID,
            //        SOURCE_FILE_NAME = p.SOURCE_FILE_NAME,
            //        SUPPLIER_CODE = p.SUPPLIER_CODE,
            //        PRODUCT = p.PRODUCT,
            //        CUSTOMER_ID = p.CUSTOMER_ID,
            //        RECEIVE_DATE = p.RECEIVE_DATE,
            //        PRE_PROCESS_STATUS = p.PRE_PROCESS_STATUS,
            //        STAGING_STATUS = p.STAGING_STATUS,
            //        FILE_GENERATED = p.FILE_GENERATED,
            //        TRANSFER_STATUS = p.TRANSFER_STATUS,
            //        IN_EDIENTERPRISE = o == null ? p.IN_EDIENTERPRISE : String.IsNullOrEmpty(o.JD_OrderNumber) ? p.IN_EDIENTERPRISE : "Y",
            //        EDI_ORD_LINE_PK = p.EDI_ORD_LINE_PK,
            //        EDI_LAST_EDIT_TIME = o == null ? p.EDI_LAST_EDIT_TIME : o.JD_SystemLastEditTimeUtc,
            //        COMMENT = p.COMMENT,
            //        FTP_RECEIVE_DATE = p.FTP_RECEIVE_DATE,
            //        FTP_VS_RECEIVE = !p.FTP_RECEIVE_DATE.HasValue ? "Unable to calculate" : (
            //    (p.RECEIVE_DATE.Value - p.FTP_RECEIVE_DATE.Value).TotalHours >= 72 ? "Over 72" :
            //                    ((p.RECEIVE_DATE.Value - p.FTP_RECEIVE_DATE.Value).TotalHours >= 48 ? "Over 48" :
            //                        (
            //                        (p.RECEIVE_DATE.Value - p.FTP_RECEIVE_DATE.Value).TotalHours >= 24 ? "Over 24" : "< 24"
            //                        )
            //                    )
            //                    )
            //    };

            //var config = new MapperConfiguration(cfg => cfg.CreateMissingTypeMaps = true);
            //var mapper = config.CreateMapper();
            //var final_data_mapped = final_data.Select(a => mapper.Map<POControlTableDTO>(a));

            //if (request.Sorts.Count > 0)
            //{
            //    foreach (var sort in request.Sorts)
            //    {
            //        final_data_mapped = final_data_mapped.OrderBy<POControlTableDTO>(sort.fieldname, sort.desc);
            //    }
            //}
            //else
            //{
            //    final_data_mapped = final_data_mapped.OrderByDescending(orderby => orderby.BATCH_ID);
            //}
            //var final_filtered_data = final_data_mapped.Skip(request.Skip).Take(request.Take);
            //response.data = final_filtered_data;
                
            //}
            //catch (Exception ex) 
            //{ }

            

            //////query method
            //List<SqlParameter> sqlp = new List<SqlParameter>();

            //sqlp.Add(new SqlParameter("@Day", FilterRange));
            //sqlp.Add(new SqlParameter("@userid", userid));
            try
            {


                //this._dwUnitofWork.DbContext.Database.CommandTimeout = 180;
                //new SqlParameter("@Day", FilterRange), new SqlParameter("@userid", userid)
                string sql = Properties.CustomQuerys.GetAllPOHeader;

                sql = sql.Replace(@"tapdbcnd04.tollgroup.local\dev", _DCTdbserver);
                sql = sql.Replace("TAPDBCND01.TOLLGROUP.LOCAL", _CW1dbserver);


                var dataPage = this.UnitOfWork.ExecuteQuery<POControlTableMasterDTO>(sql.Replace("@Day", FilterRange.ToString()).Replace("@userid", userid), new object[] { }).AsQueryable();
                dataPage = dataPage.Where(specs.SatisfiedBy());

                if (request.Sorts.Count > 0)
                {
                    foreach (var sort in request.Sorts)
                    {
                        dataPage = dataPage.OrderBy<POControlTableMasterDTO>(sort.fieldname, sort.desc);
                    }
                }
                else
                {
                    dataPage = dataPage.OrderByDescending(orderby => orderby.CUSTOMER_ID);
                }

                List<POControlTableMasterDTO> dataPageList = dataPage.ToList();


                //var masterList = dataPageList.GroupBy(x => new { x.ORDERNO, x.ORDER_SPLIT, x.CUSTOMER_ID, x.SUPPLIER_CODE })
                //                .Select(g => new POControlTableMasterDTO
                //                {
                //                    CUSTOMER_ID = g.Key.CUSTOMER_ID,
                //                    ORDERNO = g.Key.ORDERNO,
                //                    ORDER_SPLIT = g.Key.ORDER_SPLIT,
                //                    RECEIVE_DATE = g.Select(o => o.RECEIVE_DATE).Min(),
                //                    RECEIVE_DATE_UTC = g.Select(o => o.RECEIVE_DATE_UTC).Min(),
                //                    SUPPLIER_CODE = g.Key.SUPPLIER_CODE,
                //                    ORDER_STATUS = g.Select(o => o.TRANSFER_STATUS).Max(),
                //                    IN_EDIENTERPRISE = g.Select(o => o.IN_EDIENTERPRISE).Max(),
                //                    EDI_LAST_EDIT_TIME = g.Select(o => o.EDI_LAST_EDIT_TIME).Max(),
                //                    Details = g.ToList().Select(o => new POControlTableDetailDTO
                //                    {
                //                        ORDERNO = o.ORDERNO,
                //                        ORDER_SPLIT = o.ORDER_SPLIT,
                //                        CUSTOMER_ID = o.CUSTOMER_ID,
                //                        BATCH_ID = o.BATCH_ID,
                //                        RECEIVE_DATE = o.RECEIVE_DATE,
                //                        ORDER_LINENO = o.ORDER_LINENO,
                //                        PRODUCT = o.PRODUCT,
                //                        PRE_PROCESS_STATUS = o.PRE_PROCESS_STATUS,
                //                        STAGING_STATUS = o.STAGING_STATUS,
                //                        TRANSFER_STATUS = o.TRANSFER_STATUS,
                //                        FTP_RECEIVE_DATE = o.FTP_RECEIVE_DATE,
                //                        SOURCE_FILE_NAME = o.SOURCE_FILE_NAME,
                //                        FILE_GENERATED = o.FILE_GENERATED,
                //                        COMMENT = o.COMMENT,
                //                        REPROCESS = string.IsNullOrEmpty(o.SOURCE_FILE_NAME) || string.IsNullOrEmpty(o.SEND_TO_FOLDER) ? false : true,
                //                        SEND_TO_FOLDER = o.SEND_TO_FOLDER
                //                    }).ToList()
                //                }).ToList();


                //var result = dataPageList.Skip(request.Skip).Take(request.Take);
                var result = dataPageList.Skip(request.Skip).Take(request.Take); //masterList.Skip(request.Skip).Take(request.Take);

                response.data = result.ToList();

                if (request.RequireTotalCount)
                    response.totalCount = dataPageList.Count(); //masterList.Count();
                //response.totalCount = dataPageList.Count();
            }
            catch (Exception ex1)
            {
                throw ex1;
            }


            return response;
        }

        //private DevResponse SingleData(DevRequest request)
        //{
        //    DevResponse response = new DevResponse();
        //    IQueryable<POControlTableDTO> data =
        //    this.GetAll().UseAsDataSource(this._Imapper).For<POControlTableDTO>();
        //    var singledata = data.Where(predicate => predicate.OBJECT_KEY.ToString() == request.).FirstOrDefault();
        //    response.data = singledata;
        //    response.key = request.Key;
        //    return response;

        //}
        private void GetCustomerIDFilder(string userid)
        {

            IQueryable<UserSetting> data = this._dctUnitofwork.CreateSet<UserSetting>();
            data = data.Where(us => us.UserId == userid && us.DataType.ToUpper() == "TGF_PO_CONTROL_TABLE.CUSTOMERID" && us.Type.ToUpper() == "DBFILTER");
            CustomerIDFilter = (from a in data select a.Value).ToList();
            return;
        }

        private DevResponse getChartDataByCustomer()
        {
            DevResponse response = new DevResponse();
            IQueryable<TGF_PO_CONTROL_TABLE> data = this.UnitOfWork.CreateSet<TGF_PO_CONTROL_TABLE>();

            var filterDate = DateTime.Now.AddDays(FilterRange * -1);
            data = data.Where(po => po.RECEIVE_DATE.HasValue && po.RECEIVE_DATE.Value > filterDate);
            if (CustomerIDFilter.Count > 0)
                data = data.Where(po => CustomerIDFilter.Contains(po.CUSTOMER_ID));
            var group_data = data.GroupBy(p => p.CUSTOMER_ID)
                            .Select(g => new
                            {
                                Customer = g.Key,
                                OrderCount = g.Select(p => String.Concat(p.ORDERNO, p.ORDER_SPLIT)).Distinct().Count()
                            });

            response.data = group_data.ToList();
            response.totalCount = group_data.Count();

            return response;
        }

        private DevResponse getChartDataFTPVsProcessing()
        {
            DevResponse response = new DevResponse();
            IQueryable<TGF_PO_CONTROL_TABLE> data = this.UnitOfWork.CreateSet<TGF_PO_CONTROL_TABLE>();

            var filterDate = DateTime.Now.AddDays(FilterRange * -1);
            data = data.Where(po => po.RECEIVE_DATE.HasValue && po.RECEIVE_DATE.Value > filterDate && po.FTP_RECEIVE_DATE.HasValue);
            if (CustomerIDFilter.Count > 0)
                data = data.Where(po => CustomerIDFilter.Contains(po.CUSTOMER_ID));

            var staging_data = data.Select(p => new
                {
                    Status = DbFunctions.DiffHours(p.FTP_RECEIVE_DATE, p.RECEIVE_DATE).Value >= 72 ? "Over 72" :
                                (DbFunctions.DiffHours(p.FTP_RECEIVE_DATE, p.RECEIVE_DATE).Value >= 48 ? "Over 48" :
                                    (
                                    DbFunctions.DiffHours(p.FTP_RECEIVE_DATE, p.RECEIVE_DATE).Value >= 24 ? "Over 24" : "less than 24"
                                    )
                                )
                });

            var group_data = staging_data.GroupBy(p => p.Status)
                            .Select(g => new
                            {
                                Status = g.Key,
                                StatusCount = g.Select(p => p.Status).Count()
                            });

            response.data = group_data.ToList();
            response.totalCount = group_data.Count();

            return response;
        }

        private DevResponse getChartDataOrderStatus(string userid)
        {
            DevResponse response = new DevResponse();

            string sql = Properties.CustomQuerys.GetPOChartByOrderStatus;

            sql = sql.Replace(@"tapdbcnd04.tollgroup.local\dev", _DCTdbserver);
            sql = sql.Replace("TAPDBCND01.TOLLGROUP.LOCAL", _CW1dbserver);


            var data = this.UnitOfWork.ExecuteQuery<OrderStatusChart>(sql.Replace("@Day", FilterRange.ToString()).Replace("@userid", userid), new object[] {}).ToList();
            

            response.data = data;

            return response;
        }

        private DevResponse getSelectBoxClientData()
        {
            DevResponse response = new DevResponse();

            var data = this.UnitOfWork.ExecuteQuery<ClientNameObject>(Properties.CustomQuerys.GetAllPOClientReprocess, new object[] { }).ToList();

            response.data = data;

            return response;
        }

        #endregion

        #region Utility functions
        private void ConvertFilterRange(string filterText)
        {
            switch (filterText)
            {
                case "24 Hours":
                    FilterRange = 1;
                    break;

                case "7 Days":
                    FilterRange = 7;
                    break;

                case "30 Days":
                    FilterRange = 30;
                    break;

                default:
                    FilterRange = 7;
                    break;
            }
        }

        private bool checkPathPattern(string path)
        {
            return Regex.IsMatch(path, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$");
        }

        private string FormatXML(string original_xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(original_xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                return original_xml;
            }
        }

        #endregion



        public DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, LogModel logmodel)
        {
            DevRequest convertRequest = new DevRequest(request);
            switch (convertRequest.CuttentAction)
            {
                case "search":
                    return this.PageData(convertRequest, logmodel.UserID);
                case "searchsingle":
                    throw new NotImplementedException();
                    //return this.SingleData(convertRequest);
                //case "delete":
                //return this.Remove(convertRequest);
                //case "create":
                //case "update":
                //return this.CreateOrEdit(convertRequest);
                default:
                    throw new NotImplementedException();
            }
        }

        public DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, string filter, LogModel logmodel )
        {
            ConvertFilterRange(filter);

            DevRequest convertRequest = new DevRequest(request);
            switch (convertRequest.CuttentAction)
            {
                case "search":
                    return this.PageData(convertRequest, logmodel.UserID);
                case "searchsingle":
                    throw new NotImplementedException();
                //return this.SingleData(convertRequest);
                //case "delete":
                //return this.Remove(convertRequest);
                //case "create":
                //case "update":
                //return this.CreateOrEdit(convertRequest);
                default:
                    throw new NotImplementedException();
            }
        }




        public DevResponse DevChartDataByCustomer(string filter, LogModel logmodel )
        {
            ConvertFilterRange(filter);
            GetCustomerIDFilder(logmodel.UserID);
            return this.getChartDataByCustomer();
        }


        public DevResponse DevChartDataFTPVsProcessing(string filter, LogModel logmodel )
        {
            ConvertFilterRange(filter);
            GetCustomerIDFilder(logmodel.UserID);
            return this.getChartDataFTPVsProcessing();
        }


        public DevResponse DevChartDataOrderStatus(string filter, LogModel logmodel )
        {
            ConvertFilterRange(filter);
//            GetCustomerIDFilder(logmodel.UserID);
            return this.getChartDataOrderStatus(logmodel.UserID);
        }

        public string readUNC(string request, string serverpath, string unc)
        {
            string path = string.Concat(serverpath, Path.GetFileName(request));


            string returnText = "";
            if (File.Exists(path))
            {
                returnText = File.ReadAllText(path);
                if (Path.GetExtension(path) == ".xml")
                    returnText = this.FormatXML(returnText);
            }
            else
            {
                string archive_folder_check = Path.GetDirectoryName(request);
                var archive_log_file = this._unitofWork.ExecuteQuery<string>(Properties.CustomQuerys.GetArchiveFilePath,
                                    new SqlParameter("@folder", archive_folder_check),
                                    new SqlParameter("@file", Path.GetFileName(request))
                                    );
                path = Regex.Replace(archive_log_file.SingleOrDefault() ?? "", @"c:\\di", unc, RegexOptions.IgnoreCase);
                
                if (File.Exists(path))
                {
                    returnText = File.ReadAllText(path);
                    if (Path.GetExtension(path) == ".xml")
                        returnText = this.FormatXML(returnText);
                }
                else
                {
                    returnText = "File not found!";
                }
            }

            return returnText;
        }


        public bool ReprocessFile(string file, string destinationPath, string unc, LogModel logmodel)
        {
            bool result = false;

            try
            {
                if (File.Exists(file))
                {
                    var filename = "GICT_" + Path.GetFileName(file);
                    File.Copy(file, string.Concat(destinationPath, @"\", filename), true);
                    result = true;

                    logmodel.PK = System.Guid.NewGuid();
                    logmodel.Details = file;
                    logmodel.Action = "reprocess";
                    logmodel.Status = result;
                    base.Log(logmodel);
                }
                else
                {
                    string archive_folder_check = Path.GetDirectoryName(file).Replace(unc, @"c:\di");
                    var archive_log_file = this._unitofWork.ExecuteQuery<string>(Properties.CustomQuerys.GetArchiveFilePath,
                                        new SqlParameter("@folder", archive_folder_check),
                                        new SqlParameter("@file", Path.GetFileName(file))
                                        );
                    var archive_full_path = Regex.Replace(archive_log_file.SingleOrDefault() ?? "", @"c:\\di", unc, RegexOptions.IgnoreCase);
                    if (File.Exists(archive_full_path))
                    {
                        var filename = "GICT_" + Path.GetFileName(file);
                        File.Copy(archive_full_path, string.Concat(destinationPath, @"\", filename), true);
                        result = true;

                        logmodel.PK = System.Guid.NewGuid();
                        logmodel.Details = file;
                        logmodel.Action = "reprocess";
                        logmodel.Status = result;
                        base.Log(logmodel);
                    }
                }
            }
            catch (Exception e)
            {
                logmodel.PK = System.Guid.NewGuid();
                logmodel.Details = file;
                logmodel.Action = "reprocess";
                logmodel.Status = result;
                logmodel.Remark = e.ToString();
                base.Log(logmodel);
                result = false; 
            }

            return result;
        }



        public DevResponse DevSelectBoxClientData(LogModel logmodel = null)
        {
            return this.getSelectBoxClientData();
        }


        public DevResponse DevPageDetails(string range, string CustomerID, string OrderNo, int OrderSplit, string SupplierCode, LogModel logmodel = null)
        {
            ConvertFilterRange(range);

            DevResponse response = new DevResponse();
            var dataPage = this.UnitOfWork.ExecuteQuery<POControlTableDetailDTO>(Properties.CustomQuerys.GetPOGridDetails, new SqlParameter("@Day", FilterRange), new SqlParameter("@CUSTOMERID", CustomerID), new SqlParameter("@ORDERNO", OrderNo), new SqlParameter("@ORDERSPLIT", OrderSplit), new SqlParameter("@SUPPLIERCODE", SupplierCode));
            var result = dataPage.ToList();
            response.data = result;
            response.totalCount = result.Count();
            return response;
        }
    }
}
