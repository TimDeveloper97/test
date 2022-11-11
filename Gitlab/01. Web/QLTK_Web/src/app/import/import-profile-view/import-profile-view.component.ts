import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef,AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ProductStandardTpaService } from 'src/app/device/services/product-standard-tpa.service';
import { AppSetting, Configuration, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ChooseMaterialImportPrComponent } from '../choose-material-import-pr/choose-material-import-pr.component';
import { ImportProfileService } from '../services/import-profile.service';

@Component({
  selector: 'app-import-profile-view',
  templateUrl: './import-profile-view.component.html',
  styleUrls: ['./import-profile-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ImportProfileViewComponent implements OnInit {
  startIndex: number = 1;

  listFile: any[] = [];
  listReportProplem: any[] = [];

  checkedTop = false;
  listSelect = [];

  changeStepModel = {
    Id: ''
  }

  profileModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Step: 0,
    PRDueDate: null,
    PRDueDateV: null,
    ProjectCode: '',
    ManufacturerCode: '',
    PRCode: '',
    Amount: 0,
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
    ListPayment: []
  };

  payModel = {
    Id: '',
    Index: 0,
    ImportProfileId: '',
    PercentPayment: 0,
    Money: 0,
    Duedate: null,
    Status: 1,
    Note: '',
    MoneyTransferPath: '',
    MoneyTransferName: ''
  }

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  Index = 1;
  PercentPayment = 0;
  Money = 0;
  DueDate = null;
  Status = 1;
  Note = ''

  columnNameSupplier: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name_NCC_SX', Title: 'Tên nhóm' }];
  listSupplier = [];
  userLogin: any = {};

  documentSelect: any = {};
  fileUpload: any[] = [];

  @ViewChild('fileInputDocument') inputFileCOCQ: ElementRef
  @ViewChild('scrollHeader', { static: false }) scrollHeader: ElementRef;
  @ViewChild('scrollProduct', { static: false }) scrollProduct: ElementRef;
  @ViewChild('scrollFooter', { static: false }) scrollFooter: ElementRef;

  // @ViewChild('scrollCustomHeader', { static: false }) scrollCustomHeader: ElementRef;
  // @ViewChild('scrollCustomProduct', { static: false }) scrollCustomProduct: ElementRef;
  // @ViewChild('scrollCustomFooter', { static: false }) scrollCustomFooter: ElementRef;
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
    private config:Configuration
  ) {
    this.userLogin = JSON.parse(localStorage.getItem('qltkcurrentUser'));
  }

  ngOnInit(): void {

    this.profileModel.Id = this.routeA.snapshot.paramMap.get('Id');
    this.appSetting.PageTitle = 'Xem thông tin hồ sơ nhập khẩu';
    this.getById();

  }
  
  ngAfterViewInit() {
    this.scrollProduct.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      this.scrollFooter.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    // this.scrollCustomProduct.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
    //   this.scrollCustomHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    //   this.scrollCustomFooter.nativeElement.scrollLeft = event.target.scrollLeft;
    // }, true);
  }

  getById() {
    this.changeStepModel.Id = this.profileModel.Id;
    this.importProfileService.getViewById(this.changeStepModel).subscribe(
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

        this.profileModel.ListDocumentStep1.forEach(item => {
          if (item.QuoteDate) {
            item.QuoteDateV = this.dateUtils.convertDateToObject(item.QuoteDate);
          }
        });

        this.constant.ImportStep.forEach((item, index) => {
          if (item.Id == this.profileModel.Step) {
            this.indexNumber = index;
          }
        })

        if (this.profileModel.ListPayment.length > 0) {
          this.profileModel.ListPayment.forEach(item => {
            if (item.Duedate) {
              item.DuedateV = this.dateUtils.convertDateToObject(item.Duedate);
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

  updateAmount() {
    this.profileModel.Amount = 0;
    this.profileModel.AmountVND = 0;
    this.profileModel.ListMaterial.forEach(material => {
      this.profileModel.AmountVND += material.Amount; 
      this.profileModel.Amount += material.Price*material.Quantity;
    });
    this.profileModel.ListPayment.forEach(payment => {
      payment.Money = Math.round((((Number(payment.PercentPayment) * this.profileModel.Amount) / 100) + Number.EPSILON) * 100) / 100;
    });

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
  
  downloadAFile(row){   
    this.fileProcess.downloadFileBlob(row.FilePath, row.FileName);
  }

  close() {
    this.router.navigate(['nhap-khau/ho-so-nhap-khau']);
  }

  edit(){
    this.router.navigate(['nhap-khau/ho-so-nhap-khau-ket-thuc/chinh-sua/'+ this.profileModel.Id]);
  }

  searchImportProfileProblemExist(step: number) {
    this.listReportProplem = this.profileModel.ListProblem.filter((i: { Step: any; }) => i.Step == step);
  }
}
