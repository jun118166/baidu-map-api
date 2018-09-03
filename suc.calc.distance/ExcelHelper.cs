using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace suc.calc.distance
{
    public class ExcelHelper
    {
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="fileName">文件路径</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn, string fileName)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentNullException(sheetName);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName);
            }
            var data = new DataTable();
            IWorkbook workbook = null;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0)
                {
                    workbook = new HSSFWorkbook(fs);
                }

                ISheet sheet = null;
                if (workbook != null)
                {
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);
                }
                if (sheet == null) return data;
                var firstRow = sheet.GetRow(0);
                //一行最后一个cell的编号 即总的列数
                int cellCount = firstRow.LastCellNum;
                int startRow;
                if (isFirstRowColumn)
                {
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        var cell = firstRow.GetCell(i);
                        var cellValue = cell.StringCellValue;
                        if (cellValue == null) continue;
                        var column = new DataColumn(cellValue);
                        data.Columns.Add(column);
                    }
                    startRow = sheet.FirstRowNum + 1;
                }
                else
                {
                    startRow = sheet.FirstRowNum;
                }
                //最后一列的标号
                var rowCount = sheet.LastRowNum;
                for (var i = startRow; i <= rowCount; ++i)
                {
                    var row = sheet.GetRow(i);
                    //没有数据的行默认是null
                    if (row == null) continue;
                    var dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        //同理，没有数据的单元格都默认是null
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    data.Rows.Add(dataRow);
                }

                return data;
            }
            catch (IOException ioex)
            {
                throw new IOException(ioex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <param name="filePath"></param>
        public static void RenderToExcel<T>(List<T> datas, string filePath)
        {
            MemoryStream ms = new MemoryStream();
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("导出数据");
            IRow headerRow = sheet.CreateRow(0);

            int rowIndex = 1, piIndex = 0;
            Type type = typeof(T);
            PropertyInfo[] pis = type.GetProperties();
            int pisLen = pis.Length;//减2是多了2个外键引用
            PropertyInfo pi = null;
            string displayName = string.Empty;
            while (piIndex < pisLen)
            {
                pi = pis[piIndex];
                //获取属性的描述 
                Object[] obs = pi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                foreach (DescriptionAttribute record in obs)
                {
                    displayName = record.Description;
                }
                if (!displayName.Equals(string.Empty))
                {//如果该属性指定了DisplayName，则输出
                    try
                    {
                        headerRow.CreateCell(piIndex).SetCellValue(displayName);
                    }
                    catch (Exception)
                    {
                        headerRow.CreateCell(piIndex).SetCellValue("");
                    }
                }
                piIndex++;
            }
            foreach (T data in datas)
            {
                piIndex = 0;
                IRow dataRow = sheet.CreateRow(rowIndex);
                while (piIndex < pisLen)
                {
                    pi = pis[piIndex];
                    try
                    {
                        dataRow.CreateCell(piIndex).SetCellValue(pi.GetValue(data, null).ToString());
                    }
                    catch (Exception)
                    {
                        dataRow.CreateCell(piIndex).SetCellValue("");
                    }
                    piIndex++;
                }
                rowIndex++;
            }
            workbook.Write(ms);
            FileStream dumpFile = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            ms.WriteTo(dumpFile);
            ms.Flush();
            ms.Position = 0;
            dumpFile.Close();
        }
    }
}
