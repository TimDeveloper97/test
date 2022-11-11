import { Component, OnInit, ViewChild } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FunctionService } from '../../services/module-funcion.service';
import { FunctionCreateComponent } from '../function-create/function-create.component';
import { FunctionGroupsService } from '../../services/function-groups.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { FunctionGroupsCreateComponent } from '../../functiongroups/function-groups-create/function-groups-create.component';
import { DxTreeListComponent } from 'devextreme-angular';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Component({
  selector: 'app-function-manage',
  templateUrl: './function-manage.component.html',
  styleUrls: ['./function-manage.component.scss']
})
export class FunctionManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView!: DxTreeListComponent;
  constructor(

    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private functionService: FunctionService,
    public constant: Constants,
    private serviceFunctionGroup: FunctionGroupsService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) {
    this.items = [
      //{ Id: 1, text: 'Thêm mới loại phòng học', icon: 'fas fa-plus' },
      { Id: 2, text: 'Chỉnh sửa', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }
  items: any;

  StartIndex = 0;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    FunctionGroupId: '',
    Name: '',
    Code: '',
    Note: '',
    TechnicalRequire: '',
    FunctionGroupName: '',
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

  ModalInfo = {
    Title: 'Thêm mới nhóm tiêu chuẩn',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id!: string;
  ListFunctionGroup: any[] = []

  modelFunctionGroups: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    TotalItem: 0,
  }

  FunctionGroupId = '';
  selectedFunctionGroupId: any = '';

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  height = 0;
  ngOnInit() {
    this.height = window.innerHeight - 120;
    this.appSetting.PageTitle = "Quản lý tính năng";
    this.searchFunctionGroup();
    this.searchFunction("");

    this.selectedFunctionGroupId = localStorage.getItem("selectedFunctionGroupId") || '';

    localStorage.removeItem("selectedFunctionGroupId");
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã tính năng',
    Items: [
      {
        Name: 'Tên tính năng',
        FieldName: 'Name',
        Placeholder: 'Nhập tên tính năng',
        Type: 'text'
      },
    ]
  };

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      FunctionGroupId: '',
      Name: '',
      Code: '',
      Note: '',
      TechnicalRequire: '',
      FunctionGroupName: '',
    }
    this.searchFunction("");
  }
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(FunctionCreateComponent, { container: 'body', windowClass: 'Function-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    if (this.FunctionGroupId != null) {
      activeModal.componentInstance.FunctionGroupId = this.FunctionGroupId;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchFunction("");
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteFunction(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tính năng này không?").then(
      data => {
        this.deleteFunction(Id);
      },
      error => {
        
      }
    );
  }

  deleteFunction(Id: string) {
    this.functionService.deleteFunction({ Id: Id }).subscribe(
      data => {
        this.selectedFunctionGroupId = localStorage.getItem("selectedFunctionGroupId");
        this.searchFunction("");
        this.messageService.showSuccess('Xóa tính năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(FunctionGroupsCreateComponent, { container: 'body', windowClass: 'functiongroups-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchFunctionGroup();
      }
    }, (reason) => {
    });
  }

  getFunctionGroupInfo() {
    this.serviceFunctionGroup.getFunctionGroup({ Id: this.Id }).subscribe(data => {
      this.modelFunctionGroups = data;
    });
  }


  onSelectionChanged(e: any) {
    // this.selectedFunctionGroupId = e.selectedRowKeys[0];  
    // this.FunctionGroupId = e.selectedRowKeys[0];
    // this.searchFunction(e.selectedRowKeys[0]);

    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedFunctionGroupId) {
      this.selectedModelGroupId = e.selectedRowKeys[0];
      this.searchFunction(e.selectedRowKeys[0]);
      this.FunctionGroupId = e.selectedRowKeys[0];
    }
  }

  ListFunctionGroupId: any[] = [];
  searchFunctionGroup() {
    this.serviceFunctionGroup.searchFunctionGroup(this.modelFunctionGroups).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListFunctionGroup = data.ListResult;
        this.ListFunctionGroup.unshift(this.modelAll);
        this.modelFunctionGroups.TotalItems = data.TotalItem;
        if (this.selectedFunctionGroupId == null) {
          this.selectedFunctionGroupId = this.ListFunctionGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedFunctionGroupId];
        for (var item of this.ListFunctionGroup) {
          this.ListFunctionGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchFunction(FunctionGroupId: string) {
    this.model.FunctionGroupId = FunctionGroupId;
    this.functionService.searchFunction(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  clearModelFunctionGroups() {
    this.modelFunctionGroups = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
    }
    this.searchFunctionGroup();
  }

  showConfirmDeleteFunctionGroups(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm tính năng này không?").then(
      data => {
        this.deleteFunctionGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteFunctionGroup(Id: string) {
    this.serviceFunctionGroup.deleteFunctionGroup({ Id: Id }).subscribe(
      data => {
        this.searchFunctionGroup();
        this.messageService.showSuccess('Xóa nhóm tính năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdateFunctionGroup(Id: string) {
    let activeModal = this.modalService.open(FunctionGroupsCreateComponent, { container: 'body', windowClass: 'functiongroups-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchFunctionGroup();
      }
    }, (reason) => {
    });
  }

  selectedModelGroupId = '';
  typeId!: number;
  /// Skien click chuột phải
  itemClick(e: any) {
    if (e.itemData.Id == 1) {
      this.ShowCreateUpdateFunctionGroup('');
    } else if (e.itemData.Id == 2) {
      this.ShowCreateUpdateFunctionGroup(this.FunctionGroupId);
    } else if (e.itemData.Id == 3) {
      this.showConfirmDeleteFunctionGroups(this.FunctionGroupId);
    }
  }
}

