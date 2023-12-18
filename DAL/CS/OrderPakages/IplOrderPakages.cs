using Framework;
using Framework.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public class IplOrderPakages : BaseService<OrderPakages, int>, IOrderPakages
    {
        public List<OrderPakages> GetListUnProcess()
        {
            try
            {
                var data = unitOfWork.Procedure<OrderPakages>("OrderPakage_GetListUnProcess").ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public List<AccountRef> GetListAccountReferal(int idAccount)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                var data = unitOfWork.Procedure<AccountRef>("OrderPakage_GetListReferal", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public List<OrderPakagesList> ListAllPagging(int idAccount, int status, int startDate, int endDate, int offset, int limit)
        {
			try
			{
				var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@Status", status);
                p.Add("@StartDate", startDate);
                p.Add("@EndDate", endDate);
                p.Add("@Offset", offset);
                p.Add("@Limit", limit);
                var data = unitOfWork.Procedure<OrderPakagesList>("OrderPakages_ListAllPaging", p).ToList();
                return data;
            }
			catch (Exception ex)
			{
				Logging.PutError(ex.Message, ex);
				return null;
			}
        }
    }
}
