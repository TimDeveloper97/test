import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AppSetting, ComboboxService, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from '../../service/summary-quotes.service';
import { forkJoin } from 'rxjs';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowChooseQuotationStepComponent } from './show-choose-quotation-step/show-choose-quotation-step.component';
@Component({
  selector: 'app-summary-quotes-create',
  templateUrl: './summary-quotes-create.component.html',
  styleUrls: ['./summary-quotes-create.component.scss']
})
export class SummaryQuotesCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    private service: SummaryQuotesService,
    public fileProcessDataSheet: FileProcess,
    private uploadService: UploadfileService,
    public dateUtils: DateUtils,
    private routeA: ActivatedRoute,
    private modalService: NgbModal,
  ) { }

  listCustomer: any[] = [];
  listCustomerId: any[] = [];
  listSBU: any[] = [];
  listQuotes: any[] = [];
  listQuotesOutput: any[] = [];

  ListNumberYCKH: any[] = [];

  model: any = {
    Status: 0,
    CustomerId: '',
    CustomerCode: '',
    CustomerRequirementId: '',
    CustomerContactId: '',
    EmployeeId: '',
    NumberYCKH: '',
    ContentRequire: '',
    SBUid: '',
    ListQuoteDocument: [],
    ListQuotesStep: [],
    Description: '',
    AdvanceRate: '',
    ImplementationDate: '',
    ExpectedPrice: '',
    Delivery: '',
    Warranty: '',
    PaymentMethod: '',
  }

  modelIn: any = {

  }

  selectIndex = 0;

  columnName: any[] = [{ Name: 'CustomerCode', Title: 'Mã' }, { Name: 'CustomerName', Title: 'Tên' }];
  columnNameRequire: any[] = [{ Name: 'NumberRequire', Title: 'Mã YCKH' }];
  columnNameSBU: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];

  fileListFileQuoteDocument = {
    Id: '',
    FilePath: '',
    FileName: '',
    Size: '',
    Extention: '',
    Description: '',
    Thumbnail: '',
    HashValue: ''
  }
  ListFileQuoteDocument = [];

  isAction: boolean = false;

  ngOnInit(): void {

    this.model.CustomerRequirementId = this.routeA.snapshot.paramMap.get('Id');
    this.model.CustomerId = this.routeA.snapshot.paramMap.get('CustomerId');
    this.getListCustomer();
    this.getCbbData();
    this.appSetting.PageTitle = "Thêm mới báo giá";
    if (this.model.CustomerId) {
      this.changeCustomer(this.model.CustomerId);
    }
  }

  getCbbData() {
    let cbbSBU = this.comboboxService.getCBBSBU();
    forkJoin([cbbSBU]).subscribe(results => {
      this.listSBU = results[0];
    });
  }

  getListCustomer() {
    this.service.getCustomerRequire().subscribe(
      data => {
        this.listCustomer = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeCustomer(CustomerId: any) {
    this.model.ContentRequire = null;
    this.ListNumberYCKH = null;
    this.modelIn.EmployeeChargeName = null;
    this.modelIn.EmployeeChargeCode = null;
    this.modelIn.EmployeeChargePhone = null;
    this.modelIn.EmployeeChargeEmail = null;
    this.modelIn.EmployeeName = null;
    this.modelIn.EmployeePhone = null;
    this.modelIn.EmployeeEmail = null;
    this.model.EmployeeId = null;
    this.model.CustomerContactId = null;
    this.model.CustomerCode = null;
    this.service.getCustomerById(CustomerId).subscribe(
      data => {
        this.ListNumberYCKH = data.data;

        if (data.EmployeeCharge) {
          this.modelIn.EmployeeChargeName = data.EmployeeCharge.EmployeeChargeName;
          this.modelIn.EmployeeChargeCode = data.EmployeeCharge.EmployeeChargeCode;
          this.modelIn.EmployeeChargePhone = data.EmployeeCharge.EmployeeChargePhone;
          this.modelIn.EmployeeChargeEmail = data.EmployeeCharge.EmployeeChargeEmail;
          this.model.EmployeeId = data.EmployeeCharge.EmployeeChargeId;
          this.model.CustomerCode = data.EmployeeCharge.CustomerCode;
        }

        this.model.CustomerContactId = data.CusContact.CusContactId;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeRequire(RequireId: any) {
    this.modelIn.EmployeeName = null;
    this.modelIn.EmployeePhone = null;
    this.modelIn.EmployeeEmail = null;
    this.service.getRequireByNumberYCKH(RequireId).subscribe(
      data => {
        this.model.ContentRequire = data.data.ContentRequire;
        this.modelIn.EmployeeName = data.Petitioner.EmployeeName;
        this.modelIn.EmployeePhone = data.Petitioner.EmployeePhone;
        this.modelIn.EmployeeEmail = data.Petitioner.EmployeeEmail;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbSBU() {
    this.comboboxService.getCBBSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeSBU(SBUid: any) {
    this.service.getQuotesBySBU(SBUid).subscribe(
      data => {
        this.listQuotes = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showChoose(SBUid: string) {
    let activeModal = this.modalService.open(ShowChooseQuotationStepComponent, { container: 'body', windowClass: 'show-choose-quotation-step-model', backdrop: 'static' })
    activeModal.componentInstance.SBUid = SBUid;
    activeModal.componentInstance.listQuotes = this.listQuotes;
    activeModal.result.then((result) => {
      if (result) {
        this.listQuotesOutput = result;
        this.model.ListQuotesStep = result;
      }
    },)
  }

  uploadFileClick($event) {
    var fileDataSheet = this.fileProcessDataSheet.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.ListFileQuoteDocument) {
        if (ite.Id != null) {
          if (file.name == ite.FileName) {
            isExist = true;
          }
        }
        else {
          if (file.name == ite.name) {
            isExist = true;
          }
        }
      }
    }
    if (isExist) {
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
        data => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.ListFileQuoteDocument.length; index++) {

        if (this.ListFileQuoteDocument[index].Id != null) {
          if (file.name == this.ListFileQuoteDocument[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.ListFileQuoteDocument.splice(index, 1);
            }
          }
        }
        else if (file.name == this.ListFileQuoteDocument[index].name) {
          isExist = true;
          if (isReplace) {
            this.ListFileQuoteDocument.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.ListFileQuoteDocument.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.ListFileQuoteDocument.push(file);
    }
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.ListFileQuoteDocument.splice(index, 1);
      },
      error => {

      }
    );
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue: boolean) {

    this.create(isContinue);
  }

  create(isContinue) {
    var listFileUpload = [];
    this.ListFileQuoteDocument.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });

    if (this.model.QuotationDateV) {
      this.model.QuotationDate = this.dateUtils.convertObjectToDate(this.model.QuotationDateV);
    }
    if (this.model.ImplementationDateV) {
      this.model.ImplementationDate = this.dateUtils.convertObjectToDate(this.model.ImplementationDateV);
    }

    if (this.ListFileQuoteDocument.length > 0) {
      this.uploadService.uploadListFile(this.ListFileQuoteDocument, 'QuotesDocument/').subscribe((event: any) => {
        if (event.length > 0) {
          event.forEach((item, index) => {
            var file = Object.assign({}, this.fileListFileQuoteDocument);
            file.FileName = item.FileName;
            file.Size = item.FileSize;
            file.FilePath = item.FileUrl;
            file.Description = listFileUpload[index].Description;
            file.Thumbnail = item.FileUrlThum;
            this.model.ListQuoteDocument.push(file);
          });

          this.createA(isContinue);
        }
      }, error => {
        this.messageService.showError(error);
      });
    } else { this.createA(isContinue) }



  }

  createA(isContinue) {

    this.service.createQuote(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;

          this.listCustomer = [];
          this.listCustomerId = [];
          this.ListNumberYCKH = [];
          this.ListFileQuoteDocument = [];

          this.model = {
            Status: 0,
            CustomerId: '',
            CustomerCode: '',
            CustomerRequirementId: '',
            CustomerContactId: '',
            EmployeeId: '',
            NumberYCKH: '',
            ContentRequire: '',
            SBUid: '',
            ListQuoteDocument: [],
            ListQuotesStep: [],
            Description: '',
            AdvanceRate: '',
            ImplementationDate: '',
            ExpectedPrice: '',
            Delivery: '',
            Warranty: '',
            PaymentMethod: '',
          };

          this.modelIn = {
            EmployeeChargeName: '',
            EmployeeChargeCode: '',
            EmployeeChargePhone: '',
            EmployeeChargeEmail: '',
            EmployeeName: '',
            EmployeePhone: '',
            EmployeeEmail: ''
          };

          this.fileListFileQuoteDocument = {
            Id: '',
            FilePath: '',
            FileName: '',
            Size: '',
            Extention: '',
            Description: '',
            Thumbnail: '',
            HashValue: ''
          }

          this.getListCustomer();
          this.getCbbData();
          this.messageService.showSuccess('Thêm mới báo giá thành công!');
          this.router.navigate(['giai-phap/tong-hop-bao-gia']);
        }
        else {
          this.messageService.showSuccess('Thêm mới báo giá thành công!');
          this.router.navigate(['giai-phap/tong-hop-bao-gia/chinh-sua/' + data]);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['giai-phap/tong-hop-bao-gia']);
  }
}
