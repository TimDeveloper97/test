import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, Constants, PermissionService, DateUtils } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { SaleProductService } from '../sale-product.service';
import { DecimalPipe } from '@angular/common';
@Component({
  selector: 'app-product-business-details',
  templateUrl: './product-business-details.component.html',
  styleUrls: ['./product-business-details.component.scss']
})
export class ProductBusinessDetailsComponent implements OnInit {

  constructor(
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public constants: Constants,
    private router: Router,
    private routeA: ActivatedRoute,
    public fileProcess: FileProcess,
    public dateUtils: DateUtils,
    public permissionService: PermissionService,
    public saleProductService: SaleProductService,
  ) { }

  model: any = {
    Id: '',
    EName: '',
    VName: '',
    Model: '',
    GroupCode: '',
    ChildGroupCode: '',
    ManufactureId: null,
    CountryName: null,
    Specifications: '',
    CustomerSpecifications: '',
    SpecificationDate: null,
    VAT: 0,
    MaterialPrice: 0,
    EXWTPAPrice: 0,
    EXWTPADate: null,
    PublicPrice: 0,
    ExpireDateFrom: null,
    ExpireDateTo: null,
    DeliveryTime: '',
    ProductStandardTPATypeId: [],
    SaleProductJobModels: [],
    SaleProductAppModels: [],
    SaleProductDocumnetModels: [],
    SaleProductMediaModels: [],
    SaleProductAccessoryModels: [],
  }
  specificationDate: any;
  eXWTPADate: any;
  expireDateFrom: any;
  expireDateTo: any;
  productStandardTPATypes: any[] = [];
  columnName: any[] = [{ Name: 'Name', Title: 'Tên chủng loại' }];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  listImage = [];
  listCountry = [];
  productManufacture: any[];
  valid: boolean;
  isUpdate = false;
  ngOnInit() {
    this.getListCountry();
    this.getCBBProductStandardTPAType();
    this.getCBBProductManufacture();
    this.appSetting.PageTitle = "Chi tiết sản phẩm kinh doanh";
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getInfo(this.model.Id);
    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
  }
  getInfo(id){
    this.saleProductService.getProductInfoByProductId(id).subscribe((data: any) => {
      if (data) {
        this.model = data;
        if (data.SpecificationDate) {
          this.specificationDate = this.dateUtils.convertDateToObject(data.SpecificationDate);
        }
        if (data.ExpireDateFrom) {
          this.expireDateFrom = this.dateUtils.convertDateToObject(data.ExpireDateFrom);
        }
        if (data.ExpireDateTo) {
          this.expireDateTo = this.dateUtils.convertDateToObject(data.ExpireDateTo);
        }
        if (data.EXWTPADate) {
          this.eXWTPADate = this.dateUtils.convertDateToObject(data.EXWTPADate);
        }
        this.listImage=data.ListImage;
        for (var item of data.ListImage) {
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.Path,
            medium: this.config.ServerFileApi + item.Path,
            big: this.config.ServerFileApi + item.Path
          });
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  getCBBProductStandardTPAType() {
    this.comboboxService.getCBBProductStandardTPAType().subscribe((data: any) => {
      if (data) {
        this.productStandardTPATypes = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  getCBBProductManufacture() {
    this.comboboxService.getCbbManufacture().subscribe((data: any) => {
      if (data) {
        this.productManufacture = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  getListCountry() {
    this.comboboxService.getListCountry().subscribe((data: any) => {
      if (data) {
        this.listCountry = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  thumbnailActions: NgxGalleryOptions[];
  checkDelete = false;

  showCreateExportAndKeep(){
    this.router.navigate(['kinh-doanh/danh-sach-xuat-giu/them-moi/'+this.model.Id]);
  }

  closeModal() {
    this.router.navigate(['kinh-doanh/san-pham-cho-kinh-doanh']);
  }

}