using MainWork;
using MainWork.Models;
using MainWork.ViewModels;
using Newtonsoft.Json;
using prjSpotifyProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace prjSpotifyProject.Controllers
{
    public class MemberLoginController : Controller
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
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
                        return RedirectToAction("Main","Homepage");
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
        public ActionResult New(tMember m)
        {
            //string fileName = "";
            //fileName = Guid.NewGuid() + ".jpg";
            //var path = Path.Combine(Server.MapPath("~/Images"), fileName);
            //m.SaveAs(path);

            m.fPicPath = "nobody.jpg";
            db.tMembers.Add(m);
            db.SaveChanges();
            return RedirectToAction("Main","Homepage");
            //tMemberFactory factory = new tMemberFactory();
            //factory.create(c);

            //return RedirectToAction("Main","Homepage");
        }

        public ActionResult Logout()
        {
            Session[CDictionary.SK_CURRENT_LOGINED_USER] = null;
            Session[CDictionary.SK_ACCOUNT] = null;
            return RedirectToAction("Main", "Homepage");
        }

        public ActionResult LoginCheck(string fAccount)
        {
            string account = fAccount;
            tMember loginUser = db.tMembers.FirstOrDefault(m => m.fAccount == account);

            string  message = "此帳號已被使用";

            if (loginUser == null)
            {
                message = "可以使用的帳號";
            }

            if (account == "")
            {
                message = "請輸入帳號";
            }
            return Content(message);
        }
    }
}