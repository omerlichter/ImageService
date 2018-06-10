using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageServiceWeb.Models;
using ImageService.Infrastructure.Communication;
using Newtonsoft.Json.Linq;

namespace ImageServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        static ModelConfig model = new ModelConfig();

        public ConfigController()
        {
            model.Update -= Update;
            model.Update += Update;
        }

        private void Update(Object sender, EventArgs args)
        {
            Config();
        }

        // GET: Config
        public ActionResult Config()
        {
            return View(model);
        }

        // GET: Config/DeleteHandler/
        public ActionResult DeleteHandler(string handler)
        {
            model.LastHandler = handler;
            return RedirectToAction("Confirm");
        }

        [HttpPost]
        public ActionResult DeleteHandlerOK(string handler)
        {
            model.DeleteHandler();
            return RedirectToAction("Config");
        }

        // GET: Confirm
        public ActionResult Confirm()
        {
            return View(model);
        }
    }
}