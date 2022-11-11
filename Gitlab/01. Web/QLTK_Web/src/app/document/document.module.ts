import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { DevExtremeModule, DxContextMenuModule, DxDataGridModule, DxDateBoxModule, DxDropDownBoxModule, DxTreeListModule, DxTreeViewModule } from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';

import { DocumentRoutingModule } from './document-routing.module';

import { DocumentManageComponent } from './document-manage/document-manage.component';
import { DocumentGroupCreateComponent } from './document-group-create/document-group-create.component';
import { DocumentTypeManageComponent } from './document-type/document-type-manage/document-type-manage.component';
import { DocumentTypeCreateComponent } from './document-type/document-type-create/document-type-create.component';
import { DocumentCreateComponent } from './document-create/document-create.component';
import { ChooseDepartmentComponent } from './choose-department/choose-department.component';
import { ChooseWorktypeComponent } from './choose-worktype/choose-worktype.component';
import { DocumentFileTabComponent } from './document-file-tab/document-file-tab.component';
import { ChooseDocumentReferenceComponent } from './choose-document-reference/choose-document-reference.component';
import { DocumentPromulgateTabComponent } from './document-promulgate-tab/document-promulgate-tab.component';
import { DocumentPromulgateCreateComponent } from './document-promulgate-create/document-promulgate-create.component';
import { DocumentReviewTabComponent } from './document-review-tab/document-review-tab.component';
import { DocumentReviewCreateComponent } from './document-review-create/document-review-create.component';
import { ReportMeetingCreateComponent } from './report-meeting-create/report-meeting-create.component';
import { DocumentViewComponent } from './document-view/document-view.component';
import { DocumentSearchManageComponent } from './document-search-manage/document-search-manage.component';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { ChooseWorkComponent } from './choose-work/choose-work.component';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [
    DocumentManageComponent,
    DocumentGroupCreateComponent,
    DocumentTypeManageComponent,
    DocumentTypeCreateComponent,
    DocumentCreateComponent,
    ChooseDepartmentComponent,
    ChooseWorktypeComponent,
    DocumentFileTabComponent,
    ChooseDocumentReferenceComponent,
    DocumentPromulgateTabComponent,
    DocumentPromulgateCreateComponent,
    DocumentReviewTabComponent,
    DocumentReviewCreateComponent,
    ReportMeetingCreateComponent,
    DocumentViewComponent,
    DocumentSearchManageComponent,
    ChooseWorkComponent,
  ],
  imports: [
    CommonModule,
    DocumentRoutingModule,
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
    PdfJsViewerModule,
    NgSelectModule,
  ],
  providers:[
    NgbActiveModal 
  ]
})
export class DocumentModule { }
