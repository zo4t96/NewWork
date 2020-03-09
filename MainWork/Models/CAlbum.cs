using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork
{
    //使用者新增單曲時上傳音樂用的變數類別,更新Entity時沒有此型別會報錯故在此新增
    public partial class tProduct
    {
        public System.Web.HttpPostedFileBase fRealFile { get; set; }
    }
    //使用者新增專輯時上傳圖片用的變數類別,更新Entity時沒有此型別會報錯故在此新增
    public partial class tAlbum
    {
        public System.Web.HttpPostedFileBase fCoverRealFile { get; set; }
    }
}

namespace MainWork.Models
{
    public class CAlbum
    {
        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();

        //使用者新增專輯
        public string userAddAlbum(tAlbum tA, string user)
        {
            string name = Guid.NewGuid().ToString() + ".jpg";
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), name);
            tA.fCoverPath = name;
            tA.fStatus = 0;
            tA.fCoverRealFile.SaveAs(path);
            tA.fAccount = user;
            tA.fYear = DateTime.Now;
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

        //使用者新增單曲
        public string userAddsong(FormCollection formCollection, int amid, HttpPostedFileBase tP_fRealFile)
        {
            string name = Guid.NewGuid().ToString() + ".mp3";
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MusicFiles/"), name);
            tProduct tP = new tProduct();
            tP.fFilePath = name;
            tP_fRealFile.SaveAs(path);
            tP.fAlbumID = amid;

            tP.fProductName = formCollection["tP.fProductName"];
            tP.fSinger = formCollection["tP.fSinger"];
            tP.fComposer = formCollection["tP.fComposer"];
            tP.fSIPrice = (decimal)Single.Parse(formCollection["tP.fSIPrice"]);
            tP.fPlayStart = Double.Parse(formCollection["tP.fPlayStart"]);
            tP.fPlayEnd = Double.Parse(formCollection["tP.fPlayEnd"]);

            db.tProducts.Add(tP);
            try
            {
                db.SaveChanges();
                return "上傳單曲成功";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        //使用者更新試聽時間
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

        //使用者更新專輯資料不含封面
        public string userUpdateAlbumInfo(string albumid, FormCollection formCollection)
        {
            int intAlbumid = Int32.Parse(albumid);
            tAlbum tA = db.tAlbums.FirstOrDefault(p => p.fAlbumID == intAlbumid);
            string s2 = "";
            if (tA == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }
            tA.fAlbumName = formCollection["revisefAlbumName"];
            tA.fMaker = formCollection["revisefMaker"];
            tA.fYear = DateTime.Now;
            tA.fType = Int32.Parse(formCollection["revisefType"]);
            tA.fALPrice = (decimal)Single.Parse(formCollection["revisefALPrice"]);
            tA.fStatus = Int32.Parse(formCollection["o.fstatus"]);
            db.SaveChanges();
            s2 = "成功";
            return s2;
        }

        //使用者下架專輯
        public string userUpdateAlbumCloseStatus(string albumid)
        {
            int intAlbumid = Int32.Parse(albumid);
            tAlbum tA = db.tAlbums.FirstOrDefault(p => p.fAlbumID == intAlbumid);
            string s2 = "";
            if (tA == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }
            tA.fStatus = 0;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            s2 = "下架成功";
            return s2;
        }

        //使用者更新專輯封面(only封面)
        public string userUpdateAlbumFile(string albumid, HttpPostedFileBase fCoverPathUpload)
        {
            int intAlbumid = Int32.Parse(albumid);
            tAlbum tA = db.tAlbums.FirstOrDefault(p => p.fAlbumID == intAlbumid);
            string s2 = "";
            if (tA == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), tA.fCoverPath);
            fCoverPathUpload.SaveAs(path);
            s2 = "成功";
            return s2;
        }

        //使用者透過介面更新單曲資料不含檔案
        public string userUpdateSongInfo(string fProductID, FormCollection formCollection)
        {
            int intSongid = Int32.Parse(fProductID);
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == intSongid);
            string s2 = "";
            if (tP == null)
            {
                s2 = "失敗";
                return s2;
            }
            tP.fProductName = formCollection["fProductName"];
            tP.fSinger = formCollection["fSinger"];
            db.SaveChanges();
            s2 = "成功";
            return s2;
        }

        //使用者更新專輯分類
        public string userUpdateAlbumKind(string albumid, string tAfKinds)
        {
            int intAlbumid = Int32.Parse(albumid);
            tAlbum tA = db.tAlbums.FirstOrDefault(p => p.fAlbumID == intAlbumid);
            string s2 = "";
            if (tA == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }
            tA.fKinds = tAfKinds;
            db.SaveChanges();
            s2 = "成功";
            return s2;
        }

        //使用者或管理員刪除單曲
        public string userDelteSong(int amid, string s2)
        {
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == amid);
            if (tP == null)
            {
                return "Products中找不到這首";
            }
            tAlbum owner = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid && p.fAccount == s2);
            int? tMpri = db.tMembers.FirstOrDefault(p => p.fAccount == s2).fPrivilege;
            if (tMpri < 2 && owner == null)
            {
                return "您無權限執行刪除,必須是管理員或作者";
            }
            //1.關聯資料移除,2.Entity db的產生物件不能在另一個Entity db使用
            List<tPlayList> tPL = db.tPlayLists.Where(p => p.fProductID == amid).ToList();
            List<tPurchaseItem> tPI = db.tPurchaseItems.Where(p => p.fProductID == amid).ToList();
            //   List <tShoppingCart> tSC = db.tShoppingCarts.Where(p => p.fProductID == amid).ToList();
            db.tPlayLists.RemoveRange(tPL);
            db.tPurchaseItems.RemoveRange(tPI);
            //    db.tShoppingCarts.RemoveRange(tSC);
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
                    return "資料庫刪除完成,但檔案不存在或在使用中";
                }
                return "成功";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //使用者或管理員刪除專輯
        public string userDeleteAlbum(int amid, string s2)
        {
            tAlbum tA = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
            if (tA == null)
            {
                return "Album中找不到這首";
            }
            tAlbum owner = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid && p.fAccount == s2);
            int? tMpri = db.tMembers.FirstOrDefault(p => p.fAccount == s2).fPrivilege;
            if (tMpri < 2 && owner == null)
            {
                return "您無權限執行刪除,必須是管理員或作者";
            }
            List<tProduct> tP = db.tProducts.Where(p => p.fAlbumID == amid).ToList();
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
            var Coverpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), tA.fCoverPath);
            if (System.IO.File.Exists(Coverpath))
            {
                System.IO.File.Delete(Coverpath);
            }
            else
            {
                //return "檔案不存在或在使用中";
            }
            string errorFile = "檔案不存在或在使用中:";
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
            if (errorFile != "檔案不存在或在使用中:")
            {
                return errorFile;
            }
            else
            {
                return "成功";
            }
        }

        //使用者透過介面更新單曲資料含檔案
        public string userUpdateSongFile(string fProductID, HttpPostedFileBase fRealFile)
        {
            int intProductID = Int32.Parse(fProductID);
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == intProductID);
            string s2 = "";
            if (tP == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MusicFiles/"), tP.fFilePath);
            fRealFile.SaveAs(path);
            s2 = "成功";
            return s2;
        }

        //使用者直接更新單曲資料不含檔案
        public string userUpdateSongInfoDetail(int soid, FormCollection formCollection)
        {
            //     int intSongid = Int32.Parse(soid);
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == soid);
            string s2 = "";
            if (tP == null)
            {
                s2 = "失敗";
                return s2;
            }
            tP.fProductName = formCollection["revise_fProductName"];
            tP.fSinger = formCollection["revise_fSinger"];
            tP.fComposer = formCollection["revise_fComposer"];
            tP.fSIPrice = (decimal)Single.Parse(formCollection["revise_fSIPrice"]);
            tP.fPlayStart = Double.Parse(formCollection["revise_fPlayStart"]);
            tP.fPlayEnd = Double.Parse(formCollection["revise_fPlayEnd"]);
            db.SaveChanges();
            s2 = "成功";
            return s2;
        }
    }
}