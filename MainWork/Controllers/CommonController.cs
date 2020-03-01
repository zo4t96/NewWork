using MainWork.Models;
using MainWork.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        // GET: Common
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(CLoginViewModel post)
        {
            string account = post.emailoraccount;
            string password = post.password;
            Session[CDictionary.SK_ACCOUNT] = account;
            //這邊移動的網址改變
            Response.Redirect("~/Homepage/Main/");
            return View();
        }

        public ActionResult Loginout()
        {
            Session[CDictionary.SK_ACCOUNT] = "";
            //Response.Redirect(Request.Url.ToString());
            return View();
        }

        public ActionResult _PlayLists()
        {
            return Content("<span>你還沒登入喔<span>");
        }

    }
}