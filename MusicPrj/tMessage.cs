//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicPrj
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tMessage
    {
        public int fMessageId { get; set; }
        [Display(Name = "寄件人")]
        public string fAccountFrom { get; set; }
        [Display(Name = "收件人")]
        public string fAccountTo { get; set; }
        [Display(Name = "內文")]
        public string fContent { get; set; }
        public Nullable<System.DateTime> fTime { get; set; }
        public Nullable<int> fStatus { get; set; }
<<<<<<< HEAD:MusicPrj/tMessage.cs
        [Display(Name = "主旨")]
=======
>>>>>>> 3f8fc7c0f3f2b90fde00f2155b735ad35c7fe8ae:MainWork/tMessage.cs
        public string fTitle { get; set; }
        public Nullable<int> fReaded { get; set; }
    
        public virtual tMember tMember { get; set; }
        public virtual tMember tMember1 { get; set; }
    }
}
