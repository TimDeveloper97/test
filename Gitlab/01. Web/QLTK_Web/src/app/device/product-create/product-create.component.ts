import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

import { MessageService, Configuration, FileProcess, AppSetting, Constants, ComboboxService, PermissionService } from 'src/app/shared';

import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProductCreatesService } from '../services/product-create.service';
import { ShowProductModuleUpdateComponent } from '../show-product-module-update/show-product-module-update.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';
import { ProductUpdateContentComponent } from '../product-update-content/product-update-content.component';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.scss']
})
export class ProductCreateComponent implements OnInit {

  constructor(
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public constants: Constants,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private router: Router,
    private productCreatesService: ProductCreatesService,
    private routeA: ActivatedRoute,
    public fileProcess: FileProcess,
    private modalService: NgbModal,
    public fileProcessDataSheet: FileProcess,
    public permissionService: PermissionService,
    private serviceHistory: HistoryVersionService,
    private productService: ProductService,
  ) { }

  listProductGroup: any = [];
  treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  modelProduct: any = {
    ProductGroupId: '',
    Code: '',
    Name: '',
    DepartmentId: '',
    ProcedureTime: '',
    Pricing: '',
    Status: '2',
    Description: '',
    CurentVersion: 0,
    Content: '',
    IsManualExist: false,
    IsQuoteExist: false,
    IsPracticeExist: false,
    IsLayoutExist: false,
    IsMaterialExist: false,
    IsManualMaintenance: false,
    ListImage: [],
    ListFielDocument: [],
    ListProductModuleUpdate: [],
    ListFileCatalog: [],
    IsTestResult: ''
  }

  listStatus: any[] = [
    { Id: '1', Name: 'Chỉ dùng 1 lần', BadgeClass: 'badge-danger', TextClass: '' },
    { Id: '2', Name: 'Thiết bị chuẩn', BadgeClass: 'badge-warning', TextClass: '' },
    { Id: '3', Name: 'Ngừng sử dụng', BadgeClass: 'badge-success', TextClass: '' }
  ];

  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  ListImage = [];

  fileImage = {
    Id: '',
    ProductId: '',
    FileName: '',
    FilePath: '',
    ThumbnailPath: '',
    Note: ''
  }

  fileProductDocument = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: '',
    IsFileUpload: false,
    UpdateDate: null,
    File: null
  }

  fileProductCatalog = {
    Id: '',
    FilePath: '',
    FileName: '',
    FileSize: '',
    Note: '',
    FileType: '',
    IsFileUpload: false,
    UpdateDate: null,
    File: null
  };

  valid: boolean;
  isUpdate = false;
  EmployeeName: any
  DepartmentName: any;
  ngOnInit() {
    this.modelProduct.Id = this.routeA.snapshot.paramMap.get('Id');
    this.modelProduct.ProductGroupId = this.routeA.snapshot.paramMap.get('GroupId');
    this.appSetting.PageTitle = "Thêm mới Thiết bị";

    this.getCbbProductGroup();

    if (this.modelProduct.Id != '' && this.modelProduct.Id != null) {
      this.valid = true;
      this.isUpdate = true;
      this.getProductInfo();
    } else {
      this.EmployeeName = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeName;
      this.DepartmentName = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentName;
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

  thumbnailActions: NgxGalleryOptions[];
  checkDelete = false;
  deleteImage(event, index): void {
    let imageDelete = this.galleryImages[index].medium;
    let indexDelete;
    this.modelProduct.ListImage.forEach(element => {
      let url = this.config.ServerFileApi + element.ThumbnailPath;
      if (url == imageDelete) {
        indexDelete = this.modelProduct.ListImage.indexOf(element);
      }
    });
    this.messageService.showConfirm("Bạn có chắc muốn xóa ảnh không").then(
      data => {
        this.galleryImages.splice(index, 1);
        this.modelProduct.ListImage.splice(indexDelete, 1);
      },
      error => {
        
      });
  }

  getProductInfo() {
    this.productCreatesService.getProductInfo(this.modelProduct).subscribe(
      data => {
        this.modelProduct = data;
        this.EmployeeName = data.Creator;
        this.DepartmentName = data.DepartmentName;
        this.appSetting.PageTitle = "Cập nhật Thiết bị - " + this.modelProduct.Code + " - " + this.modelProduct.Name;
        this.treeBoxValue = data.ProductGroupId;
        for (var item of data.ListImage) {
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.ThumbnailPath,
            medium: this.config.ServerFileApi + item.FilePath,
            big: this.config.ServerFileApi + item.FilePath
          });
        }
        if (this.modelProduct.ListProductModuleUpdate.length > 0) {
          this.showProductModuleUpdate();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showProductModuleUpdate() {
    let activeModal = this.modalService.open(ShowProductModuleUpdateComponent, { container: 'body', windowClass: 'show-product-module-update', backdrop: 'static' });
    activeModal.componentInstance.listProductModuleUpdate = this.modelProduct.ListProductModuleUpdate;
    activeModal.componentInstance.productId = this.modelProduct.Id;
    activeModal.result.then((result) => {
    }, (reason) => {
    });
  }

  check() {
    if (this.modelProduct.Name != '' && this.modelProduct.Name != undefined && this.modelProduct.Code != '' && this.modelProduct.Code != undefined
      && this.modelProduct.Status != '' && this.modelProduct.Status != undefined
      && this.modelProduct.ProductGroupId != '' && this.modelProduct.ProductGroupId != undefined
      && this.modelProduct.CurentVersion != '' && this.modelProduct.CurentVersion != undefined) {
      this.valid = true;
    } else {
      this.valid = false;
    }
  }


  save(isContinue: boolean) {
    var validCode = this.checkSpecialCharacter.checkCode(this.modelProduct.Code);
    if (validCode) {
      this.saveInfo(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.saveInfo(isContinue);
        },
        error => {
          
        });
    }
  }

  saveInfo(isContinue) {
    if (!this.modelProduct.Id) {
      this.createProduct(isContinue);
    } else {
      this.updateProduct();
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  createProduct(isContinue) {
    this.productCreatesService.createProduct(this.modelProduct).subscribe(
      data => {
        if (isContinue) {
          this.modelProduct = {
            ProductGroupId: '',
            ListImage: [],
          };
          this.ListImage = [];
          this.galleryImages = [];
          this.treeBoxValue = '';
          this.messageService.showSuccess('Thêm mới thiết bị thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới thiết bị thành công!');
          this.closeModal();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateProduct() {
    this.productCreatesService.updateProduct(this.modelProduct).subscribe(
      () => {
        this.closeModal();
        this.messageService.showSuccess('Cập nhật thiết bị thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.router.navigate(['thiet-bi/quan-ly-thiet-bi']);
  }

  uploadFileClickImage($event) {
    this.ListImage = [];
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.ListImage.push(item);
    }

    this.uploadService.uploadListFile(this.ListImage, 'ImageModule/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.FilePath = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
          this.modelProduct.ListImage.push(file);
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.FileUrlThum,
            medium: this.config.ServerFileApi + item.FileUrlThum,
            big: this.config.ServerFileApi + item.FileUrl
          });
        });
      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  // Lấy combobox nhóm thiết bị
  getCbbProductGroup() {
    this.comboboxService.getCbbProductGroup().subscribe((data: any) => {
      if (data) {
        this.listProductGroup = data.ListResult;


      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  changeGroup() {
    if (!this.modelProduct.Id) {
      var group = this.listProductGroup.filter(t => t.Id == this.modelProduct.ProductGroupId);
      if (group.length > 0) {
        this.modelProduct.Code = group[0].Code;
      }
    }
  }

  syncTreeViewSelection(e) {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.modelProduct.ProductGroupId = this.treeBoxValue;
    this.check();
    this.closeDropDownBox();
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  showConfirmUploadVersion() {
    if (this.modelProduct.Id) {
      this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
        async data => {
          if (data) {
            await this.showEditContent();
          } else {
            this.save(false);
          }
        }
      );
    } else {
      this.save(false);
    }
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.modelProduct.Id;
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Product;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save(false);
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  changeTab(event) {
    if (event.nextId == '0') {
      this.getProductInfo();
    }
  }

  showUpdateContent() {
    let activeModal = this.modalService.open(ProductUpdateContentComponent, { container: 'body', windowClass: 'product-update-conten', backdrop: 'static' });
    activeModal.componentInstance.productId = this.modelProduct.Id;
    activeModal.result.then((result) => {
      if (result) {
        this.getProductInfo();
      }
    }, (reason) => {
    });
  }

  isConfirm: boolean = false;
  syncSaleProduct(isConfirm: boolean) {
    let list = [];
    list.push(this.modelProduct.Id);
    this.productService.syncSaleProduct(false, isConfirm, list).subscribe(
      data => {
        if (data) {
          this.confirm(data);
        } else {
          this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  confirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.syncSaleProduct(true);
      }
    );
  }
}
