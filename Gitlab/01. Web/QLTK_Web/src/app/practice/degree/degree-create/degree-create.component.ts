import { Component, OnInit, Input } from '@angular/core';

import { MessageService } from 'src/app/shared/services/message.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { DegreeService } from '../../degree/service/degree.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-degree-create',
  templateUrl: './degree-create.component.html',
  styleUrls: ['./degree-create.component.scss']
})
export class DegreeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private degreeService: DegreeService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }
  @Input() IdDegree;
  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa trình độ';
      this.ModalInfo.SaveText = 'Lưu';
      this.getDegreeInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới trình độ";
    }
  }
  ModalInfo = {
    Title: 'Thêm mới trình độ',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listDegree: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }

  getDegreeInfo() {
    this.degreeService.getDegree({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createDegree(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.addDegree(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.addDegree(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  addDegree(isContinue) {
    this.degreeService.createDegree(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
          };
          this.messageService.showSuccess('Thêm mới trình độ thành công!');
          this.IdDegree  = data;
        }
        else {
          this.messageService.showSuccess('Thêm mới trình độ thành công!');
          this.IdDegree  = data;
          this.closeModal(data);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateDegree();
    }
    else {
      this.createDegree(isContinue);
    }
  }

  saveDegree() {
    this.degreeService.updateDegree(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật trình độ thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateDegree() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveDegree();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveDegree();
        },
        error => {
          
        }
      );
    }
  }
}
