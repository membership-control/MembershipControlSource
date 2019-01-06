using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.EF;
using Core.Data.Repository;
using Core.Data.UnitOfWork;
using Core.Infrastruture.Specification;
using Core.Infrastruture.Specification.Implementation;
using Core.Services.DTO.SystemPage;
using Core.Services.Interface;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Utility;
using Core.Infrastructure.DataTables;
using Newtonsoft.Json;
using Core.Infrastructure.Specification;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Core.Identity;
using Core.Identity.Models;
using Core.Infrastructure.Dev;
using Core.Data.Model;
using System.ServiceProcess;

namespace Core.Services.Service
{

    public class ServiceStatus : Repository<TGF_GI_ControlTower_Server_Status_Setting, TGF_IntegrationEntities>, IServiceStatus
    {
        private IMapper _Imapper;

        public ServiceStatus(IntegrationUnitOfWork work, IMapper iImapper)
            : base(work)
        {
            this._Imapper = iImapper;
        }

        public DtResponse DTData(System.Collections.Specialized.NameValueCollection request)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ServiceDetailDTO> GetAllTokenDTO()
        {
            IQueryable<ServiceDetailDTO> data = 
            this.GetAll().UseAsDataSource(this._Imapper).For<ServiceDetailDTO>();
            return data.Where(Predicate => Predicate.Type == "Service");
        }


        #region CRUD methods

        private DevResponse PageData(DevRequest request)
        {
            DevResponse response = new DevResponse();
            IQueryable<ServiceDetailDTO> data =
this.GetAll().UseAsDataSource(this._Imapper).For<ServiceDetailDTO>();

            Specification<ServiceDetailDTO> specs = base.DevGridFilters<ServiceDetailDTO>(request.Filters);
            var filteredData = data.Where(specs.SatisfiedBy());

            if (request.Sorts.Count > 0)
            {
                foreach (var sort in request.Sorts)
                {
                    filteredData = filteredData.OrderBy<ServiceDetailDTO>(sort.fieldname, sort.desc);
                }
            }
            else
            {
                filteredData = filteredData.OrderByDescending(orderby => orderby.Server_Name);
            }

            //var dataPage = filteredData.Skip(request.Skip).Take(request.Take);
            IQueryable<ServiceDetailDTO> dataPage;
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
            Guid guid = Guid.Empty;
            Guid.TryParse(request.Key, out guid);
            IQueryable<ServiceDetailDTO> data =
this.GetAll().UseAsDataSource(this._Imapper).For<ServiceDetailDTO>();
            var singledata = data.Where(predicate => predicate.TGF_GI_ControlTower_Server_Status_Setting_PK == guid).FirstOrDefault();
            response.data = singledata;
            response.key = request.Key;
            return response;
        }

        private DevResponse Remove(DevRequest request, LogModel logmodel)
        {
            DevResponse response = new DevResponse();
            string key = request.Key;
            Guid guid = Guid.Empty;
            bool b = Guid.TryParse(key, out guid);
            if (b)
            {
                var result_Delete = base.DeleteByID(guid);
                if (result_Delete.Code == 0)
                {
                    response.haveError = false;
                    response.key = request.Key;
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
            return response;
        }

        private DevResponse CreateOrEdit(DevRequest request, LogModel logmodel)
        {
            DevResponse response = new DevResponse();
            ServiceDetailDTO orgin = base.ConvertObject<ServiceDetailDTO>(request.Values);

            Guid guid = Guid.Empty;
            Guid.TryParse(request.Key, out guid);

            orgin.TGF_GI_ControlTower_Server_Status_Setting_PK= guid;
            TGF_GI_ControlTower_Server_Status_Setting convert_hanlder = this._Imapper.Map<TGF_GI_ControlTower_Server_Status_Setting>(orgin);
            if (convert_hanlder.TGF_GI_ControlTower_Server_Status_Setting_PK != null && convert_hanlder.TGF_GI_ControlTower_Server_Status_Setting_PK != Guid.Empty)
            {
                var realdata = base.GetByID(convert_hanlder.TGF_GI_ControlTower_Server_Status_Setting_PK);
                base.CopyData<TGF_GI_ControlTower_Server_Status_Setting>(realdata, convert_hanlder);
                realdata.update_date = DateTime.Now;
                realdata.update_user = "CONTROL_TOWER";
                var result_update = base.Update(realdata);
                if (result_update.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.TGF_GI_ControlTower_Server_Status_Setting_PK.ToString();
                    response.data = request.Values;
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
                convert_hanlder.TGF_GI_ControlTower_Server_Status_Setting_PK = id;
                convert_hanlder.insert_date = DateTime.Now;
                convert_hanlder.insert_user = "CONTROL_TOWER";
                var result_add = base.Add(convert_hanlder);
                if (result_add.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.TGF_GI_ControlTower_Server_Status_Setting_PK.ToString();
                    response.data = request.Values;
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_add.Code + "  MSG:" + result_add.ErrMsg;
                }
            }

            logmodel.PK = System.Guid.NewGuid();
            logmodel.Details = this.UnitOfWork.Sql;
            logmodel.Action = request.CuttentAction;
            logmodel.Status = !response.haveError;
            logmodel.Remark = response.error;
            base.Log(logmodel);

            return response;
        }


        #endregion

        public List<ServiceDetailDTO> GetAllData()
        {
            using (TGF_IntegrationEntities db = new TGF_IntegrationEntities())
            {
                return (from a in db.TGF_GI_ControlTower_Server_Status_Setting
                        where a.IsEnabled == true && a.Type == "Service"
                        orderby a.Server_Name ,a.Setting_Detail ascending
                        select new ServiceDetailDTO
                        {
                            TGF_GI_ControlTower_Server_Status_Setting_PK = a.TGF_GI_ControlTower_Server_Status_Setting_PK,
                            Server_Name = a.Server_Name,
                            Setting_Detail = a.Setting_Detail,
                            UniqueKey = a.Setting_Detail + a.Server_Name,
                            Status = "",
                            Description = a.Description,
                            Display_Name = a.Display_Name
                        }).ToList();
            }
        }
        public Infrastructure.Dev.DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, LogModel logmodel)
        {
            DevRequest convertRequest = new DevRequest(request);
            switch (convertRequest.CuttentAction)
            {
                case "search":
                    return this.PageData(convertRequest);
                case "searchsingle":
                    return this.SingleData(convertRequest);
                case "delete":
                    return this.Remove(convertRequest, logmodel);
                case "create":
                case "update":
                    return this.CreateOrEdit(convertRequest, logmodel);
                default:
                    throw new NotImplementedException();
            }
        }
        public bool StartService(string key, bool action, LogModel logmodel )
        {
                ServiceDetailDTO detail;
                Guid setting_pk = Guid.Parse(key);
                using (TGF_IntegrationEntities db = new TGF_IntegrationEntities())
                {
                    detail = (from a in db.TGF_GI_ControlTower_Server_Status_Setting
                              where a.TGF_GI_ControlTower_Server_Status_Setting_PK == setting_pk
                              select new ServiceDetailDTO
                              {
                                  Server_Name = a.Server_Name,
                                  Setting_Detail = a.Setting_Detail,
                              }).FirstOrDefault();
                }
                try
                {
                    if (detail != null)
                    {
                        ServiceController sc = new ServiceController(detail.Setting_Detail, detail.Server_Name);
                        if (action)
                        {
                            sc.Start();
                            sc.WaitForStatus(ServiceControllerStatus.Running);
                        }
                        else
                        {
                            sc.Stop();
                            sc.WaitForStatus(ServiceControllerStatus.Stopped);
                        }

                        logmodel.PK = System.Guid.NewGuid();
                        logmodel.Details = this.UnitOfWork.Sql;
                        logmodel.Action = (action?"on":"off")+ ","+detail.Setting_Detail +","+detail.Server_Name;
                        logmodel.Status =true;
                        logmodel.Remark = "";
                        base.Log(logmodel);
                        return true;
                    }
                    else
                    {
                        logmodel.PK = System.Guid.NewGuid();
                        logmodel.Details = this.UnitOfWork.Sql;
                        logmodel.Action = (action ? "on" : "off") + "," + detail.Setting_Detail + "," + detail.Server_Name;
                        logmodel.Status = false;
                        logmodel.Remark = "Service not found";
                        base.Log(logmodel);
                         return false;
                    }
                }
                catch (Exception ex)
                {
                    logmodel.PK = System.Guid.NewGuid();
                    logmodel.Details = this.UnitOfWork.Sql;
                    logmodel.Action = (action ? "on" : "off") + "," + detail.Setting_Detail + "," + detail.Server_Name;
                    logmodel.Status = false;
                    logmodel.Remark = ex.Message;
                    base.Log(logmodel);
                    return false;
                }
            }          
        }
    }

