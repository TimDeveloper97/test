import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { ShowChooseEmployeeComponent } from 'src/app/sale/sale-group/show-choose-employee/show-choose-employee.component';
import { SelectMaterialComponent } from 'src/app/education/classroom/select-material/select-material/select-material.component';
import { SurveyService } from '../../service/Survey.service';
import { CustomerRequirementCreateComponent } from '../customer-requirement-create.component';
import { SurveyMaterialCreateComponent } from '../../survey-material-create/survey-material-create.component'
import { SurveyContentCreateComponent } from '../../survey-content/survey-content-create/survey-content-create.component';


@Component({
  selector: 'app-survey-create.component',
  templateUrl: './survey-create.component.html',
  styleUrls: ['./survey-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SurveyCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    public constant: Constants,
    public surveyService: SurveyService,
    private comboboxService: ComboboxService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới lượt khảo sát',
    SaveText: 'Lưu',
  };
  listRequestId: any[] = [];
  listUserId: any[] = [];
  listMaterialId: any[] = [];
  listCreate: any[] = [];
  listExpert: any[] = [];
  Survey: any[] = [];
  listSurveyTool: any[] = [];
  listCustomerContact: any[] = [];
  isAction: boolean = false;
  Id: string;
  customerId: string;
  row: any = {};
  isAdd = false;
  CustomerRequirementId: string;
  ProjectPhaseId: string;
  listUser: any[] = [];
  columnNameCustomerContact: any[] = [{ Name: 'Name', Title: 'Tên' }, { Name: 'Address', Title: 'Đia chỉ' },];

  model: any = {
    Id: '',
    ProjectPhaseId: '',
    CustomerRequirementId: '',
    SurveyDate: '',
    Level: '',
    Time: '',
    Status: 1,
    Gender: '1',
    ListUser: [],
    ListMaterial: [],
    ListSuLveyTool: [],
    ListRequest: [],
    Note: '',
    Quantity: '',
  }

  userModel: any = {
    UserId: '',
    SurveyId: '',
  }

  ChangeStepModel = {
    Id: ''
  }
  listData: any[] = []
  listTemp: any[] = [];
  listSurvey: any[] = [];
  surveyDateView = null;


  ngOnInit() {
    this.model.customerRequirementId = this.CustomerRequirementId;
    if (this.isAdd == false) {
      this.ModalInfo.Title = 'Chỉnh sửa lượt khảo sát';
      this.ModalInfo.SaveText = 'Lưu';
      this.getById();
    }
    else {
      this.ModalInfo.Title = 'Thêm mới lượt khảo sát';
    }
    this.getlistUser();
    this.getCustomerContact();
  }
  getById() {
    this.model.Id = this.Id;
    this.surveyService.getById(this.model.Id).subscribe(
      data => {
        this.model = data;

        this.model.ListUser.forEach(item => {
          this.listUserId.push(item.UserId);
        });

        this.model.ListMaterial.forEach(item => {
          this.listMaterialId.push(item.MaterialId);
        });

        this.model.ListRequest.forEach(item => {
          this.listRequestId.push(item.surveyId);
        });

        if (this.model.SurveyDate) {
          this.surveyDateView = this.dateUtils.convertDateToObject(this.model.SurveyDate);
        }
      },
    )
  }

  isCheck = true;
  save(isContinue: boolean) {
    if (this.surveyDateView) {
      this.model.SurveyDate = this.dateUtils.convertObjectToDate(this.surveyDateView);
    }

    if (this.isAdd == true) {
      this.listCreate.push(this.model)
      this.create(true);
      this.activeModal.close({ isAdd: true, modelTemp: this.listCreate });
    }
    else {
      this.activeModal.close({ isAdd: false, modelTemp: this.model });
      this.update();
    }
  }

  update() {
    this.surveyService.update(this.model.Id, this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật lượt khảo sát ');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.surveyService.create(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
          };
          this.messageService.showSuccess('Thêm mới Yêu cầu khách hàng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới Yêu cầu khách hàng thành công!');
          //this.closeModal(true);
          this.activeModal.close({ isAdd: false, modelTemp: this.model });
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  modelContent: any = {
    Id: '',
    Conten: '',
    Result: '',
    SurveyId: '',
    ListSurveyContentAttach: [],
    ListUser: [],
    EmployeeId: '',
    Name: '',
    Level: '',
  }
  listReques: any[] = [];
  showCreateUpdate(row, isAdd: boolean) {
    let activeModal = this.modalService.open(SurveyContentCreateComponent, { container: 'body', windowClass: 'survey-content-create-model', backdrop: 'static' })
    activeModal.componentInstance.row = Object.assign({}, row);
    activeModal.componentInstance.isAdd = isAdd;
    activeModal.componentInstance.listTemp = this.model.ListRequest;
    activeModal.result.then((result) => {
      if (result.modelTemp) {
        if (result.isAdd == true) {
          result.modelTemp.forEach(element => {
            if (this.model.ListRequest.length > 0) {
              // this.model.ListRequest.forEach(data => {
              //   this.model.ListRequest.push(element);
              //   this.messageService.showSuccess('Thêm mới người liên hệ!');
              // });

              var data = Object.assign({}, this.modelContent);
              data.Id = element.Id;
              data.Content = element.Content;
              data.Result = element.Result;
              data.SurveyId = element.SurveyId;
              data.ListSurveyContentAttach = element.ListSurveyContentAttach;
              data.EmployeeId = element.EmployeeId;
              data.Name = element.Name;
              data.Level= element.Level;

              this.model.ListRequest.push(data);
            }
            else {
              this.model.ListRequest.push(element);
            }
          });
        }

      }
    }, (reason) => {
    });
  }

  showComfirmDeleteContent(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá người khảo sát này không?").then(
      data => {
        this.model.ListRequest.splice(index, 1);
      },
      error => {
      }
    );
  }




  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close({ isAdd: true, modelTemp: this.listCreate });
  }

  showSelectEmployee() {
    let activeModal = this.modalService.open(ShowChooseEmployeeComponent, { container: 'body', windowClass: 'select-employee-model', backdrop: 'static' });

    activeModal.componentInstance.listEmployeeId = this.listUserId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          element.IsNew = true;
          this.model.ListUser.push(element);
        });
        var j = 1;
        this.model.ListUser.forEach(element => {
          element.index = j++;
        });
      }
    }, (reason) => {
    });
  }

  showComfirmDeleteEmployee(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá người khảo sát này không?").then(
      data => {
        this.model.ListUser.splice(index, 1);
      },
      error => {

      }
    );
  }

  showCreateMaterial() {
    let activeModal = this.modalService.open(SurveyMaterialCreateComponent, { container: 'body', windowClass: 'survey-material-create-model', backdrop: 'static' })

    activeModal.componentInstance.SurveyId = this.model.Id;
    activeModal.result.then((results) => {
      if (results) {
        //this.model.ListMaterial.push(results);
        results.forEach(element => {
          this.model.ListMaterial.push({
            IsNew: true,
            Name: element.Name,
            ManufactureCode: element.ManufactureCode,
            Code: element.Code,
            Note: element.Note,
            Quantity: element.Quantity,
          });
        });
      }
    }, (reason) => {
    });
  }

  showSelectMaterial() {
    let activeModal = this.modalService.open(SelectMaterialComponent, { container: 'body', windowClass: 'select-material-model', backdrop: 'static' });
    // this.model.ListMaterial.forEach(element => {
    //   this.listMaterialId.push(element.Id);
    // });

    activeModal.componentInstance.MaterialId = this.listMaterialId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          element.IsNew = true;
          this.model.ListMaterial.push(element);
        });
        var j = 1;
        this.model.ListMaterial.forEach(element => {
          element.index = j++;
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteMaterial(index: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa dụng cụ này không?").then(
      data => {
        this.model.ListMaterial.splice(index, 1);
        this.messageService.showSuccess('Xóa dụng cụ thành công!');
      },
      error => {

      }
    );
  }

  getlistUser() {
    this.comboboxService.getListUser().subscribe(
      data => {
        this.listUser = data;
      },
    )
  }

  getCustomerContact() {
    if (this.customerId) {
      this.comboboxService.getCustomerContact(this.customerId).subscribe(
        data => {
          this.listCustomerContact = data;
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.listCustomerContact = [];
    }

  }
}
