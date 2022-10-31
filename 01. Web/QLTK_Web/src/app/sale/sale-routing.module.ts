import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../auth/guards/auth.guard';
import { ApplicationManageComponent } from './application/application-manage/application-manage.component';
import { ExportAndKeepCreateComponent } from './export-and-keep/export-and-keep-create/export-and-keep-create.component';
import { ExportAndKeepManageComponent } from './export-and-keep/export-and-keep-manage/export-and-keep-manage.component';
import { ProductCompareSourceManageComponent } from './product-compare-source/product-compare-source-manage/product-compare-source-manage.component';
import { ProductForBusinessCreateComponent } from './product-for-business/product-for-business-create/product-for-business-create.component';
import { ProductBusinessDetailsComponent } from './product-for-business/product-business-details/product-business-details.component';
import { ProductForBussinessComponent } from './product-for-business/product-for-bussiness-manage/product-for-bussiness.component';
import { SaleGroupManageComponent } from './sale-group/sale-group-manage/sale-group-manage.component';
import { ExportAndKeepShowComponent } from './export-and-keep/export-and-keep-show/export-and-keep-show.component';
import { ExportAndKeepHistoryComponent } from './export-and-keep/export-and-keep-history/export-and-keep-history.component';
import { SaleTargetmentComponent } from './sale-targetment/sale-targetment.component';

export const routes: Routes = [
  {
    path: 'nhom-kinh-doanh',
    
    component: SaleGroupManageComponent,
  },
  {
    path: 'ung-dung',
    component: ApplicationManageComponent,
  },
  {
    path: 'dang-ky-doanh-so',
    component: SaleTargetmentComponent,
  },
  { 
    path: 'danh-sach-xuat-giu', component: ExportAndKeepManageComponent, canActivate: [AuthGuard]
    // children: [
    //   { path: 'them-moi', component: ExportAndKeepCreateComponent},
    // ]
  },
  {
    path: 'danh-sach-xuat-giu',
    canActivate: [AuthGuard],
    children: [
      { path: 'them-moi', component: ExportAndKeepCreateComponent , canActivate: [AuthGuard] },
      { path: 'them-moi/:idProduct', component: ExportAndKeepCreateComponent , canActivate: [AuthGuard] },
      { path: 'chinh-sua/:id', component: ExportAndKeepCreateComponent, canActivate: [AuthGuard] },
      { path: 'xem/:id', component: ExportAndKeepShowComponent, canActivate: [AuthGuard] },
    ]
  },
  { 
    path: 'them-moi-xuat-giu', component: ExportAndKeepCreateComponent, canActivate: [AuthGuard]
  },
  { 
    path: 'lich-su-xuat-giu', component: ExportAndKeepHistoryComponent, canActivate: [AuthGuard]
  },
  {
    path: 'lich-su-xuat-giu',
    canActivate: [AuthGuard],
    children: [
      { path: 'xem/:id', component: ExportAndKeepShowComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'san-pham-cho-kinh-doanh', component: ProductForBussinessComponent, canActivate: [AuthGuard] },
  {
    path: 'san-pham-cho-kinh-doanh',
    canActivate: [AuthGuard],
    children: [
      { path: 'them-moi', component: ProductForBusinessCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua/:Id', component: ProductForBusinessCreateComponent, canActivate: [AuthGuard] },
      { path: 'chi-tiet/:Id', component: ProductBusinessDetailsComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'so-sanh-thu-vien-san-pham-voi-nguon', component: ProductCompareSourceManageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SaleRoutingModule { }
