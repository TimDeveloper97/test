import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { ImportConfigComponent } from './import-config/import-config.component';
import { ImportPrComponent } from './import-pr/import-pr.component';
import { ImportProfileCreateComponent } from './import-profile-create/import-profile-create.component';
import { ImportProfileHistoryComponent } from './import-profile-history/import-profile-history.component';
import { ImportProfileManageComponent } from './import-profile-manage/import-profile-manage.component';
import { ImportProfileUpdateComponent } from './import-profile-update/import-profile-update.component';
import { ImportProfileViewComponent } from './import-profile-view/import-profile-view.component';
import { ReportOngoingComponent } from './reports/report-ongoing/report-ongoing.component';
import { ReportProblemExistCreateComponent } from './reports/report-problem-exist/report-problem-exist-create/report-problem-exist-create.component';
import { ReportProblemExistManageComponent } from './reports/report-problem-exist/report-problem-exist-manage/report-problem-exist-manage.component';
import { ReportSummaryComponent } from './reports/report-summary/report-summary.component';

const routes: Routes = [
  {
    path: 'import-pr',
    component: ImportPrComponent,
  },
  {
    path: 'ho-so-nhap-khau',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: ImportProfileManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: ImportProfileCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: ImportProfileUpdateComponent, canActivate: [AuthGuard] },
      { path: 'xem/:Id', component: ImportProfileViewComponent, canActivate: [AuthGuard] },
      { path: 'ket-thuc', component: ImportProfileHistoryComponent, canActivate: [AuthGuard] },
    ]
  },
  {
    path: 'ho-so-nhap-khau-ket-thuc',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: ImportProfileHistoryComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: ImportProfileUpdateComponent, canActivate: [AuthGuard] },
      { path: 'xem/:Id', component: ImportProfileViewComponent, canActivate: [AuthGuard] },
    ]
  },
  {
    path: 'cau-hinh',
    component: ImportConfigComponent,
  },
  {
    path: 'report',
    canActivate: [AuthGuard],
    children: [
      { path: 'dang-thuc-hien', component: ReportOngoingComponent },
      { path: 'tong-hop', component: ReportSummaryComponent },
      { path: 'van-de-ton-dong', component: ReportProblemExistCreateComponent, canActivate: [AuthGuard] },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ImportRoutingModule { }
