import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Injectable({
    providedIn: 'root'
})
export class JwtInterceptor implements HttpInterceptor {
    private _inProgressCount = 0;
    @BlockUI() blockUI: NgBlockUI;

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (this._inProgressCount <= 0) {
            this.blockUI.start();
        }
        this._inProgressCount++;

        // add authorization header with jwt token if available
        let userStore = localStorage.getItem('qltkcurrentUser');
        if (userStore) {
            let currentUser = JSON.parse(userStore);
            if (currentUser && currentUser.access_token) {
                request = request.clone({
                    setHeaders: {
                        Authorization: 'Bearer ' + currentUser.access_token
                    }
                });
            }
        }
        // if (!request.headers.has('Content-Type')) {
        //     request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });
        // }

        return next.handle(request).pipe(finalize(() => {
            this._inProgressCount--;
            if (this._inProgressCount === 0) {
                this.blockUI.stop();
            }
        }));
    }
}