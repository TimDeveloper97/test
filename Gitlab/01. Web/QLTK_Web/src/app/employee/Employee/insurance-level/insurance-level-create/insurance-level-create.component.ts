import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { InsuranceLevelService } from 'src/app/employee/service/insurance-level.service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-insurance-level-create',
  templateUrl: './insurance-level-create.component.html',
  styleUrls: ['./insurance-level-create.component.scss']
})
export class InsuranceLevelCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private insuranceLevelService: InsuranceLevelService,) { }

  modalInfo = {
    Title: 'Thêm mới mức đóng bảo hiểm',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  insuranceLevelModel: any = {
    Id: '',
    Name: '',
    Note: '',
    Money: 0
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa mức đóng bảo hiểm';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới mức đóng bảo hiểm';
    }
  }

  getInfo() {
    this.insuranceLevelService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.insuranceLevelModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.insuranceLevelService.update(this.insuranceLevelModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật mức đóng bảo hiểm thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.insuranceLevelService.create(this.insuranceLevelModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới mức đóng bảo hiểm thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới mức đóng bảo hiểm thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.insuranceLevelModel = {
      Id: '',
      Name: '',
      Note: '',
      Money: 0
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
