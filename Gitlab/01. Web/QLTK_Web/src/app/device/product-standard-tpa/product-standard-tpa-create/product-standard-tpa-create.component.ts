import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, Constants, PermissionService, DateUtils } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductStandardTpaService } from '../../services/product-standard-tpa.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-product-standard-tpa-create',
  templateUrl: './product-standard-tpa-create.component.html',
  styleUrls: ['./product-standard-tpa-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductStandardTpaCreateComponent implements OnInit {

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
  listCountry: any[] = [];
  model: any = {
    Id: '',
    EnglishName: '',
    VietNamName: '',
    Model: '',
    TheFirm: '',
    ProductStandardTPATypeId: null,
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
    IsCOCQ: false,
    Country: null,
    ListFile: [],
    ListImage: [],
    Note1: '',
    Note2: '',
    Note3: '',
    Note4: '',
    Note5: '',
    Note6: ''
  }

  SupplierId: string;
  columnName: any[] = [{ Name: 'Name', Title: 'Tên chủng loại' }]
  // columnNameSupplier: any[] = [{Code:'Supplier_NCC_SX', Title:'Mã nhà cung cấp'}, { Name: 'Name_NCC_SX', Title: 'Tên nhà cung cấp' }]
  columnNameSupplier: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name_NCC_SX', Title: 'Tên nhóm' }];
  listSupplier = [];
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

  productStandardTPATypes: any[] = [];

  valid: boolean;
  isUpdate = false;
  ngOnInit() {
    this.getListCountry();
    this.getlistSupplier();
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    if (this.model.Id) {
      this.appSetting.PageTitle = "Cập nhật thiết bị tiêu chuẩn";
      this.valid = true;
      this.isUpdate = true;
      this.getProductStandardTPAInfo();
    } else {
      this.appSetting.PageTitle = "Thêm mới Thiết bị tiêu chuẩn";
      let tpaTypeId = localStorage.getItem("productStandardTPATypeId");
      if (tpaTypeId) {
        this.model.ProductStandardTPATypeId = tpaTypeId;
        console.log(tpaTypeId);
      }
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

  changeSupplier() {
    this.listSupplier.forEach(item => {
      if (item.Code == this.model.Supplier_NCC_SX) {
        this.model.Address_NCC_SX = item.Address_NCC_SX
        // this.model.Supplier_NCC_SX = item.Supplier_NCC_SX
        this.model.BankPayment_NCC_SX = item.BankPayment_NCC_SX
        this.model.DeliveryTime = item.DeliveryTime
        this.model.Email_NCC_SX = item.Email_NCC_SX
        this.model.Name_NCC_SX = item.Name_NCC_SX
        this.model.PhoneNumber_NCC_SX = item.PhoneNumber_NCC_SX
        this.model.PIC_NCC_SX = item.PIC_NCC_SX
        this.model.TypePayment_NCC_SX = item.TypePayment_NCC_SX
        this.model.RulesDelivery = item.RulesDelivery_NCC_SX
        this.model.RulesPayment_NCC_SX = item.RulesPayment_NCC_SX
        this.model.Country_NCC_SX = item.Country_NCC_SX
        this.model.Website_NCC_SX = item.Website_NCC_SX
        this.model.Title_NCC_SX = item.Title_NCC_SX
      }
    })
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
      },
      error => {

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

  changePriceInPO_Price() {
    this.model.Amout_PO = parseFloat(this.model.PriceInPO_Price) * parseFloat(this.model.Ratio_PO);
    this.model.PriceProduct_TPA = this.model.Amout_PO;
    this.updatePrice_InsurrancePrice();
    this.updateProfit_TPA();
  }

  changePackingSize_RealVolume_PriceWord() {
    this.model.PriceVCInWeight_PriceWord = parseFloat(this.model.PackingSize_RealVolume_PriceWord) / 0.005;
    this.model.MassConvertedBySize_Air_PriceWord = parseFloat(this.model.PackingSize_RealVolume_PriceWord) / 0.006;
    this.updateVolumePricing_PriceWord();
  }

  updateVolumePricing_PriceWord() {
    if (parseFloat(this.model.Weight_PriceWord) < parseFloat(this.model.PriceVCInWeight_PriceWord)) {
      this.model.VolumePricing_PriceWord = this.model.PriceVCInWeight_PriceWord;
    }
    else {
      this.model.VolumePricing_PriceWord = this.model.Weight_PriceWord;
    };

    this.updateCPVCInWeight_AmoutVND_PriceWord();
  }

  updateCPVCInWeight_AmoutVND_PriceWord() {
    this.model.CPVCInWeight_AmoutVND_PriceWord = parseFloat(this.model.CPVCInWeight_Ratio_PriceWord) * parseFloat(this.model.CPVCInWeight_Price_PriceWord) * parseFloat(this.model.VolumePricing_PriceWord);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updateVolumePricing_Air_PriceWord() {
    if (parseFloat(this.model.Weight_Air_PriceWord) < parseFloat(this.model.MassConvertedBySize_Air_PriceWord)) {
      this.model.VolumePricing_Air_PriceWord = this.model.MassConvertedBySize_Air_PriceWord;
    }
    else {
      this.model.VolumePricing_Air_PriceWord = this.model.Weight_Air_PriceWord;
    };

    this.updateAmount_AirTransport_Air_PriceWord();
  }

  updateAmount_AirTransport_Air_PriceWord() {
    this.model.Amount_AirTransport_Air_PriceWord = parseFloat(this.model.Ratio_AirTransport_Air_PriceWord) * parseFloat(this.model.Price_AirTransport_Air_PriceWord) * parseFloat(this.model.VolumePricing_Air_PriceWord);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updatePackingSize_RealVolume_PriceWord() {
    this.model.PackingSize_RealVolume_PriceWord = ((parseFloat(this.model.PackingSize_D_PriceWord) + parseFloat(this.model.PackingSize_Tolerance_PriceWord)) *
      (parseFloat(this.model.PackingSize_R_PriceWord) + parseFloat(this.model.PackingSize_Tolerance_PriceWord)) *
      (parseFloat(this.model.PackingSize_C_PriceWord) + parseFloat(this.model.PackingSize_Tolerance_PriceWord))) / 1000000000;

    this.model.PackingSize_PiceVolume_PriceWord = this.model.PackingSize_RealVolume_PriceWord < 1 ? 1 : this.model.PackingSize_RealVolume_PriceWord;

    this.changePackingSize_RealVolume_PriceWord();
    this.updatePriceVCInCBM_PriceWord();
  }

  // 100000000
  updatePriceVCInCBM_PriceWord() {
    this.model.PriceVCInCBM_PriceWord = parseFloat(this.model.PackingSize_PiceVolume_PriceWord) * parseFloat(this.model.Price_PriceVC_LCLInBCM_PriceWord);

    this.updateAmout_PriceVC_LCLInBCM_PriceWord();
  }

  updateAmout_PriceVC_LCLInBCM_PriceWord() {
    this.model.Amout_PriceVC_LCLInBCM_PriceWord = parseFloat(this.model.Ratio_PriceVC_LCLInBCM_PriceWord) * parseFloat(this.model.PriceVCInCBM_PriceWord);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updateAmout_PriceVC_Cont20_PriceWord() {
    this.model.Amout_PriceVC_Cont20_PriceWord = parseFloat(this.model.Ratio_PriceVC_Cont20_PriceWord) * parseFloat(this.model.Price_PriceVC_Cont20_PriceWord);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updateAmount_PriceVC_Cont20OT_PiceWord() {
    this.model.Amount_PriceVC_Cont20OT_PiceWord = parseFloat(this.model.Ratio_PriceVC_Cont20OT_PiceWord) * parseFloat(this.model.Price_PriceVC_Cont20OT_PiceWord);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updateAmount_PriceVC_Cont40_PriceWord() {
    this.model.Amount_PriceVC_Cont40_PriceWord = parseFloat(this.model.Ratio_PriceVC_Cont40_PriceWord) * parseFloat(this.model.Price_PriceVC_Cont40_PriceWord);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updateAmount_PriceVC_Cont40OT_PriceWord() {
    this.model.Amount_PriceVC_Cont40OT_PriceWord = parseFloat(this.model.Ratio_PriceVC_Cont40OT_PriceWord) * parseFloat(this.model.Price_PriceVC_Cont40OT_PriceWord);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updateAmount_PriceLCExport() {
    this.model.Amount_PriceLCExport = parseFloat(this.model.Ratio_PriceLCExport) * parseFloat(this.model.Price_PriceLCExport);

    this.updatePrice_InsurrancePrice();
    this.updatePriceVC_TPA();
  }

  updatePrice_InsurrancePrice() {
    // this.model.Price_InsurrancePrice = parseFloat(this.model.InsurranceType) *
    //   (parseFloat(this.model.Amount_PriceLCExport) + parseFloat(this.model.Amount_PriceVC_Cont40OT_PriceWord) +
    //     parseFloat(this.model.Amount_PriceVC_Cont40_PriceWord) + parseFloat(this.model.Amount_PriceVC_Cont20OT_PiceWord) +
    //     parseFloat(this.model.Amout_PriceVC_Cont20_PriceWord) + parseFloat(this.model.Amout_PriceVC_LCLInBCM_PriceWord) +
    //     parseFloat(this.model.Amount_AirTransport_Air_PriceWord) + parseFloat(this.model.CPVCInWeight_AmoutVND_PriceWord) +
    //     parseFloat(this.model.Amout_PO));

    this.model.Price_InsurrancePrice = (parseFloat(this.model.PriceInSP_Price) * parseFloat(this.model.Ratio_InsurrancePrice) * parseFloat(this.model.InsurranceType))/ 100;

    this.updateImportTaxPrice();
    this.updateImportTaxPriceOther();

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_LSS() {
    this.model.Amount_PriceLCImport_LSS = parseFloat(this.model.Ratio_PriceLCImport_LSS) * parseFloat(this.model.Price_PriceLCImport_LSS);
    this.updateImportTaxPrice();
    this.updateImportTaxPriceOther();

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_THC() {
    this.model.Amount_PriceLCImport_THC = parseFloat(this.model.Ratio_PriceLCImport_THC) * parseFloat(this.model.Price_PriceLCImport_THC);

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_DO() {
    this.model.Amount_PriceLCImport_DO = parseFloat(this.model.Ratio_PriceLCImport_DO) * parseFloat(this.model.Price_PriceLCImport_DO);

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_CIC() {
    this.model.Amount_PriceLCImport_CIC = parseFloat(this.model.Ratio_PriceLCImport_CIC) * parseFloat(this.model.Price_PriceLCImport_CIC);

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_HL() {
    this.model.Amount_PriceLCImport_HL = parseFloat(this.model.Ratio_PriceLCImport_HL) * parseFloat(this.model.Price_PriceLCImport_HL);
  }
  updateAmount_PriceLCImport_CLF() {
    this.model.Amount_PriceLCImport_CLF = parseFloat(this.model.Ratio_PriceLCImport_CLF) * parseFloat(this.model.Price_PriceLCImport_CLF);

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_CFS() {
    this.model.Amount_PriceLCImport_CFS = parseFloat(this.model.Ratio_PriceLCImport_CFS) * parseFloat(this.model.Price_PriceLCImport_CFS);

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_Lift() {
    this.model.Amount_PriceLCImport_Lift = parseFloat(this.model.Ratio_PriceLCImport_Lift) * parseFloat(this.model.Price_PriceLCImport_Lift);

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_IF() {
    this.model.Amount_PriceLCImport_IF = parseFloat(this.model.Ratio_PriceLCImport_IF) * parseFloat(this.model.Price_PriceLCImport_IF);

    this.updateImportTaxPrice_TPA();
  }

  updateAmount_PriceLCImport_Other() {
    this.model.Amount_PriceLCImport_Other = parseFloat(this.model.Ratio_PriceLCImport_Other) * parseFloat(this.model.Price_PriceLCImport_Other);

    this.updateImportTaxPrice_TPA();
  }

  updateImportTaxPrice() {
    this.model.ImportTaxPrice = (parseFloat(this.model.ImportTax) *
      (parseFloat(this.model.Amount_PriceLCExport) + parseFloat(this.model.Amount_PriceVC_Cont40OT_PriceWord) +
        parseFloat(this.model.Amount_PriceVC_Cont40_PriceWord) + parseFloat(this.model.Amount_PriceVC_Cont20OT_PiceWord) +
        parseFloat(this.model.Amout_PriceVC_Cont20_PriceWord) + parseFloat(this.model.Amout_PriceVC_LCLInBCM_PriceWord) +
        parseFloat(this.model.Amount_AirTransport_Air_PriceWord) + parseFloat(this.model.CPVCInWeight_AmoutVND_PriceWord) +
        parseFloat(this.model.Amout_PO) + parseFloat(this.model.Price_InsurrancePrice) + parseFloat(this.model.Amount_PriceLCImport_LSS)))/100;

    this.updateImportTax_TPA();
  }

  updateImportTaxPriceOther() {
    this.model.ImportTaxPriceOther = (parseFloat(this.model.TaxOther) *
      (parseFloat(this.model.Amount_PriceLCExport) + parseFloat(this.model.Amount_PriceVC_Cont40OT_PriceWord) +
        parseFloat(this.model.Amount_PriceVC_Cont40_PriceWord) + parseFloat(this.model.Amount_PriceVC_Cont20OT_PiceWord) +
        parseFloat(this.model.Amout_PriceVC_Cont20_PriceWord) + parseFloat(this.model.Amout_PriceVC_LCLInBCM_PriceWord) +
        parseFloat(this.model.Amount_AirTransport_Air_PriceWord) + parseFloat(this.model.CPVCInWeight_AmoutVND_PriceWord) +
        parseFloat(this.model.Amout_PO) + parseFloat(this.model.Price_InsurrancePrice) + parseFloat(this.model.Amount_PriceLCImport_LSS)))/100;

    this.updateImportTax_TPA();
  }

  updatePriceVC_TPA() {
    this.model.PriceVC_TPA = parseFloat(this.model.Amount_PriceLCExport) + parseFloat(this.model.Amount_PriceVC_Cont40OT_PriceWord) +
      parseFloat(this.model.Amount_PriceVC_Cont40_PriceWord) + parseFloat(this.model.Amount_PriceVC_Cont20OT_PiceWord) +
      parseFloat(this.model.Amout_PriceVC_Cont20_PriceWord) + parseFloat(this.model.Amout_PriceVC_LCLInBCM_PriceWord) +
      parseFloat(this.model.Amount_AirTransport_Air_PriceWord) + parseFloat(this.model.CPVCInWeight_AmoutVND_PriceWord);
    this.updateProfit_TPA();
  }

  updateImportTaxPrice_TPA() {
    this.model.ImportTaxPrice_TPA = parseFloat(this.model.Surcharge) + parseFloat(this.model.PriceFW) +
      parseFloat(this.model.PriceOther) + parseFloat(this.model.Amount_PriceLCImport_Other) +
      parseFloat(this.model.Amount_PriceLCImport_IF) + parseFloat(this.model.Amount_PriceLCImport_Lift) +
      parseFloat(this.model.Amount_PriceLCImport_CFS) + parseFloat(this.model.Amount_PriceLCImport_CLF) +
      parseFloat(this.model.Amount_PriceLCImport_HL) + parseFloat(this.model.Amount_PriceLCImport_CIC) +
      parseFloat(this.model.Amount_PriceLCImport_DO) + parseFloat(this.model.Amount_PriceLCImport_THC) +
      parseFloat(this.model.Amount_PriceLCImport_LSS) + parseFloat(this.model.Price_InsurrancePrice);
    this.updateProfit_TPA();
  }

  updateImportTax_TPA() {
    this.model.ImportTax_TPA = parseFloat(this.model.ImportTaxPriceOther) + parseFloat(this.model.ImportTaxPrice);
    this.updateProfit_TPA();
  }

  updateTotalPrice() {
    this.model.TotalPrice = parseFloat(this.model.ImportTax_TPA) + parseFloat(this.model.Interest_TPA) +
      parseFloat(this.model.VAT_TPA) + parseFloat(this.model.ImportTaxPrice_TPA) + parseFloat(this.model.PriceVC_TPA) +
      parseFloat(this.model.PriceProduct_TPA);
  }

  isConfirm: boolean = false;
  syncSaleProductStandardTPA(isConfirm: boolean) {
    let list = [];
    list.push(this.model.Id);
    this.service.syncSaleProductStandardTPA(false, isConfirm, list).subscribe(
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
        this.syncSaleProductStandardTPA(true);
      }
    );
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

  updateProfit_TPA() {
    this.model.Profit_TPA = ((parseFloat(this.model.PriceEXW_TPA) - (parseFloat(this.model.PriceProduct_TPA) + parseFloat(this.model.PriceVC_TPA) + parseFloat(this.model.ImportTaxPrice_TPA) + parseFloat(this.model.ImportTax_TPA))) / parseFloat(this.model.PriceEXW_TPA)) * 100;
  }
}
