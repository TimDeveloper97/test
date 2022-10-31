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

import { ModuleSourceRoutingModule } from './module-source-routing.module';

import { ModulegroupManageComponent } from './modulegroup/modulegroup-manage/modulegroup-manage.component';
import { ModuleGroupCreateComponent } from './modulegroup/module-group-create/module-group-create.component';
import { TestCriteriaGroupManageComponent } from './testcriteriagroup/test-criteria-group-manage/test-criteria-group-manage.component';
import { TestCriteriaGroupCreateComponent } from './testcriteriagroup/test-criteria-group-create/test-criteria-group-create.component';
import { ProductStandardGroupManageComponent } from './ProductStandardGroup/product-standard-group-manage/product-standard-group-manage.component';
import { ProductStandardGroupCreateComponent } from './ProductStandardGroup/product-standard-group-create/product-standard-group-create.component';
import { TestCriteriaCreateComponent } from './testcriteria/test-criteria-create/test-criteria-create.component';
import { TestCriteriaManageComponent } from './testcriteria/test-criteria-manage/test-criteria-manage.component';
import { ProductStandardManageComponent } from './ProductStandards/product-standard-manage/product-standard-manage.component';
import { ProductStandardsCreateComponent } from './ProductStandards/product-standards-create/product-standards-create.component';
import { ModuleGroupChooseProductStandardComponent } from './modulegroup/module-group-choose-product-standard/module-group-choose-product-standard.component';
import { ModuleManageComponent } from './Module/module-manage/module-manage.component';
import { ModuleCreateComponent } from './Module/module-create/module-create.component';
import { ModuleTabDesignDocumentComponent } from './Module/module-tab-design-document/module-tab-design-document.component';
import { ProductStandardsUpdateComponent } from './ProductStandards/product-standards-update/product-standards-update.component';
import { ModuleTabCriteriaComponent } from './Module/module-tab-criteria/module-tab-criteria.component';
import { ModuleUpdateComponent } from './Module/module-update/module-update.component';
import { ModuleVersionTabComponent } from './Module/module-version-tab/module-version-tab.component';
import { ModuleChooseTestCriteiaComponent } from './Module/module-choose-test-criteia/module-choose-test-criteia.component';
import { ModuleProductstandardTabComponent } from './Module/module-productstandard-tab/module-productstandard-tab.component';
import { ModuleProjectComponent } from './Module/module-project/module-project.component';

import { ModuleChooseDesignerTabComponent } from './Module/module-choose-designer-tab/module-choose-designer-tab.component';
import { ModuleDesignerTabComponent } from './Module/module-designer-tab/module-designer-tab.component';

import { ModuleChooseFileDesignDocumentComponent } from './Module/module-choose-file-design-document/module-choose-file-design-document.component';
import { ModuleErrorTabComponent } from './Module/module-error-tab/module-error-tab.component';
import { ModuleMaterialTabComponent } from './Module/module-material-tab/module-material-tab.component';
import { ShowErrorComponent } from './Module/show-error/show-error.component';
import { ModuleProjectTestCriteiaComponent } from './Module/module-project-test-criteia/module-project-test-criteia.component';
import { SketchesComponent } from './Module/sketches/sketches.component';
import { SketchesChooseFunctionsComponent } from './Module/sketches-choose-functions/sketches-choose-functions.component';
import { FunctionGroupsManageComponent } from './functiongroups/function-groups-manage/function-groups-manage.component';
import { FunctionGroupsCreateComponent } from './functiongroups/function-groups-create/function-groups-create.component';
import { SketchesChooseMaterialComponent } from './Module/sketches-choose-material/sketches-choose-material.component';
import { FunctionCreateComponent } from './function/function-create/function-create.component';
import { FunctionManageComponent } from './function/function-manage/function-manage.component';
import { SketchesHistoryComponent } from './Module/sketches-history/sketches-history.component';
import { SketchesImportMaterialComponent } from './Module/sketches-import-material/sketches-import-material.component';
import { ChooseFolderModalComponent } from './choose-folder-modal/choose-folder-modal.component';
import { ModuleShowSimilarMaterialComponent } from './Module/module-show-similar-material/module-show-similar-material.component';
import { ChooseFolderUploadModalComponent } from './choose-folder-upload-modal/choose-folder-upload-modal.component';
import { ModuleChooseFolderDownloadComponent } from './Module/module-choose-folder-download/module-choose-folder-download.component';
import { ModuleMaterialSetupTabComponent } from './Module/module-material-setup-tab/module-material-setup-tab.component';
import { IndustryManageComponent } from './industry/industry-manage/industry-manage.component';
import { IndustryCreateComponent } from './industry/industry-create/industry-create.component';
import { ShowImageErrorComponent } from './Module/show-image-error/show-image-error.component';
import { ListPlanDesginComponent } from './Module/list-plan-desgin/list-plan-desgin.component';
import { ShowChooseProductModuleUpdateComponent } from './Module/show-choose-product-module-update/show-choose-product-module-update.component';
import { ModuleGroupStageTabComponent } from './modulegroup/module-group-stage-tab/module-group-stage-tab.component';
import { ModuleGroupChooseStageComponent } from './modulegroup/module-group-choose-stage/module-group-choose-stage.component';
import { ModuleGroupTestCriteiaComponent } from './modulegroup/module-group-test-criteia/module-group-test-criteia.component';
import { ModuleUpdateContentComponent } from './Module/module-update-content/module-update-content.component';
import { StageManageComponent } from './stage/stage-manage/stage-manage.component';
import { StageCreateComponent } from './stage/stage-create/stage-create.component';
import { ShowDocumentComponent } from './Module/show-document/show-document.component';
import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
  declarations: [
    ModulegroupManageComponent,
    TestCriteriaGroupManageComponent,
    ProductStandardGroupManageComponent,
    TestCriteriaManageComponent,
    ProductStandardManageComponent,
    ModuleManageComponent,
    ModuleCreateComponent,
    ModuleTabDesignDocumentComponent,
    ModuleTabCriteriaComponent,
    ModuleUpdateComponent,
    ModuleVersionTabComponent,
    ModuleProductstandardTabComponent,
    ModuleProjectComponent,
    ModuleDesignerTabComponent,
    ModuleErrorTabComponent,
    ModuleMaterialTabComponent,
    SketchesComponent,
    FunctionGroupsManageComponent,
    FunctionManageComponent,
    ModuleMaterialSetupTabComponent,
    IndustryManageComponent,
    ModuleGroupStageTabComponent,
    ModuleGroupTestCriteiaComponent,
    StageManageComponent,
    StageCreateComponent,
    ModuleGroupCreateComponent,
    TestCriteriaGroupCreateComponent,
    TestCriteriaCreateComponent,
    ProductStandardGroupCreateComponent,
    ProductStandardsCreateComponent,
    ProductStandardsUpdateComponent,
    ModuleGroupChooseProductStandardComponent,
    ModuleChooseTestCriteiaComponent,
    ModuleChooseDesignerTabComponent,
    ModuleChooseFileDesignDocumentComponent,
    ShowErrorComponent,
    ModuleProjectTestCriteiaComponent,
    SketchesChooseFunctionsComponent,
    FunctionGroupsCreateComponent,
    SketchesChooseMaterialComponent,
    FunctionCreateComponent,
    SketchesHistoryComponent,
    SketchesImportMaterialComponent,
    ChooseFolderModalComponent,
    ModuleShowSimilarMaterialComponent,
    ChooseFolderUploadModalComponent,
    ModuleChooseFolderDownloadComponent,
    IndustryCreateComponent,
    ShowImageErrorComponent,
    ListPlanDesginComponent,
    ShowChooseProductModuleUpdateComponent,
    ModuleGroupChooseStageComponent,
    ModuleUpdateContentComponent,
    ShowDocumentComponent,
  ],
  imports: [
    CommonModule,
    ModuleSourceRoutingModule,
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
    DragDropModule
  ],
  entryComponents: [
    ModuleGroupCreateComponent,
    TestCriteriaGroupCreateComponent,
    TestCriteriaCreateComponent,
    ProductStandardGroupCreateComponent,
    ProductStandardsCreateComponent,
    ProductStandardsUpdateComponent,
    ModuleGroupChooseProductStandardComponent,
    ModuleChooseTestCriteiaComponent,
    ModuleChooseDesignerTabComponent,
    ModuleChooseFileDesignDocumentComponent,
    ShowErrorComponent,
    ModuleProjectTestCriteiaComponent,
    SketchesChooseFunctionsComponent,
    FunctionGroupsCreateComponent,
    SketchesChooseMaterialComponent,
    FunctionCreateComponent,
    SketchesHistoryComponent,
    SketchesImportMaterialComponent,
    ChooseFolderModalComponent,
    ModuleShowSimilarMaterialComponent,
    ChooseFolderUploadModalComponent,
    ModuleChooseFolderDownloadComponent,
    IndustryCreateComponent,
    ShowImageErrorComponent,
    ListPlanDesginComponent,
    ShowChooseProductModuleUpdateComponent,
    ModuleGroupChooseStageComponent,
    ModuleUpdateContentComponent,
    StageCreateComponent
  ],
  providers:[
    NgbActiveModal 
  ]
})
export class ModuleSourceModule { }
