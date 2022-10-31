import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller'; import {
  DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
  DxDateBoxModule,
  DxDataGridModule
} from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';

import { SharedModule } from '../shared/shared.module';

import { SolutionRoutingModule } from './solution-routing.module';

import { SolutionManageComponent } from './solution-manage/solution-manage.component';
import { SolutionCreateComponent } from './solution-create/solution-create.component';
import { SolutionGroupCreateComponent } from './solution-group-create/solution-group-create.component';
import { SolutionUpdateComponent } from './solution-update/solution-update.component';
import { ChooseProjectSolutionComponent } from './choose-project-solution/choose-project-solution.component';
import { SolutionTabDesignDocumentComponent } from './solution-tab-design-document/solution-tab-design-document.component';
import { SolutionChooseFolderUploadModalComponent } from './solution-choose-folder-upload-modal/solution-choose-folder-upload-modal.component';
import { ShowEditContenComponent } from './show-edit-conten/show-edit-conten.component';
import { SolutionOldVersionComponent } from './solution-old-version/solution-old-version.component';
import { SolutionTabProductComponent } from './solution-tab-product/solution-tab-product.component';
import { SolutionProductCreateComponent } from './solution-product-create/solution-product-create.component';
import { CustomerRequirementManageComponent } from './customer-requirement/customer-requirement-manage/customer-requirement-manage.component'
import { CustomerRequirementCreateComponent } from './customer-requirement/customer-requirement-create/customer-requirement-create.component';
import { MeetingManageComponent } from './meeting/meeting-manage/meeting-manage.component';
import { MeetingCreateComponent } from './meeting/meeting-create/meeting-create.component'
import { ChooseCustomerContactComponent } from './meeting/choose-customer-contact/choose-customer-contact.component';
import { MeetingTypeCreateComponent } from './meeting/meeting-type-create/meeting-type-create.component';
import { MeetingFinishManageComponent } from './meeting/meeting-finish-manage/meeting-finish-manage.component';
import { MeetingDetailComponent } from './meeting/meeting-detail/meeting-detail.component';
import { MeetingJoinComponent } from './meeting/meeting-join/meeting-join.component';
import { MeetingJoinFinishComponent } from './meeting/meeting-join-finish/meeting-join-finish.component';
import { ProjectPhaseCreateComponent } from './project-phase/project-phase-create/project-phase-create.component';
import { ProjectPhaseManageComponent } from './project-phase/project-phase-manage/project-phase-manage.component';
import { SurveyCreateComponent } from './customer-requirement/customer-requirement-create/Survey-create/Survey-create.component';
import { SolutionAnalysisProductCreateComponent } from '../solution/customer-requirement/solution-analysis-product-create/solution-analysis-product-create.component';
import { SolutionAnalysisSupplierCreateComponent } from './customer-requirement/solution-analysis-supplier-create/solution-analysis-supplier-create.component';
import { ProductNeedSolutionCreateModalComponent } from './customer-requirement/product-need-solution-create-modal/product-need-solution-create-modal.component';
import { ProductNeedPriceCreateModalComponent } from './customer-requirement/product-need-price-create-modal/product-need-price-create-modal.component';
import { SurveyMaterialCreateComponent } from './customer-requirement/survey-material-create/survey-material-create.component';
import { SurveyContentCreateComponent } from './customer-requirement/survey-content/survey-content-create/survey-content-create.component';
import { SurveyContentManageComponent } from './customer-requirement/survey-content/survey-content-manage/survey-content-manage.component';
import { ProductCreateComponent } from './customer-requirement/customer-requirement-create/product-create/product-create.component';
import { AddContactCustomerMeetingComponent } from './meeting/add-contact-customer-meeting/add-contact-customer-meeting.component';
import { SupplierCreateComponent } from './customer-requirement/customer-requirement-create/supplier-create/supplier-create.component';
import { MaterialCreateComponent } from './customer-requirement/customer-requirement-create/material-create/material-create.component';
import { MeetingCreateRequirmentModelComponent } from './meeting/meeting-create-requirment-model/meeting-create-requirment-model.component';
import { CustomerRequirequirmentNeedToHandleComponent } from './customer-requirement/customer-requirequirment-need-to-handle/customer-requirequirment-need-to-handle.component';
import { SolutionChooseFileUploadModalComponent } from './solution-choose-file-upload-modal/solution-choose-file-upload-modal.component';
import { EndMeetingComponent } from './meeting/end-meeting/end-meeting.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { SummaryQuotesManageComponent } from './summary-quotes/summary-quotes-manage/summary-quotes-manage.component';
import { SummaryQuotesCreateComponent } from './summary-quotes/summary-quotes-create/summary-quotes-create.component';
import { SummaryQuotesProductComponent } from './summary-quotes/summary-quotes-product/summary-quotes-product.component';
import { SummaryQuotesPlanComponent } from './summary-quotes/summary-quotes-plan/summary-quotes-plan.component';
import { SummaryQuotesUpdateComponent } from './summary-quotes/summary-quotes-update/summary-quotes-update.component';
import { SummaryQuotesProductCreateComponent } from './summary-quotes/summary-quotes-product-create/summary-quotes-product-create.component';
import { ViewDocumentSurveyContentComponent } from './customer-requirement/survey-content/view-document-survey-content/view-document-survey-content.component';
import { AngularSplitModule } from 'angular-split';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { ChartsModule } from 'ng2-charts';
import { ShowChooseQuotationStepComponent } from './summary-quotes/summary-quotes-create/show-choose-quotation-step/show-choose-quotation-step.component';
import { ShowSummaryQuotesPlanCreateComponent } from './summary-quotes/summary-quotes-plan/show-summary-quotes-plan-create/show-summary-quotes-plan-create.component';
import { ShowSummaryQuotesPlanUpdateComponent } from './summary-quotes/summary-quotes-plan/show-summary-quotes-plan-update/show-summary-quotes-plan-update.component';
import { DxFileManagerModule, DxPopupModule } from 'devextreme-angular';

@NgModule({
  declarations: [
    SolutionManageComponent,
    SolutionTabDesignDocumentComponent,
    SolutionOldVersionComponent,
    SolutionTabProductComponent,
    SolutionGroupCreateComponent,
    SolutionCreateComponent,
    SolutionUpdateComponent,
    ChooseProjectSolutionComponent,
    SolutionChooseFolderUploadModalComponent,
    ShowEditContenComponent,
    SolutionProductCreateComponent,
    SurveyCreateComponent,
    CustomerRequirementCreateComponent,
    CustomerRequirementManageComponent,
    MeetingManageComponent,
    MeetingCreateComponent,
    ChooseCustomerContactComponent,
    MeetingTypeCreateComponent,
    MeetingFinishManageComponent,
    MeetingDetailComponent,
    MeetingJoinComponent,
    MeetingJoinFinishComponent,
    ProjectPhaseCreateComponent,
    ProjectPhaseManageComponent,
    SolutionAnalysisProductCreateComponent,
    SolutionAnalysisSupplierCreateComponent,
    ProductNeedSolutionCreateModalComponent,
    ProductNeedPriceCreateModalComponent,
    SurveyMaterialCreateComponent,
    SurveyContentCreateComponent,
    SurveyContentManageComponent,
    ProductCreateComponent,
    AddContactCustomerMeetingComponent,
    SupplierCreateComponent,
    MaterialCreateComponent,
    MeetingCreateRequirmentModelComponent,
    CustomerRequirequirmentNeedToHandleComponent,
    SolutionChooseFileUploadModalComponent,
    EndMeetingComponent,
    SummaryQuotesManageComponent,
    SummaryQuotesCreateComponent,
    SummaryQuotesProductComponent,
    SummaryQuotesPlanComponent,
    SummaryQuotesUpdateComponent,
    SummaryQuotesProductCreateComponent,
    ViewDocumentSurveyContentComponent,
    ShowChooseQuotationStepComponent,
    ShowSummaryQuotesPlanCreateComponent,
    ShowSummaryQuotesPlanUpdateComponent,
  ],
  imports: [
    CommonModule,
    SolutionRoutingModule,
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
    NgSelectModule,
    AngularSplitModule,
    PdfJsViewerModule,
    ChartsModule,
    DxFileManagerModule,
    DxPopupModule,
  ],
  entryComponents: [
    SolutionGroupCreateComponent,
    SolutionCreateComponent,
    SolutionUpdateComponent,
    ChooseProjectSolutionComponent,
    SolutionChooseFolderUploadModalComponent,
    ShowEditContenComponent,
    SolutionProductCreateComponent,
    AddContactCustomerMeetingComponent
  ],
  providers: [
    NgbActiveModal,
  ]
})
export class SolutionModule { }
