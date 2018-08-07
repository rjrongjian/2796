using DeepNaiWorkshop_6001.MyTool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using www_52bang_site_enjoy.MyTool;

namespace DeepNaiWorkshop_2796.MyTool
{
    public class MyDateUtil
    {

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(System.DateTime time, int length = 13)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString().Substring(0, length);
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetDateTimeFrom1970Ticks(long curSeconds)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(curSeconds);
        }

        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="interval">差值（分钟）</param>
        /// <returns></returns>
        public static bool IsTime(long time, double interval)
        {
            DateTime dt = GetDateTimeFrom1970Ticks(time);
            //取现在时间
            DateTime dt1 = DateTime.Now.AddMinutes(interval);
            DateTime dt2 = DateTime.Now.AddMinutes(interval * -1);
            if (dt > dt2 && dt < dt1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断时间戳是否正确（验证前8位）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsTime(string time)
        {
            string str = GetTimeStamp(DateTime.Now, 8);
            if (str.Equals(time))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string getCurrentDate()
        {
           return DateTime.Now.ToString();
        }

        public static long GetServerTime()
        {
            try
            {

                String content = HttpCodeUtil.GetRequest("http://api.m.taobao.com/rest/api3.do?api=mtop.common.getTimestamp", null);
                if (!string.IsNullOrWhiteSpace(content))//解析此时间 //返回格式 { "api":"mtop.common.getTimestamp","v":"*","ret":["SUCCESS::接口调用成功"],"data":{"t":"1524193088907"}}
                {
                    if (content.Contains("SUCCESS"))
                    {
                        JObject dynamicObject = (JObject)JsonConvert.DeserializeObject(content);
                        if (dynamicObject["data"] != null && dynamicObject["data"]["t"] != null)
                        {
                            return Convert.ToInt64(dynamicObject["data"]["t"]);
                        }
                    }
                }
                //如果执行到这里，说明第二种获取时间的方式失效
                content = HttpCodeUtil.GetRequest("http://quan.suning.com/getSysTime.do", null);//{"sysTime1":"20180420110128","sysTime2":"2018-04-20 11:01:28"}
                if (content.Contains("sysTime2"))//说明获取时间成功 
                {
                    JObject dynamicObject = (JObject)JsonConvert.DeserializeObject(content);
                    if (dynamicObject["sysTime2"] != null && dynamicObject["sysTime2"].ToString().Contains("-"))
                    {
                        return Convert.ToInt64(MyDateUtil.GetTimeStamp(Convert.ToDateTime(dynamicObject["sysTime2"].ToString())));
                    }
                }
                //如果执行到这里，说明第三种获取时间的方式失效
                content = HttpCodeUtil.GetRequest("http://cgi.im.qq.com/cgi-bin/cgi_svrtime", null);//2018-04-20 11:02:37
                if (!string.IsNullOrWhiteSpace(content))
                {
                    return Convert.ToInt64(MyDateUtil.GetTimeStamp(Convert.ToDateTime(content)));
                }
               
            }catch(Exception ex)
            {
                MyLogUtil.ErrToLog("获取系统消息失败，原因："+ex);
                return 0;
            }
            return 0;
        }
    }
}
