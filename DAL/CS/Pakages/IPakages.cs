using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketplace
{
    public interface IPakages : IBaseService<Pakages, int>
    {
        List<PakagesList> ListAllPagging(int offset, int limit);
    }
}
