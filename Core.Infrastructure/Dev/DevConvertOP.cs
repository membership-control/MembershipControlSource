using Core.Infrastructure.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Dev
{
    public class DevConvertOP
    {
        public static ExpressionOperator ConvertClientOP(string op)
        {
            switch(op)
            {
                case "contains":
                    return ExpressionOperator.Contains;
                case "notcontains":
                    return ExpressionOperator.NotContains;
                case "endswith":
                    return ExpressionOperator.EndsWith;
                case "startswith":
                    return ExpressionOperator.StartsWith;
                case "=":
                    return ExpressionOperator.Equal;
                case "<>":
                    return ExpressionOperator.NotEqual;
                case "<":
                    return ExpressionOperator.LessThan;
                case ">":
                    return ExpressionOperator.GreaterThan;
                case ">=":
                    return ExpressionOperator.GreaterThanOrEqual;
                case "<=":
                    return ExpressionOperator.LessThanOrEqual;
                default:
                    return ExpressionOperator.Contains;
            }
        }
        public static string ConvertClientOPSQL(string op)
        {
            switch (op)
            {
                case "contains":
                case "endswith":
                case "startswith":
                    return "like";
                case "notcontains":
                    return "not like";
                case "=":
                case "<>":
                case "<":
                case ">":
                case ">=":
                case "<=":
                    return op;
                default:
                    return op;
            }
        }
    }
}
