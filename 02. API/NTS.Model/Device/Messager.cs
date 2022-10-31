using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Device
{
    public class Messager
    {
        public string DeviceCode { get; set; }
        public string DeviceIP { get; set; } 
        public string CMDCode { get; set; } 
        public List<int> Slots { get; set; } 
        public string DoorStatus { get; set; }
        public string Temp { get; set; }
        public string ReaderStatus { get; set; }
        /// <summary>
        /// số tiền trả lại còn trong khay
        /// </summary>
        public string ReturnMoney { get; set; }
        /// <summary>
        ///  giá tiền trả lại
        /// </summary>
        public string PriceReturnMoney { get; set; }
        public List<int> SlotsMoney { get; set; }
        public string LedStatus { get; set; }
        /// <summary>
        /// số tiền dư lần mua trước - frame e
        /// </summary>
        public int MoneyResidual { get; set;}
        /// <summary>
        /// số tiền mua - frame e
        /// </summary>
        public int MoneyBuy { get; set; }
        /// <summary>
        /// ô được chọn - frame e
        /// </summary>
        public int SlotsBuySelect { get; set; }
        /// <summary>
        ///  giá ô được chọn - frame e
        /// </summary>
        public int PriceBuySelect { get; set; }
        /// <summary>
        ///  số tiền dư sau 1 lần mua - frame e,f
        /// </summary>
        public int MoneyResidualAfterBuy { get; set; }
        /// <summary>
        /// số tiền máy trả lại cho người mua - frame e,f
        /// </summary>
        public int MoneyBuyReturn { get; set; }
        /// <summary>
        /// ô lỗi - frame f
        /// </summary>
        public int SlotError { get; set; }
        /// <summary>
        ///  // giá tiền của ô lỗi - frame f
        /// </summary>
        public int PriceSlotError { get; set; }
        /// <summary>
        ///  // ô mua mới lần 1 - frame f
        /// </summary>
        public int SlotsBuySelectE1 { get; set; }
        public int PriceBuySelectE1 { get; set; }
        /// <summary>
        ///  // ô mua mới lần 2 - frame f
        /// </summary>
        public int SlotsBuySelectE2 { get; set; }
        public int PriceBuySelectE2 { get; set; }
        public DateTime? TimeDateEnd { get; set; } //thời gian kết thúc frame mua lỗi - frame f

        public int Revenue { get; set; }// - frame h
        public int TotalErrorInDay { get; set; }// - frame h
        public DateTime? RevenueDate { get; set; }// - frame h

        public int ErrorCode { get; set; }
        public int FixErrorCode { get; set; }
        public string SensorStatus { get; set; }

        public DateTime? TimeDate { get; set; }
        public string TimeAction { get; set; }

        public bool Error { get; set; }
    }
}
