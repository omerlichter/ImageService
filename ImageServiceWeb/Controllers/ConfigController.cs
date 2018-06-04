using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageServiceWeb.Models;
using ImageService.Infrastructure.Communication;

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
    }
}