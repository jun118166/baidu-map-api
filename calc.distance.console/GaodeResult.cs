using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
    public class GaodeResult
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string errdetail { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public int count { get; set; }

        public GaodeRoute route { get; set; }
    }

    public class GaodeRoute
    {
        public List<Path> paths { get; set; }
    }

    public class Path
    {
        public List<GaodeStep> steps { get; set; }
        public int distance { get; set; }
        public int duration { get; set; }
    }
    public class GaodeStep
    {
        public string instruction { get; set; }
        public decimal tolls { get; set; }
    }
}
