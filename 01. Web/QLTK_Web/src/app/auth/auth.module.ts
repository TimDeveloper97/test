import { ChangePasswordComponent } from './change-password/change-password.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthRoutingModule } from './auth-routing.routing';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { JwtInterceptor } from './helpers';
import { BlockUIModule } from 'ng-block-ui';

@NgModule({
    declarations: [
        LoginComponent,
        ChangePasswordComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        AuthRoutingModule,
        BlockUIModule
    ],
    providers: [
        AuthGuard,
    ],
    entryComponents: [
        ChangePasswordComponent
    ],
})

export class AuthModule {
}