import { NgModule, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExtraOptions, Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { AuthGuard } from '../auth/guards/auth.guard';
import { ScreenSaverComponent } from '../shared/component/screen-saver/screen-saver.component';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('../auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'tivi',
    loadChildren: () => import('../tivi/tivi.module').then(m => m.TiviModule)
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'qltk',
        pathMatch: 'full'
      },
      {
        path: 'qltk',
        canActivate: [AuthGuard],
        component: ScreenSaverComponent
      },
      {
        path: 'vat-tu',
        loadChildren: () => import('../material/material.module').then(m => m.MaterialModule)
      },
      {
        path: 'module',
        loadChildren: () => import('../module/module-source.module').then(m => m.ModuleSourceModule)
      },
      {
        path: 'thiet-bi',
        loadChildren: () => import('../device/device.module').then(m => m.DeviceModule)
      },
      {
        path: 'nhan-vien',
        loadChildren: () => import('../employee/employee.module').then(m => m.EmployeeModule)
      },
      {
        path: 'thuc-hanh',
        loadChildren: () => import('../practice/practice.module').then(m => m.PracticeModule)
      },
      {
        path: 'phong-hoc',
        loadChildren: () => import('../education/education.module').then(m => m.EducationModule)
      },
      {
        path: 'du-an',
        loadChildren: () => import('../project/project.module').then(m => m.ProjectModule)
      },
      {
        path: 'giai-phap',
        loadChildren: () => import('../solution/solution.module').then(m => m.SolutionModule)
      },
      {
        path: 'tools',
        loadChildren: () => import('../tool/tool.module').then(m => m.ToolModule)
      },
      {
        path: 'bao-cao',
        loadChildren: () => import('../report/report.module').then(m => m.ReportModule)
      },
      {
        path: 'kinh-doanh',
        canActivate: [AuthGuard],
        loadChildren: () => import('../sale/sale.module').then(m => m.SaleModule)
      },
      {
        path: 'nhap-khau',
        canActivate: [AuthGuard],
        loadChildren: () => import('../import/import.module').then(m => m.ImportModule)
      },
      {
        path: 'tuyen-dung',
        canActivate: [AuthGuard],
        loadChildren: () => import('../recruitment/recruitment.module').then(m => m.RecruitmentModule)
      },
      {
        path: 'tai-lieu',
        canActivate: [AuthGuard],
        loadChildren: () => import('../document/document.module').then(m => m.DocumentModule)
      },
      {
        path: 'huong-dan',
        canActivate: [AuthGuard],
        loadChildren: () => import('../guideline/guideline.module').then(m => m.GuidelineModule)
      },
      {
        path: 'danh-muc',
        canActivate: [AuthGuard],
        loadChildren: () => import('../category/category.module').then(m => m.CategoryModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class LayoutRoutingModule { }