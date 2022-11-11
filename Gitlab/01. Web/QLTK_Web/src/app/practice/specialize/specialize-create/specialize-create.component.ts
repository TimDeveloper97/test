import { Component, OnInit } from '@angular/core';


import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { SpecializeService } from '../service/specialize.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-specialize-create',
  templateUrl: './specialize-create.component.html',
  styleUrls: ['./specialize-create.component.scss']
})
export class SpecializeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private specializeservice: SpecializeService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa chuyên môn';
      this.ModalInfo.SaveText = 'Lưu';
      this.getSpecialize();
    }
    else {
      this.ModalInfo.Title = "Thêm mới chuyên môn";
    }
  }

  ModalInfo = {
    Title: 'Thêm mới chuyên môn',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listSpecialize: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }
 
  getSpecialize() {
    this.specializeservice.getSpecialize({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createSpecialize(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.addSpecialize(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.addSpecialize(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  addSpecialize(isContinue){
    this.specializeservice.createSpecialize(this.model).subscribe(
      data => {
        if(isContinue){
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '', 
          };
          this.messageService.showSuccess('Thêm mới chuyên môn thành công!');
        }
        else{
          this.messageService.showSuccess('Thêm mới chuyên môn thành công!');
          this.closeModal(data);
        }
      },
      error =>{
        this.messageService.showError(error);
      });
  }

   save(isContinue: boolean) {
    if (this.Id) {
      this.updateSpecialize();
    }
    else {
      this.createSpecialize(isContinue);
    }
  }
  
  saveSpecialize(){
    this.specializeservice.updateSpecialize(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật chuyên môn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateSpecialize() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveSpecialize();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveSpecialize();
        },
        error => {
          
        }
      );
    }
  }
  
}
