import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants, DateUtils, ComboboxService, PermissionService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProjectServiceService } from '../../service/project-service.service';


import { ProjectPaymentCreateModalComponent } from 'src/app/project/project/project-payment-create-modal/project-payment-create-modal.component';
import * as moment from 'moment';
import { ProjectPaymentService } from '../../service/project-payment.service';
import { ProjectEmployeeService } from '../../service/project-employee.service';

@Component({
  selector: 'app-project-update',
  templateUrl: './project-update.component.html',
  styleUrls: ['./project-update.component.scss']
})
export class ProjectUpdateComponent implements OnInit {

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
    public permissionService: PermissionService,
    public paymentService: ProjectPaymentService,
    private projectEmployeeService: ProjectEmployeeService,
    //#region thaint
    private modalService: NgbModal,
    //#endregion
  ) { }

  startIndex = 1;
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
  Index =1;
  ListPlan =[];
  PlanId : string;

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
    UpdateDate: null,
    CreateBy: '',
    Parameter: '',
    KickOffDate: null,
    WarehouseCode: '',
    Price: 0,
    DesignPrice: 0,
    CustomerFinalId: '',
    FCMPrice: 0,
    Priority: '',
    PaymentStatus: 0,
    IsBadDebt: false,
    BadDebtDate: null,
    //#region  thaint
    PaymentModels: [],

    //#endregion
  }
  //#region
  TotalWithPaymentModel: {
    PaymentModels: [{Id :''}],
    TotalPlanAmount: 0,
    ActualPlanAmount: 0,
  }
  //#endregion
  isAction: boolean = false;
  Id: string;

  //#region thaint
  total: any;
  //#endregion
  IsChange =false;
  IsChangeDate =false;
  employeeId='';
  tabId = null;
  ngOnInit() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    if (this.model.Id) {
      this.getProjectInfo();
      this.getListCustomerType();
      this.getCbbSBU();
      this.getEmployees();
      this.getPayment();

    }
    this.employeeId = currentUser.employeeId
    this.tabId =1;
  }

  selectIndex = -1;

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
    this.projectService.GetProjectInfo({ Id: this.model.Id }).subscribe(data => {
      this.model = data;

      this.model.Priority = '' + this.model.Priority;
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
      if (data.BadDebtDate) {
        let dateArray = data.BadDebtDate.split('T')[0];
        let dateValue = dateArray.split('-');
        let tempBadDebtDateV = {
          'day': parseInt(dateValue[2]),
          'month': parseInt(dateValue[1]),
          'year': parseInt(dateValue[0])
        };
        this.model.BadDebtDate = tempBadDebtDateV;
      }

      this.getCustomerTypeInfo();
    });
  }

  updateProject() {
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
      this.projectService.UpdateProject(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Cập nhật dự án thành công!');
        }, error => {
          this.messageService.showError(error);
        });
    } else {
      this.projectService.UpdateProject(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Cập nhật dự án thành công!');
        }, error => {
          this.messageService.showError(error);
        });
    }
  }

  save() {
    this.updateProject();
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-du-an']);
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

  //#region thaint
  showCreatePayment(Id: string) {
    let activeModal = this.modalService.open(ProjectPaymentCreateModalComponent, { container: 'body', windowClass: 'project-payment-create-modal', backdrop: 'static' });
    activeModal.componentInstance.ProjectId = this.model.Id;
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      this.getPayment();
    }, (reason) => {
    });
  }



  getPayment() {
    this.paymentService.SearchPayment(this.model.Id).subscribe(
      data => {
        this.TotalWithPaymentModel = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
    // this.selectIndex = -1;
    }


  showConfirmDeletePayment(Id: string, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thanh toán này không?").then(
      data => {
        this.deletePayment(Id, index);
      },
      error => {
      }
    );
  }

  Iddelete: any;
  deletePayment(Id: string, index) {
    if (Id != null && index != null) {
      this.paymentService.DeletePayment(Id).subscribe(
        data => {
          this.messageService.showSuccess('Xóa tiến độ thu tiền thành công!');
          this.getPayment();

        },
        error => {
          this.messageService.showError(error);
        }
      );
    }

  }

  selectBedDebt() {
    this.projectService.UpdateBedDebt({ Id: this.routeA.snapshot.paramMap.get('Id'), IsBadDebt: this.model.IsBadDebt }).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật tình trạng nợ xấu cho dự án thành công!');
        this.getProjectInfo();

      }, error => {
        this.messageService.showError(error);
      });
  }



  updateBadDebtDate() {
    if (this.model.BadDebtDate) {
      this.model.BadDebtDate = this.dateUtils.convertObjectToDate(this.model.BadDebtDate);
    }
    else{
      this.model.BadDebtDate = null;
    }
    this.projectService.UpdateBadDebtDate({ Id: this.routeA.snapshot.paramMap.get('Id'), BadDebtDate: this.model.BadDebtDate, IsBadDebt: this.model.IsBadDebt }).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật ngày nợ xấu cho dự án thành công!');
        this.getProjectInfo();

      }, error => {
        this.messageService.showError(error);
      });
  }
  //#endregion
  select(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      var PaymentId = this.TotalWithPaymentModel.PaymentModels[index].Id;
      this.paymentService.GetPlanByPaymentId(PaymentId).subscribe(
        data => {
          this.ListPlan = data;
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.selectIndex = -1;
      this.ListPlan=[];
    }
  }
  showConfirmDelete(Id,index){
    this.paymentService.DeletePlanById(Id).subscribe(
      data => {
        this.messageService.showSuccess('Xóa công việc thành công!');
        this.ListPlan.splice(index,1);
        this.UpdatePlanPayment();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  checkPermission(){
    this.projectEmployeeService.getProjectEmployeeByProjectId(this.model.Id).subscribe(list => {
      if (list) {
        var result =list.filter(a =>a.EmployeeId == this.employeeId);
        if(result.length == 0){
          this.IsChange =false;
        }else{
          this.IsChange =true;
          var check =result.filter(a =>a.Checked == true);
          if(check.length >0){
            this.IsChangeDate =true;
          }else{
            this.IsChangeDate =false;
          }
        }
      }
    }, error => {
      this.messageService.showError(error);
    });
  }
  getAllPayment(){
    this.getPayment();
    this.selectIndex = -1;
  }
  UpdatePlanPayment(){
    this.paymentService.UpdatePlanPaymentDate(this.model.Id).subscribe(
      result => {
        this.getPayment();
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
  getStageDelayId(PlanId : string){
    this.PlanId =PlanId;
    this.tabId='schedule-project';
  }
  checkTabId(){
    this.tabId='Dashboard-project';
  }
  setStageDelay(PlanId : string){
    this.PlanId =PlanId;
  }
}
