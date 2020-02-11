using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPrj.Models
{
    public class CTakeType
    {
        public IEnumerable<tAlbumType> takeAllType()
        {
            dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
            var result = db.tAlbumTypes.Select(p => p).ToList();
            return result;
        }
    }
}