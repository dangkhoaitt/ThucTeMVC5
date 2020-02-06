using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanh2MVC.Models;
using ThucHanh2MVC.Common;
using System.Web.Script.Serialization;
using Model.EF;
using System.Configuration;
using Common;


namespace ThucHanh2MVC.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CommonStatic.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new ProductDao().ViewDetail(productId);
            var cart = Session[CommonStatic.CartSession];
            //decimal total = 0;        
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //Tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
               // total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                //gán vào session
                Session[CommonStatic.CartSession] = list;

            }
            else
            {
                //Tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();

                list.Add(item);

                //gán vào session
                Session[CommonStatic.CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonStatic.CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }

            Session[CommonStatic.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonStatic.CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CommonStatic.CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CommonStatic.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }


        public ActionResult Payment()
        {
            var cart = Session[CommonStatic.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;

            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CommonStatic.CartSession];
                var detailDao = new OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    if (item.Product.PromotionPrice == null)
                    {
                        orderDetail.Price = item.Product.Price;
                        total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                    }
                    else
                    {
                        orderDetail.Price = item.Product.PromotionPrice;
                        total += (item.Product.PromotionPrice.GetValueOrDefault(0) * item.Quantity);
                    }
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);

                    //total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                    ViewBag.total = total;
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();


                new MailHelper().SendMail(email, "Đơn hàng mới từ Eshop", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Eshop", content);
            }
            catch (Exception)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            Session.Abandon();
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}