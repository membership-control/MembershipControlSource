using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Utility
{
    public static class ExpressionUtility
    {
        //makes expression for specific prop  
        public static Expression<Func<TSource, Tkey>> GetExpression<TSource, Tkey>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(Tkey));   //important to use the Expression.Convert   
            return Expression.Lambda<Func<TSource, Tkey>>(conversion, param);
        }

        //makes deleget for specific prop  
        public static Func<TSource, Tkey> GetFunc<TSource, Tkey>(string propertyName)
        {
            return GetExpression<TSource, Tkey>(propertyName).Compile();  //only need to compiled expression  
        }

        //OrderBy overload  
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName, bool decending = false)
        {
            Type Tkey = typeof(TSource).GetProperty(propertyName).PropertyType;
            string str = Tkey.FullName;
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(System\.Nullable`1\[\[){0,1}(?<Result>S[^\s,]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                str = match.Groups["Result"].Value;
            }
            switch (str)
            {
                case "System.Int32":
                    if (decending)
                        return source.OrderByDescending(GetFunc<TSource, int>(propertyName));
                    return source.OrderBy(GetFunc<TSource, int>(propertyName));
                case "System.DateTime":
                    if (decending)
                        return source.OrderByDescending(GetFunc<TSource, DateTime>(propertyName));
                    return source.OrderBy(GetFunc<TSource, DateTime>(propertyName));
                case "System.Single":
                    if (decending)
                        return source.OrderByDescending(GetFunc<TSource, Single>(propertyName));
                    return source.OrderBy(GetFunc<TSource, Single>(propertyName));
                case "System.String":
                    if (decending)
                        return source.OrderByDescending(GetFunc<TSource, string>(propertyName));
                    return source.OrderBy(GetFunc<TSource, string>(propertyName));
                case "System.Decimal":
                    if (decending)
                        return source.OrderByDescending(GetFunc<TSource, decimal>(propertyName));
                    return source.OrderBy(GetFunc<TSource, decimal>(propertyName));
                case "System.Guid":
                    if (decending)
                        return source.OrderByDescending(GetFunc<TSource, Guid>(propertyName));
                    return source.OrderBy(GetFunc<TSource, Guid>(propertyName));
                default:
                    throw new NotImplementedException("不识别的类型");
            }
        }

        //OrderBy overload  
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName, bool decending = false)
        {
            Type Tkey = typeof(TSource).GetProperty(propertyName).PropertyType;
            string str = Tkey.FullName;
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"(System\.Nullable`1\[\[){0,1}(?<Result>S[^\s,]*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                str = match.Groups["Result"].Value;
            }
            switch (str)
            {
                case "System.Int32":
                    if (decending)
                        return source.OrderByDescending(GetExpression<TSource, int>(propertyName));
                    return source.OrderBy(GetExpression<TSource, int>(propertyName));
                case "System.DateTime":
                    if (decending)
                        return source.OrderByDescending(GetExpression<TSource, DateTime>(propertyName));
                    return source.OrderBy(GetExpression<TSource, DateTime>(propertyName));
                case "System.Single":
                      if (decending)
                          return source.OrderByDescending(GetExpression<TSource, Single>(propertyName));
                      return source.OrderBy(GetExpression<TSource, Single>(propertyName));
                case "System.String":
                     if (decending)
                        return source.OrderByDescending(GetExpression<TSource, string>(propertyName));
                     return source.OrderBy(GetExpression<TSource, string>(propertyName));
                case "System.Decimal":
                   if (decending)
                        return source.OrderByDescending(GetExpression<TSource, decimal>(propertyName));
                   return source.OrderBy(GetExpression<TSource, decimal>(propertyName));
                case "System.Guid":
                     if (decending)
                        return source.OrderByDescending(GetExpression<TSource, Guid>(propertyName));
                     return source.OrderBy(GetExpression<TSource, Guid>(propertyName));
                default:
                     throw new NotImplementedException("不识别的类型");
            }
        }

        /// <summary>
        /// not implement yet
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public static IQueryable<TDestination> SelectDynamic<TSource, TDestination>(this IQueryable<TSource> source, IEnumerable<string> fieldNames)
        {
            throw new NotImplementedException();
        }

    }

}
