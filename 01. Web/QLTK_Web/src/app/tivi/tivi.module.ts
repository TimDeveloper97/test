import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller'; import {
  DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
  DxDateBoxModule,
  DxDataGridModule
} from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { ChartsModule } from 'ng2-charts';

import { SharedModule } from '../shared/shared.module';

import { TiviRoutingModule } from './tivi-routing.module';
import { TiviLayoutComponent } from './tivi-layout/tivi-layout.component';
import { TiviTopbarComponent } from './tivi-topbar/tivi-topbar.component';
import { TiviReportErrorComponent } from './tivi-report-error/tivi-report-error.component';
import { TiviReportErrorListComponent } from './tivi-report-error-list/tivi-report-error-list.component';
import { TiviReportErrorDepartmentComponent } from './tivi-report-error-department/tivi-report-error-department.component';
import { TiviListComponent } from './tivi-list/tivi-list.component';
import { TiviReportTKComponent } from './tivi-report-tk/tivi-report-tk.component';

@NgModule({
  declarations: [
    TiviLayoutComponent,
    TiviTopbarComponent,
    TiviReportErrorComponent,
    TiviReportErrorListComponent,
    TiviReportErrorDepartmentComponent,
    TiviListComponent,
    TiviReportTKComponent
  ],
  imports: [
    CommonModule,
    TiviRoutingModule,
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
    ChartsModule
  ]
})
export class TiviModule { }
