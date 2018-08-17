using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DeepNaiWorkshop_2796.MyModel
{
    public class TemplateConfig
    {
        public String BackImg { set; get; }//背景图片名称 xxx.jpg
        //
        public bool IsUseShopName { set; get; }//是否启用商品名称
        public String ShopNameFontColor { set; get; }//商家名称字体颜色 例如：#FFFFFF
        public String ShopNameFontType { set; get; }//字体类型 例如：宋体
        public int ShopNameSize { set; get; }//字体大小
        public int ShopNameFontX { set; get; }//左上角点x的位置
        public int ShopNameFontY { set; get; }//左上角点y的位置
        public int ShopNameFontWidth { set; get; }//画出的文字图片宽
        public int ShopNameFontHeight { set; get; }//画出的文字图片高



        //
        public bool IsUseShopSLT { set; get; }//是否使用商品缩略图
        public int ShopSLTX { set; get; }//商品缩略图左上角点x的坐标
        public int ShopSLTY { set; get; }//商品缩略图左上角点y的坐标
        public int ShopSLTWidth { set; get; }//商品缩略图宽
        public int ShopSLTHeight { set; get; }//商品缩略图高

        //
        public bool IsUseCouponValue { set; get; }//是否使用优惠券价格
        public String CouponValueFontColor { set; get; }//字体颜色 例如：#FFFFFF
        public String CouponValueFontType { set; get; }//字体类型 例如：宋体
        public int CouponValueSize { set; get; }//字体大小
        public int CouponValueFontX { set; get; }//左上角点x的位置
        public int CouponValueFontY { set; get; }//左上角点y的位置
        public int CouponValueFontWidth { set; get; }//画出的文字图片宽
        public int CouponValueFontHeight { set; get; }//画出的文字图片高


        //
        public bool IsUseCouponTime { set; get; }//是否使用优惠券价格
        public String CouponTimeFontColor { set; get; }//字体颜色 例如：#FFFFFF
        public String CouponTimeFontType { set; get; }//字体类型 例如：宋体
        public int CouponTimeSize { set; get; }//字体大小
        public int CouponTimeFontX { set; get; }//左上角点x的位置
        public int CouponTimeFontY { set; get; }//左上角点y的位置
        public int CouponTimeFontWidth { set; get; }//画出的文字图片宽
        public int CouponTimeFontHeight { set; get; }//画出的文字图片高


        //
        public bool IsUseGoodsName { set; get; }//是否使用商品名称
        public String GoodsNameFontColor { set; get; }//字体颜色 例如：#FFFFFF
        public String GoodsNameFontType { set; get; }//字体类型 例如：宋体
        public int GoodsNameSize { set; get; }//字体大小
        public int GoodsNameFontX { set; get; }//左上角点x的位置
        public int GoodsNameFontY { set; get; }//左上角点y的位置
        public int GoodsNameFontWidth { set; get; }//画出的文字图片宽
        public int GoodsNameFontHeight { set; get; }//画出的文字图片高



        //
        public bool IsUsePrice { set; get; }//是否使用现价
        public String PriceFontColor { set; get; }//字体颜色 例如：#FFFFFF
        public String PriceFontType { set; get; }//字体类型 例如：宋体
        public int PriceSize { set; get; }//字体大小
        public int PriceFontX { set; get; }//左上角点x的位置
        public int PriceFontY { set; get; }//左上角点y的位置
        public int PriceFontWidth { set; get; }//画出的文字图片宽
        public int PriceFontHeight { set; get; }//画出的文字图片高
        public bool PriceFontDelLine { set; get; }//是否增加删除线
        public bool PriceFontItalic { set; get; }//是否斜体
        public bool PriceFontBold { set; get; }//是否加粗显示


        //
        public bool IsUseVolume { set; get; }//是否使用成交量
        public String VolumeFontColor { set; get; }//字体颜色 例如：#FFFFFF
        public String VolumeFontType { set; get; }//字体类型 例如：宋体
        public int VolumeSize { set; get; }//字体大小
        public int VolumeFontX { set; get; }//左上角点x的位置
        public int VolumeFontY { set; get; }//左上角点y的位置
        public int VolumeFontWidth { set; get; }//画出的文字图片宽
        public int VolumeFontHeight { set; get; }//画出的文字图片高



        //
        public bool IsUsePriceAfter { set; get; }//是否使用成交量
        public String PriceAfterFontColor { set; get; }//字体颜色 例如：#FFFFFF
        public String PriceAfterFontType { set; get; }//字体类型 例如：宋体
        public int PriceAfterSize { set; get; }//字体大小
        public int PriceAfterFontX { set; get; }//左上角点x的位置
        public int PriceAfterFontY { set; get; }//左上角点y的位置
        public int PriceAfterFontWidth { set; get; }//画出的文字图片宽
        public int PriceAfterFontHeight { set; get; }//画出的文字图片高


    }
}
