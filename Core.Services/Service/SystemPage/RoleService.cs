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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.Services.Service
{
    public class RoleService : Repository<AspNetRole, DCT_MangementEntities>, IRole
    {
        private DCTUnitOfWork _unitofWork;
        private IMapper _Imapper;

        public RoleService(DCTUnitOfWork work, IMapper iImapper) : base(work)
        {
            this._unitofWork = work;
            this._Imapper = iImapper;
        }

 

        #region CRUD methods
        private DevResponse GetAllRolesDTO(DevRequest request)
        {
            DevResponse response = new DevResponse();
            try
            {
            var data = this._unitofWork.CreateSet<AspNetRole>().AsQueryable().ToList();
            
            var process_data = data.Select(r => new
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Discriminator = r.Discriminator,
                Users = r.AspNetUsers.Count > 0 ? r.AspNetUsers.Select(u => u.UserName).ToList() : new List<string>()
            });

            
                var config = new MapperConfiguration(cfg => cfg.CreateMissingTypeMaps = true);
                var mapper = config.CreateMapper();
                var final_data = process_data.Select(a => mapper.Map<RoleDTO>(a)).OrderBy(r => r.Name).ToList();

                response.data = final_data;
                if (request.RequireTotalCount)
                    response.totalCount = final_data.Count();

                return response;
            }
            catch (Exception ex)
            {
                return response;
            }
            
        }

        private DevResponse CreateOrEdit(DevRequest request, LogModel logmodel, dynamic permissions = null)
        {
            DevResponse response = new DevResponse();
            RoleDTO orgin = base.ConvertObject<RoleDTO>(request.Values);

            Guid guid = Guid.Empty;
            Guid.TryParse(request.Key, out guid);
            
            orgin.Id = guid == Guid.Empty ? "" : guid.ToString();
            AspNetRole convert_hanlder = this._Imapper.Map<AspNetRole>(orgin);
            if (!string.IsNullOrEmpty(convert_hanlder.Id))
            {
                var realdata = base.GetByID(convert_hanlder.Id);
                base.CopyData<AspNetRole>(realdata, convert_hanlder);
                var result_update = base.Update(realdata);
                if (result_update.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.Id;
                    response.data = request.Values;
                    //Role Permissions
                    if (permissions.RolePermissions.Count > 0)
                    {
                        try
                        {
                            List<AspNetRolePermission> list_permissions_new = new List<AspNetRolePermission>();
                            foreach (dynamic p in permissions.RolePermissions)
                            {
                                string pid = (string)p.PermissionID;
                                var p_entity = this.UnitOfWork.DbContext.AspNetRolePermissions.Find(convert_hanlder.Id, pid);
                                if (p_entity == null)
                                {
                                    AspNetRolePermission rp = new AspNetRolePermission()
                                    {
                                        RoleId = convert_hanlder.Id,
                                        PermissionId = pid,
                                        Level = p.PermissionLevel,
                                        InsertDate = DateTime.Now
                                    };
                                    list_permissions_new.Add(rp);
                                }
                                else
                                {
                                    p_entity.UpdateDate = DateTime.Now;
                                    p_entity.Level = p.PermissionLevel;
                                    this.UnitOfWork.SetModified<AspNetRolePermission>(p_entity);
                                }
                            }
                            this.UnitOfWork.CreateSet<AspNetRolePermission>().AddRange(list_permissions_new);
                            if (this.UnitOfWork.Commit() < 1)
                            {
                                response.haveError = true;
                                response.error = "CODE:1" + " MSG: Permissions save fail";
                            }
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
                            response.haveError = true;
                            response.error = "CODE:4" + " MSG: Permissions save fail，DbEntityValidationException！";
                        }
                        catch (Exception ex)
                        {
                            response.haveError = true;
                            response.error = "CODE:2" + " MSG: Permissions save fail，exeption！";
                        }
                    }
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
                convert_hanlder.Id = id.ToString();
                var result_add = base.Add(convert_hanlder);
                if (result_add.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.Id;
                    response.data = request.Values;
                    //Role Permissions
                    if (permissions.RolePermissions.Count > 0)
                    {
                        try
                        {
                            List<AspNetRolePermission> list_permissions = new List<AspNetRolePermission>();
                            foreach (dynamic p in permissions.RolePermissions)
                            {
                                AspNetRolePermission rp = new AspNetRolePermission()
                                {
                                    RoleId = convert_hanlder.Id,
                                    PermissionId = p.PermissionID,
                                    Level = p.PermissionLevel,
                                    InsertDate = DateTime.Now
                                };
                                list_permissions.Add(rp);
                            }
                            this.UnitOfWork.CreateSet<AspNetRolePermission>().AddRange(list_permissions);
                            if (this.UnitOfWork.Commit() < 1)
                            {
                                response.haveError = true;
                                response.error = "CODE:1" + " MSG: Permissions save fail";
                            }
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
                            response.haveError = true;
                            response.error = "CODE:4" + " MSG: Permissions save fail，DbEntityValidationException！";
                        }
                        catch (Exception ex)
                        {
                            response.haveError = true;
                            response.error = "CODE:2" + " MSG: Permissions save fail，exeption！";
                        }
                    }
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_add.Code + "  MSG:" + result_add.ErrMsg;
                }
            }

            logmodel.PK = System.Guid.NewGuid();
            logmodel.Details = this._unitofWork.Sql;
            logmodel.Action = request.CuttentAction;
            logmodel.Status = !response.haveError;
            logmodel.Remark = response.error;
            base.Log(logmodel);

            return response;
        }

        private DevResponse Remove(DevRequest request, LogModel logmodel)
        {
            DevResponse response = new DevResponse();
            try
            {
                string key = request.Key;
                var result_Delete = base.DeleteByID(key);
                if (result_Delete.Code == 0)
                {
                    response.haveError = false;
                    response.key = request.Key;
                    response.error = result_Delete.ErrMsg;
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_Delete.Code + "  MSG:" + result_Delete.ErrMsg;
                }

                logmodel.PK = System.Guid.NewGuid();
                logmodel.Details = this.UnitOfWork.Sql;
                logmodel.Action = request.CuttentAction;
                logmodel.Status = !response.haveError;
                logmodel.Remark = response.error;
                base.Log(logmodel);
                
            }
            catch (Exception e)
            { }
            
            return response;
        }


        #endregion

        public Infrastructure.Dev.DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, LogModel logmodel = null)
        {
            DevRequest convertRequest = new DevRequest(request);
            switch (convertRequest.CuttentAction)
            {
                case "search":
                    return this.GetAllRolesDTO(convertRequest);
                //case "searchsingle":
                //    return this.SingleData(convertRequest);
                case "delete":
                    return this.Remove(convertRequest, logmodel);
                case "create":
                case "update":
                    dynamic permissions = null;
                    if (!String.IsNullOrEmpty(request.Get("jsonObj")))
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                        permissions = serializer.Deserialize(request.Get("jsonObj"), typeof(object));
                    }
                    return this.CreateOrEdit(convertRequest, logmodel, permissions);
                default:
                    throw new NotImplementedException();
            }
        }

        public DevResponse DevRolePermissions(string id, LogModel logmodel = null)
        {
            DevResponse response = new DevResponse();
            try
            {
                var permissions = this.UnitOfWork.CreateSet<AspNetPermission>().AsQueryable();
                var rolepermissions = this.UnitOfWork.CreateSet<AspNetRolePermission>().Where(rp => rp.RoleId == id);
                var roles = this.UnitOfWork.CreateSet<AspNetRole>().AsQueryable();

                var data = from p in permissions
                           join rp in rolepermissions on p.Id equals rp.PermissionId into prp
                           from rrp in prp.DefaultIfEmpty()
                           join r in roles on rrp.RoleId equals r.Id into prrp
                           from allgroup in prrp.DefaultIfEmpty()
                           select new 
                           {
                                RoleName = allgroup.Name,
                                PermissionId = p.Id,
                                Area = p.Area,
                                Description = p.Description,
                                Level = (PermissionLevel)(rrp.Level == null ? 0 : rrp.Level)
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
    }
}
