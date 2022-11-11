import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../auth/guards/auth.guard';
import { CostWarningComponent } from './cost-warning/cost-warning.component';
import { CurrentCostWarningComponent } from './current-cost-warning/current-cost-warning.component';

import { EmployeePresentComponent } from './employee-present/employee-present.component';
import { ForecastProjectsComponent } from './forecast-projects/forecast-projects.component';
import { FuturePersonnelForecastComponent } from './future-personnel-forecast/future-personnel-forecast.component';
import { MasterEmployeeLevelComponent } from './master-employee-level/master-employee-level.component';
import { MasterEmployeeComponent } from './master-employee/master-employee.component';
import { MasterLibraryReportComponent } from './master-library-report/master-library-report.component';
import { ReportApplicationPresentComponent } from './report-application-present/report-application-present.component';
import { ReportErrorAffectComponent } from './report-error-affect/report-error-affect.component';
import { ReportErrorProgressComponent } from './report-error-progress/report-error-progress.component';
import { ReportErrorComponent } from './report-error/report-error.component';
import { ReportQualityPresentComponent } from './report-quality-present/report-quality-present.component';
import { ReportStatusModuleComponent } from './report-status-module/report-status-module.component';
import { ReportStatusProductComponent } from './reprort-status-product/report-status-product/report-status-product.component';
import { StatusReportMaterialComponent } from './status-report-material/status-report-material.component';
import { ReportProgressProjectComponent } from './report-progress-project/report-progress-project.component';
import { ReportSalesBusinessComponent } from './report-sales-business/report-sales-business.component';

const routes: Routes = [
  // { path: 'bao-cao-chat-luong-hien-tai', component: CurrentQualityReportComponent, canActivate: [AuthGuard] },
  {
    path: 'bao-cao-du-an-tuong-lai',
    component: ForecastProjectsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'du-bao-nhan-su-tuong-lai',
    component: FuturePersonnelForecastComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-nhan-vien-hien-tai',
    component: EmployeePresentComponent,
    canActivate: [AuthGuard],
  },
  // { path: 'bao-cao-toan-bo-du-lieu-data-master-theo-luong', component: TotalDatamasterStreamComponent, canActivate: [AuthGuard] }
  {
    path: 'bao-cao-master-thu-vien',
    component: MasterLibraryReportComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-master-level-nhan-su',
    component: MasterEmployeeLevelComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-master-nhan-su',
    component: MasterEmployeeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-chat-luong-hien-tai',
    component: ReportQualityPresentComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-tinh-trang-vat-tu',
    component: StatusReportMaterialComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'canh-bao-chi-phi',
    component: CostWarningComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'canh-bao-chi-phi-hien-tai',
    component: CurrentCostWarningComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-tinh-trang-module',
    component: ReportStatusModuleComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-tinh-trang-product',
    component: ReportStatusProductComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-ung-dung-hien-tai',
    component: ReportApplicationPresentComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-van-de-ton-dong',
    component: ReportErrorComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-tien-do-van-de-ton-dong',
    component: ReportErrorProgressComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-yeu-to-anh-huong',
    component: ReportErrorAffectComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'bao-cao-tien-do-trien-khai-du-an',
    component: ReportProgressProjectComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'report-sales-business',
    component: ReportSalesBusinessComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportRoutingModule {}
