using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Dynamic;
using System.Text;
using Core.Services.Converters;

namespace WebMembership.MVC
{
    public class NewJsonResult : JsonResult
    {
        public NewJsonResult(object data)
            : base()
        {
            base.Data = data;
        }

        public NewJsonResult(object data, JsonRequestBehavior jsonRequestBehavior)
            : this(data)
        {
            base.JsonRequestBehavior = jsonRequestBehavior;
        }

        public NewJsonResult(object data, JsonRequestBehavior jsonRequestBehavior, string contentType)
            : this(data, jsonRequestBehavior)
        {
            base.ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");

            }
            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JsonRequest_GetNotAllowed");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            //JavaScriptSerializer js = new JavaScriptSerializer();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };  //Chong: handle large data request during selectAll and exportExcel
            js.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() }); 
            var str = js.Serialize(base.Data);
            str = str.Replace("%20", " "); //Chong: temp solutino to decode %20 (space issue)
            //str = Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
            //{
            //    DateTime dt = new DateTime(1970, 1, 1);
            //    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
            //    dt = dt.ToLocalTime();
            //    return dt.ToString("yyyy-MM-dd");
            //});
            response.Write(str);

        }
    }


}