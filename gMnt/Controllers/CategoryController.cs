using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gLibrary.Models;
using gLibrary.DAL;

namespace gMnt.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View(_unitOfWork.GetRepository<Category>().Get());
        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id)
        {
            var category = _unitOfWork.GetRepository <Category>().GetByID(id);

            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            SetRestaurantViewBag(gMnts.NoSelection);

            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                // TODO: Add insert logic here
                _unitOfWork.GetRepository <Category>().Insert(category);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                SetRestaurantViewBag(category.RestaurantId);

                return View(category);
            }
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id)
        {
            var category = _unitOfWork.GetRepository <Category>().GetByID(id);
            SetRestaurantViewBag(category.RestaurantId);

            return View(category);
        }

        
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            try
            {
                // TODO: Add update logic here
                _unitOfWork.GetRepository <Category>().Update(category);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                SetRestaurantViewBag(category.RestaurantId);

                return View(category);
            }
        }

        //
        // GET: /Category/Delete/5
        
        public ActionResult Delete(int id)
        {
            var category = _unitOfWork.GetRepository <Category>().GetByID(id);
            if (category == null)
                return RedirectToAction("Index");

            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _unitOfWork.GetRepository<Category>().Delete(id);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        

        private void SetRestaurantViewBag(object selectedRestaurant)
        {
            ViewBag.Restaurant = new SelectList(_unitOfWork.GetRepository<Restaurant>().Get(), "Id", "Bilingual.FullName", selectedRestaurant);
        }
    }
}
