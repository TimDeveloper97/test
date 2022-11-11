using NTS.Model.Combobox;
using NTS.Model.Plans;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Reports.ReportProgressProjetct
{
    public class ReportProgressProjectModel
    {
        public string ProjectId { get; set; }
        public string TenVaMaDuAn { get; set; }
        public string MaDuAn { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string MucdoUuTien { get; set; }
        public string NguoiPhuTrach { get; set; }
        public string TinhTrangDuAn { get; set; }
        public string TinhTrangCongNo { get; set; }
        public decimal? TongTienHD { get; set; }
        public decimal SoTienDaThu { get; set; }
        public string VDTD { get; set; }
        public List<StageReportModel> Stages { get; set; }
        public DateTime? KeHoachHoanThanh { get; set; }
        public DateTime? KeHoachKickoff { get; set; }
        public DateTime? StartDate { get; set; }
        public string ChenhLech { get; set; }
        public string ThongTinDuAn { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public string ManageId { get; set; }
        public string Status { get; set; }
        public int? PaymentStatus { get; set; }
        public int StatusChenhLech { get; set; }
        public string DepartmentId { get; set; }
        public int CountNoImplementError { get; set; }
        public int CountImplementError { get; set; }
        public int TotalError { get; set; }


        public ReportProgressProjectModel()
        {
            Stages = new List<StageReportModel>();
        }
    }
    public class StageReportModel
    {
        public string ProjectProductId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public decimal DoneRatio { get; set; }
        public DateTime? MaxContractDate { get; set; }
        public int Weight { get; set; }
        public int Index { get; set; }
        public string InfoStage { get; set; }
    }

    public class ReportProject
    {
        public List<StageReportModel> Stages { get; set; }
        public int CountCKO { get; set; }
        public int CountSx { get; set; }
        public int CountDDA { get; set; }
        public int CountTD { get; set; }
        public int CountLD { get; set; }
        public int CountHC { get; set; }
        public int CountDVSD { get; set; }
        public int CountTK { get; set; }
        public int CountNT { get; set; }
        public int CountVT { get; set; }
        public int CountDTL { get; set; }
        public decimal TongTienHopDong { get; set; }
        public decimal TongSoTienDaThu { get; set; }


        public SearchResultModel<ReportProgressProjectModel> ReportProgressProjectModels { get; set; }
        public ReportProject()
        {
            Stages = new List<StageReportModel>();
            ReportProgressProjectModels = new SearchResultModel<ReportProgressProjectModel>();
        }
    }

    public class ReportProgressProjectSearch : SearchCommonModel
    {
        public string Name { get; set; }
        public DateTime? DateStartTo { get; set; }
        public DateTime? DateStartFrom { get; set; }
        public DateTime? PlanDueDateTo { get; set; }
        public DateTime? PlanDueDateFrom { get; set; }
        public DateTime? PlanKICKOFFTo { get; set; }
        public DateTime? PlanKICKOFFFrom { get; set; }
        public string SBUId { get; set; }
        public int Type { get; set; }
        public string ManageId { get; set; }
        public List<int> Status { get; set; }
        public int? PaymentStatus { get; set; }
        public int VDTD { get; set; }
        public string Stage { get; set; }
        public int StageStatus { get; set; }
        public int Delay { get; set; }
        public int DelayType { get; set; }
        public int Priority { get; set; }
        public string DepartmentId { get; set; }
        public bool IsSynchronized { get; set; }
        public ReportProgressProjectSearch()
        {
            Status = new List<int>();
        }
    }

    public class ProductReport
    {
        public List<StageReportModel> stageReportModels { get; set; }

        public ProductReport()
        {
            stageReportModels = new List<StageReportModel> ();
        }
    }

}
