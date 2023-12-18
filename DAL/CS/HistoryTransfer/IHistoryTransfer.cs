using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public interface IHistoryTransfer: IBaseService<HistoryTransfer, int>
    {
        List<HistoryTransferExtend> ListAllPagingByCustomer(int idAccount, int pageIndex, int pageSize, ref int totalRow);
        List<HistoryTransferExtend> ListAllPaging(int idAccount, int idAccountAdmin, int type, int startDate, int endDate, int pageIndex, int pageSize);
        List<HistoryTransferExtend> GetListReferalCustomerByIdAccount(int idAccount, int startDate, int endDate);
        List<HistoryTransferExtend> ListAllPagingByCustomerCondition(int idAccount, int year, int month, int pageIndex, int pageSize);
        List<HistoryTransferExtend> GetListReferalByIdAccount(int idAccount, int startDate, int endDate);
        List<HistoryTransferExtend> ListAllPaging(int idAccount, int day);
        HistoryReport GetReport(int idAccount, int startDate, int endDate);
    }
}
