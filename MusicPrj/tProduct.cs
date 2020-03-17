//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MainWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tProduct()
        {
            this.tPlayLists = new HashSet<tPlayList>();
            this.tPurchaseItems = new HashSet<tPurchaseItem>();
            this.tStatistics = new HashSet<tStatistic>();
        }
    
        public int fProductID { get; set; }
        public Nullable<int> fAlbumID { get; set; }
        [Display(Name = "單曲名稱")]
        public string fProductName { get; set; }
        [Display(Name = "歌手")]
        public string fSinger { get; set; }
        [Display(Name = "售價")]
        public Nullable<decimal> fSIPrice { get; set; }
        [Display(Name = "作曲")]
        public string fComposer { get; set; }
        [Display(Name = "音樂檔(限mp3)")]
        public string fFilePath { get; set; }
        public Nullable<double> fPlayStart { get; set; }
        public Nullable<double> fPlayEnd { get; set; }
    
        public virtual tAlbum tAlbum { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPlayList> tPlayLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPurchaseItem> tPurchaseItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tStatistic> tStatistics { get; set; }
    }
}
