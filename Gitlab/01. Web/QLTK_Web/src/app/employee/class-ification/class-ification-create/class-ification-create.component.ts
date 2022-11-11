import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ClassIficationService } from '../../service/class-ification.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-class-ification-create',
  templateUrl: './class-ification-create.component.html',
  styleUrls: ['./class-ification-create.component.scss']
})
export class ClassIficationCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private ificationService: ClassIficationService,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService


  ) { }

  ngOnInit() {
    this.getCbbClassIfication();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa xếp loại';
      this.ModalInfo.SaveText = 'Lưu';
      this.getIficationInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới xếp loại";
    }
  }
  ModalInfo = {
    Title: 'Thêm mới xếp loại',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listIfication: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Index: 0,
  }

  getIficationInfo() {
    this.ificationService.GetInforClassIfication(this.Id).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createIfication(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.addIfication(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.addIfication(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  addIfication(isContinue) {
    this.ificationService.createClassIfication(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
          };
          this.messageService.showSuccess('Thêm mới xếp loại thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới xếp loại thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateIfication();
    }
    else {
      this.createIfication(isContinue);
    }
  }

  saveIfication() {
    this.ificationService.updateClassIfication(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật xếp loại thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateIfication() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveIfication();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveIfication();
        },
        error => {
          
        }
      );
    }
  }

  listClassIfication: any[] = []

  getCbbClassIfication() {
    this.combobox.getCbbClassIfication().subscribe((data: any) => {
      this.listClassIfication = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listClassIfication.splice(this.listClassIfication.length - 1, 1);
      }

    });
  }

}
