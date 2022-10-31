import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { NumberValueAccessor } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ProductStandardTpaService } from 'src/app/device/services/product-standard-tpa.service';
import { SupplierCreateComponent } from 'src/app/material/supplier/supplier-create/supplier-create.component';
import { AppSetting, Constants, DateUtils, FileProcess, MessageService, Configuration, ComboboxService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ChooseMaterialImportPrComponent } from '../choose-material-import-pr/choose-material-import-pr.component';
import { ImportProfileService } from '../services/import-profile.service';

@Component({
  selector: 'app-import-profile-update',
  templateUrl: './import-profile-update.component.html',
  styleUrls: ['./import-profile-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ImportProfileUpdateComponent implements OnInit, AfterViewInit, OnDestroy {

  startIndex: number = 1;

  listFile: any[] = [];

  checkedTop = false;
  listSelect = [];

  ChangeStepModel = {
    Id: ''
  }

  exportListFileModel = {
    Id: '',
    Step: 0
  }

  listFileModel: any = { ListDatashet: [] }

  profileModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Step: 0,
    PRDueDate: null,
    PRDueDateV: null,
    ProjectCode: '',
    ProjectName: '',
    ManufacturerCode: '',
    PRCode: '',
    Amount: 0,
    CustomsSupplierId: null,
    ListMaterial: [],
    ListDocumentStep1: [],
    ListDocumentStep2: [],
    ListDocumentStep3: [],
    ListDocumentStep4: [],
    ListDocumentStep5: [],
    ListDocumentStep6: [],
    ListDocumentStep7: [],
    ListDocumentOtherStep1: [],
    ListDocumentOtherStep2: [],
    ListDocumentOtherStep3: [],
    ListDocumentOtherStep4: [],
    ListDocumentOtherStep5: [],
    ListDocumentOtherStep6: [],
    ListDocumentOtherStep7: [],
    ListPayment: [],
    ListProblem: [],
    ListTransportSupplier: []
  };

  payModel = {
    Id: '',
    Index: 0,
    ImportProfileId: '',
    PercentPayment: 0,
    Money: 0,
    Duedate: null,
    DuedateV: null,
    Status: 1,
    Note: '',
    MoneyTransferPath: '',
    MoneyTransferName: '',
    Content: '',
    CurrencyUnit: 4
  }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  payTemplate: any = {
    Index: 1,
    PercentPayment: 0,
    Money: 0,
    DueDate: null,
    Status: 1,
    Note: '',
    CurrencyUnit: 4
  }

  columnNameSupplier: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name_NCC_SX', Title: 'Tên nhóm' }];
  columnNameSupplierService: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  listSupplier = [];
  supplierServices = [];
  userLogin: any = {};
  quoteTemplate: any = {
    Id: '',
    Code: '',
    Quotedate: null,
    Effect: 15,
    Note: null,
    SupplierId: null,
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  quoteTransportTemplate: any = {
    Id: '',
    Code: '',
    Quotedate: null,
    Effect: 15,
    Note: null,
    TransportSupplierId: null,
    TransportLeadtime: null,
    ShippingCost: 0,
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  listId = [];

  fileOtherTemplate: any = {
    Id: '',
    Step: 1,
    Note: null,
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  documentSelect: any = {};
  fileUpload: any[] = [];
  validMessage: any = '';
  listReportProplem: any[] = [];

  @ViewChild('fileInputDocument') inputFileCOCQ: ElementRef
  @ViewChild('scrollHeader', { static: false }) scrollHeader: ElementRef;
  @ViewChild('scrollProduct', { static: false }) scrollProduct: ElementRef;
  @ViewChild('scrollFooter', { static: false }) scrollFooter: ElementRef;

  //@ViewChild('scrollCustomHeader', { static: false }) scrollCustomHeader: ElementRef;
  // @ViewChild('scrollCustomProduct', { static: false }) scrollCustomProduct: ElementRef;
  //@ViewChild('scrollCustomFooter', { static: false }) scrollCustomFooter: ElementRef;

  scrollCustomProduct: HTMLElement = null;
  scrollCustomHeader: HTMLElement = null;
  scrollCustomFooter: HTMLElement = null;

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private routeA: ActivatedRoute,
    private dateUtils: DateUtils,
    private modalService: NgbModal,
    private importProfileService: ImportProfileService,
    private service: ProductStandardTpaService,
    public router: Router,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private config: Configuration,
    private comboboxService: ComboboxService,
  ) {
    this.userLogin = JSON.parse(localStorage.getItem('qltkcurrentUser'));
  }

  ngOnInit(): void {
    this.quoteTemplate.UploadName = this.userLogin.userfullname;
    this.fileOtherTemplate.UploadName = this.userLogin.userfullname;

    this.profileModel.Id = this.routeA.snapshot.paramMap.get('Id');
    this.appSetting.PageTitle = 'Chỉnh sửa hồ sơ nhập khẩu';
    this.getById();
    this.getlistSupplier();
    this.getSupplierServices();
  }

  ngAfterViewInit() {
    this.scrollProduct.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      this.scrollFooter.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollProduct.nativeElement.removeEventListener('ps-scroll-x', null);

    this.scrollCustomProduct = document.getElementById("scrollCustomProduct");
    if (this.scrollCustomProduct) {
      this.scrollCustomProduct.removeEventListener('ps-scroll-x', null);
    }
  }

  getById() {
    this.ChangeStepModel.Id = this.profileModel.Id;
    this.importProfileService.getById(this.ChangeStepModel).subscribe(
      data => {
        this.profileModel = data;
        this.searchImportProfileProblemExist(this.profileModel.Step);
        if (this.profileModel.PRDueDate) {
          this.profileModel.PRDueDateV = this.dateUtils.convertDateToObject(this.profileModel.PRDueDate);
        }

        if (this.profileModel.QuoteDate) {
          this.profileModel.QuoteDateV = this.dateUtils.convertDateToObject(this.profileModel.QuoteDate);
        }

        if (this.profileModel.EstimatedDeliveryDate) {
          this.profileModel.EstimatedDeliveryDateV = this.dateUtils.convertDateToObject(this.profileModel.EstimatedDeliveryDate);
        }

        if (this.profileModel.WarehouseDate) {
          this.profileModel.WarehouseDateV = this.dateUtils.convertDateToObject(this.profileModel.WarehouseDate);
        }

        if (this.profileModel.CustomsDeclarationFormDate) {
          this.profileModel.CustomsDeclarationFormDateV = this.dateUtils.convertDateToObject(this.profileModel.CustomsDeclarationFormDate);
        }

        if (this.profileModel.CustomsClearanceFromDate) {
          this.profileModel.CustomsClearanceFromDateV = this.dateUtils.convertDateToObject(this.profileModel.CustomsClearanceFromDate);
        }

        if (this.profileModel.SupplierExpectedDate) {
          this.profileModel.SupplierExpectedDateV = this.dateUtils.convertDateToObject(this.profileModel.SupplierExpectedDate);
        }

        if (this.profileModel.ContractExpectedDate) {
          this.profileModel.ContractExpectedDateV = this.dateUtils.convertDateToObject(this.profileModel.ContractExpectedDate);
        }

        if (this.profileModel.PayExpectedDate) {
          this.profileModel.PayExpectedDateV = this.dateUtils.convertDateToObject(this.profileModel.PayExpectedDate);
        }

        if (this.profileModel.ProductionExpectedDate) {
          this.profileModel.ProductionExpectedDateV = this.dateUtils.convertDateToObject(this.profileModel.ProductionExpectedDate);
        }

        if (this.profileModel.ProductionExpectedDate1) {
          this.profileModel.ProductionExpectedDate1V = this.dateUtils.convertDateToObject(this.profileModel.ProductionExpectedDate1);
        }

        if (this.profileModel.ProductionExpectedDate2) {
          this.profileModel.ProductionExpectedDate2V = this.dateUtils.convertDateToObject(this.profileModel.ProductionExpectedDate2);
        }

        if (this.profileModel.TransportExpectedDate) {
          this.profileModel.TransportExpectedDateV = this.dateUtils.convertDateToObject(this.profileModel.TransportExpectedDate);
        }

        if (this.profileModel.CustomExpectedDate) {
          this.profileModel.CustomExpectedDateV = this.dateUtils.convertDateToObject(this.profileModel.CustomExpectedDate);
        }

        if (this.profileModel.WarehouseExpectedDate) {
          this.profileModel.WarehouseExpectedDateV = this.dateUtils.convertDateToObject(this.profileModel.WarehouseExpectedDate);
        }

        if (this.profileModel.SupplierFinishDate) {
          this.profileModel.SupplierFinishDateV = this.dateUtils.convertDateToObject(this.profileModel.SupplierFinishDate);
        }

        if (this.profileModel.ContractFinishDate) {
          this.profileModel.ContractFinishDateV = this.dateUtils.convertDateToObject(this.profileModel.ContractFinishDate);
        }

        if (this.profileModel.PayFinishDate) {
          this.profileModel.PayFinishDateV = this.dateUtils.convertDateToObject(this.profileModel.PayFinishDate);
        }

        if (this.profileModel.ProductionFinishDate) {
          this.profileModel.ProductionFinishDateV = this.dateUtils.convertDateToObject(this.profileModel.ProductionFinishDate);
        }

        if (this.profileModel.TransportFinishDate) {
          this.profileModel.TransportFinishDateV = this.dateUtils.convertDateToObject(this.profileModel.TransportFinishDate);
        }

        if (this.profileModel.CustomFinishDate) {
          this.profileModel.CustomFinishDateV = this.dateUtils.convertDateToObject(this.profileModel.CustomFinishDate);
        }

        if (this.profileModel.WarehouseFinishDate) {
          this.profileModel.WarehouseFinishDateV = this.dateUtils.convertDateToObject(this.profileModel.WarehouseFinishDate);
        }

        this.profileModel.ListDocumentStep1.forEach(item => {
          if (item.QuoteDate) {
            item.QuoteDateV = this.dateUtils.convertDateToObject(item.QuoteDate);
          }
        });

        this.constant.ImportStep.forEach((item, index) => {
          if (item.Id == this.profileModel.Step) {
            this.indexNumber = index;

            if (this.profileModel.Step == 6) {
              this.setScrollCustoms();
            }
          }
        })

        if (this.profileModel.ListPayment.length > 0) {
          this.profileModel.ListPayment.forEach(item => {
            if (item.Duedate) {
              item.DuedateV = this.dateUtils.convertDateToObject(item.Duedate);
            }
          });
        }

        if (this.profileModel.ListTransportSupplier.length > 0) {
          this.profileModel.ListTransportSupplier.forEach(item => {
            if (item.QuoteDate) {
              item.QuoteDateV = this.dateUtils.convertDateToObject(item.QuoteDate);
            }
          });
        }

        if (this.profileModel.ListMaterial.length > 0) {
          let amount = 0;
          let isAmount = false;
          this.profileModel.ListMaterial.forEach(item => {
            if (item.LeadTime) {
              item.LeadTimeV = this.dateUtils.convertDateToObject(item.LeadTime);
            }

            if (item.Price > 0 && item.Amount > 0) {
              isAmount = true;
              // let amount = Number(item.Price) * Number(item.Quantity);

              // item.Amount = amount + ((Number(item.ImportTax) * amount) / 100) + ((Number(item.VATTax) * amount) / 100) + ((Number(item.OtherTax) * amount) / 100);
              // item.Amount = Math.round((item.Amount + Number.EPSILON) * 100) / 100
            }
          });

          if (isAmount) {
            this.updateAmount();
          }
        }

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(type: number) {
    this.profileModel.PRDueDate = this.dateUtils.convertObjectToDate(this.profileModel.PRDueDateV);

    this.profileModel.QuoteDate = null;
    if (this.profileModel.QuoteDateV) {
      this.profileModel.QuoteDate = this.dateUtils.convertObjectToDate(this.profileModel.QuoteDateV);
    }

    this.profileModel.EstimatedDeliveryDate = null;
    if (this.profileModel.EstimatedDeliveryDateV) {
      this.profileModel.EstimatedDeliveryDate = this.dateUtils.convertObjectToDate(this.profileModel.EstimatedDeliveryDateV);
    }

    this.profileModel.CustomsDeclarationFormDate = null;
    if (this.profileModel.CustomsDeclarationFormDateV) {
      this.profileModel.CustomsDeclarationFormDate = this.dateUtils.convertObjectToDate(this.profileModel.CustomsDeclarationFormDateV);
    }

    this.profileModel.CustomsClearanceFromDate = null;
    if (this.profileModel.CustomsClearanceFromDateV) {
      this.profileModel.CustomsClearanceFromDate = this.dateUtils.convertObjectToDate(this.profileModel.CustomsClearanceFromDateV);
    }

    this.profileModel.WarehouseDate = null;
    if (this.profileModel.WarehouseDateV) {
      this.profileModel.WarehouseDate = this.dateUtils.convertObjectToDate(this.profileModel.WarehouseDateV);
    }

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

    this.profileModel.ProductionExpectedDate1 = null;
    if (this.profileModel.ProductionExpectedDate1V) {
      this.profileModel.ProductionExpectedDate1 = this.dateUtils.convertObjectToDate(this.profileModel.ProductionExpectedDate1V);
    }

    this.profileModel.ProductionExpectedDate2 = null;
    if (this.profileModel.ProductionExpectedDate2V) {
      this.profileModel.ProductionExpectedDate2 = this.dateUtils.convertObjectToDate(this.profileModel.ProductionExpectedDate2V);
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

    if (this.profileModel.ListMaterial.length > 0) {
      this.profileModel.ListMaterial.forEach(item => {
        item.LeadTime = null;
        if (item.LeadTimeV) {
          item.LeadTime = this.dateUtils.convertObjectToDate(item.LeadTimeV);
        }
      });
    }

    if (this.profileModel.ListPayment.length > 0) {
      this.profileModel.ListPayment.forEach(item => {
        item.Duedate = null;
        if (item.DuedateV) {
          item.Duedate = this.dateUtils.convertObjectToDate(item.DuedateV);
        }
      });
    }

    if (this.profileModel.ListTransportSupplier.length > 0) {
      this.profileModel.ListTransportSupplier.forEach(item => {
        item.QuoteDate = null;
        if (item.QuoteDateV) {
          item.QuoteDate = this.dateUtils.convertObjectToDate(item.QuoteDateV);
        }
      });
    }
    

    if (this.profileModel.ListDocumentStep1.length > 0) {
      this.profileModel.ListDocumentStep1.forEach(item => {
        item.QuoteDate = null;
        if (item.QuoteDateV) {
          item.QuoteDate = this.dateUtils.convertObjectToDate(item.QuoteDateV);
        }
      });
    }

    this.fileUpload = [];
    this.setFileUpload(this.profileModel.ListDocumentOtherStep1, 'O-1');
    this.setFileUpload(this.profileModel.ListDocumentOtherStep2, 'O-2');
    this.setFileUpload(this.profileModel.ListDocumentOtherStep3, 'O-3');
    this.setFileUpload(this.profileModel.ListDocumentOtherStep4, 'O-4');
    this.setFileUpload(this.profileModel.ListDocumentOtherStep5, 'O-5');
    this.setFileUpload(this.profileModel.ListDocumentOtherStep6, 'O-6');
    this.setFileUpload(this.profileModel.ListDocumentOtherStep7, 'O-7');

    this.setFileUpload(this.profileModel.ListDocumentStep1, 'D-1');
    this.setFileUpload(this.profileModel.ListDocumentStep2, 'D-2');
    this.setFileUpload(this.profileModel.ListDocumentStep3, 'D-3');
    this.setFileUpload(this.profileModel.ListDocumentStep4, 'D-4');
    this.setFileUpload(this.profileModel.ListDocumentStep5, 'D-5');
    this.setFileUpload(this.profileModel.ListDocumentStep6, 'D-6');
    this.setFileUpload(this.profileModel.ListDocumentStep7, 'D-7');

    this.uploadService.uploadListFile(this.fileUpload, 'ImportProfile/').subscribe((event: any) => {
      this.fileUpload.forEach((item, index) => {
        if (item.Type == 'O-1') {
          this.profileModel.ListDocumentOtherStep1[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'O-2') {
          this.profileModel.ListDocumentOtherStep2[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'O-3') {
          this.profileModel.ListDocumentOtherStep3[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'O-4') {
          this.profileModel.ListDocumentOtherStep4[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'O-5') {
          this.profileModel.ListDocumentOtherStep5[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'O-6') {
          this.profileModel.ListDocumentOtherStep6[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'O-7') {
          this.profileModel.ListDocumentOtherStep7[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'D-1') {
          this.profileModel.ListDocumentStep1[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'D-2') {
          this.profileModel.ListDocumentStep2[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'D-3') {
          this.profileModel.ListDocumentStep3[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'D-4') {
          this.profileModel.ListDocumentStep4[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'D-5') {
          this.profileModel.ListDocumentStep5[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'D-6') {
          this.profileModel.ListDocumentStep6[item.Index].FilePath = event[index].FileUrl;
        }
        else if (item.Type == 'D-7') {
          this.profileModel.ListDocumentStep7[item.Index].FilePath = event[index].FileUrl;
        }
      });

      if (type == 1) {
        this.importProfileService.updateImportProfile(this.profileModel).subscribe(
          data => {
            this.messageService.showSuccess('Cập nhật hồ sơ thành công!');
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
      else {
        this.importProfileService.nextStep(this.profileModel).subscribe(
          data => {
            this.getById();
            this.messageService.showSuccess('Kết thúc quy trình thành công!');
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  setFileUpload(files, type) {
    files.forEach((item, index) => {
      if (item.File) {
        item.File.Index = index;
        item.File.Type = type;
        this.fileUpload.push(item.File);
      }
    });
  }

  nextStep() {
    let valid = this.validNextStep();
    if (valid) {
      this.messageService.showConfirm("Bạn có chắc muốn kết thúc quy trình hiện tại không?").then(
        data => {
          this.save(2);
        }
      );
    }
    else {
      this.messageService.showMessage(this.validMessage);
    }
  }

  backStep() {
    this.ChangeStepModel.Id = this.profileModel.Id;
    this.messageService.showConfirm("Bạn có chắc muốn trở về quy trình trước không?").then(
      data => {
        this.importProfileService.backStep(this.ChangeStepModel).subscribe(
          data => {
            this.getById();
            this.messageService.showSuccess('Quay lại quy trình thành công!');
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
    );
  }

  chooseMaterialImportPR() {
    this.listId = [];
    if (this.profileModel.ListMaterial.length > 0) {
      this.profileModel.ListMaterial.forEach(item => {
        this.listId.push(item.Id);
      });
    }

    let activeModal = this.modalService.open(ChooseMaterialImportPrComponent, { container: 'body', windowClass: 'choose-material-import-pr', backdrop: 'static' });
    activeModal.componentInstance.ListId = this.listId;
    activeModal.result.then((result) => {
      if (result) {
        result.forEach(element => {
          this.profileModel.ListMaterial.push(element);
        });
        this.setInfo();
      }
    }, (reason) => {

    });
  }

  setInfo() {
    this.profileModel.ProjectCode = '';
    this.profileModel.PRCode = '';
    this.profileModel.ManufacturerCode = '';
    this.profileModel.PRDueDate = null;
    if (this.profileModel.ListMaterial.length > 0) {
      let dueDatePR = this.profileModel.ListMaterial[0].RequireDate;
      this.profileModel.ListMaterial.forEach(element => {
        if (!this.profileModel.ProjectCode.includes(element.ProjectCode)) {
          this.profileModel.ProjectCode += (this.profileModel.ProjectCode ? ',' : '') + element.ProjectCode;
        }

        if (!this.profileModel.ProjectName.includes(element.ProjectName)) {
          this.profileModel.ProjectName += (this.profileModel.ProjectName ? ',' : '') + element.ProjectName;
        }

        if (!this.profileModel.PRCode.includes(element.PurchaseRequestCode)) {
          this.profileModel.PRCode += (this.profileModel.PRCode ? ',' : '') + element.PurchaseRequestCode;
        }

        if (!this.profileModel.ManufacturerCode.includes(element.ManufactureCode)) {
          this.profileModel.ManufacturerCode += (this.profileModel.ManufacturerCode ? ',' : '') + element.ManufactureCode;
        }

        if (dueDatePR > element.RequireDate) {
          dueDatePR = element.RequireDate;
        }

        this.listId.push(element.Id);
      });
      this.profileModel.PRDueDate = this.dateUtils.convertDateToObject(dueDatePR);
    }

  }

  indexNumber = 0;
  seclect(item: any, index: number) {
    if (item.Id <= this.profileModel.Step) {
      this.indexNumber = index;
      this.searchImportProfileProblemExist(item.Id);
    }

    if (item.Id == 6) {
      this.setScrollCustoms();
      this.searchImportProfileProblemExist(6);
    }
  }

  setScrollCustoms() {
    setTimeout(() => {
      this.scrollCustomProduct = document.getElementById("scrollCustomProduct");
      this.scrollCustomProduct.removeEventListener('ps-scroll-x', null);

      this.scrollCustomHeader = document.getElementById("scrollCustomHeader");

      this.scrollCustomProduct.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollCustomHeader.scrollLeft = event.target.scrollLeft;
      }, true);

    }, 1000);
  }

  checkParameter = true;
  addRow() {
    if (this.payTemplate.Content && this.payTemplate.PercentPayment > 0 && this.payTemplate.DueDate) {
      var pay = Object.assign({}, this.payModel);

      pay.PercentPayment = this.payTemplate.PercentPayment;
      pay.Money = this.payTemplate.Money;
      pay.ImportProfileId = this.profileModel.Id;
      if (this.payTemplate.DueDate != null) {
        pay.DuedateV = this.payTemplate.DueDate;
      }
      pay.Status = this.payTemplate.Status;
      pay.Content = this.payTemplate.Content;
      pay.Note = this.payTemplate.Note;
      pay.CurrencyUnit = this.payTemplate.CurrencyUnit;
      this.profileModel.ListPayment.push(pay);

      this.payTemplate.PercentPayment = 0;
      this.payTemplate.Money = 0;
      this.payTemplate.DueDate = null;
      this.payTemplate.Status = 1;
      this.payTemplate.Note = '';
      this.payTemplate.Content = '';
      this.payTemplate.CurrencyUnit = 4;
    }
    else {
      this.messageService.showMessage("Bạn chưa điền đủ thông tin thanh toán!");
    }
  }

  showConfirmDelete(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư này không?").then(
      data => {
        this.profileModel.ListMaterial.splice(index, 1);
        this.setInfo();
      }
    );
  }

  getlistSupplier() {
    this.service.GetSuppliers().subscribe((data: any) => {
      if (data) {
        this.listSupplier = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getSupplierServices() {
    this.comboboxService.getCBBSupplierService().subscribe((data: any) => {
      if (data) {
        this.supplierServices = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }


  chooseFileDocument(row, index) {
    this.documentSelect = row;
    this.inputFileCOCQ.nativeElement.click();
  }

  uploadFileDocument($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    this.documentSelect.File = fileDataSheet[0];
    this.documentSelect.FileName = fileDataSheet[0].name;
    this.documentSelect.FileSize = fileDataSheet[0].size;
    this.documentSelect.UploadDate = new Date();
    this.documentSelect.UploadName = this.userLogin.userfullname;
  }

  deletePayment(i) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá lần thanh toán này?").then(
      result => {
        if (result) {
          this.profileModel.ListPayment.splice(i, 1);
          this.messageService.showSuccess("Xóa giá trị thông số vật tư thành công!");
        }
      },
      error => {

      }
    );
  }

  changePercentPay(payment) {
    payment.Money = (Number(payment.PercentPayment) * this.profileModel.Amount) / 100;
  }

  uploadFileOther($event, files, type) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of files) {
        if (file.name == ite.FileName) {
          isExist = true;
        }
      }
    }

    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không?").then(
        data => {
          this.updateFileAndReplaceDocument(fileDataSheet, true, files, type, this.fileOtherTemplate);
        }, error => {
          this.updateFileAndReplaceDocument(fileDataSheet, false, files, type, this.fileOtherTemplate);
        });
    }
    else {
      this.updateFileDocument(fileDataSheet, files, type, this.fileOtherTemplate);
    }
  }

  uploadFileQoute($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.profileModel.ListDocumentStep1) {
        if (file.name == ite.FileName) {
          isExist = true;
        }
      }
    }

    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không?").then(
        data => {
          this.updateFileAndReplaceDocument(fileDataSheet, true, this.profileModel.ListDocumentStep1, 1, this.quoteTemplate);
        }, error => {
          this.updateFileAndReplaceDocument(fileDataSheet, false, this.profileModel.ListDocumentStep1, 1, this.quoteTemplate);
        });
    }
    else {
      this.updateFileDocument(fileDataSheet, this.profileModel.ListDocumentStep1, 1, this.quoteTemplate);
    }
  }

  uploadQuoteTransportSupplier($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.profileModel.ListDocumentStep1) {
        if (file.name == ite.FileName) {
          isExist = true;
        }
      }
    }

    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không?").then(
        data => {
          this.updateFileAndReplaceDocument(fileDataSheet, true, this.profileModel.ListTransportSupplier, 1, this.quoteTransportTemplate);
        }, error => {
          this.updateFileAndReplaceDocument(fileDataSheet, false, this.profileModel.ListTransportSupplier, 1, this.quoteTransportTemplate);
        });
    }
    else {
      this.updateFileDocument(fileDataSheet, this.profileModel.ListTransportSupplier, 1, this.quoteTransportTemplate);
    }
  }

  updateFileAndReplaceDocument(files, isReplace, listFile: any[], type: number, objectTemplate: any) {
    var isExist = false;
    let docuemntTemplate;
    for (var file of files) {
      for (let index = 0; index < listFile.length; index++) {
        if (file.name == listFile[index].FileName) {
          isExist = true;
          if (isReplace) {
            listFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, objectTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        listFile.push(docuemntTemplate);
      }
    }
  }

  updateFileDocument(files, listFile: any[], type: number, objectTemplate: any) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, objectTemplate);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      listFile.push(docuemntTemplate);
    }
  }

  showConfirmDeleteFile(files, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá file này không?").then(
      data => {
        files.splice(index, 1);
      }
    );
  }

  changePrice(material) {
    let totalAmount = 0;
    this.profileModel.ListMaterial.forEach(material => {
      totalAmount += Number(material.Price) * Number(material.Quantity);
    });

    let amount = Number(material.Price) * Number(material.Quantity);
    let percentAmount = amount / totalAmount;

    material.InternationalShippingCost = Math.round((((Number(this.profileModel.TransportationInternationalCosts) * percentAmount) / material.Quantity) + Number.EPSILON) * 100) / 100;
    if (this.profileModel.TransportationInternationalCostsUnit != 4 && this.profileModel.TransportExchangeRate > 0) {
      material.InternationalShippingCost = Math.round(((material.InternationalShippingCost * Number(this.profileModel.TransportExchangeRate)) + Number.EPSILON) * 100) / 100;
    }

    material.InlandShippingCost = Math.round((((Number(this.profileModel.CustomsInlandCosts) * percentAmount) / material.Quantity) + Number.EPSILON) * 100) / 100;

    material.OtherCosts = Math.round((((Number(this.profileModel.OtherCosts) * percentAmount) / material.Quantity) + Number.EPSILON) * 100) / 100;

    this.taxChange(material);

    this.updateAmount();
  }

  updateAmount() {
    this.profileModel.AmountVND = 0;
    this.profileModel.Amount = 0;
    this.profileModel.ListMaterial.forEach(material => {
      this.profileModel.AmountVND += material.Amount;
      this.profileModel.Amount += material.Price * material.Quantity;
    });
    this.profileModel.Amount += this.profileModel.OtherCosts;

    this.profileModel.ListPayment.forEach(payment => {
      payment.Money = Math.round((((Number(payment.PercentPayment) * this.profileModel.Amount) / 100) + Number.EPSILON) * 100) / 100;
    });

    this.changePercentPay(this.payTemplate);
  }

  changeSupplier() {
    if (this.profileModel.SupplierId) {
      this.profileModel.IsSupplier = true;
    }
    else {
      this.profileModel.IsSupplier = false;
    }
  }

  downloadAFile(row) {
    this.fileProcess.downloadFileBlob(row.FilePath, row.FileName);
  }

  close() {
    this.router.navigate(['nhap-khau/ho-so-nhap-khau']);
  }

  validNextStep() {
    let dataLength = 0;
    let materialCodes = '';
    // Xác định NCC
    if (this.profileModel.Step == 1) {

      dataLength = this.profileModel.ListMaterial.forEach(element => {
        if (!element.Price || element.Price == 0 || !element.LeadTimeV) {
          materialCodes += materialCodes != '' ? ', ' + element.Code : element.Code;
        }
      });

      if (materialCodes) {
        this.validMessage = 'Các mã vật tư: ' + materialCodes + '. Bạn chưa nhập đầy đủ thông tin Giá hoặc Thời gian giao hàng!';
        return false;
      }

      if (this.profileModel.ListDocumentStep1.length == 0) {
        this.validMessage = 'Bạn chưa upload file báo giá!';
        return false;
      }

      if (!this.profileModel.SupplierId) {
        this.validMessage = 'Bạn chưa chốt nhà cung cấp!';
        return false;
      }

      if (!this.profileModel.QuoteNumber) {
        this.validMessage = 'Bạn chưa nhập Số báo giá !';
        return false;
      }

      if (!this.profileModel.QuoteDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày báo giá!';
        return false;
      }

      if (!this.profileModel.SupplierExpectedDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày hoàn thành dự kiến!';
        return false;
      }
    }
    // Làm HĐ
    else if (this.profileModel.Step == 2) {
      dataLength = this.profileModel.ListDocumentStep2.filter(element => {
        if (element.IsRequired && (!element.FilePath && !element.File)) {
          return element;
        }
      }).length;

      if (dataLength > 0) {
        this.validMessage = 'Bạn chưa upload đủ tài liệu quy định!';
        return false;
      }

      if (!this.profileModel.PONumber) {
        this.validMessage = 'Ban chưa nhập số PO!';
        return false;
      }

      if (!this.profileModel.IsContract) {
        this.validMessage = 'Bạn chưa chọn HĐ đã ký!';
        return false;
      }

      if (!this.profileModel.ContractExpectedDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày hoàn thành dự kiến!';
        return false;
      }
    }
    // Thanh toán
    else if (this.profileModel.Step == 3) {
      dataLength = this.profileModel.ListDocumentStep3.filter(element => {
        if (element.IsRequired && (!element.FilePath && !element.File)) {
          return element;
        }
      }).length;

      if (dataLength > 0) {
        this.validMessage = 'Bạn chưa upload đủ tài liệu quy định!';
        return false;
      }

      if (this.profileModel.ListPayment == 0) {
        this.validMessage = 'Bạn chưa nhập thông tin thanh toán!';
        return false;
      }

      // dataLength = this.profileModel.ListPayment.filter(element => {
      //   if (element.Status == 2) {
      //     return element;
      //   }
      // }).length;

      // if (dataLength == 0) {
      //   this.validMessage = 'Chưa có lần thanh toán nào được thanh toán!';
      //   return false;
      // }

      if (!this.profileModel.PayExpectedDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày hoàn thành dự kiến!';
        return false;
      }
    }
    // Tiến độ
    else if (this.profileModel.Step == 4) {
      dataLength = this.profileModel.ListDocumentStep4.filter(element => {
        if (element.IsRequired && (!element.FilePath && !element.File)) {
          return element;
        }
      }).length;

      if (dataLength > 0) {
        this.validMessage = 'Bạn chưa upload đủ tài liệu quy định!';
        return false;
      }

      if (!this.profileModel.EstimatedDeliveryDateV) {
        this.validMessage = 'Bạn chưa nhập ngày giao hàng dự kiến!';
        return false;
      }

      if (!this.profileModel.ProductionExpectedDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày hoàn thành dự kiến!';
        return false;
      }
    }
    // Vận chuyển
    else if (this.profileModel.Step == 5) {
      dataLength = this.profileModel.ListDocumentStep5.filter(element => {
        if (element.IsRequired && (!element.FilePath && !element.File)) {
          return element;
        }
      }).length;

      if (dataLength > 0) {
        this.validMessage = 'Bạn chưa upload đủ tài liệu quy định!';
        return false;
      }

      if (this.profileModel.ListTransportSupplier.length < 3) {
        this.validMessage = 'Bạn chưa upload đủ báo giá của 3 nhà cung cấp vận chuyển!';
        return false;
      }
      else {
        let checkSupplier = this.profileModel.ListTransportSupplier.filter(element => {
          if (!element.TransportSupplierId) {
            return element;
          }
        }).length;

        if (checkSupplier > 0) {
          this.validMessage = 'Bạn chưa chọn đầy đủ nhà cung cấp ở báo giá!';
          return false;
        }

        if (!this.profileModel.TransportationInternationalCostsUnit) {
          this.validMessage = 'Bạn chưa chọn đơn vị tiền tệ cho chi phí vận chuyển!';
          return false;
        }

        let checkLeadtime = this.profileModel.ListTransportSupplier.filter(element => {
          if (!element.TransportLeadtime) {
            return element;
          }
        }).length;

        if (checkLeadtime > 0) {
          this.validMessage = 'Bạn chưa chọn đầy đủ thời gian vận chuyển ở báo giá!';
          return false;
        }

        let checkShippingCost = this.profileModel.ListTransportSupplier.filter(element => {
          if (element.ShippingCost <= 0) {
            return element;
          }
        }).length;

        if (checkShippingCost > 0) {
          this.validMessage = 'Bạn chưa nhập đầy đủ chi phí vận chuyển ở báo giá!';
          return false;
        }
      }

      if (this.profileModel.DeliveryConditions == 'EXW' || this.profileModel.DeliveryConditions == 'FCA' || this.profileModel.DeliveryConditions == 'FOB' || this.profileModel.DeliveryConditions == 'FAS') {
        if (!this.profileModel.TransportSupplierId) {
          this.validMessage = 'Bạn chưa chọn Nhà cung cấp vận chuyển!';
          return false;
        }
      }
      // if (!this.profileModel.ShippingCost || this.profileModel.ShippingCost == 0) {
      //   this.validMessage = 'Bạn chưa nhập giá vận chuyển!';
      //   return false;
      // }

      // if (!this.profileModel.TransportationCosts || this.profileModel.TransportationCosts == 0) {
      //   this.validMessage = 'Bạn chưa nhập Cước nội địa!';
      //   return false;
      // }

      if (!this.profileModel.TransportationInternationalCosts || this.profileModel.TransportationInternationalCosts == 0) {
        this.validMessage = 'Bạn chưa nhập Chi phí vận chuyển quốc tế!';
        return false;
      }

      if (this.profileModel.TransportationInternationalCostsUnit != 4 && !this.profileModel.TransportExchangeRate) {
        this.validMessage = 'Bạn chưa nhập Tỉ giá VNĐ!';
        return false;
      }

      if (!this.profileModel.PackageQuantity) {
        this.validMessage = 'Bạn chưa nhập Số Kiện!';
        return false;
      }

      if (!this.profileModel.NetWeight) {
        this.validMessage = 'Bạn chưa nhập Net Weight!';
        return false;
      }

      if (!this.profileModel.ChargingWeight) {
        this.validMessage = 'Bạn chưa nhập Charging Weight!';
        return false;
      }

      if (this.profileModel.Amount > 2000 && !this.profileModel.IsInsurrance) {
        this.validMessage = 'Bạn chưa chọn đã mua bảo hiểm!';
        return false;
      }

      if (!this.profileModel.TransportExpectedDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày hoàn thành dự kiến!';
        return false;
      }
    }
    // Hải quan
    else if (this.profileModel.Step == 6) {
      dataLength = this.profileModel.ListDocumentStep6.filter(element => {
        if (element.IsRequired && (!element.FilePath && !element.File)) {
          return element;
        }
      }).length;

      if (dataLength > 0) {
        this.validMessage = 'Bạn chưa upload đủ tài liệu quy định!';
        return false;
      }
      materialCodes = '';
      this.profileModel.ListMaterial.forEach(element => {
        if (!element.HSCode) {
          materialCodes += materialCodes != '' ? ', ' + element.Code : element.Code;
        }
      });

      if (materialCodes) {
        this.validMessage = 'Các mã vật tư: ' + materialCodes + '. Bạn chưa nhập HS Code!';
        return false;
      }

      if (!this.profileModel.CustomsSupplierId) {
        this.validMessage = 'Bạn chưa chọn NCC dịch vụ hải quan!';
        return false;
      }

      if (!this.profileModel.CustomsName) {
        this.validMessage = 'Bạn chưa nhập Đơn vị hải quan!';
        return false;
      }

      if (!this.profileModel.CustomsDeclarationFormCode) {
        this.validMessage = 'Bạn chưa nhập số tờ khai!';
        return false;
      }

      if (!this.profileModel.CustomsDeclarationFormDateV) {
        this.validMessage = 'Bạn chưa nhập ngày tờ khai!';
        return false;
      }

      if (!this.profileModel.CustomsClearanceFromDateV) {
        this.validMessage = 'Bạn chưa nhập ngày tờ khai thông quan!';
        return false;
      }

      if (!this.profileModel.CustomsType) {
        this.validMessage = 'Bạn chưa chọn Luồng!';
        return false;
      }

      if (!this.profileModel.CustomsInlandCosts) {
        this.validMessage = 'Bạn chưa nhập Chi phí nội địa!';
        return false;
      }

      if (!this.profileModel.CustomsElearanceStatus) {
        this.validMessage = 'Bạn chưa chọn Đã thông quan!';
        return false;
      }

      if (!this.profileModel.CustomExpectedDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày hoàn thành dự kiến!';
        return false;
      }
    }
    else if (this.profileModel.Step == 7) {
      dataLength = this.profileModel.ListDocumentStep7.filter(element => {
        if (element.IsRequired && (!element.FilePath && !element.File)) {
          return element;
        }
      }).length;


      if (dataLength > 0) {
        this.validMessage = 'Bạn chưa upload đủ tài liệu quy định!';
        return false;
      }

      if (!this.profileModel.WarehouseCode) {
        this.validMessage = 'Bạn chưa nhập số phiếu nhập kho!';
        return false;
      }

      if (!this.profileModel.WarehouseDateV) {
        this.validMessage = 'Bạn chưa nhập ngày nhập kho!';
        return false;
      }

      if (!this.profileModel.WarehouseStatus) {
        this.validMessage = 'Bạn chưa chọn Đã nhập kho!';
        return false;
      }

      if (!this.profileModel.WarehouseExpectedDateV) {
        this.validMessage = 'Bạn chưa nhập Ngày hoàn thành dự kiến!';
        return false;
      }

    }

    this.listReportProplem = this.profileModel.ListProblem.filter((i: { Step: any; Note: any }) => i.Step == this.profileModel.Step && !i.Note);

    if (this.listReportProplem.length > 0) {
      this.validMessage = 'Vấn đề tồn đọng chưa nhập nội dung!';
      return false;
    }

    this.listReportProplem = this.profileModel.ListProblem.filter((i: { Step: any; Status: any }) => i.Step == this.profileModel.Step && i.Status == 2);

    if (this.listReportProplem.length > 0) {
      this.validMessage = 'Còn vấn đề tồn đọng chưa được xử lý!';
      return false;
    }

    return true;
  }

  showCreateSupplierService(type) {
    let activeModal = this.modalService.open(SupplierCreateComponent, { container: 'body', windowClass: 'suppliercreate-model', backdrop: 'static' })
    activeModal.componentInstance.supplierGroupCode = "TPAVT.Y";
    activeModal.result.then((result) => {
      if (result) {
        this.getSupplierServices();

        setTimeout(() => {
          if (this.constant.ImportStep[this.indexNumber].Id == 5) {
            this.profileModel.TransportSupplierId = result;
          } else if (this.constant.ImportStep[this.indexNumber].Id == 6) {
            this.profileModel.CustomsSupplierId = result;
          }
        }, 1000);
      }
    }, (reason) => {
    });
  }

  taxChange(material) {
    let amount = Number(material.Price);
    let otherCosts = Number(material.OtherCosts);
    if (material.CurrencyUnit != 4 && this.profileModel.SupplierExchangeRate > 0) {
      amount = amount * Number(this.profileModel.SupplierExchangeRate);
      otherCosts = otherCosts * Number(this.profileModel.SupplierExchangeRate);
    }

    material.RealPrice = amount + otherCosts + (Math.round((((Number(material.ImportTaxValue) / Number(material.Quantity))) + Number.EPSILON) * 100) / 100) + (Math.round((((Number(material.VATTaxValue) / Number(material.Quantity))) + Number.EPSILON) * 100) / 100) + material.InternationalShippingCost + material.InlandShippingCost;
    material.Amount = material.RealPrice * Number(material.Quantity);
    this.updateAmount();
  }

  transportCostChange() {
    let totalAmount = 0;
    this.profileModel.ListMaterial.forEach(material => {
      totalAmount += Number(material.Price) * Number(material.Quantity);
    });
    let amount = 0;
    let price = 0;
    let otherCosts = 0;
    let percentAmount = 0;
    this.profileModel.ListMaterial.forEach(material => {
      amount = Number(material.Price) * Number(material.Quantity);
      percentAmount = amount / totalAmount;

      material.InternationalShippingCost = Math.round((((Number(this.profileModel.TransportationInternationalCosts) * percentAmount) / material.Quantity) + Number.EPSILON) * 100) / 100;
      if (this.profileModel.TransportationInternationalCostsUnit != 4 && this.profileModel.TransportExchangeRate > 0) {
        material.InternationalShippingCost = Math.round(((material.InternationalShippingCost * Number(this.profileModel.TransportExchangeRate)) + Number.EPSILON) * 100) / 100;
      }

      material.InlandShippingCost = Math.round((((Number(this.profileModel.CustomsInlandCosts) * percentAmount) / material.Quantity) + Number.EPSILON) * 100) / 100;


      price = Number(material.Price);
      material.OtherCosts = Math.round((((Number(this.profileModel.OtherCosts) * percentAmount) / material.Quantity) + Number.EPSILON) * 100) / 100;
      otherCosts = material.OtherCosts;
      if (material.CurrencyUnit != 4 && this.profileModel.SupplierExchangeRate > 0) {
        price = price * Number(this.profileModel.SupplierExchangeRate);
        otherCosts = otherCosts * Number(this.profileModel.SupplierExchangeRate);
      }

      material.RealPrice = price + otherCosts + (Math.round((((Number(material.ImportTaxValue) / Number(material.Quantity))) + Number.EPSILON) * 100) / 100) + (Math.round((((Number(material.VATTaxValue) / Number(material.Quantity))) + Number.EPSILON) * 100) / 100) + material.InternationalShippingCost + material.InlandShippingCost;
      material.Amount = material.RealPrice * Number(material.Quantity);
    });

    this.updateAmount();
  }

  changeUnit() {
    this.profileModel.ListMaterial.forEach(element => {
      element.CurrencyUnit = this.profileModel.CurrencyUnit;
    });
  }

  downListFile(step: any) {
    this.exportListFileModel.Id = this.profileModel.Id;
    this.exportListFileModel.Step = step;
    this.importProfileService.getListFile(this.exportListFileModel).subscribe(data => {
      if (data.length <= 0) {
        this.messageService.showMessage("Không có file để tải");
        return;
      }
      var listFilePath: any[] = [];
      data.forEach(element => {
        listFilePath.push({
          Path: element.Path,
          FileName: element.FileName
        });
      });
      this.listFileModel.Name = "DanhSachChungTu_";
      this.listFileModel.ListDatashet = listFilePath;
      this.importProfileService.downloadListFile(this.listFileModel).subscribe(data => {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerFileApi + data.PathZip;
        link.download = 'Download.zip';
        document.body.appendChild(link);
        // link.focus();
        link.click();
        document.body.removeChild(link);
      }, e => {
        this.messageService.showError(e);
      });
    }, e => {
      this.messageService.showError(e);
    });

  }

  // Báo cáo vấn đề tồn đọng
  statusProblem = 2;
  noteProblem: '';
  plan: '';
  createBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
  createDate = '';
  date = new Date;

  modelProblem: any = {
    Step: 2,
    ImportProfileId: '',
    Status: null,
    Note: '',
    Plan: '',
    CreateBy: JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname,
    CreateDate: null
  }

  addRowProblem() {
    this.date = new Date;
    if (this.noteProblem) {
      var row = Object.assign({}, this.modelProblem);
      row.Status = this.statusProblem;
      row.Note = this.noteProblem;
      row.Plan = this.plan;
      row.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
      row.CreateDate = this.date;
      row.Step = this.modelSearch.Step;
      row.ImportProfileId = this.profileModel.Id;

      this.listReportProplem.push(row);
      this.profileModel.ListProblem.push(row);

      this.statusProblem = 2;
      this.noteProblem = '';
      this.plan = '';
      this.createBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
      this.createDate = '';
    }
    else {
      this.messageService.showMessage("Bạn không được để trống nội dung!");
    }

  }

  showConfirmDeleteReportProblem(row: any, index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vấn đề này không?").then(
      data => {
        this.listReportProplem.splice(index, 1);
        this.profileModel.ListProblem.splice(this.profileModel.ListProblem.indexOf(row), 1);
        this.setInfo();
      }
    );
  }

  modelSearch: any = {
    Step: 0,
    ImportProfileId: ''
  }

  modelCreateProblem: any = {
    Step: 0,
    ImportProfileId: '',
    ListProblem: []
  }

  searchImportProfileProblemExist(step: number) {
    this.listReportProplem = this.profileModel.ListProblem.filter((i: { Step: any; }) => i.Step == step);
    this.modelSearch.Step = step;
  }

  createImportProblemExist() {
    this.modelCreateProblem = {
      Step: this.modelSearch.Step,
      ImportProfileId: this.modelSearch.ImportProfileId,
      ListProblem: this.listReportProplem
    }

    this.importProfileService.createImportProblemExist(this.modelCreateProblem).subscribe((data: any) => {
      this.messageService.showSuccess('Cập nhật vấn đề tồn đọng thành công!');
    }, error => {
      this.messageService.showError(error);
    });
  }
}
