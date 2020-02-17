using MainWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Controllers
{
    public class AdminController : Controller
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        // GET: Admin
        public ActionResult Index()
        {
                return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult Index3()
        {
            return View();
        }
        public ActionResult Index4()
        {
            return View();
        }
        public ActionResult chart(bool ajax = false)
        {
            return View();
        }
        public ActionResult table(bool ajax = false)
        {
            return View();
        }
        public ActionResult form(bool ajax = false)
        {
            return View();
        }
        public ActionResult calendar(bool ajax = false)
        {
            return View();
        }
        public ActionResult map(bool ajax = false)
        {
            return View();
        }

        public ActionResult login(bool ajax = false)
        {
            return View();
        }

        public ActionResult register(bool ajax = false)
        {
            return View();
        }


        //類型曲風新刪修
        public ActionResult TypeAlter()
        {
            CSearch cs = new CSearch();
            return View(cs.takeAllType());
        }

        public ActionResult TypeNew(string typeName)
        {
            tAlbumType at = new tAlbumType();
            at.fTypeName = typeName;
            db.tAlbumTypes.Add(at);
            db.SaveChanges();
            return RedirectToAction("TypeAlter");
        }

        [HttpPost]
        public ActionResult TypeDelete(int deleteId) 
        {
            //刪除類別之前，把所有符合該類別的專輯id全部改為1(類別為不指定)，使這項目不會有NULL情況發生
            var al = db.tAlbums.Where(a => a.fType == deleteId);
            foreach (var a in al)
            {
                a.fType = 1;
            }
            db.SaveChanges();

            tAlbumType at = db.tAlbumTypes.Where(p => p.fTypeID == deleteId).FirstOrDefault();
            db.tAlbumTypes.Remove(at);
            db.SaveChanges();
            return RedirectToAction("TypeAlter");
        }

        [HttpPost]
        public ActionResult TypeAlter(int typeOrigin,string typeChange)
        {
            tAlbumType at = db.tAlbumTypes.Where(p => p.fTypeID == typeOrigin).FirstOrDefault();
            at.fTypeName = typeChange;
            db.SaveChanges();
            return RedirectToAction("TypeAlter");
        }
    }
}