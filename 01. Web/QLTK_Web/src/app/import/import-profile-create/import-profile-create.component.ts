import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { element } from 'protractor';
import { AppSetting, Constants, DateUtils, MessageService } from 'src/app/shared';
import { ChooseMaterialImportPrComponent } from '../choose-material-import-pr/choose-material-import-pr.component';
import { ImportProfileService } from '../services/import-profile.service';

@Component({
  selector: 'app-import-profile-create',
  templateUrl: './import-profile-create.component.html',
  styleUrls: ['./import-profile-create.component.scss']
})
export class ImportProfileCreateComponent implements OnInit, OnDestroy {

  startIndex: number = 1;
  listData: any[] = [];
  listFile: any[] = [];

  checkedTop = false;
  listId = [];
  listFileId = [];

  profileModel = {
    Name: '',
    Code: '',
    DueDatePRV: null,
    DueDatePR: null,
    ProjectCode: '',
    ProjectName: '',
    ManufactureCode: '',
    PRCode: '',
    Index: 0,
    SupplierExpectedDateV: null,
    SupplierExpectedDate: null,
    ContractExpectedDateV: null,
    ContractExpectedDate: null,
    PayExpectedDateV: null,
    PayExpectedDate: null,
    ProductionExpectedDateV: null,
    ProductionExpectedDate: null,
    TransportExpectedDateV: null,
    TransportExpectedDate: null,
    CustomExpectedDateV: null,
    CustomExpectedDate: null,
    WarehouseExpectedDateV: null,
    WarehouseExpectedDate: null,
    ListMaterial: [],
    ListFileId: []
  };

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private importProfileService: ImportProfileService,
    public router: Router,
    private dateUtils: DateUtils,
  ) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = 'Tạo hồ sơ nhập khẩu';
    let storage = localStorage.getItem('importPrIds');
    if (storage != null) {
      this.listData = JSON.parse(storage);
      if (this.listData != null && this.listData.length > 0) {
        this.setInfo();
      }
    }
    this.generateCode();
  }

  setInfo() {
    this.profileModel.ProjectCode = '';
    this.profileModel.PRCode = '';
    this.profileModel.ManufactureCode = '';
    this.profileModel.DueDatePR = null;
    this.profileModel.DueDatePRV = null;
    this.listId = [];
    if (this.listData.length > 0) {
      let dueDatePR = this.listData[0].RequireDate;
      this.listData.forEach(element => {
        if (!this.profileModel.ProjectCode.includes(element.ProjectCode)) {
          this.profileModel.ProjectCode += (this.profileModel.ProjectCode ? ',' : '') + element.ProjectCode;
        }

        if (!this.profileModel.ProjectName.includes(element.ProjectName)) {
          this.profileModel.ProjectName += (this.profileModel.ProjectName ? ',' : '') + element.ProjectName;
        }

        if (!this.profileModel.PRCode.includes(element.PurchaseRequestCode)) {
          this.profileModel.PRCode += (this.profileModel.PRCode ? ',' : '') + element.PurchaseRequestCode;
        }

        if (!this.profileModel.ManufactureCode.includes(element.ManufactureCode)) {
          this.profileModel.ManufactureCode += (this.profileModel.ManufactureCode ? ',' : '') + element.ManufactureCode;
        }

        if (dueDatePR > element.RequireDate) {
          dueDatePR = element.RequireDate;
        }

        this.listId.push(element.Id);
        
      });
      
      this.profileModel.DueDatePRV = this.dateUtils.convertDateToObject(dueDatePR);
      this.profileModel.Name = this.profileModel.ManufactureCode + '-' + this.profileModel.ProjectCode;
    }

  }

  generateCode() {
    this.importProfileService.getImportProfileCode().subscribe((data: any) => {
      if (data) {
        this.profileModel.Code = data.Code;
        this.profileModel.Index = data.Index;
        this.listFile = data.ListFile;
      }
    });
  }

  ngOnDestroy() {
    localStorage.removeItem('importPrIds');
  }

  chooseMaterialImportPR() {
    let activeModal = this.modalService.open(ChooseMaterialImportPrComponent, { container: 'body', windowClass: 'choose-material-import-pr', backdrop: 'static' });
    activeModal.componentInstance.ListId = this.listId;
    activeModal.result.then((result) => {
      if (result) {
        result.forEach(element => {
          this.listData.push(element);
        });
        this.setInfo();
      }
    }, (reason) => {

    });
  }

  showConfirmDelete(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư này không?").then(
      data => {
        this.listData.splice(index,1);
        this.setInfo();
      }
    );
  }

  save() {
    this.profileModel.DueDatePR = this.dateUtils.convertObjectToDate(this.profileModel.DueDatePRV);
    this.profileModel.ListMaterial = this.listData;
    this.profileModel.ListFileId = this.listFileId;

    this.profileModel.SupplierExpectedDate = null;
    if (this.profileModel.SupplierExpectedDateV) {
      this.profileModel.SupplierExpectedDate = this.dateUtils.convertObjectToDate(this.profileModel.SupplierExpectedDateV);
    }

    this.profileModel.ContractExpectedDate = null;
    if (this.profileModel.ContractExpectedDateV) {
      this.profileModel.ContractExpectedDate = this.dateUtils.convertObjectToDate(this.profileModel.ContractExpectedDateV);
    }

    this.profileModel.PayExpectedDate = null;
    if (this.profileModel.PayExpectedDateV) {
      this.profileModel.PayExpectedDate = this.dateUtils.convertObjectToDate(this.profileModel.PayExpectedDateV);
    }

    this.profileModel.ProductionExpectedDate = null;
    if (this.profileModel.ProductionExpectedDateV) {
      this.profileModel.ProductionExpectedDate = this.dateUtils.convertObjectToDate(this.profileModel.ProductionExpectedDateV);
    }

    this.profileModel.TransportExpectedDate = null;
    if (this.profileModel.TransportExpectedDateV) {
      this.profileModel.TransportExpectedDate = this.dateUtils.convertObjectToDate(this.profileModel.TransportExpectedDateV);
    }

    this.profileModel.CustomExpectedDate = null;
    if (this.profileModel.CustomExpectedDateV) {
      this.profileModel.CustomExpectedDate = this.dateUtils.convertObjectToDate(this.profileModel.CustomExpectedDateV);
    }

    this.profileModel.WarehouseExpectedDate = null;
    if (this.profileModel.WarehouseExpectedDateV) {
      this.profileModel.WarehouseExpectedDate = this.dateUtils.convertObjectToDate(this.profileModel.WarehouseExpectedDateV);
    }

    this.importProfileService.createImportProfile(this.profileModel).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới hồ sơ thành công!');
        localStorage.removeItem('importPrIds');
        this.router.navigate(['nhap-khau/ho-so-nhap-khau']);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  close() {
    localStorage.removeItem("importPrIds");
    this.router.navigate(['nhap-khau/ho-so-nhap-khau']);
  }

  checkAll() {    
    this.listFileId = [];
    this.listFile.forEach(element => {
      if (this.checkedTop) {
        element.Checked = true;
        this.listFileId.push(element.Id);
      } else {
        element.Checked = false;
      }
    });
  }

  checkItem(row, index) {
    if (this.listFile.filter(item => { if (!item.Checked) { return item } }).length == 0) {
      this.checkedTop = true;
      this.listFileId.push(row.Id);
    }
    else {
      this.checkedTop = false;
      if(this.listFileId.filter(t=>t == row.Id).length == 0)
      {
        this.listFileId.push(row.Id);
      }
      else{
        this.listFileId.splice(index,1);
      }
    }
  }
}
