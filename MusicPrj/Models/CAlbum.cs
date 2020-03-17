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
        private dbProjectMusicStoreEntities1 db = new dbProjectMusicStoreEntities1();

        //使用者新增專輯
        public string userAddAlbum(tAlbum tA, string user)
        {
            if (tA.fCoverRealFile == null)
            {
                return "並未選擇上傳檔案";
            }
            if (tA.fCoverRealFile.ContentLength <= 0)
            {
                return "檔案大小不可為0";
            }
            string fileType = tA.fCoverRealFile.FileName.Split('.').Last().ToLower();
            if (!(fileType.Equals("jpg") || fileType.Equals("jpeg") || fileType.Equals("png")))
            {
                return "只接受圖片檔(jpg,jpeg,png)";
            }
            string name = Guid.NewGuid().ToString() + "." + fileType;
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), name);
            tA.fCoverRealFile.SaveAs(path);
            tA.fCoverPath = name;
            tA.fStatus = 0;
            tA.fAccount = user;
            tA.fYear = DateTime.Now;
            tA.fDiscount = 1;
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
            if (tP_fRealFile == null)
            {
                return "並未選擇上傳檔案";
            }
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
        public string userUpdateAlbumInfo(string albumid, FormCollection formCollection,string s3)
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
            tLog tLg = new tLog();
            tLg.fIssuerFrom = s3;
            tLg.fActionTo = s3;
            tLg.fCategory = "update";
            tLg.fMessage = $"user:{s3} update albumid:{albumid} at{DateTime.Now} before{tA.fAlbumName} {tA.fMaker} {tA.fYear} {tA.fType} {tA.fALPrice} {tA.fStatus}after{formCollection["revisefAlbumName"]} {formCollection["revisefMaker"]} {Int32.Parse(formCollection["revisefType"])} {(decimal)Single.Parse(formCollection["revisefALPrice"])} {Int32.Parse(formCollection["o.fstatus"])}";
            db.tLogs.Add(tLg);
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
        public string userUpdateAlbumFile(string albumid, HttpPostedFileBase fCoverPathUpload , string s3)
        {
            if (fCoverPathUpload == null)
            {
                return "並未選擇上傳檔案";
            }
            if (fCoverPathUpload.ContentLength <= 0)
            {
                return "檔案大小不可為0";
            }
            string fileType = fCoverPathUpload.FileName.Split('.').Last().ToLower();
            if (!(fileType.Equals("jpg") || fileType.Equals("jpeg") || fileType.Equals("png")))
            {
                return "只接受圖片檔(jpg,jpeg,png)";
            }
            int intAlbumid = Int32.Parse(albumid);
            tAlbum tA = db.tAlbums.FirstOrDefault(p => p.fAlbumID == intAlbumid);
            string s2 = "";
            if (tA == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }
            var originalPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), tA.fCoverPath);
            tLog tLg = new tLog();
            tLg.fIssuerFrom = s3;
            tLg.fActionTo = s3;
            tLg.fCategory = "update";
            string name = Guid.NewGuid().ToString() + "." + fileType;
            tLg.fMessage = $"user:{s3} update albumid:{albumid} at{DateTime.Now} before{tA.fCoverPath} after{name}";
            db.tLogs.Add(tLg);
            System.IO.File.Delete(originalPath);
            var newPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/CoverFiles/"), name);
            fCoverPathUpload.SaveAs(newPath);
            tA.fCoverPath = name;
            db.SaveChanges();
            //fCoverPathUpload.SaveAs(path);
            s2 = "成功";
            return s2;
        }

        //使用者透過介面更新單曲資料不含檔案
        public string userUpdateSongInfo(string fProductID, FormCollection formCollection, string s1)
        {
            int intSongid = Int32.Parse(fProductID);
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == intSongid);
            string s2 = "";
            if (tP == null)
            {
                s2 = "失敗";
                return s2;
            }
            tLog tLg = new tLog();
            tLg.fIssuerFrom = s1;
            tLg.fActionTo = s1;
            tLg.fCategory = "update";
            tLg.fMessage = $"user:{s1} update productID:{fProductID} at{DateTime.Now} before{tP.fProductName},{tP.fSinger} after{formCollection["fProductName"]},{formCollection["fSinger"]}";
            db.tLogs.Add(tLg);
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
            if (tP.tAlbum.fAccount!= s2)
            {
                return "您無權限執行刪除,必須是作者";
            }
            //1.關聯資料移除,2.Entity db的產生物件不能在另一個Entity db使用
            List<tPlayList> tPL = db.tPlayLists.Where(p => p.fProductID == amid).ToList();
            List<tPurchaseItem> tPI = db.tPurchaseItems.Where(p => p.fProductID == amid).ToList();
            //   List <tShoppingCart> tSC = db.tShoppingCarts.Where(p => p.fProductID == amid).ToList();
            db.tPlayLists.RemoveRange(tPL);
            db.tPurchaseItems.RemoveRange(tPI);
            //    db.tShoppingCarts.RemoveRange(tSC);
            db.tProducts.Remove(tP);
            tLog tLg = new tLog();
            tLg.fIssuerFrom = s2;
            tLg.fActionTo = s2;
            tLg.fCategory = "delete";
            tLg.fMessage = $"user:{s2} delete productID:{amid} at{DateTime.Now}";
            db.tLogs.Add(tLg);
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
            tLog tLg = new tLog();
            tLg.fIssuerFrom = s2;
            tLg.fActionTo = s2;
            tLg.fCategory = "delete";
            tLg.fMessage = $"user:{s2} delete Album ID:{amid} at{DateTime.Now}";
            db.tLogs.Add(tLg);
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
        public string userUpdateSongFile(string fProductID, HttpPostedFileBase fRealFile, string s1)
        {
            if (fRealFile == null)
            {
                return "並未選擇上傳檔案";
            }
            int intProductID = Int32.Parse(fProductID);
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == intProductID);
            string s2 = "";
            if (tP == null)
            {
                //          return Redirect(s1);
                s2 = "失敗";
                return s2;
            }
            tLog tLg = new tLog();
            tLg.fIssuerFrom = s1;
            tLg.fActionTo = s1;
            tLg.fCategory = "update";
            string name = Guid.NewGuid().ToString() + ".mp3";
            tLg.fMessage = $"user:{s1} update productID:{fProductID} at{DateTime.Now} from{tP.fFilePath} to {name}";
            db.tLogs.Add(tLg);
            var originalPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MusicFiles/"), tP.fFilePath);
            System.IO.File.Delete(originalPath);
            var newPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MusicFiles/"), name);
            fRealFile.SaveAs(newPath);
            tP.fFilePath = name;
            db.SaveChanges();
            s2 = "成功";
            return s2;
        }

        //使用者直接更新單曲資料不含檔案
        public string userUpdateSongInfoDetail(FormCollection formCollection,string s1)
        {
            //     int intSongid = Int32.Parse(soid);
            int soid = Int32.Parse(formCollection["revise_fProductID"]);
            tProduct tP = db.tProducts.FirstOrDefault(p => p.fProductID == soid);
            string s2 = "";
            if (tP == null)
            {
                s2 = "失敗";
                return s2;
            }
            tLog tLg = new tLog();
            tLg.fIssuerFrom = s1;
            tLg.fActionTo = s1;
            tLg.fCategory = "update";
            tLg.fMessage = $"user:{s1} update productID:{soid} at{DateTime.Now} from{tP.fProductName} {tP.fSinger} {tP.fComposer} {tP.fSIPrice} {tP.fPlayStart} {tP.fPlayEnd}" +
                $"to {formCollection["revise_fProductName"]} {formCollection["revise_fSinger"]} {formCollection["revise_fComposer"]} {Convert.ToDecimal(formCollection["revise_fSIPrice"])}" +
                $"{Convert.ToDouble(formCollection["revise_fPlayStart"])} {Convert.ToDouble(formCollection["revise_fPlayEnd"])}";
            db.tLogs.Add(tLg);
            tP.fProductName = formCollection["revise_fProductName"];
            tP.fSinger = formCollection["revise_fSinger"];
            tP.fComposer = formCollection["revise_fComposer"];
            tP.fSIPrice = Convert.ToDecimal(formCollection["revise_fSIPrice"]);
            tP.fPlayStart = Convert.ToDouble(formCollection["revise_fPlayStart"]);
            tP.fPlayEnd = Convert.ToDouble(formCollection["revise_fPlayEnd"]);
            db.SaveChanges();
            s2 = "成功";
            return s2;
        }

        public string userBindLineAccount(string s1)
        {
            string s2 = "";
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            if (tM == null)
            {
                s2 = "找不到帳號";
                return s2;
            }
            if (tM.fLineStatus == 0 || tM.fLineStatus == 2)
            {
                s2 = "帳號未至Line進行綁定或已經綁定帳號了";
                return s2;
            }
            tM.fLineStatus = 2;
            db.SaveChanges();
            s2 = "綁定成功";
            return s2;
        }

        public string checkDownloadPrivilege(string s1, int songid)
        {
            string s2 = "";
            //不同會員有相同網址但該會員沒有購買這首時的防護
            int? amid = db.tProducts.FirstOrDefault(p => p.fProductID == songid).fAlbumID;
            if (amid == null)
            {
                amid = 0;
            }
            if (db.tPurchaseItems.FirstOrDefault(p => p.fCustomer == s1 && p.tShoppingCart.fType == 1 && (p.fProductID == songid || (p.fisAlbum == 1 && p.tProduct.tAlbum.fAlbumID == amid))) == null)
            {
                return "你沒有購買這首單曲";
            }
            return s2;
        }
    }
}