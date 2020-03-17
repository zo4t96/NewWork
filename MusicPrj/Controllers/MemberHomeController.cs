using MainWork.Models;
using prjSpotifyProject.Models;
using prjSpotifyProject.Models.Member;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MainWork.Controllers
{
    public class MemberHomeController : Controller
    {
        // 會員中心
        dbProjectMusicStoreEntities1 db = new dbProjectMusicStoreEntities1();

        public ActionResult Index(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            var member = Session[CDictionary.SK_CURRENT_LOGINED_USER] as tMember;
            var result = db.tMembers.FirstOrDefault(m => m.fAccount == member.fAccount);
            return View(result);
        }

        // 會員資料修改
        public ActionResult EditPage(/*string fAccount*/ bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            var member = Session[CDictionary.SK_CURRENT_LOGINED_USER] as tMember;
            var result = db.tMembers.FirstOrDefault(m => m.fAccount == member.fAccount);
            return View(result);

            //tMember member = db.tMembers.FirstOrDefault(m => m.fAccount == fAccount);
            //if (member == null)
            //{
            //    return RedirectToAction("Index");
            //}
            //return View(new tMember());
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase fPicPath, tMember m)
        {
            tMember member = db.tMembers.First(t => t.fAccount == m.fAccount);

            // 圖片上傳
            string fileName = "";
            if (fPicPath != null)
            {
                if (fPicPath.ContentLength > 0)
                {
                    fileName = Guid.NewGuid() + ".jpg";
                    if (member.fPicPath != "nobody.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("~/Images/" + member.fPicPath));
                    }
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    fPicPath.SaveAs(path);
                    member.fPicPath = fileName;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "MemberHome", new { ajax = true });
        }

    }
}