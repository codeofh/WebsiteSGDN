using Framework;
using Framework.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public class IplBankRecharge : BaseService<BankRecharge, int>, IBankRecharge
    {
        public List<BankRechargeList> ListAllPagging(int idAccount, int status, int startDate, int endDate, int offset, int limit)
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
                var data = unitOfWork.Procedure<BankRechargeList>("BankRecharge_ListAllPagging", p).ToList();
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
