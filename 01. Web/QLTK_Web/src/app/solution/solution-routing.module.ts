import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../auth/guards/auth.guard';

import { SolutionCreateComponent } from './solution-create/solution-create.component';
import { SolutionManageComponent } from './solution-manage/solution-manage.component';
import { SolutionUpdateComponent } from './solution-update/solution-update.component';
import { CustomerRequirementManageComponent } from './customer-requirement/customer-requirement-manage/customer-requirement-manage.component'
import { MeetingManageComponent } from './meeting/meeting-manage/meeting-manage.component';
import { MeetingFinishManageComponent } from './meeting/meeting-finish-manage/meeting-finish-manage.component';
import { MeetingJoinComponent } from './meeting/meeting-join/meeting-join.component';
import { CustomerRequirementCreateComponent } from './customer-requirement/customer-requirement-create/customer-requirement-create.component'
import { MeetingJoinFinishComponent } from './meeting/meeting-join-finish/meeting-join-finish.component';
import { ProjectPhaseManageComponent } from './project-phase/project-phase-manage/project-phase-manage.component'
import { MeetingCreateComponent } from './meeting/meeting-create/meeting-create.component';
import { SurveyCreateComponent } from './customer-requirement/customer-requirement-create/Survey-create/Survey-create.component'
import { SurveyContentManageComponent } from './customer-requirement/survey-content/survey-content-manage/survey-content-manage.component'
import { SummaryQuotesManageComponent } from './summary-quotes/summary-quotes-manage/summary-quotes-manage.component';
import { SummaryQuotesUpdateComponent } from './summary-quotes/summary-quotes-update/summary-quotes-update.component';
import { SummaryQuotesCreateComponent } from './summary-quotes/summary-quotes-create/summary-quotes-create.component';

const routes: Routes = [
  {
    path: 'quan-ly-giai-phap',
    children: [
      { path: '', component: SolutionManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi-giai-phap', component: SolutionCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua-giai-phap/:Id', component: SolutionUpdateComponent, canActivate: [AuthGuard] },
    ]
  },
  {
    path: 'yeu-cau-khach-hang',
    children: [
      { path: '', component: CustomerRequirementManageComponent, canActivate: [AuthGuard]},
      { path: 'them-moi', component: CustomerRequirementCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: CustomerRequirementCreateComponent, canActivate: [AuthGuard] },
      { path: 'khao-sat', component: SurveyCreateComponent, canActivate: [AuthGuard] },
      { path: 'khao-sat/noi-dung/:Id', component: SurveyContentManageComponent, canActivate: [AuthGuard]},
    ]
  },
  {
    path: 'tong-hop-bao-gia',
    children: [
      { path: '', component: SummaryQuotesManageComponent, canActivate: [AuthGuard]},
      { path: 'them-moi', component: SummaryQuotesCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: SummaryQuotesUpdateComponent, canActivate: [AuthGuard] },
      { path: 'them-moi-theo-yeu-cau-khach-hang/:Id/:CustomerId', component: SummaryQuotesCreateComponent, canActivate: [AuthGuard] },
    ]
  },
  {
    path: 'meeting',
    children: [
      { path: '', component: MeetingManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: MeetingCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: MeetingCreateComponent, canActivate: [AuthGuard] },
    ]
  },
  {
    path: 'meeting-ket-thuc',
    canActivate: [AuthGuard],
    component: MeetingFinishManageComponent
  },
  {
    path: 'meeting-tham-gia',
    canActivate: [AuthGuard],
    component: MeetingJoinComponent
  },
  {
    path: 'meeting-tham-gia-ket-thuc',
    canActivate: [AuthGuard],
    component: MeetingJoinFinishComponent
  },
  {
    path: 'quan-ly-giai-doan-cua-du-an',
    canActivate: [AuthGuard],
    component: ProjectPhaseManageComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SolutionRoutingModule { }
