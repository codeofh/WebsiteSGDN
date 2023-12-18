using Framework;
using Framework.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public class IplHistoryTransfer: BaseService<HistoryTransfer, int>, IHistoryTransfer
    {
        public List<HistoryTransferExtend> ListAllPagingByCustomer(int idAccount, int pageIndex, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@pageIndex", pageIndex);
                p.Add("@pageSize", pageSize);
                p.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                var data = unitOfWork.Procedure<HistoryTransferExtend>("HistoryTransfer_ListAllPagingByCustomer", p).ToList();
                totalRow = p.Get<dynamic>("@totalRow");
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public List<HistoryTransferExtend> ListAllPagingByCustomerCondition(int idAccount, int year, int month, int pageIndex, int pageSize)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@Year", year);
                p.Add("@Month", month);
                p.Add("@pageIndex", pageIndex);
                p.Add("@pageSize", pageSize);
                var data = unitOfWork.Procedure<HistoryTransferExtend>("[HistoryTransfer_ListAllPagingByCustomerCondition]", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public List<HistoryTransferExtend> ListAllPaging(int idAccount, int idAccountAdmin, int type, int startDate, int endDate, int pageIndex, int pageSize)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@IdAccountConfirm", idAccountAdmin);
                p.Add("@Type", type);
                p.Add("@StartDate", startDate);
                p.Add("@EndDate", endDate);
                p.Add("@PageIndex", pageIndex);
                p.Add("@PageSize", pageSize);
                var data = unitOfWork.Procedure<HistoryTransferExtend>("[HistoryTransfer_ListAllPaging]", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public List<HistoryTransferExtend> ListAllPaging(int idAccount, int day)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@Day", day);
                var data = unitOfWork.Procedure<HistoryTransferExtend>("[HistoryTransfer_ListCustomer]", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public List<HistoryTransferExtend> GetListReferalByIdAccount(int idAccount, int startDate, int endDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@StartDate", startDate);
                p.Add("@EndDate", endDate);
                var data = unitOfWork.Procedure<HistoryTransferExtend>("[HistoryGetAllChildOfParent]", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public List<HistoryTransferExtend> GetListReferalCustomerByIdAccount(int idAccount, int startDate, int endDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@StartDate", startDate);
                p.Add("@EndDate", endDate);
                var data = unitOfWork.Procedure<HistoryTransferExtend>("[HistoryRefGetAllChildOfParent]", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public HistoryReport GetReport(int idAccount, int startDate, int endDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@StartDate", startDate);
                p.Add("@EndDate", endDate);
                var data = unitOfWork.Procedure<HistoryReport>("[HistoryTransfer_GetReport]", p).FirstOrDefault();
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
