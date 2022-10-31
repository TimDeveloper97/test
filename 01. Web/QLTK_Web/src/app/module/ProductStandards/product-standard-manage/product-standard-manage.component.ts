import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

import { DxTreeListComponent } from 'devextreme-angular';
import { AppSetting, Configuration, MessageService, Constants, ComboboxService } from 'src/app/shared';

import { ProductStandardGroupService } from '../../services/product-standard-group.service';
import { ProductStandardsCreateComponent } from '../product-standards-create/product-standards-create.component';
import { ProductStandardsService } from '../../services/product-standards.service';
import { ProductStandardsUpdateComponent } from '../product-standards-update/product-standards-update.component';
import { ProductStandardGroupCreateComponent } from '../../ProductStandardGroup/product-standard-group-create/product-standard-group-create.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-standard-manage',
  templateUrl: './product-standard-manage.component.html',
  styleUrls: ['./product-standard-manage.component.scss']
})
export class ProductStandardManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    private productStandarservice: ProductStandardsService,
    private combobox: ComboboxService,
    private serverproductstandgroup: ProductStandardGroupService,
    private router: Router,
  ) {
    this.items = [
      { Id: 1, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }

  startIndex = 0;
  height = 0;
  items: any;
  listDA: any[] = [];
  listProductStandardGroup = [];
  listDepartment: any[] = [];
  listSBU = [];
  listProductStandGroup: any[] = [];
  listProductStandGroupId = [];
  selectedProductStandGroupId = '';
  productStandardGroupId: '';
  checkSearch: boolean = false;
  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    ProductStandardGroupId: '',
    DepartmentId: '',
    SBUId: '',
    Code: '',
    Name: '',
    CreateByName: '',
    DataType: null
  }

  productStandGroupModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
  }

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

  fileModel = {
    Id: '',
    ProductStandardId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo Mã/tên tiêu chuẩn',
    Items: [
      {
        Name: 'Người tạo',
        FieldName: 'CreateByName',
        Placeholder: 'Nhập người tạo',
        Type: 'text'
      },
      {
        Name: 'Loại',
        FieldName: 'DataType',
        Placeholder: 'Loại tiêu chuản',
        Type: 'select',
        DisplayName: 'Name',
        Data: this.constant.ListWorkType,
        ValueName: 'Id'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 3,
      },
      {
        Name: 'Bộ phận sử dụng',
        FieldName: 'DepartmentId',
        Placeholder: 'Bộ phận sử dụng',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 2,
      },
    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 135;
    if (window.innerHeight <= 768 && window.innerWidth <= 1024) {
      this.checkSearch = true;
    } else { this.checkSearch = false; }

    this.appSetting.PageTitle = "Tiêu chuẩn sản phẩm";
    this.searchProductStandGroup();
    this.getCbbSBU();
    this.searchProductStandard("");
    this.selectedProductStandGroupId = localStorage.getItem("selectedProductStandGroupId");
    localStorage.removeItem("selectedProductStandGroupId");
  }

  
  ShowCreate() {
    this.router.navigate(['module/quan-ly-tieu-chuan-san-pham/them-moi-tieu-chuan-san-pham']);
  }

  ShowUpdate(Id: string) {
    this.router.navigate(['module/quan-ly-tieu-chuan-san-pham/chinh-sua-tieu-chuan-san-pham/', Id]);
  }

  itemClick(e) {
    if (this.productStandardGroupId == '' || this.productStandardGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm tiêu chuẩn!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdate(this.productStandardGroupId);
      }
      else if (e.itemData.Id == 2) {
        this.showConfirmDeleteProductStandardGroup(this.productStandardGroupId);
      }
    }
  }

  onSelectionChanged(e) {
    // this.selectedProductStandGroupId = e.selectedRowKeys[0];
    // this.searchProductStandard(e.selectedRowKeys[0]);
    // this.productStandardGroupId = e.selectedRowKeys[0];

    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedProductStandGroupId) {
      this.selectedProductStandGroupId = e.selectedRowKeys[0];
      this.searchProductStandard(e.selectedRowKeys[0]);
      this.productStandardGroupId = e.selectedRowKeys[0];
    }
  }

  searchProductStandGroup() {
    this.serverproductstandgroup.searchProductStandardGroup(this.productStandGroupModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listProductStandGroup = data.ListResult;
        this.listProductStandGroup.unshift(this.modelAll);
        this.productStandGroupModel.TotalItems = data.TotalItem;
        if (this.selectedProductStandGroupId == null) {
          this.selectedProductStandGroupId = this.listProductStandGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedProductStandGroupId];
        for (var item of this.listProductStandGroup) {
          this.listProductStandGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchProductStandard(ProductStandardGroupId: string) {
    this.model.ProductStandardGroupId = ProductStandardGroupId;
    this.productStandarservice.searchProductStandard(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCbProductStandardGroup() {
    this.combobox.getCbbProductStandardGroup().subscribe(
      data => {
        this.listProductStandardGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchSBU() {
    this.getCbbDepartment();
    this.searchProductStandard(this.productStandardGroupId);
  }

  getCbbSBU() {
    this.combobox.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }


  getCbbDepartment() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear(ProductStandardGroupId: string) {
    this.model = {
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      ProductStandardGroupId: '',
      DepartmentId: '',
      SBUId: '',
      Code: '',
      Name: '',
      CreateBy: '',
    }
    this.searchProductStandard(ProductStandardGroupId);
  }

  showConfirmDeleteProductStandardGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm tiêu chuẩn này không?").then(
      data => {
        this.deleteProductStandardGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteProductStandardGroup(Id: string) {
    this.serverproductstandgroup.deleteProductStandardGroup({ Id: Id }).subscribe(
      data => {
        this.searchProductStandGroup();
        this.searchProductStandard("");
        this.messageService.showSuccess('Xóa nhóm tiêu chuẩn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteProductStandard(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tiêu chuẩn này không?").then(
      data => {
        this.deleteProductStandard(Id);
      },
      error => {
        
      }
    );
  }

  deleteProductStandard(Id: string) {
    this.productStandarservice.deleteProductStandard({ Id: Id }).subscribe(
      data => {
        this.searchProductStandard(this.productStandardGroupId);
        this.messageService.showSuccess('Xóa tiêu chuẩn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  // showCreate(Id: string) {
  //   let activeModal = this.modalService.open(ProductStandardsCreateComponent, { container: 'body', windowClass: 'productstandardcreate-model', backdrop: 'static' })
  //   activeModal.componentInstance.productStandardGroupId = this.productStandardGroupId;
  //   activeModal.result.then((result) => {
  //     if (result) {
  //       this.searchProductStandard(this.productStandardGroupId);
  //     }
  //   }, (reason) => {
  //   });
  // }

  showUpdate(Id: string) {
    let activeModal = this.modalService.open(ProductStandardsUpdateComponent, { container: 'body', windowClass: 'productstandardupdate-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProductStandard(this.productStandardGroupId);
      }
    }, (reason) => {
    });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ProductStandardGroupCreateComponent, { container: 'body', windowClass: 'product-standard-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProductStandGroup();
        this.searchProductStandard(Id);
      }
    }, (reason) => {
    });
  }

  showAll() {
    this.productStandardGroupId = null;
    this.searchProductStandard(this.productStandardGroupId);
  }

  exportExcel() {
    this.productStandarservice.exportExcel(this.model).subscribe(d => {
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
}
