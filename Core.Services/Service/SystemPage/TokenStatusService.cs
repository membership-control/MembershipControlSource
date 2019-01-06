using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.Model;
using Core.Data.UnitOfWork;
using Core.Infrastructure.DataTables;
using Core.Infrastructure.Model;
using Core.Infrastructure.Utility;
using Core.Services.DTO;
using Core.Services.DTO.SystemPage;
using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Service
{
    public class TokenStatusService : ITokenStatus
    {
        private IntegrationUnitOfWork _unitofWork;
        private IMapper _Imapper;

        public TokenStatusService(IntegrationUnitOfWork work, IMapper iImapper)
        {
            this._unitofWork = work;
            this._Imapper = iImapper;
        }

        #region datatables.net
        private DtResponse PageData(DtRequest request, DtResponse response)
        {
            DynamicQueryObject obj = new DynamicQueryObject();
            #region Where
            List<CriteriaObject> list_criteria = new List<CriteriaObject>();
            foreach (var column in request.Columns)
            {
                if (column.Searchable)
                {
                    list_criteria.Add(new CriteriaObject() { FieldName = column.Data, Value = column.Search.Value, OperatorCode = "Contains" });
                }
            }
            obj.Filters = list_criteria;
            #endregion

            #region Sort
            List<SortObject> list_sort = new List<SortObject>();

            foreach (var order in request.Order)
            {
                if (request.Columns[order.Column].Orderable)
                {
                    if (order != null)
                    {
                        list_sort.Add(new SortObject() { FieldName = request.Columns[order.Column].Data, Direction = (order.Dir ?? "asc").ToLowerInvariant() });
                    }
                }
            }
            obj.Sorts = list_sort;
            #endregion

            obj.BaseQuery = Properties.CustomQuerys.TokenStaus_Query;
            obj.Type = DBType.SqlType;
            obj.Start = request.Start;
            obj.Length = request.Length;
            var result = DynamicQueryUtility.DynamicQuery<TokenStatusModel>(obj);

            string base_query_count = string.Format(Properties.CustomQuerys.Base_Count_Format, Properties.CustomQuerys.TokenStaus_Query);
            string final_query_count = string.Format(Properties.CustomQuerys.Base_Count_Format, result.finalquery);
            string final_query_pagedata = result.finalqueryWithRowNO;
            try
            {
                result.dbparams.Clear();
                result.dbparams.AddRange(result.sqlparams);
                int? recordsTotal = this._unitofWork.ExecuteQuery<int>(base_query_count, new object[] { }).FirstOrDefault();
                int? recordsFiltered = this._unitofWork.ExecuteQuery<int>(final_query_count, result.dbparams.ToArray()).FirstOrDefault();


                #region cloen sqlparameters
                System.Data.SqlClient.SqlParameter[] clonedParameters = new System.Data.SqlClient.SqlParameter[result.sqlparams.Count];
                for (int i = 0, j = result.sqlparams.Count; i < j; i++)
                {
                    clonedParameters[i] = (System.Data.SqlClient.SqlParameter)((ICloneable)result.sqlparams[i]).Clone();
                }
                result.dbparams = new List<System.Data.Common.DbParameter>();
                result.dbparams.AddRange(clonedParameters);
                #endregion

                var dataPage = this._unitofWork.ExecuteQuery<TokenStatusModel>(final_query_pagedata, result.dbparams.ToArray());
                List<TokenStatusDTO> listDest = this._Imapper.Map<List<TokenStatusModel>, List<TokenStatusDTO>>(dataPage.ToList());

                response.data = listDest;
                response.draw = request.Draw;
                response.recordsFiltered = recordsFiltered;
                response.recordsTotal = recordsTotal;
            }
            catch (Exception ex)
            {
                response.error = ex.ToString();
            }
            return response;
        }

        private DtResponse Remove(DtRequest request, DtResponse response)
        {
            throw new NotImplementedException();
        }

        private DtResponse CreateOrEdit(DtRequest request, DtResponse response)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.DataTables.DtResponse DTData(System.Collections.Specialized.NameValueCollection request)
        {
            DtRequest data = new DtRequest(request);
            DtResponse response = new DtResponse();
            switch (data.RequestType)
            {
                case DtRequest.RequestTypes.DataTablesGet:
                case DtRequest.RequestTypes.DataTablesSsp:
                    return this.PageData(data, response);
                case DtRequest.RequestTypes.EditorUpload:
                    throw new NotImplementedException();
                case DtRequest.RequestTypes.EditorRemove:
                    return this.Remove(data, response);
                case DtRequest.RequestTypes.EditorCreate:
                case DtRequest.RequestTypes.EditorEdit:

                    return this.CreateOrEdit(data, response);
                default:
                    throw new NotImplementedException();
            }
        } 
        #endregion

        public List<TokenStatusDTO> GetAllDTO()
        {
            var dataPage = this._unitofWork.ExecuteQuery<TokenStatusModel>(Properties.CustomQuerys.TokenStaus_Query, new object[] { });
            List<TokenStatusDTO> listDest = this._Imapper.Map<List<TokenStatusModel>, List<TokenStatusDTO>>(dataPage.ToList());
            return listDest;
        }


        public Result DevResetToken(Guid[] keys, LogModel logmodel)
        {
            Result rst = new Result();

            if (keys == null || keys.Length==0)
            {
                rst.Code = 3;
                rst.ErrMsg = "data is null";
                return rst;
            }
           var results = (from data in this._unitofWork.DbContext.ImportListenerToken
                          where keys.Contains(data.PK)
                          select data).ToList();

            foreach(var result in results)
            {
                var token = new Data.EF.ImportListenerToken();
                token.PK = Guid.NewGuid();
                token.Status = "FINISHED";
                token.insert_date = DateTime.Now;
                token.insert_user = "CONTROLTOWER";
                token.update_date = null;
                token.update_user = null;
                token.batch_id = result.batch_id;
                
                this._unitofWork.DbContext.ImportListenerToken.Add(token);
            }

            try
            {
                if (this._unitofWork.Commit() < 1)
                {
                    rst.Code = 1;
                    rst.ErrMsg = "save fail！";
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                rst.Code = 4;
                rst.ErrMsg = "save fail，DbEntityValidationException！";
            }
            catch (Exception ex)
            {
                rst.Code = 2;
                rst.ErrMsg = "save fail，exeption！";
            }

            //Logging
            logmodel.PK = System.Guid.NewGuid();
            logmodel.Details = this._unitofWork.Sql;
            logmodel.Action = "create";
            logmodel.Status = rst.Code == 0? true : false;
            logmodel.Remark = rst.ErrMsg;
            this._unitofWork.Log(logmodel);

            return rst;
        }
    }
}
