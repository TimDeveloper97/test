import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { ProductCreateComponent } from './product-create/product-create.component';
import { ProductStandardTpaCreateComponent } from './product-standard-tpa/product-standard-tpa-create/product-standard-tpa-create.component';
import { ProductStandardTpaManageComponent } from './product-standard-tpa/product-standard-tpa-manage/product-standard-tpa-manage.component';
import { ProductStandardTPATypeManagerComponent } from './product-standard-tpa/product-standard-tpa-type-manager/product-standard-tpa-type-manager.component';
import { ProductStandardTpaViewComponent } from './product-standard-tpa/product-standard-tpa-view/product-standard-tpa-view.component';
import { ProductComponent } from './product/product.component';
import { ProductGroupManageComponent } from './productgroup/product-group-manage/product-group-manage.component';

const routes: Routes = [
  // { path: 'quan-ly-nhom-thiet-bi', component: ProductGroupManageComponent, canActivate: [AuthGuard] },
  {
    path: 'quan-ly-thiet-bi',
    children: [
      { path: '', component: ProductComponent, canActivate: [AuthGuard] },
      { path: 'them-moi/:GroupId', component: ProductCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: ProductCreateComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'quan-ly-thiet-bi-nhap-khau', 
  children: [
    { path: '', component: ProductStandardTpaManageComponent, canActivate: [AuthGuard] },
    { path: 'them-moi', component: ProductStandardTpaCreateComponent, canActivate: [AuthGuard] },
    { path: 'chinh-sua/:Id', component: ProductStandardTpaCreateComponent, canActivate: [AuthGuard] },
    { path: 'xem/:Id', component: ProductStandardTpaViewComponent, canActivate: [AuthGuard] },
  ] },
  { path: 'chung-loai-hang-hoa', component: ProductStandardTPATypeManagerComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeviceRoutingModule { }
