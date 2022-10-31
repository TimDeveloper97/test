import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { CustomerTypeManageComponent } from './customer-type/customer-type-manage/customer-type-manage.component';
import { CustomerManageComponent } from './customer/customer-manage/customer-manage.component';
import { DashboardEmployeeComponent } from './dashboard-employee/dashboard-employee.component';
import { DashbroadProjectComponent } from './dashbroad-project/dashbroad-project.component';
import { ErrorManageComponent } from './error/error-manage/error-manage.component';
import { GeneralInformationProjectComponent } from './general-information-project/general-information-project.component';
import { HolidayManagerComponent } from './plan/holiday-manager/holiday-manager.component';
import { PlanManageComponent } from './plan/plan-manage/plan-manage.component';
import { ScheduleProjectComponent } from './plan/schedule-project/schedule-project.component';
import { WorkingTimeComponent } from './plan/working-time/working-time.component';
import { ProblemExistConfirmComponent } from './problem-exist/problem-exist-confirm/problem-exist-confirm.component';
import { ProblemExistCreateComponent } from './problem-exist/problem-exist-create/problem-exist-create.component';
import { ProblemExistManageComponent } from './problem-exist/problem-exist-manage/problem-exist-manage.component';
import { ProjectCreateComponent } from './project/project-create/project-create.component';
import { ProjectManageComponent } from './project/project-manage/project-manage.component';
import { ProjectUpdateComponent } from './project/project-update/project-update.component';
import { TaskTimeStandardManageComponent } from './task-time-standard/task-time-standard-manage/task-time-standard-manage.component';
import { TaskManageComponent } from './task/task-manage/task-manage.component';
import { ShowProjectComponent } from './project/show-project/show-project.component';
import { ProjectRoleComponent } from './role/project-role/project-role.component';
import { LogWorkTimeComponent } from './log-work-time/log-work-time.component';
import { PlanHistoryViewComponent } from './plan/plan-history-view/plan-history-view.component';
import { CustomerCreateComponent } from './customer/customer-create/customer-create.component';
import { SaleCustomerCreateComponent } from '../sale/export-and-keep/customer-create/customer-create.component';
import { CustomerProjectComponent } from './customer/customer-project/customer-project.component';
import { CustomerUpdateComponent } from './customer/customer-update/customer-update.component';
import { MoneyCollectionReportComponent } from './money-collection-report/money-collection-report.component';
import { ProductCreateComponent } from '../device/product-create/product-create.component';
import { ProductNeedPublicationsComponent } from './product-need-publications/product-need-publications.component';
import { GanttChartComponent } from './plan/gantt-chart/gantt-chart.component';


const routes: Routes = [
  {
    path: 'quan-ly-du-an',
    children: [
      { path: '', component: ProjectManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: ProjectCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: ProjectUpdateComponent, canActivate: [AuthGuard] },
      { path: 'xem/:Id', component: ShowProjectComponent, canActivate: [AuthGuard] },
      { path: 'lich-su-thay-doi-gia-han-hop-dong/:Id', component: PlanHistoryViewComponent, canActivate: [AuthGuard] },
      { path: 'xem-tai-lieu-dinh-kem/:Id', component: ProductCreateComponent, canActivate: [AuthGuard]},
      { path: 'gantt-chart/:Id', component: GanttChartComponent, canActivate: [AuthGuard] }
    ]
  },
  
  { path: 'vi-tri-cong-viec', component: ProjectRoleComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-loai-khach-hang', component: CustomerTypeManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-khach-hang', children: [
    { path: '', component: CustomerManageComponent, canActivate: [AuthGuard] },
    { path: 'them-moi', component: CustomerCreateComponent, canActivate: [AuthGuard] },
    { path: 'customer/chinh-sua/:Type/:Id', component: CustomerUpdateComponent, canActivate: [AuthGuard] },
    { path: 'sale-customer/chinh-sua/:Id', component: SaleCustomerCreateComponent, canActivate: [AuthGuard] },
   
  ] },
  {
    path: 'quan-ly-van-de',
    children: [
      { path: '', component: ProblemExistManageComponent, canActivate: [AuthGuard] },
      { path: 'van-de/:Id', component: ProblemExistCreateComponent, canActivate: [AuthGuard] },
      { path: 'xac-nhan-van-de/:Id', component: ProblemExistConfirmComponent, canActivate: [AuthGuard] }
    ]
  },
  { path: 'cau-hinh-cong-viec-theo-nhom-module', component: TaskManageComponent, canActivate: [AuthGuard] },
  { path: 'thoi-gian-tieu-chuan-cho-tung-cong-viec', component: TaskTimeStandardManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ke-hoach', component: PlanManageComponent, canActivate: [AuthGuard] },
  { path: 'ke-hoach-thiet-ke', component: ScheduleProjectComponent, canActivate: [AuthGuard] },
  { path: 'cau-hinh-thong-tin-ngay-nghi', component: HolidayManagerComponent, canActivate: [AuthGuard] },
  { path: 'thoi-gian-lam-viec', component: WorkingTimeComponent, canActivate: [AuthGuard] },
  { path: 'tong-hop-ke-hoach-thiet-ke', component: GeneralInformationProjectComponent, canActivate: [AuthGuard] },
  { path: 'dashboard-employee', component: DashboardEmployeeComponent, canActivate: [AuthGuard] },
  { path: 'dashboard-project', component: DashbroadProjectComponent, canActivate: [AuthGuard] },
  { path: 'log-thoi-gian-lam-viec', component: LogWorkTimeComponent, canActivate: [AuthGuard] },
  { path: 'bao-cao-thu-tien-du-an', component: MoneyCollectionReportComponent, canActivate: [AuthGuard] },
  { path: 'thiet-bi-can-an-pham', component: ProductNeedPublicationsComponent, canActivate: [AuthGuard] },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectRoutingModule { }
