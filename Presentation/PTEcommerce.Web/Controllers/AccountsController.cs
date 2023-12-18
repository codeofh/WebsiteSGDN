using Framework.Configuration;
using Framework.EF;
using marketplace;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using PTEcommerce.Business;
using PTEcommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Controllers
{
    public class LoginController : Controller
    {
        private IAccountCustomer account;
        private ISettings settings;
        private readonly string templateMailPath = Config.GetConfigByKey("templateMail");
        private readonly string isVerifyPhone = Config.GetConfigByKey("IsVerifyPhone");

        public LoginController()
        {
            account = SingletonIpl.GetInstance<IplAccountCustomer>();
            settings = SingletonIpl.GetInstance<IplSettings>();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Recovery()
        {
            return View();
        }
        public ActionResult Register(string refer = null)
        {
            ViewBag.Refer = refer;
            return View();
        }
        [HttpPost]
        public JsonResult Recovery(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập email"
                }, JsonRequestBehavior.AllowGet);
            }
            var password = RandomString(10);
            var passwordHash = Helper.sha256_hash(password + ConstKey.keySHA);
            var accountData = account.ViewDetailByEmail(email);
            if(accountData == null) {
                return Json(new
                {
                    status = false,
                    message = "Email không tồn tại trong hệ thống"
                }, JsonRequestBehavior.AllowGet);
            }
            accountData.Password = passwordHash;
            account.Update(accountData);
            Mailer mailer = new Mailer();
            string subject = "EXNESS - Đặt lại mật khẩu";
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplates/SendPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{action}", "Đăt lại mật khẩu");
            body = body.Replace("{code}", password);
            mailer.SendEmail(subject, body, email);
            return Json(new
            {
                status = true,
                message = "Đặt lại mật khẩu thành công, vui lòng kiểm tra hộp thư hoặc spam"
            }, JsonRequestBehavior.AllowGet);
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        public JsonResult Register(AccountCustomerRegister data)
        {
            if (string.IsNullOrEmpty(data.Email.Trim()))
            {
                return Json(new
                {
                    status = false,
                    msg = "Vui lòng nhập mật khẩu"
                }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(data.Password.Trim()))
            {
                return Json(new
                {
                    status = false,
                    msg = "Vui lòng nhập mật khẩu"
                }, JsonRequestBehavior.AllowGet);
            }
            if (data.Password.Length < 8)
            {
                return Json(new
                {
                    status = false,
                    msg = "Vui lòng nhập mật khẩu lớn hơn hoặc bằng 8 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }
            if (!Helper.ValidateEmail(data.Email.Trim()))
            {
                return Json(new
                {
                    status = false,
                    msg = "Email không đúng định dạng"
                }, JsonRequestBehavior.AllowGet);
            }
            var checkEmail = account.ViewDetailByEmail(data.Email.Trim());
            if (checkEmail != null)
            {
                return Json(new
                {
                    status = false,
                    msg = "Email đã có người đăng ký"
                }, JsonRequestBehavior.AllowGet);
            }
            int refID = 0;
            if (data.Ref.HasValue)
            {
                if (data.Ref <= 0)
                {
                    return Json(new
                    {
                        status = false,
                        msg = "Mã giới thiệu không hợp lệ"
                    }, JsonRequestBehavior.AllowGet);
                }
                var dataAccountRef = account.GetById(data.Ref.Value);
                if (dataAccountRef == null)
                {
                    return Json(new
                    {
                        status = false,
                        msg = "Không tồn tại mã giới thiệu"
                    }, JsonRequestBehavior.AllowGet);
                }
                refID = dataAccountRef.Id;
            }
            string token = Helper.GetToken(data.Email.Trim());
            var flagInsert = account.Insert(new AccountCustomer
            {
                Email = data.Email.Trim(),
                Password = Helper.sha256_hash(data.Password.Trim() + ConstKey.keySHA),
                AmountAvaiable = 0,
                IsActive = true,
                CreatedDate = DateTime.Now,
                BirthOfDate = DateTime.Now,
                VerifyPhone = isVerifyPhone.ToLower() == "false" ? true: false,
                OTPEmailDate = DateTime.Now.AddMinutes(-35),
                OTPPhoneDate = DateTime.Now.AddMinutes(-35),
                Token = token,
                Ref = refID
            });
            if (flagInsert != null)
            {
                var sess = new MemberSession
                {
                    AccID = flagInsert.Id,
                    Email = data.Email,
                    Token = token
                };
                SessionCustomer.sessionName = "customer";
                SessionCustomer.SetUser(sess);
                return Json(new
                {
                    status = true,
                    msg = "Đăng ký thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                msg = "Đăng ký thất bại"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Index(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Json(new
                {
                    status = false,
                    msg = "Vui lòng nhập tài khoản hoặc mật khẩu"
                }, JsonRequestBehavior.AllowGet);
            }
            var accountData = account.ViewDetailByEmail(username);
            //var accountData = account.ViewDetailByUserNamePassword(username, Helper.sha256_hash(password + ConstKey.keySHA));
            if (accountData == null)
            {
                return Json(new
                {
                    status = false,
                    msg = "Tài khoản hoặc mật khẩu không đúng"
                }, JsonRequestBehavior.AllowGet);
            }
            string token = Helper.GetToken(accountData.Email.Trim());
            account.UpdateToken(accountData.Id, token);
            var sess = new MemberSession
            {
                AccID = accountData.Id,
                Email = accountData.Email,
                Token = token
            };
            SessionCustomer.sessionName = "customer";
            SessionCustomer.SetUser(sess);
            return Json(new
            {
                status = true,
                msg = "Đăng nhập thành công"
            }, JsonRequestBehavior.AllowGet);
        }

    }
}