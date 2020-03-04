using MainWork.Models;
using MainWork.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Controllers
{
    public class AdminController : Controller
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        CManagement manage = new CManagement();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult chart()
        {
            return View();
        }

        public ActionResult form()
        {
            return View();
        }

        public ActionResult table()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(string account,string password)
        {
            var admin = db.tMembers.Where(m => m.fAccount == "aaa").FirstOrDefault();
            if(account != "aaa")
            {
                ViewBag.msg = "帳號密碼錯誤";
                return View();
            }
            if(password != admin.fPassword)
            {
                ViewBag.msg = "帳號密碼錯誤";
                return View();
            }
            Session[CDictionary.SK_ACCOUNT] = "aaa";
            return RedirectToAction("Index","Admin");
        }

        public ActionResult register()
        {
            return View();
        }


        //販售類型新刪修
        public ActionResult TypeAlter()
        {
            CSearch cs = new CSearch();
            return View(cs.takeAllType());
        }

        public ActionResult TypeNew(string typeName)
        {
            manage.typeNew(typeName);
            return RedirectToAction("TypeAlter");
        }

        public ActionResult TypeDelete(int deleteId)
        {
            manage.typeDelete(deleteId);
            return RedirectToAction("TypeAlter");
        }

        public ActionResult TypeEdit(int typeOrigin, string typeChange)
        {
            manage.typeEdit(typeOrigin,typeChange);
            return RedirectToAction("TypeAlter");
        }

        //音樂風格新刪修
        public ActionResult KindAlter()
        {
            CSearch cs = new CSearch();
            return View(cs.takeAllKind());
        }

        public ActionResult KindNew(CKindEditObject kindObj)
        {
            //Server屬性只能在控制器端使用
            string serverPath = Server.MapPath("~/Images");
            manage.kindNew(kindObj, serverPath);
            return RedirectToAction("KindAlter");
        }

        public ActionResult KindEdit(CKindEditObject kindObj)
        {
            string serverPath = Server.MapPath("~/Images");
            manage.kindEdit(kindObj, serverPath);
            return RedirectToAction("KindAlter");
        }

        public ActionResult KindDelete(int deleteId)
        {
            manage.kindDelete(deleteId);
            return RedirectToAction("KindAlter");
        }

        //活動設計頁面(包含活動查詢與新增活動)
        public ActionResult EventPage()
        {
            CEvent ce = new CEvent();
            CSearch cs = new CSearch();
            ViewBag.types = cs.takeAllType();
            ViewBag.kinds = cs.takeAllKind();
            return View(ce.eventQuery());
        }

        public ActionResult EventAlbum(int type, int[] kinds, int eventId = 0)
        {
            CEvent ce = new CEvent();
            var result = (ce.eventAlbum(kinds, type, eventId)).Select(a => new { a.fAlbumID, a.fAlbumName, a.tAlbumType.fTypeName, a.fKinds });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EventNew(CEventObject eventObj)
        {
            CEvent ce = new CEvent();
            string filename = Guid.NewGuid().ToString() + ".jpg";
            string path = Path.Combine(Server.MapPath("~/Images"), filename);
            eventObj.eventImage.SaveAs(path);
            ce.eventCreate(eventObj, filename);
            return RedirectToAction("EventPage");
        }

        public ActionResult EventEdit(int eventId)
        {
            CEvent ce = new CEvent();
            CSearch cs = new CSearch();
            ViewBag.types = cs.takeAllType();
            ViewBag.kinds = cs.takeAllKind();
            var result = ce.eventSelect(eventId);
            ViewBag.discount = result.tAlbums.Select(a => a).First().fDiscount;
            return View(result);
        }
        
        public ActionResult EventAlter(CEventObject eventObj)
        {
            CEvent ce = new CEvent();
            ce.eventAlter(eventObj);
            return RedirectToAction("EventPage");
        }

        public ActionResult EventDelete(int eventId)
        {
            CEvent ce = new CEvent();
            ce.eventDelete(eventId);
            return RedirectToAction("EventPage");
        }

        //音樂審核頁面
        public ActionResult MusicReview()
        {
            //把所有狀態為送審中(1)的音樂挑出來
            //下架:0　送審中:1　上架:2　強制下架(不得送審):3
            var result = db.tAlbums.Where(a => a.fStatus == 1).ToList();
            return View(result);
        }
        public ActionResult Pass(string account, int albumId)
        {
            manage.pass(account, albumId);
            return Content("");
        }
        public ActionResult noPass(string account, int albumId)
        {
            manage.noPass(account, albumId);
            return Content("");
        }
        public ActionResult MultiplePass(string[] accounts, int[] albums)
        {
            manage.multiplePass(accounts, albums);
            return Content("");
        }
        public ActionResult MultipleNoPass(string[] accounts, int[] albums)
        {
            manage.multipleNoPass(accounts, albums);
            return Content("");
        }

        //上架音樂管理(含查詢&強制下架)
        public ActionResult MusicManage()
        {
            var result = db.tAlbums.Where(a => a.fStatus == 2).ToList();
            return View(result);
        }
        public ActionResult Recall(string account, int albumId)
        {
            manage.recall(account, albumId);
            return Content("");
        }
        public ActionResult MusicList(string keyword, string method)
        {
            CSearch cs = new CSearch();
            var result = cs.MusicManage(keyword, method);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}