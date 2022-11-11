import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { StageService } from '../../services/stage.service';
import { CheckSpecialCharacter } from '../../../shared/common/check-special-characters';

@Component({
  selector: 'app-stage-create',
  templateUrl: './stage-create.component.html',
  styleUrls: ['./stage-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class StageCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: StageService,
    public constant: Constants,
    public combobox: ComboboxService,
  ) { }

  isAction: boolean = false;

  Id: string = '';
  listData: any[] = [];

  model: any = {
    Id: '',
    Name: '',
    Note: '',
    DepartmentId: '',
    Code: '',
    Time: '',
    Color: '#ffffff',
    IsEnable: true,
  }

  modalInfo = {
    Title: 'Thêm mới SBU',
    SaveText: 'Lưu',
  };

  departmentid = null;
  ngOnInit() {
    let userStore = localStorage.getItem('qltkcurrentUser');
    if (userStore) {
      this.departmentid = JSON.parse(userStore).departmentId;
    }

    this.getListDepaterment();
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa công đoạn';
      this.modalInfo.SaveText = 'Lưu';
      this.getStageInfo();
    }
    else {
      this.model.DepartmentId = this.departmentid;
      this.modalInfo.Title = "Thêm mới công đoạn";
    }
  }

  getStageInfo() {
    this.service.getInfoById({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  getListDepaterment() {
    this.combobox.getCbbDepartment().subscribe(data => {
      this.listData = data;
    });
  }


  create(isContinue: boolean) {
    if (this.model.Color == "#ffffff") {
      this.messageService.showMessage('Chưa chọn màu không thể lưu!');
    }
    else {
      this.service.create(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {
              Id: '',
              Name: '',
              Note: '',
              DepartmentId: this.departmentid,
              Code: '',
              Time: '',
              Color: '#ffffff',
              IsEnable: '',
            };
            this.messageService.showSuccess('Thêm mới công đoạn thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới công đoạn thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
  }

  update() {
    this.service.update(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật công đoạn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
