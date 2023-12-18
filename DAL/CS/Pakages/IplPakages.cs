using Framework;
using Framework.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public class IplPakages : BaseService<Pakages, int>, IPakages
    {
        public List<PakagesList> ListAllPagging(int offset, int limit)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@PageIndex", offset);
                p.Add("@PageSize", limit);
                var data = unitOfWork.Procedure<PakagesList>("PakagesList_ListAllPagging", p).ToList();
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
