using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageServiceWeb.Models;

namespace ImageServiceWeb.Controllers
{
    public class LogsController : Controller
    {
        static ModelLogs model = new ModelLogs();

        public LogsController()
        {
            model.Update -= Update;
            model.Update += Update;
        }

        private void Update(Object sender, EventArgs args)
        {
            Logs();
        }

        // GET: Logs
        public ActionResult Logs()
        {
            ViewBag.filter = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Logs(FormCollection form)
        {
            string logType = form["typeFilter"].ToString();
            if (logType == null)
            {
                ViewBag.filter = "";
            } else
            {
                ViewBag.filter = logType;
            }
            return View(model);
        }
    }
}