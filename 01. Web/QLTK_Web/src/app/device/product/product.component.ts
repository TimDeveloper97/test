import { Component, OnInit, ViewChild } from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { DxTreeListComponent } from 'devextreme-angular';
import { Configuration, MessageService, AppSetting, Constants, ComponentService } from 'src/app/shared';

import { ProductGroupService } from '../services/product-group.service';
import { ProductService } from '../services/product.service';
import { ProductGroupCreateComponent } from '../productgroup/product-group-create/product-group-create.component';
import { ProductNeedPublicationsComponent } from '../product-need-publications/product-need-publications.component';


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(

    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public appSetting: AppSetting,
    public constant: Constants,
    private productGroup: ProductGroupService,
    private productService: ProductService,
    private componentService: ComponentService
  ) {
    this.pagination = Object.assign({}, appSetting.Pagination);
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fas fa-plus' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }
  fileTemplate = this.config.ServerApi + 'Template/Import_SyncSaleProduct_Template.xls';
  items: any;
  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  StartIndex = 0;
  pagination;
  LstpageSize = [5, 10, 15, 20, 25, 30];
  listProductGroup: any[] = [];
  listProduct: any[] = [];
  modelProductGroup: any = {
    Id: '',
    Code: '',
    Name: '',
    ParentId: '',
    TotalItems: 0,
  }

  modelProduct: any = {
    PageSize: 10,
    TotalItems: 0,
    Status1: 0,
    Status2: 0,
    Date: null,
    TotalItemExten: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    ProductGroupId: '',
    Code: '',
    Name: '',
    CurentVersion: '',
    ProcedureTime: '',
    Status: null,
    IsManualExist: '',
    IsQuoteExist: '',
    IsPracticeExist: '',
    IsLayoutExist: '',
    IsMaterialExist: '',
    Pricing: '',
    IsEnought: '',
    SBUId: '',
    DepartmentId: '',
    IsSendSale: null
  }

  productGroupId: '';
  selectedProductGroupId = '';

  modelAll: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }
  height = 0;

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên thiết bị',
    Items: [
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.ProductStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng dữ liệu',
        FieldName: 'IsEnought',
        Placeholder: 'Tình trạng dữ liệu',
        Type: 'select',
        Data: this.constant.ProductIsEnought,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 3,
        Permission: ['F030405'],
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 2,
        Permission: ['F030405'],
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
        Name: 'Catalogs',
        FieldName: 'TypeCatalogs',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Hướng dẫn thực hành',
        FieldName: 'TypeGuidePractice',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Hướng dẫn sử dụng',
        FieldName: 'TypeDMBTH',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Hướng dẫn bảo trì',
        FieldName: 'TypeGuideMaintenance',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
    this.modelProduct.SBUId = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
    this.modelProduct.DepartmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;
    this.height = window.innerHeight - 200;
    this.appSetting.PageTitle = "Quản lý thiết bị";
    this.searchProductGroup();
    this.searchProduct(this.productGroupId);
    this.selectedProductGroupId = localStorage.getItem("selectedProductGroupId");

    localStorage.removeItem("selectedProductGroupId");
  }

  listProductGroupId = [];
  searchProductGroup() {
    this.productGroup.searchProductGroup(this.modelProductGroup).subscribe((data: any) => {
      if (data.ListResult) {
        this.listProductGroup = data.ListResult;
        this.listProductGroup.unshift(this.modelAll);
        this.modelProductGroup.TotalItems = data.TotalItem;
        if (this.selectedProductGroupId == null) {
          this.selectedProductGroupId = this.listProductGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedProductGroupId];
        for (var item of this.listProductGroup) {
          this.listProductGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.modelProduct = {
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      Status1: 0,
      Status2: 0,
      Date: null,
      TotalItemExten: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      ProductGroupId: '',
      Code: '',
      Name: '',
      CurentVersion: '',
      ProcedureTime: '',
      Status: null,
      IsManualExist: '',
      IsQuoteExist: '',
      IsPracticeExist: '',
      IsLayoutExist: '',
      IsMaterialExist: '',
      Pricing: '',
      IsEnought: '',
      IsSendSale: null
    }
    this.productGroupId = '';
    this.selectedProductGroupId = '';
    this.searchProductGroup();
    this.searchProduct(this.productGroupId);

  }

  searchProduct(productGroupId: string) {
    this.modelProduct.ProductGroupId = productGroupId;
    this.productService.searchProduct(this.modelProduct).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.modelProduct.PageNumber - 1) * this.modelProduct.PageSize + 1);
        this.listProduct = data.ListResult;
        if (this.checkeds) {
          this.listProduct.forEach(element => {
            element.Checked = true;
          });
        }
        this.modelProduct.TotalItems = data.TotalItem;
        this.modelProduct.Status1 = data.Status1;
        this.modelProduct.Status2 = data.Status2;
        this.modelProduct.Date = data.Date;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    // this.selectedProductGroupId = e.selectedRowKeys[0];
    // this.searchProduct(e.selectedRowKeys[0]);
    // this.productGroupId = e.selectedRowKeys[0];

    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedProductGroupId) {
      this.selectedProductGroupId = e.selectedRowKeys[0];
      this.searchProduct(e.selectedRowKeys[0]);
      this.productGroupId = e.selectedRowKeys[0];
    }
  }

  GroupId: string;
  groupCode: string;
  showCreate(Id: string) {
    localStorage.setItem("selectedProductGroupId", this.selectedProductGroupId);
    if (Id == '') {
      this.GroupId = this.selectedProductGroupId;
      this.router.navigate(['thiet-bi/quan-ly-thiet-bi/them-moi/', this.GroupId]);
    }
    else {
      this.router.navigate(['thiet-bi/quan-ly-thiet-bi/chinh-sua/', Id]);
    }


  }

  showClick(Id: string, a: Number) {
    let activeModal = this.modalService.open(ProductNeedPublicationsComponent, { container: 'body', windowClass: 'product-need-publications-model', backdrop: 'static' })
    activeModal.componentInstance.IdProduct = Id;
    activeModal.componentInstance.publications = a;
    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị này không?").then(
      data => {
        this.deleteProduct(Id);
      },
      error => {

      }
    );
  }

  deleteProduct(Id: string) {
    this.productService.deleteProduct({ Id: Id }).subscribe(
      data => {
        this.searchProduct(this.productGroupId);
        this.messageService.showSuccess('Xóa thiết bị thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteProductGroups(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm thiết bị này không?").then(
      data => {
        this.deleteProductGroup(Id);
      },
      error => {

      }
    );
  }

  deleteProductGroup(Id: string) {
    this.productService.deleteProductGroup({ Id: Id }).subscribe(
      data => {
        this.searchProductGroup();
        this.messageService.showSuccess('Xóa nhóm thiết bị thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  itemClick(e) {
    if (this.productGroupId == '' || this.productGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm thiết bị!")
    } else {
      if (e.itemData.Id == 1) {
        this.ShowCreateUpdateProductGroup(this.productGroupId, false)
      }
      else if (e.itemData.Id == 2) {
        this.ShowCreateUpdateProductGroup(this.productGroupId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteProductGroups(this.productGroupId);
      }
    }
  }

  modelStatus: any = {
    ProductId: '',
    Status: 0
  }
  Config(Id) {

    this.modelStatus.ProductId = Id;
    this.modelStatus.Status = 0;
    this.productService.checkStatusProduct(this.modelStatus).subscribe(
      data => {
        this.messageService.showSuccess('Chưa xác nhận!');
        // this.modelStatus.Status = 1;
        this.searchProduct(this.productGroupId);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  NotConfig(Id) {

    this.modelStatus.ProductId = Id;
    this.modelStatus.Status = 1;
    this.productService.checkStatusProduct(this.modelStatus).subscribe(
      data => {
        this.messageService.showSuccess('Đã xác xác nhận!');
        // this.modelStatus.Status = 0;
        this.searchProduct(this.productGroupId);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  exportExcel() {
    this.productService.exportExcel(this.modelProduct).subscribe(d => {
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

  fileImport: any;
  showImportSyncSaleProduct() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.fileImport = data;
        this.productService.importSyncSaleProduct(data, this.isConfirm).subscribe(
          data => {
            if (data) {
              this.importConfirm(data);
            } else {
              this.searchProduct(this.productGroupId);
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
        this.productService.importSyncSaleProduct(this.fileImport, true,).subscribe(
          data => {
            this.searchProduct(this.productGroupId);
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
    this.listProduct.forEach(element => {
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
  syncSaleProduct(isConfirm: boolean) {
    let list = [];
    this.listCheck.forEach(element => {
      list.push(element.Id);
    });
    this.productService.syncSaleProduct(this.checkeds, isConfirm, list).subscribe(
      data => {
        if (data) {
          this.confirm(data);
        } else {
          this.searchProduct(this.productGroupId);
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

  updateNewPrice() {
    this.productService.updateNewPrice().subscribe(data => {
      this.messageService.showSuccess('Cập nhật giá mới thành công!');
    });
  }

  ShowCreateUpdateProductGroup(Id: string, isUpdate: boolean) {
    let activeModal = this.modalService.open(ProductGroupCreateComponent, { container: 'body', windowClass: 'productgroupcreate-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.idUpdate = Id;
    } else {
      activeModal.componentInstance.parentId = Id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchProductGroup();
      }
    }, (reason) => {
    });
  }
}