using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = new OnlineShopDbContext();

        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        /// <summary>
        /// List Feature Product
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        /// <summary>
        /// Get list Product by Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Product> ListByCategoryId(long categoryId, ref int totalRecord, int page = 11, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryId).Count();
            var model = db.Products.Where(x => x.CategoryID == categoryId)
                .OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        /// <summary>
        /// Get List Related Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="produt"></param>
        /// <returns></returns>
        public List<Product> ListRelatedProduct(long productId, int product)
        {
            var products = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == products.CategoryID).Take(product).ToList();
        }

        public List<Product> ListAllProduct()
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).ToList();
        }

     
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        /// <summary>
        /// Get list name Keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int page = 11, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keyword)).Count();
            var model = (from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.Name.Contains(keyword)
                        select new
                        {
                            CateMetaTitle = b.MetaTitle,
                            CateName = b.Name,
                            CreatedDate = a.CreatedDate,
                            ID = a.ID,
                            Images = a.Images,
                            Name = a.Name,
                            MetaTitle = a.MetaTile,
                            Price = a.Price,
                            PromotionPrice = a.PromotionPrice
                        }).AsEnumerable().Select(x=>new ProductViewModel()
                        {
                            CateMetaTitle = x.MetaTitle,
                            CateName = x.Name,
                            CreatedDate = x.CreatedDate,
                            ID = x.ID,
                            Images = x.Images,
                            Name = x.Name,
                            MetaTitle = x.MetaTitle,
                            Price = x.Price,
                            PromotionPrice = x.PromotionPrice
                        });
            model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }
    }
}
