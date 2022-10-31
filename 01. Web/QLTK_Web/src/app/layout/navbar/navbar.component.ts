import { Component, OnInit, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';


import { NtsNavigationService } from '../navigation/navigation.service';
import { navigation } from '../navigation/navigation';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit, OnDestroy {

    navigation: any;

    // Private
    private _unsubscribeAll: Subject<any>;

    constructor(
        private _ntsNavigationService: NtsNavigationService,
        private router: Router
    ) {
        // Set the private defaults
        this._unsubscribeAll = new Subject();

    }

    ngOnInit() {

        this._ntsNavigationService.register('menu', navigation);
        this._ntsNavigationService.setCurrentNavigation('menu');

        if (this._ntsNavigationService.isCurrentNavigation()) {
            this.navigation = this._ntsNavigationService.getCurrentNavigation();
        } 

        this._ntsNavigationService.onNavigationChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(() => {
                this.navigation = this._ntsNavigationService.getCurrentNavigation();
            });
    }

    /**
      * On destroy
      */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }
}
