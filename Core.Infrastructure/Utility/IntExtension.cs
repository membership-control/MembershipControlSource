using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class IntExtension
    {
        public static string  ToClassName(this int i, params string[] ClassName)
        {
            if (ClassName == null || ClassName.Length==0)
                return null;
            int index = ClassName.Length;
            return ClassName[(i % index)];
        }

        public static string ToCustomString(this int? i,string ifnullvalue)
        {
            if (i.HasValue)
                return i.ToString();
            return ifnullvalue;
        }
    }
}
