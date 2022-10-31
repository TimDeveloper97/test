import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services';
import { MessageService } from '../../shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  private message: string;
  returnUrl: string
  OldPassword: any;
  ConfirmOldPassword: any;
  model: any = {
    Id: JSON.parse(localStorage.getItem('qltkcurrentUser')).userid,
    OldPassword: '',
    NewPassword: '',
    ConfirmPassword: '',

  }
  constructor(private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
  ) { }

  ngOnInit() {

  }
  
  closeModal(isOK: boolean) {
    this.activeModal.close(true);
  }

  ConfirmChangePassword() {
    this.messageService.showConfirm("Bạn có chắc muốn thay đổi mật khẩu không?").then(
      data => {
        this.ChangePassword();
      },
      error => {
        
      }
    );
  }

  ChangePassword() {
    this.model.Id = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.authenticationService.ChangePassword(this.model).subscribe(
      data => {
        if (data) {
          this.closeModal(true);
          //this.notificationsService.success('Thông báo', 'Thay đổi mật khẩu thành công');
          this.authenticationService.logout();
          this.router.navigate(['auth/dang-nhap']);
        }
        else {
          //this.notificationsService.error('Thông báo','Mật khẩu cũ không đúng. Vui lòng nhập lại!');
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
