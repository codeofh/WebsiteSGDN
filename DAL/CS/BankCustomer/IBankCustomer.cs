using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public interface IBankCustomer : IBaseService<BankCustomer, int>
    {
        List<BankCustomerExtend> GetListBank(int idAccout);
    }
}
