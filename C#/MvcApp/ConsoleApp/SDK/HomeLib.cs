using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class HomeLib
    {
        public string Say()
        {
            string url = "/Home/Say";
            string res = APIClient.GetResponse(url);
            return res;
        }

        public MvcApp.Models.APIResult Add(string name)
        {
            string url = "/Home/Say";
            string res = APIClient.GetResponse(url);
            return null;
        }

        public MvcApp.Models.APIResult<Entity.User> Get(long id)
        {
            string url = "/Home/Say";
            string res = APIClient.GetResponse(url);
            return null;
        }
    }
}
