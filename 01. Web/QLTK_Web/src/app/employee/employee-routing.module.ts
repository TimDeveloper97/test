import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../auth/guards/auth.guard';

import { ClassIficationManageComponent } from './class-ification/class-ification-manage/class-ification-manage.component';
import { CoefficientEmployeesComponent } from './coefficient-employees/coefficient-employees.component';
import { CourseManageComponent } from './course/course-manage/course-manage.component';
import { DepartmentManageComponent } from './department/department-manage/department-manage.component';
import { EmployeeGroupManageComponent } from './employee-group/employee-group-manage/employee-group-manage.component';
import { EmployeeTrainingManagerComponent } from './employee-training/employee-training-manager/employee-training-manager.component';
import { BankAccountManageComponent } from './Employee/bank-account/bank-account-manage/bank-account-manage.component';
import { EmployeeCreateComponent } from './Employee/employee-create/employee-create.component';
import { EmployeeManageComponent } from './Employee/employee-manage/employee-manage.component';
import { EmployeeUpdateComponent } from './Employee/employee-update/employee-update.component';
import { InsuranceLevelManageComponent } from './Employee/insurance-level/insurance-level-manage/insurance-level-manage.component';
import { LaborContractManageComponent } from './Employee/labor-contract/labor-contract-manage/labor-contract-manage.component';
import { ReasonChangeIncomeManageComponent } from './Employee/reason-change-income/reason-change-income-manage/reason-change-income-manage.component';
import { ReasonChangeInsuranceManageComponent } from './Employee/reason-change-insurance/reason-change-insurance-manage/reason-change-insurance-manage.component';
import { ReasonEndwordkingManageComponent } from './Employee/reason-endworking/reason-endwordking-manage/reason-endwordking-manage.component';
import { WorkLocationManageComponent } from './Employee/work-location/work-location-manage/work-location-manage.component';
import { GroupUserManageComponent } from './group-user/group-user-manage/group-user-manage.component';
import { JobPositionManageComponent } from './JobPosition/job-position-manage/job-position-manage.component';
import { OutputResultManageComponent } from './output-result/output-result-manage/output-result-manage.component';
import { QuestionManageComponent } from './question/question-manage/question-manage.component';
import { SalaryGroupManageComponent } from './salary-group/salary-group-manage/salary-group-manage.component';
import { SalaryLevelManageComponent } from './salary-level/salary-level-manage/salary-level-manage.component';
import { SalaryTypeManageComponent } from './salary-type/salary-type-manage/salary-type-manage.component';
import { SBUManageComponent } from './sbu/sbumanage/sbumanage.component';
import { TaskFlowStageCreateComponent } from './task/task-flow-stage-create/task-flow-stage-create.component';
import { TaskManageComponent } from './task/task-manage/task-manage.component';
import { TotalTimePerformComponent } from './total-time-perform/total-time-perform.component';
import { UserHistoryManagaComponent } from './UserHistory/user-history-managa/user-history-managa.component';
import { WorkDiaryManageComponent } from './work-diary/work-diary-manage/work-diary-manage.component';
import { WorkDiaryViewComponent } from './work-diary/work-diary-view/work-diary-view.component';
import { WorkTimeManageComponent } from './work-time/work-time-manage/work-time-manage.component';
import { WorkSkillManageComponent } from './workskill/work-skill-manage/work-skill-manage.component';
import { WorkTypeCreateComponent } from './worktype/work-type-create/work-type-create.component';
import { WorkTypeManageComponent } from './worktype/work-type-manage/work-type-manage.component';

const routes: Routes = [
  { path: 'quan-ly-SBU', component: SBUManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-phong-ban', component: DepartmentManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-chuc-vu', component: JobPositionManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-cau-hoi', component: QuestionManageComponent, canActivate: [AuthGuard] },
  {
    path: 'quan-ly-nhan-vien',
    children: [
      { path: '', component: EmployeeManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: EmployeeCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: EmployeeUpdateComponent, canActivate: [AuthGuard] },
    ]
  },

  { path: 'quan-ly-muc-luong', component: SalaryLevelManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-nhom-luong', component: SalaryGroupManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ngach-luong', component: SalaryTypeManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ket-qua-dau-ra', component: OutputResultManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-loai-hop-dong-lao-dong', component: LaborContractManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-noi-lam-viec', component: WorkLocationManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ly-do-dieu-chinh-bhxh', component: ReasonChangeInsuranceManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ly-do-dieu-chinh-thu-nhap', component: ReasonChangeIncomeManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ly-do-nghi-viec', component: ReasonEndwordkingManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ngan-hang', component: BankAccountManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-muc-dong-bao-hiem', component: InsuranceLevelManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-nhom-nhan-vien', component: EmployeeGroupManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-nhom-quyen', component: GroupUserManageComponent, canActivate: [AuthGuard] },


  {
    path: 'quan-ly-cong-viec', canActivate: [AuthGuard],
    children: [
      { path: '', component: TaskManageComponent, },
      { path: 'them-moi', component: TaskFlowStageCreateComponent, },
      { path: 'chinh-sua/:Id', component: TaskFlowStageCreateComponent, }
    ]
  },
  {
    path: 'quan-ly-vi-tri-cong-viec',
    children: [
      { path: '', component: WorkTypeManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: WorkTypeCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: WorkTypeCreateComponent, canActivate: [AuthGuard] },
    ]
  },

  { path: 'chuong-trinh-dao-tao', component: EmployeeTrainingManagerComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-khoa-hoc', component: CourseManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ky-nang', component: WorkSkillManageComponent, canActivate: [AuthGuard] },
  {
    path: 'nhat-ky-cong-viec',
    children: [
      { path: '', component: WorkDiaryManageComponent, canActivate: [AuthGuard] },
      { path: 'xem/:Id', component: WorkDiaryViewComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'tong-thoi-gian-thuc-hien', component: TotalTimePerformComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-xep-loai', component: ClassIficationManageComponent, canActivate: [AuthGuard] },
  { path: 'he-so-nang-luc-nhan-vien', component: CoefficientEmployeesComponent, canActivate: [AuthGuard] },
  { path: 'lich-su-thao-tac', component: UserHistoryManagaComponent, canActivate: [AuthGuard] },

  // { path: 'thoi-gian-cong-viec', component: WorkTimeManageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRoutingModule { }
