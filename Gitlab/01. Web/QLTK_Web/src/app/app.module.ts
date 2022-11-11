import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AngularSplitModule } from 'angular-split';
import { BlockUIModule } from 'ng-block-ui';
import { CurrencyMaskConfig, CURRENCY_MASK_CONFIG } from 'ng2-currency-mask';
import { ToastrModule } from 'ngx-toastr';
import { loadCldr, L10n } from '@syncfusion/ej2-base';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtInterceptor } from './auth/helpers';
import { Configuration } from './shared';
import { SharedModule } from './shared/shared.module';
import { GanttModule } from '@syncfusion/ej2-angular-gantt'; 
import {
  SelectionService,
  VirtualScrollService
} from '@syncfusion/ej2-angular-gantt';


declare var require: any;

loadCldr(
  require("cldr-data/main/vi/numbers.json"),
  require("cldr-data/main/vi/ca-gregorian.json"),
  require("cldr-data/supplemental/numberingSystems.json"),
  require("cldr-data/main/vi/timeZoneNames.json"),
  require('cldr-data/supplemental/weekdata.json') 
);

export function initializeApp(appConfig: Configuration) {
  return () => appConfig.load();
}

export const CustomCurrencyMaskConfig: CurrencyMaskConfig = {
  align: "left",
  allowNegative: true,
  decimal: ",",
  precision: 0,
  prefix: "",
  suffix: "",
  thousands: "."
};

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BlockUIModule.forRoot({
      delayStart: 0,
    }),
    ToastrModule.forRoot({
      timeOut: 2000,
      closeButton: true
    }),
    AngularSplitModule,
    FormsModule,
    NgbModule,
    SharedModule,
    BrowserAnimationsModule,
    GanttModule,
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [Configuration],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    {
      provide: CURRENCY_MASK_CONFIG,
      useValue: CustomCurrencyMaskConfig
    },
    SelectionService,
    VirtualScrollService
  ],

  bootstrap: [AppComponent]
})
export class AppModule { }
