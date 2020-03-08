using MainWork.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class CEvent
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        //全活動查詢
        public IEnumerable<tActivity> eventQuery(string account = "")
        {
            List<tActivity> result = new List<tActivity>();
            //如果有帳號代表只搜索該帳號所上傳的活動
            if (account != "")
            {
                result = db.tActivities.Where(a=>a.fLauncher == account).ToList();
            }
            //沒有代表查全部
            else
            {
                result = db.tActivities.Select(a => a).ToList();
            }
            return result;
        }
        //單一活動查詢
        public tActivity eventSelect(int eventId)
        {
            var result = db.tActivities.Where(a => a.fId == eventId).FirstOrDefault();
            return result;
        }

        //新增活動時查詢音樂用(曲風只要一項有合就會挑出來，所以中間判斷會比較複雜，必須過濾重複)
        //有傳進eventid代表是修改時的查詢，「不能將修改活動對象所有的專輯排除在外!!」
        public IEnumerable<tAlbum> eventAlbum(int[] kinds ,int type ,int eventid = 0)
        {
            IEnumerable<tAlbum> albums;
            if (eventid == 0)
            {
                albums = db.tAlbums.Where(a => a.fActivityID == null && a.fStatus == 2);
            }
            else
            {
                albums = db.tAlbums.Where(a => (a.fActivityID == null || a.fActivityID == eventid) && a.fStatus == 2);
            }
            var kindList = new List<tAlbumKind>();
            if(type != 1)
            {
                albums = db.tAlbums.Where(a => a.fType == type);
            }

            //如果沒有選擇曲風，直接傳回所有專輯
            if (kinds == null)
            {
                kindList = db.tAlbumKinds.Select(k => k).ToList();
                return albums;
            }
            
            //有選擇曲風的狀態下再挑選符合的專輯
            foreach (int item in kinds)
            {
                kindList.Add(db.tAlbumKinds.Where(k => k.KindID == item).FirstOrDefault());
            }
            List<tAlbum> result = new List<tAlbum>();
            List<tAlbum> temp = new List<tAlbum>();

            foreach (var item in kindList)
            {
                temp = albums.Where(a => a.fKinds.Contains(item.KindName)).ToList();
                foreach(var t in temp)
                {
                    if (!result.Contains(t))
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }

        //新增活動
        public void eventCreate(CEventObject eventObj ,string path)
        {
            tActivity ac = new tActivity();
            ac.fTitle = eventObj.eventName;
            ac.fStartTime = eventObj.startDate;
            ac.fEndTime = eventObj.endDate;
            ac.fPhotoPath = path;
            db.tActivities.Add(ac);
            db.SaveChanges();

            //儲存活動後將新活動的id取出並跟折扣一起存在事前選取的專輯當中
            //entity會自動將語法轉換成sql語法去查詢資料庫，但資料庫沒有能夠查詢最後一個項目的方法！(只有TOP()函數)
            //因此這邊的Last()會造成程式無法判斷(只有First()能夠運作)
            //解決方法:反過來排序，使最後一個變第一個就好！！
            var latest = db.tActivities.OrderByDescending(a=>a.fId).FirstOrDefault();
            foreach (int i in eventObj.eventAlbums)
            {
                var album = db.tAlbums.Where(a => a.fAlbumID == i).FirstOrDefault();
                album.fDiscount = eventObj.discount;
                album.fActivityID = latest.fId;
            }
            db.SaveChanges();
        }

        public void eventAlter(CEventObject eventObj)
        {
            //修改資料時，先把該活動的相關專輯的活動清空
            var albums = db.tAlbums.Where(a => a.fActivityID == eventObj.eventId);
            foreach (var a in albums)
            {
                a.fActivityID = null;
            }
            
            var target = db.tActivities.Where(a => a.fId == eventObj.eventId).FirstOrDefault();
            target.fTitle = eventObj.eventName;
            target.fStartTime = eventObj.startDate;
            target.fEndTime = eventObj.endDate;
            if (eventObj.eventImage != null)
            {
                eventObj.eventImage.SaveAs(target.fPhotoPath);
            }
            foreach (int i in eventObj.eventAlbums)
            {
                var album = db.tAlbums.Where(a => a.fAlbumID == i).FirstOrDefault();
                album.fDiscount = eventObj.discount;
                album.fActivityID = eventObj.eventId;
            }
            db.SaveChanges();
        }

        public void eventDelete(int eventId)
        {
            var albums = db.tAlbums.Where(a => a.fActivityID == eventId);
            foreach (var a in albums)
            {
                a.fActivityID = null;
            }
            var target = db.tActivities.Where(a => a.fId == eventId).FirstOrDefault();
            db.tActivities.Remove(target);
            db.SaveChanges();
        }
    }
}