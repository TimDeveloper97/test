using NTS.Model.ProductStandardTPAFile;
using NTS.Model.ProductStandardTPAImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAs
{
    public class ProductStandardTPAModel : BaseModel
    {
        public string Id { get; set; }
        public string EnglishName { get; set; }
        public string VietNamName { get; set; }
        public string Model { get; set; }
        public string TheFirm { get; set; }
        public string ProductStandardTPATypeId { get; set; }
        public string ProductStandardTPATypeName { get; set; }
        public string Manufacture_NCC_SX { get; set; }
        public int? RulesDelivery_NCC_SX { get; set; }
        public string Supplier_NCC_SX { get; set; }
        public string Name_NCC_SX { get; set; }
        public string Address_NCC_SX { get; set; }
        public string PIC_NCC_SX { get; set; }
        public string PhoneNumber_NCC_SX { get; set; }
        public string Email_NCC_SX { get; set; }
        public string Title_NCC_SX { get; set; }
        public string BankPayment_NCC_SX { get; set; }
        public Nullable<int> TypePayment_NCC_SX { get; set; }
        public string RulesPayment_NCC_SX { get; set; }
        public Nullable<int> RulesDelivery { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<decimal> ListedPrice { get; set; }
        public string PriceConformQuantity { get; set; }
        public string PricePolicy { get; set; }
        public Nullable<int> MinimumQuantity { get; set; }
        public Nullable<int> MethodVC { get; set; }
        public string LoadingPort { get; set; }
        public Nullable<decimal> PriceInSP_Price { get; set; }
        public Nullable<decimal> PriceInPO_Price { get; set; }
        public Nullable<int> Currency_PO { get; set; }
        public Nullable<decimal> Ratio_PO { get; set; }
        public Nullable<decimal> Amout_PO { get; set; }
        public Nullable<decimal> Weight_PriceWord { get; set; }
        public Nullable<decimal> PriceVCInWeight_PriceWord { get; set; }
        public Nullable<decimal> VolumePricing_PriceWord { get; set; }
        public Nullable<decimal> CPVCInWeight_Price_PriceWord { get; set; }
        public Nullable<int> CPVCInWeight_Currency_PriceWord { get; set; }
        public Nullable<decimal> CPVCInWeight_Ratio_PriceWord { get; set; }
        public Nullable<decimal> CPVCInWeight_AmoutVND_PriceWord { get; set; }
        public Nullable<decimal> Weight_Air_PriceWord { get; set; }
        public Nullable<decimal> MassConvertedBySize_Air_PriceWord { get; set; }
        public Nullable<decimal> VolumePricing_Air_PriceWord { get; set; }
        public Nullable<decimal> Price_AirTransport_Air_PriceWord { get; set; }
        public Nullable<int> Curency_AirTransport_Air_PriceWord { get; set; }
        public Nullable<decimal> Ratio_AirTransport_Air_PriceWord { get; set; }
        public Nullable<decimal> Amount_AirTransport_Air_PriceWord { get; set; }
        public Nullable<decimal> PackingSize_D_PriceWord { get; set; }
        public Nullable<decimal> PackingSize_R_PriceWord { get; set; }
        public Nullable<decimal> PackingSize_C_PriceWord { get; set; }
        public Nullable<decimal> PackingSize_Tolerance_PriceWord { get; set; }
        public Nullable<decimal> PackingSize_RealVolume_PriceWord { get; set; }
        public Nullable<decimal> PackingSize_PiceVolume_PriceWord { get; set; }
        public Nullable<decimal> PriceVCInCBM_PriceWord { get; set; }
        public Nullable<decimal> Price_PriceVC_LCLInBCM_PriceWord { get; set; }
        public Nullable<int> Currency_PriceVC_LCLInBCM_PriceWord { get; set; }
        public Nullable<decimal> Ratio_PriceVC_LCLInBCM_PriceWord { get; set; }
        public Nullable<decimal> Amout_PriceVC_LCLInBCM_PriceWord { get; set; }
        public Nullable<decimal> Price_PriceVC_Cont20_PriceWord { get; set; }
        public Nullable<int> Currency_PriceVC_Cont20_PriceWord { get; set; }
        public Nullable<decimal> Ratio_PriceVC_Cont20_PriceWord { get; set; }
        public Nullable<decimal> Amout_PriceVC_Cont20_PriceWord { get; set; }
        public Nullable<decimal> Price_PriceVC_Cont20OT_PiceWord { get; set; }
        public Nullable<int> Currency_PriceVC_Cont20OT_PiceWord { get; set; }
        public Nullable<decimal> Ratio_PriceVC_Cont20OT_PiceWord { get; set; }
        public Nullable<decimal> Amount_PriceVC_Cont20OT_PiceWord { get; set; }
        public Nullable<decimal> Price_PriceVC_Cont40_PriceWord { get; set; }
        public Nullable<int> Currency_PriceVC_Cont40_PriceWord { get; set; }
        public Nullable<decimal> Ratio_PriceVC_Cont40_PriceWord { get; set; }
        public Nullable<decimal> Amount_PriceVC_Cont40_PriceWord { get; set; }
        public Nullable<decimal> Price_PriceVC_Cont40OT_PriceWord { get; set; }
        public Nullable<int> Currency_PriceVC_Cont40OT_PriceWord { get; set; }
        public Nullable<decimal> Ratio_PriceVC_Cont40OT_PriceWord { get; set; }
        public Nullable<decimal> Amount_PriceVC_Cont40OT_PriceWord { get; set; }
        public Nullable<System.DateTime> UpdateDatePice_PriceWord { get; set; }
        public Nullable<decimal> Price_PriceLCExport { get; set; }
        public Nullable<int> Currency_PriceLCExport { get; set; }
        public Nullable<decimal> Ratio_PriceLCExport { get; set; }
        public Nullable<decimal> Amount_PriceLCExport { get; set; }
        public string InsurranceType { get; set; }
        public Nullable<decimal> Price_InsurrancePrice { get; set; }
        public Nullable<decimal> Price_PriceLCImport_LSS { get; set; }
        public Nullable<int> Currency_PriceLCImport_LSS { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_LSS { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_LSS { get; set; }
        public Nullable<decimal> Price_PriceLCImport_THC { get; set; }
        public Nullable<int> Currency_PriceLCImport_THC { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_THC { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_THC { get; set; }
        public Nullable<decimal> Price_PriceLCImport_DO { get; set; }
        public Nullable<int> Currency_PriceLCImport_DO { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_DO { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_DO { get; set; }
        public Nullable<decimal> Price_PriceLCImport_CIC { get; set; }
        public Nullable<int> Currency_PriceLCImport_CIC { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_CIC { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_CIC { get; set; }
        public Nullable<decimal> Price_PriceLCImport_HL { get; set; }
        public Nullable<int> Currency_PriceLCImport_HL { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_HL { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_HL { get; set; }
        public Nullable<decimal> Price_PriceLCImport_CLF { get; set; }
        public Nullable<int> Currency_PriceLCImport_CLF { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_CLF { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_CLF { get; set; }
        public Nullable<decimal> Price_PriceLCImport_CFS { get; set; }
        public Nullable<int> Currency_PriceLCImport_CFS { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_CFS { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_CFS { get; set; }
        public Nullable<decimal> Price_PriceLCImport_Lift { get; set; }
        public Nullable<int> Currency_PriceLCImport_Lift { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_Lift { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_Lift { get; set; }
        public Nullable<decimal> Price_PriceLCImport_IF { get; set; }
        public Nullable<int> Currency_PriceLCImport_IF { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_IF { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_IF { get; set; }
        public Nullable<decimal> Price_PriceLCImport_Other { get; set; }
        public Nullable<int> Currency_PriceLCImport_Other { get; set; }
        public Nullable<decimal> Ratio_PriceLCImport_Other { get; set; }
        public Nullable<decimal> Amount_PriceLCImport_Other { get; set; }
        public string HSCode { get; set; }
        public Nullable<decimal> ImportTax { get; set; }
        public Nullable<decimal> ImportTaxPrice { get; set; }
        public Nullable<System.DateTime> UpdateDateHSCode { get; set; }
        public string NameTaxOther { get; set; }
        public Nullable<decimal> TaxOther { get; set; }
        public Nullable<decimal> ImportTaxPriceOther { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> PriceOther { get; set; }
        public Nullable<decimal> PriceFW { get; set; }
        public Nullable<decimal> Surcharge { get; set; }
        public Nullable<int> InventoryTime { get; set; }
        public Nullable<decimal> ShortInterest { get; set; }
        public Nullable<decimal> MidtermInterest { get; set; }
        public Nullable<decimal> PriceProduct_TPA { get; set; }
        public Nullable<decimal> PriceVC_TPA { get; set; }
        public Nullable<decimal> ImportTaxPrice_TPA { get; set; }
        public Nullable<decimal> ImportTax_TPA { get; set; }
        public Nullable<decimal> VAT_TPA { get; set; }
        public Nullable<decimal> Interest_TPA { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> Profit_TPA { get; set; }
        public Nullable<decimal> PriceEXW_TPA { get; set; }
        public Nullable<System.DateTime> UpdateDatePrice_TPA { get; set; }
        public Nullable<decimal> Price_L1 { get; set; }
        public Nullable<decimal> Price_L2 { get; set; }
        public Nullable<decimal> Price_L3 { get; set; }
        public Nullable<decimal> Price_L4 { get; set; }
        public Nullable<decimal> Price_L5 { get; set; }
        public string BusinessDepartment { get; set; }
        public Nullable<int> Index { get; set; }
        public string Specifications { get; set; }
        public List<ProductStandardTPAImageModel> ListImage { get; set; }
        public bool IsSendSale { get; set; }
        public string Country_NCC_SX { get; set; }
        public string Website_NCC_SX { get; set; }
        public DateTime? SyncDate { get; set; }
        public bool IsCOCQ { get; set; }
        public string Country { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public string Note5 { get; set; }
        public string Note6 { get; set; }
        public decimal? Ratio_InsurrancePrice { get; set; }
        public ProductStandardTPAModel()
        {
            ListImage = new List<ProductStandardTPAImageModel>();
        }
    }
}
