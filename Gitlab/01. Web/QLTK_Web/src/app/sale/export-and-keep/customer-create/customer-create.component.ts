import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerService } from 'src/app/project/service/customer.service';
import { AppSetting, ComboboxService, MessageService } from 'src/app/shared';
import { ExportAndKeepService } from '../service/export-and-keep.service';

@Component({
  selector: 'app-customer-create',
  templateUrl: './customer-create.component.html',
  styleUrls: ['./customer-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SaleCustomerCreateComponent implements OnInit {

  constructor(
    private router: Router,
    private activeModal: NgbActiveModal,
    public service: ExportAndKeepService,
    private messageService: MessageService,
    private customerService: CustomerService,
    private route: ActivatedRoute,
    public appSetting: AppSetting,
  ) { }

  CustomerTypeId: string;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnNameSBU: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  ListSBU: any = [];
  listCustomerType = [];
  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    CustomerTypeId: null,
    Name: '',
    Code: '',
    PhoneNumber: '',
    Note: '',
    SBUId: null,
    Index: 0
  }

  modalInfo:any = {
    Title:''
  }


  ngOnInit(): void {
    this.Id= this.route.snapshot.paramMap.get('Id');
    this.customerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      if(this.Id)
      {
        this.appSetting.PageTitle = "Chỉnh sửa khách hàng - " + this.model.Code + " - " + this.model.Name;
        this.getCustomerInfo();
      }
      else{
        this.appSetting.PageTitle = "Thêm mới khách hàng";
        this.getListCustomerType();
        this.generateCodeCustomer();
      }
    });
  }

  getCustomerInfo() {
    this.customerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.getListCustomerType();
    });
  }

  getListCustomerType() {
    this.service.GetCustomerTypes().subscribe(
      data => {
        this.listCustomerType = data;
        this.model.CustomerTypeId = this.listCustomerType[0].Id;
      },
    );
  }

  generateCodeCustomer() {
    this.service.GenerateCodeCustomer().subscribe(
      data => {
        this.model.Code = data.Code;
      },
    );
  }


  save() {
    this.service.createCustomer(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới khách hàng thành công!');
        this.closeModal();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-khach-hang']);
  }

}
