using MusicPrj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class HomepageController : Controller
    {
        //第一次進入網站會讀這個(會讀取母框架)
        public ActionResult Main(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            CSearch cs = new CSearch();
            CEvent ce = new CEvent();
            ViewBag.events = ce.eventQuery();
            //該頁面razor有使用model.xxx.導覽屬性的寫法，但IIS伺服器不吃IEnumable(的導覽屬性)
            //目前先將傳過去的資料轉換成List<>形式，可以確保頁面正常運作
            return View(cs.allAlbum().ToList());
        }

        public ActionResult _PlayLists()
        {
            return Content("<span>你還沒登入喔<span>");
        }
    }
}