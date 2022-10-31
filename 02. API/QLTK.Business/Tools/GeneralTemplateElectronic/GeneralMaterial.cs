using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.ExportTemplate;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.GeneralTemplate
{
    public class GeneralMaterial
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplateMaterial(GeneralElectronicModel model)
        {

            checkExistProduct(model);

            SearchResultModel<GeneralElectronicModel> searchResult = new SearchResultModel<GeneralElectronicModel>();
            //var dataQuery = (from a in db.Modules.AsNoTracking()
            //                 join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id into ab
            //                 from b in ab.DefaultIfEmpty()
            //                 join c in db.Designers.AsNoTracking() on a.Id equals c.ModuleId into ac
            //                 from c in ac.DefaultIfEmpty()
            //                 join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id into cd
            //                 from d in cd.DefaultIfEmpty()
            //                 join e in db.ModuleMaterials.AsNoTracking() on a.Id equals e.ModuleId
            //                 join f in db.Manufactures.AsNoTracking() on e.ManufacturerId equals f.Id into af
            //                 from f in af.DefaultIfEmpty()
            //                 join g in db.Units.AsNoTracking() on e.UnitName equals g.Id
            //                 where a.Code.Equals(model.ProductCode)
            //                 select new GeneralElectronicModel
            //                 {
            //                     Code = e.MaterialCode,
            //                     Name = e.MaterialName,
            //                     ProductName = a.Name,
            //                     ProductCode = a.Code,
            //                     Designer = d.Name,
            //                     ManufactureName = f.Name,
            //                     Quantity = e.Quantity,
            //                     Unit = g.Name,
            //                     Specification = e.Specification,
            //                     RawMaterial = e.RawMaterial, // Vật liệu
            //                     Weight = e.Weight,
            //                     Note = e.Note,
            //                     CreateBy = a.CreateBy,
            //                     CreateDate = a.CreateDate,
            //                     UpdateBy = a.UpdateBy,
            //                     UpdateDate = a.UpdateDate,
            //                 }).AsQueryable();
            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new GeneralElectronicModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();

            //List<GeneralElectronicModel> listRs = new List<GeneralElectronicModel>();
            //var lstRs = dataQuery.GroupBy(t => new { t.Name, t.Specification, t.Code, t.ProductCode, t.Unit, t.Quantity, t.RawMaterial, t.Weight, t.ManufactureName, t.Note, t.ProductName }).ToList();
            //foreach (var item in lstRs)
            //{
            //    GeneralElectronicModel rs = new GeneralElectronicModel();
            //    rs.Name = item.Key.Name;
            //    rs.Code = item.Key.Code;
            //    rs.Specification = item.Key.Specification;
            //    rs.ProductCode = item.Key.ProductCode;
            //    rs.Unit = item.Key.Unit;
            //    rs.Quantity = item.Key.Quantity;
            //    rs.RawMaterial = item.Key.RawMaterial;
            //    rs.Weight = item.Key.Weight;
            //    rs.ManufactureName = item.Key.ManufactureName;
            //    rs.Note = item.Key.Note;
            //    rs.ProductName = item.Key.ProductName;
            //    List<string> lstDesginer = new List<string>();
            //    foreach (var ite in item.ToList())
            //    {
            //        if (lstDesginer.Count > 0)
            //        {
            //            if (!lstDesginer.Contains(ite.Designer))
            //            {
            //                rs.Designer += ", " + ite.Designer;
            //                lstDesginer.Add(ite.Designer);
            //            }

            //        }
            //        else
            //        {
            //            rs.Designer += ite.Designer;
            //            lstDesginer.Add(ite.Designer);
            //        }
            //    }
            //    listRs.Add(rs);
            //}

            //if (listRs.Count == 0)
            //{
            //    throw NTSException.CreateInstance("Module không có vật tư, không thể xuất file");

            //}
            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/BieuMauDanhMucVatTu.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];

                sheet[4, 3].Value = data.UserName;
                var now = DateTime.Today;
                //if(listRs.Count() > 0)
                //{
                //    foreach (var item in listRs)
                //    {
                //        sheet.Replace("<now>", DateTime.Now.ToString(), ExcelFindOptions.MatchCase);
                //        sheet.Replace("<user>", data.UserName, ExcelFindOptions.MatchCase); // truoc la model.createby
                //        sheet.Replace("PCB.CODE", item.ProductCode, ExcelFindOptions.MatchCase);
                //        sheet.Replace("<productname>", item.ProductName, ExcelFindOptions.MatchCase);

                //        //    sheet.Replace("<paint>", model.Paint, ExcelFindOptions.MatchCase);
                //        //    sheet.Replace("<check>", model.Check, ExcelFindOptions.MatchCase);
                //        //    sheet.Replace("<approve>", model.Approve, ExcelFindOptions.MatchCase);
                //    }
                //}

                sheet.Replace("<now>", DateTime.Now.ToString(), ExcelFindOptions.MatchCase);
                sheet.Replace("<user>", data.UserName, ExcelFindOptions.MatchCase); // truoc la model.createby
                sheet.Replace("PCB.CODE", model.ProductCode, ExcelFindOptions.MatchCase);
                sheet.Replace("<productname>", model.ProductName, ExcelFindOptions.MatchCase);


                sheet[12, 1].Value = "Tân Phát,  ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                //var total = listRs.Count;

                //var listExport = listRs.Select((a, i) => new
                //{
                //    Index = i + 1,
                //    a.Name, // tên
                //    a.Specification, // thông số
                //    a.Code, // mã
                //    a.ProductCode,
                //    a.Unit, // đơn vị
                //    a.Quantity,
                //    a.RawMaterial,
                //    a.Weight,
                //    a.ManufactureName,
                //    a.Note,
                //});


                //if (listExport.Count() > 1)
                //{
                //    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                //}
                //sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                //sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 11].CellStyle.WrapText = true;



                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "BieuMauDanhMucVatTu_" + specifyName + ".xlsm");
                workbook.SaveAs(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralMaterial);

                workbook.Close();
                excelEngine.Dispose();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "BieuMauDanhMucVatTu_" + specifyName + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(GeneralElectronicModel model)
        {
            if (model.ProductCode == "")
            {
                //cần sửa lại
                throw NTSException.CreateInstance("Bạn phải nhập mã Mạch");
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
