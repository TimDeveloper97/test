import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';

import { TestcriteriagroupService } from '../../services/testcriteriagroup-service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
@Component({
  selector: 'app-test-criteria-group-create',
  templateUrl: './test-criteria-group-create.component.html',
  styleUrls: ['./test-criteria-group-create.component.scss']
})
export class TestCriteriaGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private testCriteriaGroupService: TestcriteriagroupService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }
  ModalInfo = {
    Title: 'Thêm mới nhóm tiêu chí',
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
    Note: '',
  }
  ngOnInit() {

    if(this.Id){
      this.ModalInfo.Title='Chỉnh sửa nhóm tiêu chí';
      this.ModalInfo.SaveText = 'Lưu';
      this.getTestCriteralGroup();
    }
    else
    {
      this.ModalInfo.Title = 'Thêm mới nhóm tiêu chí';
    }
  }
  getTestCriteralGroup(){
    this.testCriteriaGroupService.getTestCriteralGroup({Id: this.Id}).subscribe(data=> {
      this.model = data;
    },error => {
      this.messageService.showError(error);
    });
  }
  createTestCriteralGroup(isContinue){
    var  validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.CreateBy  = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.testCriteriaGroupService.createTestCriteralGroup(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.messageService.showSuccess('Thêm mới nhóm tiêu chí thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới nhóm tiêu chí thành công!');
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
          this.testCriteriaGroupService.createTestCriteralGroup(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới nhóm tiêu chí thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới nhóm tiêu chí thành công!');
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
  updateTestCriteralGroup(){
    var  validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.testCriteriaGroupService.updateTestCriteralGroup(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm tiêu chí thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.testCriteriaGroupService.updateTestCriteralGroup(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật nhóm tiêu chí thành công!');
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
      this.updateTestCriteralGroup();
    }
    else {
      this.createTestCriteralGroup(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
