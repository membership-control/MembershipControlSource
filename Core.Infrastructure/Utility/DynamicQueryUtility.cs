using Core.Infrastructure.Model;
using Core.Infrastructure.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Utility
{
    public class DynamicQueryUtility
    {
        public static QueryResult DynamicQuery<T>(DynamicQueryObject obj)
        {
            QueryResult result = new QueryResult();
            string strtext = " from (" + obj.BaseQuery + ") " + (obj.Type == DBType.SqlType ? "as" : string.Empty) + " Filter1";
            string strwhere = " where ";
            string strphrase = " Filter1.";
            string strand = " and ";

            string stror = " or ";
            List<System.Data.SqlClient.SqlParameter> sqlparams = new List<System.Data.SqlClient.SqlParameter>();
            List<System.Data.Common.DbParameter> objects = new List<System.Data.Common.DbParameter>();
            StringBuilder sbwhere = new StringBuilder();
            StringBuilder sbselect = new StringBuilder();
            StringBuilder sbsort = new StringBuilder();
            Boolean isand = false;
            Boolean iswhere = true;
            int whereCount = 0;
            string paramsString = string.Empty;
            if (obj.Type == DBType.SqlType)
                paramsString = "@";
            if (obj.Filters != null && obj.Filters.Count > 0)
            {
                foreach (CriteriaObject criteriaObject in obj.Filters)
                {
                    try
                    {
                        Type type = typeof(T);
                        var pi = type.GetProperty(criteriaObject.FieldName.Trim());
                        if (pi == null)
                            continue;
                        if (criteriaObject.Value == null)
                            continue;
                        if (string.IsNullOrEmpty(criteriaObject.Value.ToString()))
                            continue;

                        Type returntype = type.GetProperty(criteriaObject.FieldName.Trim()).PropertyType;
                        if (isand)
                        {
                            sbwhere.Append(strand);
                        }
                        else
                        {
                            isand = true;
                        }
                        if (iswhere)
                        {
                            sbwhere.Append(strwhere);
                            iswhere = false;
                        }
                        string strop = null;
                        ExpressionOperator op = (ExpressionOperator)Enum.Parse(typeof(ExpressionOperator), criteriaObject.OperatorCode);
                        object oparamter = null;
                        QueryParam qp;
                        Boolean isorin = false;
                        switch (op)
                        {
                            case ExpressionOperator.Equal:
                                strop = " = ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim(), returntype);
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.NotEqual:
                                strop = " != ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim(), returntype);
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.GreaterThan:
                                strop = " > ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim(), returntype);
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.GreaterThanOrEqual:
                                strop = " >= ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim(), returntype);
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.LessThan:
                                strop = " < ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim(), returntype);
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.LessThanOrEqual:
                                strop = " <= ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim(), returntype);
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.Contains:
                                strop = " like ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType("%" + oparamter + "%", paramsString + criteriaObject.FieldName.Trim(), typeof(string));
                                sqlparams.Add(qp.sqlparam);

                                break;
                            case ExpressionOperator.EndsWith:
                                strop = " like ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim() + "");
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType("%" + oparamter, paramsString + criteriaObject.FieldName.Trim(), typeof(string));
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.StartsWith:
                                strop = " like ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter + "%", paramsString + criteriaObject.FieldName.Trim(), typeof(string));
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.NotContains:
                                strop = " not like ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim());
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType("%" + oparamter + "%", paramsString + criteriaObject.FieldName.Trim(), typeof(string));
                                sqlparams.Add(qp.sqlparam);
                                break;
                            case ExpressionOperator.IN:
                                strop = " = ";
                                string[] strs = criteriaObject.Value.ToString().Split(',');
                                if (strs != null)
                                {
                                    sbwhere.Append(" ( ");
                                    for (int i = 0; i < strs.Length; i++)
                                    {
                                        if (isorin)
                                        {
                                            sbwhere.Append(stror);
                                        }
                                        else
                                        {
                                            isorin = true;
                                        }

                                        sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim() + i.ToString());
                                        oparamter = MappConvert(strs[i], returntype);

                                        qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim() + i.ToString(), returntype);
                                        sqlparams.Add(qp.sqlparam);
                                    }
                                    sbwhere.Append(" ) ");
                                }
                                break;
                            case ExpressionOperator.NotIN:
                                strop = " != ";
                                string[] strsnotin = criteriaObject.Value.ToString().Split(',');
                                if (strsnotin != null)
                                {
                                    sbwhere.Append(" ( ");
                                    for (int i = 0; i < strsnotin.Length; i++)
                                    {
                                        if (isorin)
                                        {
                                            sbwhere.Append(strand);
                                        }
                                        else
                                        {
                                            isorin = true;
                                        }
                                        sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + paramsString + criteriaObject.FieldName.Trim() + i.ToString());
                                        oparamter = MappConvert(strsnotin[i], returntype);

                                        qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim() + i.ToString(), returntype);
                                        sqlparams.Add(qp.sqlparam);
                                    }
                                    sbwhere.Append(" ) ");
                                }
                                break;
                            case ExpressionOperator.Between:
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + ">= " + paramsString + criteriaObject.FieldName.Trim() + "1");
                                oparamter = MappConvert(criteriaObject.Value, returntype);
                                qp = MappDBType(oparamter, paramsString + criteriaObject.FieldName.Trim() + "1", returntype);
                                sqlparams.Add(qp.sqlparam);
                                sbwhere.Append(strand);
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + "<= " + paramsString + criteriaObject.FieldName.Trim() + "2");
                                object oparamter1 = MappConvert(criteriaObject.Value1, returntype);
                                qp = MappDBType(oparamter1, paramsString + criteriaObject.FieldName.Trim() + "2", returntype);
                                sqlparams.Add(qp.sqlparam);

                                break;
                            case ExpressionOperator.INQuery:
                                strop = " IN  ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + " (" + criteriaObject.Value + ")");

                                break;
                            case ExpressionOperator.NotINQuery:
                                strop = " NOT IN  ";
                                sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\"" + strop + " (" + criteriaObject.Value + ")");
                                break;
                            case ExpressionOperator.Exists:
                                strop = "exists";
                                sbwhere.Append(strop + "( select 1  from (" + criteriaObject.Value + ") where " + criteriaObject.FieldName.Trim() + " = " + strphrase + criteriaObject.FieldName.Trim() + ")");
                                break;
                            case ExpressionOperator.NotExists:
                                strop = "not exists";
                                sbwhere.Append(strop + "( select 1  from (" + criteriaObject.Value + ") where " + criteriaObject.FieldName.Trim() + " = " + strphrase + criteriaObject.FieldName.Trim() + ")");
                                break;
                            case ExpressionOperator.InternalIN:
                                strop = " IN ";
                                string[] strs_InternalIN = criteriaObject.Value.ToString().Split(',');
                                if (strs_InternalIN != null)
                                {
                                    sbwhere.Append(" ( ");
                                    sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\" " + strop + " (");
                                    for (int i = 0; i < strs_InternalIN.Length; i++)
                                    {
                                        sbwhere.Append("'" + strs_InternalIN[i].Trim() + "'");
                                        if (i != strs_InternalIN.Length - 1)
                                            sbwhere.Append(",");
                                    }
                                    sbwhere.Append(") ) ");
                                }
                                break;
                            case ExpressionOperator.InternalNotIN:
                                strop = " Not IN ";
                                string[] strs_InternalNotIN = criteriaObject.Value.ToString().Split(',');
                                if (strs_InternalNotIN != null)
                                {
                                    sbwhere.Append(" ( ");
                                    sbwhere.Append(strphrase + "\"" + criteriaObject.FieldName.Trim() + "\" " + strop + " (");
                                    for (int i = 0; i < strs_InternalNotIN.Length; i++)
                                    {
                                        sbwhere.Append("'" + strs_InternalNotIN[i].Trim() + "'");
                                        if (i != strs_InternalNotIN.Length - 1)
                                            sbwhere.Append(",");
                                    }
                                    sbwhere.Append(") ) ");
                                }
                                break;
                            default:

                                break;
                        }
                        whereCount++;
                    }
                    catch
                    {
                        Console.WriteLine("error auto generate where query");
                    }
                }
            }
            string strselect = "Select ";
            if (obj.Filters == null || whereCount == 0)
            {
                result.isCriteriaNull = true;
            }

            string strcomma = ",";
            sbselect.Append(strselect);
            if (obj.SelectColumns != null && obj.SelectColumns.Count > 0)
            {
                foreach (string field in obj.SelectColumns)
                {
                    try
                    {
                        Type type = typeof(T);
                        var pi = type.GetProperty(field);
                        if (pi == null)
                            continue;
                        sbselect.Append(strphrase + "\"" + field + "\"" + strcomma);
                    }
                    catch
                    {
                        Console.WriteLine("error auto generate select query");
                    }
                }
            }
            else
            {
                Type type = typeof(T);
                foreach (var pi in type.GetProperties())
                {
                    sbselect.Append(strphrase + "\"" + pi.Name + "\"" + strcomma);
                }
            }
            result.finalquery = sbselect.ToString().TrimEnd(',') + strtext + sbwhere.ToString();
            result.sqlparams = sqlparams;
            result.dbparams = objects;


            //Sort
            if (obj.Sorts != null && obj.Sorts.Count > 0)
            {
                sbsort.Append(" order by ");
                try
                {
                    foreach (SortObject so in obj.Sorts)
                    {
                        Type type = typeof(T);
                        var pi = type.GetProperty(so.FieldName);
                        if (pi == null)
                            continue;
                        sbsort.Append(strphrase + "\"" + so.FieldName + "\"" + " " + so.Direction + " ");
                    }
                }
                catch
                {
                    Console.WriteLine("error auto generate Sort");
                }
                result.finalqueryWithSort = sbselect.ToString().TrimEnd(',') + strtext + sbwhere.ToString() + sbsort.ToString();
                if (obj.Start.HasValue && obj.Length.HasValue && obj.Length.Value > 0)
                {
                    string select = sbselect.ToString();
                    string select2 = select.TrimEnd(',').Replace("Filter1", "Filter2");
                    result.finalqueryWithRowNO = select2 + " from ( " + select + "ROW_NUMBER() OVER(" + sbsort.ToString() + ") AS \"RowNumber\" "
                        + strtext + sbwhere.ToString() + "  ) as Filter2 where Filter2.\"RowNumber\" BETWEEN " + obj.Start.Value + " and " + (obj.Start.Value + obj.Length.Value) + " ";
                }
            }
            return result;
        }

        private static object MappConvert(object source, Type t)
        {
            if (source == null)
                return DBNull.Value;
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
                    try
                    {
                        o = int.Parse(source.ToString().Trim());
                    }
                    catch
                    {
                        o = null;
                    }
                    break;
                case "System.DateTime":
                    DateTime dt = DateTime.Parse(source.ToString().Trim());
                    if (dt.ToString("yyyy-MM-dd") == DateTime.MinValue.ToString("yyyy-MM-dd"))
                        return DBNull.Value;
                    else
                        o = DateTime.Parse(dt.ToString("yyyy-MM-dd"));
                    break;
                case "System.Single":
                    try
                    {
                        o = Single.Parse(source.ToString().Trim());
                    }
                    catch
                    {
                        o = null;
                    }
                    break;
                case "System.String":
                    o = source.ToString().Trim();
                    break;
                case "System.Decimal":
                    try
                    {
                        o = Decimal.Parse(source.ToString().Trim());
                    }
                    catch
                    {
                        o = null;
                    }
                    break;
                case "System.Guid":
                    try
                    {
                        o = Guid.Parse(source.ToString().Trim());
                    }
                    catch
                    {
                        o = Guid.Empty;
                    }
                    break;
                default:
                    break;
            }
            return o;
        }

        private static QueryParam MappDBType(object ivalue, string iname, Type type)
        {
            QueryParam o = new QueryParam();
            string str = type.FullName;
            System.Data.SqlClient.SqlParameter sql;
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(System\.Nullable`1\[\[){0,1}(?<Result>S[^\s,]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                str = match.Groups["Result"].Value;
            }

            sql = new System.Data.SqlClient.SqlParameter(iname, ivalue);

            switch (str)
            {
                case "System.Int32":
                    sql.IsNullable = true;
                    sql.SqlDbType = System.Data.SqlDbType.Int;
                    break;
                case "System.DateTime":
                    sql.IsNullable = true;
                    sql.SqlDbType = System.Data.SqlDbType.Date;
                    break;
                case "System.Single":
                    sql.IsNullable = true;
                    sql.SqlDbType = System.Data.SqlDbType.SmallInt;
                    break;
                case "System.String":
                    sql.IsNullable = true;
                    sql.SqlDbType = System.Data.SqlDbType.NVarChar;
                    break;
                case "System.Decimal":
                    sql.IsNullable = true;
                    sql.SqlDbType = System.Data.SqlDbType.Decimal;
                    break;
                case "System.Guid":
                    sql.IsNullable = true;
                    sql.SqlDbType = System.Data.SqlDbType.UniqueIdentifier;
                    break;
                default:
                    break;
            }
            o.sqlparam = sql;
            return o;
        }
    }
}
