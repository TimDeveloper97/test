import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { EmployGroupServiceService } from '../../service/employ-group-service.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { EmployeeGroupCreateComponent } from '../employee-group-create/employee-group-create.component';

@Component({
  selector: 'app-employee-group-manage',
  templateUrl: './employee-group-manage.component.html',
  styleUrls: ['./employee-group-manage.component.scss']
})
export class EmployeeGroupManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private employeeGroupService: EmployGroupServiceService,
  ) { }

  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    EmployeeGroupId: '',
    Name: '',
    Code: '',
    Note: '',
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã nhóm nhân viên',
    Items: [
      {
        Name: 'Tên nhóm nhân viên',
        FieldName: 'Name',
        Placeholder: 'Nhập tên nhân viên',
        Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm nhân viên ";
    this.searchEmployeeGroup();
  }
  searchEmployeeGroup() {
    this.employeeGroupService.searchEmployeeGroups(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }


  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      EmployeeGroupId: '',
      Name: '',
      Code: '',
      Note: '',
    }
    this.searchEmployeeGroup();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchEmployeeGroup();
    }
  }

  showConfirmDeleteEmployeeGroup(EmployeeGroupId: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm nhân viên này không?").then(
      data => {
        this.deleteEmployeerGroup(EmployeeGroupId);
      },
      error => {
        
      }
    );
  }

  deleteEmployeerGroup(EmployeeGroupId: string) {
    this.employeeGroupService.deleteEmployeeGroup({ EmployeeGroupId: EmployeeGroupId}).subscribe(
      data => {
        this.searchEmployeeGroup();
        this.messageService.showSuccess('Xóa nhóm nhân viên thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(EmployeeGroupId: string) {
    let activeModal = this.modalService.open(EmployeeGroupCreateComponent, { container: 'body', windowClass: 'employee-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.EmployeeGroupId = EmployeeGroupId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchEmployeeGroup();
      }
    }, (reason) => {
    });
  }

}
