using Framework.EF;
using marketplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Areas.AdministratorManager.Controllers
{
    public class OrderPakagesController : BaseController
    {
        private IOrderPakages orderPakages;
        private IAccountCustomer accountCustomer;
        public OrderPakagesController()
        {
            orderPakages = SingletonIpl.GetInstance<IplOrderPakages>();
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
        }
        // GET: AdministratorManager/OrderPakages
        public ActionResult Index()
        {
            ViewBag.ListAccount = accountCustomer.GetAll();
            return View();
        }
        public JsonResult ListAllPagging(int idAccount, int status, string startDate, string endDate)
        {
            int totalRow = 0;
            var p = GetPagingMessage(Request.QueryString);
            int dateStart = int.Parse(startDate.Replace("-", ""));
            int dateEnd = int.Parse(endDate.Replace("-", ""));
            var data = orderPakages.ListAllPagging(idAccount, status, dateStart, dateEnd, p.PageIndex, p.PageSize);
            if (data != null && data.Count > 0)
            {
                totalRow = data[0].TotalRow;
            }
            return Json(new
            {
                rows = data,
                total = totalRow
            }, JsonRequestBehavior.AllowGet);
        }
    }
}