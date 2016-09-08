using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gLibrary.Models;
using gLibrary.DAL;

namespace gMnt.Controllers
{
    //[Authorize]
    public class CategoryController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        //
        // GET: /Category/

        public ActionResult Index(int rid)
        {
            ViewBag.RestaurantId = rid;

            return View(_unitOfWork.GetRepository<Category>().Get(c => c.RestaurantId == rid));
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

        public ActionResult Create(int rid)
        {
            //SetRestaurantViewBag(gMnts.NoSelection);
            //ViewBag.RestaurantId = rid;
            var category = new Category();
            category.RestaurantId = rid;

            return View(category);
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

                return RedirectToAction("Index", new { rid = category.RestaurantId });
            }
            catch
            {
                return View(category);
            }
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id)
        {
            var category = _unitOfWork.GetRepository <Category>().GetByID(id);

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

                return RedirectToAction("Index", new { rid = category.RestaurantId });
            }
            catch
            {
                //SetRestaurantViewBag(category.RestaurantId);

                return View(category);
            }
        }

        //
        // GET: /Category/Delete/5
        
        public ActionResult Delete(int id)
        {
            var category = _unitOfWork.GetRepository <Category>().GetByID(id);

            if (category == null)
                return RedirectToAction("Index", new { rid = category.RestaurantId });

            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //reserve the restaurant id
            var category = _unitOfWork.GetRepository<Category>().GetByID(id);

            try
            {
                // TODO: Add delete logic here
                _unitOfWork.GetRepository<Category>().Delete(id);
                _unitOfWork.Save();

                return RedirectToAction("Index", new { rid = category.RestaurantId });
            }
            catch
            {
                return View(category);
            }
        }
        

        /* private void SetRestaurantViewBag(object selectedRestaurant)
        {
            ViewBag.Restaurant = new SelectList(_unitOfWork.GetRepository<Restaurant>().Get(), "Id", "Bilingual.FullName", selectedRestaurant);
        } */
    }
}
