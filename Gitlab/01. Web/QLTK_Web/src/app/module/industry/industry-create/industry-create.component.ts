import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { IndustryService } from '../../services/industry.service';

@Component({
  selector: 'app-industry-create',
  templateUrl: './industry-create.component.html',
  styleUrls: ['./industry-create.component.scss']
})
export class IndustryCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private industryService : IndustryService
  ) { }

  ModalInfo = {
    Title: 'Thêm mới ngành hàng',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }
  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa ngành hàng';
      this.ModalInfo.SaveText = 'Lưu';
      this.getIndustry();
    }
    else {
      this.ModalInfo.Title = "Thêm mới ngành hàng";
    }
  }

  getIndustry() {
    this.industryService.getIndustry({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createIndustry(isContinue) {
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if(validCode){
      this.industryService.createIndustry(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {
              Id: '',
              Name: '',
              Code: '',
              Description: '',
            };
            this.messageService.showSuccess('Thêm mới ngành hàng thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới ngành hàng thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        });
    }
    else
    {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.industryService.createIndustry(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {
                  Id: '',
                  Name: '',
                  Code: '',
                  Description: '',
                };
                this.messageService.showSuccess('Thêm mới ngành hàng thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới ngành hàng thành công!');
                this.closeModal(true);
              }
            },
            error => {
              this.messageService.showError(error);
            });
        },
        error => {
          
        }
      );
    }
    
  }

  updateIndustry() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.industryService.updateIndustry(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật ngành hàng thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.industryService.updateIndustry(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật ngành hàng thành công!');
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
      this.updateIndustry();
    }
    else {
      this.createIndustry(isContinue);
    }
  }

}
