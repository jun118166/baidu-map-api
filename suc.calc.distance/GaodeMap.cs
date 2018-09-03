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
        public string Truck(GaodeParams gdparams) {
            string apiUrl = "http://restapi.amap.com/v4/direction/truck";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("key", apiKey);
            param.Add("origin", gdparams.origin);
            param.Add("destination", gdparams.destination);
            return HttpUtils.DoGet(apiUrl, param);
        }
    }
}
