using System;
using System.Collections.Generic;
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
    }
}