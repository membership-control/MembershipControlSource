using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Model
{
    public class QueryResult
    {
        public string finalquery { get; set; }
        public string finalqueryWithSort { get; set; }
        public string finalqueryWithRowNO { get; set; }
        public List<System.Data.SqlClient.SqlParameter> sqlparams { get; set; }
        public List<System.Data.Common.DbParameter> dbparams { get; set; }
        public bool isCriteriaNull { get; set; }
    }

    internal class QueryParam
    {
        public System.Data.SqlClient.SqlParameter sqlparam { get; set; }
        public System.Data.Common.DbParameter dbparam { get; set; }
    }

    public enum DBType
    {
        SqlType
    }

    public class CriteriaObject
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
        /// <summary>
        /// Value1 For Between Opreator
        /// </summary>
        public object Value1 { get; set; }
        public string OperatorCode { get; set; }
    }

    public class SortObject
    {
        public string FieldName { get; set; }

        public string Direction { get; set; }
    }

    public class DynamicQueryObject
    {
        public string BaseQuery { get; set; }

        public List<string> SelectColumns { get; set; }

        public List<CriteriaObject> Filters { get; set; }

        public List<SortObject> Sorts { get; set; }

        public int? Start { get; set; }

        public int? Length { get; set; }

        public DBType Type { get; set; }
    }
}
