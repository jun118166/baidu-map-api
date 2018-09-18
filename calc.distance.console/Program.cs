using suc.calc.distance;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;

namespace calc.distance.console
{
    class Program
    {
        private static string filePath { get; set; }

        private static List<SiteDistance> sourceList { get; set; }

        private static List<SiteDistance> destList { set; get; }

        private static DateTime startTime { get; set; }

        private static int pageSize { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("请确认是否阅读了当前目录System.xml文件?是（y）否（n）");
            string print = Console.ReadLine();
            if (!print.ToUpper().Equals("Y"))
            {
                Console.WriteLine("请重新阅读了当前目录System.xml文件");
                return;
            }

            startTime = DateTime.Now;
            filePath = ReadXml("filePath");
  
            BaiduMap.apiKey = ReadXml("baiduKey");

            GaodeMap.apiKey = ReadXml("gaodeKey");
            //打开文件
            List<SiteInfo> lstSite = OpenFile(filePath);

            //解析文件出list
            sourceList = Combination(lstSite);

            CalcDistance();

            Console.ReadKey();
        }

        /// <summary>
        /// 两两组合去除重复项
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        private static List<SiteDistance> Combination(List<SiteInfo> sourceList)
        {
            List<SiteDistance> d = new List<SiteDistance>();
            foreach (var site in sourceList)
            {
                foreach (var item in sourceList)
                {
                    if (!site.Equals(item))
                    {
                        SiteDistance distance = new SiteDistance();
                        distance.startCode = site.orgCode;
                        distance.startName = site.orgName;
                        distance.startAddress = site.address;
                        distance.startlat = site.lat;
                        distance.startlng = site.lng;
                        distance.endCode = item.orgCode;
                        distance.endName = item.orgName;
                        distance.endAddress = item.address;
                        distance.endlat = item.lat;
                        distance.endlng = item.lng;
                        d.Add(distance);
                    }

                }
            }
            return d;
        }

        private static void CalcDistance()
        {
            int threadCount = 10;
            pageSize = sourceList.Count / threadCount + 1;
            destList = new List<SiteDistance>();
            var waits = new List<EventWaitHandle>();
            for (int i = 0; i < threadCount; i++)
            {
                var handler = new ManualResetEvent(false);
                waits.Add(handler);
                new Thread(new ParameterizedThreadStart(calcDistance))
                {
                    Name = "thread" + i.ToString()
                }.Start(new Tuple<int, EventWaitHandle>(i, handler));
            }
            WaitHandle.WaitAll(waits.ToArray());

            expExcel();
            Console.WriteLine("输出完成——》共生成" + destList.Count + "条数据，耗时：" + DateTime.Now.Subtract(startTime).TotalSeconds);
            Console.ReadKey();
        }

        private static void calcDistance(object param)
        {
            var p = (Tuple<int, EventWaitHandle>)param;
            //Console.WriteLine(Thread.CurrentThread.Name + ": Begin!");
            int start = int.Parse(p.Item1.ToString()) * pageSize;

            for (int i = start; i < start + pageSize; i++)
            {
                if (i + 1 > sourceList.Count)
                {
                    break;
                }
                SiteDistance item = sourceList[i];
                if (item.startCode != item.endCode)
                {


                    #region 百度部分
                    //Geocoder result = JsonHelper.JsonDeserialize<Geocoder>(BaiduMap.Driving(item.startlat.ToString() + "," + item.startlng.ToString(), item.endlat.ToString() + "," + item.endlng.ToString()));                    
                    //if (result != null && result.result != null && result.result.routes != null && result.result.routes.Count > 0)
                    //{
                    //    item.distance = result.result.routes[0].distance;
                    //    item.runTime = result.result.routes[0].duration / 60;
                    //    item.steps = GetSteps(result.result.routes[0].steps);
                    //}
                    #endregion
                    string result = GaodeMap.Truck(item.startlng.ToString() + "," + item.startlat.ToString(), item.endlng.ToString() + "," + item.endlat.ToString(), "3");
                    GaodeResult gdResult = JsonHelper.JsonDeserialize<GaodeResult>(result);
                    if (gdResult != null && "0".Equals(gdResult.errcode))
                    {
                        item.distance = gdResult.data.route.paths[0].distance;
                        item.runTime = gdResult.data.route.paths[0].duration / 60;
                        item.steps = GetSteps(gdResult.data.route.paths[0].steps);
                    }
                }
                else
                {
                    item.distance = 0;
                    item.runTime = 0;
                }
                item.sn = i;
                destList.Add(item);
                Console.WriteLine("当前运算进度:" + ((decimal)destList.Count / sourceList.Count * 100).ToString("#0.00") + "%-----》正在获取" + item.startName + "===" + item.endName + "信息\n");
            }
            p.Item2.Set();
        }

        private static string GetSteps(List<Step> lstStep)
        {
            string s = string.Empty;
            foreach (var item in lstStep)
            {
                s += item.road_name + "->";
            }
            return s;
        }

        private static string GetSteps(List<GaodeStep> lstStep)
        {
            string s = string.Empty;
            foreach (var item in lstStep)
            {
                s += item.instruction + "->";
            }
            return s;
        }

        /// <summary>
        /// 打开网点信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static List<SiteInfo> OpenFile(String filePath)
        {
            DataTable dt = ExcelHelper.ExcelToDataTable("输入", true, filePath);
            List<SiteInfo> lstSite = ConvertHelper<SiteInfo>.ConvertToList(dt);
            foreach (var site in lstSite)
            {
                BaiduResult geocoder = JsonHelper.JsonDeserialize<BaiduResult>(BaiduMap.Geocoder(site.address));
                site.lat = geocoder.result.location.lat;
                site.lng = geocoder.result.location.lng;
            }
            return lstSite;
        }

        private static string ReadXml(string key)
        {
            //创建Xml文档。
            XmlDocument xmlDocument = new XmlDocument();
            //加载要读取的xml文件。
            xmlDocument.Load("System.xml");
            XmlNodeList nodeList = xmlDocument.SelectNodes("/configuration/" + key);
            //循环遍历。
            foreach (XmlNode item in nodeList)
            {
                System.Console.WriteLine(string.Format("读取{0}值:{1}", key, item.Attributes["value"].Value));
                return item.Attributes["value"].Value;
            }
            return "";
        }
        private static void expExcel()
        {
            string outfilePath = filePath.Split('.')[0];
            ExcelHelper.RenderToExcel<SiteDistance>(destList, outfilePath + DateTime.Now.ToString("yyyyMMddHHmmss") + "_输出文档.xls");
        }
    }
}
