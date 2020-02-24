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


        //販售類型新刪修
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

        public ActionResult TypeEdit(int typeOrigin, string typeChange)
        {
            tAlbumType at = db.tAlbumTypes.Where(p => p.fTypeID == typeOrigin).FirstOrDefault();
            at.fTypeName = typeChange;
            db.SaveChanges();
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
            tAlbumKind kind = new tAlbumKind();
            kind.KindName = kindObj.kindName;
            kind.fColor = kindObj.kindColor;
            //如果有勾選上傳圖片才處理圖片內容
            //圖片檔名判斷已事先在前端限定jpg圖檔，因此此處就不多加條件判斷
            if (kindObj.uploadCheck == "true")
            {
                string filename = Guid.NewGuid().ToString() + ".jpg";
                string path = Path.Combine(Server.MapPath("~/Images"), filename);

                kindObj.kindImage.SaveAs(path);

                kind.fPhotoPath = filename;
            }
            db.tAlbumKinds.Add(kind);
            db.SaveChanges();
            return RedirectToAction("KindAlter", "Admin");
        }

        public ActionResult KindEdit(CKindEditObject kindObj)
        {
            tAlbumKind kind = db.tAlbumKinds.Where(k => k.KindID == kindObj.kindId).FirstOrDefault();

            //修正曲風時假設有修正曲風名字，那麼原本擁有該曲風專輯的相關曲風會被剔除
            if (kind.KindName != kindObj.kindName)
            {
                var albums = db.tAlbums.Where(a => a.fKinds.Contains(kind.KindName));
                foreach (var a in albums)
                {
                    if (a.fKinds.Contains(kind.KindName + ","))
                    {
                        a.fKinds = a.fKinds.Replace(kind.KindName + ",", "");
                    }
                    else
                    {
                        a.fKinds = a.fKinds.Replace(kind.KindName, "");
                    }
                }
            }

            kind.fColor = kindObj.kindColor;
            kind.KindName = kindObj.kindName;
            if (kindObj.uploadCheck == "true")
            {
                string path = Path.Combine(Server.MapPath("~/Images"), kind.fPhotoPath);
                kindObj.kindImage.SaveAs(path);
            }
            db.SaveChanges();
            return RedirectToAction("KindAlter", "Admin");
        }

        public ActionResult KindDelete(int deleteId)
        {
            //刪除曲風之前先將相關專輯的相同曲風都刪除
            string kindName = db.tAlbumKinds.Where(k => k.KindID == deleteId).FirstOrDefault().KindName;
            var albums = db.tAlbums.Where(a => a.fKinds.Contains(kindName));
            foreach (var a in albums)
            {
                if (a.fKinds.Contains(kindName + ","))
                {
                    a.fKinds = a.fKinds.Replace(kindName + ",", "");
                }
                else
                {
                    a.fKinds = a.fKinds.Replace(kindName, "");
                }
            }
            db.SaveChanges();

            var kind = db.tAlbumKinds.Where(k => k.KindID == deleteId).FirstOrDefault();
            db.tAlbumKinds.Remove(kind);
            db.SaveChanges();
            return RedirectToAction("KindAlter", "Admin");
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

        public ActionResult EventAlbum(int type, int[] kinds)
        {
            CEvent ce = new CEvent();
            var result = (ce.eventAlbum(kinds, type)).Select(a => new { a.fAlbumID, a.fAlbumName, a.tAlbumType.fTypeName, a.fKinds });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EventNew(CEventObject eventObj)
        {

            CEvent ce = new CEvent();
            string filename = Guid.NewGuid().ToString() + ".jpg";
            string path = Path.Combine(Server.MapPath("~/Images"), filename);
            eventObj.eventImage.SaveAs(path);
            ce.eventCreate(eventObj, filename);
            return RedirectToAction("EventPage", "Admin");
        }

        public ActionResult EventEdit(int eventId)
        {
            CEvent ce = new CEvent();
            CSearch cs = new CSearch();
            ViewBag.types = cs.takeAllType();
            ViewBag.kinds = cs.takeAllKind();
            return View(ce.eventSelect(eventId));
        }
    }
}