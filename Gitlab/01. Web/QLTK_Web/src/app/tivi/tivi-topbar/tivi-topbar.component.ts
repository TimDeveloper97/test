import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Configuration, AppSetting, } from '../../shared';
import { ChangePasswordComponent } from './../../auth/change-password/change-password.component';

declare var $: any;

@Component({
  selector: 'app-tivi-topbar',
  templateUrl: './tivi-topbar.component.html',
  styleUrls: ['./tivi-topbar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TiviTopbarComponent implements OnInit {

  fullName: string;
  // location: Location;
  constructor(public appSetting: AppSetting,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    private router: Router,
    public config: Configuration,
    private location: Location
  ) {
    // this.location = location;
  }
  percent = 100;
  ngOnInit() {

  }

  routerLink() {
    this.router.navigate(['/main']);
  }

  zoomIn() {
    this.percent += 10;
    this.changeValueCss();
  }

  zoomOut() {
    this.percent -= 10;
    this.changeValueCss();
  }

  changeValueCss() {
    document.documentElement.style.setProperty('--tivi-zoom', (this.percent / 100).toString());
  }

  backUrl() {
    this.location.back();
  }
}
