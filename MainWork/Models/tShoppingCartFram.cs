using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class tShoppingCartFram
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        public List<tPurchaseItem> getShoppingCart(string cname)
        {
            var q = from x in db.tPurchaseItems
                    where x.fCustomer == cname && x.tShoppingCart.fType == 0
                    select x;

            return q.ToList();
        }
        public void shopcartadd(string cname, string pdid)
        {
            var q = from x in db.tShoppingCarts
                    where x.fCustomer == cname && x.fType == 0
                    select x;
            int item;

            if (pdid.Contains("si"))
            {
                item = Int32.Parse(pdid.Replace("si", ""));
                var pd = from x in db.tProducts
                         where x.fProductID == item
                         select x;
                cAdd(cname, q.ToList(), pd.ToList(), 1);

            }
            else
            {
                item = Int32.Parse(pdid);
                var pd = from x in db.tProducts
                         where x.fAlbumID == item
                         select x;
                cAdd(cname, q.ToList(), pd.ToList(), 0);
            }



        }



        public void CarType(int shopcarid)
        {

            var q = from x in db.tPurchaseItems
                    where x.fPurchaseItemID == shopcarid && x.tShoppingCart.fType == 0
                    select x;
            foreach (var x in q)
            {
                x.fDiscount = x.tProduct.tAlbum.fDiscount;
            };
            db.tShoppingCarts.First(p => p.fCartID == shopcarid).fType = 1;
            db.SaveChanges();
        }

        private void cAdd(string cname, List<tShoppingCart> q, List<tProduct> pd, int isAlbum)
        {
            if (q.Count() == 0)
            {
                var dts = db.tShoppingCarts.Add(
                     new tShoppingCart
                     {
                         fCustomer = cname,
                         fDate = DateTime.Now,
                         fType = 0
                     }
                 );
                foreach (var k in pd)
                {
                    var dpi = db.tPurchaseItems.Add(
                    new tPurchaseItem
                    {
                        fPurchaseItemID = dts.fCartID,
                        fCustomer = cname,
                        fProductID = k.fProductID,
                        fPrice = k.fSIPrice,
                        fDate = DateTime.Now,
                        fisAlbum = isAlbum
                    });
                }


                db.SaveChanges();
            }
            else
            {
                foreach (var k in pd)
                {
                    if (!q.Any(p => p.tPurchaseItems.Any(s => s.fProductID == k.fProductID)))
                    {
                        db.tPurchaseItems.Add(
                    new tPurchaseItem
                    {
                        fPurchaseItemID = q.ToList()[0].fCartID,
                        fCustomer = cname,
                        fProductID = k.fProductID,
                        fPrice = k.fSIPrice,
                        fDate = DateTime.Now,
                        fisAlbum = isAlbum
                    });
                    }
                }
                db.SaveChanges();
            }
        }

        public void CartItemDel(int pID, int cID)
        {
            tPurchaseItem tpit = db.tPurchaseItems.Single(p => p.fPurchaseItemID == cID && p.fProductID == pID);
            db.tPurchaseItems.Remove(tpit);
            db.SaveChanges();
        }
        public string Dsc(string cname)
        {
            var q = from x in db.tMembers
                    where x.fAccount == cname
                    select x.fMoney.ToString();
            return q.ToList()[0];
        }

    }
}