import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { WorkPlaceService } from '../service/work-place.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-work-place-create',
  templateUrl: './work-place-create.component.html',
  styleUrls: ['./work-place-create.component.scss']
})
export class WorkPlaceCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private workPlaceservice: WorkPlaceService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới đơn vị công tác',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listWorkPlace: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa đơn vị công tác';
      this.ModalInfo.SaveText = 'Lưu';
      this.getWorkPlace();
    }
    else {
      this.ModalInfo.Title = "Thêm mới đơn vị công tác";
    }
  }

  getWorkPlace() {
    this.workPlaceservice.getWorkPlace({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }
  
  createWorkPlace(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.addWorkPlace(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.addWorkPlace(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  addWorkPlace(isContinue){
    this.workPlaceservice.createWorkPlace(this.model).subscribe(
      data => {
        if(isContinue){
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '', 
          };
          this.messageService.showSuccess('Thêm mới đơn vị công tác thành công!');
        }
        else{
          this.messageService.showSuccess('Thêm mới đơn vị công tác thành công!');
          this.closeModal(data);
        }
      },
      error =>{
        this.messageService.showError(error);
      });
  }

   save(isContinue: boolean) {
    if (this.Id) {
      this.updateWorkPlace();
    }
    else {
      this.createWorkPlace(isContinue);
    }
  }
  
  saveWorkPlace(){
    this.workPlaceservice.updateWorkPlace(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật đơn vị công tác thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateWorkPlace() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveWorkPlace();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveWorkPlace();
        },
        error => {
          
        }
      );
    }
  }

}
