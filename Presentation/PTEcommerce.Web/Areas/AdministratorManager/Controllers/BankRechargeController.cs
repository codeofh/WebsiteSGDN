using Framework.EF;
using marketplace;
using PTEcommerce.Business;
using PTEcommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Areas.AdministratorManager.Controllers
{
    public class BankRechargeController : Controller
    {
        private IBankRecharge bankRecharge;
        private IAccountCustomer accountCustomer;
        private IAccountAdmin accountAdmin;
        private IHistoryTransfer historyTransfer;
        private INotification notification;
        public BankRechargeController()
        {
            bankRecharge = SingletonIpl.GetInstance<IplBankRecharge>();
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
            accountAdmin = SingletonIpl.GetInstance<IplAccountAdmin>();
            historyTransfer = SingletonIpl.GetInstance<IplHistoryTransfer>();
            notification = SingletonIpl.GetInstance<IplNotification>();
        }
        // GET: AdministratorManager/BankRecharge
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListAllPagging(int idAccount, int status, string startDate, string endDate, int offset, int limit)
        {
            int dateStart = int.Parse(startDate.Replace("-", ""));
            int dateEnd = int.Parse(endDate.Replace("-", ""));
            var data = bankRecharge.ListAllPagging(idAccount, status, dateStart, dateEnd, offset, limit);
            int total = 0;
            if (data != null && data.Count > 0)
            {
                total = data[0].TotalRow;
            }
            return Json(new
            {
                rows = data,
                total = total
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubmitOrder(int id, int status)
        {
            var mems = SessionAdmin.GetUser();
            var accountAdminData = accountAdmin.GetById(mems.Id);
            if (accountAdminData == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản admin không hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }
            if (accountAdminData.RoleId != 1)
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản không có quyền!"
                }, JsonRequestBehavior.AllowGet);
            }
            if (id <= 0 || status == 1)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập đầy đủ thông tin"
                }, JsonRequestBehavior.AllowGet);
            }
            var dataOrder = bankRecharge.GetById(id);
            if (dataOrder == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Không tồn tại đơn rút tiền"
                }, JsonRequestBehavior.AllowGet);
            }
            if (dataOrder.Status == 2 || dataOrder.Status == 4)
            {
                return Json(new
                {
                    status = false,
                    message = "Đơn rút tiền đã được xử lý rồi"
                }, JsonRequestBehavior.AllowGet);
            }
            dataOrder.Status = status;
            dataOrder.ApproveDate = DateTime.Now;
            dataOrder.AccountConfirm = mems.Id;
            var flag = bankRecharge.Update(dataOrder);
            if (flag)
            {
                if (status == 2)
                {
                    var dataAccount = accountCustomer.GetById(dataOrder.IdAccount);
                    if (dataAccount != null)
                    {
                        var prices = dataAccount.AmountAvaiable;
                        historyTransfer.Insert(new HistoryTransfer
                        {
                            IdAccount = dataAccount.Id,
                            Type = 1,
                            AmountBefore = prices,
                            IdAccountAdmin = accountAdminData.Id,
                            AmountModified = dataOrder.Amount,
                            AmountAfter = prices + dataOrder.Amount,
                            CreatedDate = DateTime.Now,
                            Note = "Nạp " + Helper.NumberFormat(dataOrder.Amount) + " USD thành công"
                        });
                        dataAccount.AmountAvaiable = prices + dataOrder.Amount;
                        accountCustomer.Update(dataAccount);
                        notification.Insert(new Notification
                        {
                            IdAccount = dataAccount.Id,
                            CreatedDate = DateTime.Now,
                            IsRead = false,
                            Content = "Nạp " + PTEcommerce.Business.Helper.NumberFormat(dataOrder.Amount) + " USD thành công"
                        });
                    }
                }
                return Json(new
                {
                    status = true,
                    message = "Duyệt đơn nạp tiền thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                message = "Xử lý lỗi vui lòng thử lại"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}