import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Constants, MessageService, AppSetting } from 'src/app/shared';
import { CustomerTypeService } from '../../service/customer-type.service';
import { CustomerTypeCreateComponent } from '../customer-type-create/customer-type-create.component';

@Component({
  selector: 'app-customer-type-manage',
  templateUrl: './customer-type-manage.component.html',
  styleUrls: ['./customer-type-manage.component.scss']
})
export class CustomerTypeManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private customerTypeservice: CustomerTypeService,
    public constant: Constants
  ) { }

  startIndex = 0;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã nhóm khách hàng',
    Items: [
      {
        Name: 'Tên nhóm khách hàng',
        FieldName: 'Name',
        Placeholder: 'Nhập tên khách hàng',
        Type: 'text'
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý loại khách hàng";
    this.searchCustomerType();
  }

  searchCustomerType() {
    this.customerTypeservice.searchCustomerType(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
    }
    this.searchCustomerType();
  }

  showConfirmDeleteCustomerType(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá loại khách hàng này không?").then(
      data => {
        this.deleteCustomerType(Id);
      },
      error => {
        
      }
    );
  }

  deleteCustomerType(Id: string) {
    this.customerTypeservice.deleteCustomerType({ Id: Id }).subscribe(
      data => {
        this.searchCustomerType();
        this.messageService.showSuccess('Xóa loại khách hàng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(CustomerTypeCreateComponent, { container: 'body', windowClass: 'customer-type-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchCustomerType();
      }
    }, (reason) => {
    });
  }

}
