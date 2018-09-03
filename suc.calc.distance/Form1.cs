using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace suc.calc.distance
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        private string filePath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            filePath = "D:\\模板.xlsx";
            try
            {
                BaiduMap.apiKey = txtBaiduAk.Text;
                //打开文件
                List<SiteInfo> lstSite = OpenFile(filePath);
                //解析文件出list
                List<SiteDistance> lstDistance = Combination(lstSite);
                Application.DoEvents();
                //生成集合
                //调用百度api计算距离，时间
                List<SiteDistance> destDistance = CalcDistance(lstDistance);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Form1), ex);
                MessageBox.Show(ex.Message);
            }


        }

        /// <summary>
        /// 打开网点信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private List<SiteInfo> OpenFile(String filePath)
        {
            DataTable dt = ExcelHelper.ExcelToDataTable("输入", true, filePath);
            List<SiteInfo> lstSite = ConvertHelper<SiteInfo>.ConvertToList(dt);
            foreach (var site in lstSite)
            {
                Geocoder geocoder = JsonHelper.JsonDeserialize<Geocoder>(BaiduMap.Geocoder(site.address));
                site.lat = geocoder.result.location.lat;
                site.lng = geocoder.result.location.lng;
            }
            return lstSite;
        }

        /// <summary>
        /// 两两组合去除重复项
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        private List<SiteDistance> Combination(List<SiteInfo> sourceList)
        {
            List<SiteDistance> destList = new List<SiteDistance>();
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
                        destList.Add(distance);
                    }

                }
            }
            return destList;
        }

        List<SiteDistance> sorceList { get; set; }

        List<SiteDistance> deslist { get; set; }
        /// <summary>
        /// 调用百度api计算距离
        /// </summary>
        /// <param name="sorceList"></param>
        /// <returns></returns>
        private List<SiteDistance> CalcDistance(List<SiteDistance> sorceList)
        {
            this.sorceList = sorceList;
            deslist = new List<SiteDistance>();

            pgb.Maximum = sorceList.Count;
            pgb.Value = sorceList.Count;


            var waits = new List<EventWaitHandle>();
            for (int i = 0; i < 10; i++)
            {
                var handler = new ManualResetEvent(false);
                waits.Add(handler);
                new Thread(new ParameterizedThreadStart(calcDistance))
                {
                    Name = "thread" + i.ToString()
                }.Start(new Tuple<int, EventWaitHandle>(i, handler));
            }
            WaitHandle.WaitAll(waits.ToArray());
            expExcel(deslist);
            //for (int i = 0; i < sorceList.Count; i++)
            //{
            //    SiteDistance item = sorceList[i];
            //    if (item.startCode != item.endCode)
            //    {
            //        Geocoder result = JsonHelper.JsonDeserialize<Geocoder>(BaiduMap.Driving(item.startlat.ToString() + "," + item.startlng.ToString(), item.endlat.ToString() + "," + item.endlng.ToString()));
            //        if (result != null && result.result != null && result.result.routes != null && result.result.routes.Count > 0)
            //        {
            //            item.distance = result.result.routes[0].distance;
            //            item.runTime = result.result.routes[0].duration / 60;
            //            item.steps = GetSteps(result.result.routes[0].steps);
            //        }

            //    }
            //    else
            //    {
            //        item.distance = 0;
            //        item.runTime = 0;
            //    }
            //}
            return deslist;
        }

        private void calcDistance(object param)
        {
            var p = (Tuple<int, EventWaitHandle>)param;

            int start = int.Parse(p.Item1.ToString()) * 2000;

            for (int i = start; i < start + 2000; i++)
            {
                if (i+1 > sorceList.Count)
                {
                    break;
                }
                SiteDistance item = sorceList[i];
                if (item.startCode != item.endCode)
                {
                    Geocoder result = JsonHelper.JsonDeserialize<Geocoder>(BaiduMap.Driving(item.startlat.ToString() + "," + item.startlng.ToString(), item.endlat.ToString() + "," + item.endlng.ToString()));
                    if (result != null && result.result != null && result.result.routes != null && result.result.routes.Count > 0)
                    {
                        item.distance = result.result.routes[0].distance;
                        item.runTime = result.result.routes[0].duration / 60;
                        item.steps = GetSteps(result.result.routes[0].steps);
                    }

                }
                else
                {
                    item.distance = 0;
                    item.runTime = 0;
                }
                item.sn = i;
                deslist.Add(item);
                LogHelper.WriteLog(typeof(Form1), deslist.Count.ToString());
                if (deslist.Count == sorceList.Count)
                {
                    expExcel(deslist);
                }
            }
          
        }

        private string GetSteps(List<Step> lstStep)
        {
            string s = string.Empty;
            foreach (var item in lstStep)
            {
                s += item.road_name + "->";
            }
            return s;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.xls|*.xlsx";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = ofd.FileName;
                filePath = ofd.FileName;
            }
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="sourceList"></param>
        private void expExcel(List<SiteDistance> sourceList)
        {
            //FolderBrowserDialog folder = new FolderBrowserDialog();
            //if (folder.ShowDialog() == DialogResult.OK)
            //{
            string outfilePath = filePath.Split('.')[0];
            ExcelHelper.RenderToExcel<SiteDistance>(sourceList, outfilePath + "输出文档.xls");
            //}

        }
    }
}