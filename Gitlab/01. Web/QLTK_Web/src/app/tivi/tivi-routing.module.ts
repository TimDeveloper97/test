import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TiviLayoutComponent } from './tivi-layout/tivi-layout.component';
import { TiviReportErrorDepartmentComponent } from './tivi-report-error-department/tivi-report-error-department.component';
import { TiviReportErrorListComponent } from './tivi-report-error-list/tivi-report-error-list.component';
import { TiviReportErrorComponent } from './tivi-report-error/tivi-report-error.component';
import { TiviListComponent } from './tivi-list/tivi-list.component';
import { TiviReportTKComponent } from './tivi-report-tk/tivi-report-tk.component';

const routes: Routes = [
  {
    path: '',
    component: TiviLayoutComponent,
    children: [
      {
        path: '',
        component: TiviListComponent,
      },
      {
        path: 'bao-cao',
        children: [
          {
            path: 'bao-cao-van-de-ton-dong',
            component: TiviReportErrorComponent,
          },
          {
            path: 'bao-cao-van-de-ton-dong-phong-ban',
            children: [
              {
                path: '',
                component: TiviReportErrorListComponent,
              },
              {
                path: ':id',
                component: TiviReportErrorDepartmentComponent,
              }
            ]
          },
          {
            path: 'bao-cao-phong-thiet-ke',
            component: TiviReportTKComponent
          }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TiviRoutingModule { }
