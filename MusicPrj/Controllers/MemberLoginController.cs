using MusicPrj.Models;
using MusicPrj.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
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

        public ActionResult Login()
        {
            return View();
        }
        // 登入
        [HttpPost]
        public ActionResult Login(CLoginData data)
        {
            if (string.IsNullOrEmpty(data.txtAccount))
                return View("Search", "Kind");
            if (data != null)
            {
                string account = data.txtAccount;
                tMember cust = (new dbProjectMusicStoreEntities()).tMembers.FirstOrDefault(p => p.fAccount == account);
                if (cust != null)
                {
                    if (cust.fPassword.Equals(data.txtPassword))
                    {
                        Session[CDictionary.SK_ACCOUNT] = account;
                        Session[CDictionary.SK_CURRENT_LOGINED_USER] = cust;
                        return RedirectToAction("Main", "Homepage");
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

            return RedirectToAction("Main", "Homepage");
        }

        public ActionResult Logout()
        {
            Session[CDictionary.SK_CURRENT_LOGINED_USER] = null;
            Session[CDictionary.SK_ACCOUNT] = null;
            return RedirectToAction("Main", "Homepage");
        }
    }
}