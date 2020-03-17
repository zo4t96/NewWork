using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Models
{
    public class CMessage
    {
        private dbProjectMusicStoreEntities1 db = new dbProjectMusicStoreEntities1();
        //使用者寄信
        public string userSendMail(string senderName, FormCollection formCollection)
        {
            string s2 = "";
            if (db.tMembers.FirstOrDefault(p => p.fAccount == senderName) == null)
            {
                s2 = "失敗,無此寄件人";
                return s2;
            }
            string fAccountTo = formCollection["tMes.fAccountTo"].Trim();
            fAccountTo = fAccountTo.Replace(" ", "");
            //    string fAccountToOther = formCollection["tMes.fAccountToOther"].Trim();
            //    fAccountToOther = fAccountToOther.Replace(" ", "");
            //if (db.tMembers.FirstOrDefault(p => p.fAccount == fAccountTo) == null)
            //{
            //    s2 = "失敗,無此收件人";
            //    return s2;
            //}
            string[] singelAccounts = { };
            if (!string.IsNullOrWhiteSpace(fAccountTo))
            {
                char[] c1 = { ';' };
                singelAccounts = fAccountTo.Split(c1, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in singelAccounts)
                {
                    if (db.tMembers.FirstOrDefault(p => p.fAccount == s) == null)
                    {
                        s2 = "失敗,無此收件人";
                    }
                }
            }
            if (s2 == "失敗,無此收件人")
            {
                return s2;
            }
            //信件內容加入DB
            tMessage tMes = new tMessage();
            tMes.fAccountFrom = senderName;
            //      tMes.fAccountTo = fAccountTo;
            tMes.fTitle = formCollection["tMes.fTitle"];
            tMes.fContent = formCollection["tMes.fContent"];
            tMes.fTime = DateTime.Now;
            tMes.fStatus = 1;
            //多個收件人
            foreach (string s in singelAccounts)
            {
                tMessage tMesOther = tMes;
                tMesOther.fAccountTo = s;
                db.tMessages.Add(tMesOther);
                db.SaveChanges();
            }
            //寄件備份
            tMessage tMesCopy = tMes;
            tMesCopy.fStatus = 0;
            //db.tMessages.Add(tMes);
            db.tMessages.Add(tMesCopy);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                s2 = "資料庫異常,請通知管理員";
                //   return ex.Message.ToString();
                return s2;
            }
            //副本
            //if (singelAccounts != null)
            //{
            //    foreach (string s in singelAccounts)
            //    {
            //        tMessage tMesOther = tMes;
            //        tMesOther.fAccountTo = s;
            //        db.tMessages.Add(tMesOther);
            //        db.SaveChanges();
            //    }
            //}

            s2 = "成功";
            return s2;
        }

        //使用者刪信
        public string userDeleteMail(string issuerName, int mailid)
        {
            string s2 = "";
            if (db.tMembers.FirstOrDefault(p => p.fAccount == issuerName) == null)
            {
                s2 = "失敗,查無此人";
                return s2;
            }
            tMessage tMes = db.tMessages.FirstOrDefault(p => p.fMessageId == mailid);
            if (tMes == null)
            {
                s2 = "失敗,找不到此封信";
                return s2;
            }
            if (tMes.fAccountTo != issuerName && tMes.fAccountFrom != issuerName)
            {
                s2 = "失敗,你非這封信擁有者";
                return s2;
            }
            //信件內容自DB移除
            db.tMessages.Remove(tMes);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                s2 = "資料庫異常,請通知管理員";
                //   return ex.Message.ToString();
                return s2;
            }
            s2 = "成功";
            return s2;
        }

        //使用者刪所有信
        public string userDeleteAllMail(string issuerName)
        {
            string s2 = "";
            if (db.tMembers.FirstOrDefault(p => p.fAccount == issuerName) == null)
            {
                s2 = "失敗,查無此人";
                return s2;
            }
            List<tMessage> tMes = db.tMessages.Where(p => p.fAccountTo == issuerName && p.fStatus == 1).ToList();
            if (tMes == null)
            {
                s2 = "失敗,信箱內找不到信";
                return s2;
            }
            //信件自DB移除
            db.tMessages.RemoveRange(tMes);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                s2 = "資料庫異常,請通知管理員";
                //   return ex.Message.ToString();
                return s2;
            }
            s2 = "成功";
            return s2;
        }

        //使用者刪所有寄件備份
        public string userDeleteAllMailCopy(string issuerName)
        {
            string s2 = "";
            if (db.tMembers.FirstOrDefault(p => p.fAccount == issuerName) == null)
            {
                s2 = "失敗,查無此人";
                return s2;
            }
            List<tMessage> tMes = db.tMessages.Where(p => p.fAccountFrom == issuerName && p.fStatus == 0).ToList();
            if (tMes == null)
            {
                s2 = "失敗,信箱內找不到信";
                return s2;
            }
            //信件自DB移除
            db.tMessages.RemoveRange(tMes);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                s2 = "資料庫異常,請通知管理員";
                //   return ex.Message.ToString();
                return s2;
            }
            s2 = "成功";
            return s2;
        }
    }
}