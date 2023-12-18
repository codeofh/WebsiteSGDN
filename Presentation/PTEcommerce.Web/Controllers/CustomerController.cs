using Framework.Configuration;
using Framework.EF;
using marketplace;
using Newtonsoft.Json;
using PTEcommerce.Business;
using PTEcommerce.Web.Extensions;
using PTEcommerce.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IAccountCustomer accountCustomer;
        private readonly string templateMailPath = Config.GetConfigByKey("templateMail");
        private readonly string secretKey = Config.GetConfigByKey("secretKey");
        public CustomerController()
        {
            accountCustomer = SingletonIpl.GetInstance<IplAccountCustomer>();
        }
        // GET: AccountCustomer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetInformation()
        {
            if (memberSession == null)
            {
                SessionCustomer.ClearSession();
                return Json(new
                {
                    code = 404,
                    AmountAvaiable = 0
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = accountCustomer.GetById(memberSession.AccID);
                if (memberSession.Token != data.Token)
                {
                    SessionCustomer.ClearSession();
                    return Json(new
                    {
                        code = 401,
                        AmountAvaiable = 0
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (memberSession.AccID != data.Id)
                    {
                        SessionCustomer.ClearSession();
                        return Json(new
                        {
                            code = 401,
                            AmountAvaiable = 0
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            code = 200,
                            AmountAvaiable = data.AmountAvaiable,
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        [HttpPost]
        public JsonResult SendEmailOTP()
        {
            var email = memberSession.Email;
            var dataAccount = accountCustomer.ViewDetailByEmail(email);
            if (dataAccount == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Hết phiên đăng nhập, vui lòng đăng nhập lại"
                }, JsonRequestBehavior.AllowGet);
            }
            if (dataAccount.VerifyEmail)
            {
                return Json(new
                {
                    status = false,
                    message = "Email đã được xác thực trên hệ thống"
                }, JsonRequestBehavior.AllowGet);
            }
            if (dataAccount.OTPEmailDate.AddMinutes(30) > DateTime.Now)
            {
                return Json(new
                {
                    status = false,
                    message = "Yêu cầu OTP mới sau " + dataAccount.OTPEmailDate.AddMinutes(30).ToString("dd/MM/yyyy HH:mm:ss")
                }, JsonRequestBehavior.AllowGet);
            }
            var otp = Helper.GenerateOTP(secretKey + "email" + dataAccount.Id);
            SendMailGenerateOTP(email, otp, "Xác thực email");
            dataAccount.OTPEmailDate = DateTime.Now.AddMinutes(30);
            accountCustomer.Update(dataAccount);
            return Json(new
            {
                status = true,
                message = "Đã gửi mã xác nhận vào email, vui lòng kiểm tra trong hộp thư hoặc spam"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SendPhoneOTP(string phone)
        {
            var dataAccount = accountCustomer.GetById(memberSession.AccID);
            if (dataAccount == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Số điện thoại chưa được đăng ký trên hệ thống"
                }, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(dataAccount.Phone))
            {
                if (!dataAccount.Phone.ToLower().Equals(phone))
                {
                    return Json(new
                    {
                        status = false,
                        message = "Số điện thoại không trùng với số điện thoại đăng ký"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    phone = dataAccount.Phone;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(phone))
                {
                    return Json(new
                    {
                        status = false,
                        message = "Vui lòng nhập số điện thoại"
                    }, JsonRequestBehavior.AllowGet);
                }
                else  if (phone.Length != 10)
                {
                    return Json(new
                    {
                        status = false,
                        message = "Số điện thoại không đúng định dạng"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    dataAccount.Phone = phone;
                    accountCustomer.Update(dataAccount);
                }
            }
            
            if (dataAccount.VerifyPhone)
            {
                return Json(new
                {
                    status = false,
                    message = "Số điện thoại đã được xác thực trên hệ thống"
                }, JsonRequestBehavior.AllowGet);
            }
            
            if (dataAccount.OTPPhoneDate.AddMinutes(30) > DateTime.Now)
            {
                return Json(new
                {
                    status = false,
                    message = "Yêu cầu OTP mới sau " + dataAccount.OTPEmailDate.AddMinutes(30).ToString("dd/MM/yyyy HH:mm:ss")
                }, JsonRequestBehavior.AllowGet);
            }
            var otp = Helper.GenerateOTP(secretKey + "phone" + dataAccount.Id);
            var result = SMSAPI.sendSMS(new string[] { dataAccount.Phone.Trim() }, "EXNESS - Ma xac nhan cua ban la: " + otp + ". Khong cung cap ma cho nguoi la.", 1, string.Empty);
            return Json(new
            {
                status = true,
                message = "Đã gửi mã xác nhận vào số điện thoại, vui lòng kiểm tra tin nhắn"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult VerifyAccount(int type, string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập mã xác nhận"
                }, JsonRequestBehavior.AllowGet);
            }
            string keyVal = "email";
            if (type == 1)
            {
                keyVal = "phone";
                var dataAccount = accountCustomer.GetById(memberSession.AccID);
                if (!string.IsNullOrEmpty(dataAccount.Phone))
                {
                    return Json(new
                    {
                        status = true,
                        message = "Tài khoản đã xác nhận số điện thoại rồi"
                    }, JsonRequestBehavior.AllowGet);
                }
                dataAccount.Phone = otp;
                dataAccount.VerifyPhone = true;
                accountCustomer.Update(dataAccount);
                return Json(new
                {
                    status = true,
                    message = "Xác nhận thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = Helper.ValidateOTP(otp, secretKey + keyVal + memberSession.AccID);
                if (result)
                {
                    var dataAccount = accountCustomer.GetById(memberSession.AccID);
                    if (type == 0)
                    {
                        dataAccount.VerifyEmail = true;
                    }
                    else
                    {
                        dataAccount.VerifyPhone = true;
                    }
                    accountCustomer.Update(dataAccount);
                    return Json(new
                    {
                        status = true,
                        message = "Xác nhận thành công"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                status = false,
                message = "Mã xác nhận sai hoặc hết hạn"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangePassword(string password, string repassword, string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập mã xác nhận"
                }, JsonRequestBehavior.AllowGet);
            }
            var resultOtp = Helper.ValidateOTP(otp, secretKey + "changepass" + memberSession.AccID);
            if (!resultOtp)
            {
                return Json(new
                {
                    status = false,
                    message = "Mã xác nhận sai hoặc hết hạn"
                }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập mật khẩu mới"
                }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(repassword))
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng lặp lại mật khẩu mới"
                }, JsonRequestBehavior.AllowGet);
            }
            if (!repassword.Equals(password))
            {
                return Json(new
                {
                    status = false,
                    message = "Lặp lại mật khẩu không đúng"
                }, JsonRequestBehavior.AllowGet);
            }
            var passwordHash = Helper.sha256_hash(password + ConstKey.keySHA);
            var dataAccount = accountCustomer.GetById(memberSession.AccID);
            if (dataAccount == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản không tồn tại hoặc đã bị xoá"
                }, JsonRequestBehavior.AllowGet);
            }
            dataAccount.Password = passwordHash;
            var result = accountCustomer.Update(dataAccount);
            if (result)
            {
                SessionCustomer.ClearSession();
                return Json(new
                {
                    status = true,
                    message = "Đổi mật khẩu thành công, vui lòng đăng nhập lại"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                message = "Đổi mật khẩu không thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SendOTPChangePassword()
        {
            var dataAccount = accountCustomer.GetById(memberSession.AccID);
            if (dataAccount == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Hết phiên đăng nhập, vui lòng đăng nhập lại"
                }, JsonRequestBehavior.AllowGet);
            }
            var otp = Helper.GenerateOTP(secretKey + "changepass" + memberSession.AccID);
            SendMailGenerateOTP(dataAccount.Email, otp, "Xác thực đổi mật khẩu");
            return Json(new
            {
                status = true,
                message = "Đã gửi mã xác nhận vào email, vui lòng kiểm tra trong hộp thư hoặc spam"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(string FullName, string Gender, string BirthOfDate, string Address)
        {
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Gender) || string.IsNullOrEmpty(BirthOfDate) || string.IsNullOrEmpty(Address))
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập đầy đủ thông tin"
                }, JsonRequestBehavior.AllowGet);
            }
            var arrBOD = BirthOfDate.Split('-');
            var birthDay = new DateTime(int.Parse(arrBOD[0]), int.Parse(arrBOD[1]), int.Parse(arrBOD[2]));
            var accountData = accountCustomer.GetById(memberSession.AccID);
            if(accountData == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Thông tin không khả dụng"
                }, JsonRequestBehavior.AllowGet);
            }
            accountData.FullName = FullName;
            accountData.BirthOfDate = birthDay;
            accountData.Gender = Gender;
            accountData.Address = Address;
            var result = accountCustomer.Update(accountData);
            if (result)
            {
                return Json(new
                {
                    status = true,
                    message = "Cập nhật thông tin thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                message = "Cập nhật thông tin không thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult KYCImage(HttpPostedFileBase front, HttpPostedFileBase back)
        {
            if(front == null || back == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng tải lên cả mặt trước và mặt sau"
                }, JsonRequestBehavior.AllowGet);
            }
            var dataAccount = accountCustomer.GetById(memberSession.AccID);
            if(dataAccount == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản xác minh không khả dụng"
                }, JsonRequestBehavior.AllowGet);
            }
            #region Front
            string extentionFront = Path.GetExtension(front.FileName);
            string path = Path.Combine(Server.MapPath("/Uploads/Images/KYC/"), "user_front_" + memberSession.AccID + extentionFront);
            string pathDbFront = "/Uploads/Images/KYC/" + "user_front_" + memberSession.AccID + extentionFront;
            // file is uploaded
            front.SaveAs(path);
            using (MemoryStream ms = new MemoryStream())
            {
                front.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();
            }
            #endregion

            #region Back
            string extentionBack = Path.GetExtension(back.FileName);
            string pathBack = Path.Combine(Server.MapPath("/Uploads/Images/KYC/"), "user_back_" + memberSession.AccID + extentionBack);
            string pathDbBack = "/Uploads/Images/KYC/" + "user_back_" + memberSession.AccID + extentionBack;
            // file is uploaded
            back.SaveAs(pathBack);
            using (MemoryStream ms = new MemoryStream())
            {
                back.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();
            }
            #endregion
            dataAccount.KYCFront = pathDbFront;
            dataAccount.KYCBack = pathDbBack;
            var result = accountCustomer.Update(dataAccount);
            if (result)
            {
                return Json(new
                {
                    status = true,
                    message = "Tải ảnh lên thành công, vui lòng đợi duyệt"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                message = "Tải ảnh lên không thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult KYC()
        {
            var data = accountCustomer.GetById(memberSession.AccID);
            ViewBag.Account = data;
            if (data.VerifyEmail && data.VerifyKYC && data.VerifyPhone)
            {
                return Redirect("/pa/invest");
            }
            return View();
        }
        //[HttpPost]
        //public JsonResult KYC()
        //{
        //    return Json(new
        //    {
        //        status = true,
        //        message = "Đã gửi mã xác nhận vào số điện thoại, vui lòng kiểm tra tin nhắn"
        //    }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Logout()
        {
            SessionCustomer.ClearSession();
            return Redirect("/");
        }
        private void SendMailGenerateOTP(string email, string otp, string action)
        {
            Mailer mailer = new Mailer();
            string subject = "EXNESS - " + action;
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplates/SendOTP.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{code}", otp);
            body = body.Replace("{action}", action);
            mailer.SendEmail(subject, body, email);
        }
    }
}