using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.GeneralTemplateElectronic
{
    public class GeneralProfileElectronicDesign
    {
        private QLTKEntities db = new QLTKEntities();
        public string GeneralTemplateProfileElectronicDesign(ProfileElectronicDesignModel model)
        {
            checkExistProduct(model);
            //model.IsExport = true;
            ProfileElectronicDesignModel newModel = new ProfileElectronicDesignModel();

            //newModel.DateNow = DateTime.Now.ToString();
            //DateTime datevalue = (Convert.ToDateTime(newModel.DateNow.ToString()));

            //newModel.Day = datevalue.Day.ToString();
            //newModel.Month = datevalue.Month.ToString();
            //newModel.Year = datevalue.Year.ToString();
            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new GeneralMechanicalModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();

            string templatePath = HttpContext.Current.Server.MapPath("~/Template/HoSoThietKeMachDienTu.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {
                var now = DateTime.Today;

                document.NTSReplaceFirst("2019", now.Year.ToString());
                document.NTSReplaceFirst("ngày 1  tháng 11 năm 2019", " ngày " + now.Day.ToString() + " tháng " + now.Month.ToString() + " năm " + now.Year.ToString());

                document.NTSReplaceAll("PCB.A010201", model.ProductCode);
                document.NTSReplaceFirst("MẠCH THU THẬP DỮ LIỆU 8DI/8DO, 10AI/4AO", model.ProductName);
                document.NTSReplaceFirst("Nguyễn Viết Tập", data.UserName);

                document.NTSReplaceFirst("PATK-C.PCB.A010201.docm", "PATK-C." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("PATK-CN.PCB.A010201.docm", "PATK-CN." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("PATK-LK.PCB.A010201.docm", "PATK-LK." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("KTNL.PCB.A010201.docm", "KTNL." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("TTNL.PCB.A010201.docm", "TTNL." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("VT.PCB.A010201.xlsm", "VT." + model.ProductCode + ".xlsm");
                document.NTSReplaceFirst("KTMI.PCB.A010201.docm", "KTMI." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("KTCL.PCB.A010201.docm", "KTCL." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("LR.PCB.A010201.docm", "LR." + model.ProductCode + ".docm");
                document.NTSReplaceFirst("TPAT.A010201.PcbDoc", "TPAT." + model.ProductCode.Substring(4) + ".PcbDoc");
                document.NTSReplaceAll("PCB.A010201.step", model.ProductCode + ".step");
                document.NTSReplaceAll("ASM.A010201.step", "ASM." + model.ProductCode.Substring(4) + ".step");
                document.NTSReplaceFirst("1-PCB.A010201.hex", "1-" + model.ProductCode + ".hex");
                document.NTSReplaceFirst("2-PCB.A010201.hex", "2-" + model.ProductCode + ".hex");

                document.NTSReplaceFirst("1_M", model.check_1_M ? "Có" : "Không có");
                document.NTSReplaceFirst("2_M", model.check_2_M ? "Có" : "Không có");
                document.NTSReplaceFirst("3_M", model.check_3_M ? "Có" : "Không có");
                document.NTSReplaceFirst("4_M", model.check_4_M ? "Có" : "Không có");
                document.NTSReplaceFirst("5_M", model.check_5_M ? "Có" : "Không có");
                document.NTSReplaceFirst("6_M", model.check_6_M ? "Có" : "Không có");
                document.NTSReplaceFirst("7_M", model.check_7_M ? "Có" : "Không có");
                document.NTSReplaceFirst("8_M", model.check_8_M ? "Có" : "Không có");
                document.NTSReplaceFirst("9_M", model.check_9_M ? "Có" : "Không có");
                document.NTSReplaceFirst("10_M", model.check_10_M ? "Có" : "Không có");
                document.NTSReplaceFirst("11a_M", model.check_11a_M ? "Có" : "Không có");
                document.NTSReplaceFirst("11b_M", model.check_12a_M ? "Có" : "Không có");
                document.NTSReplaceFirst("12a_M", model.check_12a_M ? "Có" : "Không có");
                document.NTSReplaceFirst("12b_M", model.check_12b_M ? "Có" : "Không có");

                document.NTSReplaceFirst("1_C", model.check_1_C ? "Có" : "Không có");
                document.NTSReplaceFirst("2_C", model.check_2_C ? "Có" : "Không có");
                document.NTSReplaceFirst("3_C", model.check_3_C ? "Có" : "Không có");
                document.NTSReplaceFirst("4_C", model.check_4_C ? "Có" : "Không có");
                document.NTSReplaceFirst("5_C", model.check_5_C ? "Có" : "Không có");
                document.NTSReplaceFirst("6_C", model.check_6_C ? "Có" : "Không có");
                document.NTSReplaceFirst("7_C", model.check_7_C ? "Có" : "Không có");
                document.NTSReplaceFirst("8_C", model.check_8_C ? "Có" : "Không có");
                document.NTSReplaceFirst("9_C", model.check_9_C ? "Có" : "Không có");
                document.NTSReplaceFirst("10_C", model.check_10_C ? "Có" : "Không có");
                document.NTSReplaceFirst("11a_C", model.check_11a_C ? "Có" : "Không có");
                document.NTSReplaceFirst("11b_C", model.check_12a_C ? "Có" : "Không có");
                document.NTSReplaceFirst("12a_C", model.check_12a_C ? "Có" : "Không có");
                document.NTSReplaceFirst("12b_C", model.check_12b_C ? "Có" : "Không có");

                var idModule = db.Modules.Where(a => a.Code.Equals(model.ProductCode)).Select(a => a.ModuleGroupId).FirstOrDefault();
                var listProductStand = (from a in db.ModuleGroupProductStandards.AsNoTracking()
                                        join b in db.ProductStandards.AsNoTracking() on a.ProductStandardId equals b.Id
                                        where a.ModuleGroupId.Equals(idModule) && b.DataType == Constants.Employee_WorkType_Dt
                                        orderby b.Name
                                        select new GenealProductStandModel()
                                        {
                                            Name = b.Name,
                                            Code = b.Code,
                                        }).ToList();

                if (listProductStand != null)
                {
                    WTable tableIndex = document.GetTableByFindText("<Data>");

                    if (tableIndex != null)
                    {
                        document.NTSReplaceFirst("<Data>", string.Empty);
                        WTableRow templateRow = tableIndex.Rows[1].Clone();
                        WTableRow row;
                        int index = 1;
                        foreach (var item in listProductStand)
                        {
                            tableIndex.Rows.Insert(index, templateRow.Clone());
                            row = tableIndex.Rows[index];
                            row.Cells[0].Paragraphs[0].Text = index.ToString();
                            row.Cells[1].Paragraphs[0].Text = item.Name;
                            row.Cells[2].Paragraphs[0].Text = item.Code;
                            index++;
                        }
                    }
                }

                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "HoSoThietKeMachDienTu" + ".docm");
                document.Save(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, null, Constants.LOG_GeneralProfileElectronicDesign);

                document.Close();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "HoSoThietKeMachDienTu" + ".docm";

            return pathreturn;
        }
        public void checkExistProduct(ProfileElectronicDesignModel model)
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
