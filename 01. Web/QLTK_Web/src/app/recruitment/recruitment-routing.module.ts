import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { ApplyCreateComponent } from './apply/apply-create/apply-create.component';
import { ApplyInterviewInfoComponent } from './apply/apply-interview-info/apply-interview-info.component';
import { ApplyManageComponent } from './apply/apply-manage/apply-manage.component';
import { CandidateCreateComponent } from './candidate/candidate-create/candidate-create.component';
import { CandidateManageComponent } from './candidate/candidate-manage/candidate-manage.component';
import { InterviewCreateComponent } from './interview/interview-create/interview-create.component';
import { InterviewManageComponent } from './interview/interview-manage/interview-manage.component';
import { RecruitmentChannelManageComponent } from './recruitment-channels/recruitment-channel-manage/recruitment-channel-manage.component';
import { RecruitmentRequestManageComponent } from './recruitment-request/recruitment-request-manage/recruitment-request-manage.component';

const routes: Routes = [
  {
    path: 'ho-so-ung-vien',
    canActivate: [AuthGuard],
    children: [
      {
        path: '', component: CandidateManageComponent
      },
      {
        path: 'them-moi', component: CandidateCreateComponent
      },
      {
        path: 'chinh-sua/:id', component: CandidateCreateComponent
      },
      {
        path: 'thong-tin-phong-van/:id', component: ApplyInterviewInfoComponent
      }
    ]
  },
  {
    path: 'ung-tuyen',
    canActivate: [AuthGuard],
    children: [
      {
        path: '', component: ApplyManageComponent
      },
      {
        path: 'them-moi/:name/:phone/:email', component: ApplyCreateComponent
      },
      {
        path: 'them-moi-yeu-cau', component: ApplyCreateComponent
      },
      {
        path: 'chinh-sua/:id', component: ApplyCreateComponent
      },
      {
        path: 'thong-tin-phong-van/:id', component: ApplyInterviewInfoComponent
      }
    ]
  },
  {
    path: 'phong-van',
    canActivate: [AuthGuard],
    children: [
      {
        path: '', component: InterviewManageComponent
      },
      {
        path: 'them-moi/:id', component: InterviewCreateComponent
      },
    ]
  },
  {
    path: 'kenh-tuyen-dung',
    canActivate: [AuthGuard],
    component: RecruitmentChannelManageComponent
  },
  {
    path: 'yeu-cau-tuyen-dung',
    canActivate: [AuthGuard],
    component: RecruitmentRequestManageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RecruitmentRoutingModule { }
