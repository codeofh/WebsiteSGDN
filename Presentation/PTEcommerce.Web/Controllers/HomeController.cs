using Framework.EF;
using marketplace;
using Newtonsoft.Json;
using PTEcommerce.Web.Extensions;
using PTEcommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contacts()
        {
            return View();
        }
        public ActionResult Maintain()
        {
            return View();
        }
    }
}