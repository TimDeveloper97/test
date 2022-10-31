using NTS.Common;
using NTS.Model;
using NTS.Model.ReportStatusProduct;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ReportStatusProduct
{
    public class ReportStatusProductBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public ReportStatusProductModel searchProduct(ReportStatusProductSearchModel searchModel)
        {
            ReportStatusProductModel model = new ReportStatusProductModel();

            var data = (from a in db.Products.AsNoTracking()
                        join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ProductId
                        join c in db.Projects.AsNoTracking() on b.ProjectId equals c.Id
                        orderby a.Code
                        select new
                        {
                            Id = a.Id,
                            ProductCode = a.Code,
                            ProductName = a.Name,
                            ProductId = a.Id,
                            ProjectId = c.Id,
                            ProjectCode = c.Code,
                            ProjectName = c.Name,
                            Quatity = b.RealQuantity,
                            CreateDate = a.CreateDate
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ProductCode))
            {
                data = data.Where(a => a.ProductCode.ToLower().Contains(searchModel.ProductCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.ProductName))
            {
                data = data.Where(a => a.ProductName.ToLower().Contains(searchModel.ProductName.ToLower()));
            }

            if (searchModel.DateFrom.HasValue)
            {
                data = data.Where(a => a.CreateDate >= searchModel.DateFrom);
            }

            if (searchModel.DateTo.HasValue)
            {
                data = data.Where(a => a.CreateDate <= searchModel.DateTo);
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectId))
            {
                data = data.Where(a => a.ProjectId.Equals(searchModel.ProjectId));
            }


            var listRs = (from t in data
                          group t by new { t.ProductCode, t.ProductName, t.ProjectCode, t.ProjectName } into x
                          select new ReportStatusProductModel
                          {
                              ProductCode = x.Key.ProductCode,
                              ProductName = x.Key.ProductName,
                              ProjectCode = x.Key.ProjectCode,
                              ProjectName = x.Key.ProjectName,
                              TotalProductInProject = x.Sum(y => y.Quatity),
                              TotalProduct = x.Count(),
                          }).AsQueryable();

            var listExcel = (from t in data
                             group t by new { t.ProductCode, t.ProductName } into x
                             select new ReportStatusProductModel
                             {
                                 ProductCode = x.Key.ProductCode,
                                 ProductName = x.Key.ProductName,
                                 TotalProductInProject = x.Sum(y => y.Quatity),
                                 TotalProduct = x.Count(),
                             }).AsQueryable();


            model.TotalProduct = listRs.Count();

            model.ListProduct = listExcel.OrderBy(i => i.ProductCode).ToList();

            if (searchModel.IsExccel)
            {
                model.ListProductUse = listRs.OrderBy(i => i.ProductCode).ToList();
            }
            else
            {
                model.ListProductUse = SQLHelpper.OrderBy(listRs, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            }
            return model;
        }


        public string ExportExcel(ReportStatusProductSearchModel searchModel)
        {
            var data = searchProduct(searchModel);
            List<ReportStatusProductModel> listProduct = data.ListProduct;

            if (listProduct.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportStatusProduct.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listProduct.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listProduct.Select((a, i) => new
                {
                    Index = i + 1,
                    a.ProductName,
                    a.ProductCode,
                    a.TotalProduct,
                    a.TotalProductInProject,
                });


                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 5].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng thiết bị" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng thiết bị" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public string ExportExcelProduct(ReportStatusProductSearchModel searchModel)
        {
            searchModel.IsExccel = true;
            var data = searchProduct(searchModel);
            List<ReportStatusProductModel> listProduct = data.ListProductUse;

            if (listProduct.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportStatusProductSearch.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listProduct.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listProduct.Select((a, i) => new
                {
                    Index = i + 1,
                    a.ProductCode,
                    a.ProductName,
                    a.ProjectCode,
                    a.ProjectName,
                    a.TotalProductInProject,
                    a.TotalProduct,
                });


                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng thiết bị theo tìm kiếm" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng thiết bị theo tìm kiếm" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }
    }
}
