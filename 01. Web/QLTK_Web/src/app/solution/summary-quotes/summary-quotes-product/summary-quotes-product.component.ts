import { DecimalPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, ComponentService, Configuration, Constants, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from '../../service/summary-quotes.service';
import { forkJoin } from 'rxjs';
import { SummaryQuotesProductCreateComponent } from '../summary-quotes-product-create/summary-quotes-product-create.component';

@Component({
  selector: 'app-summary-quotes-product',
  templateUrl: './summary-quotes-product.component.html',
  styleUrls: ['./summary-quotes-product.component.scss']
})
export class SummaryQuotesProductComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private service: SummaryQuotesService,
    private config: Configuration,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private componentService: ComponentService
  ) { }

  Id: string;
  fileTemplate = '';
  nameFile = "";
  fileToUpload: File = null;
  listData: [];
  startIndex = 1;
  TotalAmount = 0;
  
  listModule: any[] = []; //1
  listProduct: any[] = []; //2
  listSaleProduct: any[] = []; //3
  listMaterial: any[] = []; //4
  listIndustry: any[] = [];
  listUnit: any[] = [];
  listManufacture: any[] = [];

  QuotationStatus: number;

  ngOnInit(): void {
    this.Id = this.route.snapshot.paramMap.get('Id');
    this.service.getQuotationById(this.Id ).subscribe(data => {
      this.appSetting.PageTitle = 'Chỉnh sửa báo giá' + ' - '  + data.data.Code;
      this.getQuotationProduct(this.Id);
      this.QuotationStatus = data.data.Status;
    });
    
    this.getCbbData();
  }

  getCbbData() {
    let cbbModule = this.comboboxService.getCbbModule();
    let cbbProduct = this.comboboxService.getCbbProduct();
    let cbbSaleProduct = this.comboboxService.getCbbSaleProduct();
    let cbbUnit = this.comboboxService.getCbbUnit();
    let cbbManufacture = this.comboboxService.getCbbManufacture();
    let CbbIndustry = this.comboboxService.getCbbIndustry();
    let CbbMaterial = this.comboboxService.getListMaterial()
    forkJoin([cbbModule, cbbProduct, cbbSaleProduct, cbbUnit, cbbManufacture, CbbIndustry, CbbMaterial]).subscribe(results => {
      this.listModule = results[0];
      this.listProduct = results[1];
      this.listSaleProduct = results[2];
      this.listUnit = results[3];
      this.listManufacture = results[4];
      this.listIndustry = results[5];
      this.listMaterial = results[6];
    });
  }

  getListModule() {
    this.comboboxService.getListModule().subscribe(
      data => {
        this.listModule = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListProduct() {
    this.comboboxService.getListProduct().subscribe(
      data => {
        this.listProduct = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbSaleProduct() {
    this.comboboxService.getCbbSaleProduct().subscribe(
      data => {
        this.listSaleProduct = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbUnit() {
    this.comboboxService.getCbbUnit().subscribe(
      data => {
        this.listUnit = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbManufacture() {
    this.comboboxService.getCbbManufacture().subscribe(
      data => {
        this.listManufacture = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbIndustry() {
    this.comboboxService.getCbbIndustry().subscribe(
      data => {
        this.listIndustry = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }



  getQuotationProduct(Id: string){
    this.service.getQuotationProduct(Id).subscribe(
      data => {
        this.listData = data.data;
        this.TotalAmount = data.TotalAmount;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.nameFile = files.item(0).name;
  }

  showCreate(){
    let activeModal = this.modalService.open(SummaryQuotesProductCreateComponent, { container: 'body', windowClass: 'summaryQuotesProductCreate-model', backdrop: 'static' })
    activeModal.componentInstance.QuotationId = this.Id;
    activeModal.componentInstance.listModule = this.listModule;
    activeModal.componentInstance.listProduct = this.listProduct;
    activeModal.componentInstance.listSaleProduct = this.listSaleProduct;
    activeModal.componentInstance.listMaterial = this.listMaterial;
    activeModal.componentInstance.listIndustry = this.listIndustry;
    activeModal.componentInstance.listUnit = this.listUnit;
    activeModal.componentInstance.listManufacture = this.listManufacture;
    activeModal.result.then((result) => {
        this.getQuotationProduct(this.Id);
    }, (reason) => {
    });
  }

  showUpdate(Id){
    let activeModal = this.modalService.open(SummaryQuotesProductCreateComponent, { container: 'body', windowClass: 'summaryQuotesProductCreate-model', backdrop: 'static' })
    activeModal.componentInstance.QuotationId = this.Id;
    activeModal.componentInstance.QuotationProductId = Id;
    activeModal.componentInstance.listModule = this.listModule;
    activeModal.componentInstance.listProduct = this.listProduct;
    activeModal.componentInstance.listSaleProduct = this.listSaleProduct;
    activeModal.componentInstance.listMaterial = this.listMaterial;
    activeModal.componentInstance.listIndustry = this.listIndustry;
    activeModal.componentInstance.listUnit = this.listUnit;
    activeModal.componentInstance.listManufacture = this.listManufacture;
    activeModal.result.then((result) => {
        this.getQuotationProduct(this.Id);
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
    
  }
  delete(Id: string) {
    this.service.deleteProduct(Id).subscribe(
      data => {
        this.messageService.showSuccess('Xóa thiết bị thành công!');
        this.getQuotationProduct(this.Id);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showImportPopup() {
    this.service.getGroupInTemplate().subscribe(d => {
      this.fileTemplate = this.config.ServerApi + d;
      this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
        if (data) {
          this.service.importFile(data, this.Id).subscribe(
            data => {
              this.getQuotationProduct(this.Id);
              this.messageService.showSuccess('Import hãng sản xuất thành công!');
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
}
