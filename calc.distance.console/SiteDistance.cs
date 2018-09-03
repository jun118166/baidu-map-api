using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
    //网点距离
    public class SiteDistance
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        public int sn { get; set; }
        /// <summary>
        /// 始发编码
        /// </summary>
        [Description("起点编号")]
        public string startCode { get; set; }
        [Description("起点经度")]
        public double startlng { get; set; }
        [Description("起点维度")]
        public double startlat { get; set; }
        /// <summary>
        /// 始发名称
        /// </summary>
        [Description("起点名称")]
        public string startName { get; set; }
        /// <summary>
        /// 始发地址
        /// </summary>
        [Description("起点地址")]
        public string startAddress { get; set; }
        /// <summary>
        /// 结束编码
        /// </summary>
        [Description("终点编号")]
        public string endCode { get; set; }
        [Description("终点经度")]
        public double endlng { get; set; }
        [Description("终点维度")]
        public double endlat { get; set; }
        /// <summary>
        /// 结束名称
        /// </summary>
        [Description("终点名称")]
        public string endName { get; set; }
        /// <summary>
        /// 结束地址
        /// </summary>
        [Description("终点地址")]
        public string endAddress { get; set; }
        /// <summary>
        /// 运行时长
        /// </summary>
        [Description("行驶时间（单位：分）")]
        public double runTime { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        [Description("距离（单位：米）")]
        public double distance { get; set; }

        [Description("路线规划")]
        public string steps { get; set; }
    }
}