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
        //活動查詢
        public IEnumerable<tActivity> eventQuery()
        {
            var result = db.tActivities.Select(a => a);
            return result;
        }

        //新增活動時查詢音樂用(曲風只要一項有合就會挑出來，所以中間判斷會比較複雜，必須過濾重複)
        public IEnumerable<tAlbum> eventAlbum(int[] kinds , int type) {
            var albums = db.tAlbums.Select(a => a);
            var kindList = new List<tAlbumKind>();
            if(type != 1)
            {
                albums = db.tAlbums.Where(a => a.fType == type);
            }
            if (kinds != null)
            {
                foreach (int item in kinds)
                {
                    kindList.Add(db.tAlbumKinds.Where(k => k.KindID == item).FirstOrDefault());
                }
            }
            else
            {
                kindList = db.tAlbumKinds.Select(k => k).ToList();
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
        public void createEvent(CEventObject eventObj ,string path)
        {
            tActivity ac = new tActivity();
            ac.fTitle = eventObj.eventName;
            ac.fStartTime = eventObj.startDate;
            ac.fEndTime = eventObj.endDate;
            ac.fPhotoPath = path;
            db.tActivities.Add(ac);
            db.SaveChanges();

            //儲存活動後將新活動的id取出並跟折扣一起存在事前選取的專輯當中
            //var latest = db.tActivities.Select(a => a).LastOrDefault();
            //foreach(int i in eventObj.eventAlbums)
            //{
            //    var album = db.tAlbums.Where(a => a.fAlbumID == i).FirstOrDefault();
            //    album.fDiscount = (float)eventObj.discount;
            //    album.fActivity = latest.fId;
            //}
            //db.SaveChanges();
        }
    }
}