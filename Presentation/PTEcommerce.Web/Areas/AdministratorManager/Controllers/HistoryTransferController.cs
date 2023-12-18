using Framework.EF;
using marketplace;
using PTEcommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Areas.AdministratorManager.Controllers
{
    public class HistoryTransferController : BaseController
    {
        private readonly IHistoryTransfer historyTransfer;
        private readonly IAccountCustomer accountCustomer;
        private readonly IAccountAdmin accountAdmin;
        public HistoryTransferController()
        {
            historyTransfer = SingletonIpl.GetInstance<IplHistoryTransfer>();
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
            accountAdmin = SingletonIpl.GetInstance<IplAccountAdmin>();
        }
        public ActionResult Index()
        {
            ViewBag.ListAccount = accountCustomer.GetAll();
            ViewBag.ListAccountAdmin = new List<AccountAdmin>();
            var mems = SessionAdmin.GetUser();
            if(mems.RoleId == 1)
            {
                ViewBag.ListAccountAdmin = accountAdmin.GetAll();
            }
            return View();
        }
        public ActionResult HistoryPlay() {
            ViewBag.ListAccount = accountCustomer.GetAll();
            return View();
        }
        public JsonResult ListAllPaging(int idAccount, int idAccountAdmin, int type, string startDate, string endDate)
        {
            int totalRow = 0;
            var p = GetPagingMessage(Request.QueryString);
            int dateStart = int.Parse(startDate.Replace("-", ""));
            int dateEnd = int.Parse(endDate.Replace("-", ""));
            var data = historyTransfer.ListAllPaging(idAccount, idAccountAdmin, type, dateStart, dateEnd, p.PageIndex, p.PageSize);
            if (data != null && data.Count > 0)
            {
                totalRow = data[0].TotalRow;
            } 
            return Json(new
            {
                status = true,
                rows = data,
                total = totalRow
            }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ListAllPlayHistoryPaging(int idAccount)
        //{
        //    int totalRow = 0;
        //    var p = GetPagingMessage(Request.QueryString);
        //    var data = playGames.GetListHistoryPlayGame(idAccount, p.PageIndex, p.PageSize);
        //    if (data != null && data.Count > 0)
        //    {
        //        totalRow = data[0].TotalRow;
        //    }
        //    return Json(new
        //    {
        //        status = true,
        //        rows = data != null && data.Count > 0 ? data.Select(x => new
        //        {
        //            Id = x.Id,
        //            Username = x.Username,
        //            Value = Business.Helper.ConvertValue(x.Value).valuestring,
        //            Result = x.ResultString,
        //            CreatedDate = x.CreatedDate,
        //            CompletedDate = x.CompletedDate,
        //            Amount = x.Amount * Business.Helper.ConvertValue(x.Value).value.Count,
        //            AmountReceive = x.AmountReceive,
        //            SessionId = x.SessionId
        //        }).ToList() : new object(),
        //        total = totalRow
        //    }, JsonRequestBehavior.AllowGet);
        //}
    }
}