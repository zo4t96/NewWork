using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MusicPrj.Models
{
    public class CAlbum
    {
        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        public string userAddAlbum(tAlbum tA, string user)
        {
            string name = Guid.NewGuid().ToString() + ".jpg";
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), name);
            tA.fCoverPath = name;
            tA.fStatus = 1;
            tA.fCoverRealFile.SaveAs(path);
            tA.fAccount = user;
            db.tAlbums.Add(tA);
            try
            {
                db.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            //     return View();
        }
        public string userAddsong(tProduct tP, int amid)
        {
            string name = Guid.NewGuid().ToString() + ".mp3";
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MusicFiles/"), name);
            tP.fFilePath = name;
            tP.fRealFile.SaveAs(path);
            tP.fStatus = 1;
            tP.fKind = "無";
            tP.fAlbumID = amid;
            tP.fPlayStart = Convert.ToDouble(tP.fPlays);
            tP.fPlayEnd = Convert.ToDouble(tP.fPlaye);
            db.tProducts.Add(tP);
            try
            {
                db.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        public string userUpdateSongTryLimit(int amid, float start, float end)
        {
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == amid);
            string s2 = "";
            if (tP == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }

            tP.fPlayStart = start;
            tP.fPlayEnd = end;
            db.SaveChanges();
            s2 = "成功";
            return s2;
        }

        public string userDelteSong(tProduct tP)
        {
            db.tProducts.Remove(tP);
            try
            {
                db.SaveChanges();
                string fileName = tP.fFilePath;
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MusicFiles/"), fileName);
                // Delete the file if it exists.
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    return "檔案不存在或在使用中";
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string userDeleteAlbum(tAlbum tA, List<tProduct> tP)
        {
            var Coverpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), tA.fCoverPath);
            if (System.IO.File.Exists(Coverpath))
            {
                System.IO.File.Delete(Coverpath);
            }
            else
            {
                return "檔案不存在或在使用中";
            }
            string errorFile= "檔案不存在或在使用中:";
            foreach (var prod in tP)
            {
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MusicFiles/"), prod.fFilePath);
                // Delete the file if it exists.
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    errorFile += path;
                }
            }
            db.tProducts.RemoveRange(tP);
            db.tAlbums.Remove(tA);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            if(errorFile!= "檔案不存在或在使用中:")
            {
                return errorFile;
            }
            else
            {
            return "";
            }
        }


    }
}