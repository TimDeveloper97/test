import { Component, OnInit, ViewChild } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { DxTreeListComponent } from 'devextreme-angular';

import { AppSetting, Configuration, MessageService, Constants, ComponentService } from 'src/app/shared';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { ManufactureService } from '../../services/manufacture-service';
import { ManufacturerCreateComponent } from '../manufacturer-create/manufacturer-create.component';
import { ImportFileManafacturerComponent } from '../import-file-manafacturer/import-file-manafacturer.component';
import { MaterialgroupCreateComponent } from '../../materialgroup/materialgroup-create/materialgroup-create.component';

@Component({
  selector: 'app-manufacturer-manage',
  templateUrl: './manufacturer-manage.component.html',
  styleUrls: ['./manufacturer-manage.component.scss']
})
export class ManufacturerManageComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private manufactureService: ManufactureService,
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
  height = 0;
  items: any;
  total: number;
  pagination: any;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  logUserId: string;
  checkSearch: boolean = false;
  listManufactureGroup: any[] = [];
  listManufactureGroupId = [];
  selectedManufactureGroupId = '';
  manufactureGroupId: '';
  fileTemplate = '';

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

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
    Address: '',
    Phone: '',
    Description: '',
    MaterialType: '',
    Website: '',
  }

  manufactureGroupModel: any = {
    Id: '',
    Name: '',
    Code: '',
  }

  manufactureGroupAllModel: any = {
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã hãng',
    Items: [
      {
        Name: 'Tên hãng sản xuất',
        FieldName: 'Name',
        Placeholder: 'Nhập tên hãng sản xuất',
        Type: 'text'
      },
      {
        Name: 'Loại vật tư',
        FieldName: 'MaterialType',
        Placeholder: 'Loại vật tư',
        Type: 'select',
        Data: this.constant.MaterialType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 140;
    if (window.innerHeight <= 768 && window.innerWidth <= 1024) {
      this.checkSearch = true;
    } else { this.checkSearch = false; }
    this.appSetting.PageTitle = "Hãng sản xuất";
    this.searchManufactureGroup();
    this.searchManufacturer(this.manufactureGroupId)
    this.selectedManufactureGroupId = localStorage.getItem("selectedManufactureGroupId");
    localStorage.removeItem("selectedManufactureGroupId");
  }

  itemClick(e) {
    if (this.manufactureGroupId == '' || this.manufactureGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm vật tư!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateGroup(this.manufactureGroupId, 2)
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdateGroup(this.manufactureGroupId, 1);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteManufactureGroup(this.manufactureGroupId);
      }
    }
  }

  onSelectionChanged(e) {
    if(e.selectedRowKeys[0] != null  && e.selectedRowKeys[0] != this.selectedManufactureGroupId)
    {
      this.selectedManufactureGroupId = e.selectedRowKeys[0];
      this.searchManufacturer(e.selectedRowKeys[0]);
      this.manufactureGroupId = e.selectedRowKeys[0];
    }
    
  }

  searchManufactureGroup() {
    this.materialGroupService.searchMaterialGroup(this.manufactureGroupModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listManufactureGroup = data.ListResult;
        this.total = this.listManufactureGroup.length;
        this.listManufactureGroup.unshift(this.manufactureGroupAllModel);
        if (this.selectedManufactureGroupId == null) {
          this.selectedManufactureGroupId = this.listManufactureGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedManufactureGroupId];
        for (var item of this.listManufactureGroup) {
          this.listManufactureGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchManufacturer(ManufactureGroupId: string) {
    this.model.ManufactureGroupId = ManufactureGroupId;
    this.manufactureService.searchManufacturer(this.model).subscribe((data: any) => {
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
      Address: '',
      Phone: '',
      Description: '',
      MaterialType: '',
    }
    this.searchManufactureGroup();
    this.searchManufacturer(this.manufactureGroupId);
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchManufacturer(this.manufactureGroupId);
    }
  }


  showConfirmDeleteManufacturer(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá hãng sản xuất này không?").then(
      data => {
        this.deleteManufacturer(Id);
      },
      error => {
        
      }
    );
  }

  deleteManufacturer(Id: string) {
    this.manufactureService.deleteManufacturer({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        this.searchManufacturer(this.manufactureGroupId);
        this.messageService.showSuccess('Xóa hãng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteManufactureGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm vật tư này không?").then(
      data => {
        this.deleteManufactureGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteManufactureGroup(Id: string) {
    this.materialGroupService.deleteMaterialGroup({ Id: Id }).subscribe(
      data => {
        this.searchManufactureGroup();
        this.searchManufacturer(this.manufactureGroupId);
        this.messageService.showSuccess('Xóa nhóm vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showImportPopup() {
    // let activeModal = this.modalService.open(ImportFileManafacturerComponent, { container: 'body', windowClass: 'import-file-manufacturer-modal', backdrop: 'static' })
    // activeModal.result.then((result) => {
    //   if (result) {
    //     this.searchManufacturer(this.manufactureGroupId);
    //   }
    // }, (reason) => {
    // });
    this.manufactureService.getGroupInTemplate().subscribe(d => {
      this.fileTemplate = this.config.ServerApi + d;
      this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
        if (data) {
          this.manufactureService.importFile(data).subscribe(
            data => {
              this.searchManufacturer(this.manufactureGroupId);
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

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ManufacturerCreateComponent, { container: 'body', windowClass: 'manufacturecreate-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    if (this.manufactureGroupId != null) {
      activeModal.componentInstance.manufactureGroupId = this.manufactureGroupId;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchManufacturer(this.manufactureGroupId);
      }
    }, (reason) => {
    });
  }

  showCreateUpdateGroup(Id: string, Index) {
    let activeModal = this.modalService.open(MaterialgroupCreateComponent, { container: 'body', windowClass: 'manufacturecreate-group-model', backdrop: 'static' })
    if (Index == 1) {
      activeModal.componentInstance.Id = Id;
    }
    else {
      activeModal.componentInstance.parentId = Id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchManufactureGroup();
      }
    }, (reason) => {
    });
  }

  exportExcel() {
    this.manufactureService.exportExcel(this.model).subscribe(d => {
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
