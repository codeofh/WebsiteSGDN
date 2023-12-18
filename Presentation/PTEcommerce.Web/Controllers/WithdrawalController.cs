using Framework.EF;
using Framework.Helper.Logging;
using marketplace;
using PTEcommerce.Business;
using PTEcommerce.Business.Business;
using PTEcommerce.Business.IBusiness;
using PTEcommerce.Web.Extensions;
using PTEcommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Controllers
{
    public class WithdrawalController : BaseController
    {
        private readonly IAccountCustomer accountCustomer;
        private readonly IWithdrawal withdrawal;
        private readonly IBanks banks;
        private readonly IHistoryTransfer historyTransfer;
        public WithdrawalController()
        {
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
            withdrawal = SingletonIpl.GetInstance<IplWithdrawal>();
            historyTransfer = SingletonIpl.GetInstance<IplHistoryTransfer>();
            banks = SingletonIpl.GetInstance<IplBanks>();
        }
        // GET: Withdrawal
        public ActionResult Index()
        {
            ViewBag.Account = accountCustomer.GetById(memberSession.AccID);
            ViewBag.ListBank = banks.GetAll();
            return View();
        }
        public ActionResult History()
        {
            return View();
        }
        public ActionResult HistoryTransaction(int offset, int limit = 10)
        {
            var data = withdrawal.ListAllPagingByCustomer(memberSession.AccID, offset, limit);
            return View(data);
        }
    }
}