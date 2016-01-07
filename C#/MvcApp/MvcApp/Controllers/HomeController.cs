using Entity;
using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public string Say()
        {
            return "Hello world";
        }


        public APIResult Add(string name)
        {
            return new APIResult()
            {
                Code = CodeEnum.Success
            };
        }

        public APIResult<User> Get(long id)
        {
            return new APIResult<User>()
            {
                Code = CodeEnum.Success,
                Data = new User()
                {
                    ID = id,
                    Name = "aa"
                }
            };
        }
    }
}
