using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.Model;
using Core.Data.UnitOfWork;
using Core.Infrastructure.DataTables;
using Core.Infrastructure.Model;
using Core.Infrastructure.Utility;
using Core.Infrastructure.Dev;
using Core.Services.DTO;
using Core.Services.DTO.Visibility;
using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

namespace Core.Services.Service
{
    public class Icon3IntegrationService : IIcon3Integration
    {
        private IntegrationUnitOfWork _unitofWork;
        private IMapper _Imapper;
        private string _TGFDWdbserver;
        private string _CW1dbserver;

        public Icon3IntegrationService(IntegrationUnitOfWork work, WKTempUnitOfWork wk_work, IMapper iImapper)
        {
            this._unitofWork = work;
            this._Imapper = iImapper;
            this._TGFDWdbserver = System.Configuration.ConfigurationManager.AppSettings["TGFDWserver"];
            this._CW1dbserver = System.Configuration.ConfigurationManager.AppSettings["CW1server"];
        }

        #region Properties
        private int FilterRange { get; set; }
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
                    FilterRange = 1;
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

        private string DevGridFiltersToSQL(Dictionary<string, object> fileters)
        {
            string clause = "";
            string connect_op = "and";
            string str_fieldname = string.Empty;
            string str_value = string.Empty;
            string str_op = string.Empty;
            foreach (var item in fileters)
            {
                string temp = "";
                if (item.Value is Dictionary<string, object>)
                {
                    string data = DevGridFiltersToSQL(item.Value as Dictionary<string, object>);
                    if (clause == "")
                        clause = data;
                    else
                    {
                        if (connect_op == "and")
                        {
                            clause = clause + " and " + data;
                        }
                        else
                        {
                            clause = clause + " or " + data;
                        }
                    }
                }
                else
                {
                    string filter_value = item.Value.ToString();
                    string[] strs = filter_value.Split(',');
                    if (strs[0] == "and" || strs[0] == "or")
                    {
                        connect_op = strs[0];
                        continue;
                    }

                    if (strs.Length == 1)
                    {
                        string op = Core.Infrastructure.Dev.DevConvertOP.ConvertClientOPSQL(str_op);
                        string[] datepart = strs[0].Split(' ');

                        string date = datepart[1] + " " + datepart[2] + " " + datepart[3] + " " + datepart[4];
                        temp = str_fieldname + " " + op + " '" + date + "'";

                        //                        clause =  clause + str_fieldname, strs[0], op);
                        if (clause == "")
                            clause = temp;
                        else
                        {
                            if (connect_op == "and")
                            {
                                clause = clause + " and " + temp;
                            }
                            else
                            {
                                clause = clause + " or " + temp;
                            }
                        }
                        continue;
                    }
                    else if (strs.Length == 2)
                    {
                        str_fieldname = strs[0];
                        str_op = strs[1];
                        continue;
                    }
                    else if (strs.Length == 3)
                    {
                        str_fieldname = strs[0];
                        str_op = strs[1];
                        str_value = strs[2];
                    }
                    else
                    {
                        str_fieldname = strs[0];
                        str_op = strs[1];
                        for (int i = 2; i < strs.Length; i++)
                        {
                            str_value += strs[i] + ",";
                        }
                        str_value = str_value.TrimEnd(',');
                    }
                    string op1 = Core.Infrastructure.Dev.DevConvertOP.ConvertClientOPSQL(str_op);
                    if (str_op == "contains")
                        str_value = "%" + str_value + "%";
                    if (str_op == "endswith")
                        str_value = "%" + str_value;
                    if (str_op == "startswith")
                        str_value = str_value + "%";
                    if (str_op == "notcontains")
                        str_value = "%" + str_value + "%";


                    temp = str_fieldname + " " + op1 + " '" + str_value + "'";

                    if (clause == "")
                        clause = temp;
                    else
                    {
                        if (connect_op == "and" && clause != "")
                        {
                            clause = clause + " and " + temp;
                        }
                        else
                        {
                            clause = clause + " or " + temp;
                        }
                    }
                }
            }
            return clause;
        }

        #endregion

        #region CRUD private methods

        private DevResponse getChartDataset1()
        {
            DevResponse response = new DevResponse();

            string sql = Properties.CustomQuerys.GetIconIntegrationChartDataset;

            sql = sql.Replace("@days", FilterRange.ToString());
            sql = sql.Replace("TAPDBHKD05.TOLLGROUP.LOCAL", _TGFDWdbserver);
            sql = sql.Replace("TAPDBCND01.TOLLGROUP.LOCAL", _CW1dbserver);

            try 
            {
                this._unitofWork.DbContext.Database.CommandTimeout = 300;
                var data = this._unitofWork.ExecuteQuery<IconIntegrationChart1DTO>(sql, new object[] { }).ToList();
                response.data = data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        private DevResponse PageData(DevRequest request, string userid)
        {
            DevResponse response = new DevResponse();
            try
            {
                string whereclause = DevGridFiltersToSQL(request.Filters);
                string sqldata = Properties.CustomQuerys.GetAllIconIntegration;
                string sqlrowcount = Properties.CustomQuerys.GetAllIconIntegrationCount;
                string sortfield = "Listener_insert_date", sortdesc = "desc";
                if (whereclause == "")
                {
                    whereclause = "1 = 1";
                }
                else
                {
                    sqldata = Properties.CustomQuerys.GetAllIconIntegrationWithFilter;
                }

                whereclause = whereclause.Replace("'getdate() - 7'", "getdate() - 7").Replace("'getdate() - 1'", "getdate() - 1").Replace("'getdate() - 30'", "getdate() - 30");

                if (request.Sorts.Count > 0)
                {
                    sortfield = request.Sorts[0].fieldname;
                    if (request.Sorts[0].desc)
                        sortdesc = "asc";
                }

                sqldata = sqldata.Replace("@days", FilterRange.ToString()).Replace("@sortfield", sortfield).Replace("@sortdesc", sortdesc).Replace("@start", (request.Skip + 1).ToString()).Replace("@end", (request.Take + 1 + request.Skip).ToString()).Replace("@whereclause", whereclause);
                sqldata = sqldata.Replace("TAPDBHKD05.TOLLGROUP.LOCAL", _TGFDWdbserver);
                sqldata = sqldata.Replace("TAPDBCND01.TOLLGROUP.LOCAL", _CW1dbserver);
                this._unitofWork.DbContext.Database.CommandTimeout = 300;
                var dataPage = this._unitofWork.ExecuteQuery<IconIntegrationDTO>(sqldata);
                var datalist = dataPage.GroupBy(x => new { x.Log_PK, x.e_booking_no, x.fullfilename, x.summary_status })
                    .Select(g => new IconIntegrationMasterDTO
                    {
                        fullfilename = g.Key.fullfilename,
                        Log_PK = g.Key.Log_PK,
                        e_booking_no = g.Key.e_booking_no,
                        summary_status = g.Key.summary_status,
                        Icon_insert_date = g.Select(o => o.Icon_insert_date).Max(),
                        REPROCESS = (bool?)true,
                        Details = g.ToList().Select(o => new IconIntegrationDetailDTO
                        {
                            JobName = o.JobName,
                            process_PK = o.process_PK,
                            Listener_insert_date = o.Listener_insert_date,
                            Type = o.Type,
                            Keyreference = o.Keyreference,
                            DI_Process_insert_date = o.DI_Process_insert_date,
                            BATCH_ID = o.BATCH_ID,
                            LISTENER_TRIGGERED = o.LISTENER_TRIGGERED,
                            DI_CALLED = o.DI_CALLED,
                            Status = o.Status,
                            Last_Remarks = o.Last_Remarks,
                            DI_Trace = o.DI_Trace,
                            DI_Error = o.DI_Error,
                            FILE_EXPORTED = o.FILE_EXPORTED,
                            Output_file_name = o.Output_file_name,
                            CW1_Received = o.CW1_Received
                        }).ToList()
                    }).ToList();

                response.data = datalist;

                if (request.RequireTotalCount)
                {
                    sqlrowcount = sqlrowcount.Replace("@days", FilterRange.ToString()).Replace("@whereclause", whereclause);
                    sqlrowcount = sqlrowcount.Replace("TAPDBHKD05.TOLLGROUP.LOCAL", _TGFDWdbserver);
                    response.totalCount = this._unitofWork.ExecuteQuery<int>(sqlrowcount).ToList()[0];
                }
            }
            catch (Exception ex1)
            {
                throw ex1;
            }

            return response;
        }

        #endregion

        #region Inherited functions

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
                returnText = "File not found!";

            return returnText;
        }

        public Infrastructure.Dev.DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, string filter, LogModel logmodel)
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

        public string ReprocessFile(string file, string unc, LogModel logmodel = null)
        {
            string result = "";
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                //proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "echo GICTreprocess > " + unc + file;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }

                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (String.IsNullOrEmpty(errormsg))
                {
                    result = "Reprocess successfully";

                    logmodel.PK = System.Guid.NewGuid();
                    logmodel.Details = file;
                    logmodel.Action = "reprocess";
                    logmodel.Status = (bool?)true;
                    this._unitofWork.Log(logmodel);
                }
                else
                {
                    result = errormsg;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return result;
        }


        public DevResponse DevChartDataset(string filter, LogModel logmodel)
        {
            ConvertFilterRange(filter);
            return this.getChartDataset1();
        }

        #endregion


    }
}
