using Framework;
using Framework.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public class IplBankCustomer : BaseService<BankCustomer, int>, IBankCustomer
    {
        public List<BankCustomerExtend> GetListBank(int idAccout)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccout);
                var data = unitOfWork.Procedure<BankCustomerExtend>("BankCustomer_GetByIdAccount", p).ToList();
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
