using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.GeneralEquipmentByFunction
{
    public class GeneralEquipmentByFunction
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTempalteEquipmentByFunction(EquipmentByFunctionModel model)
        {
            checkExistProduct(model);
            SearchResultModel<EquipmentByFunctionModel> searchResult = new SearchResultModel<EquipmentByFunctionModel>();
            var dataQuery = (from a in db.Modules.AsNoTracking()
                             join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.ModuleDesigners.AsNoTracking() on a.Id equals c.ModuleId
                             join e in db.ModuleMaterials.AsNoTracking() on a.Id equals e.ModuleId
                             join f in db.Manufactures.AsNoTracking() on e.ManufacturerId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             join g in db.Materials.AsNoTracking() on e.MaterialId equals g.Id
                             where a.Code.Equals(model.ProductCode) && (g.MaterialGroupTPAId == "2" || g.MaterialGroupTPAId == "4")
                             select new EquipmentByFunctionModel
                             {
                                 Code = e.MaterialCode,
                                 Name = e.MaterialName,
                                 ProductName = a.Name,
                                 ProductCode = a.Code,
                                 Designer = c.Designer,
                                 Specification = e.Specification,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                             }).AsQueryable();
            List<EquipmentByFunctionModel> listRs = new List<EquipmentByFunctionModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Name, t.Code, t.ProductCode, t.ProductName, t.Specification }).ToList();
            foreach (var item in lstRs)
            {
                EquipmentByFunctionModel rs = new EquipmentByFunctionModel();
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.Specification = item.Key.Specification;
                rs.ProductCode = item.Key.ProductCode;
                rs.ProductName = item.Key.ProductName;
                List<string> lstDesginer = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstDesginer.Count > 0)
                    {
                        if (!lstDesginer.Contains(ite.Designer))
                        {
                            rs.Designer += ", " + ite.Designer;
                            lstDesginer.Add(ite.Designer);
                        }

                    }
                    else
                    {
                        rs.Designer += ite.Designer;
                        lstDesginer.Add(ite.Designer);
                    }
                }
                listRs.Add(rs);
            }
            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new EquipmentByFunctionModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();


            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");

            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                Syncfusion.XlsIO.IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/BangCacChucNangVeDien.xlsm"));

                Syncfusion.XlsIO.IWorksheet sheet = workbook.Worksheets[0];
                sheet[9, 3].Value = model.ProductName;
                sheet[9, 11].Value = " Mã: " + model.ProductCode;
                sheet[10, 3].Value = data.UserName;
                if (listRs.Count > 0)
                {
                    var now = DateTime.Today;
                    //foreach (var item in listRs)
                    //{

                    //}

                    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                    var total = listRs.Count;

                    var listExport = listRs.Select((a, i) => new
                    {
                        Index = i + 1,
                        a.Name, // tên
                        a.Code, // mã
                        a.Specification,
                    });


                    if (listExport.Count() > 1)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                    }
                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders.Color = ExcelKnownColors.Black;
                    //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 11].CellStyle.WrapText = true;
                }
                else
                {
                    //sheet.Replace("PCB.CODE", model.ProductCode, ExcelFindOptions.MatchCase);
                    //sheet[3, 10].Value = "Mã: " + model.ProductCode;
                    //sheet[9, 3].Value = model.ProductName;
                    sheet[15, 1].Value = "";
                }

                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "BangCacChucNangVeDien_" + specifyName + ".xlsm");
                workbook.SaveAs(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, null, Constants.LOG_GeneralEquipmentByFunction);

                workbook.Close();
                excelEngine.Dispose();
            }

            string pathreturn = "Template/" + Constants.FolderExport + "BangCacChucNangVeDien_" + specifyName + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(EquipmentByFunctionModel model)
        {
            if (model.ProductCode == "")
            {
                //cần sửa lại
                throw NTSException.CreateInstance("Bạn phải nhập mã Module");
            }

            var module = db.Modules.AsNoTracking().FirstOrDefault(u => u.Code.Equals(model.ProductCode));
            if (module != null)
            {
                model.ProductName = module.Name;
            }
            else
            {
                throw NTSException.CreateInstance("Mã này không tồn tại!");
            }

        }
    }
}
