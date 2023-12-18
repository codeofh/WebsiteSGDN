using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public interface IOrderPakages : IBaseService<OrderPakages, int>
    {
        List<OrderPakagesList> ListAllPagging(int idAccount, int status, int startDate, int endDate, int offset, int limit);
        List<OrderPakages> GetListUnProcess();
        List<AccountRef> GetListAccountReferal(int idAccount);
    }
}
