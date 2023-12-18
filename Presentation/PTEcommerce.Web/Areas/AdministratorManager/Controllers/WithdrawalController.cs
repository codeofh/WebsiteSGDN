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
    public class WithdrawalController : BaseController
    {

        private readonly IAccountCustomer accountCustomer;
        private readonly IWithdrawal withdrawal;
        private readonly IHistoryTransfer historyTransfer;
        private readonly IAccountAdmin accountAdmin;
        private readonly INotification notification;
        public WithdrawalController()
        {
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
            withdrawal = SingletonIpl.GetInstance<IplWithdrawal>();
            accountAdmin = SingletonIpl.GetInstance<IplAccountAdmin>();
            historyTransfer = SingletonIpl.GetInstance<IplHistoryTransfer>();
            notification = SingletonIpl.GetInstance<IplNotification>();
        }
        public ActionResult Index()
        {
            ViewBag.ListAccount = accountCustomer.GetAll();
            return View();
        }
        public JsonResult ListAllPaging(int idAccount, int status, string startDate, string endDate)
        {
            int totalRow = 0;
            var p = GetPagingMessage(Request.QueryString);
            int dateStart = int.Parse(startDate.Replace("-", ""));
            int dateEnd = int.Parse(endDate.Replace("-", ""));
            var data = withdrawal.ListAllPaging(idAccount, status, dateStart, dateEnd, p.PageIndex, p.PageSize);
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
        public JsonResult SubmitOrder(int id, int status, string note)
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
            if (id <= 0 || status == 1 || string.IsNullOrEmpty(note))
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập đầy đủ thông tin"
                }, JsonRequestBehavior.AllowGet);
            }
            var dataOrder = withdrawal.GetById(id);
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
            dataOrder.Note = note;
            dataOrder.AccountConfirm = mems.Id;
            var flag = withdrawal.Update(dataOrder);
            if (flag)
            {
                var dataAccount = accountCustomer.GetById(dataOrder.IdAccount);
                if (status == 4)
                {
                    if (dataAccount != null)
                    {
                        var prices = dataAccount.AmountAvaiable;
                        historyTransfer.Insert(new HistoryTransfer
                        {
                            IdAccount = dataAccount.Id,
                            IdAccountAdmin = accountAdminData.Id,
                            Type = (int)EnumSystem.EnumTypeHistoryTransfer.withdrawal,
                            AmountBefore = prices,
                            AmountModified = dataOrder.Amount,
                            AmountAfter = prices + dataOrder.Amount,
                            CreatedDate = DateTime.Now,
                            Note = "Rút tiền bị hủy, hoàn lại " + Helper.MoneyFormat(dataOrder.Amount) + " về tài khoản"
                        });
                        dataAccount.AmountAvaiable = prices + dataOrder.Amount;
                        accountCustomer.Update(dataAccount);
                        notification.Insert(new Notification
                        {
                            IdAccount = dataAccount.Id,
                            CreatedDate = DateTime.Now,
                            IsRead = false,
                            Content = "Mã rút tiền #" + dataOrder.Id + " đã bị huỷ"
                        });
                    }
                }
                else
                {
                    notification.Insert(new Notification
                    {
                        IdAccount = dataAccount.Id,
                        CreatedDate = DateTime.Now,
                        IsRead = false,
                        Content = "Mã rút tiền #" + dataOrder.Id + "  đã được duyệt"
                    });
                }
                return Json(new
                {
                    status = true,
                    message = "Duyệt đơn rút tiền thành công"
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