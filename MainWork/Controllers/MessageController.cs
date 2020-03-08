using MainWork;
using MainWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class MessageController : Controller
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        CMessage message = new CMessage();
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MessageBox()
        {
            Session[CDictionary.SK_ACCOUNT] = "aaa";
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            int tME = db.tMessages.Where(p => p.fAccountTo == s1 && p.fStatus == 1).Count(); ;
            ViewBag.totalPage = tME % 5 == 0 ? (tME / 5) + 1 : (tME / 5) + 2;
            return View();
        }

        public ActionResult _MessageListView(int page = 1)
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            List<tMessage> tME = db.tMessages.Where(p => p.fAccountTo == s1 && p.fStatus == 1).ToList();
            tME.Reverse();
            ViewBag.index = page;
            foreach (var a in tME.Skip(5 * (page - 1)).Take(5))
            {
                if (a.fReaded == 0)
                {
                    a.fReaded = 1;
                }
                else if (a.fReaded == 1)
                {
                    a.fReaded = 2;
                }
                else if (a.fReaded == 2)
                {
                    a.fReaded = 3;
                }
            }
            db.SaveChanges();
            if (tME != null)
            {
                return PartialView("_MessageListView", tME.Skip(5 * (page - 1)).Take(5));
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult sentMessage(FormCollection formCollection)
        {
            string senderName = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = message.userSendMail(senderName, formCollection);
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }


        public ActionResult DelteMail(int mailid)
        {
            string issuerName = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = message.userDeleteMail(issuerName, mailid);
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }
    }
}
