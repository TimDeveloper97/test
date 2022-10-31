import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { CodeRuleManageComponent } from './code-rule/code-rule-manage/code-rule-manage.component';
import { ConfigMaterialComponent } from './config-material/config-material.component';
import { ManufacturerManageComponent } from './manufacturer/manufacturer-manage/manufacturer-manage.component';
import { MaterialCreateComponent } from './material/material-create/material-create.component';
import { MaterialManageComponent } from './material/material-manage/material-manage.component';
import { MaterialUpdateComponent } from './material/material-update/material-update.component';
import { MaterialgroupManageComponent } from './materialgroup/materialgroup-manage/materialgroup-manage.component';
import { MaterialgrouptpaManageComponent } from './materialgrouptpa/materialgrouptpa-manage/materialgrouptpa-manage.component';
import { NsMaterialGroupCreateComponent } from './ns-material-group/ns-material-group-create/ns-material-group-create.component';
import { NsMaterialGroupManageComponent } from './ns-material-group/ns-material-group-manage/ns-material-group-manage.component';
import { NsMaterialGroupUpdateComponent } from './ns-material-group/ns-material-group-update/ns-material-group-update.component';
import { RawmaterialManageComponent } from './rawmaterial/rawmaterial-manage/rawmaterial-manage.component';
import { SimilarMaterialConfigManageComponent } from './similar-material-config/similar-material-config-manage/similar-material-config-manage.component';
import { SupplierManageComponent } from './supplier/supplier-manage/supplier-manage.component';
import { TestSearchNewComponent } from './test-search/test-search-new/test-search-new.component';
import { UnitManagerComponent } from './unit/unit-manager/unit-manager.component';

const routes: Routes = [
  {
    path: 'quan-ly-vat-tu',
    children: [
      { path: '', component: MaterialManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: MaterialCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: MaterialUpdateComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'nhom-vat-tu-chuc-nang', component: MaterialgroupManageComponent, canActivate: [AuthGuard] },
  { path: 'nhom-vat-tu-tpa', component: MaterialgrouptpaManageComponent, canActivate: [AuthGuard] },
  { path: 'cau-hinh-thong-so-vat-tu', component: ConfigMaterialComponent, canActivate: [AuthGuard] },
  {
    path: 'nhom-vat-tu-phi-tieu-chuan',
    children: [
      { path: '', component: NsMaterialGroupManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi', component: NsMaterialGroupCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: NsMaterialGroupUpdateComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'hang-san-xuat', component: ManufacturerManageComponent, canActivate: [AuthGuard] },
  { path: 'don-vi', component: UnitManagerComponent, canActivate: [AuthGuard] },
  { path: 'vat-lieu', component: RawmaterialManageComponent, canActivate: [AuthGuard] }, 
  { path: 'nha-cung-cap', component: SupplierManageComponent, canActivate: [AuthGuard] },
  { path: 'vat-tu-tuong-tu', component: SimilarMaterialConfigManageComponent, canActivate: [AuthGuard] },
  { path: 'cau-hinh-ma-vat-tu', component: CodeRuleManageComponent, canActivate: [AuthGuard] },
  { path: 'test-search', component: TestSearchNewComponent, canActivate: [AuthGuard] },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MaterialRoutingModule { }
