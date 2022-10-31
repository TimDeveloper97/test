import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { RoomtypeService } from '../services/roomtype.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-roomtype-create',
  templateUrl: './roomtype-create.component.html',
  styleUrls: ['./roomtype-create.component.scss']
})

export class RoomtypeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private roomtypeservice: RoomtypeService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa loại phòng học';
      this.ModalInfo.SaveText = 'Lưu';
      this.getRoomtypeInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới loại phòng học";
    }
  }

  ModalInfo = {//m
    Title: 'Thêm mới loại phòng học',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listRoomtype: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }

  getRoomtypeInfo() {
    this.roomtypeservice.getRoomType({ Id: this.Id }).subscribe(data => {
      this.model = data; // thêm error
    });
  }

  createRoomType(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.addRoomType(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.addRoomType(isContinue);
        },
        error => {
          
        }
      );
    }
  }
  
  addRoomType(isContinue){
    this.roomtypeservice.createRoomType(this.model).subscribe(
      data => {
        if(isContinue){
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '', 
          };
          this.messageService.showSuccess('Thêm mới loại phòng học thành công!');
        }
        else{
          this.messageService.showSuccess('Thêm mới loại phòng học thành công!');
          this.closeModal(true);
        }
      },
      error =>{
        this.messageService.showError(error);
      });
  }

  saveRoomType(){
    this.roomtypeservice.updateRoomType(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật loại phòng học thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateRoomType() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveRoomType();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveRoomType();
        }
      );
    }
  }
  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateRoomType();
    }
    else {
      this.createRoomType(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }
}
