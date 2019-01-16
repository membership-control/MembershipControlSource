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
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Utility;
using Core.Infrastructure.Dev;
using Core.Data.Model;

namespace Core.Services.Service
{
    public class ActivityService : Repository<MEM_Activity, DI_WK_TEMPEntities>, IActivity
    {
        private IMapper _Imapper;

        public ActivityService(WKTempUnitOfWork work, IMapper iImapper)
            : base(work)
        {
            this._Imapper = iImapper;
        }

        #region CRUD methods

        private DevResponse PageData(DevRequest request)
        {
            DevResponse response = new DevResponse();
            IQueryable<ActivityDTO> data =
this.GetAll().UseAsDataSource(this._Imapper).For<ActivityDTO>();

            Specification<ActivityDTO> specs = base.DevGridFilters<ActivityDTO>(request.Filters);
            var filteredData = data.Where(specs.SatisfiedBy());

            if (request.Sorts.Count > 0)
            {
                foreach (var sort in request.Sorts)
                {
                    filteredData = filteredData.OrderBy<ActivityDTO>(sort.fieldname, sort.desc);
                }
            }
            else
            {
                filteredData = filteredData.OrderByDescending(orderby => orderby.ACT_FromDate);
            }

            //var dataPage = filteredData.Skip(request.Skip).Take(request.Take);
            IQueryable<ActivityDTO> dataPage;
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
            IQueryable<ActivityDTO> data =
this.GetAll().UseAsDataSource(this._Imapper).For<ActivityDTO>();
            var singledata = data.Where(predicate => predicate.ACT_ID == request.Key).FirstOrDefault();
            response.data = singledata;
            response.key = request.Key;
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
            ActivityDTO orgin = base.ConvertObject<ActivityDTO>(request.Values);

            Guid guid = Guid.Empty;
            Guid.TryParse(request.Key, out guid);

            orgin.ACT_PK = guid;
            MEM_Activity convert_hanlder = this._Imapper.Map<MEM_Activity>(orgin);
            if (convert_hanlder.ACT_PK != null && convert_hanlder.ACT_PK != Guid.Empty)
            {
                var realdata = base.GetByID(convert_hanlder.ACT_PK);

                convert_hanlder.ACT_InsertDate = realdata.ACT_InsertDate;
                base.CopyData<MEM_Activity>(realdata, convert_hanlder);
                //               realdata.insert_date = DateTime.Now;
                realdata.ACT_UpdateDate = DateTime.Now;
                realdata.ACT_UpdateUser = logmodel.Insert_User;

                var result_update = base.Update(realdata);
                if (result_update.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.ACT_PK.ToString();
                    response.data = realdata;
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
                convert_hanlder.ACT_PK = id;
                convert_hanlder.ACT_InsertDate = DateTime.Now;
                convert_hanlder.ACT_InsertUser = logmodel.Insert_User;

                var result_add = base.Add(convert_hanlder);
                if (result_add.Code == 0)
                {
                    response.haveError = false;
                    response.key = convert_hanlder.ACT_PK.ToString();
                    response.data = convert_hanlder; //request.Values;
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


        public DevResponse DevPageAll(System.Collections.Specialized.NameValueCollection request, LogModel logmodel = null)
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

        public DevResponse CheckID(string ID)
        {
            DevResponse response = new DevResponse();
            
            try
            {
                IQueryable<ActivityDTO> data =
    this.GetAll().UseAsDataSource(this._Imapper).For<ActivityDTO>();
                var query_data = !data.Any(predicate => predicate.ACT_ID.ToUpper() == ID.ToUpper());
                response.data = query_data;

                response.haveError = false;
                response.error = null;

            }
            catch (Exception ex)
            {
                response.haveError = true;
                response.error = ex.InnerException.Message;
            }
            finally
            {}

            return response;
        }
    }
}
