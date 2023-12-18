using Framework;
using Framework.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public class IplWithdrawal : BaseService<Withdrawal, int>, IWithdrawal
    {
        public List<WithdrawalExtend> ListAllPagingByCustomer(int idAccount, int pageIndex, int pageSize)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@PageIndex", pageIndex);
                p.Add("@PageSize", pageSize);
                var data = unitOfWork.Procedure<WithdrawalExtend>("[Withdrawal_ListAllPagingByCustomerByCondition]", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public void ReportRechargeAndWithdrawal(ref decimal recharge, ref decimal withdrawal)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Recharge", dbType: System.Data.DbType.Decimal, direction: System.Data.ParameterDirection.Output);
                p.Add("@Withdrawal", dbType: System.Data.DbType.Decimal, direction: System.Data.ParameterDirection.Output);
                var data = unitOfWork.ProcedureExecute("ReportRechargeAndWithdrawal", p);
                if (data)
                {
                    recharge = p.Get<decimal>("@Recharge");
                    withdrawal = p.Get<decimal>("@Withdrawal");
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
            }
        }
        public List<WithdrawalExtend> ListAllPagingByCustomer(int idAccount, int pageIndex, int pageSize, ref int totalRow)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@PageIndex", pageIndex);
                p.Add("@PageSize", pageSize);
                p.Add("@TotalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                var data = unitOfWork.Procedure<WithdrawalExtend>("Withdrawal_ListAllPagingByCustomer", p).ToList();
                totalRow = p.Get<dynamic>("@TotalRow");
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public List<WithdrawalExtend> ListAllPaging(int idAccount, int status, int startDate, int endDate, int pageIndex, int pageSize)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                p.Add("@Status", status);
                p.Add("@BankId", 0);
                p.Add("@StartDate", startDate);
                p.Add("@EndDate", endDate);
                p.Add("@PageIndex", pageIndex);
                p.Add("@PageSize", pageSize);
                var data = unitOfWork.Procedure<WithdrawalExtend>("Withdrawal_ListAllPaging", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public Withdrawal CheckQuantityWithdrawal(int idAccount)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                var flag = unitOfWork.Procedure<Withdrawal>("Withdrawal_CheckTotalWithdrawal", p).FirstOrDefault();
                return flag;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
    }
}
