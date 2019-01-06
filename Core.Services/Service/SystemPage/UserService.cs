using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.Model;
using Core.Data.UnitOfWork;
using Core.Data.EF;
using Core.Data.Repository;
using Core.Identity.Models;
using Core.Infrastructure.Model;
using Core.Infrastructure.Dev;
using Core.Services.DTO;
using Core.Services.DTO.SystemPage;
using Core.Services.Interface;
using Core.Services.Converters;
using Core.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Script.Serialization;

namespace Core.Services.Service
{
    public class UserService : Repository<AspNetUser, DCT_MangementEntities>, IUser
    {
         private DCTUnitOfWork _unitofWork;
        private IMapper _Imapper;

        public class usersecurity
        {
            public string Action { get; set; }
            public string Key { get; set; }
            public usersecuritydetail Data { get; set; }
        }
        public  class usersecuritydetail
        {
            public string Type { get; set; }
            public string DataType { get; set; }
            public string Value { get; set; }
        }
        public class userpermission
        {
            public string Action { get; set; }
            public string Key { get; set; }
            public userpermissiondetail Data { get; set; }
        }
        public class userpermissiondetail
        {
            public string Level { get; set; }
           
        }
            
        public UserService(DCTUnitOfWork work, IMapper iImapper) : base(work)
        {
            this._unitofWork = work;
            this._Imapper = iImapper;
        }
        public DevResponse RoleLookup(LogModel logmodel = null)
        {
            DevResponse result = new DevResponse();
            var data = (from role in _unitofWork.DbContext.AspNetRoles
                        select new
                        {
                            roleid = role.Id,
                            rolename = role.Name
                        }).ToList();
            result.data = data;
            return result;
        }
        public DevResponse SearchUser(DevRequest request)
        {
            DevResponse result = new DevResponse();
            List<UserHeaderDTO> a = new List<UserHeaderDTO>();
            string query = Properties.CustomQuerys.GetUserHeader;
            string whereclause = DevGridFiltersToSQL(request.Filters);
            if (whereclause != "")
                whereclause = whereclause + " and ";
            query =  query.Replace("1=1", whereclause + "a.rownum > "+request.Skip.ToString()+" and a.rownum <=  "+ (request.Take + request.Skip ).ToString());
            var dataPage = this._unitofWork.ExecuteQuery<UserHeaderDTO>(query, new object[] { });
            if (dataPage != null)
            {
                result.data = dataPage.ToList();
                result.totalCount = dataPage.ToList()[0].totalcount;
            }
            else
            {
                result.data = null;
                result.totalCount = 0;
            }
            result.key = "Id";
            return result;
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
        public DevResponse DevDBFilter(string id, LogModel logmodel = null)
        {
            DevResponse response = new DevResponse();
            try
            {
                var usersetting = this.UnitOfWork.CreateSet<UserSetting>().AsQueryable();
                var data = (from us in usersetting
                           where us.UserId == id
                           select new {
                           SettingID = us.SettingID,
                           Type = us.Type,
                           UserId = us.UserId,
                           Value = us.Value,
                           DataType = us.DataType,
                           }).ToList();
                response.data = data;
                response.totalCount = data.Count();
            }
            catch (Exception ex)
            {
            }
            return response;
        }
        public bool DeleteUser(string id, LogModel logmodel = null)
        {
            DevResponse response = new DevResponse();
            try
            {
                using (DCT_MangementEntities ef = new DCT_MangementEntities())
                {
                   List<UserSetting> uss= (from us in ef.UserSettings
                               where us.UserId == id
                                select us).ToList();
                   foreach (UserSetting us in uss)
                       ef.UserSettings.Remove(us);
                   List<AspNetUserPermission> anups = (from anup in ef.AspNetUserPermissions where anup.UserId == id select anup).ToList();
                   foreach (AspNetUserPermission anup in anups)
                       ef.AspNetUserPermissions.Remove(anup);
                   AspNetUser u = (from anu in ef.AspNetUsers where anu.Id == id select anu).FirstOrDefault();
                    List<AspNetRole> rs = u.AspNetRoles.ToList();
                    foreach (AspNetRole r in rs)
                        u.AspNetRoles.Remove(r);
                    ef.AspNetUsers.Remove(u);
                    ef.SaveChanges();
                }  
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public DevResponse DevUserPermissions(string id, LogModel logmodel = null)
        {
            DevResponse response = new DevResponse();
            try
            {
                var permissions = this.UnitOfWork.CreateSet<AspNetPermission>().AsQueryable();
                var userpermissions = this.UnitOfWork.CreateSet<AspNetUserPermission>().Where(up => up.UserId == id);
                var users = this.UnitOfWork.CreateSet<AspNetUser>().AsQueryable();

                var data = from p in permissions
                           join up in userpermissions on p.Id equals up.PermissionId into pup
                           from uup in pup.DefaultIfEmpty()
                           join u in users on uup.UserId equals u.Id into puup
                           from allgroup in puup.DefaultIfEmpty()
                           select new
                           {
                               UserName = allgroup.UserName,
                               PermissionId = p.Id,
                               Area = p.Area,
                               Description = p.Description,
                               Level = (PermissionLevel)(uup.Level == null ? 0 : uup.Level)
                           };
                var final_data = data.ToList();

                response.data = final_data;
                response.totalCount = final_data.Count();
            }
            catch (Exception ex)
            {
            }

            return response;
        }
        public DevResponse UpdateUserDetail(System.Collections.Specialized.NameValueCollection requestForm, LogModel logmodel = null)
        {
            DevResponse response = new DevResponse();
            List<userpermission> UserPermissions = (List < userpermission > )new JavaScriptSerializer().Deserialize(requestForm.Get("jsonObjp"), typeof(List<userpermission>));
            List<usersecurity> UserSecurityList = (List<usersecurity>)new JavaScriptSerializer().Deserialize(requestForm.Get("jsonObjus"), typeof(List<usersecurity>));
            string userid = requestForm.Get("id"), roleid = requestForm.Get("roleid"), email = requestForm.Get("email"), username = requestForm.Get("name");
            bool usersecurity = (requestForm.Get("EnableUserSecrity") == "true" ? true : false);
            System.Guid id ;
            if (System.Guid.TryParse(userid, out id))
            {
                using (DCT_MangementEntities ef = new DCT_MangementEntities())
                {
                    try
                    {
                        AspNetUser u = (from anu in ef.AspNetUsers where anu.Id == userid select anu).FirstOrDefault();
                        u.Email = email;
                        u.UserName = username;

                        List<AspNetRole> rs = u.AspNetRoles.ToList();
                        foreach (AspNetRole r in rs)
                            u.AspNetRoles.Remove(r);
                        AspNetRole role = (from anr in ef.AspNetRoles where anr.Id == roleid select anr).FirstOrDefault();
                        u.AspNetRoles.Add(role);
                        List<AspNetUserPermission> ups = (from anup in ef.AspNetUserPermissions where anup.UserId == userid select anup).ToList();
                        if (usersecurity)
                        {
                            bool newinsert = false;
                            if (ups.Count == 0)
                            {
                                newinsert = true;
                                ups = new List<AspNetUserPermission>();
                                List<AspNetPermission> anps = (from anp in ef.AspNetPermissions select anp).ToList();
                                foreach (AspNetPermission anp in anps)
                                {
                                    AspNetUserPermission up = new AspNetUserPermission();
                                    up.Level = 0;
                                    up.InsertDate = DateTime.Now;
                                    up.PermissionId = anp.Id;
                                    up.UserId = userid;
                                    ups.Add(up);
                                }
                            }
                            foreach (userpermission userp in UserPermissions)
                            {
                                AspNetUserPermission up = ups.FirstOrDefault(a => a.PermissionId == userp.Key);
                                up.Level = int.Parse(userp.Data.Level);
                            }
                            if (newinsert)
                            {
                                foreach (AspNetUserPermission k in ups)
                                    ef.AspNetUserPermissions.Add(k);
                            }
                        }
                        else
                        {
                            foreach (AspNetUserPermission up in ups)
                                ef.AspNetUserPermissions.Remove(up);
                        }
                        foreach (usersecurity us in UserSecurityList)
                        {
                            UserSetting usersec = (from userset in ef.UserSettings where userset.SettingID.ToString() == us.Key select userset).FirstOrDefault();
                            if (us.Action == "remove")
                                ef.UserSettings.Remove(usersec);
                            if (us.Action == "update")
                            {
                                usersec.DataType = us.Data.DataType;
                                usersec.Type = us.Data.Type;
                                usersec.Value = us.Data.Value;
                            }
                            if (us.Action == "insert")
                            {    
                                usersec = new UserSetting();
                                usersec.UserId = id.ToString();
                                usersec.SettingID = System.Guid.NewGuid();
                                usersec.DataType = us.Data.DataType;
                                usersec.Type = us.Data.Type;
                                usersec.Value = us.Data.Value;
                                ef.UserSettings.Add(usersec);
                            }
                        }
                        ef.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    { }
                    catch (Exception ex)
                    { }
                }
            }
            else
            {
                using (DCT_MangementEntities ef = new DCT_MangementEntities())
                {
                    try
                    {
                        id = System.Guid.NewGuid();
                        AspNetUser u = new AspNetUser();
                        u.Email = email;
                        u.UserName = username;
                        u.Id = id.ToString();
                        u.LockoutEnabled = true;
                        u.PhoneNumberConfirmed = false;
                        u.TwoFactorEnabled = false;
                        u.AccessFailedCount = 0;
                        AspNetRole r = (from anr in ef.AspNetRoles where anr.Id == roleid select anr).FirstOrDefault();
                        u.AspNetRoles.Add(r);
                        ef.AspNetUsers.Add(u);
                        if (usersecurity)
                        {
                            List<AspNetUserPermission> ups = new List<AspNetUserPermission>();
                            List<AspNetPermission> anps = (from anp in ef.AspNetPermissions select anp).ToList();
                            foreach (AspNetPermission anp in anps)
                            {
                                AspNetUserPermission up = new AspNetUserPermission();
                                up.Level = 0;
                                up.InsertDate = DateTime.Now;
                                up.PermissionId = anp.Id;
                                up.UserId = userid;
                                ups.Add(up);
                            }
                            foreach (userpermission userp in UserPermissions)
                            {
                                AspNetUserPermission up = ups.FirstOrDefault(a => a.PermissionId == userp.Key);
                                up.Level = int.Parse(userp.Data.Level);
                            }
                            foreach (AspNetUserPermission k in ups)
                                ef.AspNetUserPermissions.Add(k);
                        }
                        foreach (usersecurity us in UserSecurityList)
                        {
                            UserSetting usersec = (from userset in ef.UserSettings where userset.SettingID.ToString() == us.Key select userset).FirstOrDefault();                      
                            if (us.Action == "insert")
                            {
                                usersec = new UserSetting();
                                usersec.UserId = id.ToString();
                                usersec.SettingID = System.Guid.NewGuid();
                                usersec.DataType = us.Data.DataType;
                                usersec.Type = us.Data.Type;
                                usersec.Value = us.Data.Value;
                                ef.UserSettings.Add(usersec);
                            }
                        }
                        ef.SaveChanges();
                        response.key = id.ToString();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    { }
                    catch (Exception ex)
                    { }
                }
            }
            
            return response;
        }
        public DevResponse GetAllUsers(System.Collections.Specialized.NameValueCollection request, LogModel logmodel = null)
        {
            DevRequest convertRequest = new DevRequest(request);
            switch (convertRequest.CuttentAction)
            {
                case "search":
                  //  return null;
                //case "searchsingle":
                    return this.SearchUser(convertRequest);
                case "delete":
                    return null;
                case "create":
                case "update":
                    return null;
                case"modify":
                    return this.UpdateUserDetail(request);
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
