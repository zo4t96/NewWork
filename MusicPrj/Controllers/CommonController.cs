using MusicPrj.Models;
using MusicPrj.ViewModel;
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
            CWebInitailize ad = new CWebInitailize();
            ViewBag.InitialModel = ad.advancedInitial();
            return View();
        }

        [HttpPost]
        public ActionResult Login(CLoginViewModel post)
        {
            string account = post.emailoraccount;
            string password = post.password;
            Session[CDictionary.SK_ACCOUNT] = account;
            CWebInitailize ad = new CWebInitailize();
            ViewBag.InitialModel = ad.advancedInitial();
            Response.Redirect("~/Album/Index/");
            return View();
        }

        public ActionResult Loginout()
        {
            CWebInitailize ad = new CWebInitailize();
            ViewBag.InitialModel = ad.advancedInitial();
            Session[CDictionary.SK_ACCOUNT] = "";
         //   Response.Redirect(Request.Url.ToString());
            return View();
        }

        public ActionResult _PlayLists()
        {
                return Content("<span>你還沒登入喔<span>");
        }

        }
}