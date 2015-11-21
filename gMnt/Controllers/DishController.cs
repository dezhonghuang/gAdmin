using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using gLibrary.Models;
using gLibrary.DAL;

namespace gMnt.Controllers
{
    [Authorize]
    public class DishController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();
        //
        // GET: /Dish/

        public ActionResult Index()
        {
            var dishes = _unitOfWork.GetRepository<Dish>().Get(includeProperties: "Category").OrderBy(d => d.CategoryId);

            return View(dishes);
        }

        //
        // GET: /Dish/Details/5

        public ActionResult Details(int id)
        {
            var dish = _unitOfWork.GetRepository<Dish>().GetByID(id);

            return View(dish);
        }

        //
        // GET: /Dish/Create

        public ActionResult Create()
        {
            SetCategories(-1, gMnts.NoSelection);
            SetRestaurants();

            return View();
        }

        //
        // POST: /Dish/Create

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, Dish dish)
        {
            try
            {
                //Upload the image file if it's not null
                if ((file != null) && (file.ContentLength > 0))
                {
                    dish.UploadImage(file, Server.MapPath(dish.ImageFolder));
                }

                // TODO: Add insert logic here
                _unitOfWork.GetRepository<Dish>().Insert(dish);
                _unitOfWork.Save();

                return RedirectToAction("Details", new { Id = dish.Id });
            }
            catch
            {
                SetCategories(-1, dish.CategoryId);
                SetRestaurants(dish.RestaurantId);

                return View(dish);
            }
        }

        //
        // GET: /Dish/Edit/5

        public ActionResult Edit(int id)
        {
            var dish = _unitOfWork.GetRepository<Dish>().GetByID(id);

            if (dish == null)
                RedirectToAction("Index");

            SetCategories(dish.RestaurantId, dish.CategoryId);

            return View(dish);
        }

        //
        // POST: /Dish/Edit/5

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, Dish dish)
        {
            try
            {
                // TODO: Add update logic here

                //Upload the image file if it's not null
                if ((file != null) && (file.ContentLength > 0))
                {
                    dish.UploadImage(file, Server.MapPath(dish.ImageFolder));
                }

                //update dish entity
                _unitOfWork.GetRepository<Dish>().Update(dish);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                SetCategories(dish.RestaurantId, dish.CategoryId);

                return View(dish);
            }
        }

        //
        // GET: /Dish/Delete/5
        
        public ActionResult Delete(int id)
        {
            var dish = _unitOfWork.GetRepository<Dish>().GetByID(id);

            if (dish == null)
                return RedirectToAction("Index");

            return View(dish);
        }
        
        //
        // POST: /Dish/Delete/5

        [HttpPost]
        public ActionResult Delete(Dish dish, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _unitOfWork.GetRepository<Dish>().Delete(dish.Id);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(dish);
            }
        }
        
        public JsonResult CategoryList(int rid)
        {
            var categories = from c in _unitOfWork.GetRepository<Category>().Get(c => c.RestaurantId == rid)
                             select new
                             {
                                 Text = c.Name,
                                 Value = c.Id.ToString()
                             };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        private void SetCategories(int rid, object selectedCategory = null)
        {
            ViewBag.Category = new SelectList(GetCategories(rid), "Id", "Bilingual.Name", selectedCategory);
        }

        private void SetRestaurants(object selectedRestaurant = null)
        {
            ViewBag.RestaurantId = new SelectList(_unitOfWork.GetRepository<Restaurant>().Get(), "Id", "Bilingual.Name", selectedRestaurant);
        }

        private IEnumerable<Category> GetCategories(int rid)
        {
            var categories = from c in _unitOfWork.GetRepository<Category>().Get(c => c.RestaurantId == rid)
                             select c;

            return categories;
        }
    }
}
