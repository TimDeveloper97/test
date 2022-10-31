import { Component, OnInit, ViewChild, ElementRef, OnDestroy, AfterViewInit } from '@angular/core';
import { ProductStandardTpaService } from '../../services/product-standard-tpa.service';
import { MessageService, Configuration, AppSetting, Constants, ComponentService, PermissionService, ComboboxService } from 'src/app/shared';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { ProductStandardTpaTypeService } from '../../services/product-standard-tpa-type.service';
import { ProductStandardTPATypeCreateComponent } from '../product-standard-tpa-type-create/product-standard-tpa-type-create.component';

@Component({
  selector: 'app-product-standard-tpa-manage',
  templateUrl: './product-standard-tpa-manage.component.html',
  styleUrls: ['./product-standard-tpa-manage.component.scss']
})
export class ProductStandardTpaManageComponent implements OnInit, OnDestroy, AfterViewInit {

  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public appSetting: AppSetting,
    public constant: Constants,
    private service: ProductStandardTpaService,
    private componentService: ComponentService,
    private permissionService: PermissionService,
    public comboboxService: ComboboxService,
    public productStandardTpaTypeService: ProductStandardTpaTypeService
  ) {
    this.pagination = Object.assign({}, appSetting.Pagination);
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }

  fileTemplate = this.config.ServerApi + 'Template/Template_ThietBiTieuChuan.xlsx';
  fileTemplateSaleProduct = this.config.ServerApi + 'Template/Import_SyncSaleProduct_TPA_Template.xls';
  startIndex = 0;
  pagination: any;
  listData: any[] = [];
  listProductStandardTPAType = [];
  logUserId: string;
  items: any;
  productStandardTPATypeId: string = '';
  selectedProductStandardTPATypeId: string;


  model: any = {
    PageSize: 10,
    TotalItems: 0,
    Date: null,
    PageNumber: 1,
    OrderBy: 'Model',
    OrderType: true,

    Id: '',
    Code: '',
    Name: '',
    Manufacture: '',
    Supplier: '',
    IsSendSale: null,
    IsCOCQ: null,
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã',
    Items: [
      {
        Name: 'Tên tiếng việt/tiếng anh',
        FieldName: 'Name',
        Placeholder: 'Nhập tên thiết bị nhập khẩu',
        Type: 'text'
      },
      {
        Name: 'Hãng sản xuất',
        FieldName: 'Manufacture',
        Placeholder: 'Nhập tên hãng',
        Type: 'text'
      },
      {
        Name: 'Nhà cung cấp',
        FieldName: 'Supplier',
        Placeholder: 'Nhập tên nhà cung cấp',
        Type: 'text'
      },
      {
        Name: 'Chuyển thư viện',
        FieldName: 'IsSendSale',
        Placeholder: 'Chọn tình trạng chuyển thư viện',
        Type: 'select',
        Data: this.constant.SendSale,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tài liệu hồ sơ chứng từ',
        FieldName: 'IsCOCQ',
        //Placeholder: 'Chọn tình trạng chuyển thư viện',
        Type: 'select',
        Data: this.constant.FileCOCQ,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  height = 400;
  heightLeft = 0;
  selectIndex = -1;
  minWidth = 16500;
  isKD = false;
  expandGroupKeys: any[] = [];
  selectGroupKeys: any[] = [];

  modelAll: any = {
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  @ViewChild('scrollProducStandard', { static: false }) scrollProducStandard: ElementRef;
  @ViewChild('scrollProducStandardHeader', { static: false }) scrollProducStandardHeader: ElementRef;

  ngOnInit() {
    this.heightLeft = window.innerHeight - 140;
    this.height = window.innerHeight - 330;
    if (!this.permissionService.checkPermission(['F110805']) && this.permissionService.checkPermission(['F110807']) && this.permissionService.checkPermission(['F110806'])) {
      this.minWidth = 15740;
    }
    else if (!this.permissionService.checkPermission(['F110807']) || (!this.permissionService.checkPermission(['F110805']) && !this.permissionService.checkPermission(['F110806']))) {
      this.minWidth = 16500;
    }
    else if (!this.permissionService.checkPermission(['F110806']) && this.permissionService.checkPermission(['F110805']) && this.permissionService.checkPermission(['F110807'])) {
      this.minWidth = 2000;
      this.isKD = true;
    }
    else {
      this.minWidth = 900;
    }

    this.appSetting.PageTitle = "Quản lý thiết bị nhập khẩu";
    this.searchProductStandardTPA();
    this.searchProductStandardTPAType();
  }

  ngAfterViewInit() {
    this.scrollProducStandard.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollProducStandardHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollProducStandard.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      Date: null,
      PageNumber: 1,
      OrderBy: 'Model',
      OrderType: true,
      Id: '',
      Code: '',
      Name: '',
      Manufacture: '',
      Supplier: '',
      IsSendSale: null,
      ProductStandardTPATypeId: ''
    }
    this.searchProductStandardTPA();

  }

  productStandardTPA: number;

  searchProductStandardTPAType() {
    this.comboboxService.getCBBProductStandardTPAType().subscribe((data: any) => {
      if (data) {
        this.listProductStandardTPAType = data;
        this.productStandardTPA = this.listProductStandardTPAType.length;
        this.listProductStandardTPAType.unshift(this.modelAll);
        // this.setSelectGroup();
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  setSelectGroup() {
    if (!this.selectedProductStandardTPATypeId) {
      this.selectedProductStandardTPATypeId = '';
    }
    else {
      var parentId = '';
      this.expandGroupKeys = [];
      this.listProductStandardTPAType.forEach(element => {
        if (element.Id == this.selectedProductStandardTPATypeId) {
          parentId = element.ParentId;
        }
      });

      this.setExpandKey(parentId);
    }

    this.selectGroupKeys = [this.selectedProductStandardTPATypeId];
  }

  setExpandKey(parentId) {
    var group;
    this.listProductStandardTPAType.forEach(element => {
      if (element.Id == parentId) {
        group = element;
      }
    });

    if (group) {
      this.expandGroupKeys.push(group.Id);
      this.setExpandKey(group.ParentId);
    }
  }

  searchProductStandardTPA() {
    this.service.searchProductStandardTPA(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        if (this.checkeds) {
          this.listData.forEach(element => {
            element.Checked = true;
          });
        }
        this.model.TotalItems = data.TotalItem;
        this.model.Date = data.Date;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị nhập khẩu này không?").then(
      data => {
        this.deleteProductStandardTPA(Id);
      },
      error => {

      }
    );
  }

  deleteProductStandardTPA(Id: string) {
    this.service.deleteProductStandardTPA({ Id: Id }).subscribe(
      data => {
        this.searchProductStandardTPA();
        this.messageService.showSuccess('Xóa thiết bị nhập khẩu thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    if (Id == '') {
      localStorage.setItem("productStandardTPATypeId", this.productStandardTPATypeId);
      this.router.navigate(['thiet-bi/quan-ly-thiet-bi-nhap-khau/them-moi']);
    }
    else {
      this.router.navigate(['thiet-bi/quan-ly-thiet-bi-nhap-khau/chinh-sua/', Id]);
    }
  }

  importProductStandardTPA() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.service.importProductStandardTPA(data).subscribe(
          data => {
            this.searchProductStandardTPA();
            this.messageService.showSuccess('Import thiết bị nhập khẩu thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  selectProduct(index) {
    this.selectIndex == index;
  }

  exportExcel() {
    this.service.exportExcel(this.model).subscribe(d => {
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

  exportExcelAccountant() {
    this.service.exportExcelAccountant(this.model).subscribe(d => {
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

  exportExcelBusiness() {
    this.service.exportExcelBusiness(this.model).subscribe(d => {
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

  showConfirmType(id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chủng loại này không?").then(
      data => {
        this.deleteType(id);
      },
      error => {

      }
    );
  }

  deleteType(id) {
    this.productStandardTpaTypeService.deleteType({ Id: id }).subscribe(
      data => {
        this.searchProductStandardTPAType();
        this.messageService.showSuccess('Xóa chủng loại thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdateType(Id: string, isUpdate: Boolean) {
    let activeModal = this.modalService.open(ProductStandardTPATypeCreateComponent, { container: 'body', windowClass: 'product-standard-tpa-type-create-create-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.id = Id;
    } else {
      activeModal.componentInstance.parentId = Id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchProductStandardTPAType();
      }
    }, (reason) => {
    });
  }

  itemClick(e) {
    if (this.productStandardTPATypeId == '' || this.productStandardTPATypeId == null) {
      this.messageService.showMessage("Đây không phải chủng loại!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateType(this.productStandardTPATypeId, false);
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdateType(this.productStandardTPATypeId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmType(this.productStandardTPATypeId);
      }
    }
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null) {
      this.productStandardTPATypeId = e.selectedRowKeys[0];
      this.model.ProductStandardTPATypeId = this.productStandardTPATypeId
      this.searchProductStandardTPA();
      // this.moduleGroupId = e.selectedRowKeys[0];
      // localStorage.setItem("selectedModuleGroupId", this.selectedModelGroupId);
    }
  }

  fileImport: any;
  showImportSyncSaleProductStandardTPA() {
    this.componentService.showImportExcel(this.fileTemplateSaleProduct, false).subscribe(data => {
      if (data) {
        this.fileImport = data;
        this.service.importSyncSaleProductStandardTPA(data, this.isConfirm).subscribe(
          data => {
            if (data) {
              this.importConfirm(data);
            } else {
              this.searchProductStandardTPA();
              this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
            }
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  importConfirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.service.importSyncSaleProductStandardTPA(this.fileImport, true,).subscribe(
          data => {
            this.searchProductStandardTPA();
            this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    );
  }

  checkeds: boolean = false;
  listCheck: any[] = [];
  selectAllFunction() {
    this.listData.forEach(element => {
      element.Checked = this.checkeds;
    });
  }

  pushChecker(row: any) {
    if (row.Checked) {
      this.listCheck.push(row);
    } else {
      this.checkeds = false;
      this.listCheck.splice(this.listCheck.indexOf(row), 1);
    }
  }

  isConfirm: boolean = false;
  syncSaleProductStandardTPA(isConfirm: boolean) {
    let list = [];
    this.listCheck.forEach(element => {
      list.push(element.Id);
    });
    this.service.syncSaleProductStandardTPA(this.checkeds, isConfirm, list).subscribe(
      data => {
        if (data) {
          this.confirm(data);
        } else {
          this.searchProductStandardTPA();
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
}
