using Framework.Configuration;
using Framework.EF;
using marketplace;
using PTEcommerce.Business;
using PTEcommerce.Web.Extensions;
using PTEcommerce.Web.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using RestSharp;
using Framework.Helper.Logging;
using System.IO;

namespace PTEcommerce.Web.Controllers
{
    public class PAController : BaseController
    {
        private readonly ISettings settings;
        private readonly IBankRecharge bankRecharge;
        private readonly INotification notification;
        private readonly IBanks banks;
        private readonly IBankCustomer bankCustomer;
        private readonly IWithdrawal withdrawal;
        private readonly IPakages pakages;
        private readonly IOrderPakages orderPakages;
        private readonly IAccountCustomer accountCustomer;
        private readonly IHistoryTransfer historyTransfer;
        private readonly string url = Config.GetConfigByKey("url");
        private readonly string secretKey = Config.GetConfigByKey("secretKey");
        public PAController()
        {
            bankRecharge = SingletonIpl.GetInstance<IplBankRecharge>();
            pakages = SingletonIpl.GetInstance<IplPakages>();
            notification = SingletonIpl.GetInstance<IplNotification>();
            withdrawal = SingletonIpl.GetInstance<IplWithdrawal>();
            orderPakages = SingletonIpl.GetInstance<IplOrderPakages>();
            bankCustomer = SingletonIpl.GetInstance<IplBankCustomer>();
            banks = SingletonIpl.GetInstance<IplBanks>();
            settings = SingletonIpl.GetInstance<IplSettings>();
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
            historyTransfer = SingletonIpl.GetInstance<IplHistoryTransfer>();
        }
        // GET: Proccess
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Customer()
        {
            ViewBag.Account = accountCustomer.GetById(memberSession.AccID);
            return View();
        }
        public ActionResult Deposit()
        {
            ViewBag.Account = accountCustomer.GetById(memberSession.AccID);
            return View();
        }
        public ActionResult Invest()
        {
            var data = pakages.GetAll();
            ViewBag.ListPakageDay = data.Where(x => x.Type == 1).ToList();
            ViewBag.ListPakageMonth = data.Where(x => x.Type != 1).ToList();
            return View();
        }
        public ActionResult Chart()
        {
            return View();
        }
        public ActionResult DepositDetail()
        {
            ViewBag.USD2VND = settings.GetValueByKey("usdt2vndrecharge").Value;
            ViewBag.MinRecharge = settings.GetValueByKey("minrecharge").Value;
            ViewBag.MaxRecharge = settings.GetValueByKey("maxrecharge").Value;
            ViewBag.BankName = settings.GetValueByKey("bankname").Value;
            ViewBag.BankAccount = settings.GetValueByKey("bankaccount").Value;
            ViewBag.BankNumber = settings.GetValueByKey("banknumber").Value;
            ViewBag.BankSyntax = settings.GetValueByKey("banksyntax").Value;
            return View();
        }
        public ActionResult HistoryDetail(int day)
        {
            var data = historyTransfer.ListAllPaging(memberSession.AccID, day);
            return View(data);
        }
        public ActionResult Withdrawal()
        {
            ViewBag.Account = accountCustomer.GetById(memberSession.AccID);
            return View();
        }
        public ActionResult WithdrawalDetail()
        {
            ViewBag.MinWithdrawal = settings.GetValueByKey("minwithdrawal").Value;
            ViewBag.MaxWithdrawal = settings.GetValueByKey("maxwithdrawal").Value;
            ViewBag.USD2VND = settings.GetValueByKey("usdt2vnd").Value;
            ViewBag.ListBank = bankCustomer.GetListBank(memberSession.AccID);
            return View();
        }
        public ActionResult History()
        {
            return View();
        }
        public ActionResult Analytics()
        {
            return View();
        }
        public ActionResult Socialtrading()
        {
            return View();
        }
        public ActionResult Performance()
        {
            return View();
        }
        public ActionResult Settings(string tag = "home")
        {
            ViewBag.Account = accountCustomer.GetById(memberSession.AccID);
            ViewBag.Active = tag.ToLower();
            ViewBag.ListCustomerBank = bankCustomer.GetListBank(memberSession.AccID);
            ViewBag.ListBank = banks.GetAll();
            return View();
        }
        public ActionResult HistoryWithdrawal()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreateDeposit(decimal price)
        {
            if (price <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập số tiền hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }
            var usdt2vnd = decimal.Parse(settings.GetValueByKey("usdt2vndrecharge").Value);
            var min = decimal.Parse(settings.GetValueByKey("minrecharge").Value);
            var max = decimal.Parse(settings.GetValueByKey("maxrecharge").Value);
            if (price > max * usdt2vnd || price <= min * usdt2vnd)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nạp từ " + PTEcommerce.Business.Helper.NumberFormat(min) + "USD đến " + PTEcommerce.Business.Helper.NumberFormat(max) + "USD"
                }, JsonRequestBehavior.AllowGet);
            }
            var data = bankRecharge.Insert(new BankRecharge
            {
                ApproveDate = DateTime.Now,
                Amount = price / usdt2vnd,
                CreatedDate = DateTime.Now,
                IdAccount = memberSession.AccID,
                Type = 1,
                Status = 1,
                TransactionId = Helper.GetTimeUnix().ToString(),
                Usd2Vnd = (int)usdt2vnd
            });
            if (data != null)
            {
                return Json(new
                {
                    status = true,
                    message = "Yêu cầu đã được ghi nhận, vui lòng đợi duyệt"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = true,
                message = "Nạp tiền không thành công, vui lòng liên hệ admin"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateWithdrawal(int bankId, decimal price)
        {
            if (bankId <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng chọn ngân hàng"
                }, JsonRequestBehavior.AllowGet);
            }
            var bankList = bankCustomer.GetListBank(memberSession.AccID);
            if (bankList == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Không có tài khoản hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }
            var bankInfo = bankList.Where(x => x.Id == bankId).FirstOrDefault();
            if (bankInfo == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản ngân hàng không hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }
            if (price <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập số tiền hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }
            var usdt2vnd = decimal.Parse(settings.GetValueByKey("usdt2vnd").Value);
            var min = decimal.Parse(settings.GetValueByKey("minwithdrawal").Value);
            var max = decimal.Parse(settings.GetValueByKey("maxwithdrawal").Value);
            if (price > max || price < min)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng rút từ " + PTEcommerce.Business.Helper.NumberFormat(min) + "USD đến " + PTEcommerce.Business.Helper.NumberFormat(max) + "USD"
                }, JsonRequestBehavior.AllowGet);
            }
            var data = withdrawal.Insert(new Withdrawal
            {
                BankId = bankInfo.BankId,
                Amount = price,
                CreatedDate = DateTime.Now,
                IdAccount = memberSession.AccID,
                BankAccount = bankInfo.BankAccount,
                BankNumber = bankInfo.BankNumber,
                Status = 1,
                TransactionId = Helper.GetTimeUnix().ToString(),
                Usd2Vnd = (int)usdt2vnd
            });
            if (data != null)
            {
                var dataAccount = accountCustomer.GetById(memberSession.AccID);
                var beforeData = dataAccount.AmountAvaiable;
                dataAccount.AmountAvaiable -= price;
                accountCustomer.Update(dataAccount);
                historyTransfer.Insert(new HistoryTransfer
                {
                    IdAccount = dataAccount.Id,
                    CreatedDate = DateTime.Now,
                    AmountBefore = beforeData,
                    AmountModified = price,
                    AmountAfter = dataAccount.AmountAvaiable,
                    Note = "Rút tiền về ngân hàng " + bankInfo.BankName,
                    Type = 2
                });
                return Json(new
                {
                    status = true,
                    message = "Yêu cầu đã được ghi nhận, vui lòng đợi duyệt"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = true,
                message = "Rút tiền không thành công, vui lòng liên hệ admin"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateInvest(int pakageId)
        {
            if (pakageId <= 0)
            {
                return Json(new
                {
                    status = false,
                    code = 0,
                    message = "Vui lòng chọn gói đầu tư hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }
            var pakageData = pakages.GetById(pakageId);
            if (pakageData == null)
            {
                return Json(new
                {
                    status = false,
                    code = 0,
                    message = "Gói đầu tư không khả dụng"
                }, JsonRequestBehavior.AllowGet);
            }
            var dataAccount = accountCustomer.GetById(memberSession.AccID);
            if (pakageData.Prices > dataAccount.AmountAvaiable)
            {
                return Json(new
                {
                    status = false,
                    code = 1,
                    message = "Tài khoản không đủ để mua gói đầu tư"
                }, JsonRequestBehavior.AllowGet);
            }
            var result = orderPakages.Insert(new OrderPakages
            {
                IdAccount = dataAccount.Id,
                PakageId = pakageData.Id,
                Prices = pakageData.Prices,
                RateNow = pakageData.RateNow,
                Status = 1,
                CreatedDate = DateTime.Now,
                CompleteDate = DateTime.Now.AddDays(pakageData.Type),
                FinalPrice = 0
            });
            if (result != null)
            {
                var beforeData = dataAccount.AmountAvaiable;
                dataAccount.AmountAvaiable -= pakageData.Prices;
                accountCustomer.Update(dataAccount);
                historyTransfer.Insert(new HistoryTransfer
                {
                    IdAccount = dataAccount.Id,
                    CreatedDate = DateTime.Now,
                    AmountBefore = beforeData,
                    AmountModified = pakageData.Prices,
                    AmountAfter = dataAccount.AmountAvaiable,
                    Note = "Mua gói đầu tư " + (pakageData.Type == 1 ? "ngày" : "tháng") + " (" + pakageData.Name + ")",
                    Type = 3
                });
                if (pakageData.Bonus > 0)
                {
                    historyTransfer.Insert(new HistoryTransfer
                    {
                        IdAccount = dataAccount.Id,
                        CreatedDate = DateTime.Now,
                        AmountBefore = dataAccount.AmountAvaiable,
                        AmountModified = pakageData.Prices,
                        AmountAfter = dataAccount.AmountAvaiable + pakageData.Bonus,
                        Note = "Khuyến mãi " + pakageData.Bonus + " USD khi mua gói đầu tư " + pakageData.Name,
                        Type = 4
                    });
                    dataAccount.AmountAvaiable += pakageData.Bonus;
                    accountCustomer.Update(dataAccount);
                }
                return Json(new
                {
                    status = false,
                    code = 0,
                    message = "Mua gói đầu tư thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                code = 0,
                message = "Mua gói đầu tư không thành công, vui lòng liên hệ admin"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateOrEditBank(int id, int bankId, string bankAccount, string bankNumber)
        {
            if (id == 0)
            {
                if (bankId <= 0)
                {
                    return Json(new
                    {
                        status = false,
                        message = "Vui lòng chọn ngân hàng"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(bankAccount))
                {
                    return Json(new
                    {
                        status = false,
                        message = "Vui lòng nhập chủ tài khoản"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(bankNumber))
                {
                    return Json(new
                    {
                        status = false,
                        message = "Vui lòng số tài khoản"
                    }, JsonRequestBehavior.AllowGet);
                }
                var listRegister = bankCustomer.GetAll().Where(x => x.IdAccount == memberSession.AccID).ToList();
                if (listRegister.Count >= 8)
                {
                    return Json(new
                    {
                        status = false,
                        message = "Bạn chỉ được phép tao tối đa 8 tài khoản ngân hàng"
                    }, JsonRequestBehavior.AllowGet);
                }
                var result = bankCustomer.Insert(new BankCustomer
                {
                    IdAccount = memberSession.AccID,
                    BankId = bankId,
                    BankAccount = bankAccount,
                    BankNumber = bankNumber
                });
                if (result != null)
                {

                    return Json(new
                    {
                        status = true,
                        message = "Thêm tài khoản ngân hàng thành công"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    status = false,
                    message = "Thêm tài khoản ngân hàng không thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var bankCustomerData = bankCustomer.GetById(id);
                if (bankCustomer == null)
                {
                    return Json(new
                    {
                        status = true,
                        message = "Tài khoản chỉnh sửa không tồn tại"
                    }, JsonRequestBehavior.AllowGet);
                }
                bankCustomerData.BankId = bankId;
                bankCustomerData.BankAccount = bankAccount;
                bankCustomerData.BankNumber = bankNumber;
                var result = bankCustomer.Update(bankCustomerData);
                if (result)
                {

                    return Json(new
                    {
                        status = true,
                        message = "Cập nhật tài khoản ngân hàng thành công"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    status = false,
                    message = "Cập nhật khoản ngân hàng không thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                message = "Có lỗi xảy ra"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteBank(int id)
        {
            if (id <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Bản ghi không tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }
            var bankCustomerData = bankCustomer.GetById(id);
            if (bankCustomerData == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Bản ghi không tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }
            var result = bankCustomer.Delete(bankCustomerData);
            if (result)
            {

                return Json(new
                {
                    status = true,
                    message = "Xoá tài khoản ngân hàng thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                message = "Xoá khoản ngân hàng không thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateIsRead(int id)
        {
            var data = notification.GetById(id);
            if (data != null)
            {
                data.IsRead = true;
                notification.Update(data);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}