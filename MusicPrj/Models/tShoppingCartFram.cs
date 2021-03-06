﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class tShoppingCartFram
    {
        dbProjectMusicStoreEntities1 db = new dbProjectMusicStoreEntities1();
        CStatistic Statistic = new CStatistic();
        //購物車清單
        public List<tPurchaseItem> getShoppingCart(string cname)
        {
            var q = from x in db.tPurchaseItems
                    where x.fCustomer == cname && x.tShoppingCart.fType == 0
                    select x;

            return q.ToList();
        }
        //加入購物車
        public string shopcartadd(string cname, string pdid)
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
                return cAdd(cname, q.ToList(), pd.ToList(), 0);

            }
            else
            {
                item = Int32.Parse(pdid);
                var pd = from x in db.tProducts
                         where x.fAlbumID == item
                         select x;
                return cAdd(cname, q.ToList(), pd.ToList(), 1);
            }



        }


        //更改購物車是否已購買
        public string CarType(int shopcarid, int dis)

        {
            var q = from x in db.tPurchaseItems
                    where x.fPurchaseItemID == shopcarid && x.tShoppingCart.fType == 0
                    select x;
            //foreach (var x in q)
            //{
            //    x.fDiscount = x.tProduct.tAlbum.fDiscount;
            //};
            
            var mob = q.ToList();
            for (int i = 0; i < mob.Count; i++)
            {
                mob[i].fDiscount = mob[i].tProduct.tAlbum.fDiscount;
                Statistic.userAddBuy(mob[i].fCustomer, mob[i].fProductID, mob[i].fisAlbum);
            }

            int a = Convert.ToInt32(q.First().tMember.fMoney);
            q.First().tMember.fMoney = a - dis;
            q.First().tShoppingCart.fType = 1;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "suces";
        }
        //購買包月狀態更新
        internal void BMounth(string account)
        {
            var q = db.tMembers.First(p => p.fAccount == account);
            if (q.fSubscriptStartDate == null)
            {
                q.fSubscriptStartDate = DateTime.Now;
                q.fSubscriptEndDate = DateTime.Now.AddDays(30);
                q.fPrivilege = 1;
            }
            else if (q.fSubscriptEndDate.Value.CompareTo(DateTime.Now) > 0)
            {
                var x = q.fSubscriptEndDate;
                q.fSubscriptEndDate = x.Value.AddDays(30);
            }
            else if (q.fSubscriptEndDate.Value.CompareTo(DateTime.Now) <= 0)
            {
                q.fSubscriptEndDate = DateTime.Now.AddDays(30);
            }
            db.SaveChanges();
        }
        //延續加入購物車的部分
        private string cAdd(string cname, List<tShoppingCart> q, List<tProduct> pd, int isAlbum)
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
                    decimal? money = 0;
                    if (isAlbum == 0)
                    {
                        if (db.tPurchaseItems.Where(p => p.fCustomer == cname).All(p => p.fProductID != k.fProductID))
                        {

                            money = k.fSIPrice;
                            db.tPurchaseItems.Add(
                            new tPurchaseItem
                            {
                                fPurchaseItemID = dts.fCartID,
                                fCustomer = cname,
                                fProductID = k.fProductID,
                                fPrice = money,
                                fDate = DateTime.Now,
                                fisAlbum = isAlbum
                            });
                        }
                        else
                        {
                            return "此首歌曲已經有囉~無法加入購物車";
                        }
                    }
                    else
                    {

                        if (db.tPurchaseItems.Where(p => p.fCustomer == cname).All(p => p.fProductID != k.fProductID))
                        {

                            money = k.tAlbum.fALPrice;
                            db.tPurchaseItems.Add(
                            new tPurchaseItem
                            {
                                fPurchaseItemID = dts.fCartID,
                                fCustomer = cname,
                                fProductID = k.fProductID,
                                fPrice = money,
                                fDate = DateTime.Now,
                                fisAlbum = isAlbum
                            });
                        }
                        else
                        {
                            var tpi = db.tPurchaseItems.Where(p => p.fCustomer == cname && p.fProductID == k.fProductID);
                            foreach (var x in tpi)
                            {
                                x.fPurchaseItemID = dts.fCartID;
                                x.fCustomer = cname;
                                x.fProductID = k.fProductID;
                                x.fPrice = money;
                                x.fDate = DateTime.Now;
                                x.fisAlbum = isAlbum;
                            }
                        }
                    }
                }
                db.SaveChanges();
                return "加入購物車成功";
            }
            else
            {
                foreach (var k in pd)
                {
                    decimal? money = 0;
                    if (isAlbum == 0)
                    {
                        money = k.fSIPrice;
                        if (db.tPurchaseItems.Where(p => p.fCustomer == cname).All(p => p.fProductID != k.fProductID))
                        {


                            db.tPurchaseItems.Add(
                            new tPurchaseItem
                            {
                                fPurchaseItemID = q.ToList()[0].fCartID,
                                fCustomer = cname,
                                fProductID = k.fProductID,
                                fPrice = money,
                                fDate = DateTime.Now,
                                fisAlbum = isAlbum
                            });
                        }
                        else
                        {
                            return "此首歌曲已經有囉~無法加入購物車";
                        }
                    }
                    else
                    {
                        money = k.tAlbum.fALPrice;

                        if (db.tPurchaseItems.Where(p => p.fCustomer == cname).All(p => p.fProductID != k.fProductID))
                        {


                            db.tPurchaseItems.Add(
                            new tPurchaseItem
                            {
                                fPurchaseItemID = q.ToList()[0].fCartID,
                                fCustomer = cname,
                                fProductID = k.fProductID,
                                fPrice = money,
                                fDate = DateTime.Now,
                                fisAlbum = isAlbum
                            });
                        }
                        else
                        {
                            var tpi = db.tPurchaseItems.Where(p => p.fCustomer == cname && p.fProductID == k.fProductID);
                            foreach (var x in tpi)
                            {
                                x.fPurchaseItemID = q.ToList()[0].fCartID;
                                x.fCustomer = cname;
                                x.fProductID = k.fProductID;
                                x.fPrice = money;
                                x.fDate = DateTime.Now;
                                x.fisAlbum = isAlbum;
                            }
                        }
                    }
                }
                db.SaveChanges();
                return "加入購物車成功";
            }
        }
        //刪除購物車資料
        public void CartItemDel(int pID, int cID)
        {
            if (db.tPurchaseItems.Where(p => p.fPurchaseItemID == cID && p.fProductID == pID).Select(p => p.fisAlbum).ToList()[0] == 0)
            {
                tPurchaseItem tpit = db.tPurchaseItems.Single(p => p.fPurchaseItemID == cID && p.fProductID == pID);
                db.tPurchaseItems.Remove(tpit);
            }
            else
            {
                var mon = from x in db.tPurchaseItems
                          where x.fPurchaseItemID == cID && x.fProductID == pID
                          select x.tProduct.fAlbumID;
                var a = mon.ToList()[0].Value;
                var tpit = from x in db.tPurchaseItems
                           where x.fPurchaseItemID == cID && x.tProduct.fAlbumID == a
                           select x;

                var p = tpit.ToList();
                for (int i = 0; i < p.Count; i++)
                {
                    db.tPurchaseItems.Remove(p[i]);
                }
            }

            db.SaveChanges();
        }
        //抓取會員積分
        public string Dsc(string cname)
        {
            var q = from x in db.tMembers
                    where x.fAccount == cname
                    select x.fMoney.ToString();
            return q.ToList()[0];
        }
        //訂單資料
        public List<tPurchaseItem> CCheck(int cartid)
        {
            var q = from x in db.tPurchaseItems
                    where x.fPurchaseItemID == cartid
                    select x;
            return q.ToList();
        }
    }
}