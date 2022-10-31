import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RecruitmentRoutingModule } from './recruitment-routing.module';
import { CandidateManageComponent } from './candidate/candidate-manage/candidate-manage.component';
import { SharedModule } from '../shared/shared.module';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { DevExtremeModule, DxContextMenuModule, DxDataGridModule, DxDateBoxModule, DxDropDownBoxModule, DxTreeListModule, DxTreeViewModule } from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { ChartsModule } from 'ng2-charts';
import { CandidateCreateComponent } from './candidate/candidate-create/candidate-create.component';
import { RecruitmentChannelManageComponent } from './recruitment-channels/recruitment-channel-manage/recruitment-channel-manage.component';
import { RecruitmentChannelCreateComponent } from './recruitment-channels/recruitment-channel-create/recruitment-channel-create.component';
import { CandidateFollowTabComponent } from './candidate/candidate-follow-tab/candidate-follow-tab.component';
import { CandidateApplyTabComponent } from './candidate/candidate-apply-tab/candidate-apply-tab.component';
import { ApplyManageComponent } from './apply/apply-manage/apply-manage.component';
import { ApplyCreateComponent } from './apply/apply-create/apply-create.component';
import { ApplyCheckCandidateComponent } from './apply/apply-check-candidate/apply-check-candidate.component';
import { ChooseQuestionComponent } from './interview/choose-question/choose-question.component';
import { InterviewCreateComponent } from './interview/interview-create/interview-create.component';
import { InterviewManageComponent } from './interview/interview-manage/interview-manage.component';
import { ApplyInterviewTabComponent } from './apply/apply-interview-tab/apply-interview-tab.component';
import { RecruitmentRequestManageComponent } from './recruitment-request/recruitment-request-manage/recruitment-request-manage.component';
import { RecruitmentRequestCreateComponent } from './recruitment-request/recruitment-request-create/recruitment-request-create.component';
import { QuestionInterviewCreateComponent } from './interview/question-interview-create/question-interview-create.component';
import { ApplyInterviewInfoComponent } from './apply/apply-interview-info/apply-interview-info.component';
import { CandidateListComponent } from './recruitment-request/candidate-list/candidate-list.component';
import { CandidateApplysListComponent } from './recruitment-request/candidate-applys-list/candidate-applys-list.component';
import { MoreInterviewsComponent } from './apply/more-interviews/more-interviews.component';
import { QuestionMoreInterviewsCreateComponent } from './apply/question-more-interviews-create/question-more-interviews-create.component';
import { CandidatePassedComponent } from './candidate/candidate-passed/candidate-passed.component';


@NgModule({
  declarations: [
    CandidateCreateComponent,
    CandidateManageComponent,
    RecruitmentChannelManageComponent,
    RecruitmentChannelCreateComponent,
    CandidateFollowTabComponent,
    CandidateApplyTabComponent,
    ApplyManageComponent,
    ApplyCreateComponent,
    ApplyCheckCandidateComponent,
    InterviewCreateComponent,
    InterviewManageComponent,
    ChooseQuestionComponent,
    ApplyInterviewTabComponent,
    RecruitmentRequestManageComponent,
    RecruitmentRequestCreateComponent,
    QuestionInterviewCreateComponent,
    ApplyInterviewInfoComponent,
    CandidateListComponent,
    CandidateApplysListComponent,
    MoreInterviewsComponent,
    QuestionMoreInterviewsCreateComponent,
    CandidatePassedComponent
  ],
  imports: [
     CommonModule,
    SharedModule,
    PerfectScrollbarModule,
    NgbModule,
    FormsModule,
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
    RecruitmentRoutingModule
  ],
  providers:[
    NgbActiveModal 
  ]
})
export class RecruitmentModule { }
