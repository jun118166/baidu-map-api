using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
    public class BaiduResult
    {
        public string status { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public Location location { get; set; }
        public int precise { get; set; }
        public int confidence { get; set; }
        public string level { get; set; }

        public List<Route> routes { get; set; }
    }
    /// <summary>
    /// 路由
    /// </summary>
    public class Route
    {
        public int distance { get; set; }
        public int duration { get; set; }
        public List<Step> steps { get; set; }
    }

    public class Location
    {
        public double lng { get; set; }
        public double lat { get; set; }
    }

    /// <summary>
    /// 规划路线
    /// </summary>
    public class Step
    {
        /// <summary>
        /// 道路名
        /// </summary>
        public string road_name { get; set; }
    }
}
