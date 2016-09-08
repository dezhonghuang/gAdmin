using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MessagingToolkit.QRCode.Codec;
using gLibrary.DAL;
using gLibrary.Models;

namespace gMnt.Controllers
{
    //[Authorize]
    public class TableController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();
        //
        // GET: /Table/

        public ActionResult Index(int rid)
        {
            ViewBag.RestaurantId = rid;

            var tables = _unitOfWork.GetRepository<Table>().Get(t => t.RestaurantId == rid);

            return View(tables);
        }

        //
        // GET: /Table/Details/5

        public ActionResult Details(int id)
        {
            var table = _unitOfWork.GetRepository<Table>().GetByID(id);

            return View(table);
        }

        public ActionResult QRCode(int id)
        {
            var table = _unitOfWork.GetRepository<Table>().GetByID(id);

            return View("QRCode", "_QRLayout", table);  
        }

        //
        // GET: /Table/Details/5

        public ActionResult GenerateQRCode(int tid, int rid)
        {

            QRCodeEncoder encoder = new QRCodeEncoder();

            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            encoder.QRCodeScale = 5;

            string tidString = Encryption.Encrypt(tid.ToString(), gMnts.TripleDesKeyPrefix).ToString();
            //string ridString = Encryption.Encrypt(rid.ToString(), gMnts.TripleDesKeyPrefix + rid.ToString());

            //QR code = "http://www.gmenu.co.nz/Home/Order/rid?tid=xxx"
            Bitmap img = encoder.Encode(gMnts.QRCode + rid.ToString() + "?tid=" + tidString);

            //Image logo = Image.FromFile(Server.MapPath(gMnts.ImageFolder + gMnts.RestaurantFolder + rid.ToString() + gMnts.LogoFolder + "logo.png"));

            //draw logo in img
            //left point of logo

            /* using (Graphics g = Graphics.FromImage(img))
            {
                int left = (img.Width - logo.Width) / 2;
                int top = (img.Height - logo.Height) / 2;

                g.DrawImage(logo, new Point(left, top));
            } */

            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }

            return File(byteArray, "image/png");
        }

        //
        // GET: /Table/Create

        public ActionResult Create(int rid)
        {
            var table = new Table();
            table.RestaurantId = rid;

            return View(table);
        }

        //
        // POST: /Table/Create

        [HttpPost]
        public ActionResult Create(Table table)
        {
            try
            {
                _unitOfWork.GetRepository<Table>().Insert(table);
                _unitOfWork.Save();

                return RedirectToAction("Index", new { rid = table.RestaurantId });
            }
            catch
            {
                return View(table);
            }
        }

        //
        // GET: /Table/Edit/5

        public ActionResult Edit(int id)
        {
            var table = _unitOfWork.GetRepository<Table>().GetByID(id);

            return View(table);
        }

        //
        // POST: /Table/Edit/5

        [HttpPost]
        public ActionResult Edit(Table table)
        {
            try
            {
                _unitOfWork.GetRepository<Table>().Update(table);
                _unitOfWork.Save();

                return RedirectToAction("Index", new { rid = table.RestaurantId });
            }
            catch
            {
                return View(table);
            }
        }

        //
        // GET: /Table/Delete/5

        public ActionResult Delete(int id)
        {
            var table = _unitOfWork.GetRepository<Table>().GetByID(id);
            if (table == null)
                return RedirectToAction("Index", new { rid = table.RestaurantId });

            return View(table);
        }

        //
        // POST: /Table/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // save a copy of the table variable
            var table = _unitOfWork.GetRepository<Table>().GetByID(id);

            try
            {

                // TODO: Add delete logic here
                _unitOfWork.GetRepository<Table>().Delete(id);
                _unitOfWork.Save();

                return RedirectToAction("Index", new { rid = table.RestaurantId});
            }
            catch
            {
                return View(table);
            }
        }
    }
}
