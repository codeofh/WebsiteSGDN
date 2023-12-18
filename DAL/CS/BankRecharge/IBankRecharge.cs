using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public interface IBankRecharge : IBaseService<BankRecharge, int>
    {
        List<BankRechargeList> ListAllPagging(int idAccount, int status, int startDate, int endDate, int offset, int limit);
    }
}
