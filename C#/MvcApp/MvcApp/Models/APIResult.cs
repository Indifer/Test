using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcApp.Models
{

    public class APIResult : ActionResult
    {
        /// <summary>
        /// 结果编码，1为成功
        /// </summary>
        public CodeEnum Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        public string ContentType { get; set; }

        /// <summary>
        /// 序列化设置
        /// </summary>
        protected static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public APIResult()
            : base()
        {
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = ContentType ?? "application/json";
            var res = JsonConvert.SerializeObject(new { Code = Code, Message = Message }, settings);

            response.Write(res);
            response.End();
        }

    }

    public class APIResult<T> : APIResult
    {
        public T Data { get; set; }

        /// <summary>
        /// 序列化设置
        /// </summary>
        protected static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public APIResult()
            : base()
        {
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = ContentType ?? "application/json";
            var res = JsonConvert.SerializeObject(new { Code = Code, Message = Message, Data = Data }, settings);

            response.Write(res);
            response.End();
        }

    }
}
