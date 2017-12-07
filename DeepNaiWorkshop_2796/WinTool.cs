using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;

namespace DeepNaiWorkshop_2796
{   /**
     * windows 常用工具
     */
    class WinTool
    {

        // 取得设备硬盘的卷标号  
        public static string GetDiskVolumeSerialNumber()
        {
            //使用此类需要添加引用System.Management
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid='C:'");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        //获得CPU的序列号  
        public static string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        //生成机器码  
        public static string getMNum()
        {
            string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号  
            string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码  
            return strMNum;
        }
        public static int[] intCode = new int[127];//存储密钥  
        public static int[] intNumber = new int[25];//存机器码的Ascii值  
        public static char[] Charcode = new char[25];//存储机器码字  
        public static void setIntCode()//给数组赋值小于10的数  
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }


        /*
         * 生成注册码
         * 方法一
         * @param mNum 机器码
         */
        public static string getRNum(String mNum)
        {
            setIntCode();//初始化127位数组  
            for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中  
            {
                Charcode[i] = Convert.ToChar(mNum.Substring(i - 1, 1));
            }
            for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。  
            {
                intNumber[j] = intCode[Convert.ToInt32(Charcode[j])] + Convert.ToInt32(Charcode[j]);
            }
            string strAsciiName = "";//用于存储注册码  
            for (int j = 1; j < intNumber.Length; j++)
            {
                if (intNumber[j] >= 48 && intNumber[j] <= 57)//判断字符ASCII值是否0－9之间  
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 65 && intNumber[j] <= 90)//判断字符ASCII值是否A－Z之间  
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 97 && intNumber[j] <= 122)//判断字符ASCII值是否a－z之间  
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else//判断字符ASCII值不在以上范围内  
                {
                    if (intNumber[j] > 122)//判断字符ASCII值是否大于z  
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                    }
                    else
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                    }
                }
            }
            return strAsciiName;
        }



        // <summary>  
        // 检测是否注册  
        public bool CheckReg(String rNNum)
        {
            try
            {
                string str = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\mysoft", "SoftInfo", 0).ToString();
                if (string.IsNullOrEmpty(str) || (str != rNNum.Trim()))
                {
                    //MessageBox.Show("本软件未经许可，不能使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    //MessageBox.Show("本软件已经注册，不需重复注册！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            catch
            {
                //MessageBox.Show("本软件未经许可，不能使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

    }

}
