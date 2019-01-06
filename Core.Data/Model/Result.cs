using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Model
{
    public class Result
    {
        public Result()
        {
            Code = 0;
        }

        public Result(int code, string errMsg)
        {
            Code = code;
            ErrMsg = errMsg;
        }

        public Result(int code, Exception ex)
        {
            Code = code;
            ErrMsg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
        }
        /// <summary>
        /// 返回代码，0为成功，其他均为错误。
        /// </summary>
        public int Code { get; set; }


        public string ErrMsg { get; set; }
    }
}
