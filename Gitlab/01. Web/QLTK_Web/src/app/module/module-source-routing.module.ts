import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';

import { FunctionManageComponent } from './function/function-manage/function-manage.component';
import { FunctionGroupsManageComponent } from './functiongroups/function-groups-manage/function-groups-manage.component';
import { IndustryManageComponent } from './industry/industry-manage/industry-manage.component';
import { ModuleCreateComponent } from './Module/module-create/module-create.component';
import { ModuleManageComponent } from './Module/module-manage/module-manage.component';
import { ModuleProjectComponent } from './Module/module-project/module-project.component';
import { ModuleUpdateComponent } from './Module/module-update/module-update.component';
import { ModulegroupManageComponent } from './modulegroup/modulegroup-manage/modulegroup-manage.component';
import { ProductStandardGroupManageComponent } from './ProductStandardGroup/product-standard-group-manage/product-standard-group-manage.component';
import { ProductStandardManageComponent } from './ProductStandards/product-standard-manage/product-standard-manage.component';
import { ProductStandardsCreateComponent } from './ProductStandards/product-standards-create/product-standards-create.component';
import { ProductStandardsUpdateComponent } from './ProductStandards/product-standards-update/product-standards-update.component';
import { StageManageComponent } from './stage/stage-manage/stage-manage.component';
import { TestCriteriaManageComponent } from './testcriteria/test-criteria-manage/test-criteria-manage.component';
import { TestCriteriaGroupManageComponent } from './testcriteriagroup/test-criteria-group-manage/test-criteria-group-manage.component';

const routes: Routes = [
  // { path: 'quan-ly-module', component: ModuleManageComponent, canActivate: [AuthGuard] },
  {
    path: 'quan-ly-module',
    children: [
      { path: '', component: ModuleManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi-module', component: ModuleCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua-module/:Id', component: ModuleUpdateComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'quan-ly-tieu-chi-kiem-tra', component: TestCriteriaManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-tieu-chuan-san-pham', component: ProductStandardManageComponent, canActivate: [AuthGuard] },
  {
    path: 'quan-ly-tieu-chuan-san-pham',
    children: [
      { path: '', component: ProductStandardManageComponent, canActivate: [AuthGuard] },
      { path: 'them-moi-tieu-chuan-san-pham', component: ProductStandardsCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua-tieu-chuan-san-pham/:Id', component: ProductStandardsUpdateComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'quan-ly-tinh-nang', component: FunctionManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-nganh', component: IndustryManageComponent, canActivate: [AuthGuard] },
  { path: 'module-project', component: ModuleProjectComponent, canActivate: [AuthGuard] },
  { path: 'cong-doan-san-xuat', component: StageManageComponent, canActivate: [AuthGuard] },
  // { path: 'quan-ly-nhom-module', component: ModulegroupManageComponent, canActivate: [AuthGuard] },
  // { path: 'quan-ly-nhom-tieu-chi-kiem-tra', component: TestCriteriaGroupManageComponent, canActivate: [AuthGuard] },
  // { path: 'quan-ly-nhom-tieu-chuan-san-pham', component: ProductStandardGroupManageComponent, canActivate: [AuthGuard] },

  // { path: 'quan-ly-nhom-tinh-nang', component: FunctionGroupsManageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ModuleSourceRoutingModule { }
