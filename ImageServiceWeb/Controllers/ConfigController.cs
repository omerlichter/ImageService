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
        public ConfigController()
        {
            ModelConfig.Instance.Update -= Update;
            ModelConfig.Instance.Update += Update;
        }

        private void Update(Object sender, EventArgs args)
        {
            Config();
        }

        // GET: Config
        public ActionResult Config()
        {
            return View(ModelConfig.Instance);
        }

        // GET: Config/DeleteHandler/
        public ActionResult DeleteHandler(string handler)
        {
            ModelConfig.Instance.LastHandler = handler;
            return RedirectToAction("Confirm");
        }

        [HttpPost]
        public ActionResult DeleteHandlerOK(string handler)
        {
            ModelConfig.Instance.DeleteHandler();
            return RedirectToAction("Config");
        }

        // GET: Confirm
        public ActionResult Confirm()
        {
            return View(ModelConfig.Instance);
        }
    }
}