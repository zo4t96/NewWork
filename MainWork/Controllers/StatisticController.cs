using MainWork;
using MainWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MusicPrj.Controllers
{
    public class StatisticController : Controller
    {

        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        CStatistic Statistic = new CStatistic();
        //該會員近6個月試聽總數
        public ActionResult getStatistic_tryNum()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGettryNum(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_tryNum()
        {
            return Json(Statistic.adminGettryNum(), JsonRequestBehavior.AllowGet);
        }

        //該會員近6個月專輯/單曲被購買總數
        public ActionResult getStatistic_purchaseNum()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetpurchaseNum(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_purchaseNum()
        {
            return Json(Statistic.adminGetpurchaseNum(), JsonRequestBehavior.AllowGet);
        }

        //該會員試聽購買占比
        public ActionResult getStatistic_purchaseTry()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetpurchaseTry(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_purchaseTry()
        {
            return Json(Statistic.adminGetpurchaseTry(), JsonRequestBehavior.AllowGet);
        }

        //該會員被試聽週間變化
        public ActionResult getStatistic_tryWeek()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGettryWeek(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_tryWeek()
        {
            return Json(Statistic.adminGettryWeek(), JsonRequestBehavior.AllowGet);
        }

        //該會員賣最好5首單曲
        public ActionResult getStatistic_memberTop5Song()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetmemberTop5Song(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_memberTop5Song()
        {
            return Json(Statistic.adminGetmemberTop5Song(), JsonRequestBehavior.AllowGet);
        }

        //該會員賣最好5首專輯
        public ActionResult getStatistic_memberTop5Album()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetmemberTop5Album(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_memberTop5Album()
        {
            return Json(Statistic.adminGetmemberTop5Album(), JsonRequestBehavior.AllowGet);
        }

        //該會員本周最多試聽5首單曲
        public ActionResult getStatistic_memberTryWeekTop5Song()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetmemberTryWeekTop5Song(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_memberTryWeekTop5Song()
        {
            return Json(Statistic.adminGetmemberTryWeekTop5Song(), JsonRequestBehavior.AllowGet);
        }

        //該會員本月最多試聽5首單曲
        public ActionResult getStatistic_memberTryMonthTop5Song()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetmemberTryMonthTop5Song(s1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdminStatistic_memberTryMonthTop5Song()
        {
            return Json(Statistic.adminGetmemberTryMonthTop5Song(), JsonRequestBehavior.AllowGet);
        }

        //該會員單曲
        public ActionResult getStatistic_memberSongListTable()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetmemberSongListTable(s1), JsonRequestBehavior.AllowGet);
        }

        //該會員單曲搜尋
        public ActionResult myProductSearch(string product)
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(Statistic.userGetmyProductSearch(s1, product), JsonRequestBehavior.AllowGet);
        }

        //該會員單曲搜尋提示字
        public ActionResult findProd(string term = "")
        {
            var getresult = db.tProducts.Where(p => p.fProductName.Contains(term)).Select(p => p.fProductName);
            return Json(getresult, JsonRequestBehavior.AllowGet);
        }
    }

}