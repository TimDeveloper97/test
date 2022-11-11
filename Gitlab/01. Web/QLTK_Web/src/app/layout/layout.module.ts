import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

import { LayoutComponent } from './layout.component';
import { LayoutRoutingModule } from './layout-routing.routing';
import { SharedModule } from '../shared/shared.module';
import { TopbarComponent } from './topbar/topbar.component';
import { LeftbarComponent } from './leftbar/leftbar.component';
import { NavbarComponent } from './navbar/navbar.component';
import { NavCollapsableComponent } from './collapsable/collapsable.component';
import { NavItemComponent } from './item/item.component';
import { NtsNavigationService } from './navigation/navigation.service';
import { NtsInfoComponent } from './nts-info/nts-info.component';

@NgModule({
    declarations: [
        LayoutComponent,
        TopbarComponent,
        LeftbarComponent,
        NavbarComponent,
        NavCollapsableComponent,
        NavItemComponent,
        NtsInfoComponent        
    ],
    imports: [
        CommonModule,
        FormsModule,
        LayoutRoutingModule,
        NgbModule,
        PerfectScrollbarModule,
        SharedModule,
    ],
    providers: [
        NtsNavigationService,
    ],
    entryComponents: [],
})

export class LayoutModule {
}