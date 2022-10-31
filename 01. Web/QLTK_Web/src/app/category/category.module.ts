import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CategoryRoutingModule } from './category-routing.module';
import { FormsModule } from '@angular/forms';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule, DxDateBoxModule, DxDataGridModule } from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { SharedModule } from '../shared/shared.module';
import { CategoryComponent } from './category/category.component';
import { CategoryCreateComponent } from './category-create/category-create.component';
import { QuoteStepManageComponent } from './quote-step/quote-step-manage/quote-step-manage.component';
import { QuoteStepCreateComponent } from './quote-step/quote-step-create/quote-step-create.component';
import { TechnologySolutionManageComponent } from './technology-solution/technology-solution-manage/technology-solution-manage.component';
import { TechnologySolutionCreateComponent } from './technology-solution/technology-solution-create/technology-solution-create.component';
import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
  declarations: [
    CategoryComponent,
    CategoryCreateComponent,
    QuoteStepManageComponent,
    QuoteStepCreateComponent,
    TechnologySolutionManageComponent,
    TechnologySolutionCreateComponent,
  ],
  imports: [
    CommonModule,
    CategoryRoutingModule,
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
  ]
})
export class CategoryModule { }
