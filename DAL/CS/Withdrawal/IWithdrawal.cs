using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public interface IWithdrawal :IBaseService<Withdrawal, int>
    {
        List<WithdrawalExtend> ListAllPaging(int idAccount, int status, int startDate, int endDate, int pageIndex, int pageSize);
        void ReportRechargeAndWithdrawal(ref decimal recharge, ref decimal withdrawal);
        List<WithdrawalExtend> ListAllPagingByCustomer(int idAccount, int pageIndex, int pageSize);
        List<WithdrawalExtend> ListAllPagingByCustomer(int idAccount, int pageIndex, int pageSize, ref int totalRow);
        Withdrawal CheckQuantityWithdrawal(int idAccount);
    }
}
