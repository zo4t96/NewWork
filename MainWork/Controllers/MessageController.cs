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
        CMessage message = new CMessage();
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MessageBox()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
            List<tMessage> tME = db.tMessages.Where(p => p.fAccountTo == s1 &&p.fStatus==1).ToList();
            tME.Reverse();
            if (tME != null)
            {

                return View(tME);
            }
            else
            {
                return View();
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
            string s1 = "MessageBox";
            string issuerName = Session[CDictionary.SK_ACCOUNT].ToString();
                ViewBag.Msg = message.userDeleteMail(issuerName, mailid);
            TempData["message"] = ViewBag.Msg;
            return Redirect(s1);
        }
    }
}