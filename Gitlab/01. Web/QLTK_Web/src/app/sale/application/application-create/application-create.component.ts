import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ApplicationService } from '../application.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-application-create',
  templateUrl: './application-create.component.html',
  styleUrls: ['./application-create.component.scss']
})
export class ApplicationCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private applicationService: ApplicationService,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService
  ) { }

  ngOnInit() {
    this.getCbbApplication();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa ứng dụng';
      this.ModalInfo.SaveText = 'Lưu';
      this.getApplicationInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới ứng dụng";
    }
  }
  ModalInfo = {
    Title: 'Thêm mới ứng dụng',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listApplication: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Note: '',
    Index: 0,
  }

  getApplicationInfo() {
    this.applicationService.GetInforApplication(this.Id).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createApplication(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validName = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validName) {
      this.addApplication(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa kí tự đặc biệt!").then(
        data => {
          this.addApplication(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  addApplication(isContinue) {
    this.applicationService.createApplication(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
         this.getCbbApplication() 
          this.model = {
            Id: '',
            Name: '',
            Note:'',
            Index:0
          };
          this.messageService.showSuccess('Thêm mới ứng dụng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới ứng dụng thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateApplication();
    }
    else {
      this.createApplication(isContinue);
    }
  }

  saveApplication() {
    this.applicationService.updateApplication(this.model).subscribe(
      data => {
      
          this.messageService.showSuccess('Cập nhật ứng dụng thành công!');
          this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      });
  }

  updateApplication() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveApplication();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa kí tự đặc biệt!").then(
        data => {
          this.saveApplication();
        },
        error => {
          
        }
      );
    }
  }

  getCbbApplication() {
    this.combobox.getCBBApplication().subscribe((data: any) => {
      this.listApplication = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listApplication.splice(this.listApplication.length - 1, 1);
      }

    });
  }
}
