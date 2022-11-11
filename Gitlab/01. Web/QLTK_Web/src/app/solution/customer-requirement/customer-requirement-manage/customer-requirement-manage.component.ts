import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService, ComboboxService } from 'src/app/shared';
import { CustomerRequirementService } from '../service/customer-requirement.service';
import { CustomerRequirementCreateComponent } from '../customer-requirement-create/customer-requirement-create.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customer-requirement-manage',
  templateUrl: './customer-requirement-manage.component.html',
  styleUrls: ['./customer-requirement-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerRequirementManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private router: Router,
    public constant: Constants,
    private customerrequirementService: CustomerRequirementService,
    private comboboxService: ComboboxService,
  ) { }

  customerId = JSON.parse(localStorage.getItem('qltkcurrentUser')).customerId

  listCustomer = [];
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên yêu cầu khách hàng hoặc số YCKH',
    Items: [
      {
        Name: 'Tên khách hàng',
        FieldName: 'CustomerId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Name', Title: 'Tên khách hàng' }],
        DisplayName: 'Name',
        ValueName: 'Name',
        Placeholder: 'Chọn Tên khách hàng',
      },
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Tình trạng ',
        Type: 'select',
        Data: this.constant.CustomerRequirementStatusManger,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Code: '',
    Name:'',
    CustomerId: this.customerId,
    listCustomer: [],
    Status: null
  };

  listRequest: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý yêu cầu khách hàng";
    this.getListCustomer();
    this.search();
  }

  search() {
    this.customerrequirementService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.listRequest = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
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

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá yêu cầu khách hàng này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.customerrequirementService.delete(Id).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa yêu cầu khách hàng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(CustomerRequirementCreateComponent, { container: 'body', windowClass: 'customer-requirement-create-modal', backdrop: 'static' })
    activeModal.componentInstance.CustomerRequirementId = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
        this.getListCustomer();
      }
    }, (reason) => {
    });
  }

  showCreate() {
    this.router.navigate(['giai-phap/yeu-cau-khach-hang/them-moi']);
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Code: '',
      Name:'',
      CustomerId: this.customerId,
      listCustomer: [],
      Status: null
    }
    this.search();

  }

  checkCandidate(Id: string, CustomerId: string) {
    this.router.navigate(['giai-phap/tong-hop-bao-gia/them-moi-theo-yeu-cau-khach-hang',  Id ,  CustomerId] );
  }
}
