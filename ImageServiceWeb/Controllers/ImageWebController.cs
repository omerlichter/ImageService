using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageServiceWeb.Models;
using ImageService.Infrastructure.Communication;

namespace ImageServiceWeb.Controllers
{
    public class ImageWebController : Controller
    {
        static ModelImageWeb model = new ModelImageWeb();

        public ImageWebController()
        {
            model.Update -= Update;
            model.Update += Update;   
        }

        private void Update(Object sender, EventArgs args)
        {
            ImageWeb();
        }

        // GET: ImageWeb
        public ActionResult ImageWeb()
        {
            model.CheckUpdate();
            return View(model);
        }
    }
}