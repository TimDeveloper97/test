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

import { DeviceRoutingModule } from './device-routing.module';

import { UserUpdateComponent } from './user/user-update/user-update.component';
import { ProductmaterialsComponent } from './productmaterials/productmaterials.component';
import { ProductGroupManageComponent } from './productgroup/product-group-manage/product-group-manage.component';
import { ProductGroupCreateComponent } from './productgroup/product-group-create/product-group-create.component';
import { ProductAccessoriesTabComponent } from './aroductaccessories/product-accessories-tab/product-accessories-tab.component';
import { ProductMaterialsTabComponent } from './product-materials-tab/product-materials-tab.component';
import { ProductMaterialsChooseTabComponent } from './product-materials-choose-tab/product-materials-choose-tab.component';
import { ProductaccessorieschooseComponent } from './productaccessorieschoose/productaccessorieschoose.component';
import { ProductComponent } from './product/product.component';
import { ProductCreateComponent } from './product-create/product-create.component';
import { ProductChooseModuleComponent } from './product-choose-module/product-choose-module.component';
import { ProductModuleTabComponent } from './product-module-tab/product-module-tab.component';
import { ProductSketchesComponent } from './product-sketches/product-sketches.component';
import { PopupPracticeCreateComponent } from './popup-practice-create/popup-practice-create.component';
import { ProductTabDesignDocumentComponent } from './product-tab-design-document/product-tab-design-document.component';
import { ProductChooseFolderUploadModalComponent } from './product-choose-folder-upload-modal/product-choose-folder-upload-modal.component';
import { ShowProductModuleUpdateComponent } from './show-product-module-update/show-product-module-update.component';
import { ProductErrorTabComponent } from './product-error-tab/product-error-tab.component';
import { ProductTabResultTestComponent } from './product-tab-result-test/product-tab-result-test.component';
import { ProductTabDocumentAttachComponent } from './product-tab-document-attach/product-tab-document-attach.component';
import { ProductStandardTpaManageComponent } from './product-standard-tpa/product-standard-tpa-manage/product-standard-tpa-manage.component';
import { ProductStandardTpaCreateComponent } from './product-standard-tpa/product-standard-tpa-create/product-standard-tpa-create.component';
import { ProductStandardTpaViewComponent } from './product-standard-tpa/product-standard-tpa-view/product-standard-tpa-view.component';
import { ProductStandardTpaFileComponent } from './product-standard-tpa/product-standard-tpa-file/product-standard-tpa-file.component';
import { ProductUpdateContentComponent } from './product-update-content/product-update-content.component';
import { ProductStandardTpaFileViewComponent } from './product-standard-tpa/product-standard-tpa-file-view/product-standard-tpa-file-view.component';
import { ProductStandardTPATypeManagerComponent } from './product-standard-tpa/product-standard-tpa-type-manager/product-standard-tpa-type-manager.component';
import { ProductStandardTPATypeCreateComponent } from './product-standard-tpa/product-standard-tpa-type-create/product-standard-tpa-type-create.component';
import { ProductNeedPublicationsComponent } from './product-need-publications/product-need-publications.component';

@NgModule({
  declarations: [
    UserUpdateComponent,
    ProductGroupManageComponent,
    ProductAccessoriesTabComponent,
    ProductMaterialsTabComponent,
    ProductComponent,
    ProductCreateComponent,
    ProductModuleTabComponent,
    ProductSketchesComponent,
    ProductTabDesignDocumentComponent,
    ProductErrorTabComponent,
    ProductTabResultTestComponent,
    ProductTabDocumentAttachComponent,
    ProductStandardTpaManageComponent,
    ProductStandardTpaFileComponent,
    ProductStandardTpaFileViewComponent,
    ProductStandardTPATypeManagerComponent,
    ProductGroupCreateComponent,
    ProductaccessorieschooseComponent,
    ProductMaterialsChooseTabComponent,
    ProductChooseModuleComponent,
    PopupPracticeCreateComponent,
    ProductChooseFolderUploadModalComponent,
    ShowProductModuleUpdateComponent,
    ProductStandardTpaCreateComponent,
    ProductStandardTpaViewComponent,
    ProductUpdateContentComponent,
    ProductStandardTPATypeCreateComponent,
    ProductmaterialsComponent,
    ProductNeedPublicationsComponent
  ],
  imports: [
    CommonModule,
    DeviceRoutingModule,
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
  ],
  entryComponents: [
    ProductGroupCreateComponent,
    ProductaccessorieschooseComponent,
    ProductMaterialsChooseTabComponent,
    ProductChooseModuleComponent,
    PopupPracticeCreateComponent,
    ProductChooseFolderUploadModalComponent,
    ShowProductModuleUpdateComponent,
    ProductStandardTpaCreateComponent,
    ProductStandardTpaViewComponent,
    ProductUpdateContentComponent,
    ProductStandardTPATypeCreateComponent
  ],
  providers:[
    NgbActiveModal 
  ]
})
export class DeviceModule { }
