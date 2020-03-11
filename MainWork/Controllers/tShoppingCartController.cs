using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainWork.Models;

namespace MusicPrj.Controllers
{
    public class tShoppingCartController : Controller
    {
        // GET: tShoppingCart
        tShoppingCartFram tCart = new tShoppingCartFram();
        //購物車頁面

        public ActionResult ShoppingCart(string cname, bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }

            return View(tCart.getShoppingCart(cname));


        }
        //加入購物車

        public ActionResult AddCart(string cname, string pdid)
        {
            return JavaScript($"alert('{tCart.shopcartadd(cname, pdid)}');");
            //return Content("已加入購物車");

        }
        //購物車資料刪除
        [HttpPost]
        public ActionResult DelCart(int pID, int cID)
        {
            tCart.CartItemDel(pID, cID);
            return Content("已刪除該筆訂單");
        }
        //購買專輯確認付款後相關狀態更新
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult BuyCheck(int shopcartid, int dis, bool ajax = false)
        {
            tCart.CarType(shopcartid, dis);
            return RedirectToAction("CheckCart", "tShoppingCart", new { cartid = shopcartid, ajax });
        }
        //確認剩餘積分
        [HttpPost]
        public ActionResult CheckDis(string cname)
        {
            return Content(tCart.Dsc(cname));
        }
        //購買專輯確認付款後導向的訂單頁面或是要倒其他頁?
        [HttpPost]
        public ActionResult CheckCart(int cartid, bool ajax = false)
        {

            ViewBag.ajax = ajax;

            return View(tCart.CCheck(cartid));
        }
        //購買包月付款後狀態更新
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult BuyMonth(string account)
        {
            tCart.BMounth(account);
            return View();
        }
    }
}