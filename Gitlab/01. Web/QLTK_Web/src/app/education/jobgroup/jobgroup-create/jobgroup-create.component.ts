import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';

import { JobGroupService } from '../../../education/service/job-group.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-jobgroup-create',
  templateUrl: './jobgroup-create.component.html',
  styleUrls: ['./jobgroup-create.component.scss']
})
export class JobgroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private jobGroupService: JobGroupService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }
  ModalInfo = {
    Title: 'Thêm mới nhóm nghề',
    SaveText: 'Lưu',

  };
  isAction: boolean = false;
  Id: string;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }
  ngOnInit() {
    if(this.Id){
      this.ModalInfo.Title='Chỉnh sửa nhóm nghề';
      this.ModalInfo.SaveText = 'Lưu';
      this.getJobGroup();
    }
    else
    {
      this.ModalInfo.Title = 'Thêm mới nhóm nghề';
    }
  }

  getJobGroup(){
    this.jobGroupService.GetJobGroup({Id: this.Id}).subscribe(data=> {
      this.model = data;
    });
  }
  createJobGroup(isContinue){
    var  validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.CreateBy  = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.jobGroupService.AddJobGroup(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.messageService.showSuccess('Thêm mới nhóm nghề thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới nhóm nghề thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.jobGroupService.AddJobGroup(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới nhóm nghề thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới nhóm nghề thành công!');
                this.closeModal(true);
              }
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {
          
        }
      );
    }
  }
  updateJobGroup(){
    var  validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.jobGroupService.UpdateJobGroup(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm nghề thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.jobGroupService.UpdateJobGroup(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật nhóm nghề thành công!');
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {
          
        }
      );
    }
  }
  save(isContinue: boolean) {
    if (this.Id) {
      this.updateJobGroup();
    }
    else {
      this.createJobGroup(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
