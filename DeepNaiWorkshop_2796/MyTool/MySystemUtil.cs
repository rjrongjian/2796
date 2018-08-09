
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace www_52bang_site_enjoy.MyTool
{
    public class MySystemUtil
    {
        /// <summary>  
        /// 获取当前执行的dll的目录 例如： D:\dir\dir\
        /// </summary>  
        /// <returns></returns>  
        public static string GetPath()
        {
            /*
            string str = Assembly.GetExecutingAssembly().CodeBase;
            int start = 8;// 去除file:///  
            int end = str.LastIndexOf('/');// 去除文件名xxx.dll及文件名前的/  
            str = str.Substring(start, end - start);
            str = str + "/";
            str = Path.GetDirectoryName(str) + "\\";
            return str;
            */
            return Environment.CurrentDirectory + "\\";


        }
        /// <summary>
        /// 获取插件dll根目录，用于存放此dll应用信息
        /// </summary>
        /// <returns></returns>
        public static string GetDllRoot()
        {
            string dirPath = GetPath()+@"local\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
        /// <summary>
        /// 获取本地qq对话存储路径,每天一个
        /// </summary>
        /// <returns></returns>
        public static string GetQQDialogueDir()
        {
            string dirPath = GetDllRoot()+@"QQDialogueLog\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            dirPath = dirPath + DateTime.Now.ToString("yyyy-MM-dd") + @"\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            return dirPath;

        }
        /// <summary>
        /// 获取会员文件存放目录
        /// </summary>
        /// <returns></returns>
        public static string GetMemberPath()
        {
            string dirPath = GetDllRoot() + @"Member\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            return dirPath;
        }

        

        /// <summary>
        /// 获取配置文件的目录
        /// </summary>
        /// <returns></returns>
        public static string GetSystemConfigJsonPath()
        {
            string dirPath = GetDllRoot() + "SystemConfig.txt";
            return dirPath;
        }
        /// <summary>
        /// 获取会员文件完整路径
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetMemberPath(long qq)
        {
            return GetMemberPath() + qq + ".txt";
        }


        public static string GetBaseJsonPath()
        {
            return GetDllRoot() + "BaseJson.txt"; 
        }

        public static string GetMovieJsonPath()
        {
            return GetDllRoot() + "MovieConfig.txt"; 
        }

        public static string GetTemplateImgRoot()
        {
            string dirPath = GetPath() + @"Template\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
        /// <summary>
        /// 获取所有文件夹名称
        /// </summary>
        /// <returns></returns>
        public static List<String> GetAllTemplateNames()
        {
            List<String> list = new List<string>();
            String templateDir = GetTemplateImgRoot();
            Console.WriteLine("获取指定的目录："+ templateDir);
            DirectoryInfo root = new DirectoryInfo(templateDir);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                //Console.WriteLine("获取的文件夹名："+d.Name);
                list.Add(d.Name);
            }
            return list;
        }



    }
}
