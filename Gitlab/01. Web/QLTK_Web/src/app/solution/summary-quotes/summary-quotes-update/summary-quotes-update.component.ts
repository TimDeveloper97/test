import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSetting, ComboboxService, Configuration, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from '../../service/summary-quotes.service';
import { forkJoin } from 'rxjs';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowChooseQuotationStepComponent } from '../summary-quotes-create/show-choose-quotation-step/show-choose-quotation-step.component';
import { DownloadService } from 'src/app/shared/services/download.service';

@Component({
  selector: 'app-summary-quotes-update',
  templateUrl: './summary-quotes-update.component.html',
  styleUrls: ['./summary-quotes-update.component.scss']
})
export class SummaryQuotesUpdateComponent implements OnInit {

  constructor(
    private config: Configuration,
    public appSetting: AppSetting,
    private router: Router,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    private service: SummaryQuotesService,
    public fileProcessDataSheet: FileProcess,
    private uploadService: UploadfileService,
    public dateUtils: DateUtils,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private dowloadservice: DownloadService,
  ) { }

  ListFileQuoteDocument = [];
  isAction: boolean = false;
  selectIndex = 0;
  listCustomer: any[] = [];
  listCustomerId: any[] = [];
  listSBU: any[] = [];
  listQuotes: any[] = [];
  listFile: any[] = [];

  listQuotesOutput: any[] = [];

  ListNumberYCKH: any[] = [];
  columnName: any[] = [{ Name: 'CustomerCode', Title: 'Mã' }, { Name: 'CustomerName', Title: 'Tên' }];
  columnNameRequire: any[] = [{ Name: 'NumberRequire', Title: 'Mã YCKH' }];
  columnNameSBU: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }];

  model: any = {
    Id: '',
    Status: 0,
    CustomerId: '',
    CustomerCode: '',
    CustomerRequirementId: '',
    CustomerContactId: '',
    EmployeeId: '',
    NumberRequire: '',
    ContentRequire: '',
    SBUId: '',
    ListQuoteDocument: [],
    ListQuotesStep: [],
    Description: '',
    AdvanceRate: '',
    ImplementationDate: '',
    ExpectedPrice: '',
    Delivery: '',
    Warranty: '',
    PaymentMethod: '',
    isShowChosse: false
  }

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

  modelIn: any = {

  }

  Id: string;

  ngOnInit(): void {
    this.Id = this.route.snapshot.paramMap.get('Id');
    this.model.Id = this.Id;
    
    this.getListCustomer();
    this.getCbbData();
    this.service.getQuotationById(this.Id).subscribe(data => {
      this.appSetting.PageTitle = 'Chỉnh sửa báo giá' + ' - ' + data.data.Code;
      this.model = data.data;
      
      this.model.ListQuoteDocument = [];
      this.listQuotesOutput = data.ListQuotationPlan;
      this.model.ListQuotesStep = data.dataStepInQuotation;
      if (data.data.QuotationDate) {
        let dateArray = data.data.QuotationDate.split('T')[0];
        let dateValue = dateArray.split('-');
        let QuotationDateV = {
          'day': parseInt(dateValue[2]),
          'month': parseInt(dateValue[1]),
          'year': parseInt(dateValue[0])
        };
        this.model.QuotationDateV = QuotationDateV;
      }
      if (data.data.ImplementationDate) {
        let dateArray = data.data.ImplementationDate.split('T')[0];
        let dateValue = dateArray.split('-');
        let ImplementationDateV = {
          'day': parseInt(dateValue[2]),
          'month': parseInt(dateValue[1]),
          'year': parseInt(dateValue[0])
        };
        this.model.ImplementationDateV = ImplementationDateV;
      }
      this.model.SBUid = data.data.SBUId;
      this.getCustomer(data.data.CustomerId);
      this.changeCustomer(data.data.CustomerId);
      this.changeSBU(data.data.SBUId);
      this.getFileInfor(this.Id);
      this.getRequire(data.data.CustomerRequirementId);
    });

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

  getFileInfor(quotationId: any) {
    this.service.getFileInfor(quotationId).subscribe(
      data => {
        this.ListFileQuoteDocument = data;
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
    activeModal.componentInstance.listQuotesOutputUpdate = this.listQuotesOutput;
    activeModal.result.then((result) => {
      if (result) {
        this.listQuotesOutput = result;
        this.model.ListQuotesStep = result;
        this.model.isShowChosse = true;
      }
    },)
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
        this.modelIn.EmployeeChargeName = data.EmployeeCharge.EmployeeChargeName;
        this.modelIn.EmployeeChargeCode = data.EmployeeCharge.EmployeeChargeCode;
        this.modelIn.EmployeeChargePhone = data.EmployeeCharge.EmployeeChargePhone;
        this.modelIn.EmployeeChargeEmail = data.EmployeeCharge.EmployeeChargeEmail;
        this.model.EmployeeId = data.EmployeeCharge.EmployeeChargeId;
        this.model.CustomerContactId = data.CusContact.CusContactId,
        this.model.CustomerCode = data.EmployeeCharge.CustomerCode;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getCustomer(CustomerId: any) {
    this.service.getCustomerById(CustomerId).subscribe(
      data => {
        //this.model.CustomerRequirementId = data.data[0].CustomerRequirementId;
        //this.changeRequire(data.data[0].CustomerRequirementId);
        this.modelIn.EmployeeChargeName = data.EmployeeCharge.EmployeeChargeName;
        this.modelIn.EmployeeChargeCode = data.EmployeeCharge.EmployeeChargeCode;
        this.modelIn.EmployeeChargePhone = data.EmployeeCharge.EmployeeChargePhone;
        this.modelIn.EmployeeChargeEmail = data.EmployeeCharge.EmployeeChargeEmail;
        this.model.EmployeeId = data.EmployeeCharge.EmployeeChargeId;
        this.model.CustomerContactId = data.CusContact.CusContactId,
          this.model.CustomerCode = data.EmployeeCharge.CustomerCode;
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

  getRequire(RequireId: any){
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
    if(this.model.SBUId != SBUid)
    {
      this.listQuotesOutput = [];
    }
    else{
      this.service.getQuotationById(this.Id).subscribe(data => {
        this.listQuotesOutput = data.ListQuotationPlan;
      });
    }
    this.service.getQuotesBySBU(SBUid).subscribe(
      data => {
        this.listQuotes = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  clickQuote(status) {
    if (status == 0) {
      this.checkSoldQuotation(this.Id);
    }
    if (status == 1) {
      this.messageService.showConfirm("Bạn có chắc chắn muốn HỦY CHỐT báo giá này không?").then(
        data => {
          this.model.Status = 0;
          this.service.updateQuote(this.model).subscribe();
        },
        error => {}
      );
    }
  }

  checkSoldQuotation(quotationId)
  {
    this.service.checkSoldQuotation(quotationId).subscribe(
      data =>{
        if(data == null)
        {
          this.messageService.showConfirm("Bạn có chắc chắn muốn CHỐT báo giá này không?").then(
            data => {
              this.model.Status = 1;
              this.service.updateQuote(this.model).subscribe();
            },
            error => {}
          );
        }
        else{
          this.messageService.showConfirm("Đã có báo giá " + data.QuotationName + " chốt cho YCKH " + data.YCKH + ". Bạn có hủy báo giá cũ không?").then(
            data1 => {
              this.service.ChangeStatusQuotation(data.QuotationId).subscribe(
                data2 => {
                  this.model.Status = 1;
                  this.service.updateQuote(this.model).subscribe();
                },
                error => {
                  this.messageService.showError(error);
                }
              );
            },
            error => {}
          )
        }
      });
  }

  UpdateQuotation() {
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
            this.ListFileQuoteDocument.push(file);
          });
        }
        this.ListFileQuoteDocument.forEach(element => {
          if (element.FilePath) {
            this.model.ListQuoteDocument.push(element);
          }
        });
        this.Update();
      }, error => {
        this.messageService.showError(error);
      });
    } else { this.Update() }


  }

  Update() {
    this.model.ListQuotesStep =  this.listQuotesOutput;
    this.service.updateQuote(this.model).subscribe(
      data => {

        this.listCustomer = [];
        this.listCustomerId = [];
        this.ListNumberYCKH = [];
        this.ListFileQuoteDocument = [];

        this.model = {
          Id: '',
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
          isShowChosse: false
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
        this.messageService.showSuccess('Chỉnh sửa báo giá thành công!');
        this.router.navigate(['giai-phap/tong-hop-bao-gia']);

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['giai-phap/tong-hop-bao-gia']);
  }

  exportExcel(QuotationId) {
    this.service.exportExcel(this.model, QuotationId).subscribe(d => {
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

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.FilePath == null) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
      listFilePath.push({
        Path: Datashet.FilePath,
        FileName: Datashet.FileName
      });

    let modelDowload: any = {
      Name: '',
      ListDatashet: []
    }

    modelDowload.Name = "Tài liệu";
    modelDowload.ListDatashet = listFilePath;
    this.dowloadservice.downloadAll(modelDowload).subscribe(data => {
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
  }
}
