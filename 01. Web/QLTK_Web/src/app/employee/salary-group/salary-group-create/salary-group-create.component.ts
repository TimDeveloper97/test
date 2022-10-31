import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { SalaryGroupService } from '../../service/salary-group.service';

@Component({
  selector: 'app-salary-group-create',
  templateUrl: './salary-group-create.component.html',
  styleUrls: ['./salary-group-create.component.scss']
})
export class SalaryGroupCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private salaryGroupService: SalaryGroupService,
    private combobox: ComboboxService,
    private constant: Constants) { }

  modalInfo = {
    Title: 'Thêm mới nhóm lương',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  salaryGroupModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Salary: ''
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm lương';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới nhóm lương';
    }
  }

  getInfo() {
    this.salaryGroupService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.salaryGroupModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.salaryGroupService.update(this.salaryGroupModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm lương thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.salaryGroupService.create(this.salaryGroupModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới nhóm lương thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm lương thành công!');
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
    this.salaryGroupModel = {
      Id: '',
      Name: '',
      Code: '',
      Note: ''
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
