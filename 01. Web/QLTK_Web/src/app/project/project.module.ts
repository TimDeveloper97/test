import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller'; 
import {
  DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
  DxDateBoxModule,
  DxDataGridModule,
  DxGanttModule,
  DxCheckBoxModule,
  DxSelectBoxModule
} from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { AngularSplitModule } from 'angular-split';

import { SharedModule } from '../shared/shared.module';

import { ProjectRoutingModule } from './project-routing.module';

import { ProjectManageComponent } from './project/project-manage/project-manage.component';
import { ProjectCreateComponent } from './project/project-create/project-create.component';
import { ProjectUpdateComponent } from './project/project-update/project-update.component';
import { ProjectAttachTabComponent } from './project/project-attach-tab/project-attach-tab.component';
import { CustomerTypeManageComponent } from './customer-type/customer-type-manage/customer-type-manage.component';
import { CustomerTypeCreateComponent } from './customer-type/customer-type-create/customer-type-create.component';
import { CustomerManageComponent } from './customer/customer-manage/customer-manage.component';
import { CustomerCreateComponent } from './customer/customer-create/customer-create.component';
import { ImportFileComponent } from './customer/import-file/import-file.component';
import { ErrorManageComponent } from './error/error-manage/error-manage.component';
import { ErrorCreateComponent } from './error/error-create/error-create.component';
import { ErrorGroupCreateComponent } from './error-group/error-group-create/error-group-create.component';
import { ErrorConfirmComponent } from './error/error-confirm/error-confirm.component';
import { ErrorHistoryComponent } from './error/error-history/error-history.component';
import { ProblemExistManageComponent } from './problem-exist/problem-exist-manage/problem-exist-manage.component';
import { ProjectErrorComponent } from './project/project-errors/project-error/project-error.component';
import { ProjectProductComponent } from './project/project-product/project-product.component';
import { ProjectProductCreateComponent } from './project/project-product-create/project-product-create.component';
import { TaskManageComponent } from './task/task-manage/task-manage.component';
import { TaskCreateComponent } from './task/task-create/task-create.component';
import { TaskTimeStandardManageComponent } from './task-time-standard/task-time-standard-manage/task-time-standard-manage.component';
import { TaskTimeStandardCreateComponent } from './task-time-standard/task-time-standard-create/task-time-standard-create.component';
import { TaskModuleGroupManageComponent } from './task-module-group/task-module-group-manage/task-module-group-manage.component';
import { TaskModuleGroupCreateComponent } from './task-module-group/task-module-group-create/task-module-group-create.component';
import { PlanManageComponent } from './plan/plan-manage/plan-manage.component';
import { PlanCreateComponent } from './plan/plan-create/plan-create.component';
import { PlanViewComponent } from './plan/plan-view/plan-view.component';
import { ProjectSolutionComponent } from './project/project-solution/project-solution.component';
import { ProblemExistConfirmComponent } from './problem-exist/problem-exist-confirm/problem-exist-confirm.component';
import { ProblemExistCreateComponent } from './problem-exist/problem-exist-create/problem-exist-create.component';
import { ProblemExistHistoryComponent } from './problem-exist/problem-exist-history/problem-exist-history.component';
import { ScheduleProjectComponent } from './plan/schedule-project/schedule-project.component';
import { WorkingTimeComponent } from './plan/working-time/working-time.component';
import { GeneralInformationProjectComponent } from './general-information-project/general-information-project.component';
import { DashboardEmployeeComponent } from './dashboard-employee/dashboard-employee.component';
import { DashbroadProjectComponent } from './dashbroad-project/dashbroad-project.component';
import { ProjectTransferAttachComponent } from './project/project-transfer-attach/project-transfer-attach.component';
import { InformationErrorComponent } from './project/information-error/information-error.component';
import { CustomerContactCreateComponent } from './customer/customer-contact-create/customer-contact-create.component';
import { ChooseSolutionComponent } from './project/choose-solution/choose-solution.component';
import { ProjectErrorConfirmComponent } from './project/project-errors/project-error-confirm/project-error-confirm.component';
import { ProjectErrorCreateComponent } from './project/project-errors/project-error-create/project-error-create.component';
import { ProjectErrorHistoryComponent } from './project/project-errors/project-error-history/project-error-history.component';
import { ProjectGeneralDesignCreateComponent } from './project/project-general-design-create/project-general-design-create.component';
import { ProjectGeneralDesignComponent } from './project/project-general-design/project-general-design.component';
import { ChooseMaterialGeneralDesignComponent } from './project/choose-material-general-design/choose-material-general-design.component';
import { ProjectGeneralDesignManagaComponent } from './project/project-general-design-managa/project-general-design-managa.component';
import { ProjectProductBomComponent } from './project/project-product-bom/project-product-bom.component';
import { ProjectProductBomCreateComponent } from './project/project-product-bom-create/project-product-bom-create.component';
import { HolidayManagerComponent } from './plan/holiday-manager/holiday-manager.component';
import { ViewDetailComponent } from './view-detail/view-detail/view-detail.component';
import { CreateTimePerformComponent } from './plan/create-time-perform/create-time-perform.component';
import { ShowChoosePlanDesignComponent } from './project/show-choose-plan-design/show-choose-plan-design.component';
import { ShowChooseModuleComponent } from './project/show-choose-module/show-choose-module.component';
import { ConfirmTransferComponent } from './project/confirm-transfer/confirm-transfer.component';
import { ViewPlanDetailComponent } from './plan/view-plan-detail/view-plan-detail.component';
import { TaskTimeStandardChooseYearModalComponent } from './task-time-standard-choose-year-modal/task-time-standard-choose-year-modal.component';
import { OTPlanTimeComponent } from './plan/otplan-time/otplan-time.component';
import { ProjectAttachCreateComponent } from './project/project-attach-create/project-attach-create.component';
import { ProjectProductMaterialComponent } from './project/project-product-material/project-product-material.component';
import { ShowChooseModuleSaleComponent } from './project/show-choose-module-sale/show-choose-module-sale.component';
import { IncurredMaterialComponent } from './project/incurred-material/incurred-material.component';
import { ProblemExistConfirmModalComponent } from './problem-exist/problem-exist-confirm-modal/problem-exist-confirm-modal.component';
import { ShowProjectComponent } from './project/show-project/show-project.component';
import { ShowProjectErrorsComponent } from './project/show-project-errors/show-project-errors.component';
import { ShowProjectAttachTabComponent } from './project/show-project-attach-tab/show-project-attach-tab.component';
import { ShowProjectTransferAttachComponent } from './project/show-project-transfer-attach/show-project-transfer-attach.component';
import { ShowProjectProductComponent } from './project/show-project-product/show-project-product.component';
import { ShowProjectSolutionComponent } from './project/show-project-solution/show-project-solution.component';
import { ShowProjectProductBomComponent } from './project/show-project-product-bom/show-project-product-bom.component';
import { ShowProjectProductMaterialComponent } from './project/show-project-product-material/show-project-product-material.component';
import { ShowProjectGeneralDesignComponent } from './project/show-project-general-design/show-project-general-design.component';
import { ViewProjectAttachTabComponent } from './project/show-project-attach-tab/view-project-attach-tab/view-project-attach-tab.component';
import { ProjectProductMaterialCompareComponent } from './project/project-product-material-compare/project-product-material-compare.component';
import { ProjectMaterialThreeTableCompareComponent } from './project/project-material-three-table-compare/project-material-three-table-compare.component';
import { ProjectAttachTabTypeComponent } from './project/project-attach-tab-type/project-attach-tab-type.component';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { ProjectPaymentCreateModalComponent } from './project/project-payment-create-modal/project-payment-create-modal.component';
import { ChooseStageModalComponent } from './plan/choose-stage-modal/choose-stage-modal.component';
import { RoleCreateUpdateComponent } from './role/role-create-update/role-create-update.component';
import { ProjectRoleComponent } from './role/project-role/project-role.component';

import { ProblemExistCreateChangePlanComponent } from './problem-exist/problem-exist-create-change-plan/problem-exist-create-change-plan.component';
import { ProblemExistHistoryChangePlanComponent } from './problem-exist/problem-exist-history-change-plan/problem-exist-history-change-plan.component';
import { ProjectProductStatusPerformComponent } from './project/project-product-status-perform/project-product-status-perform.component';
import { WorkingReportComponent } from './plan/working-report/working-report.component';
import { PiechartComponent } from './plan/piechart/piechart.component';
import { ChartsModule } from 'ng2-charts';
import { OverallProjectComponent } from './plan/overall-project/overall-project.component';

import { ProjectEmployeeComponent } from './project/project-employee/project-employee.component';
import { ChooseEmployeeComponent } from './project/choose-employee/choose-employee.component';
import { ProjectEmployeeCreateComponent } from './project/project-employee-create/project-employee-create.component';
import { ProjectEmployeeUpdateStatusSubsidyHistoryComponent } from './project/project-employee-update-status-subsidy-history/project-employee-update-status-subsidy-history.component';

import { DashboardPlanComponent } from './plan/dashboard-plan/dashboard-plan.component';
import { PopupSearchComponent } from './plan/popup-search/popup-search.component';
import { RiskProblemProjectComponent } from './risk-problem-project/risk-problem-project.component';
import { PlanAdjustmentComponent } from './plan/plan-adjustment/plan-adjustment.component';
import { PlanProjectCreateComponent } from './plan/plan-project-create/plan-project-create.component';
import { PlanCopyComponent } from './plan/plan-copy/plan-copy.component';
import { PlanEmployeeComponent } from './plan/plan-employee/plan-employee.component';
import { LogWorkTimeComponent } from './log-work-time/log-work-time.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { PlanHistoryViewComponent } from './plan/plan-history-view/plan-history-view.component';
import { PlanHistoryComponent } from './plan/plan-history/plan-history.component';
import { CustomerProjectComponent } from './customer/customer-project/customer-project.component';
import { CustomerRequirementComponent } from './customer/customer-requirement/customer-requirement.component';
import { CustomerUpdateComponent } from './customer/customer-update/customer-update.component';
import { MoneyCollectionReportComponent } from './money-collection-report/money-collection-report.component';
import { ShowListMoneyCollectionComponent } from './money-collection-report/show-list-money-collection/show-list-money-collection.component';
import { CustomerMeetingComponent } from './customer/customer-meeting/customer-meeting.component';
import { ProjectProductQcComponent } from './project/project-product-qc/project-product-qc.component';
import { ShowExportExcelProjectPlanComponent } from './money-collection-report/show-export-excel-project-plan/show-export-excel-project-plan.component';
import { ProductNeedPublicationsComponent } from './product-need-publications/product-need-publications.component';
import { CustomerQuotationComponent } from './customer/customer-quotation/customer-quotation.component';
import { QcCheckListCreateComponent } from './project/qc-check-list-create/qc-check-list-create.component';
import { CopyQcCheckListComponent } from './project/copy-qc-check-list/copy-qc-check-list.component';
import { ShowQcCheckListComponent } from './project/show-qc-check-list/show-qc-check-list.component';
import { DayMarkersService, GanttModule } from '@syncfusion/ej2-angular-gantt';
import { DashboardProjectComponent } from './plan/dashboard-project/dashboard-project.component';
import { ReportPlanByDateComponent } from './plan/report-plan-by-date/report-plan-by-date.component';
import { GanttChartComponent } from './plan/gantt-chart/gantt-chart.component';
import { CreateUpdatePlanSaleTargetmentComponent } from './plan/create-update-plan-sale-targetment/create-update-plan-sale-targetment.component';

@NgModule({
  declarations: [
    ProjectManageComponent,
    ProjectCreateComponent,
    ProjectUpdateComponent,
    ProjectAttachTabComponent,
    CustomerTypeManageComponent,
    CustomerManageComponent,
    ErrorManageComponent,
    ErrorCreateComponent,
    ErrorConfirmComponent,
    ErrorHistoryComponent,
    ProblemExistManageComponent,
    ProjectErrorComponent,
    ProjectProductComponent,
    TaskManageComponent,
    TaskTimeStandardManageComponent,
    TaskModuleGroupManageComponent,
    PlanManageComponent,
    ProjectSolutionComponent,
    ProblemExistConfirmComponent,
    ProblemExistCreateComponent,
    ProblemExistHistoryComponent,
    ScheduleProjectComponent,
    WorkingTimeComponent,
    GeneralInformationProjectComponent,
    DashboardEmployeeComponent,
    DashbroadProjectComponent,
    ProjectTransferAttachComponent,
    ProjectErrorHistoryComponent,
    ProjectGeneralDesignManagaComponent,
    ProjectProductBomComponent,
    HolidayManagerComponent,
    CustomerTypeCreateComponent,
    CustomerCreateComponent,
    ImportFileComponent,
    ErrorGroupCreateComponent,
    ProjectProductCreateComponent,
    TaskCreateComponent,
    TaskTimeStandardCreateComponent,
    TaskModuleGroupCreateComponent,
    PlanCreateComponent,
    InformationErrorComponent,
    CustomerContactCreateComponent,
    ChooseSolutionComponent,
    ProjectErrorConfirmComponent,
    ProjectErrorCreateComponent,
    ProjectGeneralDesignComponent,
    ProjectGeneralDesignCreateComponent,
    ChooseMaterialGeneralDesignComponent,
    ProjectProductBomCreateComponent,
    ViewDetailComponent,
    CreateTimePerformComponent,
    ShowChoosePlanDesignComponent,
    ShowChooseModuleComponent,
    ConfirmTransferComponent,
    ViewPlanDetailComponent,
    TaskTimeStandardChooseYearModalComponent,
    OTPlanTimeComponent,
    PlanViewComponent,
    ProjectAttachCreateComponent,
    ProjectProductMaterialComponent,
    ShowChooseModuleSaleComponent,
    IncurredMaterialComponent,
    ProblemExistConfirmModalComponent,
    ShowProjectComponent,
    ShowProjectErrorsComponent,
    ShowProjectAttachTabComponent,
    ShowProjectTransferAttachComponent,
    ShowProjectProductComponent,
    ShowProjectSolutionComponent,
    ShowProjectProductBomComponent,
    ShowProjectProductMaterialComponent,
    ShowProjectGeneralDesignComponent,
    ViewProjectAttachTabComponent,
    ProjectProductMaterialCompareComponent,
    ProjectMaterialThreeTableCompareComponent,
    ProjectAttachTabTypeComponent,
    ProjectPaymentCreateModalComponent,
    ChooseStageModalComponent,
    RoleCreateUpdateComponent,
    ProjectRoleComponent,
    ProblemExistCreateChangePlanComponent,
    ProblemExistHistoryChangePlanComponent,
    ProjectProductStatusPerformComponent,
    WorkingReportComponent,
    PiechartComponent,
    OverallProjectComponent,
    DashboardPlanComponent,
    PopupSearchComponent,
    ProjectEmployeeComponent,
    ChooseEmployeeComponent,
    ProjectEmployeeCreateComponent,
    ProjectEmployeeUpdateStatusSubsidyHistoryComponent,
    DashboardPlanComponent,
    RiskProblemProjectComponent,
    PlanAdjustmentComponent,
    PlanProjectCreateComponent,
    PlanCopyComponent,
    PlanEmployeeComponent,
    LogWorkTimeComponent,
    PlanHistoryViewComponent,
    PlanHistoryComponent,
    CustomerProjectComponent,
    CustomerRequirementComponent,
    CustomerUpdateComponent,
    MoneyCollectionReportComponent,
    ShowListMoneyCollectionComponent,
    CustomerMeetingComponent,
    ProjectProductQcComponent,
    ShowExportExcelProjectPlanComponent,
    ProductNeedPublicationsComponent,
    CustomerQuotationComponent,
    QcCheckListCreateComponent,
    CopyQcCheckListComponent,
    ShowQcCheckListComponent,
    DashboardProjectComponent,
    ReportPlanByDateComponent,
    GanttChartComponent,
    CreateUpdatePlanSaleTargetmentComponent
  ],
  imports: [
    CommonModule,
    ProjectRoutingModule,
    FormsModule,
    SharedModule,
    PerfectScrollbarModule,
    NgbModule,
    VirtualScrollerModule,
    DevExtremeModule,
    DxTreeListModule,
    DxTreeViewModule,
    DxDropDownBoxModule,
    DxContextMenuModule,
    DxDateBoxModule,
    DxDataGridModule,
    CurrencyMaskModule,
    NgxGalleryModule,
    AngularSplitModule,
    PdfJsViewerModule,
    ChartsModule,
    NgSelectModule,
    DxGanttModule,
    DxCheckBoxModule,
    DxSelectBoxModule,
    GanttModule,
  ],
  entryComponents: [
    CustomerTypeCreateComponent,
    CustomerCreateComponent,
    ImportFileComponent,
    ErrorGroupCreateComponent,
    ProjectProductCreateComponent,
    TaskCreateComponent,
    TaskTimeStandardCreateComponent,
    TaskModuleGroupCreateComponent,
    PlanCreateComponent,
    InformationErrorComponent,
    CustomerContactCreateComponent,
    ChooseSolutionComponent,
    ProjectErrorConfirmComponent,
    ProjectErrorCreateComponent,
    ProjectGeneralDesignComponent,
    ProjectGeneralDesignCreateComponent,
    ChooseMaterialGeneralDesignComponent,
    ProjectProductBomCreateComponent,
    ViewDetailComponent,
    CreateTimePerformComponent,
    ShowChoosePlanDesignComponent,
    ShowChooseModuleComponent,
    ConfirmTransferComponent,
    ViewPlanDetailComponent,
    TaskTimeStandardChooseYearModalComponent,
    OTPlanTimeComponent,
    PlanViewComponent,
    ProjectAttachCreateComponent,
    ProblemExistConfirmModalComponent
  ],
  providers: [
    NgbActiveModal,
    DatePipe,
    DayMarkersService,
  ]
})
export class ProjectModule { }
