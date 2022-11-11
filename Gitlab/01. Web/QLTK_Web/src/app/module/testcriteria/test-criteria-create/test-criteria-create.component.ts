import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService, Configuration, Constants } from 'src/app/shared';

import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { TestcriteriaService } from '../../services/testcriteria';
@Component({
  selector: 'app-test-criteria-create',
  templateUrl: './test-criteria-create.component.html',
  styleUrls: ['./test-criteria-create.component.scss']
})
export class TestCriteriaCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private testCriteriaService: TestcriteriaService,
    private comboboxService: ComboboxService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public constants: Constants
  ) { }

  ModalInfo = {
    Title: 'Thêm mới tiêu chí',
    SaveText: 'Lưu',
  };
  manufactureIds = [];
  isAction: boolean = false;
  Id: string;
  listCRI: any[] = [];
  moduleTestCRIId: string;
  model: any = {
    Id: '',
    Code: '',
    Name: '',
    TestCriteriaGroupName: '',
    TechnicalRequirements: '',
    Note: '',
    DataType: 1
  }
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  ngOnInit() {
    this.model.TestCriteriaGroupId = this.moduleTestCRIId;
    this.getCbbCriter();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa tiêu chí';
      this.ModalInfo.SaveText = 'Lưu';
      this.getTestCriteral();
    }
    else {
      this.ModalInfo.Title = 'Thêm mới tiêu chí';
    }
  }

  getTestCriteral() {
    this.testCriteriaService.GetTestCriteriaInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  getCbbCriter() {
    this.comboboxService.getCbbCriter().subscribe((data: any) => {
      this.listCRI = data;
    });
  }

  createTestCriteral(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.testCriteriaService.AddTestCriteria(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.messageService.showSuccess('Thêm mới tiêu chí thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới tiêu chí thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.testCriteriaService.AddTestCriteria(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới nhóm tiêu chí thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới nhóm tiêu chí thành công!');
                this.closeModal(true);
              }
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

  updateTestCriteria() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.testCriteriaService.UpdateTestCriteria(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm tiêu chí thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.testCriteriaService.UpdateTestCriteria(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật nhóm tiêu chí thành công!');
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
      this.updateTestCriteria();
    }
    else {
      this.createTestCriteral(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
