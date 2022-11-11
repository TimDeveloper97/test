import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ImportRoutingModule } from './import-routing.module';
import { SharedModule } from '../shared/shared.module';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { DevExtremeModule, DxContextMenuModule, DxDataGridModule, DxDateBoxModule, DxDropDownBoxModule, DxTreeListModule, DxTreeViewModule } from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { ImportProfileManageComponent } from './import-profile-manage/import-profile-manage.component';
import { ImportProfileCreateComponent } from './import-profile-create/import-profile-create.component';
import { ImportProfileUpdateComponent } from './import-profile-update/import-profile-update.component';
import { ImportConfigComponent } from './import-config/import-config.component';
import { ImportPrComponent } from './import-pr/import-pr.component';
import { ChooseMaterialImportPrComponent } from './choose-material-import-pr/choose-material-import-pr.component';
import { ReportOngoingComponent } from './reports/report-ongoing/report-ongoing.component';
import { ReportSummaryComponent } from './reports/report-summary/report-summary.component';
import { ChartsModule } from 'ng2-charts';
import { ImportProfileHistoryComponent } from './import-profile-history/import-profile-history.component';
import { ImportProfileViewComponent } from './import-profile-view/import-profile-view.component';
import { ReportProblemExistManageComponent } from './reports/report-problem-exist/report-problem-exist-manage/report-problem-exist-manage.component';
import { ReportProblemExistCreateComponent } from './reports/report-problem-exist/report-problem-exist-create/report-problem-exist-create.component';


@NgModule({
  declarations: [
    ImportProfileManageComponent,
    ImportProfileCreateComponent,
    ImportProfileUpdateComponent,
    ImportConfigComponent,
    ImportPrComponent,
    ChooseMaterialImportPrComponent,
    ReportOngoingComponent,
    ReportSummaryComponent,
    ImportProfileHistoryComponent,
    ImportProfileViewComponent,
    ReportProblemExistManageComponent,
    ReportProblemExistCreateComponent,
  ],
  imports: [
    CommonModule,
    ImportRoutingModule,
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
    ChartsModule
  ],
  entryComponents: [
    ChooseMaterialImportPrComponent
  ],
  providers:[
    NgbActiveModal
  ]
})
export class ImportModule { }
