import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { SalaryTypeService } from '../../service/salary-type.service';

@Component({
  selector: 'app-salary-type-create',
  templateUrl: './salary-type-create.component.html',
  styleUrls: ['./salary-type-create.component.scss']
})
export class SalaryTypeCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private salaryTypeService: SalaryTypeService,
    private combobox: ComboboxService,
    private constant: Constants) { }

  modalInfo = {
    Title: 'Thêm mới ngạch lương',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  salaryTypeModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Salary: ''
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa ngạch lương';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới ngạch lương';
    }
  }

  getInfo() {
    this.salaryTypeService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.salaryTypeModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.salaryTypeService.update(this.salaryTypeModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật ngạch lương thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.salaryTypeService.create(this.salaryTypeModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới ngạch lương thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới ngạch lương thành công!');
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
    this.salaryTypeModel = {
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
