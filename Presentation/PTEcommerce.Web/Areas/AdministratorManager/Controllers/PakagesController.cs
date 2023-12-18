using Framework.EF;
using marketplace;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTEcommerce.Web.Areas.AdministratorManager.Controllers
{
    public class PakagesController : BaseController
    {
        private IPakages pakages;
        public PakagesController()
        {
            pakages = SingletonIpl.GetInstance<IplPakages>();
        }
        // GET: AdministratorManager/Pakages
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListAllPagging(int offset, int limit)
        {
            int total = 0;
            var data = pakages.ListAllPagging(offset, limit);
            if (data != null && data.Count > 0)
            {
                total = data[0].TotalRow;
            }
            return Json(new
            {
                rows = data,
                total = total,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Pakages item)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    status = false,
                    message = "Vui lòng nhập đầy đủ dữ liệu"
                }, JsonRequestBehavior.AllowGet);
            }
            var data = pakages.GetById(item.Id);
            if(data == null)
            {
                return Json(new
                {
                    status = false,
                    message = "Gói không tồn tại hoặc bị xoá"
                }, JsonRequestBehavior.AllowGet);
            }
            data.Name = item.Name;
            data.MinRate = item.MinRate;
            data.MaxRate = item.MaxRate;
            data.Bonus = item.Bonus;
            data.Prices = item.Prices;
            data.Type = item.Type;
            data.RateNow = item.RateNow;
            var result = pakages.Update(data);
            if (result)
            {
                return Json(new
                {
                    status = true,
                    message = "Cập nhật thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false,
                message = "Cập nhật không thành công"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}