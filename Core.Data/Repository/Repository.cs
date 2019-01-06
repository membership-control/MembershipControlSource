using Core.Data.Model;
using Core.Data.UnitOfWork;
using Core.Infrastruture.Specification;
using Core.Infrastruture.Specification.Contract;
using Core.Infrastruture.Specification.Extensions;
using Core.Infrastruture.Specification.Implementation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Core.Infrastructure.Specification;

namespace Core.Data.Repository
{
    public class Repository<T, W> : IRepository<T, W>
        where T : class
        where W : System.Data.Entity.DbContext
    {
        #region Members

        IUnitOfWork<W> _UnitOfWork;

        protected const int PageSize = 10;


        #endregion

        #region Constructor

        public Repository(IUnitOfWork<W> _iUnitOfWork)
        {
            this._UnitOfWork = _iUnitOfWork;

        }


        #endregion

        #region IRepository Members

        public IUnitOfWork<W> UnitOfWork
        {
            get
            {
                return _UnitOfWork;
            }
        }

        public virtual Result Add(T item)
        {
            Result rst = new Result();
            if (item == (T)null)
            {
                rst.Code = 3;
                rst.ErrMsg = "data is null";
                return rst;
            }
            // System.Transactions.TransactionOptions transactionOption = new System.Transactions.TransactionOptions();

            ////设置事务隔离级别
            //transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

            //// 设置事务超时时间为60秒
            //transactionOption.Timeout = new TimeSpan(0, 0, 60);

            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOption))
            //{
                try
                {
                    this._UnitOfWork.CreateSet<T>().Add(item);
                    if (this._UnitOfWork.Commit() < 1)
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
            //    scope.Complete();
            //}
            return rst;

        }

        public virtual Result AddRange(List<T> items)
        {
            Result rst = new Result();
            if (items == null || items.Count == 0)
            {
                rst.Code = 3;
                rst.ErrMsg = "data is null";
                return rst;
            }
            try
            {
                this._UnitOfWork.CreateSet<T>().AddRange(items);
                if (this._UnitOfWork.Commit() < 1)
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
            return rst;

        }

        public virtual Result DeleteBy(T item)
        {
            Result rst = new Result();
            if (item == (T)null)
            {
                rst.Code = 3;
                rst.ErrMsg = "data is null";
                return rst;
            }
            try
            {
                this._UnitOfWork.Attach(item);
                this._UnitOfWork.CreateSet<T>().Remove(item);
                if (this._UnitOfWork.Commit() < 1)
                {
                    rst.Code = 1;
                    rst.ErrMsg = "数据不存在！";
                }
            }
            catch
            {
                rst.Code = 2;
                rst.ErrMsg = "数据删除失败，删除数据时产生异常！";
            }
            return rst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">keys</param>
        /// <returns></returns>
        public virtual Result DeleteByID(params object[] ids)
        {
            Result rst = new Result();
            T entity = this.GetByID(ids);
            Type t = entity.GetType();
            string data = "";
            foreach (var opi in t.GetProperties())
            {
                var value = opi.GetValue(entity);
                data = data + opi.Name + "="+ value+"|"; 
            }
            Result r = this.DeleteBy(entity);
            if (r.Code == 0)
            {
                r.ErrMsg = data;
            }
            return r; 
        }

        public virtual Result DeleteRangeBy(params T[] items)
        {
            Result rst = new Result();
            if (items == (T)null)
            {
                rst.Code = 3;
                rst.ErrMsg = "data is null";
                return rst;
            }
            try
            {
                foreach (var item in items)
                {
                    this._UnitOfWork.Attach(item);
                }
                this._UnitOfWork.CreateSet<T>().RemoveRange(items);
                if (this._UnitOfWork.Commit() < 1)
                {
                    rst.Code = 1;
                    rst.ErrMsg = "数据不存在！";
                }
            }
            catch
            {
                rst.Code = 2;
                rst.ErrMsg = "数据删除失败，删除数据时产生异常！";
            }
            return rst;
        }


        public Result RemoveBy(T item)
        {
            Result rst = new Result();
            this._UnitOfWork.Attach(item);
            this._UnitOfWork.CreateSet<T>().Remove(item);
            return rst;
        }

        public virtual Result Update(T item)
        {
            Result rst = new Result();
            if (item == (T)null)
            {
                rst.Code = 3;
                rst.ErrMsg = "data is null";
                return rst;
            }
            try
            {
                this._UnitOfWork.SetModified(item);
                if (this._UnitOfWork.Commit() < 1)
                {
                    rst.Code = 1;
                    rst.ErrMsg = "save fail！";
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                rst.Code = 5;
                rst.ErrMsg = "`更新数据并发控制";
            }
            catch (Exception ex)
            {
                rst.Code = 2;
                rst.ErrMsg = "数据修改失败，保存数据时产生异常！";
            }
            return rst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">keys</param>
        /// <returns></returns>
        public virtual T GetByID(params object[] ids)
        {
            if (ids != null)
                return this._UnitOfWork.CreateSet<T>().Find(ids);
            else
                return null;
        }

        public virtual IQueryable<T> GetAll()
        {
            return this._UnitOfWork.CreateSet<T>();
        }


        public virtual IQueryable<T> AllMatching(ISpecification<T> specification)
        {
            return this._UnitOfWork.CreateSet<T>().Where(specification.SatisfiedBy());
        }

        public virtual IQueryable<T> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<T, KProperty>> orderByExpression, bool ascending)
        {
            var set = this._UnitOfWork.CreateSet<T>();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
        }

        public virtual IQueryable<T> GetFiltered(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            return this._UnitOfWork.CreateSet<T>().Where(filter);
        }

        public virtual Result Merge(T persisted, T current)
        {

            Result rst = new Result();
            if (persisted == (T)null || current == (T)null)
            {
                rst.Code = 3;
                rst.ErrMsg = "data is null";
                return rst;
            }
            try
            {
                _UnitOfWork.ApplyCurrentValues(persisted, current);
                if (this._UnitOfWork.Commit() < 1)
                {
                    rst.Code = 1;
                    rst.ErrMsg = "save fail！";
                }
            }
            catch
            {
                rst.Code = 2;
                rst.ErrMsg = "数据修改失败，保存数据时产生异常！";
            }
            return rst;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            if (_UnitOfWork != null)
                _UnitOfWork.Dispose();
        }

        #endregion

        #region Logging method

        protected virtual void Log(LogModel logmodel)
        {
            this._UnitOfWork.Log(logmodel);
        }

        #endregion

        #region Function

        protected virtual Specification<R> FilterExpression<R>(string PropertyName, object Value, ExpressionOperator expOP) where R : class
        {
            if (Value == null || string.IsNullOrEmpty(Value.ToString().Trim()))
                return new TrueSpecification<R>();
            ParameterExpression pe = Expression.Parameter(typeof(R), "Filter");
            Type type = typeof(R);
            List<string> listProp = PropertyName.Split('.').ToList();
            var joincondition = this.RecursionProperty(listProp, type, pe);
            if (joincondition == null)
                return new TrueSpecification<R>();
            Expression Left = joincondition.Expression;
            UnaryExpression Right = MappConvert(Value, joincondition.PropertyType);
            Expression con;
            switch (expOP)
            {
                case ExpressionOperator.Equal:
                    con = Expression.Equal(Left, Right);
                    break;
                case ExpressionOperator.Contains:
                    Right = MappConvert(Value, typeof(string));
                    con = Expression.Call(Left, Left.Type.GetMethod("Contains", new Type[] { typeof(string) }), Right);
                    break;
                case ExpressionOperator.NotEqual:
                    con = Expression.NotEqual(Left, Right);
                    break;
                case ExpressionOperator.GreaterThan:
                    con = Expression.GreaterThan(Left, Right);
                    break;
                case ExpressionOperator.GreaterThanOrEqual:
                    con = Expression.GreaterThanOrEqual(Left, Right);
                    break;
                case ExpressionOperator.LessThan:
                    con = Expression.LessThan(Left, Right);
                    break;
                case ExpressionOperator.LessThanOrEqual:
                    con = Expression.LessThanOrEqual(Left, Right);
                    break;
                case ExpressionOperator.NotContains:
                    Right = MappConvert(Value, typeof(string));
                    con = Expression.Not(Expression.Call(Left, Left.Type.GetMethod("Contains"), Right));
                    break;
                case ExpressionOperator.StartsWith:
                    Right = MappConvert(Value, typeof(string));
                    con = Expression.Call(Left, Left.Type.GetMethod("StartsWith", new Type[] { typeof(string) }), Right);
                    break;
                case ExpressionOperator.EndsWith:
                    Right = MappConvert(Value, typeof(string));
                    con = Expression.Call(Left, Left.Type.GetMethod("EndsWith", new Type[] { typeof(string) }), Right);
                    break;
                default:
                    throw new NotImplementedException("OP not finished");
            }

            Expression<Func<R, bool>> result = Expression.Lambda<Func<R, bool>>(con, new ParameterExpression[] { pe });
            return new DirectSpecification<R>(result);
        }

        private ExpressionProperty RecursionProperty(List<string> oList, Type oType, Expression oExpression)
        {
            if (oList.Count == 0)
            {
                return new ExpressionProperty() { Expression = oExpression, PropertyType = oType };
            }
            if (oList[0] == oType.Name)
            {
                oList.RemoveAt(0);
                return RecursionProperty(oList, oType, oExpression);
            }
            var pi = oType.GetProperty(oList[0]);
            if (pi == null)
                return null;
            Type type = oType.GetProperty(oList[0]).PropertyType;
            Expression Left = oExpression;
            if (oExpression != null)
                Left = Expression.Property(oExpression, oList[0]);
            oList.RemoveAt(0);
            return RecursionProperty(oList, type, Left);
        }

        /// <summary>
        /// 针对模型字段类型与数据库模型进行转换
        /// ray.zhang
        /// </summary>
        /// <param name="source"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private UnaryExpression MappConvert(object source, Type t)
        {
            object o = this.ConvertData(source, t);

            return Expression.Convert(Expression.Constant(o), t);
        }

        private object ConvertData(object source, Type t)
        {
            object o = null;
            string str = t.FullName;
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(System\.Nullable`1\[\[){0,1}(?<Result>S[^\s,]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                str = match.Groups["Result"].Value;
            }
            switch (str)
            {
                case "System.Int32":
                    int res = int.MinValue;
                    int.TryParse(source.ToString().Trim(), out res);
                    o = res;
                    break;
                case "System.DateTime":
                    string value = source.ToString().Trim();
                    value = value.Replace("GMT", "");
                    value = System.Text.RegularExpressions.Regex.Replace(value, @"\([^\)]*\)", "");
                    //DateTime dt = DateTime.Parse(value);
                    DateTime dt;
                    if (DateTime.TryParse(value, out dt))
                    {
                        if (dt.ToString("yyyy-MM-dd") == DateTime.MinValue.ToString("yyyy-MM-dd"))
                            return null;
                        else
                            o = DateTime.Parse(dt.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                        return null;
                    break;
                case "System.Single":
                    o = Single.Parse(source.ToString().Trim());
                    break;
                case "System.String":
                    o = source.ToString().Trim();
                    break;
                case "System.Decimal":
                    decimal parse_decimal;
                    if (Decimal.TryParse(source.ToString().Trim(), out parse_decimal))
                        o = parse_decimal; //Decimal.Parse(source.ToString().Trim());
                    else
                        return null;
                    break;
                case "System.Guid":
                    //o = Guid.Parse(source.ToString().Trim());
                    o = !String.IsNullOrEmpty(source.ToString()) ? Guid.Parse(source.ToString().Trim()): Guid.Empty;
                    break;
                case "System.Boolean":
                    o = source;
                    break;
                default:
                    break;
            }
            return o;
        }

        internal protected R ConvertObject<R, O>(O origin)
            where R : class, new()
            where O : class, new()
        {
            R result = new R();
            Type t = result.GetType();
            Type o = origin.GetType();
            foreach (var opi in o.GetProperties())
            {
                var tpi = t.GetProperty(opi.Name);
                if (tpi != null)
                    tpi.SetValue(result, opi.GetValue(origin));
            }
            return result;
        }

        /// <summary>
        /// abandon function
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="origin"></param>
        /// <returns></returns>
        protected R ConvertObject<R>(Dictionary<string, object> origin) where R : class, new()
        {
            R result = new R();
            Type t = result.GetType();
            foreach (var value in origin)
            {
                var tpi = t.GetProperty(value.Key);
                if (tpi != null)
                {
                    var convertValue = this.ConvertData(value.Value, tpi.PropertyType);
                    tpi.SetValue(result, convertValue);
                }

            }
            return result;
        }

        protected R ConvertObject<R>(Dictionary<string, object> origin, List<Core.Infrastructure.DataTables.DtResponse.FieldError> fieldErrors) where R : class, new()
        {
            R result = new R();
            Type t = result.GetType();
            foreach (var value in origin)
            {
                var tpi = t.GetProperty(value.Key);
                if (tpi != null)
                {
                    if (tpi.IsDefined(typeof(Core.Infrastructure.DataTables.Attributes.RequireAttribute)))
                    {
                        if (value.Value == null || string.IsNullOrEmpty(value.Value.ToString()))
                        {
                            var err = tpi.GetCustomAttribute<Core.Infrastructure.DataTables.Attributes.RequireAttribute>();
                            Core.Infrastructure.DataTables.DtResponse.FieldError fe = new Infrastructure.DataTables.DtResponse.FieldError();
                            fe.name = tpi.Name;
                            fe.status = err.Msg;
                            fieldErrors.Add(fe);
                            continue;
                        }
                    }
                    object convertValue = null;
                    try
                    {
                        convertValue = this.ConvertData(value.Value, tpi.PropertyType);
                    }
                    catch (Exception ex)
                    {
                        Core.Infrastructure.DataTables.DtResponse.FieldError fe = new Infrastructure.DataTables.DtResponse.FieldError();
                        fe.name = tpi.Name;
                        fe.status = ex.Message;
                        fieldErrors.Add(fe);
                        continue;
                    }
                    tpi.SetValue(result, convertValue);
                }

            }
            return result;
        }

        internal List<Core.Infrastructure.DataTables.DtResponse.FieldError> ValidObject<R>(R obj) where R : class
        {
            List<Core.Infrastructure.DataTables.DtResponse.FieldError> list = new List<Infrastructure.DataTables.DtResponse.FieldError>();
            Type t = obj.GetType();
            var pis = t.GetProperties();
            foreach (var pi in pis)
            {
                if (pi.IsDefined(typeof(Core.Infrastructure.DataTables.Attributes.RequireAttribute)))
                {
                    var value = pi.GetValue(obj);
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                    {
                        var err = pi.GetCustomAttribute<Core.Infrastructure.DataTables.Attributes.RequireAttribute>();
                        Core.Infrastructure.DataTables.DtResponse.FieldError fe = new Infrastructure.DataTables.DtResponse.FieldError();
                        fe.name = pi.Name;
                        fe.status = err.Msg;
                        list.Add(fe);
                    }
                }
            }
            return list;
        }

        protected void CopyData<T>(T target,T origin)
        {
            Type t = target.GetType();
            Type o = origin.GetType();
            foreach (var opi in o.GetProperties())
            {
                var tpi = t.GetProperty(opi.Name);
                var value = opi.GetValue(origin);
                if (tpi != null && value!=null)
                    tpi.SetValue(target, opi.GetValue(origin));
            }
        }

        protected virtual Specification<R> DevGridFilters<R>(Dictionary<string,object> fileters) where R : class
        {
            Specification<R> specs = new TrueSpecification<R>();
            string connect_op = "and";
            string str_fieldname = string.Empty;
            string str_value = string.Empty;
            string str_op = string.Empty;
            foreach (var item in fileters)
            {
                if (item.Value is Dictionary<string, object>)
                {
                    var data = DevGridFilters<R>(item.Value as Dictionary<string, object>);
                    if (connect_op == "and")
                    {
                        specs = (specs & data);
                    }
                    else
                    {
                        specs = (specs | data);
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
                        ExpressionOperator op = Core.Infrastructure.Dev.DevConvertOP.ConvertClientOP(str_op);
                        var spec = this.FilterExpression<R>(str_fieldname, strs[0], op);
                        if (connect_op == "and")
                        {
                            specs = (specs & spec);
                        }
                        else
                        {
                            specs = (specs | spec);
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
                    ExpressionOperator op1 = Core.Infrastructure.Dev.DevConvertOP.ConvertClientOP(str_op);
                    var spec1 = this.FilterExpression<R>(str_fieldname, str_value, op1);
                    if (connect_op == "and")
                    {
                        specs = (specs & spec1);
                    }
                    else
                    {
                        specs = (specs | spec1);
                    }
                }
            }

            return specs;
        }
        #endregion
    }
}
