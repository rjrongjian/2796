using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyTools
{
    public class ImageTool
    {
        public static Image getImageBy(String imgUrl)
        {
            System.Net.WebRequest webreq = System.Net.WebRequest.Create(imgUrl);
            System.Net.WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            Image image;
            image = Image.FromStream(stream);
            stream.Close();
            return image;
        }

        /// <summary>
        /// 读取本地图片
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static Image getLocalImageBy(String imgPath)
        {
            Image img = null;
            Stream s = File.Open(imgPath, FileMode.Open);
            img = Image.FromStream(s);
            s.Close();
            return img;
        }

        /// <summary>
        /// 判断当前图片路径是否有效，且是网络图片还是本地图片
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <returns>0 不合法 1 网络图片 2 本地图片</returns>
        public static int isLegalOfImgPath(string imgPath)
        {
            if (String.IsNullOrWhiteSpace(imgPath))
            {
                return 0;
            }
            //网络图片
            Regex reg2 = new Regex("[http://|https://][\\s\\S]+");
            Match match2 = reg2.Match(imgPath);
            if (match2.Success)
            {
                return 1;
            }
            Regex reg = new Regex("[A-Za-z]:[\\s\\S]+[.jpg|.jpeg|.gif|.png|.bmp]");
            Match match = reg.Match(imgPath);

            if (match.Success)
            {
                return 2;
            }
            

            return 0;

        }
        /// <summary>
        /// 重新设置图片大小
        /// </summary>
        /// <param name="oriImg">原始图片</param>
        /// <param name="width">更改后的图宽</param>
        /// <param name="height">更改后的图高</param>
        /// <returns>返回一个调整大小后的克隆对象</returns>
        public static Image resetImgSize(Image oriImg,int width,int height)
        {
            Image img = (Image)oriImg.Clone();
            return new Bitmap(img, width, height);

        }


        /// <summary>
        /// 图片添加任意角度文字(文字旋转是中心旋转，角度顺时针为正)
        /// </summary>
        /// <param name="originalImg">原始图片</param>
        /// <param name="locationLeftTop">文字左上角定位(x1,y1)</param>
        /// <param name="fontSize">字体大小，单位为像素</param>
        /// <param name="text">文字内容</param>
        /// <param name="angle">文字旋转角度</param>
        /// <param name="fontName">字体名称</param>
        /// <returns>添加文字后的Bitmap对象</returns>
        public static Bitmap AddText(Image originalImg, string locationLeftTop, int fontSize, string text, int angle = 0, string fontName = "华文行楷")
        {
            Image img = originalImg;
            int width = img.Width;
            int height = img.Height;
            Bitmap bmp = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bmp);
            // 画底图
            graphics.DrawImage(img, 0, 0, width, height);
            Font font = new Font(fontName, fontSize, GraphicsUnit.Pixel);
            SizeF sf = graphics.MeasureString(text, font); // 计算出来文字所占矩形区域
                                                           // 左上角定位
            string[] location = locationLeftTop.Split(',');
            float x1 = float.Parse(location[0]);
            float y1 = float.Parse(location[1]);
            // 进行文字旋转的角度定位
            if (angle != 0)
            {
                #region 法一：TranslateTransform平移 + RotateTransform旋转
                /* 
                 * 注意：
                 * Graphics.RotateTransform的旋转是以Graphics对象的左上角为原点，旋转整个画板的。
                 * 同时x，y坐标轴也会跟着旋转。即旋转后的x，y轴依然与矩形的边平行
                 * 而Graphics.TranslateTransform方法，是沿着x，y轴平移的
                 * 因此分三步可以实现中心旋转
                 * 1.把画板(Graphics对象)平移到旋转中心
                 * 2.旋转画板
                 * 3.把画板平移退回相同的距离(此时的x，y轴仍然是与旋转后的矩形平行的)
                 */
                //// 把画板的原点(默认是左上角)定位移到文字中心
                //graphics.TranslateTransform(x1 + sf.Width / 2, y1 + sf.Height / 2);
                //// 旋转画板
                //graphics.RotateTransform(angle);
                //// 回退画板x,y轴移动过的距离
                //graphics.TranslateTransform(-(x1 + sf.Width / 2), -(y1 + sf.Height / 2));
                #endregion
                #region 法二：矩阵旋转
                Matrix matrix = graphics.Transform;
                matrix.RotateAt(angle, new PointF(x1 + sf.Width / 2, y1 + sf.Height / 2));
                graphics.Transform = matrix;
                #endregion
            }
            // 写上自定义角度的文字
            graphics.DrawString(text, font, new SolidBrush(Color.Black), x1, y1);
            graphics.Dispose();
            img.Dispose();
            return bmp;
        }

        public static Bitmap AddText(Image originalImg, string locationLeftTop, Font fontStyle, SolidBrush solidBrush, string text, int angle = 0, string fontName = "华文行楷")
        {
            Image img = originalImg;
            int width = img.Width;
            int height = img.Height;
            Bitmap bmp = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bmp);
            // 画底图
            graphics.DrawImage(img, 0, 0, width, height);
            Font font = fontStyle;
            SizeF sf = graphics.MeasureString(text, font); // 计算出来文字所占矩形区域
                                                           // 左上角定位
            string[] location = locationLeftTop.Split(',');
            float x1 = float.Parse(location[0]);
            float y1 = float.Parse(location[1]);
            // 进行文字旋转的角度定位
            if (angle != 0)
            {
                #region 法一：TranslateTransform平移 + RotateTransform旋转
                /* 
                 * 注意：
                 * Graphics.RotateTransform的旋转是以Graphics对象的左上角为原点，旋转整个画板的。
                 * 同时x，y坐标轴也会跟着旋转。即旋转后的x，y轴依然与矩形的边平行
                 * 而Graphics.TranslateTransform方法，是沿着x，y轴平移的
                 * 因此分三步可以实现中心旋转
                 * 1.把画板(Graphics对象)平移到旋转中心
                 * 2.旋转画板
                 * 3.把画板平移退回相同的距离(此时的x，y轴仍然是与旋转后的矩形平行的)
                 */
                //// 把画板的原点(默认是左上角)定位移到文字中心
                //graphics.TranslateTransform(x1 + sf.Width / 2, y1 + sf.Height / 2);
                //// 旋转画板
                //graphics.RotateTransform(angle);
                //// 回退画板x,y轴移动过的距离
                //graphics.TranslateTransform(-(x1 + sf.Width / 2), -(y1 + sf.Height / 2));
                #endregion
                #region 法二：矩阵旋转
                Matrix matrix = graphics.Transform;
                matrix.RotateAt(angle, new PointF(x1 + sf.Width / 2, y1 + sf.Height / 2));
                graphics.Transform = matrix;
                #endregion
            }
            // 写上自定义角度的文字
            graphics.DrawString(text, font, solidBrush, x1, y1);
            graphics.Dispose();
            //img.Dispose();
            return bmp;
        }

        /// <summary>
        /// 图片添加任意角度图片
        /// </summary>
        /// <param name="originalImg"></param>
        /// <param name="locationLeftTop"></param>
        /// <param name="fontStyle"></param>
        /// <param name="solidBrush"></param>
        /// <param name="text"></param>
        /// <param name="angle"></param>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static Bitmap AddImg(Image originalImg, string locationLeftTop, Image watermark, int angle = 0, string fontName = "华文行楷")
        {
            Image img = originalImg;
            int width = img.Width;
            int height = img.Height;
            Bitmap bmp = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bmp);
            // 画底图
            graphics.DrawImage(img, 0, 0, width, height);
            //Font font = fontStyle;
            SizeF sf = new SizeF(watermark.Width,watermark.Height); // 计算出来文字所占矩形区域
                                                           // 左上角定位
            string[] location = locationLeftTop.Split(',');
            float x1 = float.Parse(location[0]);
            float y1 = float.Parse(location[1]);
            // 进行文字旋转的角度定位
            if (angle != 0)
            {
                #region 法一：TranslateTransform平移 + RotateTransform旋转
                /* 
                 * 注意：
                 * Graphics.RotateTransform的旋转是以Graphics对象的左上角为原点，旋转整个画板的。
                 * 同时x，y坐标轴也会跟着旋转。即旋转后的x，y轴依然与矩形的边平行
                 * 而Graphics.TranslateTransform方法，是沿着x，y轴平移的
                 * 因此分三步可以实现中心旋转
                 * 1.把画板(Graphics对象)平移到旋转中心
                 * 2.旋转画板
                 * 3.把画板平移退回相同的距离(此时的x，y轴仍然是与旋转后的矩形平行的)
                 */
                //// 把画板的原点(默认是左上角)定位移到文字中心
                //graphics.TranslateTransform(x1 + sf.Width / 2, y1 + sf.Height / 2);
                //// 旋转画板
                //graphics.RotateTransform(angle);
                //// 回退画板x,y轴移动过的距离
                //graphics.TranslateTransform(-(x1 + sf.Width / 2), -(y1 + sf.Height / 2));
                #endregion
                #region 法二：矩阵旋转
                Matrix matrix = graphics.Transform;
                matrix.RotateAt(angle, new PointF(x1 + sf.Width / 2, y1 + sf.Height / 2));
                graphics.Transform = matrix;
                #endregion
            }
            // 写上自定义角度的文字
            //graphics.DrawString(text, font, new SolidBrush(Color.Black), x1, y1);
            graphics.DrawImage(watermark,x1,y1);
            graphics.Dispose();
            //img.Dispose();
            return bmp;
        }

    }
}
