import { Configuration, AppSetting } from '../../shared';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from './../../auth/change-password/change-password.component';
import { NtsNavigationService } from '../navigation/navigation.service';
import { ShowAccountComponent } from '../../employee/show-account/show-account.component'

declare var $: any;

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class TopbarComponent implements OnInit {

  fullName: string;
  constructor(public appSetting: AppSetting,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    private router: Router,
    public config: Configuration,
  ) {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.fullName = currentUser.userfullname;
  }

  linkManual: string;
  linkImage: string;
  ngOnInit() {
    if (JSON.parse(localStorage.getItem('qltkcurrentUser')).imageLink) {

      this.linkImage = this.config.ServerFileApi + JSON.parse(localStorage.getItem('qltkcurrentUser')).imageLink;
    }
  }

  menuChatToggle(type) {
    if (type == 'menu') {
      this.appSetting.MenuFolded = !this.appSetting.MenuFolded;
      this.appSetting.chatFolded = false;
    }
  }

  logout() {
    localStorage.removeItem('qltkcurrentUser');
    this.router.navigate(['auth/dang-nhap']);
  }

  routerLink() {
    this.router.navigate(['/main']);
  }

  fnChangePassword() {
    let activeModal = this.modalService.open(ChangePasswordComponent, { container: 'body' });
    activeModal.result.then((result) => {

    }, (reason) => {

    });
  }

  AccountInformation () {
    let activeModal = this.modalService.open(ShowAccountComponent, { container: 'body', windowClass: 'show-account-model', backdrop: 'static' });
    activeModal.result.then((result) => {

    }, (reason) => {

    });
  }

  showManual() {

  }

  errorImage(event) {
    this.linkImage = '/assets/img/noavatar.gif';
  }
}
