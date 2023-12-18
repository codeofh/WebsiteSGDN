using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public class IplNotification : BaseService<Notification, int>, INotification
    {
        public List<Notification> GetListNotificationByUser(int idAccount)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@IdAccount", idAccount);
                var data = unitOfWork.Procedure<Notification>("Notification_GetByIdAccount", p).ToList();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
