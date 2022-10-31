using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ProductStandardTPAFile;
using NTS.Model.ProductStandardTPAImage;
using NTS.Model.ProductStandardTPAs;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

namespace QLTK.Business.ProductStandardTPAs
{
    public class ProductStandardTPABussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<ProductStandardTPAModel> SearchProductStandardTPA(ProductStandardTPASearchModel modelSearch)
        {
            SearchResultModel<ProductStandardTPAModel> searchResult = new SearchResultModel<ProductStandardTPAModel>();
            List<string> listParentId = new List<string>();

            var dataQuey = (from a in db.ProductStandardTPAs.AsNoTracking()
                            join t in db.ProductStandardTPATypes.AsNoTracking() on a.ProductStandardTPATypeId equals t.Id into at
                            from atn in at.DefaultIfEmpty()
                            orderby a.Model
                            select new ProductStandardTPAModel
                            {
                                Id = a.Id,
                                EnglishName = a.EnglishName,
                                VietNamName = a.VietNamName,
                                Model = a.Model,
                                TheFirm = a.TheFirm,
                                ProductStandardTPATypeId = a.ProductStandardTPATypeId,
                                ProductStandardTPATypeName = atn != null ? atn.Name : string.Empty,
                                Manufacture_NCC_SX = a.Manufacture_NCC_SX,
                                Supplier_NCC_SX = a.Supplier_NCC_SX,
                                Website_NCC_SX = a.Website_NCC_SX,
                                Country_NCC_SX = a.Country_NCC_SX,
                                Name_NCC_SX = a.Name_NCC_SX,
                                Address_NCC_SX = a.Address_NCC_SX,
                                PIC_NCC_SX = a.PIC_NCC_SX,
                                PhoneNumber_NCC_SX = a.PhoneNumber_NCC_SX,
                                Email_NCC_SX = a.Email_NCC_SX,
                                Title_NCC_SX = a.Title_NCC_SX,
                                BankPayment_NCC_SX = a.BankPayment_NCC_SX,
                                TypePayment_NCC_SX = a.TypePayment_NCC_SX,
                                RulesPayment_NCC_SX = a.RulesPayment_NCC_SX,
                                RulesDelivery = a.RulesDelivery,
                                DeliveryTime = a.DeliveryTime,
                                ListedPrice = a.ListedPrice,
                                PriceConformQuantity = a.PriceConformQuantity,
                                PricePolicy = a.PricePolicy,
                                MinimumQuantity = a.MinimumQuantity,
                                MethodVC = a.MethodVC,
                                LoadingPort = a.LoadingPort,
                                PriceInSP_Price = a.PriceInSP_Price,
                                PriceInPO_Price = a.PriceInPO_Price,
                                Currency_PO = a.Currency_PO,
                                Ratio_PO = a.Ratio_PO,
                                Amout_PO = a.Amout_PO,
                                Weight_PriceWord = a.Weight_PriceWord,
                                PriceVCInWeight_PriceWord = a.PriceVCInWeight_PriceWord,
                                VolumePricing_PriceWord = a.VolumePricing_PriceWord,
                                CPVCInWeight_Price_PriceWord = a.CPVCInWeight_Price_PriceWord,
                                CPVCInWeight_Currency_PriceWord = a.CPVCInWeight_Currency_PriceWord,
                                CPVCInWeight_Ratio_PriceWord = a.CPVCInWeight_Ratio_PriceWord,
                                CPVCInWeight_AmoutVND_PriceWord = a.CPVCInWeight_AmoutVND_PriceWord,
                                Weight_Air_PriceWord = a.Weight_Air_PriceWord,
                                MassConvertedBySize_Air_PriceWord = a.MassConvertedBySize_Air_PriceWord,
                                VolumePricing_Air_PriceWord = a.VolumePricing_Air_PriceWord,
                                Price_AirTransport_Air_PriceWord = a.Price_AirTransport_Air_PriceWord,
                                Curency_AirTransport_Air_PriceWord = a.Curency_AirTransport_Air_PriceWord,
                                Ratio_AirTransport_Air_PriceWord = a.Ratio_AirTransport_Air_PriceWord,
                                Amount_AirTransport_Air_PriceWord = a.Amount_AirTransport_Air_PriceWord,
                                PackingSize_D_PriceWord = a.PackingSize_D_PriceWord,
                                PackingSize_R_PriceWord = a.PackingSize_R_PriceWord,
                                PackingSize_C_PriceWord = a.PackingSize_C_PriceWord,
                                PackingSize_Tolerance_PriceWord = a.PackingSize_Tolerance_PriceWord,
                                PackingSize_RealVolume_PriceWord = a.PackingSize_RealVolume_PriceWord,
                                PackingSize_PiceVolume_PriceWord = a.PackingSize_PiceVolume_PriceWord,
                                PriceVCInCBM_PriceWord = a.PriceVCInCBM_PriceWord,
                                Price_PriceVC_LCLInBCM_PriceWord = a.Price_PriceVC_LCLInBCM_PriceWord,
                                Currency_PriceVC_LCLInBCM_PriceWord = a.Currency_PriceVC_LCLInBCM_PriceWord,
                                Ratio_PriceVC_LCLInBCM_PriceWord = a.Ratio_PriceVC_LCLInBCM_PriceWord,
                                Amout_PriceVC_LCLInBCM_PriceWord = a.Amout_PriceVC_LCLInBCM_PriceWord,
                                Price_PriceVC_Cont20_PriceWord = a.Price_PriceVC_Cont20_PriceWord,
                                Currency_PriceVC_Cont20_PriceWord = a.Currency_PriceVC_Cont20_PriceWord,
                                Ratio_PriceVC_Cont20_PriceWord = a.Ratio_PriceVC_Cont20_PriceWord,
                                Amout_PriceVC_Cont20_PriceWord = a.Amout_PriceVC_Cont20_PriceWord,
                                Price_PriceVC_Cont20OT_PiceWord = a.Price_PriceVC_Cont20OT_PiceWord,
                                Currency_PriceVC_Cont20OT_PiceWord = a.Currency_PriceVC_Cont20OT_PiceWord,
                                Ratio_PriceVC_Cont20OT_PiceWord = a.Ratio_PriceVC_Cont20OT_PiceWord,
                                Amount_PriceVC_Cont20OT_PiceWord = a.Amount_PriceVC_Cont20OT_PiceWord,
                                Price_PriceVC_Cont40_PriceWord = a.Price_PriceVC_Cont40_PriceWord,
                                Currency_PriceVC_Cont40_PriceWord = a.Currency_PriceVC_Cont40_PriceWord,
                                Ratio_PriceVC_Cont40_PriceWord = a.Ratio_PriceVC_Cont40_PriceWord,
                                Amount_PriceVC_Cont40_PriceWord = a.Amount_PriceVC_Cont40_PriceWord,
                                Price_PriceVC_Cont40OT_PriceWord = a.Price_PriceVC_Cont40OT_PriceWord,
                                Currency_PriceVC_Cont40OT_PriceWord = a.Currency_PriceVC_Cont40OT_PriceWord,
                                Ratio_PriceVC_Cont40OT_PriceWord = a.Ratio_PriceVC_Cont40OT_PriceWord,
                                Amount_PriceVC_Cont40OT_PriceWord = a.Amount_PriceVC_Cont40OT_PriceWord,
                                UpdateDatePice_PriceWord = a.UpdateDatePice_PriceWord,
                                Price_PriceLCExport = a.Price_PriceLCExport,
                                Currency_PriceLCExport = a.Currency_PriceLCExport,
                                Ratio_PriceLCExport = a.Ratio_PriceLCExport,
                                Amount_PriceLCExport = a.Amount_PriceLCExport,
                                InsurranceType = a.InsurranceType,
                                Price_InsurrancePrice = a.Price_InsurrancePrice,
                                Price_PriceLCImport_LSS = a.Price_PriceLCImport_LSS,
                                Currency_PriceLCImport_LSS = a.Currency_PriceLCImport_LSS,
                                Ratio_PriceLCImport_LSS = a.Ratio_PriceLCImport_LSS,
                                Amount_PriceLCImport_LSS = a.Amount_PriceLCImport_LSS,
                                Price_PriceLCImport_THC = a.Price_PriceLCImport_THC,
                                Currency_PriceLCImport_THC = a.Currency_PriceLCImport_THC,
                                Ratio_PriceLCImport_THC = a.Ratio_PriceLCImport_THC,
                                Amount_PriceLCImport_THC = a.Amount_PriceLCImport_THC,
                                Price_PriceLCImport_DO = a.Price_PriceLCImport_DO,
                                Currency_PriceLCImport_DO = a.Currency_PriceLCImport_DO,
                                Ratio_PriceLCImport_DO = a.Ratio_PriceLCImport_DO,
                                Amount_PriceLCImport_DO = a.Amount_PriceLCImport_DO,
                                Price_PriceLCImport_CIC = a.Price_PriceLCImport_CIC,
                                Currency_PriceLCImport_CIC = a.Currency_PriceLCImport_CIC,
                                Ratio_PriceLCImport_CIC = a.Ratio_PriceLCImport_CIC,
                                Amount_PriceLCImport_CIC = a.Amount_PriceLCImport_CIC,
                                Price_PriceLCImport_HL = a.Price_PriceLCImport_HL,
                                Currency_PriceLCImport_HL = a.Currency_PriceLCImport_HL,
                                Ratio_PriceLCImport_HL = a.Ratio_PriceLCImport_HL,
                                Amount_PriceLCImport_HL = a.Amount_PriceLCImport_HL,
                                Price_PriceLCImport_CLF = a.Price_PriceLCImport_CLF,
                                Currency_PriceLCImport_CLF = a.Currency_PriceLCImport_CLF,
                                Ratio_PriceLCImport_CLF = a.Ratio_PriceLCImport_CLF,
                                Amount_PriceLCImport_CLF = a.Amount_PriceLCImport_CLF,
                                Price_PriceLCImport_CFS = a.Price_PriceLCImport_CFS,
                                Currency_PriceLCImport_CFS = a.Currency_PriceLCImport_CFS,
                                Ratio_PriceLCImport_CFS = a.Ratio_PriceLCImport_CFS,
                                Amount_PriceLCImport_CFS = a.Amount_PriceLCImport_CFS,
                                Price_PriceLCImport_Lift = a.Price_PriceLCImport_Lift,
                                Currency_PriceLCImport_Lift = a.Currency_PriceLCImport_Lift,
                                Ratio_PriceLCImport_Lift = a.Ratio_PriceLCImport_Lift,
                                Amount_PriceLCImport_Lift = a.Amount_PriceLCImport_Lift,
                                Price_PriceLCImport_IF = a.Price_PriceLCImport_IF,
                                Currency_PriceLCImport_IF = a.Currency_PriceLCImport_IF,
                                Ratio_PriceLCImport_IF = a.Ratio_PriceLCImport_IF,
                                Amount_PriceLCImport_IF = a.Amount_PriceLCImport_IF,
                                Price_PriceLCImport_Other = a.Price_PriceLCImport_Other,
                                Currency_PriceLCImport_Other = a.Currency_PriceLCImport_Other,
                                Ratio_PriceLCImport_Other = a.Ratio_PriceLCImport_Other,
                                Amount_PriceLCImport_Other = a.Amount_PriceLCImport_Other,
                                HSCode = a.HSCode,
                                ImportTax = a.ImportTax,
                                ImportTaxPrice = a.ImportTaxPrice,
                                UpdateDateHSCode = a.UpdateDateHSCode,
                                NameTaxOther = a.NameTaxOther,
                                TaxOther = a.TaxOther,
                                ImportTaxPriceOther = a.ImportTaxPriceOther,
                                VAT = a.VAT,
                                PriceOther = a.PriceOther,
                                PriceFW = a.PriceFW,
                                Surcharge = a.Surcharge,
                                InventoryTime = a.InventoryTime,
                                ShortInterest = a.ShortInterest,
                                MidtermInterest = a.MidtermInterest,
                                PriceProduct_TPA = a.PriceProduct_TPA,
                                PriceVC_TPA = a.PriceVC_TPA,
                                ImportTaxPrice_TPA = a.ImportTaxPrice_TPA,
                                ImportTax_TPA = a.ImportTax_TPA,
                                VAT_TPA = a.VAT_TPA,
                                Interest_TPA = a.Interest_TPA,
                                TotalPrice = a.TotalPrice,
                                Profit_TPA = a.Profit_TPA,
                                PriceEXW_TPA = a.PriceEXW_TPA,
                                UpdateDatePrice_TPA = a.UpdateDatePrice_TPA,
                                Price_L1 = a.Price_L1,
                                Price_L2 = a.Price_L2,
                                Price_L3 = a.Price_L3,
                                Price_L4 = a.Price_L4,
                                Price_L5 = a.Price_L5,
                                BusinessDepartment = a.BusinessDepartment,
                                Index = a.Index,
                                Specifications = a.Specifications,
                                IsSendSale = a.IsSendSale,
                                SyncDate = a.SyncDate,
                                IsCOCQ = a.IsCOCQ
                            }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuey = dataQuey.Where(i => i.EnglishName.ToUpper().Contains(modelSearch.Name.ToUpper()) || i.VietNamName.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuey = dataQuey.Where(i => i.Model.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Manufacture))
            {
                dataQuey = dataQuey.Where(i => i.Manufacture_NCC_SX.ToUpper().Contains(modelSearch.Manufacture.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Supplier))
            {
                dataQuey = dataQuey.Where(i => i.Supplier_NCC_SX.ToUpper().Contains(modelSearch.Supplier.ToUpper()));
            }

            //if (modelSearch.IsSendSale != null)
            //{
            //    dataQuey = dataQuey.Where(i => i.IsSendSale.Equals(modelSearch.IsSendSale));
            //}

            if (modelSearch.IsCOCQ.HasValue)
            {
                dataQuey = dataQuey.Where(i => i.IsCOCQ == modelSearch.IsCOCQ);
            }

            if (!string.IsNullOrEmpty(modelSearch.ProductStandardTPATypeId))
            {
                //dataQuey = dataQuey.Where(i => i.ProductStandardTPATypeId.Equals(modelSearch.ProductStandardTPATypeId));

                var productStandardTPA = db.ProductStandardTPATypes.AsNoTracking().FirstOrDefault(i => i.Id.Equals(modelSearch.ProductStandardTPATypeId));
                if (productStandardTPA != null)
                {
                    listParentId.Add(productStandardTPA.Id);
                }

                listParentId.AddRange(GetListParent(modelSearch.ProductStandardTPATypeId));
                var listModuleGroup = listParentId.AsQueryable();
                dataQuey = (from a in dataQuey
                            join b in listModuleGroup.AsQueryable() on a.ProductStandardTPATypeId equals b
                            select a).AsQueryable();
            }

            searchResult.TotalItem = dataQuey.Count();
            if (modelSearch.IsExport)
            {
                searchResult.ListResult = dataQuey.OrderBy(i => i.Model).ToList();
            }
            else
            {
                searchResult.Date = dataQuey.Max(i => i.SyncDate);
                var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;
            }
            return searchResult;
        }


        public List<string> GetListParent(string id)
        {
            List<string> listChild = new List<string>();
            var moduleGroup = db.ProductStandardTPATypes.AsNoTracking().Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(moduleGroup);
            if (moduleGroup.Count > 0)
            {
                foreach (var item in moduleGroup)
                {
                    listChild.AddRange(GetListParent(item));
                }
            }
            return listChild;
        }

        public void CreateProductStandardTPA(ProductStandardTPAModel model)
        {
            if (db.ProductStandardTPAs.AsNoTracking().FirstOrDefault(o => o.Model.Equals(model.Model)) != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductStandardTPA);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ProductStandardTPA productStandardTPA = new ProductStandardTPA()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EnglishName = model.EnglishName,
                        VietNamName = model.VietNamName,
                        Model = model.Model,
                        TheFirm = model.TheFirm,
                        ProductStandardTPATypeId = model.ProductStandardTPATypeId,
                        Manufacture_NCC_SX = model.Manufacture_NCC_SX,
                        Supplier_NCC_SX = model.Supplier_NCC_SX,
                        Name_NCC_SX = model.Name_NCC_SX,
                        Address_NCC_SX = model.Address_NCC_SX,
                        PIC_NCC_SX = model.PIC_NCC_SX,
                        PhoneNumber_NCC_SX = model.PhoneNumber_NCC_SX,
                        Email_NCC_SX = model.Email_NCC_SX,
                        Title_NCC_SX = model.Title_NCC_SX,
                        BankPayment_NCC_SX = model.BankPayment_NCC_SX,
                        TypePayment_NCC_SX = model.TypePayment_NCC_SX,
                        RulesPayment_NCC_SX = model.RulesPayment_NCC_SX,
                        RulesDelivery = model.RulesDelivery,
                        DeliveryTime = model.DeliveryTime,
                        Website_NCC_SX = model.Website_NCC_SX,
                        Country_NCC_SX = model.Country_NCC_SX,
                        ListedPrice = model.ListedPrice,
                        PriceConformQuantity = model.PriceConformQuantity,
                        PricePolicy = model.PricePolicy,
                        MinimumQuantity = model.MinimumQuantity,
                        MethodVC = model.MethodVC,
                        LoadingPort = model.LoadingPort,
                        PriceInSP_Price = model.PriceInSP_Price,
                        PriceInPO_Price = model.PriceInPO_Price,
                        Currency_PO = model.Currency_PO,
                        Ratio_PO = model.Ratio_PO,
                        Amout_PO = model.Amout_PO,
                        Weight_PriceWord = model.Weight_PriceWord,
                        PriceVCInWeight_PriceWord = model.PriceVCInWeight_PriceWord,
                        VolumePricing_PriceWord = model.VolumePricing_PriceWord,
                        CPVCInWeight_Price_PriceWord = model.CPVCInWeight_Price_PriceWord,
                        CPVCInWeight_Currency_PriceWord = model.CPVCInWeight_Currency_PriceWord,
                        CPVCInWeight_Ratio_PriceWord = model.CPVCInWeight_Ratio_PriceWord,
                        CPVCInWeight_AmoutVND_PriceWord = model.CPVCInWeight_AmoutVND_PriceWord,
                        Weight_Air_PriceWord = model.Weight_Air_PriceWord,
                        MassConvertedBySize_Air_PriceWord = model.MassConvertedBySize_Air_PriceWord,
                        VolumePricing_Air_PriceWord = model.VolumePricing_Air_PriceWord,
                        Price_AirTransport_Air_PriceWord = model.Price_AirTransport_Air_PriceWord,
                        Curency_AirTransport_Air_PriceWord = model.Curency_AirTransport_Air_PriceWord,
                        Ratio_AirTransport_Air_PriceWord = model.Ratio_AirTransport_Air_PriceWord,
                        Amount_AirTransport_Air_PriceWord = model.Amount_AirTransport_Air_PriceWord,
                        PackingSize_D_PriceWord = model.PackingSize_D_PriceWord,
                        PackingSize_R_PriceWord = model.PackingSize_R_PriceWord,
                        PackingSize_C_PriceWord = model.PackingSize_C_PriceWord,
                        PackingSize_RealVolume_PriceWord = model.PackingSize_RealVolume_PriceWord,
                        PackingSize_PiceVolume_PriceWord = model.PackingSize_PiceVolume_PriceWord,
                        PackingSize_Tolerance_PriceWord = model.PackingSize_Tolerance_PriceWord,
                        PriceVCInCBM_PriceWord = model.PriceVCInCBM_PriceWord,
                        Price_PriceVC_LCLInBCM_PriceWord = model.Price_PriceVC_LCLInBCM_PriceWord,
                        Currency_PriceVC_LCLInBCM_PriceWord = model.Currency_PriceVC_LCLInBCM_PriceWord,
                        Ratio_PriceVC_LCLInBCM_PriceWord = model.Ratio_PriceVC_LCLInBCM_PriceWord,
                        Amout_PriceVC_LCLInBCM_PriceWord = model.Amout_PriceVC_LCLInBCM_PriceWord,
                        Price_PriceVC_Cont20_PriceWord = model.Price_PriceVC_Cont20_PriceWord,
                        Currency_PriceVC_Cont20_PriceWord = model.Currency_PriceVC_Cont20_PriceWord,
                        Ratio_PriceVC_Cont20_PriceWord = model.Ratio_PriceVC_Cont20_PriceWord,
                        Amout_PriceVC_Cont20_PriceWord = model.Amout_PriceVC_Cont20_PriceWord,
                        Price_PriceVC_Cont20OT_PiceWord = model.Price_PriceVC_Cont20OT_PiceWord,
                        Currency_PriceVC_Cont20OT_PiceWord = model.Currency_PriceVC_Cont20OT_PiceWord,
                        Ratio_PriceVC_Cont20OT_PiceWord = model.Ratio_PriceVC_Cont20OT_PiceWord,
                        Amount_PriceVC_Cont20OT_PiceWord = model.Amount_PriceVC_Cont20OT_PiceWord,
                        Price_PriceVC_Cont40_PriceWord = model.Price_PriceVC_Cont40_PriceWord,
                        Currency_PriceVC_Cont40_PriceWord = model.Currency_PriceVC_Cont40_PriceWord,
                        Ratio_PriceVC_Cont40_PriceWord = model.Ratio_PriceVC_Cont40_PriceWord,
                        Amount_PriceVC_Cont40_PriceWord = model.Amount_PriceVC_Cont40_PriceWord,
                        Price_PriceVC_Cont40OT_PriceWord = model.Price_PriceVC_Cont40OT_PriceWord,
                        Currency_PriceVC_Cont40OT_PriceWord = model.Currency_PriceVC_Cont40OT_PriceWord,
                        Ratio_PriceVC_Cont40OT_PriceWord = model.Ratio_PriceVC_Cont40OT_PriceWord,
                        Amount_PriceVC_Cont40OT_PriceWord = model.Amount_PriceVC_Cont40OT_PriceWord,
                        UpdateDatePice_PriceWord = model.UpdateDatePice_PriceWord,
                        Price_PriceLCExport = model.Price_PriceLCExport,
                        Currency_PriceLCExport = model.Currency_PriceLCExport,
                        Ratio_PriceLCExport = model.Ratio_PriceLCExport,
                        Amount_PriceLCExport = model.Amount_PriceLCExport,
                        InsurranceType = model.InsurranceType,
                        Price_InsurrancePrice = model.Price_InsurrancePrice,
                        Price_PriceLCImport_LSS = model.Price_PriceLCImport_LSS,
                        Currency_PriceLCImport_LSS = model.Currency_PriceLCImport_LSS,
                        Ratio_PriceLCImport_LSS = model.Ratio_PriceLCImport_LSS,
                        Amount_PriceLCImport_LSS = model.Amount_PriceLCImport_LSS,
                        Price_PriceLCImport_THC = model.Price_PriceLCImport_THC,
                        Currency_PriceLCImport_THC = model.Currency_PriceLCImport_THC,
                        Ratio_PriceLCImport_THC = model.Ratio_PriceLCImport_THC,
                        Amount_PriceLCImport_THC = model.Amount_PriceLCImport_THC,
                        Price_PriceLCImport_DO = model.Price_PriceLCImport_DO,
                        Currency_PriceLCImport_DO = model.Currency_PriceLCImport_DO,
                        Ratio_PriceLCImport_DO = model.Ratio_PriceLCImport_DO,
                        Amount_PriceLCImport_DO = model.Amount_PriceLCImport_DO,
                        Price_PriceLCImport_CIC = model.Price_PriceLCImport_CIC,
                        Currency_PriceLCImport_CIC = model.Currency_PriceLCImport_CIC,
                        Ratio_PriceLCImport_CIC = model.Ratio_PriceLCImport_CIC,
                        Amount_PriceLCImport_CIC = model.Amount_PriceLCImport_CIC,
                        Price_PriceLCImport_HL = model.Price_PriceLCImport_HL,
                        Currency_PriceLCImport_HL = model.Currency_PriceLCImport_HL,
                        Ratio_PriceLCImport_HL = model.Ratio_PriceLCImport_HL,
                        Amount_PriceLCImport_HL = model.Amount_PriceLCImport_HL,
                        Price_PriceLCImport_CLF = model.Price_PriceLCImport_CLF,
                        Currency_PriceLCImport_CLF = model.Currency_PriceLCImport_CLF,
                        Ratio_PriceLCImport_CLF = model.Ratio_PriceLCImport_CLF,
                        Amount_PriceLCImport_CLF = model.Amount_PriceLCImport_CLF,
                        Price_PriceLCImport_CFS = model.Price_PriceLCImport_CFS,
                        Currency_PriceLCImport_CFS = model.Currency_PriceLCImport_CFS,
                        Ratio_PriceLCImport_CFS = model.Ratio_PriceLCImport_CFS,
                        Amount_PriceLCImport_CFS = model.Amount_PriceLCImport_CFS,
                        Price_PriceLCImport_Lift = model.Price_PriceLCImport_Lift,
                        Currency_PriceLCImport_Lift = model.Currency_PriceLCImport_Lift,
                        Ratio_PriceLCImport_Lift = model.Ratio_PriceLCImport_Lift,
                        Amount_PriceLCImport_Lift = model.Amount_PriceLCImport_Lift,
                        Price_PriceLCImport_IF = model.Price_PriceLCImport_IF,
                        Currency_PriceLCImport_IF = model.Currency_PriceLCImport_IF,
                        Ratio_PriceLCImport_IF = model.Ratio_PriceLCImport_IF,
                        Amount_PriceLCImport_IF = model.Amount_PriceLCImport_IF,
                        Price_PriceLCImport_Other = model.Price_PriceLCImport_Other,
                        Currency_PriceLCImport_Other = model.Currency_PriceLCImport_Other,
                        Ratio_PriceLCImport_Other = model.Ratio_PriceLCImport_Other,
                        Amount_PriceLCImport_Other = model.Amount_PriceLCImport_Other,
                        HSCode = model.HSCode,
                        ImportTax = model.ImportTax,
                        ImportTaxPrice = model.ImportTaxPrice,
                        UpdateDateHSCode = model.UpdateDateHSCode,
                        NameTaxOther = model.NameTaxOther,
                        TaxOther = model.TaxOther,
                        ImportTaxPriceOther = model.ImportTaxPriceOther,
                        VAT = model.VAT,
                        PriceOther = model.PriceOther,
                        PriceFW = model.PriceFW,
                        Surcharge = model.Surcharge,
                        InventoryTime = model.InventoryTime,
                        BusinessDepartment = model.BusinessDepartment,
                        Index = model.Index,
                        Specifications = model.Specifications,
                        IsCOCQ = model.IsCOCQ,
                        Country = model.Country,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        Note1 = model.Note1,
                        Note2 = model.Note2,
                        Note3 = model.Note3,
                        Note4 = model.Note4,
                        Note5 = model.Note5,
                        Note6 = model.Note6,
                        PriceProduct_TPA = model.PriceProduct_TPA,
                        PriceVC_TPA = model.PriceVC_TPA,
                        ImportTaxPrice_TPA = model.ImportTaxPrice_TPA,
                        ImportTax_TPA = model.ImportTax_TPA,
                        TotalPrice = model.TotalPrice,
                        Ratio_InsurrancePrice = model.Ratio_InsurrancePrice,
                        Profit_TPA = model.Profit_TPA
                    };

                    if (model.ListImage.Count() > 0)
                    {
                        ProductStandardTPAImage productStandardTPAImage;
                        foreach (var item in model.ListImage)
                        {
                            productStandardTPAImage = new ProductStandardTPAImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductStandardTPAId = productStandardTPA.Id,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                ThumbnailPath = item.ThumbnailPath,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now
                            };
                            db.ProductStandardTPAImages.Add(productStandardTPAImage);
                        }
                    }

                    db.ProductStandardTPAs.Add(productStandardTPA);
                    //UserLogUtil.LogHistotyAdd(db, model.CreateBy, newClassRoom.Code, newClassRoom.Id, Constants.LOG_ClassRoom);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public void UpdateProductStandardTPA(ProductStandardTPAModel model, bool isEditPriceTPA)
        {
            if (db.ProductStandardTPAs.AsNoTracking().FirstOrDefault(i => !i.Id.Equals(model.Id) && i.Model.Equals(model.Model)) != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductStandardTPA);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productStandardTPA = db.ProductStandardTPAs.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    productStandardTPA.EnglishName = model.EnglishName;
                    productStandardTPA.VietNamName = model.VietNamName;
                    productStandardTPA.Model = model.Model;
                    productStandardTPA.TheFirm = model.TheFirm;
                    productStandardTPA.ProductStandardTPATypeId = model.ProductStandardTPATypeId;
                    productStandardTPA.Manufacture_NCC_SX = model.Manufacture_NCC_SX;
                    productStandardTPA.Supplier_NCC_SX = model.Supplier_NCC_SX;
                    productStandardTPA.Name_NCC_SX = model.Name_NCC_SX;
                    productStandardTPA.Address_NCC_SX = model.Address_NCC_SX;
                    productStandardTPA.PIC_NCC_SX = model.PIC_NCC_SX;
                    productStandardTPA.PhoneNumber_NCC_SX = model.PhoneNumber_NCC_SX;
                    productStandardTPA.Email_NCC_SX = model.Email_NCC_SX;
                    productStandardTPA.Title_NCC_SX = model.Title_NCC_SX;
                    productStandardTPA.BankPayment_NCC_SX = model.BankPayment_NCC_SX;
                    productStandardTPA.TypePayment_NCC_SX = model.TypePayment_NCC_SX;
                    productStandardTPA.RulesPayment_NCC_SX = model.RulesPayment_NCC_SX;
                    productStandardTPA.RulesDelivery = model.RulesDelivery;
                    productStandardTPA.DeliveryTime = model.DeliveryTime;
                    productStandardTPA.Website_NCC_SX = model.Website_NCC_SX;
                    productStandardTPA.Country_NCC_SX = model.Country_NCC_SX;
                    productStandardTPA.ListedPrice = model.ListedPrice;
                    productStandardTPA.PriceConformQuantity = model.PriceConformQuantity;
                    productStandardTPA.PricePolicy = model.PricePolicy;
                    productStandardTPA.MinimumQuantity = model.MinimumQuantity;
                    productStandardTPA.MethodVC = model.MethodVC;
                    productStandardTPA.LoadingPort = model.LoadingPort;
                    productStandardTPA.PriceInSP_Price = model.PriceInSP_Price;
                    productStandardTPA.PriceInPO_Price = model.PriceInPO_Price;
                    productStandardTPA.Currency_PO = model.Currency_PO;
                    productStandardTPA.Ratio_PO = model.Ratio_PO;
                    productStandardTPA.Amout_PO = model.Amout_PO;
                    productStandardTPA.Weight_PriceWord = model.Weight_PriceWord;
                    productStandardTPA.PriceVCInWeight_PriceWord = model.PriceVCInWeight_PriceWord;
                    productStandardTPA.VolumePricing_PriceWord = model.VolumePricing_PriceWord;
                    productStandardTPA.CPVCInWeight_Price_PriceWord = model.CPVCInWeight_Price_PriceWord;
                    productStandardTPA.CPVCInWeight_Currency_PriceWord = model.CPVCInWeight_Currency_PriceWord;
                    productStandardTPA.CPVCInWeight_Ratio_PriceWord = model.CPVCInWeight_Ratio_PriceWord;
                    productStandardTPA.CPVCInWeight_AmoutVND_PriceWord = model.CPVCInWeight_AmoutVND_PriceWord;
                    productStandardTPA.Weight_Air_PriceWord = model.Weight_Air_PriceWord;
                    productStandardTPA.MassConvertedBySize_Air_PriceWord = model.MassConvertedBySize_Air_PriceWord;
                    productStandardTPA.VolumePricing_Air_PriceWord = model.VolumePricing_Air_PriceWord;
                    productStandardTPA.Price_AirTransport_Air_PriceWord = model.Price_AirTransport_Air_PriceWord;
                    productStandardTPA.Curency_AirTransport_Air_PriceWord = model.Curency_AirTransport_Air_PriceWord;
                    productStandardTPA.Ratio_AirTransport_Air_PriceWord = model.Ratio_AirTransport_Air_PriceWord;
                    productStandardTPA.Amount_AirTransport_Air_PriceWord = model.Amount_AirTransport_Air_PriceWord;
                    productStandardTPA.PackingSize_D_PriceWord = model.PackingSize_D_PriceWord;
                    productStandardTPA.PackingSize_R_PriceWord = model.PackingSize_R_PriceWord;
                    productStandardTPA.PackingSize_C_PriceWord = model.PackingSize_C_PriceWord;
                    productStandardTPA.PackingSize_RealVolume_PriceWord = model.PackingSize_RealVolume_PriceWord;
                    productStandardTPA.PackingSize_PiceVolume_PriceWord = model.PackingSize_PiceVolume_PriceWord;
                    productStandardTPA.PackingSize_Tolerance_PriceWord = model.PackingSize_Tolerance_PriceWord;
                    productStandardTPA.PriceVCInCBM_PriceWord = model.PriceVCInCBM_PriceWord;
                    productStandardTPA.Price_PriceVC_LCLInBCM_PriceWord = model.Price_PriceVC_LCLInBCM_PriceWord;
                    productStandardTPA.Currency_PriceVC_LCLInBCM_PriceWord = model.Currency_PriceVC_LCLInBCM_PriceWord;
                    productStandardTPA.Ratio_PriceVC_LCLInBCM_PriceWord = model.Ratio_PriceVC_LCLInBCM_PriceWord;
                    productStandardTPA.Amout_PriceVC_LCLInBCM_PriceWord = model.Amout_PriceVC_LCLInBCM_PriceWord;
                    productStandardTPA.Price_PriceVC_Cont20_PriceWord = model.Price_PriceVC_Cont20_PriceWord;
                    productStandardTPA.Currency_PriceVC_Cont20_PriceWord = model.Currency_PriceVC_Cont20_PriceWord;
                    productStandardTPA.Ratio_PriceVC_Cont20_PriceWord = model.Ratio_PriceVC_Cont20_PriceWord;
                    productStandardTPA.Amout_PriceVC_Cont20_PriceWord = model.Amout_PriceVC_Cont20_PriceWord;
                    productStandardTPA.Price_PriceVC_Cont20OT_PiceWord = model.Price_PriceVC_Cont20OT_PiceWord;
                    productStandardTPA.Currency_PriceVC_Cont20OT_PiceWord = model.Currency_PriceVC_Cont20OT_PiceWord;
                    productStandardTPA.Ratio_PriceVC_Cont20OT_PiceWord = model.Ratio_PriceVC_Cont20OT_PiceWord;
                    productStandardTPA.Amount_PriceVC_Cont20OT_PiceWord = model.Amount_PriceVC_Cont20OT_PiceWord;
                    productStandardTPA.Price_PriceVC_Cont40_PriceWord = model.Price_PriceVC_Cont40_PriceWord;
                    productStandardTPA.Currency_PriceVC_Cont40_PriceWord = model.Currency_PriceVC_Cont40_PriceWord;
                    productStandardTPA.Ratio_PriceVC_Cont40_PriceWord = model.Ratio_PriceVC_Cont40_PriceWord;
                    productStandardTPA.Amount_PriceVC_Cont40_PriceWord = model.Amount_PriceVC_Cont40_PriceWord;
                    productStandardTPA.Price_PriceVC_Cont40OT_PriceWord = model.Price_PriceVC_Cont40OT_PriceWord;
                    productStandardTPA.Currency_PriceVC_Cont40OT_PriceWord = model.Currency_PriceVC_Cont40OT_PriceWord;
                    productStandardTPA.Ratio_PriceVC_Cont40OT_PriceWord = model.Ratio_PriceVC_Cont40OT_PriceWord;
                    productStandardTPA.Amount_PriceVC_Cont40OT_PriceWord = model.Amount_PriceVC_Cont40OT_PriceWord;
                    productStandardTPA.UpdateDatePice_PriceWord = model.UpdateDatePice_PriceWord;
                    productStandardTPA.Price_PriceLCExport = model.Price_PriceLCExport;
                    productStandardTPA.Currency_PriceLCExport = model.Currency_PriceLCExport;
                    productStandardTPA.Ratio_PriceLCExport = model.Ratio_PriceLCExport;
                    productStandardTPA.Amount_PriceLCExport = model.Amount_PriceLCExport;
                    productStandardTPA.InsurranceType = model.InsurranceType;
                    productStandardTPA.Price_InsurrancePrice = model.Price_InsurrancePrice;
                    productStandardTPA.Price_PriceLCImport_LSS = model.Price_PriceLCImport_LSS;
                    productStandardTPA.Currency_PriceLCImport_LSS = model.Currency_PriceLCImport_LSS;
                    productStandardTPA.Ratio_PriceLCImport_LSS = model.Ratio_PriceLCImport_LSS;
                    productStandardTPA.Amount_PriceLCImport_LSS = model.Amount_PriceLCImport_LSS;
                    productStandardTPA.Price_PriceLCImport_THC = model.Price_PriceLCImport_THC;
                    productStandardTPA.Currency_PriceLCImport_THC = model.Currency_PriceLCImport_THC;
                    productStandardTPA.Ratio_PriceLCImport_THC = model.Ratio_PriceLCImport_THC;
                    productStandardTPA.Amount_PriceLCImport_THC = model.Amount_PriceLCImport_THC;
                    productStandardTPA.Price_PriceLCImport_DO = model.Price_PriceLCImport_DO;
                    productStandardTPA.Currency_PriceLCImport_DO = model.Currency_PriceLCImport_DO;
                    productStandardTPA.Ratio_PriceLCImport_DO = model.Ratio_PriceLCImport_DO;
                    productStandardTPA.Amount_PriceLCImport_DO = model.Amount_PriceLCImport_DO;
                    productStandardTPA.Price_PriceLCImport_CIC = model.Price_PriceLCImport_CIC;
                    productStandardTPA.Currency_PriceLCImport_CIC = model.Currency_PriceLCImport_CIC;
                    productStandardTPA.Ratio_PriceLCImport_CIC = model.Ratio_PriceLCImport_CIC;
                    productStandardTPA.Amount_PriceLCImport_CIC = model.Amount_PriceLCImport_CIC;
                    productStandardTPA.Price_PriceLCImport_HL = model.Price_PriceLCImport_HL;
                    productStandardTPA.Currency_PriceLCImport_HL = model.Currency_PriceLCImport_HL;
                    productStandardTPA.Ratio_PriceLCImport_HL = model.Ratio_PriceLCImport_HL;
                    productStandardTPA.Amount_PriceLCImport_HL = model.Amount_PriceLCImport_HL;
                    productStandardTPA.Price_PriceLCImport_CLF = model.Price_PriceLCImport_CLF;
                    productStandardTPA.Currency_PriceLCImport_CLF = model.Currency_PriceLCImport_CLF;
                    productStandardTPA.Ratio_PriceLCImport_CLF = model.Ratio_PriceLCImport_CLF;
                    productStandardTPA.Amount_PriceLCImport_CLF = model.Amount_PriceLCImport_CLF;
                    productStandardTPA.Price_PriceLCImport_CFS = model.Price_PriceLCImport_CFS;
                    productStandardTPA.Currency_PriceLCImport_CFS = model.Currency_PriceLCImport_CFS;
                    productStandardTPA.Ratio_PriceLCImport_CFS = model.Ratio_PriceLCImport_CFS;
                    productStandardTPA.Amount_PriceLCImport_CFS = model.Amount_PriceLCImport_CFS;
                    productStandardTPA.Price_PriceLCImport_Lift = model.Price_PriceLCImport_Lift;
                    productStandardTPA.Currency_PriceLCImport_Lift = model.Currency_PriceLCImport_Lift;
                    productStandardTPA.Ratio_PriceLCImport_Lift = model.Ratio_PriceLCImport_Lift;
                    productStandardTPA.Amount_PriceLCImport_Lift = model.Amount_PriceLCImport_Lift;
                    productStandardTPA.Price_PriceLCImport_IF = model.Price_PriceLCImport_IF;
                    productStandardTPA.Currency_PriceLCImport_IF = model.Currency_PriceLCImport_IF;
                    productStandardTPA.Ratio_PriceLCImport_IF = model.Ratio_PriceLCImport_IF;
                    productStandardTPA.Amount_PriceLCImport_IF = model.Amount_PriceLCImport_IF;
                    productStandardTPA.Price_PriceLCImport_Other = model.Price_PriceLCImport_Other;
                    productStandardTPA.Currency_PriceLCImport_Other = model.Currency_PriceLCImport_Other;
                    productStandardTPA.Ratio_PriceLCImport_Other = model.Ratio_PriceLCImport_Other;
                    productStandardTPA.Amount_PriceLCImport_Other = model.Amount_PriceLCImport_Other;
                    productStandardTPA.HSCode = model.HSCode;
                    productStandardTPA.ImportTax = model.ImportTax;
                    productStandardTPA.ImportTaxPrice = model.ImportTaxPrice;
                    productStandardTPA.UpdateDateHSCode = model.UpdateDateHSCode;
                    productStandardTPA.NameTaxOther = model.NameTaxOther;
                    productStandardTPA.TaxOther = model.TaxOther;
                    productStandardTPA.ImportTaxPriceOther = model.ImportTaxPriceOther;
                    productStandardTPA.VAT = model.VAT;
                    productStandardTPA.PriceOther = model.PriceOther;
                    productStandardTPA.PriceFW = model.PriceFW;
                    productStandardTPA.Surcharge = model.Surcharge;
                    productStandardTPA.InventoryTime = model.InventoryTime;
                    productStandardTPA.BusinessDepartment = model.BusinessDepartment;
                    productStandardTPA.Index = model.Index;
                    productStandardTPA.Specifications = model.Specifications;
                    productStandardTPA.IsCOCQ = model.IsCOCQ;
                    productStandardTPA.Country = model.Country;
                    productStandardTPA.UpdateBy = model.UpdateBy;
                    productStandardTPA.UpdateDate = DateTime.Now;
                    productStandardTPA.Note1 = model.Note1;
                    productStandardTPA.Note2 = model.Note2;
                    productStandardTPA.Note3 = model.Note3;
                    productStandardTPA.Note4 = model.Note4;
                    productStandardTPA.Note5 = model.Note5;
                    productStandardTPA.Note6 = model.Note6;
                    productStandardTPA.Ratio_InsurrancePrice = model.Ratio_InsurrancePrice;
                    productStandardTPA.Profit_TPA = model.Profit_TPA;

                    productStandardTPA.PriceProduct_TPA = model.PriceProduct_TPA;
                    productStandardTPA.PriceVC_TPA = model.PriceVC_TPA;
                    productStandardTPA.ImportTaxPrice_TPA = model.ImportTaxPrice_TPA;
                    productStandardTPA.ImportTax_TPA = model.ImportTax_TPA;
                    productStandardTPA.TotalPrice = model.TotalPrice;

                    if (isEditPriceTPA)
                    {
                        productStandardTPA.ShortInterest = model.ShortInterest;
                        productStandardTPA.MidtermInterest = model.MidtermInterest;
                        productStandardTPA.VAT_TPA = model.VAT_TPA;
                        productStandardTPA.Interest_TPA = model.Interest_TPA;
                        productStandardTPA.Profit_TPA = model.Profit_TPA;
                        productStandardTPA.PriceEXW_TPA = model.PriceEXW_TPA;
                        productStandardTPA.UpdateDatePrice_TPA = model.UpdateDatePrice_TPA;
                        productStandardTPA.Price_L1 = model.Price_L1;
                        productStandardTPA.Price_L2 = model.Price_L2;
                        productStandardTPA.Price_L3 = model.Price_L3;
                        productStandardTPA.Price_L4 = model.Price_L4;
                        productStandardTPA.Price_L5 = model.Price_L5;
                    }

                    var productStandardTPAImages = db.ProductStandardTPAImages.Where(i => i.ProductStandardTPAId.Equals(model.Id)).ToList();
                    if (productStandardTPAImages.Count > 0)
                    {
                        db.ProductStandardTPAImages.RemoveRange(productStandardTPAImages);
                    }

                    if (model.ListImage.Count() > 0)
                    {
                        ProductStandardTPAImage productStandardTPAImage;
                        foreach (var item in model.ListImage)
                        {
                            productStandardTPAImage = new ProductStandardTPAImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductStandardTPAId = productStandardTPA.Id,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                ThumbnailPath = item.ThumbnailPath,
                                CreateBy = model.UpdateBy,
                                CreateDate = DateTime.Now
                            };
                            db.ProductStandardTPAImages.Add(productStandardTPAImage);
                        }
                    }

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_ClassRoom, newClassRoom.Id, newClassRoom.Code, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public void DeleteProductStandardTPA(ProductStandardTPAModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productStandardTPA = db.ProductStandardTPAs.FirstOrDefault(i => i.Id.Equals(model.Id));
                    if (productStandardTPA == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductStandardTPA);
                    }

                    var productStandardTPAFiles = db.ProductStandardTPAFiles.Where(i => i.ProductStandardTPAId.Equals(model.Id)).ToList();
                    if (productStandardTPAFiles.Count > 0)
                    {
                        db.ProductStandardTPAFiles.RemoveRange(productStandardTPAFiles);
                    }

                    var productStandardTPAImages = db.ProductStandardTPAImages.Where(i => i.ProductStandardTPAId.Equals(model.Id)).ToList();
                    if (productStandardTPAImages.Count > 0)
                    {
                        db.ProductStandardTPAImages.RemoveRange(productStandardTPAImages);
                    }

                    db.ProductStandardTPAs.Remove(productStandardTPA);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public object GetProductStandardTPAInfo(ProductStandardTPAModel model)
        {
            var resultInfo = db.ProductStandardTPAs.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(a => new ProductStandardTPAModel
            {
                Id = a.Id,
                EnglishName = a.EnglishName,
                VietNamName = a.VietNamName,
                Model = a.Model,
                TheFirm = a.TheFirm,
                ProductStandardTPATypeId = a.ProductStandardTPATypeId,
                Manufacture_NCC_SX = a.Manufacture_NCC_SX,
                Supplier_NCC_SX = a.Supplier_NCC_SX,
                Name_NCC_SX = a.Name_NCC_SX,
                Address_NCC_SX = a.Address_NCC_SX,
                PIC_NCC_SX = a.PIC_NCC_SX,
                PhoneNumber_NCC_SX = a.PhoneNumber_NCC_SX,
                Email_NCC_SX = a.Email_NCC_SX,
                Title_NCC_SX = a.Title_NCC_SX,
                BankPayment_NCC_SX = a.BankPayment_NCC_SX,
                TypePayment_NCC_SX = a.TypePayment_NCC_SX,
                RulesPayment_NCC_SX = a.RulesPayment_NCC_SX,
                RulesDelivery = a.RulesDelivery,
                DeliveryTime = a.DeliveryTime,
                Website_NCC_SX = a.Website_NCC_SX,
                Country_NCC_SX = a.Country_NCC_SX,
                ListedPrice = a.ListedPrice,
                PriceConformQuantity = a.PriceConformQuantity,
                PricePolicy = a.PricePolicy,
                MinimumQuantity = a.MinimumQuantity,
                MethodVC = a.MethodVC,
                LoadingPort = a.LoadingPort,
                PriceInSP_Price = a.PriceInSP_Price,
                PriceInPO_Price = a.PriceInPO_Price,
                Currency_PO = a.Currency_PO,
                Ratio_PO = a.Ratio_PO,
                Amout_PO = a.Amout_PO,
                Weight_PriceWord = a.Weight_PriceWord,
                PriceVCInWeight_PriceWord = a.PriceVCInWeight_PriceWord,
                VolumePricing_PriceWord = a.VolumePricing_PriceWord,
                CPVCInWeight_Price_PriceWord = a.CPVCInWeight_Price_PriceWord,
                CPVCInWeight_Currency_PriceWord = a.CPVCInWeight_Currency_PriceWord,
                CPVCInWeight_Ratio_PriceWord = a.CPVCInWeight_Ratio_PriceWord,
                CPVCInWeight_AmoutVND_PriceWord = a.CPVCInWeight_AmoutVND_PriceWord,
                Weight_Air_PriceWord = a.Weight_Air_PriceWord,
                MassConvertedBySize_Air_PriceWord = a.MassConvertedBySize_Air_PriceWord,
                VolumePricing_Air_PriceWord = a.VolumePricing_Air_PriceWord,
                Price_AirTransport_Air_PriceWord = a.Price_AirTransport_Air_PriceWord,
                Curency_AirTransport_Air_PriceWord = a.Curency_AirTransport_Air_PriceWord,
                Ratio_AirTransport_Air_PriceWord = a.Ratio_AirTransport_Air_PriceWord,
                Amount_AirTransport_Air_PriceWord = a.Amount_AirTransport_Air_PriceWord,
                PackingSize_D_PriceWord = a.PackingSize_D_PriceWord,
                PackingSize_R_PriceWord = a.PackingSize_R_PriceWord,
                PackingSize_C_PriceWord = a.PackingSize_C_PriceWord,
                PackingSize_Tolerance_PriceWord = a.PackingSize_Tolerance_PriceWord,
                PriceVCInCBM_PriceWord = a.PriceVCInCBM_PriceWord,
                Price_PriceVC_LCLInBCM_PriceWord = a.Price_PriceVC_LCLInBCM_PriceWord,
                Currency_PriceVC_LCLInBCM_PriceWord = a.Currency_PriceVC_LCLInBCM_PriceWord,
                Ratio_PriceVC_LCLInBCM_PriceWord = a.Ratio_PriceVC_LCLInBCM_PriceWord,
                Amout_PriceVC_LCLInBCM_PriceWord = a.Amout_PriceVC_LCLInBCM_PriceWord,
                Price_PriceVC_Cont20_PriceWord = a.Price_PriceVC_Cont20_PriceWord,
                Currency_PriceVC_Cont20_PriceWord = a.Currency_PriceVC_Cont20_PriceWord,
                Ratio_PriceVC_Cont20_PriceWord = a.Ratio_PriceVC_Cont20_PriceWord,
                Amout_PriceVC_Cont20_PriceWord = a.Amout_PriceVC_Cont20_PriceWord,
                Price_PriceVC_Cont20OT_PiceWord = a.Price_PriceVC_Cont20OT_PiceWord,
                Currency_PriceVC_Cont20OT_PiceWord = a.Currency_PriceVC_Cont20OT_PiceWord,
                Ratio_PriceVC_Cont20OT_PiceWord = a.Ratio_PriceVC_Cont20OT_PiceWord,
                Amount_PriceVC_Cont20OT_PiceWord = a.Amount_PriceVC_Cont20OT_PiceWord,
                Price_PriceVC_Cont40_PriceWord = a.Price_PriceVC_Cont40_PriceWord,
                Currency_PriceVC_Cont40_PriceWord = a.Currency_PriceVC_Cont40_PriceWord,
                Ratio_PriceVC_Cont40_PriceWord = a.Ratio_PriceVC_Cont40_PriceWord,
                Amount_PriceVC_Cont40_PriceWord = a.Amount_PriceVC_Cont40_PriceWord,
                Price_PriceVC_Cont40OT_PriceWord = a.Price_PriceVC_Cont40OT_PriceWord,
                Currency_PriceVC_Cont40OT_PriceWord = a.Currency_PriceVC_Cont40OT_PriceWord,
                Ratio_PriceVC_Cont40OT_PriceWord = a.Ratio_PriceVC_Cont40OT_PriceWord,
                Amount_PriceVC_Cont40OT_PriceWord = a.Amount_PriceVC_Cont40OT_PriceWord,
                UpdateDatePice_PriceWord = a.UpdateDatePice_PriceWord,
                Price_PriceLCExport = a.Price_PriceLCExport,
                Currency_PriceLCExport = a.Currency_PriceLCExport,
                Ratio_PriceLCExport = a.Ratio_PriceLCExport,
                Amount_PriceLCExport = a.Amount_PriceLCExport,
                InsurranceType = a.InsurranceType,
                Price_InsurrancePrice = a.Price_InsurrancePrice,
                Price_PriceLCImport_LSS = a.Price_PriceLCImport_LSS,
                Currency_PriceLCImport_LSS = a.Currency_PriceLCImport_LSS,
                Ratio_PriceLCImport_LSS = a.Ratio_PriceLCImport_LSS,
                Amount_PriceLCImport_LSS = a.Amount_PriceLCImport_LSS,
                Price_PriceLCImport_THC = a.Price_PriceLCImport_THC,
                Currency_PriceLCImport_THC = a.Currency_PriceLCImport_THC,
                Ratio_PriceLCImport_THC = a.Ratio_PriceLCImport_THC,
                Amount_PriceLCImport_THC = a.Amount_PriceLCImport_THC,
                Price_PriceLCImport_DO = a.Price_PriceLCImport_DO,
                Currency_PriceLCImport_DO = a.Currency_PriceLCImport_DO,
                Ratio_PriceLCImport_DO = a.Ratio_PriceLCImport_DO,
                Amount_PriceLCImport_DO = a.Amount_PriceLCImport_DO,
                Price_PriceLCImport_CIC = a.Price_PriceLCImport_CIC,
                Currency_PriceLCImport_CIC = a.Currency_PriceLCImport_CIC,
                Ratio_PriceLCImport_CIC = a.Ratio_PriceLCImport_CIC,
                Amount_PriceLCImport_CIC = a.Amount_PriceLCImport_CIC,
                Price_PriceLCImport_HL = a.Price_PriceLCImport_HL,
                Currency_PriceLCImport_HL = a.Currency_PriceLCImport_HL,
                Ratio_PriceLCImport_HL = a.Ratio_PriceLCImport_HL,
                Amount_PriceLCImport_HL = a.Amount_PriceLCImport_HL,
                Price_PriceLCImport_CLF = a.Price_PriceLCImport_CLF,
                Currency_PriceLCImport_CLF = a.Currency_PriceLCImport_CLF,
                Ratio_PriceLCImport_CLF = a.Ratio_PriceLCImport_CLF,
                Amount_PriceLCImport_CLF = a.Amount_PriceLCImport_CLF,
                Price_PriceLCImport_CFS = a.Price_PriceLCImport_CFS,
                Currency_PriceLCImport_CFS = a.Currency_PriceLCImport_CFS,
                Ratio_PriceLCImport_CFS = a.Ratio_PriceLCImport_CFS,
                Amount_PriceLCImport_CFS = a.Amount_PriceLCImport_CFS,
                Price_PriceLCImport_Lift = a.Price_PriceLCImport_Lift,
                Currency_PriceLCImport_Lift = a.Currency_PriceLCImport_Lift,
                Ratio_PriceLCImport_Lift = a.Ratio_PriceLCImport_Lift,
                Amount_PriceLCImport_Lift = a.Amount_PriceLCImport_Lift,
                Price_PriceLCImport_IF = a.Price_PriceLCImport_IF,
                Currency_PriceLCImport_IF = a.Currency_PriceLCImport_IF,
                Ratio_PriceLCImport_IF = a.Ratio_PriceLCImport_IF,
                Amount_PriceLCImport_IF = a.Amount_PriceLCImport_IF,
                Price_PriceLCImport_Other = a.Price_PriceLCImport_Other,
                Currency_PriceLCImport_Other = a.Currency_PriceLCImport_Other,
                Ratio_PriceLCImport_Other = a.Ratio_PriceLCImport_Other,
                Amount_PriceLCImport_Other = a.Amount_PriceLCImport_Other,
                HSCode = a.HSCode,
                ImportTax = a.ImportTax,
                ImportTaxPrice = a.ImportTaxPrice,
                UpdateDateHSCode = a.UpdateDateHSCode,
                NameTaxOther = a.NameTaxOther,
                TaxOther = a.TaxOther,
                ImportTaxPriceOther = a.ImportTaxPriceOther,
                VAT = a.VAT,
                PriceOther = a.PriceOther,
                PriceFW = a.PriceFW,
                Surcharge = a.Surcharge,
                InventoryTime = a.InventoryTime,
                ShortInterest = a.ShortInterest,
                MidtermInterest = a.MidtermInterest,
                PriceProduct_TPA = a.PriceProduct_TPA,
                PriceVC_TPA = a.PriceVC_TPA,
                ImportTaxPrice_TPA = a.ImportTaxPrice_TPA,
                ImportTax_TPA = a.ImportTax_TPA,
                VAT_TPA = a.VAT_TPA,
                Interest_TPA = a.Interest_TPA,
                TotalPrice = a.TotalPrice,
                Profit_TPA = a.Profit_TPA,
                PriceEXW_TPA = a.PriceEXW_TPA,
                UpdateDatePrice_TPA = a.UpdateDatePrice_TPA,
                Price_L1 = a.Price_L1,
                Price_L2 = a.Price_L2,
                Price_L3 = a.Price_L3,
                Price_L4 = a.Price_L4,
                Price_L5 = a.Price_L5,
                BusinessDepartment = a.BusinessDepartment,
                Index = a.Index,
                Specifications = a.Specifications,
                IsSendSale = a.IsSendSale,
                IsCOCQ = a.IsCOCQ,
                Country = a.Country,
                Note1 = a.Note1,
                Note2 = a.Note2,
                Note3 = a.Note3,
                Note4 = a.Note4,
                Note5 = a.Note5,
                Note6 = a.Note6,
                Ratio_InsurrancePrice = a.Ratio_InsurrancePrice
            }).FirstOrDefault();

            var listImage = (from a in db.ProductStandardTPAImages.AsNoTracking()
                             where a.ProductStandardTPAId.Equals(model.Id)
                             select new ProductStandardTPAImageModel
                             {
                                 Id = a.Id,
                                 FileName = a.FileName,
                                 FilePath = a.FilePath,
                                 ThumbnailPath = a.ThumbnailPath,
                             }).ToList();

            resultInfo.ListImage = listImage;

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProjectProduct);
            }

            return resultInfo;
        }

        public void ImportProductStandardTPA(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            string
                englishName_2,
                vietNamName_3,
                model_4,
                thefirm_5,
                typeMerchandise_6,
                manufacture_NCC_SX_7,
                supplier_NCC_SX_8,
                name_NCC_SX_9,
                address_NCC_SX_10,
                pIC_NCC_SX_11,
                phoneNumber_NCC_SX_12,
                email_NCC_SX_13,
                title_NCC_SX_14,
                bankPayment_NCC_SX_15,
                typePayment_NCC_SX_16,
                rulesPayment_NCC_SX_18,
                rulesDelivery_19,
                deliveryTime,
                country_NCC_SX,
                website_NCC_SX,
                listedPrice_21,
                priceConformQuantity_22,
                pricePolicy_23,
                minimumQuantity_24,
                methodVC_25,
                loadingPort_26,
                priceInSP_Price_27,
                priceInPO_Price,
                currency_PO,
                ratio_PO,
                amout_PO,
                weight_PriceWord,
                priceVCInWeight_PriceWord,
                volumePricing_PriceWord,
                cPVCInWeight_Price_PriceWord,
                cPVCInWeight_Currency_PriceWord,
                cPVCInWeight_Ratio_PriceWord,
                cPVCInWeight_AmoutVND_PriceWord,
                weight_Air_PriceWord,
                massConvertedBySize_Air_PriceWord,
                volumePricing_Air_PriceWord,
                price_AirTransport_Air_PriceWord,
                curency_AirTransport_Air_PriceWord,
                ratio_AirTransport_Air_PriceWord,
                amount_AirTransport_Air_PriceWord,
                packingSize_D_PriceWord,
                packingSize_R_PriceWord,
                packingSize_C_PriceWord,
                packingSize_Tolerance_PriceWord,
                packingSize_RealVolume_PriceWord,
                packingSize_PiceVolume_PriceWord,
                priceVCInCBM_PriceWord,
                price_PriceVC_LCLInBCM_PriceWord,
                currency_PriceVC_LCLInBCM_PriceWord,
                ratio_PriceVC_LCLInBCM_PriceWord,
                amout_PriceVC_LCLInBCM_PriceWord,
                price_PriceVC_Cont20_PriceWord,
                currency_PriceVC_Cont20_PriceWord,
                ratio_PriceVC_Cont20_PriceWord,
                amout_PriceVC_Cont20_PriceWord,
                price_PriceVC_Cont20OT_PiceWord,
                currency_PriceVC_Cont20OT_PiceWord,
                ratio_PriceVC_Cont20OT_PiceWord,
                amount_PriceVC_Cont20OT_PiceWord,
                price_PriceVC_Cont40_PriceWord,
                currency_PriceVC_Cont40_PriceWord,
                ratio_PriceVC_Cont40_PriceWord,
                amount_PriceVC_Cont40_PriceWord,
                price_PriceVC_Cont40OT_PriceWord,
                currency_PriceVC_Cont40OT_PriceWord,
                ratio_PriceVC_Cont40OT_PriceWord,
                amount_PriceVC_Cont40OT_PriceWord,
                updateDatePice_PriceWord,
                price_PriceLCExport,
                currency_PriceLCExport,
                ratio_PriceLCExport,
                amount_PriceLCExport,
                insurranceType,
                price_InsurrancePrice,
                price_PriceLCImport_LSS,
                currency_PriceLCImport_LSS,
                ratio_PriceLCImport_LSS,
                amount_PriceLCImport_LSS,
                price_PriceLCImport_THC,
                currency_PriceLCImport_THC,
                ratio_PriceLCImport_THC,
                amount_PriceLCImport_THC,
                price_PriceLCImport_DO,
                currency_PriceLCImport_DO,
                ratio_PriceLCImport_DO,
                amount_PriceLCImport_DO,
                price_PriceLCImport_CIC,
                currency_PriceLCImport_CIC,
                ratio_PriceLCImport_CIC,
                amount_PriceLCImport_CIC,
                price_PriceLCImport_HL,
                currency_PriceLCImport_HL,
                ratio_PriceLCImport_HL,
                amount_PriceLCImport_HL,
                price_PriceLCImport_CLF,
                currency_PriceLCImport_CLF,
                ratio_PriceLCImport_CLF,
                amount_PriceLCImport_CLF,
                price_PriceLCImport_CFS,
                currency_PriceLCImport_CFS,
                ratio_PriceLCImport_CFS,
                amount_PriceLCImport_CFS,
                price_PriceLCImport_Lift,
                currency_PriceLCImport_Lift,
                ratio_PriceLCImport_Lift,
                amount_PriceLCImport_Lift,
                price_PriceLCImport_IF,
                currency_PriceLCImport_IF,
                ratio_PriceLCImport_IF,
                amount_PriceLCImport_IF,
                price_PriceLCImport_Other,
                currency_PriceLCImport_Other,
                ratio_PriceLCImport_Other,
                amount_PriceLCImport_Other,
                hSCode_61,
                importTax_62,
                importTaxPrice_63,
                updateDateHSCode_64,
                nameTaxOther_65,
                taxOther_66,
                importTaxPriceOther_67,
                vat_68,
                priceOther_69,
                priceFW_70,
                surcharge_71,
                inventoryTime_72,
                shortInterest_73,
                midtermInterest_74,
                priceProduct_TPA_75,
                priceVC_TPA_76,
                importTaxPrice_TPA_77,
                importTax_TPA_78,
                vAT_TPA_79,
                interest_TPA_80,
                totalPrice_81,
                profit_TPA_82,
                priceEXW_TPA_83,
                updateDatePrice_TPA_84;

            #region
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();

            List<ProductStandardTPA> list = new List<ProductStandardTPA>();
            ProductStandardTPA itemC;
            var listData = db.ProductStandardTPAs.ToList();
            bool exist = false;
            ListModel listModels = new ListModel();
            ListDataModel model = new ListDataModel();

            List<int> rowEnglishName_2 = new List<int>();
            List<int> rowVietNamName_3 = new List<int>();
            List<int> rowModel_4 = new List<int>();
            List<int> rowTypeMerchandise_6 = new List<int>();
            List<int> rowEmail_NCC_SX_13 = new List<int>();
            List<int> rowTypePayment_NCC_SX = new List<int>();
            List<int> rowCurrency_NCC_SX = new List<int>();
            List<int> rowListedPrice = new List<int>();
            List<int> rowPricePolicy = new List<int>();
            List<int> rowMethodVC = new List<int>();
            List<int> rowPriceInSP_Price = new List<int>();
            List<int> rowPriceInPO_Price = new List<int>();
            List<int> rowRatio_PO = new List<int>();
            List<int> rowAmout_PO = new List<int>();
            List<int> rowWeight_PriceWord = new List<int>();
            List<int> rowPriceVCInWeight_PriceWord = new List<int>();
            List<int> rowUpdateDatePrice_Weight_PriceWord = new List<int>();
            List<int> rowCPVCInWeight_PriceWord = new List<int>();
            List<int> rowCPVCInWeight_Ratio_PriceWord = new List<int>();
            List<int> rowCPVCInWeight_AmoutVND_PriceWord = new List<int>();
            List<int> rowWeight_Air_PriceWord = new List<int>();
            List<int> rowMassConvertedBySize_Air_PriceWord = new List<int>();
            List<int> rowVolumePricing_Air_PriceWord = new List<int>();
            List<int> rowPrice_AirTransport_Air_PriceWord = new List<int>();
            List<int> rowRatio_AirTransport_Air_PriceWord = new List<int>();
            List<int> rowAmount_AirTransport_Air_PriceWord = new List<int>();
            List<int> rowPackingSize_D_PriceWord = new List<int>();
            List<int> rowPackingSize_R_PriceWord = new List<int>();
            List<int> rowPackingSize_C_PriceWord = new List<int>();
            List<int> rowPackingSize_Tolerance_PriceWord = new List<int>();
            List<int> rowPackingSize_RealVolume_PriceWord = new List<int>();
            List<int> rowPackingSize_PiceVolume_PriceWord = new List<int>();
            List<int> rowPriceVCInCBM_PriceWord = new List<int>();
            List<int> rowPriceVC_LCLInBCM_PriceWord = new List<int>();
            List<int> rowRatio_PriceVC_LCLInBCM_PriceWord = new List<int>();
            List<int> rowAmout_PriceVC_LCLInBCM_PriceWord = new List<int>();
            List<int> rowPriceVC_Cont20_PriceWord = new List<int>();
            List<int> rowRatio_PriceVC_Cont20_PriceWord = new List<int>();
            List<int> rowAmout_PriceVC_Cont20_PriceWord = new List<int>();
            List<int> rowPriceVC_Cont20OT_PiceWord = new List<int>();
            List<int> rowRatio_PriceVC_Cont20OT_PiceWord = new List<int>();
            List<int> rowAmount_PriceVC_Cont20OT_PiceWord = new List<int>();
            List<int> rowPriceVC_Cont40_PriceWord = new List<int>();
            List<int> rowRatio_PriceVC_Cont40_PriceWord = new List<int>();
            List<int> rowAmount_PriceVC_Cont40_PriceWord = new List<int>();
            List<int> rowPriceVC_Cont40OT_PriceWord = new List<int>();
            List<int> rowRatio_PriceVC_Cont40OT_PriceWord = new List<int>();
            List<int> rowAmount_PriceVC_Cont40OT_PriceWord = new List<int>();
            List<int> rowUpdateDatePice_Cont40OT_PriceWord = new List<int>();
            List<int> rowPriceLCExport = new List<int>();
            List<int> rowRatio_PriceLCExport = new List<int>();
            List<int> rowAmount_PriceLCExport = new List<int>();
            List<int> rowInsurrancePrice = new List<int>();
            List<int> rowPriceLCImport_LSS = new List<int>();
            List<int> rowRatio_PriceLCImport_LSS = new List<int>();
            List<int> rowAmount_PriceLCImport_LSS = new List<int>();
            List<int> rowPriceLCImport_THC = new List<int>();
            List<int> rowRatio_PriceLCImport_THC = new List<int>();
            List<int> rowAmount_PriceLCImport_THC = new List<int>();
            List<int> rowPriceLCImport_DO = new List<int>();
            List<int> rowRatio_PriceLCImport_DO = new List<int>();
            List<int> rowAmount_PriceLCImport_DO = new List<int>();
            List<int> rowPriceLCImport_CIC = new List<int>();
            List<int> rowRatio_PriceLCImport_CIC = new List<int>();
            List<int> rowAmount_PriceLCImport_CIC = new List<int>();
            List<int> rowPriceLCImport_HL = new List<int>();
            List<int> rowRatio_PriceLCImport_HL = new List<int>();
            List<int> rowAmount_PriceLCImport_HL = new List<int>();
            List<int> rowPriceLCImport_CLF = new List<int>();
            List<int> rowRatio_PriceLCImport_CLF = new List<int>();
            List<int> rowAmount_PriceLCImport_CLF = new List<int>();
            List<int> rowPriceLCImport_CFS = new List<int>();
            List<int> rowRatio_PriceLCImport_CFS = new List<int>();
            List<int> rowAmount_PriceLCImport_CFS = new List<int>();
            List<int> rowPriceLCImport_Lift = new List<int>();
            List<int> rowRatio_PriceLCImport_Lift = new List<int>();
            List<int> rowAmount_PriceLCImport_Lift = new List<int>();
            List<int> rowPriceLCImport_IF = new List<int>();
            List<int> rowRatio_PriceLCImport_IF = new List<int>();
            List<int> rowAmount_PriceLCImport_IF = new List<int>();
            List<int> rowPriceLCImport_Other = new List<int>();
            List<int> rowRatio_PriceLCImport_Other = new List<int>();
            List<int> rowAmount_PriceLCImport_Other = new List<int>();
            List<int> rowImportTax = new List<int>();
            List<int> rowImportTaxPrice = new List<int>();
            List<int> rowUpdateDateHSCode = new List<int>();
            List<int> rowTaxOther = new List<int>();
            List<int> rowImportTaxPriceOther = new List<int>();
            List<int> rowVAT = new List<int>();
            List<int> rowPriceOther = new List<int>();
            List<int> rowPriceFW = new List<int>();
            List<int> rowSurcharge = new List<int>();
            List<int> rowInventoryTime = new List<int>();
            List<int> rowShortInterest = new List<int>();
            List<int> rowMidtermInterest = new List<int>();
            List<int> rowPriceProduct_TPA = new List<int>();
            List<int> rowPriceVC_TPA = new List<int>();
            List<int> rowImportTaxPrice_TPA = new List<int>();
            List<int> rowImportTax_TPA = new List<int>();
            List<int> rowVAT_TPA = new List<int>();
            List<int> rowInterest_TPA = new List<int>();
            List<int> rowTotalPrice = new List<int>();
            List<int> rowProfit_TPA = new List<int>();
            List<int> rowPriceEXW_TPA = new List<int>();
            List<int> rowUpdateDatePrice_TPA = new List<int>();

            try
            {
                ProductStandardTPAType productStandardTPAType = null;
                var productStandardTPATypes = db.ProductStandardTPATypes.AsNoTracking().ToList();
                for (int i = 5; i <= rowCount; i++)
                {
                    if (string.IsNullOrEmpty(sheet[i, 1].Value))
                    {
                        continue;
                    }

                    exist = false;
                    itemC = new ProductStandardTPA();
                    englishName_2 = sheet[i, 2].Value;
                    vietNamName_3 = sheet[i, 3].Value;
                    model_4 = sheet[i, 4].Value;
                    thefirm_5 = sheet[i, 5].Value;
                    typeMerchandise_6 = sheet[i, 6].Value;
                    manufacture_NCC_SX_7 = sheet[i, 7].Value;
                    supplier_NCC_SX_8 = sheet[i, 8].Value;
                    name_NCC_SX_9 = sheet[i, 9].Value;
                    address_NCC_SX_10 = sheet[i, 10].Value;
                    pIC_NCC_SX_11 = sheet[i, 11].Value;
                    phoneNumber_NCC_SX_12 = sheet[i, 12].Value;
                    email_NCC_SX_13 = sheet[i, 13].Value;
                    title_NCC_SX_14 = sheet[i, 14].Value;
                    bankPayment_NCC_SX_15 = sheet[i, 15].Value;
                    typePayment_NCC_SX_16 = sheet[i, 16].Value;
                    rulesPayment_NCC_SX_18 = sheet[i, 17].Value;
                    rulesDelivery_19 = sheet[i, 18].Value;
                    deliveryTime = sheet[i, 19].Value;
                    country_NCC_SX = sheet[i, 20].Value;
                    website_NCC_SX = sheet[i, 21].Value;
                    listedPrice_21 = sheet[i, 22].Value;
                    priceConformQuantity_22 = sheet[i, 23].Value;
                    pricePolicy_23 = sheet[i, 24].Value;
                    minimumQuantity_24 = sheet[i, 25].Value;
                    methodVC_25 = sheet[i, 26].Value;
                    loadingPort_26 = sheet[i, 27].Value;
                    priceInSP_Price_27 = sheet[i, 28].Value;
                    priceInPO_Price = sheet[i, 29].Value;
                    currency_PO = sheet[i, 30].Value;
                    ratio_PO = sheet[i, 31].Value;
                    amout_PO = sheet[i, 32].CalculatedValue;
                    weight_PriceWord = sheet[i, 33].Value;
                    priceVCInWeight_PriceWord = sheet[i, 34].CalculatedValue;
                    volumePricing_PriceWord = sheet[i, 35].CalculatedValue;
                    cPVCInWeight_Price_PriceWord = sheet[i, 36].Value;
                    cPVCInWeight_Currency_PriceWord = sheet[i, 37].Value;
                    cPVCInWeight_Ratio_PriceWord = sheet[i, 38].Value;
                    cPVCInWeight_AmoutVND_PriceWord = sheet[i, 39].CalculatedValue;
                    weight_Air_PriceWord = sheet[i, 40].Value;
                    massConvertedBySize_Air_PriceWord = sheet[i, 41].CalculatedValue;
                    volumePricing_Air_PriceWord = sheet[i, 42].CalculatedValue;
                    price_AirTransport_Air_PriceWord = sheet[i, 43].Value;
                    curency_AirTransport_Air_PriceWord = sheet[i, 44].Value;
                    ratio_AirTransport_Air_PriceWord = sheet[i, 45].Value;
                    amount_AirTransport_Air_PriceWord = sheet[i, 46].CalculatedValue;
                    packingSize_D_PriceWord = sheet[i, 47].CalculatedValue;
                    if (string.IsNullOrEmpty(packingSize_D_PriceWord))
                    {
                        packingSize_D_PriceWord = sheet[i, 47].Value;
                    }
                    packingSize_R_PriceWord = sheet[i, 48].CalculatedValue;
                    if (string.IsNullOrEmpty(packingSize_R_PriceWord))
                    {
                        packingSize_R_PriceWord = sheet[i, 48].Value;
                    }
                    packingSize_C_PriceWord = sheet[i, 49].CalculatedValue;
                    if (string.IsNullOrEmpty(packingSize_C_PriceWord))
                    {
                        packingSize_C_PriceWord = sheet[i, 49].Value;
                    }
                    packingSize_Tolerance_PriceWord = sheet[i, 50].Value;
                    packingSize_RealVolume_PriceWord = sheet[i, 51].CalculatedValue;
                    packingSize_PiceVolume_PriceWord = sheet[i, 52].CalculatedValue;
                    priceVCInCBM_PriceWord = sheet[i, 53].CalculatedValue;
                    price_PriceVC_LCLInBCM_PriceWord = sheet[i, 54].Value;
                    currency_PriceVC_LCLInBCM_PriceWord = sheet[i, 55].Value;
                    ratio_PriceVC_LCLInBCM_PriceWord = sheet[i, 56].Value;
                    amout_PriceVC_LCLInBCM_PriceWord = sheet[i, 57].CalculatedValue;
                    price_PriceVC_Cont20_PriceWord = sheet[i, 58].Value;
                    currency_PriceVC_Cont20_PriceWord = sheet[i, 59].Value;
                    ratio_PriceVC_Cont20_PriceWord = sheet[i, 60].Value;
                    amout_PriceVC_Cont20_PriceWord = sheet[i, 61].CalculatedValue;
                    price_PriceVC_Cont20OT_PiceWord = sheet[i, 62].Value;
                    currency_PriceVC_Cont20OT_PiceWord = sheet[i, 63].Value;
                    ratio_PriceVC_Cont20OT_PiceWord = sheet[i, 64].Value;
                    amount_PriceVC_Cont20OT_PiceWord = sheet[i, 65].CalculatedValue;
                    price_PriceVC_Cont40_PriceWord = sheet[i, 66].Value;
                    currency_PriceVC_Cont40_PriceWord = sheet[i, 67].Value;
                    ratio_PriceVC_Cont40_PriceWord = sheet[i, 68].Value;
                    amount_PriceVC_Cont40_PriceWord = sheet[i, 69].CalculatedValue;
                    price_PriceVC_Cont40OT_PriceWord = sheet[i, 70].Value;
                    currency_PriceVC_Cont40OT_PriceWord = sheet[i, 71].Value;
                    ratio_PriceVC_Cont40OT_PriceWord = sheet[i, 72].Value;
                    amount_PriceVC_Cont40OT_PriceWord = sheet[i, 73].CalculatedValue;
                    updateDatePice_PriceWord = sheet[i, 74].Value;
                    price_PriceLCExport = sheet[i, 75].CalculatedValue;
                    currency_PriceLCExport = sheet[i, 76].Value;
                    ratio_PriceLCExport = sheet[i, 77].Value;
                    amount_PriceLCExport = sheet[i, 78].CalculatedValue;
                    insurranceType = sheet[i, 79].Value;
                    price_InsurrancePrice = sheet[i, 80].CalculatedValue;
                    price_PriceLCImport_LSS = sheet[i, 81].CalculatedValue;
                    currency_PriceLCImport_LSS = sheet[i, 82].Value;
                    ratio_PriceLCImport_LSS = sheet[i, 83].Value;
                    amount_PriceLCImport_LSS = sheet[i, 84].CalculatedValue;
                    price_PriceLCImport_THC = sheet[i, 85].CalculatedValue;
                    currency_PriceLCImport_THC = sheet[i, 86].Value;
                    ratio_PriceLCImport_THC = sheet[i, 87].Value;
                    amount_PriceLCImport_THC = sheet[i, 88].CalculatedValue;
                    price_PriceLCImport_DO = sheet[i, 89].CalculatedValue;
                    currency_PriceLCImport_DO = sheet[i, 90].Value;
                    ratio_PriceLCImport_DO = sheet[i, 91].Value;
                    amount_PriceLCImport_DO = sheet[i, 92].CalculatedValue;
                    price_PriceLCImport_CIC = sheet[i, 93].CalculatedValue;
                    currency_PriceLCImport_CIC = sheet[i, 94].Value;
                    ratio_PriceLCImport_CIC = sheet[i, 95].Value;
                    amount_PriceLCImport_CIC = sheet[i, 96].CalculatedValue;
                    price_PriceLCImport_HL = sheet[i, 97].CalculatedValue;
                    currency_PriceLCImport_HL = sheet[i, 98].Value;
                    ratio_PriceLCImport_HL = sheet[i, 99].Value;
                    amount_PriceLCImport_HL = sheet[i, 100].CalculatedValue;
                    price_PriceLCImport_CLF = sheet[i, 101].CalculatedValue;
                    currency_PriceLCImport_CLF = sheet[i, 102].Value;
                    ratio_PriceLCImport_CLF = sheet[i, 103].Value;
                    amount_PriceLCImport_CLF = sheet[i, 104].CalculatedValue;
                    price_PriceLCImport_CFS = sheet[i, 105].CalculatedValue;
                    currency_PriceLCImport_CFS = sheet[i, 106].Value;
                    ratio_PriceLCImport_CFS = sheet[i, 107].Value;
                    amount_PriceLCImport_CFS = sheet[i, 108].CalculatedValue;
                    price_PriceLCImport_Lift = sheet[i, 109].CalculatedValue;
                    currency_PriceLCImport_Lift = sheet[i, 110].Value;
                    ratio_PriceLCImport_Lift = sheet[i, 111].Value;
                    amount_PriceLCImport_Lift = sheet[i, 112].CalculatedValue;
                    price_PriceLCImport_IF = sheet[i, 113].CalculatedValue;
                    currency_PriceLCImport_IF = sheet[i, 114].Value;
                    ratio_PriceLCImport_IF = sheet[i, 115].Value;
                    amount_PriceLCImport_IF = sheet[i, 116].CalculatedValue;
                    price_PriceLCImport_Other = sheet[i, 117].CalculatedValue;
                    currency_PriceLCImport_Other = sheet[i, 118].Value;
                    ratio_PriceLCImport_Other = sheet[i, 119].Value;
                    amount_PriceLCImport_Other = sheet[i, 120].CalculatedValue;
                    hSCode_61 = sheet[i, 121].Value;
                    importTax_62 = sheet[i, 122].Value;
                    importTaxPrice_63 = sheet[i, 123].CalculatedValue;
                    updateDateHSCode_64 = sheet[i, 124].Value;
                    nameTaxOther_65 = sheet[i, 125].Value;
                    taxOther_66 = sheet[i, 126].Value;
                    importTaxPriceOther_67 = sheet[i, 127].CalculatedValue;
                    vat_68 = sheet[i, 128].Value;
                    priceOther_69 = sheet[i, 129].CalculatedValue;
                    priceFW_70 = sheet[i, 130].CalculatedValue;
                    surcharge_71 = sheet[i, 131].CalculatedValue;
                    inventoryTime_72 = sheet[i, 132].Value;
                    shortInterest_73 = sheet[i, 133].Value;
                    midtermInterest_74 = sheet[i, 134].Value;
                    priceProduct_TPA_75 = sheet[i, 135].CalculatedValue;
                    priceVC_TPA_76 = sheet[i, 136].CalculatedValue;
                    importTaxPrice_TPA_77 = sheet[i, 137].CalculatedValue;
                    importTax_TPA_78 = sheet[i, 138].CalculatedValue;
                    vAT_TPA_79 = sheet[i, 139].Value;
                    interest_TPA_80 = sheet[i, 140].Value;
                    totalPrice_81 = sheet[i, 141].CalculatedValue;
                    profit_TPA_82 = sheet[i, 142].Value;
                    priceEXW_TPA_83 = sheet[i, 143].Value;
                    updateDatePrice_TPA_84 = sheet[i, 144].Value;

                    itemC = listData.FirstOrDefault(a => a.Model.ToUpper().Equals(model_4.ToUpper()));
                    if (itemC == null)
                    {
                        exist = true;
                        itemC = new ProductStandardTPA();
                        itemC.Id = Guid.NewGuid().ToString();
                    }

                    // 2
                    if (!string.IsNullOrEmpty(englishName_2))
                    {
                        itemC.EnglishName = englishName_2;
                    }
                    else
                    {
                        rowEnglishName_2.Add(i);
                    }

                    // 3
                    if (!string.IsNullOrEmpty(vietNamName_3))
                    {
                        itemC.VietNamName = vietNamName_3;
                    }
                    else
                    {
                        rowVietNamName_3.Add(i);
                    }

                    // 4
                    if (!string.IsNullOrEmpty(model_4))
                    {
                        itemC.Model = model_4;
                    }
                    else
                    {
                        rowModel_4.Add(i);
                    }

                    // 5
                    if (!string.IsNullOrEmpty(thefirm_5))
                    {
                        itemC.TheFirm = thefirm_5;
                    }

                    // 6
                    if (!string.IsNullOrEmpty(typeMerchandise_6))
                    {
                        productStandardTPAType = productStandardTPATypes.FirstOrDefault(r => r.Name.ToUpper().Equals(typeMerchandise_6.Trim().ToUpper()));

                        //if (typeMerchandise_6.ToUpper().Trim().Equals(Constants.ProductStandardTPA_TypeMerchandise_Accessories.ToUpper()))
                        //{
                        //    itemC.TypeMerchandise = Constants.ProductStandardTPA_TypeMerchandise_AccessoriesInt;
                        //}
                        //else if (typeMerchandise_6.ToUpper().Trim().Equals(Constants.ProductStandardTPA_TypeMerchandise_Machine.ToUpper()))
                        //{
                        //    itemC.TypeMerchandise = Constants.ProductStandardTPA_TypeMerchandise_MachineInt;
                        //}

                        if (productStandardTPAType != null)
                        {
                            itemC.ProductStandardTPATypeId = productStandardTPAType.Id;
                        }
                        else
                        {
                            rowTypeMerchandise_6.Add(i);
                        }
                    }

                    // 7
                    if (!string.IsNullOrEmpty(manufacture_NCC_SX_7))
                    {
                        itemC.Manufacture_NCC_SX = manufacture_NCC_SX_7;
                    }

                    // 8
                    if (!string.IsNullOrEmpty(supplier_NCC_SX_8))
                    {
                        itemC.Supplier_NCC_SX = supplier_NCC_SX_8;
                    }

                    // 9
                    if (!string.IsNullOrEmpty(name_NCC_SX_9))
                    {
                        itemC.Name_NCC_SX = name_NCC_SX_9;
                    }

                    // 10
                    if (!string.IsNullOrEmpty(address_NCC_SX_10))
                    {
                        itemC.Address_NCC_SX = address_NCC_SX_10;
                    }

                    // 11
                    if (!string.IsNullOrEmpty(pIC_NCC_SX_11))
                    {
                        itemC.PIC_NCC_SX = pIC_NCC_SX_11;
                    }

                    // 12
                    if (!string.IsNullOrEmpty(phoneNumber_NCC_SX_12))
                    {
                        itemC.PhoneNumber_NCC_SX = phoneNumber_NCC_SX_12.Trim();
                    }

                    // 13
                    try
                    {
                        if (!string.IsNullOrEmpty(email_NCC_SX_13))
                        {
                            //var email = new System.Net.Mail.MailAddress(email_NCC_SX_13);
                            itemC.Email_NCC_SX = email_NCC_SX_13;
                        }
                    }
                    catch (Exception)
                    {
                        rowEmail_NCC_SX_13.Add(i);
                        continue;
                    }

                    // 14
                    if (!string.IsNullOrEmpty(title_NCC_SX_14))
                    {
                        itemC.Title_NCC_SX = title_NCC_SX_14;
                    }

                    // 15
                    if (!string.IsNullOrEmpty(bankPayment_NCC_SX_15))
                    {
                        itemC.BankPayment_NCC_SX = bankPayment_NCC_SX_15;
                    }

                    // 16
                    if (!string.IsNullOrEmpty(typePayment_NCC_SX_16))
                    {
                        model = listModels.ListTypePayment_NCC_SX.FirstOrDefault(a => a.Name.ToUpper().Equals(typePayment_NCC_SX_16.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.TypePayment_NCC_SX = model.Id;
                        }
                        else
                        {
                            rowTypePayment_NCC_SX.Add(i);
                        }
                    }

                    // 17
                    if (!string.IsNullOrEmpty(rulesPayment_NCC_SX_18))
                    {
                        itemC.RulesPayment_NCC_SX = rulesPayment_NCC_SX_18;
                    }

                    // 18
                    if (!string.IsNullOrEmpty(rulesDelivery_19))
                    {
                        model = listModels.ListRulesDelivery_NCC_SX.FirstOrDefault(a => a.Name.ToUpper().Equals(rulesDelivery_19.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.RulesDelivery = model.Id;
                        }
                    }

                    if (!string.IsNullOrEmpty(deliveryTime))
                    {
                        itemC.DeliveryTime = deliveryTime;
                    }

                    if (!string.IsNullOrEmpty(country_NCC_SX))
                    {
                        itemC.Country_NCC_SX = country_NCC_SX;
                    }

                    if (!string.IsNullOrEmpty(website_NCC_SX))
                    {
                        itemC.Website_NCC_SX = website_NCC_SX;
                    }


                    // 20
                    try
                    {
                        if (!string.IsNullOrEmpty(listedPrice_21))
                        {
                            itemC.ListedPrice = Convert.ToDecimal(listedPrice_21);
                        }
                    }
                    catch (Exception)
                    {
                        rowListedPrice.Add(i);
                        continue;
                    }

                    // 21
                    if (!string.IsNullOrEmpty(priceConformQuantity_22))
                    {
                        itemC.PriceConformQuantity = priceConformQuantity_22;
                    }

                    if (!string.IsNullOrEmpty(pricePolicy_23))
                    {
                        itemC.PricePolicy = pricePolicy_23;
                    }

                    // 22
                    try
                    {
                        if (!string.IsNullOrEmpty(minimumQuantity_24))
                        {
                            itemC.MinimumQuantity = Convert.ToInt32(minimumQuantity_24);
                        }
                    }
                    catch (Exception)
                    {
                        rowPricePolicy.Add(i);
                        continue;
                    }

                    // 23
                    if (!string.IsNullOrEmpty(methodVC_25))
                    {
                        if (methodVC_25.Trim().ToUpper().Equals("Biển".ToUpper()))
                        {
                            itemC.MethodVC = 1;
                        }
                        else if (methodVC_25.Trim().ToUpper().Equals("Bộ".ToUpper()))
                        {
                            itemC.MethodVC = 2;
                        }
                        else if (methodVC_25.Trim().ToUpper().Equals("Hàng không".ToUpper()))
                        {
                            itemC.MethodVC = 3;
                        }
                        else
                        {
                            rowMethodVC.Add(i);
                        }
                    }

                    // 24
                    if (!string.IsNullOrEmpty(loadingPort_26))
                    {
                        itemC.LoadingPort = loadingPort_26;
                    }

                    // 25
                    try
                    {
                        if (!string.IsNullOrEmpty(priceInSP_Price_27))
                        {
                            itemC.PriceInSP_Price = Convert.ToDecimal(priceInSP_Price_27);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceInSP_Price.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(priceInPO_Price))
                        {
                            itemC.PriceInPO_Price = Convert.ToDecimal(priceInPO_Price);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceInPO_Price.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PO))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PO.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PO = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PO))
                        {
                            itemC.Ratio_PO = Convert.ToDecimal(ratio_PO);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PO.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amout_PO))
                        {
                            itemC.Amout_PO = Convert.ToDecimal(amout_PO);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmout_PO.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(weight_PriceWord))
                        {
                            itemC.Weight_PriceWord = Convert.ToDecimal(weight_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowWeight_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(priceVCInWeight_PriceWord))
                        {
                            itemC.PriceVCInWeight_PriceWord = Convert.ToDecimal(priceVCInWeight_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVCInWeight_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(volumePricing_PriceWord))
                        {
                            itemC.VolumePricing_PriceWord = Convert.ToDecimal(volumePricing_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowUpdateDatePrice_Weight_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(cPVCInWeight_Price_PriceWord))
                        {
                            itemC.CPVCInWeight_Price_PriceWord = Convert.ToDecimal(cPVCInWeight_Price_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowCPVCInWeight_PriceWord.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(cPVCInWeight_Currency_PriceWord))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(cPVCInWeight_Currency_PriceWord.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.CPVCInWeight_Currency_PriceWord = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(cPVCInWeight_Ratio_PriceWord))
                        {
                            itemC.CPVCInWeight_Ratio_PriceWord = Convert.ToDecimal(cPVCInWeight_Ratio_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowCPVCInWeight_Ratio_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(cPVCInWeight_AmoutVND_PriceWord))
                        {
                            itemC.CPVCInWeight_AmoutVND_PriceWord = Convert.ToDecimal(cPVCInWeight_AmoutVND_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowCPVCInWeight_AmoutVND_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(weight_Air_PriceWord))
                        {
                            itemC.Weight_Air_PriceWord = Convert.ToDecimal(weight_Air_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowWeight_Air_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(massConvertedBySize_Air_PriceWord))
                        {
                            itemC.MassConvertedBySize_Air_PriceWord = Convert.ToDecimal(massConvertedBySize_Air_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowMassConvertedBySize_Air_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(volumePricing_Air_PriceWord))
                        {
                            itemC.VolumePricing_Air_PriceWord = Convert.ToDecimal(volumePricing_Air_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowVolumePricing_Air_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_AirTransport_Air_PriceWord))
                        {
                            itemC.Price_AirTransport_Air_PriceWord = Convert.ToDecimal(price_AirTransport_Air_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPrice_AirTransport_Air_PriceWord.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(curency_AirTransport_Air_PriceWord))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.Equals(curency_AirTransport_Air_PriceWord));
                        if (model != null)
                        {
                            itemC.Curency_AirTransport_Air_PriceWord = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_AirTransport_Air_PriceWord))
                        {
                            itemC.Ratio_AirTransport_Air_PriceWord = Convert.ToDecimal(ratio_AirTransport_Air_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_AirTransport_Air_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_AirTransport_Air_PriceWord))
                        {
                            itemC.Amount_AirTransport_Air_PriceWord = Convert.ToDecimal(amount_AirTransport_Air_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_AirTransport_Air_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(packingSize_D_PriceWord))
                        {
                            itemC.PackingSize_D_PriceWord = Convert.ToDecimal(packingSize_D_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPackingSize_D_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(packingSize_R_PriceWord))
                        {
                            itemC.PackingSize_R_PriceWord = Convert.ToDecimal(packingSize_R_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPackingSize_R_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(packingSize_C_PriceWord))
                        {
                            itemC.PackingSize_C_PriceWord = Convert.ToDecimal(packingSize_C_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPackingSize_C_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(packingSize_Tolerance_PriceWord))
                        {
                            itemC.PackingSize_Tolerance_PriceWord = Convert.ToDecimal(packingSize_Tolerance_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPackingSize_Tolerance_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(packingSize_RealVolume_PriceWord))
                        {
                            itemC.PackingSize_RealVolume_PriceWord = Convert.ToDecimal(packingSize_RealVolume_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPackingSize_RealVolume_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(packingSize_PiceVolume_PriceWord))
                        {
                            itemC.PackingSize_PiceVolume_PriceWord = Convert.ToDecimal(packingSize_PiceVolume_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPackingSize_PiceVolume_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(priceVCInCBM_PriceWord))
                        {
                            itemC.PriceVCInCBM_PriceWord = Convert.ToDecimal(priceVCInCBM_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVCInCBM_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceVC_LCLInBCM_PriceWord))
                        {
                            itemC.Price_PriceVC_LCLInBCM_PriceWord = Convert.ToDecimal(price_PriceVC_LCLInBCM_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVC_LCLInBCM_PriceWord.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceVC_LCLInBCM_PriceWord))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceVC_LCLInBCM_PriceWord.Trim().ToUpper()));
                        itemC.Currency_PriceVC_LCLInBCM_PriceWord = model.Id;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceVC_LCLInBCM_PriceWord))
                        {
                            itemC.Ratio_PriceVC_LCLInBCM_PriceWord = Convert.ToDecimal(ratio_PriceVC_LCLInBCM_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceVC_LCLInBCM_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amout_PriceVC_LCLInBCM_PriceWord))
                        {
                            itemC.Amout_PriceVC_LCLInBCM_PriceWord = Convert.ToDecimal(amout_PriceVC_LCLInBCM_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmout_PriceVC_LCLInBCM_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceVC_Cont20_PriceWord))
                        {
                            itemC.Price_PriceVC_Cont20_PriceWord = Convert.ToDecimal(price_PriceVC_Cont20_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVC_Cont20_PriceWord.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceVC_Cont20_PriceWord))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceVC_Cont20_PriceWord.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceVC_Cont20_PriceWord = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceVC_Cont20_PriceWord))
                        {
                            itemC.Ratio_PriceVC_Cont20_PriceWord = Convert.ToDecimal(ratio_PriceVC_Cont20_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceVC_Cont20_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amout_PriceVC_Cont20_PriceWord))
                        {
                            itemC.Amout_PriceVC_Cont20_PriceWord = Convert.ToDecimal(amout_PriceVC_Cont20_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmout_PriceVC_Cont20_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceVC_Cont20OT_PiceWord))
                        {
                            itemC.Price_PriceVC_Cont20OT_PiceWord = Convert.ToDecimal(price_PriceVC_Cont20OT_PiceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVC_Cont20OT_PiceWord.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceVC_Cont20OT_PiceWord))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceVC_Cont20OT_PiceWord.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceVC_Cont20OT_PiceWord = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceVC_Cont20OT_PiceWord))
                        {
                            itemC.Ratio_PriceVC_Cont20OT_PiceWord = Convert.ToDecimal(ratio_PriceVC_Cont20OT_PiceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceVC_Cont20OT_PiceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceVC_Cont20OT_PiceWord))
                        {
                            itemC.Amount_PriceVC_Cont20OT_PiceWord = Convert.ToDecimal(amount_PriceVC_Cont20OT_PiceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceVC_Cont20OT_PiceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceVC_Cont40_PriceWord))
                        {
                            itemC.Price_PriceVC_Cont40_PriceWord = Convert.ToDecimal(price_PriceVC_Cont40_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVC_Cont40_PriceWord.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceVC_Cont40_PriceWord))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceVC_Cont40_PriceWord.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceVC_Cont40_PriceWord = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceVC_Cont40_PriceWord))
                        {
                            itemC.Ratio_PriceVC_Cont40_PriceWord = Convert.ToDecimal(ratio_PriceVC_Cont40_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceVC_Cont40_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceVC_Cont40_PriceWord))
                        {
                            itemC.Amount_PriceVC_Cont40_PriceWord = Convert.ToDecimal(amount_PriceVC_Cont40_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceVC_Cont40_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceVC_Cont40OT_PriceWord))
                        {
                            itemC.Price_PriceVC_Cont40OT_PriceWord = Convert.ToDecimal(price_PriceVC_Cont40OT_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVC_Cont40OT_PriceWord.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceVC_Cont40OT_PriceWord))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceVC_Cont40OT_PriceWord.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceVC_Cont40OT_PriceWord = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceVC_Cont40OT_PriceWord))
                        {
                            itemC.Ratio_PriceVC_Cont40OT_PriceWord = Convert.ToDecimal(ratio_PriceVC_Cont40OT_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceVC_Cont40OT_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceVC_Cont40OT_PriceWord))
                        {
                            itemC.Amount_PriceVC_Cont40OT_PriceWord = Convert.ToDecimal(amount_PriceVC_Cont40OT_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceVC_Cont40OT_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(updateDatePice_PriceWord))
                        {
                            itemC.UpdateDatePice_PriceWord = Convert.ToDateTime(updateDatePice_PriceWord);
                        }
                    }
                    catch (Exception)
                    {
                        rowUpdateDatePice_Cont40OT_PriceWord.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCExport))
                        {
                            itemC.Price_PriceLCExport = Convert.ToDecimal(price_PriceLCExport);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCExport.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCExport))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCExport.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCExport = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCExport))
                        {
                            itemC.Ratio_PriceLCExport = Convert.ToDecimal(ratio_PriceLCExport);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCExport.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCExport))
                        {
                            itemC.Amount_PriceLCExport = Convert.ToDecimal(amount_PriceLCExport);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCExport.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(insurranceType))
                    {
                        itemC.InsurranceType = insurranceType;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_InsurrancePrice))
                        {
                            itemC.Price_InsurrancePrice = Convert.ToDecimal(price_InsurrancePrice);
                        }
                    }
                    catch (Exception)
                    {
                        rowInsurrancePrice.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_LSS))
                        {
                            itemC.Price_PriceLCImport_LSS = Convert.ToDecimal(price_PriceLCImport_LSS);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_LSS.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_LSS))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_LSS.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_LSS = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_LSS))
                        {
                            itemC.Ratio_PriceLCImport_LSS = Convert.ToDecimal(ratio_PriceLCImport_LSS);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_LSS.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_LSS))
                        {
                            itemC.Amount_PriceLCImport_LSS = Convert.ToDecimal(amount_PriceLCImport_LSS);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_LSS.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_THC))
                        {
                            itemC.Price_PriceLCImport_THC = Convert.ToDecimal(price_PriceLCImport_THC);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_THC.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_THC))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_THC.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_THC = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_THC))
                        {
                            itemC.Ratio_PriceLCImport_THC = Convert.ToDecimal(ratio_PriceLCImport_THC);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_THC.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_THC))
                        {
                            itemC.Amount_PriceLCImport_THC = Convert.ToDecimal(amount_PriceLCImport_THC);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_THC.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_DO))
                        {
                            itemC.Price_PriceLCImport_DO = Convert.ToDecimal(price_PriceLCImport_DO);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_DO.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_DO))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_DO.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_DO = model.Id; ;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_DO))
                        {
                            itemC.Ratio_PriceLCImport_DO = Convert.ToDecimal(ratio_PriceLCImport_DO);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_DO.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_DO))
                        {
                            itemC.Amount_PriceLCImport_DO = Convert.ToDecimal(amount_PriceLCImport_DO);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_DO.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_CIC))
                        {
                            itemC.Price_PriceLCImport_CIC = Convert.ToDecimal(price_PriceLCImport_CIC);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_CIC.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_CIC))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_CIC.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_CIC = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_CIC))
                        {
                            itemC.Ratio_PriceLCImport_CIC = Convert.ToDecimal(ratio_PriceLCImport_CIC);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_CIC.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_CIC))
                        {
                            itemC.Amount_PriceLCImport_CIC = Convert.ToDecimal(amount_PriceLCImport_CIC);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_CIC.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_HL))
                        {
                            itemC.Price_PriceLCImport_HL = Convert.ToDecimal(price_PriceLCImport_HL);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_HL.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_HL))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_HL.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_HL = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_HL))
                        {
                            itemC.Ratio_PriceLCImport_HL = Convert.ToDecimal(ratio_PriceLCImport_HL);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_HL.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_HL))
                        {
                            itemC.Amount_PriceLCImport_HL = Convert.ToDecimal(amount_PriceLCImport_HL);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_HL.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_CLF))
                        {
                            itemC.Price_PriceLCImport_CLF = Convert.ToDecimal(price_PriceLCImport_CLF);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_CLF.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_CLF))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_CLF.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_CLF = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_CLF))
                        {
                            itemC.Ratio_PriceLCImport_CLF = Convert.ToDecimal(ratio_PriceLCImport_CLF);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_CLF.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_CLF))
                        {
                            itemC.Amount_PriceLCImport_CLF = Convert.ToDecimal(amount_PriceLCImport_CLF);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_CLF.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_CFS))
                        {
                            itemC.Price_PriceLCImport_CFS = Convert.ToDecimal(price_PriceLCImport_CFS);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_CFS.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_CFS))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_CFS.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_CFS = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_CFS))
                        {
                            itemC.Ratio_PriceLCImport_CFS = Convert.ToDecimal(ratio_PriceLCImport_CFS);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_CFS.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_CFS))
                        {
                            itemC.Amount_PriceLCImport_CFS = Convert.ToDecimal(amount_PriceLCImport_CFS);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_CFS.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_Lift))
                        {
                            itemC.Price_PriceLCImport_Lift = Convert.ToDecimal(price_PriceLCImport_Lift);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_Lift.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_Lift))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_Lift.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_Lift = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_Lift))
                        {
                            itemC.Ratio_PriceLCImport_Lift = Convert.ToDecimal(ratio_PriceLCImport_Lift);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_Lift.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_Lift))
                        {
                            itemC.Amount_PriceLCImport_Lift = Convert.ToDecimal(amount_PriceLCImport_Lift);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_Lift.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_IF))
                        {
                            itemC.Price_PriceLCImport_IF = Convert.ToDecimal(price_PriceLCImport_IF);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_IF.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_IF))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_IF.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_IF = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_IF))
                        {
                            itemC.Ratio_PriceLCImport_IF = Convert.ToDecimal(ratio_PriceLCImport_IF);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_IF.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_IF))
                        {
                            itemC.Amount_PriceLCImport_IF = Convert.ToDecimal(amount_PriceLCImport_IF);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_IF.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(price_PriceLCImport_Other))
                        {
                            itemC.Price_PriceLCImport_Other = Convert.ToDecimal(price_PriceLCImport_Other);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceLCImport_Other.Add(i);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currency_PriceLCImport_Other))
                    {
                        model = listModels.ListCurency.FirstOrDefault(a => a.Name.ToUpper().Equals(currency_PriceLCImport_Other.Trim().ToUpper()));
                        if (model != null)
                        {
                            itemC.Currency_PriceLCImport_Other = model.Id;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ratio_PriceLCImport_Other))
                        {
                            itemC.Ratio_PriceLCImport_Other = Convert.ToDecimal(ratio_PriceLCImport_Other);
                        }
                    }
                    catch (Exception)
                    {
                        rowRatio_PriceLCImport_Other.Add(i);
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(amount_PriceLCImport_Other))
                        {
                            itemC.Amount_PriceLCImport_Other = Convert.ToDecimal(amount_PriceLCImport_Other);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount_PriceLCImport_Other.Add(i);
                        continue;
                    }

                    // 59
                    if (!string.IsNullOrEmpty(hSCode_61))
                    {
                        itemC.HSCode = hSCode_61;
                    }

                    // 60
                    try
                    {
                        if (!string.IsNullOrEmpty(importTax_62))
                        {
                            itemC.ImportTax = Convert.ToDecimal(importTax_62);
                        }
                    }
                    catch (Exception)
                    {
                        rowImportTax.Add(i);
                        continue;
                    }

                    // 61
                    try
                    {
                        if (!string.IsNullOrEmpty(importTaxPrice_63))
                        {
                            itemC.ImportTaxPrice = Convert.ToDecimal(importTaxPrice_63);
                        }
                    }
                    catch (Exception)
                    {
                        rowImportTaxPrice.Add(i);
                        continue;
                    }

                    // 62
                    try
                    {
                        if (!string.IsNullOrEmpty(updateDateHSCode_64))
                        {
                            itemC.UpdateDateHSCode = Convert.ToDateTime(updateDateHSCode_64);
                        }
                    }
                    catch (Exception)
                    {
                        rowUpdateDateHSCode.Add(i);
                        continue;
                    }

                    // 63
                    if (!string.IsNullOrEmpty(nameTaxOther_65))
                    {
                        itemC.NameTaxOther = nameTaxOther_65;
                    }

                    // 64
                    try
                    {
                        if (!string.IsNullOrEmpty(taxOther_66))
                        {
                            itemC.TaxOther = Convert.ToDecimal(taxOther_66);
                        }
                    }
                    catch (Exception)
                    {
                        rowTaxOther.Add(i);
                        continue;
                    }

                    // 65
                    try
                    {
                        if (!string.IsNullOrEmpty(importTaxPriceOther_67))
                        {
                            itemC.ImportTaxPriceOther = Convert.ToDecimal(importTaxPriceOther_67);
                        }
                    }
                    catch (Exception)
                    {
                        rowImportTaxPriceOther.Add(i);
                        continue;
                    }

                    // 66
                    try
                    {
                        if (!string.IsNullOrEmpty(vat_68))
                        {
                            itemC.VAT = Convert.ToDecimal(vat_68);
                        }
                    }
                    catch (Exception)
                    {
                        rowVAT.Add(i);
                        continue;
                    }

                    // 67
                    try
                    {
                        if (!string.IsNullOrEmpty(priceOther_69))
                        {
                            itemC.PriceOther = Convert.ToDecimal(priceOther_69);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceOther.Add(i);
                        continue;
                    }

                    // 68
                    try
                    {
                        if (!string.IsNullOrEmpty(priceFW_70))
                        {
                            itemC.PriceFW = Convert.ToDecimal(priceFW_70);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceFW.Add(i);
                        continue;
                    }

                    // 69
                    try
                    {
                        if (!string.IsNullOrEmpty(surcharge_71))
                        {
                            itemC.Surcharge = Convert.ToDecimal(surcharge_71);
                        }
                    }
                    catch (Exception)
                    {
                        rowSurcharge.Add(i);
                        continue;
                    }

                    // 70
                    try
                    {
                        if (!string.IsNullOrEmpty(inventoryTime_72))
                        {
                            itemC.InventoryTime = Convert.ToInt32(inventoryTime_72);
                        }
                    }
                    catch (Exception)
                    {
                        rowInventoryTime.Add(i);
                        continue;
                    }

                    // 71
                    try
                    {
                        if (!string.IsNullOrEmpty(shortInterest_73))
                        {
                            itemC.ShortInterest = Convert.ToDecimal(shortInterest_73);
                        }
                    }
                    catch (Exception)
                    {
                        rowShortInterest.Add(i);
                        continue;
                    }

                    // 72
                    try
                    {
                        if (!string.IsNullOrEmpty(midtermInterest_74))
                        {
                            itemC.MidtermInterest = Convert.ToDecimal(midtermInterest_74);
                        }
                    }
                    catch (Exception)
                    {
                        rowMidtermInterest.Add(i);
                        continue;
                    }

                    // 73
                    try
                    {
                        if (!string.IsNullOrEmpty(priceProduct_TPA_75))
                        {
                            itemC.PriceProduct_TPA = Convert.ToDecimal(priceProduct_TPA_75);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceProduct_TPA.Add(i);
                        continue;
                    }

                    // 74
                    try
                    {
                        if (!string.IsNullOrEmpty(priceVC_TPA_76))
                        {
                            itemC.PriceVC_TPA = Convert.ToDecimal(priceVC_TPA_76);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceVC_TPA.Add(i);
                        continue;
                    }

                    // 75
                    try
                    {
                        if (!string.IsNullOrEmpty(importTaxPrice_TPA_77))
                        {
                            itemC.ImportTaxPrice_TPA = Convert.ToDecimal(importTaxPrice_TPA_77);
                        }
                    }
                    catch (Exception)
                    {
                        rowImportTaxPrice_TPA.Add(i);
                        continue;
                    }

                    // 76
                    try
                    {
                        if (!string.IsNullOrEmpty(importTax_TPA_78))
                        {
                            itemC.ImportTax_TPA = Convert.ToDecimal(importTax_TPA_78);
                        }
                    }
                    catch (Exception)
                    {
                        rowImportTax_TPA.Add(i);
                        continue;
                    }

                    // 77
                    try
                    {
                        if (!string.IsNullOrEmpty(vAT_TPA_79))
                        {
                            itemC.VAT_TPA = Convert.ToDecimal(vAT_TPA_79);
                        }
                    }
                    catch (Exception)
                    {
                        rowVAT_TPA.Add(i);
                        continue;
                    }

                    // 78
                    try
                    {
                        if (!string.IsNullOrEmpty(interest_TPA_80))
                        {
                            itemC.Interest_TPA = Convert.ToDecimal(interest_TPA_80);
                        }
                    }
                    catch (Exception)
                    {
                        rowInterest_TPA.Add(i);
                        continue;
                    }

                    // 79
                    try
                    {
                        if (!string.IsNullOrEmpty(totalPrice_81))
                        {
                            itemC.TotalPrice = Convert.ToDecimal(totalPrice_81);
                        }
                    }
                    catch (Exception)
                    {
                        rowTotalPrice.Add(i);
                        continue;
                    }

                    // 80
                    try
                    {
                        if (!string.IsNullOrEmpty(profit_TPA_82))
                        {
                            itemC.Profit_TPA = Convert.ToDecimal(profit_TPA_82);
                        }
                    }
                    catch (Exception)
                    {
                        rowProfit_TPA.Add(i);
                        continue;
                    }

                    // 81
                    try
                    {
                        if (!string.IsNullOrEmpty(priceEXW_TPA_83))
                        {
                            itemC.PriceEXW_TPA = Convert.ToDecimal(priceEXW_TPA_83);
                        }
                    }
                    catch (Exception)
                    {
                        rowPriceEXW_TPA.Add(i);
                        continue;
                    }

                    // 82
                    try
                    {
                        if (!string.IsNullOrEmpty(updateDatePrice_TPA_84))
                        {
                            itemC.UpdateDatePrice_TPA = Convert.ToDateTime(updateDatePrice_TPA_84);
                        }
                    }
                    catch (Exception)
                    {
                        rowUpdateDatePrice_TPA.Add(i);
                        continue;
                    }

                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;

                    if (exist)
                    {
                        list.Add(itemC);
                    }
                }

                #endregion

                if (rowEnglishName_2.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên tên tiếng anh dòng <" + string.Join(", ", rowEnglishName_2) + "> không được để trống!");
                }

                if (rowVietNamName_3.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên tên tiếng việt dòng <" + string.Join(", ", rowVietNamName_3) + "> không được để trống!");
                }

                if (rowModel_4.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã thiết bị chuẩn dòng <" + string.Join(", ", rowModel_4) + "> không được để trống!");
                }

                if (rowTypeMerchandise_6.Count > 0)
                {
                    throw NTSException.CreateInstance("Chủng loại hàng hóa dòng <" + string.Join(", ", rowTypeMerchandise_6) + "> không đúng!");
                }

                if (rowEmail_NCC_SX_13.Count > 0)
                {
                    throw NTSException.CreateInstance("Email dòng <" + string.Join(", ", rowEmail_NCC_SX_13) + "> không đúng định dạng!");
                }

                if (rowTypePayment_NCC_SX.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại hình thanh toán dòng <" + string.Join(", ", rowTypePayment_NCC_SX) + "> không đúng!");
                }

                if (rowCurrency_NCC_SX.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại tiền tệ dòng <" + string.Join(", ", rowCurrency_NCC_SX) + "> không đúng!");
                }

                if (rowListedPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá niêm yết dòng <" + string.Join(", ", rowListedPrice) + "> không đúng định dạng!");
                }

                if (rowPricePolicy.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng tối thiểu dòng <" + string.Join(", ", rowPricePolicy) + "> không đúng định dạng!");
                }

                if (rowMethodVC.Count > 0)
                {
                    throw NTSException.CreateInstance("Phương thức vận chuyển dòng <" + string.Join(", ", rowMethodVC) + "> không đúng định dạng!");
                }

                if (rowPriceInSP_Price.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá theo sản phẩm dòng <" + string.Join(", ", rowPriceInSP_Price) + "> không đúng định dạng!");
                }

                if (rowPriceInPO_Price.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá theo PO dòng <" + string.Join(", ", rowPriceInPO_Price) + "> không đúng định dạng!");
                }

                if (rowRatio_PO.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ lệ theo PO dòng <" + string.Join(", ", rowRatio_PO) + "> không đúng định dạng!");
                }

                if (rowAmout_PO.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ theo PO dòng <" + string.Join(", ", rowAmout_PO) + "> không đúng định dạng!");
                }

                if (rowWeight_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Trọng lượng chi phí chuyển phát nhanh dòng <" + string.Join(", ", rowWeight_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPriceVCInWeight_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Khối lượng quy đổi theo kích thước dòng <" + string.Join(", ", rowPriceVCInWeight_PriceWord) + "> không đúng định dạng!");
                }

                if (rowUpdateDatePrice_Weight_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Khối lượng để tính giá chi phí chuyển phát nhanh dòng <" + string.Join(", ", rowUpdateDatePrice_Weight_PriceWord) + "> không đúng định dạng!");
                }

                if (rowCPVCInWeight_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Chi phí vận chuển theo trọng lượng chuyển phát nhanh dòng <" + string.Join(", ", rowCPVCInWeight_PriceWord) + "> không đúng định dạng!");
                }

                if (rowCPVCInWeight_Ratio_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá vận chuển theo trọng lượng chuyển phát nhanh dòng <" + string.Join(", ", rowCPVCInWeight_Ratio_PriceWord) + "> không đúng định dạng!");
                }

                if (rowCPVCInWeight_AmoutVND_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ vận chuển theo trọng lượng chuyển phát nhanh dòng <" + string.Join(", ", rowCPVCInWeight_AmoutVND_PriceWord) + "> không đúng định dạng!");
                }

                if (rowWeight_Air_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Trọng lượng chi phí Air Cargo dòng <" + string.Join(", ", rowWeight_Air_PriceWord) + "> không đúng định dạng!");
                }

                if (rowMassConvertedBySize_Air_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Khối lượng quy đổi theo kích thước chi phí Air Cargo dòng <" + string.Join(", ", rowMassConvertedBySize_Air_PriceWord) + "> không đúng định dạng!");
                }

                if (rowVolumePricing_Air_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Khối lượng để tính giá chi phí Air Cargo dòng <" + string.Join(", ", rowVolumePricing_Air_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPrice_AirTransport_Air_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá chi phí chuyển phát nhanh theo trọng lượng dòng <" + string.Join(", ", rowPrice_AirTransport_Air_PriceWord) + "> không đúng định dạng!");
                }

                if (rowRatio_AirTransport_Air_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá chi phí chuyển phát nhanh theo trọng lượng dòng <" + string.Join(", ", rowRatio_AirTransport_Air_PriceWord) + "> không đúng định dạng!");
                }

                if (rowAmount_AirTransport_Air_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ chi phí chuyển phát nhanh theo trọng lượng dòng <" + string.Join(", ", rowAmount_AirTransport_Air_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPackingSize_D_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Kích thước đóng gói D dòng <" + string.Join(", ", rowPackingSize_D_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPackingSize_R_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Kích thước đóng gói R dòng <" + string.Join(", ", rowPackingSize_R_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPackingSize_C_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Kích thước đóng gói C dòng <" + string.Join(", ", rowPackingSize_C_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPackingSize_Tolerance_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Thể tích thực tế dòng <" + string.Join(", ", rowPackingSize_Tolerance_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPackingSize_RealVolume_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Thể tích để tính giá dòng <" + string.Join(", ", rowPackingSize_RealVolume_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPackingSize_PiceVolume_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Dung sai đóng gói dòng <" + string.Join(", ", rowPackingSize_PiceVolume_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPriceVCInCBM_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("1,000,000,000 dòng <" + string.Join(", ", rowPriceVCInCBM_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPriceVC_LCLInBCM_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá chi phí vận chuyển LCL theo CBM dòng <" + string.Join(", ", rowPriceVC_LCLInBCM_PriceWord) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceVC_LCLInBCM_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá chi phí vận chuyển LCL theo CBM dòng <" + string.Join(", ", rowRatio_PriceVC_LCLInBCM_PriceWord) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceVC_LCLInBCM_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ chi phí vận chuyển LCL theo CBM dòng <" + string.Join(", ", rowRatio_PriceVC_LCLInBCM_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPriceVC_Cont20_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá vận chuyển CONT 20 dòng <" + string.Join(", ", rowPriceVC_Cont20_PriceWord) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceVC_Cont20_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá vận chuyển CONT 20 dòng <" + string.Join(", ", rowRatio_PriceVC_Cont20_PriceWord) + "> không đúng định dạng!");
                }

                if (rowAmout_PriceVC_Cont20_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ vận chuyển CONT 20 dòng <" + string.Join(", ", rowAmout_PriceVC_Cont20_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPriceVC_Cont20OT_PiceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá vận chuyển CONT 20 OT dòng <" + string.Join(", ", rowPriceVC_Cont20OT_PiceWord) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceVC_Cont20OT_PiceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá vận chuyển CONT 20 OT dòng <" + string.Join(", ", rowRatio_PriceVC_Cont20OT_PiceWord) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceVC_Cont20OT_PiceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ vận chuyển CONT 20 OT dòng <" + string.Join(", ", rowAmount_PriceVC_Cont20OT_PiceWord) + "> không đúng định dạng!");
                }

                if (rowPriceVC_Cont40_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá vận chuyển CONT 40 dòng <" + string.Join(", ", rowPriceVC_Cont40_PriceWord) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceVC_Cont40_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá vận chuyển CONT 40 dòng <" + string.Join(", ", rowRatio_PriceVC_Cont40_PriceWord) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceVC_Cont40_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ vận chuyển CONT 40 dòng <" + string.Join(", ", rowAmount_PriceVC_Cont40_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPriceVC_Cont40OT_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Chi phí vận chuyển CONT 40 OT dòng <" + string.Join(", ", rowPriceVC_Cont40OT_PriceWord) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceVC_Cont40OT_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá vận chuyển CONT 40 OT dòng <" + string.Join(", ", rowRatio_PriceVC_Cont40OT_PriceWord) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceVC_Cont40_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ vận chuyển CONT 40 OT dòng <" + string.Join(", ", rowAmount_PriceVC_Cont40_PriceWord) + "> không đúng định dạng!");
                }

                if (rowUpdateDatePice_Cont40OT_PriceWord.Count > 0)
                {
                    throw NTSException.CreateInstance("Ngày cập nhật chi phí vận chuyển quốc tế dòng <" + string.Join(", ", rowUpdateDatePice_Cont40OT_PriceWord) + "> không đúng định dạng!");
                }

                if (rowPriceLCExport.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu xuất dòng <" + string.Join(", ", rowPriceLCExport) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCExport.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu xuất dòng <" + string.Join(", ", rowRatio_PriceLCExport) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCExport.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu xuất dòng <" + string.Join(", ", rowAmount_PriceLCExport) + "> không đúng định dạng!");
                }

                if (rowInsurrancePrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí bảo hiểm dòng <" + string.Join(", ", rowInsurrancePrice) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_LSS.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập LSS dòng <" + string.Join(", ", rowPriceLCImport_LSS) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_LSS.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập LSS dòng <" + string.Join(", ", rowRatio_PriceLCImport_LSS) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_LSS.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập LSS dòng <" + string.Join(", ", rowAmount_PriceLCImport_LSS) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_THC.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập THC dòng <" + string.Join(", ", rowPriceLCImport_THC) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_THC.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập THC dòng <" + string.Join(", ", rowRatio_PriceLCImport_THC) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_THC.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập THC dòng <" + string.Join(", ", rowAmount_PriceLCImport_THC) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_DO.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập DO dòng <" + string.Join(", ", rowPriceLCImport_DO) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_DO.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập DO dòng <" + string.Join(", ", rowRatio_PriceLCImport_DO) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_DO.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập DO dòng <" + string.Join(", ", rowAmount_PriceLCImport_DO) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_CIC.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập CIC dòng <" + string.Join(", ", rowPriceLCImport_CIC) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_CIC.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập CIC dòng <" + string.Join(", ", rowRatio_PriceLCImport_CIC) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_CIC.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập CIC dòng <" + string.Join(", ", rowAmount_PriceLCImport_CIC) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_HL.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập HL dòng <" + string.Join(", ", rowPriceLCImport_HL) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_HL.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập HL dòng <" + string.Join(", ", rowRatio_PriceLCImport_HL) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_HL.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập HL dòng <" + string.Join(", ", rowAmount_PriceLCImport_HL) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_CLF.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập CLF dòng <" + string.Join(", ", rowPriceLCImport_CLF) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_CLF.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập CLF dòng <" + string.Join(", ", rowRatio_PriceLCImport_CLF) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_CLF.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập CLF dòng <" + string.Join(", ", rowAmount_PriceLCImport_CLF) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_CFS.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập CFS dòng <" + string.Join(", ", rowPriceLCImport_CFS) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_CFS.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập CFS dòng <" + string.Join(", ", rowRatio_PriceLCImport_CFS) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_CFS.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập CFS dòng <" + string.Join(", ", rowAmount_PriceLCImport_CFS) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_Lift.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập Lift dòng <" + string.Join(", ", rowPriceLCImport_Lift) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_Lift.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập Lift dòng <" + string.Join(", ", rowRatio_PriceLCImport_Lift) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_Lift.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập Lift dòng <" + string.Join(", ", rowAmount_PriceLCImport_Lift) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_IF.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập IF dòng <" + string.Join(", ", rowPriceLCImport_IF) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_IF.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập IF dòng <" + string.Join(", ", rowRatio_PriceLCImport_IF) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_IF.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập IF dòng <" + string.Join(", ", rowAmount_PriceLCImport_IF) + "> không đúng định dạng!");
                }

                if (rowPriceLCImport_Other.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá phí LC đầu nhập khác dòng <" + string.Join(", ", rowPriceLCImport_Other) + "> không đúng định dạng!");
                }

                if (rowRatio_PriceLCImport_Other.Count > 0)
                {
                    throw NTSException.CreateInstance("Tỷ giá phí LC đầu nhập khác dòng <" + string.Join(", ", rowRatio_PriceLCImport_Other) + "> không đúng định dạng!");
                }

                if (rowAmount_PriceLCImport_Other.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá VNĐ phí LC đầu nhập khác dòng <" + string.Join(", ", rowAmount_PriceLCImport_Other) + "> không đúng định dạng!");
                }

                if (rowImportTax.Count > 0)
                {
                    throw NTSException.CreateInstance("Thuế nhập khẩu dòng <" + string.Join(", ", rowImportTax) + "> không đúng định dạng!");
                }

                if (rowImportTaxPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Chi phí thuế nhập khẩu dòng <" + string.Join(", ", rowImportTaxPrice) + "> không đúng định dạng!");
                }

                if (rowUpdateDateHSCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Ngày cập nhật mã HS dòng <" + string.Join(", ", rowUpdateDateHSCode) + "> không đúng định dạng!");
                }

                if (rowTaxOther.Count > 0)
                {
                    throw NTSException.CreateInstance("Thuế khác dòng <" + string.Join(", ", rowTaxOther) + "> không đúng định dạng!");
                }

                if (rowImportTaxPriceOther.Count > 0)
                {
                    throw NTSException.CreateInstance("Chi phí thuế nhập khẩu khác dòng <" + string.Join(", ", rowImportTaxPriceOther) + "> không đúng định dạng!");
                }

                if (rowVAT.Count > 0)
                {
                    throw NTSException.CreateInstance("VAT dòng <" + string.Join(", ", rowVAT) + "> không đúng định dạng!");
                }

                if (rowPriceOther.Count > 0)
                {
                    throw NTSException.CreateInstance("Phí khác theo yêu cầu hải quan dòng <" + string.Join(", ", rowPriceOther) + "> không đúng định dạng!");
                }

                if (rowPriceFW.Count > 0)
                {
                    throw NTSException.CreateInstance("Phí FW dòng <" + string.Join(", ", rowPriceFW) + "> không đúng định dạng!");
                }

                if (rowSurcharge.Count > 0)
                {
                    throw NTSException.CreateInstance("Phụ phí phát sinh dòng <" + string.Join(", ", rowSurcharge) + "> không đúng định dạng!");
                }

                if (rowInventoryTime.Count > 0)
                {
                    throw NTSException.CreateInstance("Thời gian tồn kho dòng <" + string.Join(", ", rowInventoryTime) + "> không đúng định dạng!");
                }

                if (rowShortInterest.Count > 0)
                {
                    throw NTSException.CreateInstance("Lãi suất vay ngắn hạn dòng <" + string.Join(", ", rowShortInterest) + "> không đúng định dạng!");
                }

                if (rowMidtermInterest.Count > 0)
                {
                    throw NTSException.CreateInstance("Lãi suất vay trung hạn dòng <" + string.Join(", ", rowMidtermInterest) + "> không đúng định dạng!");
                }

                if (rowPriceProduct_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá sản phẩm TPA dòng <" + string.Join(", ", rowPriceProduct_TPA) + "> không đúng định dạng!");
                }

                if (rowPriceVC_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Phí vận chuyển EXW TPA dòng <" + string.Join(", ", rowPriceVC_TPA) + "> không đúng định dạng!");
                }

                if (rowImportTaxPrice_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Chi phí nhập khẩu EXW dòng <" + string.Join(", ", rowImportTaxPrice_TPA) + "> không đúng định dạng!");
                }

                if (rowImportTax_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Thuế nhập khẩu EXW dòng <" + string.Join(", ", rowImportTax_TPA) + "> không đúng định dạng!");
                }

                if (rowVAT_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("VAT EXW TPA dòng <" + string.Join(", ", rowVAT_TPA) + "> không đúng định dạng!");
                }

                if (rowInterest_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Lãi vay EXW TPA dòng <" + string.Join(", ", rowInterest_TPA) + "> không đúng định dạng!");
                }

                if (rowTotalPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Tổng chi phí dòng <" + string.Join(", ", rowTotalPrice) + "> không đúng định dạng!");
                }

                if (rowProfit_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Lợi nhuận dòng <" + string.Join(", ", rowProfit_TPA) + "> không đúng định dạng!");
                }

                if (rowPriceEXW_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá bán EXW TPA dòng <" + string.Join(", ", rowPriceEXW_TPA) + "> không đúng định dạng!");
                }

                if (rowUpdateDatePrice_TPA.Count > 0)
                {
                    throw NTSException.CreateInstance("Ngày cập nhật giá EXW TPA dòng <" + string.Join(", ", rowUpdateDatePrice_TPA) + "> không đúng định dạng!");
                }

                db.ProductStandardTPAs.AddRange(list);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                //fs.Close();
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(model, ex);
            }

            //fs.Close();
            workbook.Close();
            excelEngine.Dispose();
        }

        public void UploadFile(ProductStandardTPAFileCreateModel model)
        {
            var productStandardTPAFiles = db.ProductStandardTPAFiles.Where(i => i.ProductStandardTPAId.Equals(model.ProductStandardTPAId)).ToList();
            if (productStandardTPAFiles.Count > 0)
            {
                db.ProductStandardTPAFiles.RemoveRange(productStandardTPAFiles);
            }

            var documentObject = db.DocumentObjects.Where(a => a.ObjectId.Equals(model.ProductStandardTPAId)).ToList();
            if (documentObject.Count > 0)
            {
                db.DocumentObjects.RemoveRange(documentObject);
            }

            if (model.ListFile.Count() > 0)
            {
                foreach (var item in model.ListFile)
                {
                    if (!item.IsDocument)
                    {
                        ProductStandardTPAFile productStandardTPAFile = new ProductStandardTPAFile()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductStandardTPAId = model.ProductStandardTPAId,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            FileSize = item.FileSize,
                            Note = item.Note,
                            Type = item.Type,
                            CreateBy = model.CreateBy,
                            CreateDate = DateTime.Now,
                            DocumentTemplateId = item.DocumentTemplateId
                        };

                        db.ProductStandardTPAFiles.Add(productStandardTPAFile);
                    }
                    else
                    {
                        DocumentObject newDocumentObject = new DocumentObject()
                        {
                            Id = Guid.NewGuid().ToString(),
                            DocumentId = item.Id,
                            ObjectId = model.ProductStandardTPAId,
                            ObjectType = Constants.ObjectType_Device_Imported
                        };
                        db.DocumentObjects.Add(newDocumentObject);
                    }

                }
            }

            db.SaveChanges();
        }

        public object GetListProductStandardTPAFile(ProductStandardTPAFileCreateModel model)
        {
            var list = (from a in db.ProductStandardTPAFiles.AsNoTracking()
                        where a.ProductStandardTPAId.Equals(model.ProductStandardTPAId)
                        orderby a.FileName
                        select new ProductStandardTPAFileModel
                        {
                            Id = a.Id,
                            FileName = a.FileName,
                            FilePath = a.FilePath,
                            FileSize = a.FileSize,
                            Note = a.Note,
                            Type = a.Type,
                            CreateBy = a.CreateBy,
                            CreateDate = a.CreateDate,
                            DocumentTemplateId = a.DocumentTemplateId
                        }).ToList();

            var listCatolog = list.Where(i => i.Type == Constants.ProductStandardTPAFile_Type_Catolog).ToList();
            var listFile = list.Where(i => i.Type == Constants.ProductStandardTPAFile_Type_File).ToList();
            var listFileCOCQ = list.Where(i => i.Type == Constants.ProductStandardTPAFile_Type_COCQ).ToList();

            var documentObjectFile = (from a in db.DocumentObjects.AsNoTracking()
                                      join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                      join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                      where a.ObjectId.Equals(model.ProductStandardTPAId)
                                      select new ProductStandardTPAFileModel
                                      {
                                          Id = a.DocumentId,
                                          FileName = b.Name,
                                          Note = b.Description,
                                          IsDocument = true,
                                          DocumentGroupCode = c.Code
                                      }).ToList();

            listFile.AddRange(documentObjectFile.Where(a=>a.DocumentGroupCode.Equals(Constants.DocumentGroup_Code_Module_DocumentHDSD)).ToList());
            listCatolog.AddRange(documentObjectFile.Where(a=>a.DocumentGroupCode.Equals(Constants.DocumentGroup_Code_Product_Catelog)).ToList());

            var documentTemplates = db.DocumentTemplates.AsNoTracking().Where(r => r.TableName.Equals("ProductStandardTPA") && r.Type.Equals(Constants.ProductStandardTPAFile_Type_COCQ)).ToList();

            listFileCOCQ = (from f in documentTemplates
                            join a in listFileCOCQ on f.Id equals a.DocumentTemplateId into fd
                            from fdn in fd.DefaultIfEmpty()
                            select new ProductStandardTPAFileModel
                            {
                                Id = fdn != null ? fdn.Id : string.Empty,
                                FileName = fdn != null ? fdn.FileName : string.Empty,
                                FilePath = fdn != null ? fdn.FilePath : string.Empty,
                                FileSize = fdn != null ? fdn.FileSize : null,
                                Note = fdn != null ? fdn.Note : string.Empty,
                                Type = Constants.ProductStandardTPAFile_Type_COCQ,
                                CreateBy = fdn != null ? fdn.CreateBy : string.Empty,
                                CreateDate = fdn != null ? fdn.CreateDate : null,
                                DocumentTemplateId = f.Id,
                                DocumentName = f.Name
                            }).ToList();

            return new
            {
                listCatolog,
                listFile,
                listFileCOCQ
            };
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcel(ProductStandardTPASearchModel model)
        {
            model.IsExport = true;
            var data = SearchProductStandardTPA(model);
            var list = data.ListResult.ToList();

            var listData = db.ProductStandardTPAs.AsNoTracking().ToList();
            ProductStandardTPA productStandardTPA;
            foreach (var item in data.ListResult)
            {
                productStandardTPA = new ProductStandardTPA();
                productStandardTPA = listData.FirstOrDefault(i => i.Id.Equals(item.Id));
                if (productStandardTPA != null)
                {
                    productStandardTPA.Index = productStandardTPA.Index + 1;
                }
            }

            ListModel listModel = new ListModel();

            if (list.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;
                application.EnableIncrementalFormula = true;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Template_ThietBiTieuChuanExport.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = list.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

                var b = total + 5;

                var listExport = list.Select((a, i) => new
                {
                    Index = i + 1,
                    a.EnglishName,
                    a.VietNamName,
                    a.Model,
                    a.TheFirm,
                    view1 = a.ProductStandardTPATypeName,
                    a.Manufacture_NCC_SX,
                    a.Supplier_NCC_SX,
                    a.Name_NCC_SX,
                    a.Address_NCC_SX,
                    a.PIC_NCC_SX,
                    a.PhoneNumber_NCC_SX,
                    a.Email_NCC_SX,
                    a.Title_NCC_SX,
                    a.BankPayment_NCC_SX,
                    view2 = listModel.ListTypePayment_NCC_SX.FirstOrDefault(e => e.Id.Equals(a.TypePayment_NCC_SX)) != null ? listModel.ListTypePayment_NCC_SX.FirstOrDefault(e => e.Id.Equals(a.TypePayment_NCC_SX)).Name : "",
                    a.RulesPayment_NCC_SX,
                    viewRulesDelivery = listModel.ListRulesDelivery_NCC_SX.FirstOrDefault(e => e.Id.Equals(a.RulesDelivery)) != null ? listModel.ListRulesDelivery_NCC_SX.FirstOrDefault(e => e.Id.Equals(a.RulesDelivery)).Name : "",
                    a.DeliveryTime,
                    a.Country_NCC_SX,
                    a.Website_NCC_SX,
                    a.ListedPrice,
                    a.PriceConformQuantity,
                    a.PricePolicy,
                    a.MinimumQuantity,
                    view3 = a.MethodVC == 1 ? "Biển" : a.MethodVC == 2 ? "Bộ" : a.MethodVC == 3 ? "Hàng không" : "",
                    a.LoadingPort,
                    a.PriceInSP_Price,
                    a.PriceInPO_Price,
                    view4 = a.Currency_PO == 1 ? "USD" : a.Currency_PO == 2 ? "EUR" : a.Currency_PO == 3 ? "CNY" : a.Currency_PO == 4 ? "VND" : "",
                    a.Ratio_PO,
                    viewPO = "",//a.Amout_PO,
                    a.Weight_PriceWord,
                    viewPriceVCInWeight_PriceWord = "",// a.PriceVCInWeight_PriceWord,
                    a.VolumePricing_PriceWord,
                    a.CPVCInWeight_Price_PriceWord,
                    view5 = a.CPVCInWeight_Currency_PriceWord == 1 ? "USD" : a.CPVCInWeight_Currency_PriceWord == 2 ? "EUR" : a.CPVCInWeight_Currency_PriceWord == 3 ? "CNY" : a.CPVCInWeight_Currency_PriceWord == 4 ? "VND" : "",
                    a.CPVCInWeight_Ratio_PriceWord,
                    a.CPVCInWeight_AmoutVND_PriceWord,
                    a.Weight_Air_PriceWord,
                    a.MassConvertedBySize_Air_PriceWord,
                    a.VolumePricing_Air_PriceWord,
                    a.Price_AirTransport_Air_PriceWord,
                    viewCurency_AirTransport_Air_PriceWord = a.Curency_AirTransport_Air_PriceWord == 1 ? "USD" : a.Curency_AirTransport_Air_PriceWord == 2 ? "EUR" : a.Curency_AirTransport_Air_PriceWord == 3 ? "CNY" : a.Curency_AirTransport_Air_PriceWord == 4 ? "VND" : "",
                    a.Ratio_AirTransport_Air_PriceWord,
                    a.Amount_AirTransport_Air_PriceWord,
                    a.PackingSize_D_PriceWord,
                    a.PackingSize_R_PriceWord,
                    a.PackingSize_C_PriceWord,
                    a.PackingSize_Tolerance_PriceWord,
                    a.PackingSize_RealVolume_PriceWord,
                    a.PackingSize_PiceVolume_PriceWord,
                    a.PriceVCInCBM_PriceWord,
                    a.Price_PriceVC_LCLInBCM_PriceWord,
                    view6 = a.Currency_PriceVC_LCLInBCM_PriceWord == 1 ? "USD" : a.Currency_PriceVC_LCLInBCM_PriceWord == 2 ? "EUR" : a.Currency_PriceVC_LCLInBCM_PriceWord == 3 ? "CNY" : a.Currency_PriceVC_LCLInBCM_PriceWord == 4 ? "VND" : "",
                    a.Ratio_PriceVC_LCLInBCM_PriceWord,
                    a.Amout_PriceVC_LCLInBCM_PriceWord,
                    a.Price_PriceVC_Cont20_PriceWord,
                    view7 = a.Currency_PriceVC_Cont20_PriceWord == 1 ? "USD" : a.Currency_PriceVC_Cont20_PriceWord == 2 ? "EUR" : a.Currency_PriceVC_Cont20_PriceWord == 3 ? "CNY" : a.Currency_PriceVC_Cont20_PriceWord == 4 ? "VND" : "",
                    a.Ratio_PriceVC_Cont20_PriceWord,
                    a.Amout_PriceVC_Cont20_PriceWord,
                    a.Price_PriceVC_Cont20OT_PiceWord,
                    view8 = a.Currency_PriceVC_Cont20OT_PiceWord == 1 ? "USD" : a.Currency_PriceVC_Cont20OT_PiceWord == 2 ? "EUR" : a.Currency_PriceVC_Cont20OT_PiceWord == 3 ? "CNY" : a.Currency_PriceVC_Cont20OT_PiceWord == 4 ? "VND" : "",
                    a.Ratio_PriceVC_Cont20OT_PiceWord,
                    a.Amount_PriceVC_Cont20OT_PiceWord,
                    a.Price_PriceVC_Cont40_PriceWord,
                    view9 = a.Currency_PriceVC_Cont40_PriceWord == 1 ? "USD" : a.Currency_PriceVC_Cont40_PriceWord == 2 ? "EUR" : a.Currency_PriceVC_Cont40_PriceWord == 3 ? "CNY" : a.Currency_PriceVC_Cont40_PriceWord == 4 ? "VND" : "",
                    a.Ratio_PriceVC_Cont40_PriceWord,
                    a.Amount_PriceVC_Cont40_PriceWord,
                    a.Price_PriceVC_Cont40OT_PriceWord,
                    view10 = a.Currency_PriceVC_Cont40OT_PriceWord == 1 ? "USD" : a.Currency_PriceVC_Cont40OT_PriceWord == 2 ? "EUR" : a.Currency_PriceVC_Cont40OT_PriceWord == 3 ? "CNY" : a.Currency_PriceVC_Cont40OT_PriceWord == 4 ? "VND" : "",
                    a.Ratio_PriceVC_Cont40OT_PriceWord,
                    a.Amount_PriceVC_Cont40OT_PriceWord,
                    a.UpdateDatePice_PriceWord,
                    a.Price_PriceLCExport,
                    view11 = a.Currency_PriceLCExport == 1 ? "USD" : a.Currency_PriceLCExport == 2 ? "EUR" : a.Currency_PriceLCExport == 3 ? "CNY" : a.Currency_PriceLCExport == 4 ? "VND" : "",
                    a.Ratio_PriceLCExport,
                    a.Amount_PriceLCExport,
                    a.InsurranceType,
                    a.Price_InsurrancePrice,
                    a.Price_PriceLCImport_LSS,
                    view13 = a.Currency_PriceLCImport_LSS == 1 ? "USD" : a.Currency_PriceLCImport_LSS == 2 ? "EUR" : a.Currency_PriceLCImport_LSS == 3 ? "CNY" : a.Currency_PriceLCImport_LSS == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_LSS,
                    a.Amount_PriceLCImport_LSS,
                    a.Price_PriceLCImport_THC,
                    view14 = a.Currency_PriceLCImport_THC == 1 ? "USD" : a.Currency_PriceLCImport_THC == 2 ? "EUR" : a.Currency_PriceLCImport_THC == 3 ? "CNY" : a.Currency_PriceLCImport_THC == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_THC,
                    a.Amount_PriceLCImport_THC,
                    a.Price_PriceLCImport_DO,
                    view15 = a.Currency_PriceLCImport_DO == 1 ? "USD" : a.Currency_PriceLCImport_DO == 2 ? "EUR" : a.Currency_PriceLCImport_DO == 3 ? "CNY" : a.Currency_PriceLCImport_DO == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_DO,
                    a.Amount_PriceLCImport_DO,
                    a.Price_PriceLCImport_CIC,
                    view16 = a.Currency_PriceLCImport_CIC == 1 ? "USD" : a.Currency_PriceLCImport_CIC == 2 ? "EUR" : a.Currency_PriceLCImport_CIC == 3 ? "CNY" : a.Currency_PriceLCImport_CIC == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_CIC,
                    a.Amount_PriceLCImport_CIC,
                    a.Price_PriceLCImport_HL,
                    view17 = a.Currency_PriceLCImport_HL == 1 ? "USD" : a.Currency_PriceLCImport_HL == 2 ? "EUR" : a.Currency_PriceLCImport_HL == 3 ? "CNY" : a.Currency_PriceLCImport_HL == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_HL,
                    a.Amount_PriceLCImport_HL,
                    a.Price_PriceLCImport_CLF,
                    view18 = a.Currency_PriceLCImport_CLF == 1 ? "USD" : a.Currency_PriceLCImport_CLF == 2 ? "EUR" : a.Currency_PriceLCImport_CLF == 3 ? "CNY" : a.Currency_PriceLCImport_CLF == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_CLF,
                    a.Amount_PriceLCImport_CLF,
                    a.Price_PriceLCImport_CFS,
                    view19 = a.Currency_PriceLCImport_CFS == 1 ? "USD" : a.Currency_PriceLCImport_CFS == 2 ? "EUR" : a.Currency_PriceLCImport_CFS == 3 ? "CNY" : a.Currency_PriceLCImport_CFS == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_CFS,
                    a.Amount_PriceLCImport_CFS,
                    a.Price_PriceLCImport_Lift,
                    view20 = a.Currency_PriceLCImport_Lift == 1 ? "USD" : a.Currency_PriceLCImport_Lift == 2 ? "EUR" : a.Currency_PriceLCImport_Lift == 3 ? "CNY" : a.Currency_PriceLCImport_Lift == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_Lift,
                    a.Amount_PriceLCImport_Lift,
                    a.Price_PriceLCImport_IF,
                    view21 = a.Currency_PriceLCImport_IF == 1 ? "USD" : a.Currency_PriceLCImport_IF == 2 ? "EUR" : a.Currency_PriceLCImport_IF == 3 ? "CNY" : a.Currency_PriceLCImport_IF == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_IF,
                    a.Amount_PriceLCImport_IF,
                    a.Price_PriceLCImport_Other,
                    view22 = a.Currency_PriceLCImport_Other == 1 ? "USD" : a.Currency_PriceLCImport_Other == 2 ? "EUR" : a.Currency_PriceLCImport_Other == 3 ? "CNY" : a.Currency_PriceLCImport_Other == 4 ? "VND" : "",
                    a.Ratio_PriceLCImport_Other,
                    a.Amount_PriceLCImport_Other,
                    a.HSCode,
                    a.ImportTax,
                    a.ImportTaxPrice,
                    a.UpdateDateHSCode,
                    a.NameTaxOther,
                    a.TaxOther,
                    a.ImportTaxPriceOther,
                    a.VAT,
                    a.PriceOther,
                    a.PriceFW,
                    a.Surcharge,
                    a.InventoryTime,
                    a.ShortInterest,
                    a.MidtermInterest,
                    a.PriceProduct_TPA,
                    a.PriceVC_TPA,
                    a.ImportTaxPrice_TPA,
                    a.ImportTax_TPA,
                    a.VAT_TPA,
                    a.Interest_TPA,
                    a.TotalPrice,
                    a.Profit_TPA,
                    a.PriceEXW_TPA,
                    a.UpdateDatePrice_TPA,
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 142].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 142].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 142].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 142].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 142].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 142].CellStyle.WrapText = true;

                sheet["AD5:AD1000"].Formula = "=AA5*AC5";
                sheet["AF5:AF1000"].Formula = "=AW5/$AF$4";
                sheet["AG5:AG1000"].Formula = "=IF(AF5>AE5,AF5,AE5)";
                sheet["AK5:AK1000"].Formula = "=AH5*AJ5*AG5";
                sheet["AM5:AM1000"].Formula = "=AW5/$AM$4";
                sheet["AN5:AN1000"].Formula = "=IF(AM5>AL5,AM5,AL5)";
                sheet["AR5:AR1000"].Formula = "=AO5*AQ5*AN5";
                sheet["AW5:AW1000"].Formula = "=((AS5+AV5)*(AT5+AV5)*(AU5+AV5)/$AY$4)";
                sheet["AX5:AX1000"].Formula = "=+IF(((AS5+AV5)*(AT5+AV5)*(AU5+AV5)/$AY$4)<1, 1, (AS5+AV5)*(AT5+AV5)*(AU5+AV5)/$AY$4)";
                sheet["AY5:AY1000"].Formula = "=IF(AX5<1, 1*AZ5, AX5*AZ5)";
                sheet["BC5:BC1000"].Formula = "=AY5*BB5";
                sheet["BG5:BG1000"].Formula = "=BD5*BF5";
                sheet["BK5:BK1000"].Formula = "=BH5*BJ5";
                sheet["BO5:BO1000"].Formula = "=BL5*BN5";
                sheet["BS5:BS1000"].Formula = "=BP5*BR5";
                //sheet["BU5:BU1000"].Formula = "=IF(AX5<1,15*1+85+110+150,15*AX5+85+110+150)";
                sheet["BX5:BX1000"].Formula = "=BU5*BW5";
                sheet["BZ5:BZ1000"].Formula = "=BY5*(AD5+AK5+AR5+BC5+BG5+BK5+BO5+BS5+BX5)";
                sheet["CD5:CD1000"].Formula = "=CA5*CC5";
                sheet["CH5:CH1000"].Formula = "=CE5*CG5";
                sheet["CL5:CL1000"].Formula = "=CI5*CK5";
                sheet["CP5:CP1000"].Formula = "=CM5*CO5";
                sheet["CT5:CT1000"].Formula = "=CQ5*CS5";
                sheet["CX5:CX1000"].Formula = "=CU5*CW5";
                sheet["DB5:DB1000"].Formula = "=CY5*DA5";
                sheet["DF5:DF1000"].Formula = "=DC5*DE5";
                sheet["DJ5:DJ1000"].Formula = "=DG5*DI5";
                sheet["DN5:DN1000"].Formula = "=DK5*DM5";
                sheet["DQ5:DQ1000"].Formula = "=DP5*(AD5+(AK5+AR5+BC5+BG5+BK5+BO5+BS5+BX5)+BX5+BZ5+CD5)";
                sheet["DU5:DU1000"].Formula = "=DT5*(AD5+(AK5+AR5+BC5+BG5+BK5+BO5+BS5+BX5)+BX5+BZ5+CD5)";
                //sheet["DY5:DY1000"].Formula = "=IF(AX5<1,(140000+140000+14000),AX5*(140000+140000+14000))";
                sheet["EC5:EC1000"].Formula = "=AD5";
                sheet["ED5:ED1000"].Formula = "=(AK5+AR5+BC5+BG5+BK5+BO5+BS5+BX5)";
                sheet["EE5:EE1000"].Formula = "=BZ5+CD5+CH5+CL5+CP5+CT5+CX5+DB5+DF5+DJ5+DN5+DW5+DX5+DY5";
                sheet["EF5:EF1000"].Formula = "=DQ5+DU5";
                sheet["EI5:EI1000"].Formula = "=SUM(EC5:EH5)";

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách thiết bị tiêu chuẩn" + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách thiết bị tiêu chuẩn" + ".xlsx";
                db.SaveChanges();
                return resultPathClient;

            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcelBusiness(ProductStandardTPASearchModel model)
        {
            model.IsExport = true;
            var data = SearchProductStandardTPA(model);
            var list = data.ListResult.ToList();

            var listData = db.ProductStandardTPAs.ToList();
            ProductStandardTPA productStandardTPA;
            foreach (var item in data.ListResult)
            {
                productStandardTPA = new ProductStandardTPA();
                productStandardTPA = listData.FirstOrDefault(i => i.Id.Equals(item.Id));
                if (productStandardTPA != null)
                {
                    productStandardTPA.Index = productStandardTPA.Index + 1;
                }
            }

            ListModel listModel = new ListModel();

            if (list.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;
                application.EnableIncrementalFormula = true;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Template_ThietBiTieuChuanExport_Business.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = list.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

                var b = total + 5;

                var listExport = list.Select((a, i) => new
                {
                    Index = i + 1,
                    a.EnglishName,
                    a.VietNamName,
                    a.Model,
                    a.TheFirm,
                    view1 = a.ProductStandardTPATypeName,
                    a.Manufacture_NCC_SX,
                    a.Supplier_NCC_SX,
                    a.Name_NCC_SX,
                    a.Price_L1,
                    a.Price_L2,
                    a.Price_L3,
                    a.Price_L4,
                    a.Price_L5,
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 14].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách thiết bị tiêu chuẩn - KD.xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách thiết bị tiêu chuẩn - KD.xlsx";
                db.SaveChanges();
                return resultPathClient;

            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy danh sách nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public List<SupplierModel> GetSuppliers()
        {
            var listSupplier = (from a in db.Suppliers.AsNoTracking()
                                join b in db.SupplierContacts.AsNoTracking() on a.Id equals b.SupplierId into ab
                                from ba in ab.DefaultIfEmpty()
                                orderby a.Name
                                select new SupplierModel
                                {
                                    Id = a.Id,
                                    Address_NCC_SX = a.Address,
                                    BankPayment_NCC_SX = a.BankPayment,
                                    DeliveryTime = a.DeliveryTime,
                                    Email_NCC_SX = a.Email,
                                    Name_NCC_SX = a.Name,
                                    PhoneNumber_NCC_SX = a.PhoneNumber,
                                    PIC_NCC_SX = ba.Name,
                                    TypePayment_NCC_SX = a.TypePayment,
                                    Supplier_NCC_SX = a.Code,
                                    RulesDelivery_NCC_SX = a.RulesDelivery,
                                    RulesPayment_NCC_SX = a.RulesPayment,
                                    Code = a.Code,
                                    Title_NCC_SX = ba.Location,
                                    Country_NCC_SX = a.Country,
                                    Website_NCC_SX = a.Website,
                                }).ToList();
            return listSupplier;
        }

        public string ImportFile(HttpPostedFile file, bool isConfirm, string userId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            string model;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<int> rowCode = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<string> listId = new List<string>();
            string confirm = string.Empty;

            try
            {
                var list = db.ProductStandardTPAs.AsNoTracking().ToList();
                for (int i = 3; i <= rowCount; i++)
                {
                    model = sheet[i, 2].Value;

                    if (!string.IsNullOrEmpty(model))
                    {
                        var data = list.FirstOrDefault(a => a.Model.Equals(model));
                        if (data != null)
                        {
                            listId.Add(data.Id);
                        }
                        else
                        {
                            rowCheckCode.Add(i);
                        }
                    }
                    else
                    {
                        rowCode.Add(i);
                    }
                }

                #endregion

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Model dòng <" + string.Join(", ", rowCode) + "> không được để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Model dòng <" + string.Join(", ", rowCheckCode) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                confirm = SyncSaleProductStandardTPA(false, isConfirm, listId, userId);
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();

            return confirm;
        }

        public string SyncSaleProductStandardTPA(bool isAll, bool isConfirm, List<string> listProductStandardTPAId, string userId)
        {
            SaleProduct saleProduct;
            SaleProduct check;
            SaleProductMedia saleProductMedia;
            List<SaleProductMedia> saleProductMedias;
            SaleProductDocument saleProductDocument;
            List<SaleProductDocument> saleProductDocuments;
            List<SaleProduct> listSaleProduct = new List<SaleProduct>();
            List<SaleProductMedia> listSaleProductMedia = new List<SaleProductMedia>();
            List<SaleProductDocument> listSaleProductDocument = new List<SaleProductDocument>();
            SyncSaleProduct syncSaleProduct;
            List<SyncSaleProduct> syncSaleProducts = new List<SyncSaleProduct>();
            ProductStandardTPA productStandardTPA;
            List<ProductStandardTPAImage> listImage;
            List<ProductStandardTPAFile> listFile;
            string groupCode = string.Empty;
            string manufacureId = string.Empty;
            string conten = $"Đồng bộ sản phẩm kinh doanh";
            var listProductStandardTPAType = db.ProductStandardTPATypes.AsNoTracking().ToList();
            List<string> listCodeError = new List<string>();

            if (isAll)
            {
                listProductStandardTPAId = new List<string>();
                listProductStandardTPAId = db.ProductStandardTPAs.AsNoTracking().Select(i => i.Id).ToList();
            }

            foreach (var id in listProductStandardTPAId)
            {
                productStandardTPA = new ProductStandardTPA();
                productStandardTPA = db.ProductStandardTPAs.FirstOrDefault(i => i.Id.Equals(id));
                if (productStandardTPA == null)
                {
                    continue;
                }
                productStandardTPA.SyncDate = DateTime.Now;
                productStandardTPA.IsSendSale = true;
                listImage = new List<ProductStandardTPAImage>();
                listImage = db.ProductStandardTPAImages.AsNoTracking().Where(i => i.ProductStandardTPAId.Equals(productStandardTPA.Id)).ToList();

                listFile = new List<ProductStandardTPAFile>();
                listFile = db.ProductStandardTPAFiles.AsNoTracking().Where(i => i.ProductStandardTPAId.Equals(productStandardTPA.Id)).ToList();

                groupCode = listProductStandardTPAType.FirstOrDefault(i => i.Id.Equals(productStandardTPA.ProductStandardTPATypeId))?.Name;
                manufacureId = db.Manufactures.FirstOrDefault(i => i.Name.ToUpper().Equals(productStandardTPA.Manufacture_NCC_SX.ToUpper()))?.Id;

                // Kiểm tra thông tin thiết bị nhập khẩu, nếu thiếu thì cảnh báo
                if (!isConfirm && (string.IsNullOrEmpty(groupCode) ||
                    string.IsNullOrEmpty(manufacureId) ||
                    string.IsNullOrEmpty(productStandardTPA.Country_NCC_SX) ||
                    string.IsNullOrEmpty(productStandardTPA.Specifications) ||
                    !productStandardTPA.VAT.HasValue || !productStandardTPA.PriceEXW_TPA.HasValue || string.IsNullOrEmpty(productStandardTPA.DeliveryTime)))
                {
                    listCodeError.Add(productStandardTPA.Model);
                    continue;
                }

                check = new SaleProduct();
                check = db.SaleProducts.FirstOrDefault(i => i.Type == Constants.SaleProductStandTPA && i.SourceId.Equals(productStandardTPA.Id));
                if (check != null)
                {
                    check.Model = productStandardTPA.Model;
                    check.EName = productStandardTPA.EnglishName;
                    check.VName = productStandardTPA.VietNamName;
                    check.GroupCode = groupCode;
                    check.ManufactureId = manufacureId;
                    check.CountryName = productStandardTPA.Country_NCC_SX;
                    check.Specifications = productStandardTPA.Specifications;
                    check.CustomerSpecifications = productStandardTPA.Specifications;
                    if (productStandardTPA.VAT.HasValue)
                    {
                        check.VAT = productStandardTPA.VAT.Value;
                    }
                    if (productStandardTPA.PriceEXW_TPA.HasValue)
                    {
                        check.EXWTPAPrice = productStandardTPA.PriceEXW_TPA.Value;
                    }
                    //check.MaterialPrice = productStandardTPA.PriceProduct_TPA;
                    check.EXWTPADate = productStandardTPA.UpdateDatePrice_TPA;
                    //check.PublicPrice;
                    check.DeliveryTime = productStandardTPA.DeliveryTime;
                    check.SourceId = productStandardTPA.Id;
                    check.Type = Constants.SaleProductStandTPA;
                    check.IsSync = true;
                    check.Status = true;
                    check.UpdateBy = userId;
                    check.UpdateDate = DateTime.Now;

                    if (listImage.Count > 0)
                    {
                        check.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProductStandardTPA,
                        Date = DateTime.Now
                    };
                    syncSaleProducts.Add(syncSaleProduct);

                    saleProductMedias = new List<SaleProductMedia>();
                    saleProductMedias = db.SaleProductMedias.Where(i => i.SaleProductId.Equals(check.Id) && i.Type == Constants.SaleProductMedia_Type_LibraryImage).ToList();

                    saleProductDocuments = new List<SaleProductDocument>();
                    saleProductDocuments = db.SaleProductDocuments.Where(i => i.SaleProductId.Equals(check.Id) && (i.Type == Constants.SaleProductDocument_Type_Catalog || i.Type == Constants.SaleProductDocument_Type_UserManual)).ToList();

                    db.SaleProductMedias.RemoveRange(saleProductMedias);
                    db.SaleProductDocuments.RemoveRange(saleProductDocuments);

                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };
                        listSaleProductMedia.Add(saleProductMedia);
                    }

                    // Catalog
                    foreach (var item in listFile.Where(i => i.Type == Constants.ProductStandardTPAFile_Type_Catolog))
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = item.FileSize.Value,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductDocument_Type_Catalog
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Hướng dẫn thực hành
                    foreach (var item in listFile.Where(i => i.Type == Constants.ProductStandardTPAFile_Type_File))
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = item.FileSize.Value,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductDocument_Type_UserManual
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncProductTPA, check.Id, check.Model, conten);
                }
                else
                {
                    saleProduct = new SaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EName = productStandardTPA.EnglishName,
                        VName = productStandardTPA.VietNamName,
                        Model = productStandardTPA.Model,
                        GroupCode = groupCode,
                        ManufactureId = manufacureId,
                        CountryName = productStandardTPA.Country_NCC_SX,
                        Specifications = productStandardTPA.Specifications,
                        CustomerSpecifications = productStandardTPA.Specifications,
                        VAT = productStandardTPA.VAT.HasValue ? productStandardTPA.VAT.Value : 0,
                        //check.MaterialPrice = productStandardTPA.PriceProduct_TPA,
                        EXWTPAPrice = productStandardTPA.PriceEXW_TPA.HasValue ? productStandardTPA.PriceEXW_TPA.Value : 0,
                        EXWTPADate = productStandardTPA.UpdateDatePrice_TPA,
                        //check.PublicPrice,
                        DeliveryTime = productStandardTPA.DeliveryTime,
                        SourceId = productStandardTPA.Id,
                        Type = Constants.SaleProductStandTPA,
                        IsSync = true,
                        Status = true,
                        CreateDate = DateTime.Now,
                        CreateBy = userId,
                        UpdateDate = DateTime.Now,
                        UpdateBy = userId,
                    };

                    if (listImage.Count > 0)
                    {
                        saleProduct.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    listSaleProduct.Add(saleProduct);

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProductStandardTPA,
                        Date = DateTime.Now
                    };

                    syncSaleProducts.Add(syncSaleProduct);

                    // Image
                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };
                        listSaleProductMedia.Add(saleProductMedia);
                    }

                    // Catalog
                    foreach (var item in listFile.Where(i => i.Type == Constants.ProductStandardTPAFile_Type_Catolog))
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = item.FileSize.Value,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductDocument_Type_Catalog
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    // Hướng dẫn thực hành
                    foreach (var item in listFile.Where(i => i.Type == Constants.ProductStandardTPAFile_Type_File))
                    {
                        saleProductDocument = new SaleProductDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = item.FileSize.Value,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductDocument_Type_UserManual
                        };
                        listSaleProductDocument.Add(saleProductDocument);
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncProductTPA, saleProduct.Id, saleProduct.Model, conten);
                }
            }

            if (listCodeError.Count > 0)
            {
                return "Model thiết bị nhập khẩu <" + string.Join(", ", listCodeError) + "> đang thiếu thông tin đồng bộ. Bạn có muốn tiếp tục đồng bộ!";
            }

            using (var trans = db.Database.BeginTransaction())
            {
                db.SaleProducts.AddRange(listSaleProduct);
                db.SyncSaleProducts.AddRange(syncSaleProducts);
                db.SaleProductMedias.AddRange(listSaleProductMedia);
                db.SaleProductDocuments.AddRange(listSaleProductDocument);
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }

            return null;
        }
    }
}
