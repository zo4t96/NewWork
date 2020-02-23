using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbMusicStoreTest.Models
{
    public class tProductsForm
    {
        
        public List<tProducts> getAllProducts()
        {
            var q = from x in dbMusicStore.dbme.tProducts
                    select x;
            return q.ToList();
        }
    }
}