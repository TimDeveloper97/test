import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbModule, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller'; import {
  DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
  DxDateBoxModule,
  DxDataGridModule
} from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';

import { SharedModule } from '../shared/shared.module';

import { MaterialRoutingModule } from './material-routing.module';

import { ManufacturerManageComponent } from './manufacturer/manufacturer-manage/manufacturer-manage.component';
import { ManufacturerCreateComponent } from './manufacturer/manufacturer-create/manufacturer-create.component';

import { UnitManagerComponent } from './unit/unit-manager/unit-manager.component';
import { UnitCreateComponent } from './unit/unit-create/unit-create.component';
import { RawmaterialManageComponent } from './rawmaterial/rawmaterial-manage/rawmaterial-manage.component';
import { RawmaterialCreateComponent } from './rawmaterial/rawmaterial-create/rawmaterial-create.component';
import { ConfigMaterialComponent } from './config-material/config-material.component';
import { MaterialgroupManageComponent } from './materialgroup/materialgroup-manage/materialgroup-manage.component';

import { MaterialgroupCreateComponent } from './materialgroup/materialgroup-create/materialgroup-create.component';
import { MaterialManageComponent } from './material/material-manage/material-manage.component';

import { NsMaterialGroupManageComponent } from './ns-material-group/ns-material-group-manage/ns-material-group-manage.component';
import { NsMaterialGroupCreateComponent } from './ns-material-group/ns-material-group-create/ns-material-group-create.component';
import { MaterialCreateComponent } from './material/material-create/material-create.component';
import { NsMaterialGroupUpdateComponent } from './ns-material-group/ns-material-group-update/ns-material-group-update.component';
import { CreateNsmaterialCodeModalComponent } from './ns-material-group/create-nsmaterial-code-modal/create-nsmaterial-code-modal.component';
import { MaterialUpdateComponent } from './material/material-update/material-update.component';
import { ConfirmOverwriteModalComponent } from './material/confirm-overwrite-modal/confirm-overwrite-modal.component';
import { SupplierManageComponent } from './supplier/supplier-manage/supplier-manage.component';
import { SupplierCreateComponent } from './supplier/supplier-create/supplier-create.component';
import { MaterialBuyHistoryModalComponent } from './material/material-buy-history-modal/material-buy-history-modal.component';
import { MaterialgrouptpaManageComponent } from './materialgrouptpa/materialgrouptpa-manage/materialgrouptpa-manage.component';
import { MaterialgrouptpaCreateComponent } from './materialgrouptpa/materialgrouptpa-create/materialgrouptpa-create.component';
import { ImportMaterialBuyHistoryModalComponent } from './material/import-material-buy-history-modal/import-material-buy-history-modal.component';
import { SimilarMaterialConfigManageComponent } from './similar-material-config/similar-material-config-manage/similar-material-config-manage.component';
import { SimilarMaterialCreateComponent } from './similar-material/similar-material-create/similar-material-create.component';
import { ChooseMaterialComponent } from './similar-material-config/choose-material/choose-material.component';
import { ShowMaterialComponent } from './similar-material-config/show-material/show-material.component';
import { ImportFileManafacturerComponent } from './manufacturer/import-file-manafacturer/import-file-manafacturer.component';
import { ConverUnitComponent } from './material/conver-unit/conver-unit.component';
import { ImportMaterialComponent } from './material/import-material/import-material.component';
import { ImportSupplierComponent } from './supplier/import-supplier/import-supplier.component';
import { SimilarMaterialConfigComponent } from './material/similar-material-config/similar-material-config.component';
import { SearchSimilarMaterialComponent } from './similar-material-config/search-similar-material/search-similar-material.component';
import { ManufactureGroupCreateComponent } from './manufacture-group/manufacture-group-create/manufacture-group-create.component';
import { SupplierGroupCreateComponent } from './supplier-group/supplier-group-create/supplier-group-create.component';
import { ChooseMaterialGroupModalComponent } from './code-rule/choose-material-group-modal/choose-material-group-modal.component';
import { CodeRuleManageComponent } from './code-rule/code-rule-manage/code-rule-manage.component';
import { TestSearchNewComponent } from './test-search/test-search-new/test-search-new.component';
import { ChooseManufactureComponent } from './supplier/choose-manufacture/choose-manufacture.component';
import { SupplierContractCreateComponent } from './supplier/supplier-contract-create/supplier-contract-create.component';


@NgModule({
  declarations: [
    ManufacturerManageComponent,
    ManufacturerCreateComponent,
    UnitManagerComponent,
    UnitCreateComponent,
    RawmaterialManageComponent,
    RawmaterialCreateComponent,
    ConfigMaterialComponent,
    MaterialgroupManageComponent,
    MaterialgroupCreateComponent,
    MaterialManageComponent,
    NsMaterialGroupManageComponent,
    NsMaterialGroupCreateComponent,
    MaterialCreateComponent,
    NsMaterialGroupUpdateComponent,
    CreateNsmaterialCodeModalComponent,
    MaterialUpdateComponent,
    ConfirmOverwriteModalComponent,
    SupplierManageComponent,
    SupplierCreateComponent,
    MaterialBuyHistoryModalComponent,
    MaterialgrouptpaManageComponent,
    ImportMaterialBuyHistoryModalComponent,
    SimilarMaterialConfigManageComponent,
    SimilarMaterialCreateComponent,
    ChooseMaterialComponent,
    ShowMaterialComponent,
    ImportFileManafacturerComponent,
    ConverUnitComponent,
    ImportMaterialComponent,
    ImportSupplierComponent,
    SimilarMaterialConfigComponent,
    SearchSimilarMaterialComponent,
    ManufactureGroupCreateComponent,
    SupplierGroupCreateComponent,
    ChooseMaterialGroupModalComponent,
    CodeRuleManageComponent,
    TestSearchNewComponent,
    ChooseManufactureComponent,
    MaterialgrouptpaCreateComponent,
    SupplierContractCreateComponent,
  ],
  imports: [
    CommonModule,
    MaterialRoutingModule,
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
    ManufacturerCreateComponent,
    UnitCreateComponent,
    RawmaterialCreateComponent,
    CreateNsmaterialCodeModalComponent,
    ConfirmOverwriteModalComponent,
    SupplierCreateComponent,
    MaterialBuyHistoryModalComponent,
    MaterialgrouptpaCreateComponent,
    MaterialgroupCreateComponent,
    ImportMaterialBuyHistoryModalComponent,
    SimilarMaterialCreateComponent,
    ChooseMaterialComponent,
    ShowMaterialComponent,
    ImportFileManafacturerComponent,
    ImportMaterialComponent,
    ImportSupplierComponent,
    SearchSimilarMaterialComponent,
    ManufactureGroupCreateComponent,
    SupplierGroupCreateComponent,
    ChooseMaterialGroupModalComponent,
    ChooseManufactureComponent,
  ],
  providers:[
    NgbActiveModal
  ]
})
export class MaterialModule { }
