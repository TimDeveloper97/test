import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, Constants, PermissionService, DateUtils } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductStandardTpaService } from '../../services/product-standard-tpa.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-product-standard-tpa-view',
  templateUrl: './product-standard-tpa-view.component.html',
  styleUrls: ['./product-standard-tpa-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductStandardTpaViewComponent implements OnInit {

  constructor(
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public constants: Constants,
    private router: Router,
    private service: ProductStandardTpaService,
    private routeA: ActivatedRoute,
    public fileProcess: FileProcess,
    private modalService: NgbModal,
    public dateUtils: DateUtils,
    public permissionService: PermissionService,
  ) { }

  updateDatePrice_Weight_PriceWord = null;
  updateDatePrice_BCM_PriceWord = null;
  updateDatePrice_Cont20_PriceWord = null;
  updateDatePrice_Cont20OT_PriceWord = null;
  updateDatePrice_Cont40_PriceWord = null;
  updateDatePice_Cont40OT_PriceWord = null;
  updateDateHSCode = null;
  updateDatePrice_TPA = null;
  model: any = {
    Id: '',
    EnglishName: '',
    VietNamName: '',
    Model: '',
    TheFirm: '',
    TypeMerchandise: 0,
    Manufacture_NCC_SX: '',
    Supplier_NCC_SX: '',
    Name_NCC_SX: '',
    Address_NCC_SX: '',
    PIC_NCC_SX: '',
    PhoneNumber_NCC_SX: '',
    Email_NCC_SX: '',
    Title_NCC_SX: '',
    BankPayment_NCC_SX: '',
    TypePayment_NCC_SX: 0,
    RulesPayment_NCC_SX: '',
    RulesDelivery: 0,
    DeliveryTime: '',
    ListedPrice: 0,
    PriceConformQuantity: '',
    PricePolicy: '',
    MinimumQuantity: 0,
    MethodVC: 0,
    LoadingPort: '',
    PriceInSP_Price: 0,
    PriceInPO_Price: 0,
    Currency_PO: 0,
    Ratio_PO: 0,
    Amout_PO: 0,
    Weight_PriceWord: 0,
    PriceVCInWeight_PriceWord: 0,
    VolumePricing_PriceWord: 0,
    CPVCInWeight_Price_PriceWord: 0,
    CPVCInWeight_Currency_PriceWord: 0,
    CPVCInWeight_Ratio_PriceWord: 0,
    CPVCInWeight_AmoutVND_PriceWord: 0,
    Weight_Air_PriceWord: 0,
    MassConvertedBySize_Air_PriceWord: 0,
    VolumePricing_Air_PriceWord: 0,
    Price_AirTransport_Air_PriceWord: 0,
    Curency_AirTransport_Air_PriceWord: 0,
    Ratio_AirTransport_Air_PriceWord: 0,
    Amount_AirTransport_Air_PriceWord: 0,
    PackingSize_D_PriceWord: 0,
    PackingSize_R_PriceWord: 0,
    PackingSize_C_PriceWord: 0,
    PackingSize_Tolerance_PriceWord: 0,
    PackingSize_RealVolume_PriceWord: 0,
    PackingSize_PiceVolume_PriceWord: 0,
    PriceVCInCBM_PriceWord: 0,
    UpdateDatePrice_BCM_PriceWord: null,
    Price_PriceVC_LCLInBCM_PriceWord: 0,
    Currency_PriceVC_LCLInBCM_PriceWord: 0,
    Ratio_PriceVC_LCLInBCM_PriceWord: 0,
    Amout_PriceVC_LCLInBCM_PriceWord: 0,
    Price_PriceVC_Cont20_PriceWord: 0,
    Currency_PriceVC_Cont20_PriceWord: 0,
    Ratio_PriceVC_Cont20_PriceWord: 0,
    Amout_PriceVC_Cont20_PriceWord: 0,
    Price_PriceVC_Cont20OT_PiceWord: 0,
    Currency_PriceVC_Cont20OT_PiceWord: 0,
    Ratio_PriceVC_Cont20OT_PiceWord: 0,
    Amount_PriceVC_Cont20OT_PiceWord: 0,
    Price_PriceVC_Cont40_PriceWord: 0,
    Currency_PriceVC_Cont40_PriceWord: 0,
    Ratio_PriceVC_Cont40_PriceWord: 0,
    Amount_PriceVC_Cont40_PriceWord: 0,
    Price_PriceVC_Cont40OT_PriceWord: 0,
    Currency_PriceVC_Cont40OT_PriceWord: 0,
    Ratio_PriceVC_Cont40OT_PriceWord: 0,
    Amount_PriceVC_Cont40OT_PriceWord: 0,
    UpdateDatePice_PriceWord: null,
    Price_PriceLCExport: 0,
    Currency_PriceLCExport: 0,
    Ratio_PriceLCExport: 0,
    Amount_PriceLCExport: 0,
    InsurranceType: 0,
    Price_InsurrancePrice: 0,
    Currency_InsurrancePrice: 0,
    Ratio_InsurrancePrice: 0,
    Amount_InsurrancePrice: 0,
    Price_PriceLCImport_LSS: 0,
    Currency_PriceLCImport_LSS: 0,
    Ratio_PriceLCImport_LSS: 0,
    Amount_PriceLCImport_LSS: 0,
    Price_PriceLCImport_THC: 0,
    Currency_PriceLCImport_THC: 0,
    Ratio_PriceLCImport_THC: 0,
    Amount_PriceLCImport_THC: 0,
    Price_PriceLCImport_DO: 0,
    Currency_PriceLCImport_DO: 0,
    Ratio_PriceLCImport_DO: 0,
    Amount_PriceLCImport_DO: 0,
    Price_PriceLCImport_CIC: 0,
    Currency_PriceLCImport_CIC: 0,
    Ratio_PriceLCImport_CIC: 0,
    Amount_PriceLCImport_CIC: 0,
    Price_PriceLCImport_HL: 0,
    Currency_PriceLCImport_HL: 0,
    Ratio_PriceLCImport_HL: 0,
    Amount_PriceLCImport_HL: 0,
    Price_PriceLCImport_CLF: 0,
    Currency_PriceLCImport_CLF: 0,
    Ratio_PriceLCImport_CLF: 0,
    Amount_PriceLCImport_CLF: 0,
    Price_PriceLCImport_CFS: 0,
    Currency_PriceLCImport_CFS: 0,
    Ratio_PriceLCImport_CFS: 0,
    Amount_PriceLCImport_CFS: 0,
    Price_PriceLCImport_Lift: 0,
    Currency_PriceLCImport_Lift: 0,
    Ratio_PriceLCImport_Lift: 0,
    Amount_PriceLCImport_Lift: 0,
    Price_PriceLCImport_IF: 0,
    Currency_PriceLCImport_IF: 0,
    Ratio_PriceLCImport_IF: 0,
    Amount_PriceLCImport_IF: 0,
    Price_PriceLCImport_Other: 0,
    Currency_PriceLCImport_Other: 0,
    Ratio_PriceLCImport_Other: 0,
    Amount_PriceLCImport_Other: 0,
    HSCode: '',
    ImportTax: 0,
    ImportTaxPrice: 0,
    UpdateDateHSCode: null,
    NameTaxOther: '',
    TaxOther: 0,
    ImportTaxPriceOther: 0,
    VAT: 0,
    PriceOther: 0,
    PriceFW: 0,
    Surcharge: 0,
    InventoryTime: 0,
    ShortInterest: 0,
    MidtermInterest: 0,
    PriceProduct_TPA: 0,
    PriceVC_TPA: 0,
    ImportTaxPrice_TPA: 0,
    ImportTax_TPA: 0,
    VAT_TPA: 0,
    Interest_TPA: 0,
    TotalPrice: 0,
    Profit_TPA: 0,
    PriceEXW_TPA: 0,
    UpdateDatePrice_TPA: null,
    Price_L1: 0,
    Price_L2: 0,
    Price_L3: 0,
    Price_L4: 0,
    Price_L5: 0,
    BusinessDepartment: '',
    Index: 0,
    Specifications: '',
    ListFile: [],
    ListImage: []
  }

  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  listImage = [];

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

  columnNameSupplier: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name_NCC_SX', Title: 'Tên nhóm' }];

  productStandardTPATypes: any[] = [];
  listSupplier = [];
  valid: boolean;
  isUpdate = false;
  ngOnInit() {
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getlistSupplier();
    if (this.model.Id) {
      this.appSetting.PageTitle = "Xem thông tin thiết bị tiêu chuẩn";
      this.valid = true;
      this.isUpdate = true;
      this.getProductStandardTPAInfo();
    } else {
      this.appSetting.PageTitle = "Thêm mới Thiết bị tiêu chuẩn";
    }
    this.getCBBProductStandardTPAType();
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

  getlistSupplier() {
    this.service.GetSuppliers().subscribe((data: any) => {
      if (data) {
        this.listSupplier = data;
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

  thumbnailActions: NgxGalleryOptions[];
  checkDelete = false;
  deleteImage(event, index): void {
    let imageDelete = this.galleryImages[index].medium;
    let indexDelete;
    this.model.ListImage.forEach(element => {
      let url = this.config.ServerFileApi + element.ThumbnailPath;
      if (url == imageDelete) {
        indexDelete = this.model.ListImage.indexOf(element);
      }
    });
    this.messageService.showConfirm("Bạn có chắc muốn xóa ảnh không").then(
      data => {
        this.galleryImages.splice(index, 1);
        this.model.ListImage.splice(indexDelete, 1);
      });
  }

  getProductStandardTPAInfo() {
    this.service.getProductStandardTPAInfo(this.model).subscribe(
      data => {
        this.model = data;
        this.appSetting.PageTitle = "Cập nhật thiết bị tiêu chuẩn - " + this.model.Model + " - " + this.model.VietNamName;

        if (data.UpdateDatePice_PriceWord) {
          let dateArray = data.UpdateDatePice_PriceWord.split('T')[0];
          let dateValue = dateArray.split('-');
          let tempDateFromV = {
            'day': parseInt(dateValue[2]),
            'month': parseInt(dateValue[1]),
            'year': parseInt(dateValue[0])
          };
          this.updateDatePice_Cont40OT_PriceWord = tempDateFromV;
        }

        if (data.UpdateDateHSCode) {
          let dateArray = data.UpdateDateHSCode.split('T')[0];
          let dateValue = dateArray.split('-');
          let tempDateFromV = {
            'day': parseInt(dateValue[2]),
            'month': parseInt(dateValue[1]),
            'year': parseInt(dateValue[0])
          };
          this.updateDateHSCode = tempDateFromV;
        }

        if (data.UpdateDatePrice_TPA) {
          let dateArray = data.UpdateDatePrice_TPA.split('T')[0];
          let dateValue = dateArray.split('-');
          let tempDateFromV = {
            'day': parseInt(dateValue[2]),
            'month': parseInt(dateValue[1]),
            'year': parseInt(dateValue[0])
          };
          this.updateDatePrice_TPA = tempDateFromV;
        }

        for (var item of data.ListImage) {
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.ThumbnailPath,
            medium: this.config.ServerFileApi + item.FilePath,
            big: this.config.ServerFileApi + item.FilePath
          });
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue) {
    if (!this.model.Id) {
      this.createProductStandardTPA(isContinue);
    } else {
      this.updateProductStandardTPA();
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  createProductStandardTPA(isContinue: boolean) {
    var regex = this.constants.validEmailRegEx;
    if (this.model.Email_NCC_SX) {
      if (!regex.test(this.model.Email_NCC_SX)) {
        return this.messageService.showMessage("Email không đúng!");
      }
    }

    if (this.updateDatePice_Cont40OT_PriceWord) {
      this.model.UpdateDatePice_PriceWord = this.dateUtils.convertObjectToDate(this.updateDatePice_Cont40OT_PriceWord);
    } else { this.model.UpdateDatePice_PriceWord = null; }

    if (this.updateDateHSCode) {
      this.model.UpdateDateHSCode = this.dateUtils.convertObjectToDate(this.updateDateHSCode);
    } else { this.model.UpdateDateHSCode = null; }

    if (this.updateDatePrice_TPA) {
      this.model.UpdateDatePrice_TPA = this.dateUtils.convertObjectToDate(this.updateDatePrice_TPA);
    } else { this.model.UpdateDatePrice_TPA = null; }

    this.service.createProductStandardTPA(this.model).subscribe(
      data => {
        if (isContinue) {
          this.model = {
            ProductGroupId: '',
            ListImage: [],
          };
          this.listImage = [];
          this.galleryImages = [];
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

  updateProductStandardTPA() {
    var regex = this.constants.validEmailRegEx;
    if (this.model.Email_NCC_SX) {
      if (!regex.test(this.model.Email_NCC_SX)) {
        this.messageService.showMessage("Email không đúng!");
      }
    }

    if (this.updateDatePice_Cont40OT_PriceWord) {
      this.model.UpdateDatePice_PriceWord = this.dateUtils.convertObjectToDate(this.updateDatePice_Cont40OT_PriceWord);
    } else { this.model.UpdateDatePice_PriceWord = null; }

    if (this.updateDateHSCode) {
      this.model.UpdateDateHSCode = this.dateUtils.convertObjectToDate(this.updateDateHSCode);
    } else { this.model.UpdateDateHSCode = null; }

    if (this.updateDatePrice_TPA) {
      this.model.UpdateDatePrice_TPA = this.dateUtils.convertObjectToDate(this.updateDatePrice_TPA);
    } else { this.model.UpdateDatePrice_TPA = null; }

    this.service.updateProductStandardTPA(this.model).subscribe(
      () => {
        this.closeModal();
        this.messageService.showSuccess('Cập nhật thiết bị tiêu chuẩn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.router.navigate(['thiet-bi/quan-ly-thiet-bi-nhap-khau']);
  }

  check() {
    if (this.model.VietNamName && this.model.EnglishName && this.model.Model) {
      this.valid = true;
    } else {
      this.valid = false;
    }
  }

  uploadFileClickImage($event) {
    this.listImage = [];
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.listImage.push(item);
    }

    this.uploadService.uploadListFile(this.listImage, 'ImageProductStandardTPA/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.FilePath = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
          this.model.ListImage.push(file);
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
}
