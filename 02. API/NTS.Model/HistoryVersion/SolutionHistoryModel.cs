using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class SolutionHistoryModel
    {
        public string Id { get; set; }
        public string SolutionGroupId { get; set; }
        public string SBUBusinessId { get; set; }
        public string DepartmentBusinessId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string CustomerId { get; set; }
        public string EndCustomerId { get; set; }
        public string BusinessUserId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public bool Has3DSolution { get; set; }
        public bool Has2D { get; set; }
        public bool HasExplan { get; set; }
        public bool HasDMVT { get; set; }
        public bool HasFCM { get; set; }
        public bool HasTSKT { get; set; }
        public string SolutionMaker { get; set; }
        public string SBUSolutionMakerId { get; set; }
        public string DepartmentSolutionMakerId { get; set; }
        public int Status { get; set; }
        public decimal SaleNoVAT { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string DepartmentId { get; set; }
        public int Index { get; set; }
        public int CurentVersion { get; set; }
        public string MechanicalMaker { get; set; }
        public string ElectricMaker { get; set; }
        public string ElectronicMaker { get; set; }
    }
}
