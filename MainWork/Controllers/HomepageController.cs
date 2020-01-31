using MainWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Controllers
{
    public class HomepageController : Controller
    {
        // GET: Homepage
        
        public ActionResult homepage()
        {
            return View();
        }

        //第一次進入網站會讀這個(會讀取母框架)
        public ActionResult Main()
        {
            //為了進階搜尋內的選項，所以將可選擇的項目在一開始就初始化完畢(最好每個頁面都實作?)
            CWebInitailize ad = new CWebInitailize();
            ViewBag.InitialModel = ad.advancedInitial();
            return View();
        }

        //根據點擊站內連結換頁面的話則會讀這個(不會讀母框架只讀更新的頁面)
        [HttpPost]
        public ActionResult Main(bool ajax)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            return View();
        }
    }
}