using dbMusicStoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbMusicStoreTest.Controllers
{
    public class tProductsController : Controller
    {
        tProductsForm tpdf = new tProductsForm();
        // GET: tProducts
        public ActionResult Index()
        {
            return View(tpdf.getAllProducts());
        }
    }
}