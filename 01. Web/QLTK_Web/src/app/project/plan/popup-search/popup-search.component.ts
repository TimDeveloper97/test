import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { element } from 'protractor';
import { ComboboxService, Constants, DateUtils, MessageService } from 'src/app/shared';
import { ScheduleProjectService } from '../../service/schedule-project.service';

@Component({
  selector: 'app-popup-search',
  templateUrl: './popup-search.component.html',
  styleUrls: ['./popup-search.component.scss'],
  encapsulation: ViewEncapsulation.None,

})
export class PopupSearchComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constant: Constants,
    private dateUtils: DateUtils,
    private comboboxService: ComboboxService
  ) { }

  isAction: boolean = false;
  idProject: string;

  daysOfMonth: any = [];
  dayOfWeek: any = [];
  listProjectProduct: any = [];
  listExplanedId: any = [];
  ItemSearch: any[] = [];

  Type = [
    { Id: 0, Name: "Tất cả", Code: "0" },
    { Id: 1, Name: "Theo kế hoạch", Code: "KH" },
    { Id: 2, Name: "Phát sinh - tính phí (PC)", Code: "PC" },
    { Id: 3, Name: "Phát sinh không tính phí", Code: "PS" },
  ]

  PlanStatus = [
    { Id: 0, Name: "Tất cả", Code: "0" },
    { Id: 1, Name: "OK", Code: "KH" },
    { Id: 2, Name: "Quá hạn HĐ", Code: "PC" },
    { Id: 3, Name: "Quá hạn HT", Code: "PS" },
    { Id: 4, Name: "Thiếu ngày TK", Code: "PS" },
  ]

  StatusPlan = [
    { Id: 0, Name: "Tất cả" },
    { Id: 1, Name: "Chậm tiến độ" },
  ]

  WorkStatus = [
    { Id: 0, Name: "Tất cả", Code: "0" },
    { Id: 1, Name: 'Open', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    { Id: 2, Name: 'On Going', BadgeClass: 'badge-yellow', TextClass: 'text-green' },
    { Id: 3, Name: 'Close', BadgeClass: 'badge-green', TextClass: '' },
    { Id: 4, Name: 'Stop', BadgeClass: 'badge-secondary', TextClass: '' },
    { Id: 5, Name: 'Cancel', BadgeClass: 'badge-secondary', TextClass: '' },
  ]

  ErrorProduct=[
    { Id: 0, Name: "Tất cả", Code: "0" },
    { Id: 1, Name: 'SP thiếu ngày triển khai', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    { Id: 2, Name: 'SP thiếu ngày hợp đồng', BadgeClass: 'badge-yellow', TextClass: 'text-green' },
    { Id: 3, Name: 'SP phát sinh', BadgeClass: 'badge-green', TextClass: '' },
  ]

  ErrorModule=[
    { Id: 0, Name: "Tất cả", Code: "0" },
    { Id: 1, Name: 'Module thiếu ngày triển khai', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    { Id: 2, Name: 'Module thiếu ngày hợp đồng', BadgeClass: 'badge-yellow', TextClass: 'text-green' },
    { Id: 3, Name: 'Module phát sinh', BadgeClass: 'badge-green', TextClass: '' },
  ]

  ErrorStage=[
    { Id: 0, Name: "Tất cả", Code: "0" },
    { Id: 1, Name: 'CĐ thiếu ngày triển khai', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    { Id: 2, Name: 'CĐ thiếu ngày hợp đồng', BadgeClass: 'badge-yellow', TextClass: 'text-green' },
    { Id: 3, Name: 'CĐ quá hạn HĐ', BadgeClass: 'badge-green', TextClass: '' },
  ]

  modelSearchProject: any = {
    ProjectId: '',
    ContractStartDateToV: null,
    ContractStartDateFromV: null,
    ContractDueDateToV: null,
    ContractDueDateFromV: null,
    PlanStartDateToV: null,
    PlanStartDateFromV: null,
    PlanDueDateToV: null,
    PlanDueDateFromV: null,
    EmployeeId: '',
    WorkProgress: 0,
    StageId: '',
    ImplementingAgenciesCode: '',
    DepartmentId: '',
    WorkClassify: 0,
    PlanStatus: 0,
    WorkStatus: 0,
    DateFromV: null,
    DateToV: null,
    ErrorProduct :0,
    ErrorModule : 0,
    ErrorStage:0,
    NameProduct:'',
    StageDelayId :''
  }

  _searchItems: any[] = [];
  count = 0;
  _searchValues: any[] = [];
  public _searchModel: any = {};
  Employee: any[] = [];
  Department: any[] = [];

  ColumnsEmployee = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Id: 'Tên nhân viên' }];
  ColumnsSupplier = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Id: 'Tên đơn vị' }];
  ColumnsDepartment = [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Id: 'Tên phòng ban' }];

  Departments: any[] = [];
  Employees: any[] = [];
  Stages: any[] = [];
  Suppliers: any[] = [];
  listUserId: any[] = [];


  ngOnInit(): void {
    this.modelSearchProject.ProjectId = this.idProject;
    this.getEmployee();
    this.getDepartment();
    this.getStage();
    this.getSupplier();

    if (this.modelSearchProject.ContractStartDateToV) {
      this.modelSearchProject.ContractStartDateToV = this.dateUtils.convertDateToObject(this.modelSearchProject.ContractStartDateToV);
    }
    if (this.modelSearchProject.ContractStartDateFromV) {
      this.modelSearchProject.ContractStartDateFromV = this.dateUtils.convertDateToObject(this.modelSearchProject.ContractStartDateFromV)
    }
    if (this.modelSearchProject.ContractDueDateToV) {
      this.modelSearchProject.ContractDueDateToV = this.dateUtils.convertDateToObject(this.modelSearchProject.ContractDueDateToV);
    }
    if (this.modelSearchProject.ContractDueDateFromV) {
      this.modelSearchProject.ContractDueDateFromV = this.dateUtils.convertDateToObject(this.modelSearchProject.ContractDueDateFromV)
    }
    if (this.modelSearchProject.PlanStartDateToV) {
      this.modelSearchProject.PlanStartDateToV = this.dateUtils.convertDateToObject(this.modelSearchProject.PlanStartDateToV);
    }
    if (this.modelSearchProject.PlanStartDateFromV) {
      this.modelSearchProject.PlanStartDateFromV = this.dateUtils.convertDateToObject(this.modelSearchProject.PlanStartDateFromV)
    }
    if (this.modelSearchProject.PlanDueDateToV) {
      this.modelSearchProject.PlanDueDateToV = this.dateUtils.convertDateToObject(this.modelSearchProject.PlanDueDateToV);
    }
    if (this.modelSearchProject.PlanDueDateFromV) {
      this.modelSearchProject.PlanDueDateFromV = this.dateUtils.convertDateToObject(this.modelSearchProject.PlanDueDateFromV)
    }
    if (this.modelSearchProject.DateFromV) {
      this.modelSearchProject.DateFromV = this.dateUtils.convertDateToObject(this.modelSearchProject.DateFromV)
    }
    if (this.modelSearchProject.DateToV) {
      this.modelSearchProject.DateToV = this.dateUtils.convertDateToObject(this.modelSearchProject.DateToV)
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }



  isSearchConditon: boolean;
  projectIdSelect: any;


  getEmployee() {
    var emp={Id: 'thieunv', Name: "Thiếu NS TK",Code :'TNSTK'};
    this.comboboxService.getCbbEmployee().subscribe(
      data => {
        this.Employees.push(emp);
        if (this.listUserId) {
          this.listUserId.forEach(element => {
            data.forEach(item => {
              if (item.ObjectId == element) {
                this.Employees.push(item);
              }
            })
          })
        }
      });
  }

  getDepartment() {
    this.comboboxService.getCbbDepartment().subscribe(
      data => {
        this.Departments = data;
      });
  }

  getStage() {
    this.comboboxService.getCbbStage().subscribe(
      data => {
        this.Stages = data;
      });
  }

  getSupplier() {
    this.comboboxService.getSupplierbyProject(this.idProject).subscribe(
      data => {
        this.Suppliers = data;
      });
  }

  Search() {
    this.ItemSearch = [];
    if (this.modelSearchProject.DepartmentId) {
      this.showItemSearch(this.Departments, this.modelSearchProject.DepartmentId, 'Phòng ban');
      this.modelSearchProject.DepartmentId = this.modelSearchProject.DepartmentId;
    }
    if (this.modelSearchProject.WorkStatus) {
      this.showItemSearch(this.WorkStatus, this.modelSearchProject.WorkStatus, 'Tình trạng công việc');
      this.modelSearchProject.WorkStatus = this.modelSearchProject.WorkStatus;
    }
    if (this.modelSearchProject.ErrorProduct) {
      this.showItemSearch(this.ErrorProduct, this.modelSearchProject.ErrorProduct, 'Vấn đề của SP');
      this.modelSearchProject.ErrorProduct = this.modelSearchProject.ErrorProduct;
    }
    if (this.modelSearchProject.NameProduct) {
      this.showItemSearchDate(this.modelSearchProject.NameProduct, 'Tên sản phẩm');
      this.modelSearchProject.NameProduct = this.modelSearchProject.NameProduct;
    }
    if (this.modelSearchProject.ErrorModule) {
      this.showItemSearch(this.ErrorModule, this.modelSearchProject.ErrorModule, 'Vấn đề của module');
      this.modelSearchProject.ErrorModule = this.modelSearchProject.ErrorModule;
    }
    if (this.modelSearchProject.ErrorStage) {
      this.showItemSearch(this.ErrorStage, this.modelSearchProject.ErrorStage, 'Vấn đề của CĐ');
      this.modelSearchProject.ErrorStage = this.modelSearchProject.ErrorStage;
    }
    if (this.modelSearchProject.WorkClassify) {
      this.showItemSearch(this.Type, this.modelSearchProject.WorkClassify, 'Phân loại công việc');
      this.modelSearchProject.WorkClassify = this.modelSearchProject.WorkClassify;
    }
    if (this.modelSearchProject.WorkProgress) {
      this.showItemSearch(this.StatusPlan, this.modelSearchProject.WorkProgress, 'Trạng thái công việc');
      this.modelSearchProject.WorkProgress = this.modelSearchProject.WorkProgress;
    }
    if (this.modelSearchProject.StageId) {
      this.showItemSearch(this.Stages, this.modelSearchProject.StageId, 'Công đoạn');
      this.modelSearchProject.StageId = this.modelSearchProject.StageId;
    }
    if (this.modelSearchProject.PlanStatus) {
      this.showItemSearch(this.PlanStatus, this.modelSearchProject.PlanStatus, 'Tình trạng lên kế hoạch');
      this.modelSearchProject.PlanStatus = this.modelSearchProject.PlanStatus;
    }
    if (this.modelSearchProject.EmployeeId) {
      this.showItemSearch(this.Employees, this.modelSearchProject.EmployeeId, 'Nhân viên');
      this.modelSearchProject.EmployeeId = this.modelSearchProject.EmployeeId;
    }
    if (this.modelSearchProject.ImplementingAgenciesCode) {
      this.showItemSearch(this.Suppliers, this.modelSearchProject.ImplementingAgenciesCode, 'Đơn vị thực hiện');
      this.modelSearchProject.ImplementingAgenciesCode = this.modelSearchProject.ImplementingAgenciesCode;
    }
    var contactsStartDate = '';
    if (this.modelSearchProject.ContractStartDateToV) {
      this.modelSearchProject.ContractStartDateToV = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractStartDateToV);
      contactsStartDate = contactsStartDate + this.modelSearchProject.ContractStartDateToV;
    }
    if (this.modelSearchProject.ContractStartDateFromV) {
      this.modelSearchProject.ContractStartDateFromV = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractStartDateFromV);
      contactsStartDate = contactsStartDate + ' - ' + this.modelSearchProject.ContractStartDateFromV;
    }
    if (contactsStartDate != '') {
      this.showItemSearchDate(contactsStartDate, 'Bắt đầu hợp đồng');
    }
    var contactDueDate = '';
    if (this.modelSearchProject.ContractDueDateToV) {
      this.modelSearchProject.ContractDueDateToV = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractDueDateToV);
      contactDueDate = contactDueDate + this.modelSearchProject.ContractDueDateToV;
    }
    if (this.modelSearchProject.ContractDueDateFromV) {
      this.modelSearchProject.ContractDueDateFromV = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractDueDateFromV);
      contactDueDate = contactDueDate + ' - ' + this.modelSearchProject.ContractDueDateFromV;
    }
    if (contactDueDate != '') {
      this.showItemSearchDate(contactDueDate, 'Kết thúc hợp đồng');
    }
    var planStartDate = '';
    if (this.modelSearchProject.PlanStartDateToV) {
      this.modelSearchProject.PlanStartDateToV = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanStartDateToV);
      planStartDate = planStartDate + this.modelSearchProject.PlanStartDateToV;
    }
    if (this.modelSearchProject.PlanStartDateFromV) {
      this.modelSearchProject.PlanStartDateFromV = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanStartDateFromV);
      planStartDate = planStartDate + ' - ' + this.modelSearchProject.PlanStartDateFromV;
    }
    if (planStartDate != '') {
      this.showItemSearchDate(planStartDate, 'Bất đầu triển khai');
    }
    var planDueDate = '';
    if (this.modelSearchProject.PlanDueDateToV) {
      this.modelSearchProject.PlanDueDateToV = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanDueDateToV);
      planDueDate = planDueDate + this.modelSearchProject.PlanDueDateToV;
    }
    if (this.modelSearchProject.PlanDueDateFromV) {
      this.modelSearchProject.PlanDueDateFromV = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanDueDateFromV);
      planDueDate = planDueDate + ' - ' + this.modelSearchProject.PlanDueDateFromV;
    }
    if (planDueDate != '') {
      this.showItemSearchDate(planDueDate, 'Kết thúc triển khai');
    }
    this.closeModal(true);
  }
  Clear() {
    this.ItemSearch = [];
    if (this.modelSearchProject.DepartmentId) {
      this.modelSearchProject.DepartmentId = "";
    }
    if (this.modelSearchProject.WorkStatus) {
      this.modelSearchProject.WorkStatus = "";
    }
    if (this.modelSearchProject.WorkClassify) {
      this.modelSearchProject.WorkClassify = "";
    }
    if (this.modelSearchProject.WorkProgress) {
      this.modelSearchProject.WorkProgress = "";
    }
    if (this.modelSearchProject.StageId) {
      this.modelSearchProject.StageId = "";
    }
    if (this.modelSearchProject.PlanStatus) {
      this.modelSearchProject.PlanStatus = "";
    }
    if (this.modelSearchProject.EmployeeId) {
      this.modelSearchProject.EmployeeId = "";
    }
    if (this.modelSearchProject.ImplementingAgenciesCode) {
      this.modelSearchProject.ImplementingAgenciesCode = "";
    }
    if (this.modelSearchProject.ContractStartDateToV) {
      this.modelSearchProject.ContractStartDateToV = null;
    }
    if (this.modelSearchProject.ContractStartDateFromV) {
      this.modelSearchProject.ContractStartDateFromV = null;
    }
    if (this.modelSearchProject.ContractDueDateToV) {
      this.modelSearchProject.ContractDueDateToV = null;
    }
    if (this.modelSearchProject.ContractDueDateFromV) {
      this.modelSearchProject.ContractDueDateFromV = null;
    }
    if (this.modelSearchProject.PlanStartDateToV) {
      this.modelSearchProject.PlanStartDateToV = null;
    }
    if (this.modelSearchProject.PlanStartDateFromV) {
      this.modelSearchProject.PlanStartDateFromV = null;
    }
    if (this.modelSearchProject.PlanDueDateToV) {
      this.modelSearchProject.PlanDueDateToV = null;
    }
    if (this.modelSearchProject.PlanDueDateFromV) {
      this.modelSearchProject.PlanDueDateFromV = null;
    }
    if (this.modelSearchProject.DateFromV) {
      this.modelSearchProject.DateFromV = null;
    }
    if (this.modelSearchProject.DateToV) {
      this.modelSearchProject.DateToV = null;
    }
    if (this.modelSearchProject.ErrorProduct) {
      this.modelSearchProject.ErrorProduct = "";
    }
    if (this.modelSearchProject.ErrorModule) {
      this.modelSearchProject.ErrorModule = "";
    }
    if (this.modelSearchProject.ErrorStage) {
      this.modelSearchProject.ErrorStage = "";
    }
    if (this.modelSearchProject.NameProduct) {
      this.modelSearchProject.NameProduct = "";
    }
    if(this.modelSearchProject.StageDelayId){
      this.modelSearchProject.StageDelayId="";
    }
    this.closeModal(true);
  }
  showItemSearch(arr: any, id: any, name: any) {
    arr.forEach(element => {
      if (element.Id == id) {
        var item = {
          Name: name,
          Value: element.Name
        }
        this.ItemSearch.push(item);
      }
    })
  }
  showItemSearchDate(value: any, name: any) {
    var item = {
      Name: name,
      Value: value
    }
    this.ItemSearch.push(item);
  }
}
