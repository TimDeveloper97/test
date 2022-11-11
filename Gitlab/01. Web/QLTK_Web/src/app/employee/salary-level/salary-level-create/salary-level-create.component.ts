import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';
import { SalaryLevelService } from '../../service/salary-level.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-salary-level-create',
  templateUrl: './salary-level-create.component.html',
  styleUrls: ['./salary-level-create.component.scss']
})
export class SalaryLevelCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private salaryLevelService: SalaryLevelService,
    private comboboxService: ComboboxService,
    private constant: Constants) { }

  modalInfo = {
    Title: 'Thêm mới mức lương',
    SaveText: 'Lưu',
  };
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];

  isAction: boolean = false;
  id: string;

  salaryLevelModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Salary: '',
    SalaryGroupId: null,
    SalaryTypeId: null
  };
  salaryGroups: any[] = [];
  salaryTypes: any[] = [];

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa mức lương';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới mức lương';
    }

    this.getCBBData();
  }

  getCBBData() {
    let cbbSalaryGroup = this.comboboxService.getCbbSalaryGroups();
    let cbbSalaryType = this.comboboxService.getCbbSalarytypes();

    forkJoin([ cbbSalaryGroup, cbbSalaryType]).subscribe(results => {    
      this.salaryGroups = results[0];
      this.salaryTypes = results[1];     
    });
  }

  getInfo() {
    this.salaryLevelService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.salaryLevelModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.salaryLevelService.update(this.salaryLevelModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật mức lương thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.salaryLevelService.create(this.salaryLevelModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới mức lương thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới mức lương thành công!');
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
    this.salaryLevelModel = {
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
