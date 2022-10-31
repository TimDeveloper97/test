import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DxTreeListComponent } from 'devextreme-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ExportAndKeepService } from 'src/app/sale/export-and-keep/service/export-and-keep.service';
import { forkJoin } from 'rxjs';
import { MessageService, ComboboxService, Constants, AppSetting } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { CustomerService } from '../../service/customer.service';
import { CustomerContactCreateComponent } from '../customer-contact-create/customer-contact-create.component';


@Component({
  selector: 'app-customer-update',
  templateUrl: './customer-update.component.html',
  styleUrls: ['./customer-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerUpdateComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    public service: ExportAndKeepService,
    private router: Router,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private CustomerService: CustomerService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService,
    public constant: Constants,
    private modalService: NgbModal,
    private route: ActivatedRoute,
  ) { }

  modalInfo = {
    SaveText: 'Lưu',
  };

  listCountry: [];
  startCustomerOfCustomerIndex = 1;
  isAction: boolean = false;
  Id: string;
  Type: string;
  listCustomerType: any[] = [];
  districts: any[] = [];
  model: any = {
    Id: '',
    CustomerTypeId: '',
    Name: '',
    Code: '',
    Address: '',
    PhoneNumber: '',
    Contact: '',
    Note: '',
    Tax: '',
    SBUId: '',
    EmployeeId: '',
    Acreage: '',
    EmployeeQuantity: '',
    EmployeeCode: '',
    EmployeeName: '',
    Capital: '',
    Field: '',
    CodeChar: 'A',
    CustomerOfCustomer: [],
    ProvinceId: '',
    ListJobGroupId: [],
  }

  ListJobGroup: any[] = [];

  customerOfCustomerModel: any = {
    Id: '',
    CountryName: '',
    Name: '',
  }


  isDropDownBoxOpened = false;

  chars: any[] = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"]
  CustomerTypeId: string;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã Lĩnh vực' }, { Name: 'Name', Title: 'Tên Lĩnh vực' }];
  columnNameSBU: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  columnNameEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  columnNameAddress: any[] = [{ Name: 'Name', Title: 'Tên' }];
  ListSBU: any = [];
  ListEmployee: any = [];

  treeBoxValue: string[];
  provinces: any[] = [];
  selectedRowKeys: any[] = [];

  ngOnInit() {
    this.Id = this.route.snapshot.paramMap.get('Id');
    this.Type = this.route.snapshot.paramMap.get('Type');
    this.getCbbData();
    if (this.Id) {
      this.CustomerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
        this.model = data;
        this.appSetting.PageTitle = "Chỉnh sửa khách hàng - " + this.model.Code + " - " + this.model.Name;

        this.modalInfo.SaveText = 'Lưu';
        this.getListCountry();
        this.getCustomerInfo();

        var userLogin = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (userLogin) {
          this.model.SBUId = userLogin.sbuId;
        }
      });
    }
  }

  getCbbData() {
    let cbbProvinces = this.combobox.getCbbProvince();

    forkJoin([
      cbbProvinces,
      this.combobox.getCbbJobGroup(),
    ]).subscribe(results => {
      this.provinces = results[0];
      this.ListJobGroup = results[1].ListResult;
    });
  }


  getDistricts(provinceId: string) {
    this.combobox.getCbbDistrict(provinceId).subscribe(
      data => {
        this.districts = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCBBSBU() {
    this.combobox.getCbbSBU().subscribe((data: any) => {
      if (data) {
        this.ListSBU = data;
      }
    });
  }
  getCBBEmployee() {
    this.combobox.getCbbEmployee().subscribe((data: any) => {
      if (data) {
        this.ListEmployee = data;
      }
    });
  }

  getListCountry() {
    this.combobox.getListCountry().subscribe((data: any) => {
      if (data) {
        this.listCountry = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  validateCustomerOfCustomer() {

    let newCustomerOfCustomerModel = Object.assign({}, this.customerOfCustomerModel);
    this.model.CustomerOfCustomer.push(newCustomerOfCustomerModel);
    this.customerOfCustomerModel = {
      Id: '',
      CountryName: '',
      Name: '',
    }

  }

  getListCustomerType() {
    this.combobox.getListCustomerType().subscribe(
      data => {
        this.listCustomerType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCustomerInfo() {
    this.CustomerService.getCustomerInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.treeBoxValue = this.model.ListJobGroupId;
      this.selectedRowKeys = this.model.ListJobGroupId;
      this.listCustomerContact = data.ListCustomerContact;
      this.getListCustomerType();
      this.getCBBSBU();
      this.getCBBEmployee();
      this.getCBBJobGroup();
    });
  }


  getCustomerCode() {
    this.CustomerService.getCustomerCode({ CodeChar: this.model.CodeChar }).subscribe(data => {
      this.model.Code = data;
    });
  }

  createCustomer(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supCreate(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supCreate(isContinue);
        },
        error => {

        }
      );
    }
  }

  deleteCustomerOfCustomer(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá khách hàng của khách hàng này không?").then(
      data => {
        if (this.model.CustomerOfCustomer.length > 0) {
          this.model.CustomerOfCustomer.splice(index, 1);
        }
      },
      error => {
      }
    );
  }

  updateCustomer() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supUpdate();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supUpdate();
        },
        error => {
        }
      );
    }
  }

  save(isContinue: boolean) {

    if (this.Id) {
      this.updateCustomer();
    }
    else {
      this.createCustomer(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.model.ListCustomerContact = this.listCustomerContact;
    this.CustomerService.createCustomer(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model.CustomerOfCustomer = this.customerOfCustomerModel;
          this.model = {
            CodeChar: 'A'
          };
          this.listCustomerContact = [];
          this.messageService.showSuccess('Thêm mới khách hàng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới khách hàng thành công!');
          this.closeModal();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.model.ListCustomerContact = this.listCustomerContact;
    this.CustomerService.updateCustomer(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật khách hàng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-khach-hang']);
  }

  modelCustomerContact: any = {
    Id: '',
    Name: '',
    PhoneNumber: '',
    Address: '',
    Note: '',
    Avatar: '',
  }
  listCustomerContact: any[] = [];

  showCreateUpdateCustomerContact(row, index, isAdd: boolean) {
    let activeModal = this.modalService.open(CustomerContactCreateComponent, { container: 'body', windowClass: 'customer-contact-create-model', backdrop: 'static' })
    activeModal.componentInstance.customerId = this.Id;
    activeModal.componentInstance.row = Object.assign({}, row);
    activeModal.componentInstance.isAdd = isAdd;
    activeModal.componentInstance.listTemp = this.listCustomerContact;
    activeModal.result.then((result) => {
      if (result.modelTemp) {
        this.getListCustomerContact(this.model.Id);
      }
    }, (reason) => {
    });
  }
  showConfirmDeleteCustomerContact(customerContactId: string, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá người liên hệ này không?").then(
      data => {
        this.checkDeleteCustomerContactId(customerContactId, index);
        //this.removeCustomerContact(index)

      },
      error => {

      }
    );
  }

  checkDeleteCustomerContactId(customerContactId, index) {
    if (customerContactId == '' || customerContactId == undefined) {
      this.removeCustomerContact(index);
    } else {
      this.CustomerService.checkDeleteCustomerContact(customerContactId).subscribe((data: any) => {
        this.removeCustomerContact(index);
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  removeCustomerContact(index) {
    this.getListCustomerContact(this.model.Id);
  }

  getListCustomerContact(customerId: string) {
    this.CustomerService.getListCustomerContact(customerId).subscribe(data => {
      this.listCustomerContact = data;
    });
  }

  saveSaleCustomer() {
    this.service.createCustomer(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Chỉnh sửa khách hàng thành công!');
        this.closeModal();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
    if (!e.value) {
      this.selectedRowKeys = [];
      e.previousValue = [];
      this.model.ListJobGroupId = [];
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys;
    this.model.ListJobGroupId = e.selectedRowKeys;
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  getCBBJobGroup() {
    this.combobox.getCbbJobGroup().subscribe((data: any) => {
      if (data) {
        this.ListJobGroup = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
}
