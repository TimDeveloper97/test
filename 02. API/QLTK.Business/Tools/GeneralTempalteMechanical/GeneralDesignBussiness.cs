using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.ProductAccessories;
using NTS.Model.ProjectProducts;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.GeneralTempalteMechanical
{
    public class GeneralDesignBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public object GeneralDesign(ProjectProductsResuldModel model)
        {
            var data = (from a in db.ProjectProducts.AsNoTracking()
                        join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                        where a.Id.Equals(model.Id)
                        select new ProjectProductsResuldModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            ProductId = a.ProductId,
                            ContractName = a.ContractName,
                            ContractCode = a.ContractCode,
                            ModuleName = b.Name,
                            ModuleCode = b.Code,
                            ModuleStatus = a.ModuleStatus,
                            Manufacture = "TPA",
                            Quantity = a.Quantity
                        }).ToList();
            foreach (var item in data)
            {
                var total = db.Errors.AsNoTracking().Where(i => item.ModuleId.Equals(i.ModuleErrorVisualId)).Count();
                item.TotalError = total;
            }
            model.ProductId = data.Select(i => i.ProductId).FirstOrDefault();
            var dataQuery = (from a in db.ProductAccessories.AsNoTracking()
                             join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.Manufactures.AsNoTracking() on c.ManufactureId equals d.Id
                             where a.ProductId.Equals(model.ProductId)
                             orderby c.Code
                             select new ProductAccessoriesModel
                             {
                                 Id = a.Id,
                                 Code = c.Code,
                                 Name = c.Name,
                                 ProductId = a.ProductId,
                                 MaterialId = a.MaterialId,
                                 Manafacture = d.Name,
                                 Quantity = a.Quantity,
                                 Price = a.Price,
                                 Note = a.Note,
                                 TotalError = 0
                             }).ToList();
            //foreach (var item in dataQuery)
            //{
            //    var total= db.Errors.Where(i=>item.ProductId.Equals(i.pro))
            //}
            return new
            {
                ListModule = data,
                ListMaterial = dataQuery
            };
        }

        public string GetCustomerByProjectId(string projectId)
        {
            var customerName = string.Empty;
            var customer = (from a in db.Projects.AsNoTracking()
                                join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                                where a.Id.Equals(projectId)
                                select new { b.Name }).FirstOrDefault();
            if(customer != null)
            {
                customerName = customer.Name;
            }
            return customerName;
        }

        public object GetModuleByProjectproductId(string projectProductId)
        {
            var data = (from a in db.ProjectProducts.AsNoTracking()
                        join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                        where a.Id.Equals(projectProductId)
                        select new { b.Name, b.Code }).FirstOrDefault();
            return data;
        }

        public string ExpoetGeneralDesign(ExportGeneralDesignModel model)
        {
            var customer = (from a in db.Projects.AsNoTracking()
                            join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                            where a.Id.Equals(model.ProjectId)
                            select new { b.Name, b.Code, b.Contact }).FirstOrDefault();

            var deparmentRequestId = "";
            var deparmentRequest = db.Departments.AsNoTracking().FirstOrDefault(i => model.DepartmentRequestId.Equals(i.Id));
            if (deparmentRequest == null)
            {
                deparmentRequestId = "";
            }
            else
            {
                deparmentRequestId = deparmentRequest.Name;
            }

            var deparmentPerformId = "";
            var deparmentPerform = db.Departments.AsNoTracking().FirstOrDefault(i => model.DepartmentPerformId.Equals(i.Id));
            if (deparmentPerform == null)
            {
                deparmentPerformId = "";
            }
            else
            {
                deparmentPerformId = deparmentPerform.Name;
            }

            var employee = "";
            var employees = db.Employees.AsNoTracking().FirstOrDefault(i => model.EmployeeId.Equals(i.Id));
            if (employees == null)
            {
                employee = "";
            }
            else
            {
                employee = employees.Name;
            }

            DateTime? finishdate;
            var finishdates = db.ProjectProducts.AsNoTracking().FirstOrDefault(i => model.Id.Equals(i.Id));
            if (finishdates == null)
            {
                finishdate = null;
            }
            else
            {
                finishdate = finishdates.DesignFinishDate;
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/TongHopThietKe_Template.xlsm"));

            IWorksheet sheet = workbook.Worksheets[0];

            if (customer != null)
            {
                sheet[4, 1].Value = "Tên KH cuối: " + customer.Name;
                sheet[4, 8].Value = customer.Code;
                sheet[5, 11].Value = "Phụ trách HĐ: " + customer.Contact;
            }

            sheet[5, 1].Value = "Bộ Phận YC: " + deparmentRequestId;
            if (model.RequestDate != null)
            {
                sheet[5, 8].Value = "Ngày YC: " + DateTimeHelper.ToStringDDMMYY(model.RequestDate.Value);
            }
            sheet[6, 1].Value = "Bộ Phận TH: " + deparmentPerformId;
            sheet[6, 8].Value = "Ngày HT: " + finishdate;
            sheet[6, 11].Value = "Phụ trách TK: " + employee;
            if (model.ListModule.Count > 0)
            {
                sheet[7, 1].Value = "Tên sản phẩm theo hợp đồng: " + model.ListModule.FirstOrDefault().ContractName;
                sheet[7, 8].Value = "Mã SP theo hợp đồng: " + model.ListModule.FirstOrDefault().ContractCode;
                sheet[8, 1].Value = "Tên sản phẩm theo thiết kế: " + model.ListModule.FirstOrDefault().ModuleName;
                sheet[8, 8].Value = "Mã SP theo thiết kế: " + model.ListModule.FirstOrDefault().ModuleName;
            }
            sheet[7, 12].Value = "Hạng mục: " + model.Categories;        


            var Time = DateTime.Now;
            IRange iRangeData = sheet.FindFirst("<Data1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data1>", string.Empty);
            IRange iRangeData1 = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData1.Text = iRangeData1.Text.Replace("<data>", string.Empty);
            IRange iRangeData2 = sheet.FindFirst("<day>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData2.Text = iRangeData2.Text.Replace("<day>", Time.Day.ToString());
            IRange iRangeData3 = sheet.FindFirst("<month>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData3.Text = iRangeData3.Text.Replace("<month>", Time.Month.ToString());
            IRange iRangeData4 = sheet.FindFirst("<year>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData4.Text = iRangeData4.Text.Replace("<year>", Time.Year.ToString());
            var total = model.ListModule.Count();
            var total1 = model.ListMaterial.Count();
            if (model.ListModule.Count > 0)
            {
                var listExport = model.ListModule.Select((a, i) => new
                {
                    index = i + 1,
                    a.ContractName,
                    a.ModuleName,
                    a.ContractCode,
                    a.ModuleCode,
                    a.Manufacture,
                    a.Quantity,
                    a.TotalError,
                    view9 = "",
                    view10 = a.ModuleStatus == 1 ? "Dự án" : "Bổ sung"
                });

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 14].CellStyle.WrapText = true;
            }

            if (model.ListMaterial.Count > 0)
            {
                var listMaterial = model.ListMaterial.Select((a, i) => new
                {
                    index = i + 1,
                    view2 = "",
                    a.Name,
                    view3 = "",
                    a.Code,
                    a.Manafacture,
                    a.Quantity,
                    a.TotalError,
                    view9 = "",
                    view10 = "",
                    a.Price,
                    view12 = "",
                    view13 = "",
                    view14 = "00000",
                    view15 = a.Quantity * a.Price
                });

                sheet.InsertRow(17, total1 - 2, ExcelInsertOptions.FormatAsBefore);
                sheet.ImportData(listMaterial, iRangeData1.Row, iRangeData1.Column, false);
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData1.Row, 1, iRangeData1.Row + total1 - 1, 14].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData1.Row - 1, 1, iRangeData1.Row + total1 - 1, 14].CellStyle.WrapText = true;
            }
                     
            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");

            string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "BieuMauTongHopThietKe_" + specifyName + ".xlsm");
            workbook.SaveAs(pathFileSave);
            workbook.Close();
            string pathreturn = "Template/" + Constants.FolderExport + "BieuMauTongHopThietKe_" + specifyName + ".xlsm";
            return pathreturn;
        }
    }
}
