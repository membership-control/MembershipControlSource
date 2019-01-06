using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastruture.Specification.Extensions
{
    public class ExpressionProperty
    {
        //表达式
        public Expression Expression { get; set; }
        //数据类型
        public Type PropertyType { get; set; }
    }
}
