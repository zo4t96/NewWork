using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using CDictionary;

namespace MainWork.Models
{
    public class CPlaylist
    {
        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        //包月連續撥放
        public List<tProduct> getUserPlaylistSubscribe(string s1)
        {
            int? fLastPlaySong = db.tMembers.FirstOrDefault(p => p.fAccount == s1).fLastPlaySong;
            List<tPlayList> tPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            List<tPlayList> tPLOut = null;    
            if (fLastPlaySong != null)
            {
                int lastPlayID = tPL.FirstOrDefault(p => p.fPlayId == fLastPlaySong).fPlayId;
                tPLOut = tPL.Where(p => p.fPlayId >= lastPlayID).ToList();
                tPLOut.AddRange(tPL.Where(p => p.fPlayId < lastPlayID));
            }
            else
            {
                tPLOut = tPL;
            }
            // List<tProduct> tp = null;
            List<tProduct> tp = new List<tProduct>();
            foreach (var a in tPLOut)
            {
                tp.Add(db.tProducts.FirstOrDefault(p => p.fProductID == a.fProductID));
            }
            return tp;
        }
        //包月下一首
        public List<tProduct> getUserPlaylistSubscribeNext(string s1, int amid)
        {
            //int? fLastPlaySong = db.tMembers.FirstOrDefault(p => p.fAccount == s1).fLastPlaySong;
            List<tPlayList> tPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            int lastPlayID = tPL.FirstOrDefault(p => p.fProductID == amid).fPlayId;
            List<tPlayList> tPLOut = null;
            if (lastPlayID != 0)
            {
                tPLOut = tPL.Where(p => p.fPlayId >= lastPlayID).ToList();
                tPLOut.AddRange(tPL.Where(p => p.fPlayId < lastPlayID));
            }
            else
            {
                tPLOut = tPL;
            }
            // List<tProduct> tp = null;
            List<tProduct> tp = new List<tProduct>();
            foreach (var a in tPLOut)
            {
                tp.Add(db.tProducts.FirstOrDefault(p => p.fProductID == a.fProductID));
            }
            return tp;
        }

        public List<tProduct> getUserPlaylistNormal(string s1)
        {
            int? fLastPlaySong = db.tMembers.FirstOrDefault(p => p.fAccount == s1).fLastPlaySong;
            List<tPurchaseItem> tPL = db.tPurchaseItems.Where(p => p.fCustomer == s1 && p.tShoppingCart.fType == 1).ToList();
            tPurchaseItem tPI = tPL.FirstOrDefault(p => p.fProductID == fLastPlaySong);
            List<tPurchaseItem> tPLOut = null;
            if (tPI != null)
            {
                int lastPlayID = tPI.fProductID;
                tPLOut = tPL.Where(p => p.fProductID >= lastPlayID).ToList();
                tPLOut.AddRange(tPL.Where(p => p.fProductID < lastPlayID));
            }
            else
            {
                tPLOut = tPL;
            }
            // List<tProduct> tp = null;
            List<tProduct> tp = new List<tProduct>();
            foreach (var a in tPLOut)
            {
                //如果是買整張專輯則列出所有單曲
                if (a.fisAlbum == 1)
                {
                    tp.AddRange(db.tProducts.Where(p => p.fAlbumID == a.tProduct.fAlbumID));
                }
                //如果是買單曲則只列該單曲
                else
                {
                    tp.Add(db.tProducts.FirstOrDefault(p => p.fProductID == a.fProductID));
                }
            }
            return tp;
        }

        public List<tProduct> getUserPlaylistNormalNext(string s1, int amid)
        {
            //int? fLastPlaySong = db.tMembers.FirstOrDefault(p => p.fAccount == s1).fLastPlaySong;
            List<tPurchaseItem> tPL = db.tPurchaseItems.Where(p => p.fCustomer == s1).ToList();
            tPurchaseItem tPI = tPL.FirstOrDefault(p => p.fProductID == amid);
            List<tPurchaseItem> tPLOut = null;
            if (tPI != null)
            {
                int lastPlayID = tPI.fPurchaseItemID;
                tPLOut = tPL.Where(p => p.fPurchaseItemID >= lastPlayID).ToList();
                tPLOut.AddRange(tPL.Where(p => p.fPurchaseItemID < lastPlayID));
            }
            else
            {
                tPLOut = tPL;
            }
            // List<tProduct> tp = null;
            List<tProduct> tp = new List<tProduct>();
            foreach (var a in tPLOut)
            {
                tp.Add(db.tProducts.FirstOrDefault(p => p.fProductID == a.fProductID));
            }
            return tp;
        }

        public string userAddPlayLists(string s1, int amid)
        {
            string s2 = "";
            //有無此人
            if (db.tMembers.FirstOrDefault(p => p.fAccount == s1) == null)
            {
                s2 = "撥放清單需登入才能使用";
                return s2;
            }
            //是否過期
            DateTime? dt = db.tMembers.FirstOrDefault(p => p.fAccount == s1).fSubscriptEndDate;
            if (dt == null || dt < DateTime.Now)
            {
                s2 = "此帳號包月已過期無法加入單曲";
                return s2;
            }
            //有無此單曲
            if (db.tProducts.FirstOrDefault(p => p.fProductID == amid) == null)
            {
                s2 = "查無此單曲";
                return s2;
            }
            tPlayList tPL = new tPlayList();
            tPL.fAccount = s1;
            tPL.fProductID = amid;
            List<tPlayList> tnowPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            if (tnowPL.FirstOrDefault(p => p.fProductID == amid) != null)
            {
                s2 = "撥放清單已經有此首音樂了";
                return s2;
            }
            db.tPlayLists.Add(tPL);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                s2 = ex.ToString();
                return s2;
            }
            s2 = "成功";
            return s2;
        }

        public string userDeletePlayLists(string s1, int amid)
        {
            string s2 = "";
            //有無此人
            if (db.tMembers.FirstOrDefault(p => p.fAccount == s1) == null)
            {
                s2 = "撥放清單需登入才能使用";
                return s2;
            }
            //是否過期
            DateTime? dt = db.tMembers.FirstOrDefault(p => p.fAccount == s1).fSubscriptEndDate;
            if (dt == null || dt < DateTime.Now)
            {
                s2 = "此帳號包月已過期無法刪除單曲";
                return s2;
            }
            //有無此單曲
            if (db.tProducts.FirstOrDefault(p => p.fProductID == amid) == null)
            {
                s2 = "查無此單曲";
                return s2;
            }
            //查詢該用戶的撥放清單有無此首單曲ID
            tPlayList tPL = db.tPlayLists.FirstOrDefault(p => p.fAccount == s1 && p.fProductID == amid);
            if (tPL == null)
            {
                s2 = "你的撥放清單找不到此首音樂";
                return s2;
            }
            //最後一首單曲時移除member欄位的最後單曲紀錄
            List<tPlayList> tnowPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            if (tnowPL.Count() == 1)
            {
                tM.fLastPlaySong = null;
            }
            //移除此單曲
            db.tPlayLists.Remove(tPL);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                s2 = ex.ToString();
                return s2;
            }
            s2 = "";
            return s2;
        }
    }
}