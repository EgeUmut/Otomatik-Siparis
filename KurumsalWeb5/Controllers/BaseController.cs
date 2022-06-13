using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb5.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public string Language_code() {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
    }
}