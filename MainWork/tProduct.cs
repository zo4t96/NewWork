
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
    
public partial class tProduct
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tProduct()
    {

        this.tShoppingCarts = new HashSet<tShoppingCart>();

    }


    public int fProductID { get; set; }

    public Nullable<int> fAlbumID { get; set; }

    public string fProductName { get; set; }

    public string fArtist { get; set; }

    public Nullable<decimal> fSIPrice { get; set; }

    public string fComposer { get; set; }

    public string fArranger { get; set; }

    public string fLyricist { get; set; }

    public string fKind { get; set; }

    public byte[] fMusic { get; set; }

    public string fFilePath { get; set; }

    public Nullable<System.TimeSpan> fPlayStart { get; set; }

    public Nullable<System.TimeSpan> fPlayEnd { get; set; }

    public Nullable<int> fStatus { get; set; }

    public Nullable<int> fDownloadCount { get; set; }

    public string fModifiedDate { get; set; }

    public string fNation { get; set; }



    public virtual tAlbum tAlbum { get; set; }

    public virtual tPurchaseItem tPurchaseItem { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tShoppingCart> tShoppingCarts { get; set; }

}

}
