import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import {
  DevExtremeModule,
  DxTreeListModule,
  DxTreeViewModule,
  DxDropDownBoxModule,
  DxContextMenuModule,
  DxDateBoxModule,
  DxDataGridModule,
} from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { ChartsModule } from 'ng2-charts';

import { SharedModule } from '../shared/shared.module';

import { ReportRoutingModule } from './report-routing.module';

import { CurrentQualityReportComponent } from './current-quality-report/current-quality-report.component';
import { FuturePersonnelForecastComponent } from './future-personnel-forecast/future-personnel-forecast.component';
import { ForecastProjectsComponent } from './forecast-projects/forecast-projects.component';
import { EmployeePresentComponent } from './employee-present/employee-present.component';
import { TotalDatamasterStreamComponent } from './total-datamaster-stream/total-datamaster-stream.component';
import { MasterLibraryReportComponent } from './master-library-report/master-library-report.component';
import { MasterEmployeeLevelComponent } from './master-employee-level/master-employee-level.component';
import { CostWarningComponent } from './cost-warning/cost-warning.component';
import { MasterEmployeeComponent } from './master-employee/master-employee.component';
import { ReportQualityPresentComponent } from './report-quality-present/report-quality-present.component';
import { StatusReportMaterialComponent } from './status-report-material/status-report-material.component';
import { ReportStatusModuleComponent } from './report-status-module/report-status-module.component';
import { SelectProjectComponent } from './select-project/select-project.component';
import { ReportApplicationPresentComponent } from './report-application-present/report-application-present.component';
import { CurrentCostWarningComponent } from './current-cost-warning/current-cost-warning.component';
import { ReportStatusProductComponent } from './reprort-status-product/report-status-product/report-status-product.component';
import { ReportErrorComponent } from './report-error/report-error.component';
import { ReportErrorWorkComponent } from './report-error-work/report-error-work.component';
import { ReportErrorProgressComponent } from './report-error-progress/report-error-progress.component';
import { ReportErrorAffectComponent } from './report-error-affect/report-error-affect.component';
import { ReportErrorChangePlanComponent } from './report-error-change-plan/report-error-change-plan.component';
import { ReportProgressProjectComponent } from './report-progress-project/report-progress-project.component';
import { ReportSalesBusinessComponent } from './report-sales-business/report-sales-business.component';

@NgModule({
  declarations: [
    CurrentQualityReportComponent,
    FuturePersonnelForecastComponent,
    ForecastProjectsComponent,
    EmployeePresentComponent,
    TotalDatamasterStreamComponent,
    MasterLibraryReportComponent,
    MasterEmployeeLevelComponent,
    CostWarningComponent,
    MasterEmployeeComponent,
    ReportQualityPresentComponent,
    StatusReportMaterialComponent,
    ReportStatusModuleComponent,
    ReportApplicationPresentComponent,
    CurrentCostWarningComponent,
    ReportStatusProductComponent,
    SelectProjectComponent,
    ReportErrorComponent,
    ReportErrorWorkComponent,
    ReportErrorProgressComponent,
    ReportErrorAffectComponent,
    ReportErrorChangePlanComponent,
    ReportProgressProjectComponent,
    ReportSalesBusinessComponent,
  ],
  imports: [
    CommonModule,
    ReportRoutingModule,
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
    ChartsModule,
  ],
  entryComponents: [SelectProjectComponent],
  providers: [NgbActiveModal],
})
export class ReportModule {}
