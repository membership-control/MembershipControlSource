using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.EF;
using Core.Data.Repository;
using Core.Data.UnitOfWork;
using Core.Infrastruture.Specification;
using Core.Infrastruture.Specification.Implementation;
using Core.Services.DTO.Administration;
using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.Utility;
using Core.Infrastructure.Dev;
using Core.Data.Model;
using System.Collections.Specialized;
using OfficeOpenXml;
using System.Reflection;
using System.IO;

namespace Core.Services.Service
{
    public class MemberService : Repository<MEM_Membership, DI_WK_TEMPEntities>, IMember
    {
        private IMapper _Imapper;
        private const string _sheetname = "Members";

        public MemberService(WKTempUnitOfWork work, IMapper iImapper)
            : base(work)
        {
            this._Imapper = iImapper;
        }

        #region CRUD methods

        private DevResponse PageData(DevRequest request)
        {
            DevResponse response = new DevResponse();
            IQueryable<MemberDTO> data =
this.GetAll().UseAsDataSource(this._Imapper).For<MemberDTO>();

            Specification<MemberDTO> specs = base.DevGridFilters<MemberDTO>(request.Filters);
            var filteredData = data.Where(specs.SatisfiedBy());

            if (request.Sorts.Count > 0)
            {
                foreach (var sort in request.Sorts)
                {
                    filteredData = filteredData.OrderBy<MemberDTO>(sort.fieldname, sort.desc);
                }
            }
            else
            {
                filteredData = filteredData.OrderBy(orderby => orderby.MBR_Name);
            }

            //var dataPage = filteredData.Skip(request.Skip).Take(request.Take);
            IQueryable<MemberDTO> dataPage;
            if (!request.RequireTotalCount && request.Take <= 0)
                dataPage = filteredData;
            else
                dataPage = filteredData.Skip(request.Skip).Take(request.Take);

            response.data = dataPage.ToList();
            if (request.RequireTotalCount)
                response.totalCount = filteredData.Count();
            return response;
        }

        private DevResponse SingleData(DevRequest request)
        {
            DevResponse response = new DevResponse();
            //Guid guid = Guid.Empty;
            //Guid.TryParse(request.Key, out guid);
            IQueryable<MemberDTO> data =
this.GetAll().UseAsDataSource(this._Imapper).For<MemberDTO>();
            //var singledata = data.Where(predicate => predicate.MBR_PK == guid).FirstOrDefault();
            var singledata = data.Where(predicate => predicate.MBR_Phone1 == request.Key || predicate.MBR_Phone2 == request.Key).FirstOrDefault();
            response.data = singledata;
            response.key = singledata?.MBR_PK.ToString();
            return response;
        }

        //        private DevResponse Remove(DevRequest request, LogModel logmodel)
        //        {
        //            DevResponse response = new DevResponse();
        //            string key = request.Key;
        //            Guid guid = Guid.Empty;
        //            bool b = Guid.TryParse(key, out guid);
        //            if (b)
        //            {
        //                var result_Delete = base.DeleteByID(guid);
        //                if (result_Delete.Code == 0)
        //                {
        //                    response.haveError = false;
        //                    response.key = request.Key;
        //                    response.error = result_Delete.ErrMsg;
        //                }
        //                else
        //                {
        //                    response.haveError = true;
        //                    response.error = "CODE:" + result_Delete.Code + "  MSG:" + result_Delete.ErrMsg;
        //                }

        //                logmodel.PK = System.Guid.NewGuid();
        //                logmodel.Details = this.UnitOfWork.Sql;
        //                logmodel.Action = request.CuttentAction;
        //                logmodel.Status = !response.haveError;
        //                logmodel.Remark = response.error;
        //                base.Log(logmodel);
        //            }
        //            return response;
        //        }

        private DevResponse CreateOrEdit(DevRequest request, LogModel logmodel)
        {
            DevResponse response = new DevResponse();
            MemberDTO orgin = base.ConvertObject<MemberDTO>(request.Values);

            Guid guid = Guid.Empty;
            Guid.TryParse(request.Key, out guid);

            orgin.MBR_PK = guid;
            MEM_Membership convert_hanlder = this._Imapper.Map<MEM_Membership>(orgin);
            if (convert_hanlder.MBR_PK != null && convert_hanlder.MBR_PK != Guid.Empty)
            {
                var realdata = base.GetByID(convert_hanlder.MBR_PK);

                convert_hanlder.MBR_InsertDate = realdata.MBR_InsertDate;
                base.CopyData<MEM_Membership>(realdata, convert_hanlder);
                //               realdata.insert_date = DateTime.Now;
                realdata.MBR_UpdateDate = DateTime.Now;
                realdata.MBR_UpdateUser = logmodel.Insert_User;

                var result_update = base.Update(realdata);
                if (result_update.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.MBR_PK.ToString();
                    response.data = orgin; //this._Imapper.Map<MemberDTO>(realdata);
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_update.Code + "  MSG:" + result_update.ErrMsg;
                }
            }
            else
            {
                Guid id = Guid.NewGuid();
                convert_hanlder.MBR_PK = id;
                convert_hanlder.MBR_InsertDate = DateTime.Now;
                convert_hanlder.MBR_InsertUser = logmodel.Insert_User;

                var result_add = base.Add(convert_hanlder);
                if (result_add.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.MBR_PK.ToString();
                    response.data = this._Imapper.Map<MemberDTO>(convert_hanlder); //request.Values;
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_add.Code + "  MSG:" + result_add.ErrMsg;
                }
            }

            logmodel.Action = request.CuttentAction;
            logmodel.Remark = response.error;
            base.Log(logmodel);

            return response;
        }


        #endregion

        public DevResponse CheckID(string ID)
        {
            DevResponse response = new DevResponse();

            try
            {
                if (String.IsNullOrEmpty(ID))
                {
                    response.data = true;
                    response.haveError = false;
                }
                else
                {
                    IQueryable<MemberDTO> data =
        this.GetAll().UseAsDataSource(this._Imapper).For<MemberDTO>();
                    var query_data = !data.Any(predicate => predicate.MBR_MemberID.ToUpper() == ID.ToUpper());
                    response.data = query_data;
                    response.haveError = false;
                }
            }
            catch (Exception ex)
            {
                response.haveError = true;
                response.error = ex.InnerException.Message;
            }
            finally
            { }

            return response;
        }

        public DevResponse ExportMembers()
        {
            DevResponse response = new DevResponse();

            IQueryable<MemberDTO> data = this.GetAll().UseAsDataSource(this._Imapper).For<MemberDTO>().OrderBy(orderby => orderby.MBR_Name);
            int data_count = data.Count();
            List<MemberDTO> data_list = new List<MemberDTO>();
            for (int i = 0; i < data_count; i++)
            {
                MemberDTO dto = data.Skip(i).FirstOrDefault();
                data_list.Add(dto);
            }

            using (ExcelPackage ep = new ExcelPackage())
            {
                ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(_sheetname);
                UInt16 col = 1;

                PropertyInfo[] dto_properties = typeof(MemberDTO).GetProperties();
                foreach (PropertyInfo property in dto_properties)
                {
                    sheet.Cells[1, col].Value = property.Name;
                    sheet.Cells[1, col].Style.Font.Bold = true;
                    if (property.PropertyType.FullName.Contains("System.DateTime"))
                        sheet.Column(col).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                    col++;
                }
                sheet.Cells[2, 1].LoadFromCollection(data_list);

                //remove PK col
                sheet.DeleteColumn(1);

                response.data = ep.GetAsByteArray();
                response.key = "MemberList.xlsx";
            }


            return response;
        }

        public DevResponse ImportMembers(byte[] filebinary, string filename = null, LogModel logmodel = null)
        {
            DevResponse response = new DevResponse();
            using (MemoryStream memoryStream = new MemoryStream(filebinary))
            {
                using (ExcelPackage ep = new ExcelPackage(memoryStream))
                {
                    if (ep.Workbook.Worksheets.Count == 0) //Check any worksheet exists
                    {
                        response.haveError = true;
                        //response.error = "No worksheet found";
                        throw new Exception("No worksheet found");
                    }
                    else if (ep.Workbook.Worksheets.Select(s => s.Name == _sheetname).Count() == 0)  //restrict to only read from worksheet name
                    {
                        response.haveError = true;
                        //response.error = "Worksheet named " + _sheetname + " not found";
                        throw new Exception("Worksheet named " + _sheetname + " not found");
                    }
                    else
                    {
                        using (ExcelWorksheet sheet = ep.Workbook.Worksheets[_sheetname])
                        {
                            UInt16 col = 1;
                            IEnumerable<PropertyInfo> dto_properties = typeof(MemberDTO).GetProperties().Where(p => p.Name != "MBR_PK");
                            foreach (PropertyInfo property in dto_properties)
                            {
                                if ((string)sheet.Cells[1, col].Value != property.Name)  //check header matching columns name first
                                {
                                    response.haveError = true;
                                    //response.error = "Template header column name mismatched";
                                    throw new Exception("Template header column name mismatched");
                                }
                                
                                col++;
                            }

                            if (!response.haveError)
                            {
                                try
                                {
                                    List<MemberDTO> insert_member_list = new List<MemberDTO>();
                                    for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                                    {
                                        col = 1;
                                        MemberDTO insert_member = new MemberDTO();
                                        insert_member.MBR_PK = System.Guid.NewGuid();
                                        insert_member.MBR_InsertDate = DateTime.Now;
                                        insert_member.MBR_InsertUser = logmodel?.Insert_User;

                                        foreach (PropertyInfo property in dto_properties)
                                        {
                                            if (!property.Name.Contains("MBR_ID") && !property.Name.Contains("MBR_InsertDate") &&
                                                !property.Name.Contains("MBR_InsertUser") && !property.Name.Contains("MBR_UpdateDate") && !property.Name.Contains("MBR_UpdateUser"))
                                            {
                                                string cell_value = Convert.ToString(sheet.Cells[row, col].Value);
                                                object converted_value = null;

                                                //Check name and phone1 cannot empty
                                                if ((property.Name.Contains("MBR_Name") || property.Name.Contains("MBR_Phone1")) &&
                                                    String.IsNullOrEmpty(cell_value))
                                                    throw new Exception("Name or Phone1 empty data found");

                                                string str = property.PropertyType.FullName;
                                                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(System\.Nullable`1\[\[){0,1}(?<Result>S[^\s,]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                                if (match.Success)
                                                {
                                                    str = match.Groups["Result"].Value;
                                                }

                                                switch (str)
                                                {
                                                    case "System.DateTime":
                                                        cell_value = System.Text.RegularExpressions.Regex.Replace(cell_value, @"\([^\)]*\)", "");
                                                        DateTime out_dt;
                                                        if (DateTime.TryParse(cell_value, out out_dt))
                                                        {
                                                            converted_value = DateTime.Parse(out_dt.ToString("yyyy-MM-dd HH:mm:ss"));
                                                        }
                                                        break;

                                                    case "System.Boolean":
                                                        if (String.IsNullOrEmpty(cell_value))
                                                            converted_value = false;
                                                        else
                                                        {
                                                            if (cell_value.ToUpper() == "TRUE")
                                                                converted_value = true;
                                                            else
                                                                converted_value = false;
                                                        }
                                                        break;

                                                    case "System.Int32":
                                                        int out_int = 0;
                                                        int.TryParse(cell_value, out out_int);
                                                        converted_value = out_int;
                                                        break;

                                                    case "System.String":
                                                        converted_value = cell_value?.Trim();
                                                        break;

                                                    default:
                                                        converted_value = null;
                                                        break;
                                                }

                                                property.SetValue(insert_member, converted_value, null);
                                            }

                                            col++;
                                        }

                                        insert_member_list.Add(insert_member);
                                    }

                                    if (insert_member_list.Count() > 0)
                                    {
                                        List<MEM_Membership> insert_entities = this._Imapper.Map<List<MemberDTO>, List<MEM_Membership>>(insert_member_list);
                                        var result_add = base.AddRange(insert_entities);
                                        if (result_add.Code == 0)
                                        {
                                            response.haveError = false;
                                            //response.data = insert_entities.Count().ToString() + " has been inserted successfully.";

                                            logmodel.Action = "import";
                                            logmodel.Remark =  insert_entities.Count().ToString() + " record has been inserted successfully (" + filename + ")";
                                            base.Log(logmodel);
                                        }
                                        else
                                        {
                                            response.haveError = true;
                                            //response.error = "CODE:" + result_add.Code + "  MSG:" + result_add.ErrMsg;
                                            throw new Exception("CODE:" + result_add.Code + "  MSG:" + result_add.ErrMsg);
                                        }
                                    }
                                    else
                                    {
                                        response.haveError = true;
                                        //response.error = "Worksheet contains no data";
                                        throw new Exception("Worksheet contains no data");
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    response.haveError = true;
                                    //response.error = "Error! Insert not commited. Please check system log for more info ";

                                    logmodel.Action = "error";
                                    logmodel.Remark = ex1.Message.ToString();
                                    base.Log(logmodel);                                    
                                }
                            }

                        }
                    }
                }
                
            }

            return response;
        }

        public DevResponse DevPageAll(NameValueCollection request, LogModel logmodel = null)
        {
            DevRequest convertRequest = new DevRequest(request);
            switch (convertRequest.CuttentAction)
            {
                case "search":
                    return this.PageData(convertRequest);
                case "searchsingle":
                    return this.SingleData(convertRequest);
                case "delete":
                    throw new NotImplementedException();//return this.Remove(convertRequest, logmodel);
                case "create":
                case "update":
                    return this.CreateOrEdit(convertRequest, logmodel);
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
