using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ImageServiceWeb.Models;

namespace ImageServiceWeb.Controllers
{
    public class PhotosController : Controller
    {
        private static ModelPhotos model = new ModelPhotos();

        public PhotosController()
        {
            model.Update -= Update;
            model.Update += Update;
        }

        private void Update(Object sender, EventArgs args)
        {
            Photos();
        }

        // GET: Photos
        public ActionResult Photos()
        {
            model.CheckUpdate();
            return View(model.Photos);
        }

        // GET: Photos
        public ActionResult ViewPhoto(string photoPath)
        {
            return View(model.GetPhotoFromPath(photoPath));
        }

        // GET: Photos
        public ActionResult DeletePhoto(string photoPath)
        {
            model.LastPhoto = photoPath;
            ViewBag.photoPath = Path.GetFileName(photoPath);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePhotoOK()
        {
            model.DeletePhoto();
            return RedirectToAction("Photos");
        }
    }
}