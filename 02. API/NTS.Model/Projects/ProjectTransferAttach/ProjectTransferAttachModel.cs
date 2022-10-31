using NTS.Model.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectTransferAttach
{
    public class ProjectTransferAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public decimal? FileSize { get; set; }
        public string FilePath { get; set; }
        public string Note { get; set; }
        public string CreateByName { get; set; }
        public bool IsDelete { get; set; }
        public string NumberOfReport { get; set; }
        public DateTime? SignDate { get; set; }
    }

    public class ProjectTransferAttachAddModel : BaseModel
    {
        public string ProjectId { get; set; }
        public List<ProjectTransferAttachModel> ListFile { get; set; }
        public ProjectTransferAttachAddModel()
        {
            ListFile = new List<ProjectTransferAttachModel>();
        }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public List<ProjectProductToTranferModel> ListProductTranfer { get; set; }
    }

    public class PlanTransferByProjectModel
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string ProjectId { get; set; }
        public decimal ExecutionTime { get; set; }
        public string ProjectName { get; set; }
        public string projectCode { get; set; }
        public string ProjectProductId { get; set; }
        public int Type { get; set; }
        public DateTime? RealStartDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ResponsiblePersion { get; set; }
        public decimal EsimateTime { get; set; }
        public int Status { get; set; }
        public DateTime? ExpectedDesignFinishDate { get; set; }
        public DateTime? ExpectedMakeFinishDate { get; set; }
        public DateTime? ExpectedTransferDate { get; set; }
        public int DataType { get; set; }
        public string ProductId { get; set; }
        public string ModuleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }

    }
}
