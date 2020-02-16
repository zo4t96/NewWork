using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using CDictionary;

namespace MusicPrj.Models
{
    public class CPlaylist
    {
        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        public List<tProduct> getUserPlaylistSubscribe(string s1)
        {
            int? fLastPlaySong = db.tMembers.FirstOrDefault(p => p.fAccount == s1).fLastPlaySong;
            List<tPlayList> tPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            int lastPlayID = tPL.FirstOrDefault(p => p.fPlayId == fLastPlaySong).fPlayId;
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
            List<tPurchaseItem> tPL = db.tPurchaseItems.Where(p => p.fCustomer == s1).ToList();
            tPurchaseItem tPI = tPL.FirstOrDefault(p => p.fPurchaseItemID == fLastPlaySong);
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
            tPlayList tPL = new tPlayList();
            tPL.fAccount = s1;
            tPL.fProductID = amid;
            if (tPL == null)
            {
                //          return Redirect(s1);
                s2 = "資料空白,儲存失敗";
                return s2;
            }
            List<tPlayList> tnowPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            if (tnowPL.FirstOrDefault(p => p.fProductID == amid) != null)
            {
                s2 = "資料已存在";
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
    }
}