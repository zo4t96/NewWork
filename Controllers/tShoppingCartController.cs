using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dbMusicStoreTest.Models;

namespace dbMusicStoreTest.Controllers
{
    public class tShoppingCartController : Controller
    {
        // GET: tShoppingCart
        tShoppingCartFram tCart = new tShoppingCartFram();
        public ActionResult ShoppingCart(string cname = "aaa")
        {
            if(tCart.getShoppingCart(cname).Count != 0)
            {
                return View(tCart.getShoppingCart(cname));
            }else
            {
                return RedirectToAction("Index", "tProducts");
            }
                       
        }
        public ActionResult AddCart(string cname, string pdid)
        {
            
            tCart.shopcartadd(cname, pdid);
            return Content("已加入購物車");
            
        }
        public ActionResult DelCart(int pID,int cID)
        {
            tCart.CartItemDel(pID, cID);
            return Content("已刪除該筆訂單");
        }
        public ActionResult BuyCheck(int shopcarid)
        {
            tCart.CarType(shopcarid);
            return Content("test");
        }
    }
}