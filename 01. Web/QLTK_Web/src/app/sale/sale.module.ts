import { APP_INITIALIZER, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SaleRoutingModule } from './sale-routing.module';
import { SharedModule } from '../shared/shared.module';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { BrowserModule } from '@angular/platform-browser';
import {
  DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
  DxDateBoxModule,
  DxDataGridModule
} from 'devextreme-angular';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SaleGroupManageComponent } from './sale-group/sale-group-manage/sale-group-manage.component';
import { SaleGroupCreateComponent } from './sale-group/sale-group-create/sale-group-create.component';
import { ApplicationManageComponent } from './application/application-manage/application-manage.component';
import { ApplicationCreateComponent } from './application/application-create/application-create.component';
import { ShowChooseProductComponent } from './sale-group/show-choose-product/show-choose-product.component';
import { ShowChooseEmployeeComponent } from './sale-group/show-choose-employee/show-choose-employee.component';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { ExportAndKeepManageComponent } from './export-and-keep/export-and-keep-manage/export-and-keep-manage.component';
import { ExportAndKeepCreateComponent } from './export-and-keep/export-and-keep-create/export-and-keep-create.component';
import { ExportAndKeepDetailComponent } from './export-and-keep/export-and-keep-detail/export-and-keep-detail.component';
import { ProductForBussinessComponent } from './product-for-business/product-for-bussiness-manage/product-for-bussiness.component';
import { ProductForBusinessCreateComponent } from './product-for-business/product-for-business-create/product-for-business-create.component';
import { ImageGalleryManageComponent } from '../sale/product-for-business/image-gallery-manage/image-gallery-manage.component';
import { DocumentManageComponent } from '../sale/product-for-business/document-manage/document-manage.component';
import { AccessoryManageComponent } from '../sale/product-for-business/accessory-manage/accessory-manage.component';
import { ChooseAccessoryComponent } from '../sale/product-for-business/choose-accessory/choose-accessory.component';
import { AccessProductViewComponent } from '../sale/product-for-business/accessory-product-view/access-product-view.component';
import { ProductBusinessDetailsComponent } from '../sale/product-for-business/product-business-details/product-business-details.component';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
// import { NgDragDropModule } from 'ng-drag-drop';
// import { DragDropModule } from '@angular/cdk/drag-drop';
import { ProductCompareSourceManageComponent } from './product-compare-source/product-compare-source-manage/product-compare-source-manage.component';
import { ProductCompareSorceProductDetailComponent } from './product-compare-source/product-compare-sorce-product-detail/product-compare-sorce-product-detail.component';
import { AppCarreeTabComponent } from './product-for-business/app-carree-tab/app-carree-tab.component';
import { ShowChooseApplicationtComponent } from './product-for-business/show-choose-applicationt/show-choose-applicationt.component';
import { ShowChooseCareeComponent } from './product-for-business/show-choose-caree/show-choose-caree.component';
import { ProductCompareSourcDeviceDetailComponent } from './product-compare-source/product-compare-sourc-device-detail/product-compare-sourc-device-detail.component';
import { UploadImageVideoComponent } from './product-for-business/upload-image-video/upload-image-video.component';
import { ProductCompareSourceMaterialDetailComponent } from './product-compare-source/product-compare-source-material-detail/product-compare-source-material-detail.component';
import { ProductCompareSourcModuleDetailComponent } from './product-compare-source/product-compare-sourc-module-detail/product-compare-sourc-module-detail.component';
import { SaleCustomerCreateComponent } from './export-and-keep/customer-create/customer-create.component';
import { AppCareeDetailComponent } from './product-for-business/product-view-detail/app-caree-detail/app-caree-detail.component';
import { ImageGalleryDetailComponent } from './product-for-business/product-view-detail/image-gallery-detail/image-gallery-detail.component';
import { DocumentDetailComponent } from './product-for-business/product-view-detail/document-detail/document-detail.component';
import { AccessoryViewDeatilComponent } from './product-for-business/product-view-detail/accessory-view-deatil/accessory-view-deatil.component';
import { ExportAndKeepShowComponent } from './export-and-keep/export-and-keep-show/export-and-keep-show.component';
import { GroupSaleProductComponent } from './product-for-business/group-sale-product/group-sale-product.component';
import { ChooseGroupProductSaleComponent } from './product-for-business/choose-group-product-sale/choose-group-product-sale.component';
import { ExportAndKeepHistoryComponent } from './export-and-keep/export-and-keep-history/export-and-keep-history.component';
import { SaleProductTypeCreateComponent } from './product-for-business/sale-product-type-create/sale-product-type-create.component';
import { SaleTargetmentComponent } from './sale-targetment/sale-targetment.component';
import { CreateUpdateSaleTartgetmentComponent } from './create-update-sale-tartgetment/create-update-sale-tartgetment.component';


@NgModule({

  declarations: [
    SaleGroupManageComponent,
    SaleGroupCreateComponent,
    ShowChooseEmployeeComponent,
    ShowChooseProductComponent,
    ApplicationManageComponent,
    ApplicationCreateComponent,
    ProductForBussinessComponent,
    ProductForBusinessCreateComponent,
    ProductBusinessDetailsComponent,
    ImageGalleryManageComponent,
    DocumentManageComponent,
    AccessoryManageComponent,
    ChooseAccessoryComponent,
    AccessProductViewComponent,
    ExportAndKeepManageComponent,
    ExportAndKeepCreateComponent,
    ExportAndKeepDetailComponent,
    ProductCompareSourceManageComponent,
    ProductCompareSorceProductDetailComponent,
    AppCarreeTabComponent,
    ShowChooseApplicationtComponent,
    ShowChooseCareeComponent,
    ProductCompareSourcDeviceDetailComponent,
    ProductCompareSourceMaterialDetailComponent,
    ProductCompareSourcModuleDetailComponent,
    UploadImageVideoComponent,
    SaleCustomerCreateComponent,
    AppCareeDetailComponent,
    ImageGalleryDetailComponent,
    DocumentDetailComponent,
    AccessoryViewDeatilComponent,
    ExportAndKeepShowComponent,
    GroupSaleProductComponent,
    ChooseGroupProductSaleComponent,
    ExportAndKeepShowComponent,
    ExportAndKeepHistoryComponent,
    SaleProductTypeCreateComponent,
    SaleTargetmentComponent,
    CreateUpdateSaleTartgetmentComponent
  ],
  imports: [
    CommonModule,
    SaleRoutingModule,
    SharedModule,
    PerfectScrollbarModule,
    NgbModule,
    FormsModule,
    VirtualScrollerModule,

    DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
    DxDateBoxModule,
    DxDataGridModule,
    CurrencyMaskModule,
    NgxGalleryModule,
    DxTreeListModule,
    DxTreeViewModule,
  ],
  entryComponents: [
    SaleGroupCreateComponent,
    ApplicationCreateComponent,
    ShowChooseEmployeeComponent,
    ShowChooseProductComponent,
    ExportAndKeepCreateComponent,
    ExportAndKeepDetailComponent,
    ChooseAccessoryComponent,
    AccessProductViewComponent,
    ProductCompareSorceProductDetailComponent,
    ShowChooseApplicationtComponent,
    ShowChooseCareeComponent,
    UploadImageVideoComponent,
    SaleCustomerCreateComponent,
    SaleProductTypeCreateComponent
  ],
  providers:[
    NgbActiveModal 
  ]
})
export class SaleModule { }
