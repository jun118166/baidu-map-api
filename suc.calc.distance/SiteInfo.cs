using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
    //网点信息
    public class SiteInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        public string sn { get; set; }
        /// <summary>
        /// 网点编码
        /// </summary>
        [Description("网点编号")]
        public string orgCode { get; set; }
        /// <summary>
        /// 网点名称
        /// </summary>
        [Description("网点名称")]
        public string orgName { get; set; }
        /// <summary>
        /// 网点地址
        /// </summary>
        [Description("地址")]
        public string address { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [Description("经度")]
        public double lng { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        [Description("纬度")]
        public double lat { get; set; }
    }
}
