using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendingMachinesSystem.Model;

namespace VendingMachinesSystem.Api.Models
{
    public class VMSDVDeviceSoldLever2Model:Common.CommonModel
    {
        public string CustomerDeviceId { get; set; }
        //thiet bi
        public string DeviceId { get; set; }
        public string DeviceTypeId { get; set; }
        public string DeviceTypeName { get; set; }
        public string DeviceName { get; set; }
        public string DeviceCode { get; set; }
        public string Status { get; set; } //tình trạng thiết bị
        public DateTime? ProduceDate { get; set; }
        public string ImagePath { get; set; }
        public string Imei { get; set; }
        /// <summary>
        /// Guarantee:bảo hành 
        /// </summary>
        public int Guarantee { get; set; }//tháng
        public int GuaranteeCustomer { get; set; } //hạn bảo hành của người dùng
        public DateTime? GuaranteeDateCustomer { get; set; } //hạn bảo hành
        public int SecurityState { get; set; }//tình trạng bảo hành 
        public DateTime? BuyDate { get; set; }//ngày mua
        /// <summary>
        /// tình trạng thiết bị
        /// </summary> 
        public string RelayStatus1 { get; set; } //tình trạng thiết bị
        public string RelayStatus2 { get; set; } //tình trạng thiết bị
        public string DeviceIp { get; set; }
        /// <summary>
        /// chỉ số tiêu thụ
        /// </summary>
        public decimal CurrentNumber { set; get; }
        public decimal PreviousNumber { set; get; }
        public decimal Electricity { set; get; }
        public decimal Electric { set; get; }
        /// <summary>
        /// khách hàng cấp 2
        /// </summary>
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumberCustomer { get; set; }
        public string AddressCustomer { get; set; }
        public string BuildingName { get; set; }//tên tào nhà
        public DateTime? ProvidedDate { get; set; }//ngày cấp
        /// <summary>
        /// Thông tin gửi nhận thông báo
        /// </summary>
        public int CycleTime { get; set; }
        public int AmpereAlarm1 { get; set; }
        public int AmpereAlarm2 { get; set; }
        public int AmpereOFF1 { get; set; }
        public int AmpereOFF2 { get; set; }
    }
}