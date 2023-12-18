using Framework.EF;
using marketplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Controllers
{
    public class PartnersController : BaseController
    {
        private IAccountCustomer accountCustomer;
        private IOrderPakages orderPakages;
        private IHistoryTransfer historyTransfer;
        public PartnersController()
        {
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
            orderPakages = SingletonIpl.GetInstance<IplOrderPakages>();
            historyTransfer = SingletonIpl.GetInstance<IplHistoryTransfer>();
        }
        // GET: Partners
        public ActionResult Index()
        {
            ViewBag.Account = accountCustomer.GetById(memberSession.AccID);
            return View();
        }
        public JsonResult ReportPartner(string startdate, string enddate)
        {
            var dateS = int.Parse(startdate.Replace("-", ""));
            var dateE = int.Parse(enddate.Replace("-", ""));
            var listAccount = accountCustomer.GetListPartnerByIdAccount(memberSession.AccID);
            var report = historyTransfer.GetReport(memberSession.AccID, dateS, dateE);
            return Json(new
            {
                account = listAccount != null ? listAccount.Count : 0,
                prices = report.Prices,
                total = report.Total
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Referal()
        {
            return View();
        }
        public ActionResult GetListReferal()
        {
            var data = accountCustomer.GetListPartnerReportByIdAccount(memberSession.AccID);
            return View(data);
        }
        public ActionResult History()
        {
            return View();
        }
        public ActionResult CustomerTransfer()
        {
            return View();
        }
        public ActionResult CustomerTransfers()
        {
            return View(new List<AccountCustomer>());
        }
        public ActionResult CustomerTransferHistory(string startdate, string enddate)
        {
            if (string.IsNullOrEmpty(startdate)) {
                startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(enddate))
            {
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var dateS = int.Parse(startdate.Replace("-", ""));
            var dateE = int.Parse(enddate.Replace("-", ""));
            var data = historyTransfer.GetListReferalByIdAccount(memberSession.AccID, dateS, dateE);
            return View(data);
        }
        public ActionResult HistoryDetail(string startdate, string enddate)
        {
            if (string.IsNullOrEmpty(startdate)) {
                startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(enddate))
            {
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            var dateS = int.Parse(startdate.Replace("-", ""));
            var dateE = int.Parse(enddate.Replace("-", ""));
            var data = historyTransfer.GetListReferalByIdAccount(memberSession.AccID, dateS, dateE);
            return View(data);
        }
    }
}