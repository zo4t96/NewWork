using MusicPrj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MessageBox()
        {
            CWebInitailize ad = new CWebInitailize();
            ViewBag.InitialModel = ad.advancedInitial();
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
            List<tMessage> tME = db.tMessages.Where(p => p.fAccountTo == s1).ToList();
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
    }
}