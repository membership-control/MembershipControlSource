using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Specification
{
    public enum ExpressionOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith,
        NotContains,
        IN,
        NotIN,
        Between,
        INQuery,//dynamic query for in (select  ...)
        NotINQuery,//dynamic query for not  in (select ,,,)
        Exists,
        NotExists,
        InternalIN,
        InternalNotIN
    }
}
