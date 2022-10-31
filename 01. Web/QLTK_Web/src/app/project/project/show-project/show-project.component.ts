import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants, DateUtils, ComboboxService, PermissionService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProjectServiceService } from '../../service/project-service.service';
import * as moment from 'moment';

@Component({
  selector: 'app-show-project',
  templateUrl: './show-project.component.html',
  styleUrls: ['./show-project.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowProjectComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private messageService: MessageService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private routeA: ActivatedRoute,
    private titleservice: Title,
    private comboboxService: ComboboxService,
    public constant: Constants,
    public dateUtils: DateUtils,
    public activeModal: NgbActiveModal,
    private projectService: ProjectServiceService,
    public permissionService: PermissionService
  ) { }

  startIndex = 1;
  id: String;
  dateFrom: any;
  dateTo: any;
  kickOffDate: any;
  listDepartment: any[] = [];
  listSBU = [];
  listCustomerType: any[] = [];
  employees: any[] = [];
  listCustomer: any[] = [];
  listCustomerId: any[] = [];
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
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

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
    WarehouseCode: '',
    Price: 0,
    DesignPrice: 0,
    CustomerFinalId: '',
    FCMPrice: 0,
  }
  isAction: boolean = false;
  Id: string;

  ngOnInit() {

    //this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    if (this.id) {
      this.getProjectInfo();
      this.getListCustomerType();
      this.getCbbSBU();
      this.getEmployees();
    }
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
  getCbbSBU() {
    this.comboboxService.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
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

  getProjectInfo() {
    this.model.Id = this.id;
    this.projectService.GetProjectInfo({ Id: this.model.Id }).subscribe(data => {
      this.model = data;
      this.appSetting.PageTitle = "Chỉnh sửa dự án - " + this.model.Code + " - " + this.model.Name;
      this.getListCustomer();
      this.getCbbDepartment();
      if (data.DateFrom) {
        let dateArray = data.DateFrom.split('T')[0];
        let dateValue = dateArray.split('-');
        let tempDateFromV = {
          'day': parseInt(dateValue[2]),
          'month': parseInt(dateValue[1]),
          'year': parseInt(dateValue[0])
        };
        this.dateFrom = tempDateFromV;
      }
      if (data.DateTo) {
        let dateArray1 = data.DateTo.split('T')[0];
        let dateValue1 = dateArray1.split('-');
        let tempDateFromV1 = {
          'day': parseInt(dateValue1[2]),
          'month': parseInt(dateValue1[1]),
          'year': parseInt(dateValue1[0])
        };
        this.dateTo = tempDateFromV1;
      }
      if (data.KickOffDate) {
        let dateArray3 = data.KickOffDate.split('T')[0];
        let dateValue3 = dateArray3.split('-');
        let tempDateFromV3 = {
          'day': parseInt(dateValue3[2]),
          'month': parseInt(dateValue3[1]),
          'year': parseInt(dateValue3[0])
        };
        this.kickOffDate = tempDateFromV3;
      }

      this.getCustomerTypeInfo();
    });
  }

 

 

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  getCustomerTypeInfo() {
    this.projectService.GetCustomerTypeInfo({ Id: this.model.CustomerId }).subscribe(data => {
      if (data) {
        this.model.CustomerTypeId = data.CustomerTypeId;
      }
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
