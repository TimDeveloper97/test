using NTS.Model.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashBroadProject
{
    public class DashBroadProjectModel
    {
        public string ProjectCode { get; set; }       
       
        /// <summary>
        /// Dự án về mặt thiết kế (bị chậm)
        /// </summary>
        public int TotalDelayProjectDesign { get; set; }

        /// <summary>
        /// Dự án về mặt thiết kế nhưng chưa lập kế hoạch
        /// </summary>
        public int TotalProjectDesignNotPlan { get; set; }
        /// <summary>
        /// Số lượng thiết kế đang đúng tiền độ	
        /// </summary>
        public int TotalProjectDesignDoneBeforPlan { get; set; }
        /// <summary>
        /// Dự án về mặt tài liệu (bị chậm)
        /// </summary>
        public int TotalDelayProjectDoc { get; set; }

        /// <summary>
        /// Dự án về mặt tài liệu nhưng chưa lập kế hoạch
        /// </summary>
        public int TotalProjectDocNotPlan { get; set; }
        /// <summary>
        /// Số lượng tài liệu đang đúng tiền độ	
        /// </summary>
        public int TotalProjectDocDoneBeforPlan { get; set; }
        /// <summary>
        /// Dự án về mặt chuyển giao (bị chậm)
        /// </summary>
        public int TotalDelayProjectTransfer { get; set; }
        /// <summary>
        /// Dự án về mặt chuyển giao nhưng chưa lập kế hoạch
        /// </summary>
        public int TotalProjectTransferNotPlan { get; set; }
        /// <summary>
        /// Số lượng chuyển giao đang đúng tiền độ	
        /// </summary>
        public int TotalProjectTransferDoneBeforPlan { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        /// <summary>
        /// % hoàn thành công việc
        /// </summary>
        public decimal PercentDesign { get; set; }
        /// <summary>
        /// Tổng số module thiết kế
        /// </summary>
        public int TotalModuleDesign { get; set; }
        /// <summary>
        /// Tổng số module sửa thiết kế cũ
        /// </summary>
        public int TotalModuleUpdateDesign { get; set; }
        /// <summary>
        /// Tổng số module tận dụng (sử dụng)
        /// </summary>
        public int TotalModuleUse { get; set; }
        /// <summary>
        /// Tổng số mô hình thiết kế
        /// </summary>
        public int TotalParadigmDesign { get; set; }
        /// <summary>
        /// Tổng số mô hình sửa thiết kế cũ
        /// </summary>
        public int TotalParadigmUpdateDesign { get; set; }
        /// <summary>
        /// tổng số mô hình tận dụng (sử dụng)
        /// </summary>
        public int TotalParadigmUse { get; set; }
        /// <summary>
        /// Tổng số công việc chưa có kế hoạch cho module
        /// </summary>
        public int TotalModuleTaskNotPlan { get; set; }
        /// <summary>
        /// Tổng số công việc chưa có kế hoạch theo mô hình
        /// </summary>
        public int TotalParadigmTaskNotPlan { get; set; }
        /// <summary>
        /// Số lượng công việc hoàn thành theo module
        /// </summary>
        public int TotalModuleFinish { get; set; }
        /// <summary>
        /// Số lượng công việc chưa(đang) hoàn thành theo module
        /// </summary>
        public int TotalModuleMakeDesign { get; set; }
        /// <summary>
        /// Số lượng công việc hoàn thành theo mô hình
        /// </summary>
        public int TotalParadigmFinish { get; set; }
        /// <summary>
        /// Số lượng công việc chưa(đang) hoàn thành theo mô hình
        /// </summary>
        public int TotalParadigmMakeDesign { get; set; }
        /// <summary>
        ///  Số lượng công việc chậm theo module 
        /// </summary>
        public int TotalModuleDelay { get; set; }
        /// <summary>
        /// Số lượng công việc chậm theo mô hình
        /// </summary>
        public int TotalParadigmDelay { get; set; }
        /// <summary>
        /// Công việc đến deadline nhưng chưa hoàn thành của (Thiết kế)
        /// </summary>
        public int Total_Task_Design_Delay { get; set; }
        /// <summary>
        /// Số ngày chậm lớn nhất Thiết Kế
        /// </summary>
        public int TotalDelayDay_Design_Max { get; set; }

        /// <summary>
        /// % hoàn thành tài liệu
        /// </summary>
        public decimal PercentDoc { get; set; }
        /// <summary>
        /// Tổng số tài liệu mới
        /// </summary>
        public int TotalDocDesign { get; set; }
        /// <summary>
        /// Tổng số tài liệu sửa
        /// </summary>
        public int TotalDocUpdateDesign { get; set; }
        /// <summary>
        /// Tổng số tài liệu tận dụng
        /// </summary>
        public int TotalDocUse { get; set; }
        /// <summary>
        /// Tổng số công việc chưa có kế hoạch theo module (Tài liệu)
        /// </summary>
        public int TotalModuleTaskNotPlan_Doc { get; set; }
        /// <summary>
        /// Tổng số công việc chưa có kế hoạch theo mô hình (Tài liệu)
        /// </summary>
        public int TotalParadigmTaskNotPlan_Doc { get; set; }
        /// <summary>
        /// Tổng số công việc hoàn thành theo module (Tài liệu)
        /// </summary>
        public int TotalModuleFinish_Doc { get; set; }
        /// <summary>
        /// Tổng số công việc chưa(đang) hoàn thành theo module (Tài liệu)
        /// </summary>
        public int TotalModuleMakeDesign_Doc { get; set; }
        /// <summary>
        /// Tổng số công việc hoàn thành theo mô hình (Tài liệu)
        /// </summary>
        public int TotalParadigmFinish_Doc { get; set; }
        /// <summary>
        /// Tổng số công việc chưa(đang) hoàn thành theo mô hình (Tài liệu)
        /// </summary>
        public int TotalParadigmMakeDesign_Doc { get; set; }
        /// <summary>
        /// Số lượng công việc bị chậm theo module (Tài liệu)
        /// </summary>
        public int TotalModuleDelay_Doc { get; set; }
        /// <summary>
        /// Số lượng công việc bị chậm theo mô hình (Tài liệu)
        /// </summary>
        public int TotalParadigmDelay_Doc { get; set; }
        /// <summary>
        /// Công việc đến deadline nhưng chưa hoàn thành của (Tài liệu)
        /// </summary>
        public int Total_Task_Doc_Delay { get; set; }

        /// <summary>
        /// Số ngày chậm lớn nhất của Tài liệu
        /// </summary>
        public int TotalDelayDay_Doc_Max { get; set; }
        /// <summary>
        /// % tiến độ hoàn thành dự án theo công việc (Chuyển giao)
        /// </summary>
        public decimal PercentTransfer { get; set; }
        /// <summary>
        /// Sô lượng dự án chưa có kế hoạch (Chuyển giao)
        /// </summary>
        public int TotalProjectIsNotPlan_Transfer { get; set; }
        /// <summary>
        /// Công việc đến deadline nhưng chưa hoàn thành của (Chuyển giao)
        /// </summary>
        public int Total_Task_Transfer_Delay { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch cho module
        /// </summary>
        public int TotalModuleTaskHavePlan { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch theo mô hình
        /// </summary>
        public int TotalParadigmTaskHavePlan { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch đáp ứng deadline trong ké hoạch
        /// </summary>
        public int TotalTaskInPlanSatisfy { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch đáp ứng deadline trễ ké hoạch
        /// </summary>
        public int TotalTaskOutPlanSatisfy { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch không đáp ứng deadline trong ké hoạch
        /// </summary>
        public int TotalTaskInPlanNotSatisfy { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch không đáp ứng deadline trễ ké hoạch
        /// </summary>
        public int TotalTaskOutPlanNotSatisfy { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch không đáp ứng deadline trễ ké hoạch <=3day
        /// </summary>
        public int TotalTaskOutPlanNotSatisfyLessThanThreeDay { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch không đáp ứng deadline trễ ké hoạch 3 day -7 day
        /// </summary>
        public int TotalTaskOutPlanNotSatisfyThreeToSevenDay { get; set; }
        /// <summary>
        /// Tổng số công việc có kế hoạch không đáp ứng deadline trễ ké hoạch > 7day
        /// </summary>
        public int TotalTaskOutPlanNotSatisfyGreaterThanSevenDay { get; set; }
        public int Priority { get; set; }
//update new dashboard dự án
        /// <summary>
        /// hoàn thành công việc/tổng tg cv
        /// </summary>
        public string Design { get; set; }
        /// <summary>
        /// Tổng số module thiết kế/tổng cv
        /// </summary>
        public string ModuleDesign { get; set; }
        /// <summary>
        /// Tổng số module sửa thiết kế cũ/tổng cv
        /// </summary>
        public string ModuleUpdateDesign { get; set; }
        /// <summary>
        /// Tổng số module tận dụng (sử dụng)/tổng cv
        /// </summary>
        public string ModuleUse { get; set; }
        /// <summary>
        /// Tổng số mô hình thiết kế/tổng cv
        /// </summary>
        public string ParadigmDesign { get; set; }
        /// <summary>
        /// Tổng số mô hình sửa thiết kế cũ/tổng cv
        /// </summary>
        public string ParadigmUpdateDesign { get; set; }
        /// <summary>
        /// tổng số mô hình tận dụng (sử dụng)/tổng cv
        /// </summary>
        public string ParadigmUse { get; set; }
        /// <summary>
        /// Tổng số công việc chưa có kế hoạch cho module
        /// </summary>
        public string ModuleTaskNotPlan { get; set; }
        /// <summary>
        /// Tổng số công việc chưa có kế hoạch theo mô hình/tổng cv
        /// </summary>
        public string ParadigmTaskNotPlan { get; set; }
    }
    
}
