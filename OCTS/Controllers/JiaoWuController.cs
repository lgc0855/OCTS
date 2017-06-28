using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCTS.Models;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Microsoft.Office.Interop.Excel;

namespace OCTS.Controllers
{
    public class JiaoWuController : Controller
    {

        private DBHelper dbHelper = new DBHelper();
        // GET: JiaoWu
        public ActionResult Index()
        {
            return View();
        }

        public string AddSemester(string startTime , string endTime)
        {
            char c = ' ';
            string[] temp = startTime.Split(c);
            int year = Convert.ToInt32(temp[2]);
            int month = Convert.ToInt32(temp[1]);
            int day = Convert.ToInt32(temp[0]);
            DateTime sTime = new DateTime(year,month,day);
            temp = endTime.Split(c);
            year = Convert.ToInt32(temp[2]);
            month = Convert.ToInt32(temp[1]);
            day = Convert.ToInt32(temp[0]);
            DateTime eTime = new DateTime(year,month,day);
            TimeSpan duration = eTime - sTime;
            int i = 0;
            TimeSpan basic = new TimeSpan(24, 0, 0);
            int result = duration.CompareTo(basic);
            if(result <=  0)
            {
                return "设置学期天数必须大于一天！";
            }
            day = duration.Days;
            string s = dbHelper.addSemester(sTime, eTime, day / 7 + 1);
            if(s.StartsWith("succeed"))
            return "成功设置学期！";

            return "设置学期失败！";
        }


        public string batchAddStudents(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            var savePath = Path.Combine(severPath, file.FileName);
            string result = "{}";
            try
            {
                if (string.Empty.Equals(file.FileName) || ".xlsx" != Path.GetExtension(file.FileName))
                {
                    return "error";
                }

                file.SaveAs(savePath);
                //启动Excel应用程序
                Microsoft.Office.Interop.Excel.Application xls = new Microsoft.Office.Interop.Excel.Application();
                //打开filename表
                _Workbook book = xls.Workbooks.Open(savePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                _Worksheet sheet;//定义sheet变量
                xls.Visible = false;//设置Excel后台运行
                xls.DisplayAlerts = false;//设置不显示确认修改提示

                try
                {
                    sheet = (_Worksheet)book.Worksheets.get_Item(1);//获得第index个sheet，准备读取
                }
                catch (Exception ex)//不存在就退出
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                Console.WriteLine(sheet.Name);
                int row = sheet.UsedRange.Rows.Count;//获取不为空的行数
                int col = sheet.UsedRange.Columns.Count;//获取不为空的列数
                // Array value = (Array)sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[row, col]).Cells.Value2;//获得区域数据赋值给Array数组，方便读取\
                string tempId;
                string tempName;
                int idcol = -1;
                int idrow = -1;
                int namecol = -1;
                for (var i = 1; i <= row; i++)
                {
                    for (var j = 1; j <= col; j++)
                    {
                        tempId = ((Range)sheet.Cells[i, j]).Text;
                        if (tempId.Equals("学号"))
                        {
                            idcol = j;
                            idrow = i;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                    }

                    if (idcol >= 0 && idrow >= 0)
                    {
                        break;
                    }
                }

                UserDBContext userdb = new UserDBContext();

                for (var i = idrow + 1; i <= row; i++)
                {
                    tempId = ((Range)sheet.Cells[i, idcol]).Text;
                    tempName = ((Range)sheet.Cells[i, namecol]).Text;
                    if (isInt(tempId) && tempName != "")
                    {
                        User user = new Models.User();
                        user.userId = tempId;
                        user.userName = tempName;
                        user.setUserType("student");
                        user.userPassword = "8CB2237D0679CA88DB6464EAC60DA96345513964";
                        userdb.users.Add(user);
                    }
                }

                userdb.SaveChanges();


                book.Save();//保存
                book.Close(false, Missing.Value, Missing.Value);//关闭打开的表
                xls.Quit();//Excel程序退出
                //sheet,book,xls设置为null，防止内存泄露
                sheet = null;
                book = null;
                xls = null;
                GC.Collect();//系统回收资源
                System.IO.File.Delete(savePath);
                result = "{}";
            }
            catch (Exception e)
            {
                result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
            }
            finally
            {
                System.IO.File.Delete(savePath);
            }
            return result;
        }


        public string batchAddTeachers(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            var savePath = Path.Combine(severPath, file.FileName);
            string result = "{}";
            try
            {
                if (string.Empty.Equals(file.FileName) || ".xlsx" != Path.GetExtension(file.FileName))
                {
                    return "error";
                }

                file.SaveAs(savePath);
                //启动Excel应用程序
                Microsoft.Office.Interop.Excel.Application xls = new Microsoft.Office.Interop.Excel.Application();
                //打开filename表
                _Workbook book = xls.Workbooks.Open(savePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                _Worksheet sheet;//定义sheet变量
                xls.Visible = false;//设置Excel后台运行
                xls.DisplayAlerts = false;//设置不显示确认修改提示

                try
                {
                    sheet = (_Worksheet)book.Worksheets.get_Item(1);//获得第index个sheet，准备读取
                }
                catch (Exception ex)//不存在就退出
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                Console.WriteLine(sheet.Name);
                int row = sheet.UsedRange.Rows.Count;//获取不为空的行数
                int col = sheet.UsedRange.Columns.Count;//获取不为空的列数
                // Array value = (Array)sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[row, col]).Cells.Value2;//获得区域数据赋值给Array数组，方便读取\
                string tempId;
                string tempName;
                int idcol = -1;
                int idrow = -1;
                int namecol = -1;
                for (var i = 1; i <= row; i++)
                {
                    for (var j = 1; j <= col; j++)
                    {
                        tempId = ((Range)sheet.Cells[i, j]).Text;
                        if (tempId.Equals("工号"))
                        {
                            idcol = j;
                            idrow = i;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                    }

                    if (idcol >= 0 && idrow >= 0)
                    {
                        break;
                    }
                }

                UserDBContext userdb = new UserDBContext();

                for (var i = idrow + 1; i <= row; i++)
                {
                    tempId = ((Range)sheet.Cells[i, idcol]).Text;
                    tempName = ((Range)sheet.Cells[i, namecol]).Text;
                    if (isInt(tempId) && tempName != "")
                    {
                        User user = new Models.User();
                        user.userId = tempId;
                        user.userName = tempName;
                        user.setUserType("teacher");
                        user.userPassword = "8CB2237D0679CA88DB6464EAC60DA96345513964";
                        userdb.users.Add(user);
                    }
                }

                userdb.SaveChanges();


                book.Save();//保存
                book.Close(false, Missing.Value, Missing.Value);//关闭打开的表
                xls.Quit();//Excel程序退出
                //sheet,book,xls设置为null，防止内存泄露
                sheet = null;
                book = null;
                xls = null;
                GC.Collect();//系统回收资源
                System.IO.File.Delete(savePath);
                result = "{}";
            }
            catch (Exception e)
            {
                result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
            }
            finally
            {
                System.IO.File.Delete(savePath);
            }
            return result;
        }

        private bool isInt(string s)
        {
            int i = 0 ;
            try
            {
                i = int.Parse(s);
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}