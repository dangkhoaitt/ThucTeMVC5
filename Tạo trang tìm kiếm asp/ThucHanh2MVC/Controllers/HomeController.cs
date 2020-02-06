using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanh2MVC.Common;
using ThucHanh2MVC.Models;
using Model.EF;


namespace ThucHanh2MVC.Controllers
{
    public class HomeController : Controller
    {
        
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var productDao = new ProductDao();
            ViewBag.NewProducts = productDao.ListNewProduct(6);
            ViewBag.FeatureProducts = productDao.ListFeatureProduct(4);
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 60 * 60)]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            return PartialView(model);
        }

    

        [ChildActionOnly]
        [OutputCache(Duration = 60 * 60)]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupId(2);       
           
            return PartialView(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 60 * 60)]
        public ActionResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonStatic.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}