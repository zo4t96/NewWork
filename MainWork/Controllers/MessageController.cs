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
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            int tME = db.tMessages.Where(p => p.fAccountTo == s1 && p.fStatus == 1).Count(); ;
            ViewBag.totalPage = tME % 5 == 0 ? (tME / 5) + 1 : (tME / 5) + 2;
            return View();
        }

        public ActionResult _MessageListView(int page = 1)
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            if (page != -1)
            {
                List<tMessage> tME = db.tMessages.Where(p => p.fAccountTo == s1 && p.fStatus == 1).ToList();
                tME.Reverse();
                ViewBag.index = page;
                foreach (var a in tME.Skip(5 * (page - 1)).Take(5))
                {
                    //未讀信第一次讀取
                    if (a.fReaded == 0 || a.fReaded == null)
                    {
                        a.fReaded = 1;
                    }
                    //已讀信第一次讀取
                    else if (a.fReaded == 1)
                    {
                        a.fReaded = 2;
                    }
                }
                db.SaveChanges();
                if (tME != null)
                {
                    return PartialView("_MessageListView", tME.Skip(5 * (page - 1)).Take(5));
                }
                else
                {
                    return PartialView("_MessageListView");
                }
            }
            else
            {
                List<tMessage> tMesCopy = (new dbProjectMusicStoreEntities()).tMessages.Where(p => p.fAccountFrom == s1 && p.fStatus == 0).ToList();
                tMesCopy.Reverse();
                if (tMesCopy != null)
                {
                    return PartialView("_MessageListView", tMesCopy);
                }
                else
                {
                    return PartialView("_MessageListView");
                }
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

        public ActionResult DelteAllMail()
        {
            string issuerName = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = message.userDeleteAllMail(issuerName);
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }

        public ActionResult DelteAllMailCopy()
        {
            string issuerName = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = message.userDeleteAllMailCopy(issuerName);
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }

        public ActionResult MessageSearch(string text = "")
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            List<tMessage> tME = db.tMessages.Where(p => p.fAccountTo == s1 && p.fStatus == 1 && (p.fContent.Contains(text) || p.fTitle.Contains(text))).ToList();
            tME.Reverse();
            if (tME != null)
            {
                return PartialView("_MessageListView", tME);
            }
            else
            {
                return PartialView("_MessageListView");
            }
        }

    }
}
