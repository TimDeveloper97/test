import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { ClassRoomToolComponent } from './class-room-tool/class-room-tool.component';
import { ConfigManageComponent } from './config/config-manage/config-manage.component';
import { DataDistributionComponent } from './data-distribution/data-distribution.component';
import { DesignStructureComponent } from './design-structure/design-structure.component';
import { DownloadListModuleComponent } from './download-list-module/download-list-module.component';
import { FolderDefinitionComponent } from './folder-definition/folder-definition.component';
import { GeneralDesignComponent } from './general-design/general-design.component';
import { GeneralMaterialSapComponent } from './general-material-sap/general-material-sap.component';
import { GeneralTemplateComponent } from './general-template/general-template.component';
import { ScanFileComponent } from './scan-file/scan-file.component';
import { TestDesignManagerComponent } from './test-design-manager/test-design-manager.component';
import { TestMaterialPriceComponent } from './test-material-price/test-material-price/test-material-price.component';

const routes: Routes = [
  { path: 'tao-cau-truc-thu-muc', component: DesignStructureComponent, canActivate: [AuthGuard] },
  { path: 'tao-bieu-mau', component: GeneralTemplateComponent, canActivate: [AuthGuard] },
  { path: 'dinh-nghia-thu-muc', component: FolderDefinitionComponent, canActivate: [AuthGuard] },
  { path: 'kiem-tra-cau-truc-thiet-ke', component: TestDesignManagerComponent, canActivate: [AuthGuard] },
  { path: 'tong-hop-thiet-ke', component: GeneralDesignComponent, canActivate: [AuthGuard] },
  { path: 'phan-bo-du-lieu', component: DataDistributionComponent, canActivate: [AuthGuard] },
  { path: 'download-danh-sach-module', component: DownloadListModuleComponent, canActivate: [AuthGuard] },
  { path: 'scan-file', component: ScanFileComponent, canActivate: [AuthGuard] },
  { path: 'download-dmvt-sap', component: GeneralMaterialSapComponent, canActivate: [AuthGuard] },
  { path: 'kiem-tra-gia-vat-tu', component: TestMaterialPriceComponent, canActivate: [AuthGuard] },
  { path: 'cau-hinh', component: ConfigManageComponent, canActivate: [AuthGuard] },
  { path: 'tool-ho-tro', component: ClassRoomToolComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ToolRoutingModule { }
