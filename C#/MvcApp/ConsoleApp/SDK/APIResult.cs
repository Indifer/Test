using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApp.Models
{

    public class APIResult
    {
        /// <summary>
        /// 结果编码，1为成功
        /// </summary>
        public CodeEnum Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
        

        public APIResult()
            : base()
        {
        }
    }

    public class APIResult<T> : APIResult
    {
        public T Data { get; set; }

        public APIResult()
            : base()
        {
        }

    }
}
