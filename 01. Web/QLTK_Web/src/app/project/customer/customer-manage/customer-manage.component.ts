import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

import { AppSetting, MessageService, Constants, Configuration, ComboboxService, ComponentService } from 'src/app/shared';
import { DxTreeListComponent } from 'devextreme-angular';

import { CustomerService } from '../../service/customer.service';
import { CustomerTypeService } from '../../service/customer-type.service';
import { CustomerTypeCreateComponent } from '../../customer-type/customer-type-create/customer-type-create.component';
import { CustomerCreateComponent } from '../customer-create/customer-create.component';
import { SaleCustomerCreateComponent } from 'src/app/sale/export-and-keep/customer-create/customer-create.component';
import { ImportFileComponent } from '../import-file/import-file.component';

@Component({
  selector: 'app-customer-manage',
  templateUrl: './customer-manage.component.html',
  styleUrls: ['./customer-manage.component.scss']
})
export class CustomerManageComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private titleservice: Title,
    private customerservice: CustomerService,
    private customertypeservice: CustomerTypeService,
    private combobox: ComboboxService,
    public constant: Constants,
    private componentService: ComponentService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.items = [
      { Id: 3, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 1, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times text-danger' },

    ];
  }

  startIndex = 0;
  height = 0;
  items: any;
  listData: any[] = [];
  listCustomerType: any[] = [];
  listCustomerTypeId = [];
  selectedCustomerTypeId = '';
  customerTypeId: '';
  customerType_Type = 0;
  fileTemplate = this.config.ServerApi + 'Template/Khách hàng_Template.xls';
  sbuid = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Address: '',
    CustomerTypeId: '',
    ListCustomerContact: [],
    SBUId: ''
  }

  customerTypeModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
  }

  modelAll: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã hoặc tên khách hàng',
    Items: [
      // {
      //   Name: 'Tên khách hàng',
      //   FieldName: 'Name',
      //   Placeholder: 'Nhập tên khách hàng',
      //   Type: 'text'
      // },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn SBU',
        Permission: ['F060205'],
      },
    ]
  };

  Type: string;

  ngOnInit() {
    this.height = window.innerHeight - 120;
    this.appSetting.PageTitle = "Quản lý Khách hàng";
    this.model.SBUId = this.sbuid;
    this.searchCustomerType();
    this.searchCustomer("");
    this.selectedCustomerTypeId = localStorage.getItem("selectedCustomerTypeId");
    localStorage.removeItem("selectedCustomerTypeId");
  }

  itemClick(e) {
    if (this.customerTypeId == '' || this.customerTypeId == null) {
      this.messageService.showMessage("Đây không phải loại khách hàng!")
    } else {
      if (e.itemData.Id == 3) {
        this.showCreateUpdateCustomerType(this.customerTypeId, false);
      }
      if (e.itemData.Id == 1) {
        this.showCreateUpdateCustomerType(this.customerTypeId, true);
      }
      else if (e.itemData.Id == 2) {
        this.showConfirmDeleteCustomerType(this.customerTypeId);
      }
    }
  }

  onSelectionChanged(e) {
    // this.selectedCustomerTypeId = e.selectedRowKeys[0];
    // this.searchCustomer(e.selectedRowKeys[0]);
    // this.customerTypeId = e.selectedRowKeys[0];

    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedCustomerTypeId) {
      this.selectedCustomerTypeId = e.selectedRowKeys[0];
      this.searchCustomer(e.selectedRowKeys[0]);
      this.customerTypeId = e.selectedRowKeys[0];

      this.customerType_Type =  e.selectedRowsData[0].Type;
    }
  }

  searchCustomerType() {
    this.combobox.getListCustomerType().subscribe((data: any) => {
      if (data) {
        this.listCustomerType = data;
        this.customerTypeModel.TotalItems = data.length;
        this.listCustomerType.unshift(this.modelAll);

        if (this.selectedCustomerTypeId == null) {
          this.selectedCustomerTypeId = this.listCustomerType[0].Id;
        }
        this.treeView.selectedRowKeys = [this.selectedCustomerTypeId];
        for (var item of this.listCustomerType) {
          this.listCustomerTypeId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchCustomer(CustomerTypeId: string) {
    this.model.CustomerTypeId = CustomerTypeId;
    this.customerservice.searchCustomer(this.model).subscribe((data: any) => {
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

  clear(CustomerTypeId: string) {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
      Address: '',
      CustomerTypeId: '',
    }
    this.listCustomerType = [];
    this.listCustomerTypeId = [];
    this.selectedCustomerTypeId = '';
    CustomerTypeId = '';
    this.searchCustomerType();
    this.searchCustomer("");
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
    this.customertypeservice.deleteCustomerType({ Id: Id }).subscribe(
      data => {
        this.selectedCustomerTypeId = '';
        this.searchCustomerType();
        this.searchCustomer("");
        this.messageService.showSuccess('Xóa loại khách thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteCustomer(Id: string, CustomerTypeId: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá khách hàng này không?").then(
      data => {
        this.deleteCustomer(Id, CustomerTypeId);
      },
      error => {
        
      }
    );
  }

  deleteCustomer(Id: string, CustomerTypeId: string) {
    this.customerservice.deleteCustomer({ Id: Id }).subscribe(
      data => {
        this.searchCustomer(CustomerTypeId);
        this.messageService.showSuccess('Xóa khách hàng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel() {
    this.customerservice.exportExcel(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showCreateUpdate(Id: string, Type: number) {
    if(Type != 1)
    {
      let activeModal = this.modalService.open(CustomerCreateComponent, { container: 'body', windowClass: 'customer-model', backdrop: 'static' })
      activeModal.componentInstance.Id = Id;
      if (this.selectedCustomerTypeId != null) {
        activeModal.componentInstance.CustomerTypeId = this.selectedCustomerTypeId;
      }
      activeModal.result.then((result) => {
        if (result) {
          this.searchCustomer(this.customerTypeId);
        }
      }, (reason) => {
      });
    }
    else{
      let activeModal = this.modalService.open(SaleCustomerCreateComponent, { container: 'body', windowClass: 'customer-create-model', backdrop: 'static' })
      activeModal.componentInstance.Id = Id;
      if (this.selectedCustomerTypeId != null) {
        activeModal.componentInstance.CustomerTypeId = this.selectedCustomerTypeId;
      }
      activeModal.result.then((result) => {
        if (result) {
          this.searchCustomer(this.customerTypeId);
        }
      }, (reason) => {
      });
    }
  }

  showCreateUpdateCustomerType(Id: string,  isUpdate: boolean) {
    let activeModal = this.modalService.open(CustomerTypeCreateComponent, { container: 'body', windowClass: 'customer-type-model', backdrop: 'static' })
    if(isUpdate){
      activeModal.componentInstance.Id = Id;
    }
    else {
      activeModal.componentInstance.parentId = Id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchCustomerType();
        this.selectedCustomerTypeId = result.Id;
      }
    }, (reason) => {
    });
  }

  showImportPopup() {
    this.customerservice.getGroupInTemplate().subscribe(d => {
      this.fileTemplate = this.config.ServerApi + d;
      this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
        if (data) {
          this.customerservice.importFile(data).subscribe(
            data => {
              this.searchCustomer(this.customerTypeId);
              this.messageService.showSuccess('Import khách hàng thành công!');
            },
            error => {
              this.messageService.showError(error);
            });
        }
      });
    }, e => {
      this.messageService.showError(e);
    });
  }

  showUpdate(Id: string, Type: number) {
    if(Type != 1)
    {
      this.router.navigate(['/du-an/quan-ly-khach-hang/customer/chinh-sua/0/', Id]);
     
    }
    else{
      this.router.navigate(['/du-an/quan-ly-khach-hang/customer/chinh-sua/1/', Id]);
    }
  }

  showCreate(){
      this.router.navigate(['du-an/quan-ly-khach-hang/them-moi']);
  }
}
