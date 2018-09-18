using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
    public class GaodeMap
    {
        public static string apiKey { get; set; }
        public static string Truck(GaodeParams gdparams)
        {
            string apiUrl = "https://restapi.amap.com/v4/direction/truck";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("key", apiKey);
            param.Add("origin", gdparams.origin);
            param.Add("destination", gdparams.destination);
            return HttpUtils.DoGet(apiUrl, param);
        }

        public static string Truck(string origin, string destination,string size)
        {
            string apiUrl = "https://restapi.amap.com/v4/direction/truck";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("key", apiKey);
            param.Add("origin", origin);
            param.Add("destination", destination);
            param.Add("size", size);
            return HttpUtils.DoGet(apiUrl, param);
        }
    }
}
