using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
   public class GaodeParams
    {
        public string origin { get; set; }
        public string destination { get; set; }
        //1：微型车，2：轻型车（默认值），3：中型车，4：重型车
        public string size { get; set; }
    }
}
