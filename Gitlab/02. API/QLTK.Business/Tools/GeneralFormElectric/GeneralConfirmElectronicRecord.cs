using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using Spire.Xls.Core;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.GeneralConfirmElectronicRecord
{
    public class GeneralConfirmElectronicRecord
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplateConfirmElectronicRecord(ConfirmElectronicRecordModel model)
        {

            checkExistProduct(model);

            SearchResultModel<ConfirmElectronicRecordModel> searchResult = new SearchResultModel<ConfirmElectronicRecordModel>();
            var dataQuery = (from a in db.Modules.AsNoTracking()
                             join b in db.ModuleMaterials.AsNoTracking() on a.Id equals b.ModuleId
                             join c in db.Materials.AsNoTracking() on b.MaterialId equals c.Id
                             join d in db.ModuleDesigners.AsNoTracking() on a.Id equals d.ModuleId into ad
                             from d in ad.DefaultIfEmpty()
                             join f in db.Units.AsNoTracking() on c.UnitId equals f.Id
                             join g in db.Manufactures.AsNoTracking() on c.ManufactureId equals g.Id
                             where a.Code.Equals(model.ProductCode) && (c.MaterialGroupTPAId.Equals("2") || c.MaterialGroupTPAId.Equals("4"))
                             select new ConfirmElectronicRecordModel
                             {
                                 Code = b.MaterialCode,
                                 Name = b.MaterialName,
                                 ProductName = a.Name,
                                 ProductCode = a.Code,
                                 Designer = d.Designer,
                                 ManufactureName = g.Name,
                                 Quantity = b.Quantity,
                                 Unit = g.Name,
                                 Specification = b.Specification,
                                 RawMaterial = b.RawMaterial, // Vật liệu
                                 Weight = b.Weight,
                                 Note = b.Note,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                             }).AsQueryable();

            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new ConfirmElectronicRecordModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();
            List<ConfirmElectronicRecordModel> listRs = new List<ConfirmElectronicRecordModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Name, t.Specification, t.Code, t.ProductCode, t.Unit, t.Quantity, t.RawMaterial, t.Weight, t.ManufactureName, t.Note, t.ProductName }).ToList();
            foreach (var item in lstRs)
            {
                ConfirmElectronicRecordModel rs = new ConfirmElectronicRecordModel();
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.Specification = item.Key.Specification;
                rs.ProductCode = item.Key.ProductCode;
                rs.Unit = item.Key.Unit;
                rs.Quantity = item.Key.Quantity;
                rs.RawMaterial = item.Key.RawMaterial;
                rs.Weight = item.Key.Weight;
                rs.ManufactureName = item.Key.ManufactureName;
                rs.Note = item.Key.Note;
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

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            Syncfusion.XlsIO.IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/XacNhanVatTuDien.xlsm"));

            Syncfusion.XlsIO.IWorksheet sheet = workbook.Worksheets[0];
            sheet[4, 3].Value = data.UserName;
            sheet[17, 1].Value = "Tân Phát,  ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            if (listRs.Count > 0)
            {
                var now = DateTime.Today;
                foreach (var item in listRs)
                {
                    sheet[3, 3].Value = item.ProductName;
                    sheet[3, 10].Value = "Mã: " + item.ProductCode;

                }
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var total = listRs.Count;

                var listExport = listRs.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name, // tên
                    a.Specification, // thông số
                    a.Code, // mã
                    a.ProductCode,
                    a.Unit, // đơn vị
                    a.Quantity,
                    a.RawMaterial,
                    a.Weight,
                    a.ManufactureName,
                    a.Note,
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
                sheet.Replace("PCB.CODE", model.ProductCode, ExcelFindOptions.MatchCase);
                sheet[3, 10].Value = "Mã: " + model.ProductCode;
                sheet[3, 3].Value = model.ProductName;
                sheet[7, 1].Value = "";
            }


            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");

            string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "XacNhanVatTuDien_" + specifyName + ".xlsm");
            workbook.SaveAs(pathFileSave);
            workbook.Close();
            string pathreturn = "Template/" + Constants.FolderExport + "XacNhanVatTuDien_" + specifyName + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(ConfirmElectronicRecordModel model)
        {
            if (model.ProductCode == "")
            {
                //cần sửa lại
                throw NTSException.CreateInstance("Bạn phải nhập mã Module");
            }

            if (db.Modules.Where(u => u.Code.Equals(model.ProductCode)).Count() > 0)
            {
                model.ProductName = db.Modules.FirstOrDefault(u => u.Code.Equals(model.ProductCode)).Name;
            }
            else
            {
                throw NTSException.CreateInstance("Mã này không tồn tại!");
            }

        }
    }
}