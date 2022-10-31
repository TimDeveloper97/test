import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DragDropModule } from '@angular/cdk/drag-drop';

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

import { ToolRoutingModule } from './tool-routing.module';

import { DesignStructureComponent } from './design-structure/design-structure.component';
import { DesignStructureManagerComponent } from './design-structure-manager/design-structure-manager.component';
import { CreateDesignStructureComponent } from './create-design-structure/create-design-structure.component';
import { CreateDesignStructureFileComponent } from './create-design-structure-file/create-design-structure-file.component';
import { FolderDefinitionComponent } from './folder-definition/folder-definition.component';
import { FolderDefinitionCreateComponent } from './folder-definition-create/folder-definition-create.component';
import { TestDesignManagerComponent } from './test-design-manager/test-design-manager.component';
import { TestStandardSuppliesComponent } from './test-standard-supplies/test-standard-supplies.component';
import { TestDesignStructureFolderComponent } from './test-design-structure-folder/test-design-structure-folder.component';
import { ChooseFolderUploadImageComponent } from './choose-folder-upload-image/choose-folder-upload-image.component';
import { TestDesignPopupReportComponent } from './test-design-popup-report/test-design-popup-report.component';
import { TestDesignCadComponent } from './test-design-cad/test-design-cad.component';
import { TestSoftHardCadComponent } from './test-soft-hard-cad/test-soft-hard-cad.component';
import { TestFileDmvtComponent } from './test-file-dmvt/test-file-dmvt.component';
import { ChooseFolderFileComponent } from './choose-folder-file/choose-folder-file.component';
import { CheckElectronicComponent } from './check-electronic/check-electronic.component';
import { CheckFileElectricComponent } from './check-file-electric/check-file-electric.component';
import { TestFolderMatComponent } from './test-folder-mat/test-folder-mat.component';
import { GeneralTemplateComponent } from './general-template/general-template.component';
import { GeneralDesignComponent } from './general-design/general-design.component';
import { DataDistributionComponent } from './data-distribution/data-distribution.component';
import { DataDistributionCreateFolderComponent } from './data-distribution-create-folder/data-distribution-create-folder.component';
import { DataDistributionFileChooseComponent } from './data-distribution-file-choose/data-distribution-file-choose.component';
import { DataDistributionFileCreateUpdateComponent } from './data-distribution-file-create-update/data-distribution-file-create-update.component';
import { DownloadListModuleComponent } from './download-list-module/download-list-module.component';
import { ScanFileComponent } from './scan-file/scan-file.component';
import { GeneralMaterialSapComponent } from './general-material-sap/general-material-sap.component';
import { ConfigScanFileComponent } from './config-scan-file/config-scan-file.component';
import { ChooseConfigScanFileComponent } from './choose-config-scan-file/choose-config-scan-file.component';
import { RenameFileComponent } from './rename-file/rename-file.component';
import { DataDistributionFileManageModalComponent } from './data-distribution-file-manage-modal/data-distribution-file-manage-modal.component';
import { ChooseMaterialForTemplateComponent } from './choose-material-for-template/choose-material-for-template.component';
import { TestFolderIgsComponent } from './test-folder-igs/test-folder-igs.component';
import { TestMaterialPriceComponent } from './test-material-price/test-material-price/test-material-price.component';
import { ConfigManageComponent } from './config/config-manage/config-manage.component';
import { ConfigCreateComponent } from './config/config-create/config-create.component';
import { ClassRoomToolComponent } from './class-room-tool/class-room-tool.component';
import { ChooseProductToolComponent } from './choose-product-tool/choose-product-tool.component';
@NgModule({
  declarations: [
    DesignStructureComponent,
    FolderDefinitionComponent,
    TestDesignManagerComponent,
    TestStandardSuppliesComponent,
    TestDesignStructureFolderComponent,
    ChooseFolderUploadImageComponent,
    TestDesignCadComponent,
    TestSoftHardCadComponent,
    TestFileDmvtComponent,
    CheckElectronicComponent,
    CheckFileElectricComponent,
    TestFolderMatComponent,
    GeneralDesignComponent,
    DataDistributionComponent,
    DownloadListModuleComponent,
    ScanFileComponent,
    GeneralMaterialSapComponent,
    TestFolderIgsComponent,
    TestMaterialPriceComponent,
    ConfigManageComponent,
    ClassRoomToolComponent,
    DesignStructureManagerComponent,
    CreateDesignStructureComponent,
    CreateDesignStructureFileComponent,
    GeneralTemplateComponent,
    FolderDefinitionCreateComponent,
    TestDesignPopupReportComponent,
    ChooseFolderFileComponent,
    DataDistributionCreateFolderComponent,
    DataDistributionFileChooseComponent,
    DataDistributionFileCreateUpdateComponent,
    ConfigScanFileComponent,
    ChooseConfigScanFileComponent,
    RenameFileComponent,
    DataDistributionFileManageModalComponent,
    ChooseMaterialForTemplateComponent,
    ConfigCreateComponent,
    ChooseProductToolComponent,
  ],
  imports: [
    CommonModule,
    ToolRoutingModule,
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
    DesignStructureManagerComponent,
    CreateDesignStructureComponent,
    CreateDesignStructureFileComponent,
    GeneralTemplateComponent,
    FolderDefinitionCreateComponent,
    TestDesignPopupReportComponent,
    ChooseFolderFileComponent,
    DataDistributionCreateFolderComponent,
    DataDistributionFileChooseComponent,
    DataDistributionFileCreateUpdateComponent,
    ConfigScanFileComponent,
    ChooseConfigScanFileComponent,
    RenameFileComponent,
    DataDistributionFileManageModalComponent,
    ChooseMaterialForTemplateComponent,
    ConfigCreateComponent,
    ChooseProductToolComponent,
  ],
  providers: [
    NgbActiveModal
  ]
})
export class ToolModule { }
