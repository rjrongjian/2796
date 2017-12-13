using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796
{
    class ImageTool
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
    }
}
