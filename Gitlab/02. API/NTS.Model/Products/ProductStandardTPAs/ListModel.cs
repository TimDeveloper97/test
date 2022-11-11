using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAs
{
    public class ListModel
    {
        public List<ListDataModel> ListTypePayment_NCC_SX { get; set; }
        public List<ListDataModel> ListRulesDelivery_NCC_SX { get; set; }
        public List<ListDataModel> ListCurency { get; set; }
        public ListModel()
        {
            ListTypePayment_NCC_SX = new List<ListDataModel>();
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 1, Name = "Điện chuyển tiền (TT: Telegraphic Transfer Remittance)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 2, Name = "Thư chuyển tiền (MTR: Mail Transfer Remittance)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 3, Name = "Trả tiền lấy chứng từ (C.A.D: Cash Against Document)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 4, Name = "Nhờ thu (Collection)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 5, Name = "Thư tín dụng hủy ngang (Revocable L/C)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 6, Name = "Thư tín dụng không thể hủy ngang (Irrevocable L/C)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 7, Name = "Thư tín dụng trả chậm (Usance Payable L/C) UPAC" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 8, Name = "Thư tín dụng trả dần (Defered L/C)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 9, Name = "Thư tín dụng dự phòng (Standby letter of Credit)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 10, Name = "Thư tín dụng tuần hoàn (Revolving Letter of Credit)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 11, Name = "Thư tín dụng chuyển nhượng (Transferable Letter of Credit)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 12, Name = "Thư tín dụng giáp lưng (Back-to-Back Letter of Credit)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 13, Name = "Thư tín dụng đối ứng (Reciprocal L/C)" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 14, Name = "Bitcoin" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 15, Name = "Paypal" });
            ListTypePayment_NCC_SX.Add(new ListDataModel { Id = 16, Name = "Visa" });

            ListRulesDelivery_NCC_SX = new List<ListDataModel>();
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 1, Name = "EXW (EXWORK) giao hàng tại xưởng (địa điểm quy định...)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 2, Name = "FAS (Free alongside ship): giao dọc mạng tàu (...cảng bốc quy định)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 3, Name = "FOB (Free on board)(Named port of shipment): giao lên tàu (cảng giao hàng xác định)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 4, Name = "FCA (free carrier...named point):(giao hàng cho người chuyên chở)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 5, Name = "CFR (Cost and Freight)(Named port of destination): tiền hàng và cước phí (cảng đến xác định)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 6, Name = "CPT (Freight or carriage paid to destination) - cước phí trả tới...." });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 7, Name = "CIF (Cost, Insurance and Freight)(Named port of destination): tiền hàng, bảo hiểm và cước phí (cảng đến xác định)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 8, Name = "CIP (Carriage, Insurance Paid to)(Named place of destination): cước phí, phí bảo hiểm trả đến (địa điểm đến xác định)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 9, Name = "DAT (Delivered at Terminal): giao tại bến (…nơi đến quy định)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 10, Name = "DAP (Delivered At Place): giao tại nơi đến (nơi đến quy định)" });
            ListRulesDelivery_NCC_SX.Add(new ListDataModel { Id = 11, Name = "DDP (Delivered Duty Paid): giao tới đích đã nộp thuế (…đích quy định)" });

            ListCurency = new List<ListDataModel>();
            ListCurency.Add(new ListDataModel { Id = 1, Name = "USD" });
            ListCurency.Add(new ListDataModel { Id = 2, Name = "EUR" });
            ListCurency.Add(new ListDataModel { Id = 3, Name = "CNY" });
            ListCurency.Add(new ListDataModel { Id = 4, Name = "VND" });
        }

    }

    public class ListDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
