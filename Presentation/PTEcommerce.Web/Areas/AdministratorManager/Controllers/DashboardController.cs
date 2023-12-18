using Framework.EF;
using Framework.Helper.Session;
using marketplace;
using Microsoft.AspNet.SignalR;
using PTEcommerce.Business;
using PTEcommerce.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Areas.AdministratorManager.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }


    }
}