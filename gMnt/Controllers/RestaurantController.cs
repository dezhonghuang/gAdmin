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
using gLibrary.ViewModels;
using gLibrary.DAL;
using MessagingToolkit.QRCode.Codec;

namespace gMnt.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();
        //
        // GET: /Restaurant/

        public ActionResult Index()
        {
            var restaurants = _unitOfWork.GetRepository<Restaurant>().Get().OrderBy(r => r.Bilingual.Name);

            return View(restaurants);
        }

        //
        // GET: /Restaurant/Details/5

        public ActionResult Details(int id)
        {
            var restaurant = _unitOfWork.GetRepository<Restaurant>().Get(r => r.Id == id, includeProperties: "Address").SingleOrDefault();

            return View(restaurant);
        }

        //
        // GET: /Restaurant/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Restaurant/Create

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file,  Restaurant restaurant)
        {
            try
            {
                //restaurant logo
                if (file != null && file.ContentLength > 0)
                {
                    restaurant.UploadLogo(file, Server.MapPath(restaurant.LogoFolder));
                }
                
                ////restaurant address
                if (restaurant.Address.StreetNumber != null && restaurant.Address.StreetId > 0)
                {
                    _unitOfWork.GetRepository<Address>().Insert(restaurant.Address);
                    _unitOfWork.Save();

                    //link restaurant address with newly created address
                    restaurant.AddressId = restaurant.Address.Id;
                }
                else
                {
                    restaurant.Address = null;
                }
                
                //restaurant
                _unitOfWork.GetRepository<Restaurant>().Insert(restaurant);
                _unitOfWork.Save();

                return RedirectToAction("Details", new { id = restaurant.Id });
            }
            catch
            {
                return View(restaurant);
            }
        }

        //
        // GET: /Restaurant/Edit/5

        public ActionResult Edit(int id)
        {
            //Address address;

            //var restaurant = _unitOfWork.GetRepository<Restaurant>().GetByID(id);

            //if (restaurant.AddressId != null)
            //{
            //    address = _unitOfWork.GetRepository<Address>().GetByID(restaurant.AddressId);
            //}
            //else
            //{
            //    address = new Address();
            //}

            //var addressViewModel = new AddressViewModel()
            //{
            //    Restaurant = restaurant,
            //    Address = address
            //};

            var restaurant = _unitOfWork.GetRepository<Restaurant>().Get(r => r.Id == id, includeProperties: "Address").Single();

            return View(restaurant);
        }

        //
        // POST: /Restaurant/Edit/5

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, Restaurant restaurant)
        {
            int addressToDelete = 0;

            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    restaurant.UploadLogo(file, Server.MapPath(restaurant.LogoFolder));
                }

                ////restaurant address
                if (restaurant.Address.StreetNumber != null && restaurant.Address.StreetId > 0)
                {
                    //existing address
                    if (restaurant.AddressId != null)
                    {
                        _unitOfWork.GetRepository<Address>().Update(restaurant.Address);
                    }
                    else
                    { //new address
                        _unitOfWork.GetRepository<Address>().Insert(restaurant.Address);
                        _unitOfWork.Save();

                        //link restaurant address with newly created address
                        restaurant.AddressId = restaurant.Address.Id;
                    }
                }
                else
                {
                    //existing address 
                    if (restaurant.AddressId != null)
                    {
                        addressToDelete = (int)restaurant.AddressId;
                        restaurant.AddressId = null;
                    }

                    restaurant.Address = null;
                }

                //update restaurant
                _unitOfWork.GetRepository<Restaurant>().Update(restaurant);
                _unitOfWork.Save();

                //existing address 
                if (addressToDelete > 0)
                {
                    _unitOfWork.GetRepository<Address>().Delete(addressToDelete);
                }

                return RedirectToAction("Details", new { id = restaurant.Id });
            }
            catch
            {
                ModelState.AddModelError("", "Invalid restaurant address");

                return View(restaurant);
            }
        }

        //
        // GET: /Restaurant/Delete/5
        
        public ActionResult Delete(int id)
        {
            var restaurant = _unitOfWork.GetRepository<Restaurant>().GetByID(id);
            if (restaurant == null)
                return RedirectToAction("Index");

            return View(restaurant);
        }

        //
        // POST: /Restaurant/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _unitOfWork.GetRepository<Restaurant>().Delete(id);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        public JsonResult StreetList(string term)
        {
            term = term.Trim();
            string streetNumber = gMnts.StreetNumber;
            //remove the first string if it starts with digit
            if (Char.IsDigit(term[0]))
            {
                string[] termArray = term.Split(' ');
                streetNumber = termArray[0];
                if (termArray.Length > 1)
                    term = termArray[1];
                else
                    term = String.Empty;
            }

            var streets = from s in _unitOfWork.GetRepository<Street>().Get(s => s.Name.Contains(term))
                          .Take(5)
                          select new { id = s.Id, label = streetNumber + " " + s.Name + ", " + s.PostCode, streetNo = streetNumber };

            return Json(streets, JsonRequestBehavior.AllowGet);
        }

        private void PopulateDropDowns(object street = null, object suburb = null, object city = null, object state = null, object country = null)
        {
            ViewBag.StreetId = new SelectList(_unitOfWork.GetRepository<Street>().Get(), "Id", "Name", street);
            ViewBag.SuburbId = new SelectList(_unitOfWork.GetRepository<Suburb>().Get(), "Id", "Name", suburb);
            ViewBag.CityId = new SelectList(_unitOfWork.GetRepository<City>().Get(), "Id", "Name", city);
            ViewBag.StateId = new SelectList(_unitOfWork.GetRepository<State>().Get(), "Id", "Name", state);
            ViewBag.CountryId = new SelectList(_unitOfWork.GetRepository<Country>().Get(), "Id", "Name", country);
        }

        private void SetTableViewBag(object table = null)
        {
            ViewBag.Tid = new SelectList(_unitOfWork.GetRepository<Table>().Get(), "Id", "TableNumber", table);
        }
    }
}
