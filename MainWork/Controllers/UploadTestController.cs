using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Controllers
{
    public class UploadTestController : Controller
    {
        // GET: UploadTest
        public ActionResult upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult upload(tUpload u)
        {
            //若上傳檔案超出最大限制(4mb)，到Web.config的<system.web>標籤內的<httpRuntime>標籤添加新的屬性↓
            //maxRequestLength="想更改的長度限制"，單位是KB
            
            //string name = Path.GetFileName(u.file.FileName) + ".png";
            string name = Guid.NewGuid().ToString() + ".mp3";
            var path = Path.Combine(Server.MapPath("~/Music"), name);


            u.file.SaveAs(path);
            u.fDate = DateTime.Now;
            u.fPath = "/Music/" + name;
            dbTestEntities db = new dbTestEntities();
            db.tUpload.Add(u);
            db.SaveChanges();
            return View();
        }
    }
}