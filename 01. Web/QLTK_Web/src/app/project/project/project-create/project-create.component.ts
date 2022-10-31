import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProjectServiceService } from '../../service/project-service.service';
import * as moment from 'moment';


@Component({
  selector: 'app-project-create',
  templateUrl: './project-create.component.html',
  styleUrls: ['./project-create.component.scss']
})
export class ProjectCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    public dateUtils: DateUtils,
    public activeModal: NgbActiveModal,
    private projectService: ProjectServiceService
  ) { }
  startIndex = 1;
  listDepartment: any[] = [];
  listSBU = [];
  listCustomerType: any[] = [];
  listCustomer: any[] = [];
  listCustomerId: any[] = [];
  employees: any[] = [];
  dateFrom: string;
  dateTo: string;
  kickOffDate: string;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  sbuColumnName: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  departmentColumnName: any[] = [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }];
  employeeColumnName: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  listStatus: any[] = [
    { Id: '1', Name: 'Chưa kickoff' },
    { Id: '8', Name: 'Thiết kế' },
    { Id: '2', Name: 'Sản xuất' },
    { Id: '5', Name: 'Lắp đặt' },
    { Id: '6', Name: 'Hiệu chỉnh' },
    { Id: '7', Name: 'Đưa vào sử dụng' },
    { Id: '9', Name: 'Nghiệm thu' },
    { Id: '4', Name: 'Tạm dừng' },
    { Id: '3', Name: 'Đóng dự án' },
    { Id: '11', Name: 'Vật tư' }

  ]

  model: any = {
    Id: '',
    SBUId: '',
    DepartmentId: '',
    CustomerId: '',
    CustomerTypeId: '',
    Name: '',
    Code: '',
    Note: '',
    Status: '1',
    DateFrom: null,
    DateTo: null,
    CreateBy: '',
    Parameter: '',
    KickOffDate: null,
    SaleNoVAT: '',
    WarehouseCode: '',
    Price: 0,
    DesignPrice: 0,
    CustomerFinalId: '',
    FCMPrice: 0,
    Type: 1,
    Priority : '2',
  }
  isAction: boolean = false;
  Id: string;

  ngOnInit() {
    this.appSetting.PageTitle = "Thêm mới dự án";
    let user = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (user) {
      this.model.SBUId = user.sbuId;
      this.model.DepartmentId = user.departmentId;
    }
    this.getListCustomer();
    this.getListCustomerType();
    this.getCbbSBU();
    this.getEmployees();
  }

  getCbbSBU() {
    this.comboboxService.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
        if (!this.model.SBUId && this.listSBU.length > 0) {
          this.model.SBUId = this.listSBU[0].Id;
        }

        this.getCbbDepartment();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getEmployees() {
    this.comboboxService.getCbbEmployee().subscribe(
      data => {
        this.employees = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
        if (!this.model.DepartmentId && this.listDepartment.length > 0) {
          this.model.DepartmentId = this.listDepartment[0].Id;
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListCustomer() {
    this.comboboxService.getListCustomer().subscribe(
      data => {
        this.listCustomer = data;
        for (var item of this.listCustomer) {
          this.listCustomerId.push(item.Id);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListCustomerType() {
    this.comboboxService.getListCustomerType().subscribe(
      data => {
        this.listCustomerType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCustomerType() {
    if (this.model.CustomerId) {
      this.getCustomerTypeInfo();
    } else {
      this.model.CustomerTypeId = '';
    }
  }

  createproject(isContinue) {
    if (this.dateFrom) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.dateFrom);
    }
    if (this.dateTo) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.dateTo);
      var time1 = moment(this.model.DateFrom).format('YYYY-MM-DD');
      var time2 = moment(this.model.DateTo).format('YYYY-MM-DD');
      if (time1 > time2) {
        this.messageService.showMessage('Ngày kết thúc phải lớn hơn ngày bắt đầu');
        return;
      }
    }
    if (this.kickOffDate) {
      this.model.KickOffDate = this.dateUtils.convertObjectToDate(this.kickOffDate);
    }
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.projectService.AddProject(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {
              Id: '',
              SBUId: '',
              DepartmentId: '',
              CustomerId: '',
              CustomerTypeId: '',
              Name: '',
              Code: '',
              Note: '',
              Status: '',
              DateFrom: null,
              DateTo: null,
              CreateBy: '',
              Parameter: '',
              KickOffDate: null,
              WarehouseCode: '',
              FCMPrice: 0,
              Type: 1,
              Priority : '2',
            };
            this.dateFrom = null;
            this.dateTo = null;
            this.kickOffDate = null;
            this.messageService.showSuccess('Thêm mới dự án thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới dự án thành công!');
            this.closeModal();
          }
        }, error => {
          this.messageService.showError(error);
        });
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.projectService.AddProject(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.dateFrom = '';
                this.dateTo = '';
                this.kickOffDate = '';
                this.messageService.showSuccess('Thêm mới dự án thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới dự án thành công!');
                this.closeModal();
              }
            }, error => {
              this.messageService.showError(error);
            });
        },
        error => {

        }
      );
    }
  }

  save(isContinue: boolean) {
    this.createproject(isContinue);

  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-du-an']);
  }

  getCustomerTypeInfo() {
    this.projectService.GetCustomerTypeInfo({ Id: this.model.CustomerId }).subscribe(data => {
      this.model.CustomerTypeId = data.CustomerTypeId;
    }, error => {
      this.messageService.showError(error);
    });
  }

  treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.CustomerId = e.selectedRowKeys[0];
    this.getCustomerTypeInfo();
    this.closeDropDownBox();
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  changeEmployee() {
    this.employees.forEach(item => {
      if (this.model.ManageId == item.Id) {
        this.model.PhoneNumber = item.Exten;
      }
    });
  }
}
