using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMembership.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class BooleanRequired : ValidationAttribute, IClientValidatable
    {
        public BooleanRequired()
        {
        }

        /// <summary>
        /// 服务器端验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }

        /// <summary>
        /// 客户端JS方法的验证
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule>
               GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "brequired",
            };
            //validationRule.ValidationParameters.Add("value", EqualValue);

            yield return validationRule;
        }
    }
}