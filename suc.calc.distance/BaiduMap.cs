using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
    public class BaiduMap
    {
        public static string apiKey { get; set; }
        /// <summary>
        /// 根据地址获取经纬度信息
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string Geocoder(string address)
        {
            string apiUrl = "http://api.map.baidu.com/geocoder/v2/";
            string output = "json";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ak", apiKey);
            param.Add("output", output);
            param.Add("address", address);
            return HttpUtils.DoGet(apiUrl, param);
        }
        /// <summary>
        /// 路线规划
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static string Direction(string origin, string destination)
        {
            string apiUrl = "http://api.map.baidu.com/direction/v1/driving";
            string output = "json";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ak", apiKey);
            param.Add("output", output);
            //param.Add("origin", origin);
            //param.Add("destination", destination);
            param.Add("origin", origin);
            param.Add("destination", destination);
            param.Add("origin_region", "广州");
            param.Add("destination_region", "广州");
            return HttpUtils.DoGet(apiUrl, param);
        }

        /// <summary>
        /// 驾车路线规划
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static string Driving(string origin, string destination)
        {
            string apiUrl = "http://api.map.baidu.com/direction/v2/driving";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ak", apiKey);
            param.Add("origin", origin);
            param.Add("destination", destination);
            //param.Add("tactics", "8");
            return HttpUtils.DoGet(apiUrl, param);
        }
    }
}
