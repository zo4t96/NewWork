using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPrj.Models
{
    public class CTakeKind
    {
        public IEnumerable<tAlbumKind> takeAllKind()
        {
            dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
            var result = db.tAlbumKinds.Select(p => p);
            return result;
        }
    }
}