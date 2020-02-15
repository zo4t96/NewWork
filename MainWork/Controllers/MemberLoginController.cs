using MainWork;
using MainWork.Models;
using prjSpotifyProject.Models;
using prjSpotifyProject.Models.Member;
using prjSpotifyProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSpotifyProject.Controllers
{
    public class MemberLoginController : Controller
    {
        // 登入連結畫面
        public ActionResult Index()
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] == null)
                return RedirectToAction("Login");
            return View();
        }

        // 登入
        public ActionResult Login(CLoginData data)
        {
            if (string.IsNullOrEmpty(data.txtAccount))
                return View();
            if (data != null)
            {
                tMember cust = (new tMemberFactory()).getByAccount(data.txtAccount);
                if (cust != null)
                {
                    if (cust.fPassword.Equals(data.txtPassword))
                    {
                        Session[CDictionary.SK_CURRENT_LOGINED_USER] = cust;
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.ErrorMessage = "帳號與密碼不符";
            }
            return View();
        }

        // Creat
        public ActionResult New()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult New(tMember c)
        {
            tMemberFactory factory = new tMemberFactory();
            factory.create(c);

            return RedirectToAction("Main","Homepage");
        }
    }
}