import { Component, OnInit, ViewChild } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { DxTreeListComponent } from 'devextreme-angular';

import { AppSetting, Configuration, MessageService, Constants, ComponentService } from 'src/app/shared';
import { SupplierService } from '../../services/supplier-service';
import { SupplierCreateComponent } from '../supplier-create/supplier-create.component';
import { ImportSupplierComponent } from '../import-supplier/import-supplier.component';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { MaterialgroupCreateComponent } from '../../materialgroup/materialgroup-create/materialgroup-create.component';

@Component({
  selector: 'app-supplier-manage',
  templateUrl: './supplier-manage.component.html',
  styleUrls: ['./supplier-manage.component.scss']
})
export class SupplierManageComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private supplierService: SupplierService,
    private materialGroupService: MaterialGroupService,
    public constant: Constants,
    private componentService: ComponentService
  ) {
    this.pagination = Object.assign({}, appSetting.Pagination);
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fas fa-plus' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }

  startIndex = 0;
  items: any;
  total: number;
  pagination: any;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  logUserId: string;
  listSupplierGroup: any[] = [];
  listSupplierGroupId = [];
  selectedSupplierGroupId = '';
  supplierGroupId: '';
  fileTemplate = '';

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  height = 0;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Email: '',
    PhoneNumber: '',
    Note: '',

    ListSupplierContact: []
  }

  supplierGroupModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: ''
  }

  modelAll: any = {
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã nhà cung cấp',
    Items: [
      {
        Name: 'Tên nhà cung cấp',
        FieldName: 'Name',
        Placeholder: 'Nhập tên nhà cung cấp',
        Type: 'text'
      },
      {
        Name: 'Email nhà cung cấp',
        FieldName: 'Email',
        Placeholder: 'Email',
        Type: 'text'
      },
      {
        Name: 'Mã hãng sản xuất',
        FieldName: 'ManufactureId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manuafacture,
        Columns: [{ Name: 'Code', Title: 'Mã hãng sản xuất' }, { Name: 'Name', Title: 'Tên hãng sản xuất' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn mã hãng sản xuất'
      },

    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 135;
    this.appSetting.PageTitle = "Nhà cung cấp";
    this.searchSupplierGroup();
    this.searchSupplier(this.supplierGroupId);
    this.selectedSupplierGroupId = localStorage.getItem("selectedSupplierGroupId");
    localStorage.removeItem("selectedSupplierGroupId");
  }

  itemClick(e) {
    if (this.supplierGroupId == '' || this.supplierGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm vật tư!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateGroup(this.supplierGroupId, 2)
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdateGroup(this.supplierGroupId, 1);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteSupplierGroup(this.supplierGroupId);
      }
    }
  }

  onSelectionChanged(e) {
    if(e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedSupplierGroupId)
    {
      this.selectedSupplierGroupId = e.selectedRowKeys[0];
      this.searchSupplier(e.selectedRowKeys[0]);
      this.supplierGroupId = e.selectedRowKeys[0];
    }
    
  }

  searchSupplier(SupplierGroupId: string) {
    this.model.SupplierGroupId = SupplierGroupId;
    this.supplierService.searchSupplier(this.model).subscribe((data: any) => {
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

  searchSupplierGroup() {
    this.materialGroupService.searchMaterialGroup(this.supplierGroupModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listSupplierGroup = data.ListResult;
        this.total = this.listSupplierGroup.length;
        this.listSupplierGroup.unshift(this.modelAll);
        if (this.selectedSupplierGroupId == null) {
          this.selectedSupplierGroupId = this.listSupplierGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedSupplierGroupId];
        for (var item of this.listSupplierGroup) {
          this.listSupplierGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      Email: '',
      PhoneNumber: '',
      ManufactureId: '',
      Note: '',
      ListSupplierContact: []
    }
    this.searchSupplierGroup();
    this.searchSupplier(this.supplierGroupId);
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchSupplier(this.supplierGroupId);
    }
  }

  showImportPopup() {
    this.supplierService.getGroupInTemplate().subscribe(d => {
      this.fileTemplate = this.config.ServerApi + d;
      this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
        if (data) {
          this.supplierService.importFile(data).subscribe(
            data => {
              this.searchSupplier(this.supplierGroupId);
              this.messageService.showSuccess('Import nhà cung cấp thành công!');
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

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SupplierCreateComponent, { container: 'body', windowClass: 'suppliercreate-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    if (this.supplierGroupId != null) {
      activeModal.componentInstance.supplierGroupId = this.supplierGroupId;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchSupplier(this.supplierGroupId);
      }
    }, (reason) => {
    });
  }

  showCreateUpdateGroup(Id: string, Index) {
    let activeModal = this.modalService.open(MaterialgroupCreateComponent, { container: 'body', windowClass: 'supplier-group-model', backdrop: 'static' })
    if (Index == 1) {
      activeModal.componentInstance.Id = Id;
    }
    else {
      activeModal.componentInstance.parentId = Id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchSupplierGroup();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhà cung cấp này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.supplierService.deleteSupplier({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        this.searchSupplier(this.supplierGroupId);
        this.messageService.showSuccess('Xóa nhà cung cấp thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteSupplierGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm vật tư này không?").then(
      data => {
        this.deleteSupplierGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteSupplierGroup(Id: string) {
    this.materialGroupService.deleteMaterialGroup({ Id: Id }).subscribe(
      data => {
        this.searchSupplierGroup();
        this.searchSupplier(this.supplierGroupId);
        this.messageService.showSuccess('Xóa nhóm vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel() {
    this.supplierService.exportExcel(this.model).subscribe(d => {
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
