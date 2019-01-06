using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequireAttribute : Attribute
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Constructor for a field attribute defining an error message
        /// </summary>
        /// <param name="msg">Error message</param>
        public RequireAttribute(string msg)
        {
            Msg = msg;
        }
    }
}
