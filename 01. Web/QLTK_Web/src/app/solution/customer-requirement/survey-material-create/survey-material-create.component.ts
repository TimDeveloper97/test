import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { SurveyMaterialService } from '../service/survey-material.service'
@Component({
  selector: 'app-survey-material-create',
  templateUrl: './survey-material-create.component.html',
  styleUrls: ['./survey-material-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SurveyMaterialCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private surveyMaterialService: SurveyMaterialService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới dụng cụ khảo sát',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  SurveyId: string;
  ListMaterial: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Quantity: '',

  }

  ngOnInit() {
    this.model.survayId = this.SurveyId;
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa dụng cụ khảo sát';
      this.ModalInfo.SaveText = 'Lưu';
      this.getSurveyMaterial();
    }
    else {
      this.ModalInfo.Title = "Thêm mới dụng cụ khảo sát";
    }
  }

  getSurveyMaterial() {
    this.surveyMaterialService.getSurveyMaterial({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    if(isOK){
    this.activeModal.close( this.ListMaterial);
    }
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createSurveyMaterial(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.addSurveyMaterial(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.addSurveyMaterial(isContinue);
        },
        error => {

        }
      );
    }
  }

  addSurveyMaterial(isContinue) {
    this.surveyMaterialService.createSurveyMaterial(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Note: '',
            SurveyId: '',
            Quantity: '',
          };
          this.model.survayId = this.SurveyId;
          this.messageService.showSuccess('Thêm mới thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới thành công!');
          this.closeModal(data);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    // if (this.Id) {
    //   this.updateSurveyMaterial();
    // }
    // else {
    //   this.createSurveyMaterial(isContinue);
    // }
    this.create(isContinue);
  }

  saveSurveyMaterial() {
    this.surveyMaterialService.updateSurveyMaterial(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateSurveyMaterial() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveSurveyMaterial();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveSurveyMaterial();
        },
        error => {

        }
      );
    }
  }

  create(isContinue) {
    this.ListMaterial.push(this.model);
    if (isContinue) {
      this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
      this.model = {
        Id: '',
        Name: '',
        Code: '',
        Note: '',
        SurveyId: '',
        Quantity: '',
      }
    }
    else {
      this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
      this.closeModal(true);
    }
  }

}
