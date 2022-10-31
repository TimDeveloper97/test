import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, Constants, PermissionService, DateUtils } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { SaleProductService } from '../sale-product.service';
import { DecimalPipe } from '@angular/common';
import { ManufacturerCreateComponent } from 'src/app/material/manufacturer/manufacturer-create/manufacturer-create.component';
import { SaleProductTypeService } from '../../service/sale-product-type.service';

@Component({
  selector: 'app-product-for-business-create',
  templateUrl: './product-for-business-create.component.html',
  styleUrls: ['./product-for-business-create.component.scss']
})
export class ProductForBusinessCreateComponent implements OnInit {

  constructor(
    private modalService: NgbModal,
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
    private saleProductTypeService: SaleProductTypeService,
  ) { }

  validMessage: any = '';
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
    SaleProductTypeId: '',
    SaleProductJobModels: [],
    SaleProductAppModels: [],
    SaleProductDocumnetModels: [],
    SaleProductMediaModels: [],
    SaleGroupProduct: [],
    SaleProductAccessoryModels: [],
  }
  specificationDate: any;
  eXWTPADate: any;
  expireDateFrom: any;
  expireDateTo: any;
  listJob: any[] = [];
  listApp: any[] = [];
  listDocument: any[] = [];
  listOldDocument: any[] = [];
  fileDocument = {
    Path: '',
    FileName: '',
    FileSize: 0,
    Type: 0,
    CreateDate: new Date(),
  }
  modelAppCaree = {
    listApp: [],
    listCaree: [],
  };
  modelListDocument = {
    listCatalog: [],
    listFileSolution: [],
    listTechnicalTraning: [],
    listSaleTraining: [],
    listUserManual: [],
    listFixError: [],
    listOtherDocument: [],
  };
  listCatalog: any[] = [];
  listFileSolution: any[] = [];
  listTechnicalTraning: any[] = [];
  listSaleTraining: any[] = [];
  listUserManual: any[] = [];
  listFixError: any[] = [];
  listOtherDocument: any[] = [];
  listMedia: any[] = [];
  listAccessory: any[] = [];
  listGroupSaleProduct: any[] = [];
  saleProductTypes: any[] = [];
  columnNameManufacture: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên chủng loại' }];
  columnNameSupplier: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name_NCC_SX', Title: 'Tên nhóm' }];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  listImage = [];
  listCountry = [];
  fileImage = {
    Path: '',
    FileName: '',
    FileSize: 0,
    Type: 0,
    CreateDate: new Date(),
  }
  Valid: boolean;
  productManufacture: any[];
  isUpdate = false;

  exwRate: number = 0;
  publicRate: number = 0;

  ngOnInit() {
    this.getListCountry();
    this.getCBBSaleProductType();
    this.getCBBProductManufacture();
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    if (this.model.Id) {
      this.appSetting.PageTitle = "Cập nhật thư viện sản phẩm kinh doanh";
      this.Valid = true;
      this.isUpdate = true;
      this.getInfo(this.model.Id);
    } else {
      this.appSetting.PageTitle = "Thêm mới thư viện sản phẩm kinh doanh";
    }
    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
  }

  showAddManufacture() {
    let activeModal = this.modalService.open(ManufacturerCreateComponent, { container: 'body', windowClass: 'manufacturecreate-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.getCBBProductManufacture();
      }
    }, (reason) => {
    });
  }
  getInfo(id) {
    this.saleProductService.getProductInfoByProductId(id).subscribe((data: any) => {
      if (data) {
        this.model = data;
        console.log(this.model);
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
        this.listImage = data.ListImage;
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
  getCBBSaleProductType() {
    this.comboboxService.getCBBSaleProductType().subscribe((data: any) => {
      if (data) {
        this.saleProductTypes = data;
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
  deleteImage(event, index): void {
    let imageDelete = this.galleryImages[index].medium;
    let indexDelete;
    this.listImage.forEach(element => {
      let url = this.config.ServerFileApi + element.Path;
      if (url == imageDelete) {
        indexDelete = this.listImage.indexOf(element);
      }
    });
    this.messageService.showConfirm("Bạn có chắc muốn xóa ảnh không").then(
      data => {
        this.galleryImages.splice(index, 1);
        this.listImage.splice(indexDelete, 1);
      },
      error => {

      });
  }
  save(isContinue) {
    this.model.SaleProductJobModels = [];
    this.model.SaleProductMediaModels = [];
    this.model.SaleProductAppModels = [];
    this.model.SaleProductDocumnetModels = [];
    this.model.SaleProductAccessoryModels = [];
    this.model.SaleGroupProduct = [];
    this.listDocument = [];
    this.listOldDocument = [];
    this.modelListDocument.listCatalog.forEach((element: any) => {
      if (element.File != null) {
        this.listDocument.push(element);
      }
      else {
        this.listOldDocument.push(element);
      }
    });
    this.modelListDocument.listFileSolution.forEach((element: any) => {
      if (element.File != null) {
        this.listDocument.push(element);
      } else {
        this.listOldDocument.push(element);
      }
    });
    this.modelListDocument.listTechnicalTraning.forEach((element: any) => {
      if (element.File != null) {
        this.listDocument.push(element);
      } else {
        this.listOldDocument.push(element);
      }
    });
    this.modelListDocument.listSaleTraining.forEach((element: any) => {
      if (element.File != null) {
        this.listDocument.push(element);
      } else {
        this.listOldDocument.push(element);
      }
    });
    this.modelListDocument.listUserManual.forEach((element: any) => {
      if (element.File != null) {
        this.listDocument.push(element);
      } else {
        this.listOldDocument.push(element);
      }
    });
    this.modelListDocument.listFixError.forEach((element: any) => {
      if (element.File != null) {
        this.listDocument.push(element);
      } else {
        this.listOldDocument.push(element);
      }
    });
    this.modelListDocument.listOtherDocument.forEach((element: any) => {
      if (element.File != null) {
        this.listDocument.push(element);
      } else {
        this.listOldDocument.push(element);
      }
    });
    this.modelAppCaree.listApp.forEach((element: any) => {
      this.model.SaleProductAppModels.push(element.Id);
    });
    this.listGroupSaleProduct.forEach((element: any) => {
      this.model.SaleGroupProduct.push(element.Id);
    });
    this.modelAppCaree.listCaree.forEach((element: any) => {
      this.model.SaleProductJobModels.push(element.Id);
    });

    this.listAccessory.forEach((element: any) => {
      this.model.SaleProductAccessoryModels.push(element.Id);
    });

    this.listMedia.forEach((element: any) => {
      this.model.SaleProductMediaModels.push(element);
    });
    this.listImage.forEach((element: any) => {
      this.model.SaleProductMediaModels.push(element);
    });
    if (this.specificationDate) {
      this.model.SpecificationDate = this.dateUtils.convertObjectToDate(this.specificationDate);
    }

    if (this.expireDateFrom) {
      this.model.ExpireDateFrom = this.dateUtils.convertObjectToDate(this.expireDateFrom);
    }
    if (this.expireDateTo) {
      this.model.ExpireDateTo = this.dateUtils.convertObjectToDate(this.expireDateTo);
    }
    if (this.eXWTPADate) {
      this.model.EXWTPADate = this.dateUtils.convertObjectToDate(this.eXWTPADate);
    }
    if (!this.model.Id) {
      this.createProductForBusiness(isContinue);
    } else {
      this.updateProductForBusiness();
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  createProductForBusiness(isContinue: boolean) {
    var listTypeFileUpload = [];
    this.listDocument.forEach((element: any) => {
      listTypeFileUpload.push(element.Type);
    });
    this.uploadService.uploadListFile(this.listDocument, 'SaleProductDocument/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.Type = listTypeFileUpload[index];
          file.CreateDate = new Date();
          this.model.SaleProductDocumnetModels.push(file);
        });
      }
      for (var item of this.model.SaleProductDocumnetModels) {
        if (item.Path == null || item.Path == "") {
          this.listDocument.splice(this.listDocument.indexOf(item), 1);
        }
      }

      let isValid = this.valid();
      if (isValid) {
        this.saleProductService.createSaleProduct(this.model).subscribe(
          data => {
            if (isContinue) {
              this.model = {
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
                SaleProductTypeId: '',
                SaleProductJobModels: [],
                SaleProductAppModels: [],
                SaleProductDocumnetModels: [],
                SaleProductMediaModels: [],
                SaleGroupProduct: [],
                SaleProductAccessoryModels: [],
              };
              this.modelAppCaree = {
                listApp: [],
                listCaree: [],
              };
              this.specificationDate = null;
              this.eXWTPADate = null;
              this.expireDateFrom = null;
              this.expireDateTo = null
              this.listJob = [];
              this.listApp = [];
              this.listDocument = [];
              this.listOldDocument = [];
  
              this.modelListDocument = {
                listCatalog: [],
                listFileSolution: [],
                listTechnicalTraning: [],
                listSaleTraining: [],
                listUserManual: [],
                listFixError: [],
                listOtherDocument: [],
              };
              this.listCatalog = [];
              this.listFileSolution = [];
              this.listTechnicalTraning = [];
              this.listSaleTraining = [];
              this.listUserManual = [];
              this.listFixError = [];
              this.listOtherDocument = [];
              this.listMedia = [];
              this.listAccessory = [];
              this.listGroupSaleProduct = [];
              this.listImage = [];
              this.listDocument = [];
              this.galleryImages = [];
              this.messageService.showSuccess('Thêm mới sản phẩm cho kinh doanh thành công!');
            }
            else {
              this.messageService.showSuccess('Thêm mới sản phẩm cho kinh doanh thiết bị thành công!');
              this.closeModal();
            }
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
      else{
        this.messageService.showMessage(this.validMessage);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  updateProductForBusiness() {
    var listTypeFileUpload = [];
    this.listDocument.forEach((element: any) => {
      listTypeFileUpload.push(element.Type);
    });
    this.model.SaleProductDocumnetModels = this.listOldDocument;
    this.uploadService.uploadListFile(this.listDocument, 'SaleProductDocument/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileDocument);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.Type = listTypeFileUpload[index];
          file.CreateDate = new Date();
          this.model.SaleProductDocumnetModels.push(file);
        });
      }
      for (var item of this.model.SaleProductDocumnetModels) {
        if (item.Path == null || item.Path == "") {
          this.listDocument.splice(this.listDocument.indexOf(item), 1);
        }
      }
      let isValid = this.valid();
      if (isValid) {
        this.saleProductService.updateSaleProduct(this.model.Id, this.model).subscribe(
          data => {

            this.messageService.showSuccess('Sửa sản phẩm cho kinh doanh thiết bị thành công!');
            this.closeModal();
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
      else{
        this.messageService.showMessage(this.validMessage);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.router.navigate(['kinh-doanh/san-pham-cho-kinh-doanh']);
  }

  check() {
    if (this.model.VName != '' && this.model.Model != '' &&
      this.model.PublicPrice != '') {
      this.Valid = true;
    } else {
      this.Valid = false;
    }
  }

  uploadFileClickImage($event) {

    //this.listImage = [];
    var listFileImage = []
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      item.Type = 1;
      listFileImage.push(item);
    }
    this.uploadService.uploadListFile(listFileImage, 'ImageProductBusiness/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.Path = item.FileUrl;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.FileName = item.FileName;
          file.Type = 1;
          file.CreateDate = new Date();
          this.listImage.push(file);
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.FileUrl,
            medium: this.config.ServerFileApi + item.FileUrl,
            big: this.config.ServerFileApi + item.FileUrl
          });
        });
      }
      for (var item of this.listImage) {
        if (item.Path == null || item.Path == "") {
          this.listImage.splice(this.listImage.indexOf(item), 1);
        }
      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  getRate() {
    if (this.model.SaleProductTypeId) {
      this.saleProductTypeService.getTypeInfo({ Id: this.model.SaleProductTypeId }).subscribe(data => {
        this.exwRate = data.EXWRate;
        this.publicRate = data.PublicRate;
        this.model.EXWTPAPrice = Number(this.exwRate) * Number(this.model.MaterialPrice);
        this.model.PublicPrice = Number(this.publicRate) * Number(this.model.MaterialPrice);
      });
    }

  }

  changeMaterialPrice() {
    this.model.EXWTPAPrice = Number(this.exwRate) * Number(this.model.MaterialPrice);
    this.model.PublicPrice = Number(this.publicRate) * Number(this.model.MaterialPrice);
  }

  valid() {
    if (!this.model.ManufactureId) {
      this.validMessage = 'Chưa chọn Mã hãng sản xuất. Vui lòng kiểm tra lại!';
      return false;
    }

    if (!this.model.SaleProductTypeId) {
      this.validMessage = 'Chưa chọn Chủng loại hàng hóa. Vui lòng kiểm tra lại!';
      return false;
    }

    if (this.model.SaleProductMediaModels.length == 0) {
      this.validMessage = 'Chưa có ảnh thiết bị. Vui lòng kiểm tra lại!';
      return false;
    }
    return true;
  }
}