using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbMusicStoreTest.Models
{
    public class tShoppingCartFram
    {

        public List<tPurchaseItem> getShoppingCart(string cname)
        {
            var q = from x in dbMusicStore.dbme.tPurchaseItem
                    where x.fCustomer == cname && x.tShoppingCart.fType == 0
                    select x;

            return q.ToList();
        }
        public void shopcartadd(string cname,string pdid)
        {
            var q = from x in dbMusicStore.dbme.tShoppingCart
                    where x.fCustomer == cname && x.fType == 0
                    select x;
            int item;
            
            if (pdid.Contains("si"))
            {
                item = Int32.Parse(pdid.Replace("si", ""));
                var pd = from x in dbMusicStore.dbme.tProducts
                         where x.fAlbumID == item
                         select x;
                cAdd(cname, q, pd,1);
                
            }
            else
            {
                item = Int32.Parse(pdid);
                var pd = from x in dbMusicStore.dbme.tProducts
                         where x.fProductID == item
                         select x;
                cAdd(cname, q, pd,0);
            }
            
            

        }

        public void CarType(int shopcarid)
        {
            dbMusicStore.dbme.tShoppingCart.First(p=>p.fCartID == shopcarid).fType = 1;
            dbMusicStore.dbme.SaveChangesAsync();
        }

        private void cAdd(string cname, IQueryable<tShoppingCart> q, IQueryable<tProducts> pd, int isAlbum)
        {
            if (q.Count() == 0)
            {
                var dts = dbMusicStore.dbme.tShoppingCart.Add(
                     new tShoppingCart
                     {
                         fCustomer = cname,
                         fDate = DateTime.Now.ToLongDateString(),
                         fType = 0
                     }
                 );
                foreach (var k in pd)
                {
                    var dpi = dbMusicStore.dbme.tPurchaseItem.Add(
                    new tPurchaseItem
                    {
                        fPurchaseItemID = dts.fCartID,
                        fCustomer = cname,
                        fProductID = k.fProductID,
                        fPrice = k.fSIPrice,
                        fDate = DateTime.Now.ToLongDateString(),
                        fisAlbum = isAlbum
                    });
                }


                dbMusicStore.dbme.SaveChangesAsync();
            }
            else
            {
                foreach (var k in pd)
                {
                    var dpi = dbMusicStore.dbme.tPurchaseItem.Add(
                    new tPurchaseItem
                    {
                        fPurchaseItemID = q.ToList()[0].fCartID,
                        fCustomer = cname,
                        fProductID = k.fProductID,
                        fPrice = k.fSIPrice,
                        fDate = DateTime.Now.ToLongDateString(),
                        fisAlbum = isAlbum
                    });
                }
                dbMusicStore.dbme.SaveChangesAsync();
            }
        }

        public void CartItemDel(int pID,int cID) 
        {
            tPurchaseItem tpit = dbMusicStore.dbme.tPurchaseItem.Single(p => p.fPurchaseItemID == cID && p.fProductID == pID);
            dbMusicStore.dbme.tPurchaseItem.Remove(tpit);
            dbMusicStore.dbme.SaveChanges();      
        }


    }
}