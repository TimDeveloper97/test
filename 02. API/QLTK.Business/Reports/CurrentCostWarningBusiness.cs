using NTS.Common;
using NTS.Model.CurrentCostWarning;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.CurrentCostWarning
{
    public class CurrentCostWarningBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public object getDataCurrentCostWarning(CurrentCostWarningSearchModel model)
        {
            decimal totalModuleGroup = 0;
            decimal totalError = 0;

            //Danh sách dự án đã triển khai
            var listProjects = db.Projects.AsNoTracking().Where(i => !i.Status.Equals(Constants.Prooject_Status_NotStartedYet)).ToList();

            if (!string.IsNullOrEmpty(model.ProjectId))
            {
                listProjects = listProjects.Where(i => i.Id.Equals(model.ProjectId)).ToList();
            }

            //Tổng số dự án đã triển khai
            var totalProject = listProjects.Count;

            //Tổng số tiền giá dự án
            decimal totalPriceProject = 0;

            //Tổng số tiền giá thiết kế
            decimal totalDesignPrice = 0;

            if (listProjects.Count > 0)
            {
                totalPriceProject = listProjects.Sum(i => i.Price);
                totalDesignPrice = listProjects.Sum(i => i.DesignPrice);
            }

            //Tổng số dự án đang triển khai
            decimal totalProjectProcessing = 0;

            //Tổng số tiền dự án đang triển khai
            decimal totalPriceProcessing = 0;

            //Tổng số tiền thiết kế đang triển khai
            decimal totalDesignPriceProcessing = 0;

            var listProjectProcessing = listProjects.Where(i => i.Status.Equals(Constants.Prooject_Status_Processing)).ToList();

            totalProjectProcessing = listProjectProcessing.Count;
            totalPriceProcessing = listProjectProcessing.Sum(i => i.Price);
            totalDesignPriceProcessing = listProjectProcessing.Sum(i => i.DesignPrice);

            //Tổng số tiền theo dự án phân theo dòng sản phẩm và Tổng chi phí lãng phí theo lỗi, theo tồn đọng
            var listProjectProductModule = (from a in db.ProjectProducts.AsNoTracking()
                                            join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                            join c in db.Projects.AsNoTracking() on a.ProjectId equals c.Id
                                            select new { a.ProjectId, a.ModuleId, b.ModuleGroupId, a.Price, c.KickOffDate }).AsQueryable();
            var listErrors = (from a in db.Errors.AsNoTracking()
                              join b in db.Modules.AsNoTracking() on a.ObjectId equals b.Id
                              join c in db.Projects.AsNoTracking() on a.ProjectId equals c.Id
                              where a.Type == Constants.Error_Type_Error
                              select new { a.ProjectId, a.ObjectId, b.ModuleGroupId, a.ErrorCost, c.KickOffDate }).AsQueryable();

            if (!string.IsNullOrEmpty(model.ProjectId))
            {
                listProjectProductModule = listProjectProductModule.Where(i => model.ProjectId.Equals(i.ProjectId));
                listErrors = listErrors.Where(i => model.ProjectId.Equals(i.ProjectId));
            }

            if (model.DateFrom.HasValue)
            {
                listProjectProductModule = listProjectProductModule.Where(i => i.KickOffDate >= model.DateFrom.Value);
                listErrors = listErrors.Where(i => i.KickOffDate >= model.DateFrom.Value);
            }

            if (model.DateTo.HasValue)
            {
                listProjectProductModule = listProjectProductModule.Where(i => i.KickOffDate <= model.DateTo.Value);
                listErrors = listErrors.Where(i => i.KickOffDate <= model.DateTo.Value);
            }

            var listProjectModule = listProjectProductModule.ToList();
            var listErrorModel = listErrors.ToList();
            var listProjectProductId = listProjectModule.GroupBy(i => i.ModuleGroupId).Select(i => i.Key).ToList();
            var listErrorId = listErrorModel.GroupBy(i => i.ModuleGroupId).Select(i => i.Key).ToList();

            List<AddModel> listPriceProjectproduct = new List<AddModel>();
            List<AddModel> listPriceError = new List<AddModel>();

            var listModule = db.Modules.AsNoTracking().ToList();
            var quantity = 0;
            foreach (var item in listProjectProductId)
            {
                var moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(i => i.Id.Equals(item));
                if (moduleGroup != null)
                {
                    var price = listProjectModule.Where(i => i.ModuleGroupId.Equals(item)).Sum(i => i.Price);
                    quantity = listModule.Where(i => i.ModuleGroupId.Equals(item)).Count();
                    listPriceProjectproduct.Add(new AddModel
                    {
                        Id = moduleGroup.Id,
                        ModuleGroupCode = moduleGroup.Code,
                        ModuleGroupName = moduleGroup.Name,
                        Price = price,
                        Quantity = quantity,
                        AveragePrice = Math.Round(price / quantity)
                    });
                }
            }

            foreach (var item in listErrorId)
            {
                var moduleGroup = db.ModuleGroups.AsNoTracking().FirstOrDefault(i => i.Id.Equals(item));
                if (moduleGroup != null)
                {
                    var price = listErrorModel.Where(i => i.ModuleGroupId.Equals(item)).Sum(i => i.ErrorCost);
                    quantity = listModule.Where(i => i.ModuleGroupId.Equals(item)).Count();
                    listPriceError.Add(new AddModel
                    {
                        Id = moduleGroup.Id,
                        ModuleGroupCode = moduleGroup.Code,
                        ModuleGroupName = moduleGroup.Name,
                        Price = price,
                        Quantity = quantity,
                        AveragePrice = Math.Round(price / quantity)
                    });
                }
            }
            totalModuleGroup = listPriceProjectproduct.Sum(i => i.Price);
            foreach (var item in listPriceProjectproduct)
            {
                item.Percent = Math.Round((item.Price / totalModuleGroup) * 100, 2);
            }
            totalError = listPriceError.Sum(i => i.Price);
            foreach (var item in listPriceError)
            {
                if (totalError > 0)
                {
                    item.Percent = Math.Round((item.Price / totalError) * 100, 2);
                }
            }

            var listData = (from a in listPriceProjectproduct.AsEnumerable()
                            join b in listPriceError.AsEnumerable() on a.Id equals b.Id into ab
                            from ba in ab.DefaultIfEmpty()
                            orderby a.ModuleGroupCode
                            select new GroupAddModel
                            {
                                ModuleGroupCode = a.ModuleGroupCode,
                                ModuleGroupName = a.ModuleGroupName,
                                QuantityModule = a.Quantity,
                                PriceProjectProduct = a.Price,
                                PercentProjectProduct = a.Percent,
                                AveragePriceProjectProduct = a.AveragePrice,
                                PriceError = ba != null ? ba.Price : 0,
                                PercentError = ba != null ? ba.Percent : 0,
                                AverageError = ba != null ? ba.AveragePrice : 0
                            }).ToList();

            return new
            {
                TotalProject = totalProject,
                TotalPriceProject = totalPriceProject,
                TotalDesignPrice = totalDesignPrice,
                TotalProjectProcessing = totalProjectProcessing,
                TotalPriceProcessing = totalPriceProcessing,
                TotalDesignPriceProcessing = totalDesignPriceProcessing,
                //ListPriceProjectproduct = listPriceProjectproduct.OrderBy(i => i.ModuleGroupCode),
                //ListPriceError = listPriceError.OrderBy(i => i.ModuleGroupCode),
                ListData = listData,
                TotalModuleGroup = totalModuleGroup,
                TotalError = totalError
            };
        }

        public string ExcelCurrentCostWarning(List<GroupAddModel> listData)
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportCurrentCostWarning.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = listData.Count;

            IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

            var listExport = listData.Select((a, i) => new
            {
                Index = i + 1,
                a.ModuleGroupCode,
                a.ModuleGroupName,
                a.QuantityModule,
                a.PriceProjectProduct,
                a.PercentProjectProduct,
                a.AveragePriceProjectProduct,
                a.PriceError,
                a.PercentError,
                a.AverageError
            });

            if (listExport.Count() > 2)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 2, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders.Color = ExcelKnownColors.Black;
            //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 10].CellStyle.WrapText = true;


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo chi phí hiện tại" + ".xlsx");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo chi phí hiện tại" + ".xlsx";

            return resultPathClient;
        }
    }
}
