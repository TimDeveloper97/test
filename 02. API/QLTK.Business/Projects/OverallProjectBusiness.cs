using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Plans;
using NTS.Model.ProjectProducts;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using NTS.Model.TaskTimeStandardModel;
using QLTK.Business.Plans;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;

namespace QLTK.Business.OverallProject
{
    public class OverallProjectBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Lọc danh sách tổng thể dự án
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>

        //private IQueryable<OverallProjectModel> SearchWorkingReports(string id)
        //{

        //    SearchResultModel<OverallProjectModel> searchResult = new SearchResultModel<OverallProjectModel>();
        //    //lấy danh sách sản phẩm
        //    var dataQuery = (from a in db.ProjectProducts.AsNoTracking()
        //                     where a.ProjectId.Equals(id)
        //                     select new OverallProjectModel()
        //                     {
        //                         Id = a.Id,
        //                         ContractName = a.ContractName,
        //                         DataType = a.DataType,
        //                         ProductId = a.ProductId,
        //                         ModuleId = a.ModuleId,
        //                         ProjectId = a.ProjectId,
        //                     }).AsQueryable();
        //    Product product;
        //    Module module;
        //    var queryStagePlan = (from a in db.Plans.AsNoTracking()
        //                          join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
        //                          where a.ProjectId.Equals(id) && a.IsPlan && a.StageId != null
        //                          select new OverallProjectModel()
        //                          {
        //                              Id = a.Id,
        //                              Name = a.Name,
        //                          }).ToList();


        //    searchResult.ListResult = dataQuery.ToList();
        //    searchResult.ListDayChange = queryStagePlan;
        //    List<string> listIssue;
        //    foreach (var item in searchResult.ListResult)
        //    {
        //        //lấy tên, mã theo thiết kế
        //        if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
        //        {
        //            product = db.Products.AsNoTracking().FirstOrDefault(i => item.ProductId.Equals(i.Id));
        //            if (product != null)
        //            {
        //                item.Code = product.Code;
        //                item.Name = product.Name;
        //            }
        //        }
        //        else if (item.DataType == Constants.ProjectProduct_DataType_Module || item.DataType == Constants.ProjectProduct_DataType_Paradigm)
        //        {
        //            module = db.Modules.AsNoTracking().FirstOrDefault(i => item.ModuleId.Equals(i.Id));
        //            if (module != null)
        //            {
        //                item.Code = module.Code;
        //                item.Name = module.Name;
        //            }
        //        }

        //        foreach (var ite in queryStagePlan)
        //        {
        //            var productStage = (from a in db.Plans.AsNoTracking()
        //                                join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
        //                                where a.ProjectProductId.Equals(item.Id) && a.IsPlan && a.StageId != null && a.Id.Equals(ite.Id)
        //                                select new OverallProjectModel()
        //                                {
        //                                    Id = a.Id,
        //                                    Name = a.Name,
        //                                    Status = a.Status
        //                                }).FirstOrDefault();
        //            if (productStage != null)
        //            {

        //                switch (productStage.Status)
        //                {
        //                    case 4:
        //                        item.listStage.Add("Hold");
        //                        break;
        //                    case 3:
        //                        item.listStage.Add("completed");
        //                        break;
        //                    case 2:
        //                        item.listStage.Add("on going");
        //                        break;
        //                    default:
        //                        item.listStage.Add("pending");
        //                        break;
        //                }

        //            }
        //            else
        //            {
        //                item.listStage.Add("N/A");
        //            }
        //        }

        //        listIssue = (from s in db.Errors.AsNoTracking()
        //                     where s.ProjectId.Equals(item.ProjectId)
        //                     join r in db.ErrorFixs.AsNoTracking() on s.Id equals r.ErrorId
        //                     select r.Solution).ToList();

        //        item.Issue = string.Join(", ", listIssue);

        //        var dateTo = (from s in db.Errors.AsNoTracking()
        //                      where s.ProjectId.Equals(item.ProjectId)
        //                      select s.PlanFinishDate).FirstOrDefault();
        //        item.DateTo = dateTo;

        //    }
        //    return searchResult;
        //}


        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Excel(string id)
        {


            SearchResultModel<OverallProjectModel> searchResult = new SearchResultModel<OverallProjectModel>();
            //lấy danh sách sản phẩm
            var dataQuery = (from a in db.ProjectProducts.AsNoTracking()
                             where a.ProjectId.Equals(id)
                             select new OverallProjectModel()
                             {
                                 Id = a.Id,
                                 ContractName = a.ContractName,
                                 DataType = a.DataType,
                                 ProductId = a.ProductId,
                                 ModuleId = a.ModuleId,
                                 ProjectId = a.ProjectId,
                             }).AsQueryable();
            Product product;
            Module module;
            var queryStagePlan = (from a in db.Plans.AsNoTracking()
                                  join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                  where a.ProjectId.Equals(id) && a.IsPlan == false && a.StageId != null
                                  group a by new { a.StageId, b.index } into g
                                 
                                  select new OverallProjectModel()
                                  {
                                      Id = g.FirstOrDefault().Id,
                                      Name = g.FirstOrDefault().Name,
                                      Index = g.Key.index ,
                                  }).ToList();
            var StagePlan = queryStagePlan.OrderBy(x => x.Index).ToList();


            searchResult.ListResult = dataQuery.ToList();
            searchResult.ListDayChange = StagePlan;
            List<string> listIssue;
            foreach (var item in searchResult.ListResult)
            {
                //lấy tên, mã theo thiết kế
                if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    product = db.Products.AsNoTracking().FirstOrDefault(i => item.ProductId.Equals(i.Id));
                    if (product != null)
                    {
                        item.Code = product.Code;
                        item.Name = product.Name;
                    }
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module || item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    module = db.Modules.AsNoTracking().FirstOrDefault(i => item.ModuleId.Equals(i.Id));
                    if (module != null)
                    {
                        item.Code = module.Code;
                        item.Name = module.Name;
                    }
                }

                foreach (var ite in StagePlan)
                {
                    var productStage = (from a in db.Plans.AsNoTracking()
                                        where a.ProjectProductId.Equals(item.Id) && a.StageId != null && a.Id.Equals(ite.Id)
                                        select new OverallProjectModel()
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Status = a.Status
                                        }).FirstOrDefault();
                    if (productStage != null)
                    {

                        switch (productStage.Status)
                        {
                            case 5:
                                item.listStage.Add("Stop");
                                break;
                            case 4:
                                item.listStage.Add("Stop");
                                break;
                            case 3:
                                item.listStage.Add("Close");
                                break;
                            case 2:
                                item.listStage.Add("On-Going");
                                break;
                            default:
                                item.listStage.Add("Open");
                                break;
                        }

                    }
                    else
                    {
                        item.listStage.Add("N/A");
                    }
                }

                listIssue = (from s in db.Errors.AsNoTracking()
                             where s.ProjectId.Equals(item.ProjectId)
                             join r in db.ErrorFixs.AsNoTracking() on s.Id equals r.ErrorId
                             select r.Solution).ToList();

                item.Issue = string.Join(", ", listIssue);

                var dateTo = (from s in db.Errors.AsNoTracking()
                              where s.ProjectId.Equals(item.ProjectId)
                              select s.PlanFinishDate).FirstOrDefault();
                item.DateTo = dateTo;

            }






            var listModel = searchResult.ListResult;
            var listDayChange = searchResult.ListDayChange;

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/TienDoTongTheDuAn.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;
                var totall = listDayChange.Count;

                IRange iRangeData1 = sheet.FindFirst("<daychange>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData1.Text = iRangeData1.Text.Replace("<daychange>", string.Empty);
                //var listDayChanges = listDayChange.Select((a, i) => new
                //{
                //    a.Name,
                //});
                //var list = listDayChanges.ToList();
                List<string> listDayChanges = new List<string>();
                foreach (var item in listDayChange)
                {
                    listDayChanges.Add(item.Name);
                }
                if (listDayChange.Count() > 1)
                {
                    sheet.InsertColumn(iRangeData1.Column + 1, listDayChanges.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportArray(listDayChanges.ToArray(), iRangeData1.Row, iRangeData1.Column, false);

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                IRange iRangeData2 = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData2.Text = iRangeData2.Text.Replace("<data>", string.Empty);
                //int i = 0;
                //ArrayList listExport = new ArrayList();
                //foreach (var item in listModel)
                //{
                //    List<string> lists = new List<string>();
                //    lists.Add(item.ContractName);
                //    lists.Add(item.Name);
                //    lists.Add(item.Code);
                //    foreach (var line in item.listStage)
                //    {
                //        lists.Add(line);
                //    }
                //    listExport.Add(lists);
                //}

                var listExport = listModel.Select((a, i) => new
                {
                    a.ContractName,
                    a.Name,
                    a.Code,

                });

                

                //sheet.ImportArray(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);

                int index = 0;
                foreach (var item in listModel)
                {
                    sheet.ImportArray(item.listStage.ToArray(), iRangeData2.Row+index , iRangeData2.Column, false);
                    index++;
                }


                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 14].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Tiến độ tổng thể dự án" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Tiến độ tổng thể dự án" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(id, ex);
            }
        }



    }





}
