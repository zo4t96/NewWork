using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class CAutoComplete
    {
        dbProjectMusicStoreEntities1 db = new dbProjectMusicStoreEntities1();
        List<string> result = new List<string>();

        public List<string> searchSong(string term)
        {
            var items = db.tProducts.Where(p => p.tAlbum.fStatus == 2);
            var test = items.Where(p => p.fProductName.Contains(term));
            foreach (var t in test)
            {
                if (!result.Contains(t.fProductName))
                {
                    result.Add(t.fProductName);
                }
            }
            result = result.Take(10).ToList();
            return result;
        }

        public List<string> searchSiger(string term)
        {
            var items = db.tProducts.Where(p => p.tAlbum.fStatus == 2);
            var test = items.Where(p => p.fSinger.StartsWith(term));
            foreach (var t in test)
            {
                if (!result.Contains(t.fSinger))
                {
                    result.Add(t.fSinger);
                }
            }
            result = result.Take(10).ToList();
            return result;
        }

        public List<string> searchGroup(string term)
        {
            var items = db.tAlbums.Where(p => p.fStatus == 2);
            var test = items.Where(p => p.fMaker.Contains(term));
            foreach (var t in test)
            {
                if (!result.Contains(t.fMaker))
                {
                    result.Add(t.fMaker);
                }
            }
            result = result.Take(10).ToList();
            return result;
        }

        public List<string> searchComposer(string term)
        {
            var items = db.tProducts.Where(p => p.tAlbum.fStatus == 2);
            var test = items.Where(p => p.fComposer.StartsWith(term));
            foreach (var t in test)
            {
                if (!result.Contains(t.fComposer))
                {
                    result.Add(t.fComposer);
                }
            }
            result = result.Take(10).ToList();
            return result;
        }

        public List<string> searchAlbum(string term)
        {
            var items = db.tAlbums.Where(p => p.fStatus == 2);
            var test = items.Where(p => p.fAlbumName.Contains(term));
            foreach (var t in test)
            {
                if (!result.Contains(t.fAlbumName))
                {
                    result.Add(t.fAlbumName);
                }
            }
            result = result.Take(10).ToList();
            return result;
        }

        internal object searchAccount(string term)
        {
            var items = db.tAlbums.Where(p => p.fStatus == 2);
            var test = items.Where(p => p.fAccount.Contains(term));
            foreach (var t in test)
            {
                if (!result.Contains(t.fAccount))
                {
                    result.Add(t.fAccount);
                }
            }
            result = result.Take(10).ToList();
            return result;
        }
    }
}