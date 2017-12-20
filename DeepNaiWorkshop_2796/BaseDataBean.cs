using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepNaiWorkshop_2796
{
    class BaseDataBean
    {
        private String _name;//商品标题
        private Image _mainPic;//商品主图
        private String _mainPicStr;//商品主图路径
        private String _startDateOfCoupon;//优惠券开始日期 格式：2017.12.07
        private String _endDateOfCoupon;//优惠券结束日期
        private double _price;//商品价格
        private int _couponValue;//券价值
        private String _shopName;//店铺名称
        private int _volume;//成交量
        private Image _shopPic;//店铺图标
        private List<String> _commentariesList;//商品评论
        private String _spuid;//
        private String _sellerId;//商家id
        private String _itemId;//商品id
        private String _rateUrl;//评论网址


        public string Name { get => _name; set => _name = value; }
        public Image MainPic { get => _mainPic; set => _mainPic = value; }
        public string StartDateOfCoupon { get => _startDateOfCoupon; set => _startDateOfCoupon = value; }
        public string EndDateOfCoupon { get => _endDateOfCoupon; set => _endDateOfCoupon = value; }
        public double Price { get => _price; set => _price = value; }
        public int CouponValue { get => _couponValue; set => _couponValue = value; }
        public string ShopName { get => _shopName; set => _shopName = value; }
        public int Volume { get => _volume; set => _volume = value; }
        public Image ShopPic { get => _shopPic; set => _shopPic = value; }
        public List<string> CommentariesList { get => _commentariesList; set => _commentariesList = value; }
        public string MainPicStr { get => _mainPicStr; set => _mainPicStr = value; }
        public string Spuid { get => _spuid; set => _spuid = value; }
        public string SellerId { get => _sellerId; set => _sellerId = value; }
        public string ItemId { get => _itemId; set => _itemId = value; }
        public string RateUrl { get => _rateUrl; set => _rateUrl = value; }
    }
}
