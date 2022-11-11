import { Component, OnInit, ViewChild } from '@angular/core';
import { DxTreeListComponent } from 'devextreme-angular';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { AppSetting, MessageService, Constants, ComboboxService, Configuration } from 'src/app/shared';
import { DxTreeListModule } from "devextreme-angular";
import { TestCriteriaCreateComponent } from '../test-criteria-create/test-criteria-create.component';
import { TestcriteriaService } from '../../services/testcriteria';
import { TestcriteriagroupService } from '../../services/testcriteriagroup-service';
import { TestCriteriaGroupCreateComponent } from '../../testcriteriagroup/test-criteria-group-create/test-criteria-group-create.component';

@Component({
  selector: 'app-test-criteria-manage',
  templateUrl: './test-criteria-manage.component.html',
  styleUrls: ['./test-criteria-manage.component.scss']
})
export class TestCriteriaManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private testcriteriaService: TestcriteriaService,
    private testCriteriaGroupService: TestcriteriagroupService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    private config :  Configuration
  ) {
    this.pagination = Object.assign({}, appSetting.Pagination);
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fas fa-plus' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ]; }
  items: any;
  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  listTestCriterGroup: any[] = [];
  listCRI: any[] = [];
  logUserId: string;
  moduleTestCRIId: '';
  selectedTestCRIGroupId = '';
  listTestCRIGroupId = [];

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Code: '',
    Name: '',
    TestCriteriaGroupName: '',
    TestCriteriaGroupId: '',
    TechnicalRequirements: '',
    Note: '',

  }

  Id: string;
  modelTestCrieee: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,


    Id: '',
    Name: '',
    Code: '',
    Note: '',

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
    DataType:null
  }

  getFunctionGroupInfo() {
    this.testCriteriaGroupService.getTestCriteralGroup({ Id: this.Id }).subscribe(data => {
      this.getFunctionGroupInfo = data;
    });
  }
  height = 0;
  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.getCbbCriter();
    this.appSetting.PageTitle = "Tiêu chí kiểm tra";
    this.searchTestCRIGroup();
    this.searchTestCriteria(this.moduleTestCRIId);

    this.selectedTestCRIGroupId = localStorage.getItem("selectedTestCRIGroupId");
    localStorage.removeItem("selectedTestCRIGroupId");
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã tiêu chí',
    Items: [
      {
        Name: 'Tên tiêu chí',
        FieldName: 'Name',
        Placeholder: 'Nhập tên tiêu chí',
        Type: 'text'
      },
      {
        Name: 'Yêu cầu kĩ thuật',
        FieldName: 'TechnicalRequirements',
        Placeholder: 'Nhập yêu cầu kĩ thuật',
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
    ]
  };

  itemClick(e) {
    if (this.moduleTestCRIId == '' || this.moduleTestCRIId == null) {
      this.messageService.showMessage("Đây không phải nhóm tiêu chí!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateGroup("",1)
      }
      else if (e.itemData.Id == 2) {
        this.showCreateGroup(this.moduleTestCRIId, 1);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteTestCriterGroup(this.moduleTestCRIId);
      }
    }
  }

  searchTestCRIGroup() {
    this.testCriteriaGroupService.searchTestCriterialGroup((this.modelTestCrieee)).subscribe((data: any) => {
      if (data.ListResult) {
        this.listTestCriterGroup = data.ListResult;
        this.listTestCriterGroup.unshift(this.modelAll);
        this.modelTestCrieee.totalItems = data.TotalItem;
        if (this.selectedTestCRIGroupId == null) {
          this.selectedTestCRIGroupId = this.listTestCriterGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedTestCRIGroupId];
        for (var item of this.listTestCriterGroup) {
          this.listTestCRIGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchTestCriteria(moduleTestCRIId: string) {
    this.model.TestCriteriaGroupId = moduleTestCRIId;
    this.testcriteriaService.SearchTestCriteria(this.model).subscribe((data: any) => {
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

  getCbbCriter() {
    this.comboboxService.getCbbCriter().subscribe((data: any) => {
      this.listCRI = data;
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
      TestCriteriaGroupId: '',
      Name: '',
      TechnicalRequirements: '',
      moduleTestCRIId: '',
      Code: '',
      Note: '',
    }
    this.searchTestCRIGroup();
    this.searchTestCriteria(this.moduleTestCRIId);
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchTestCriteria("");
    }
  }

  showConfirmDeleteTestCriterGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm tiêu chí này không?").then(
      data => {
        this.deleteTestCriterGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteTestCriterGroup(Id: string) {
    this.testCriteriaGroupService.deleteTestCriteralGroup({ Id: Id }).subscribe(
      data => {
        this.searchTestCRIGroup();
        this.messageService.showSuccess('Xóa nhóm tiêu chí thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }


  showConfirmDeleteTestCriteria(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tiêu chí này không?").then(
      data => {
        this.deleteTestCriteria(Id);
      },
      error => {
        
      }
    );
  }

  deleteTestCriteria(Id: string) {
    this.testcriteriaService.DeleteTestCriteria({ Id: Id }).subscribe(
      data => {
        this.check = true;
        this.searchTestCriteria(this.moduleTestCRIId);
        this.messageService.showSuccess('Xóa tiêu chí thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateGroup(Id: string, Index) {
    let activeModal = this.modalService.open(TestCriteriaGroupCreateComponent, { container: 'body', windowClass: 'test-criteria-group-create-model', backdrop: 'static' })
    if (Index == 1) {
      activeModal.componentInstance.Id = Id;
    }
    else {
      activeModal.componentInstance.parentId = Id;
    }
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchTestCRIGroup();
      }
    }, (reason) => {
    });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(TestCriteriaCreateComponent, { container: 'body', windowClass: 'test-criteria-create-modal', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.moduleTestCRIId = this.moduleTestCRIId
    activeModal.result.then((result) => {
      if (result) {
        this.searchTestCriteria(this.moduleTestCRIId);
      }
    }, (reason) => {
    });
  }

  check: boolean = true;
  onSelectionChanged(e) {
    
    if(e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedTestCRIGroupId)
    {
      this.selectedTestCRIGroupId = e.selectedRowKeys[0];
      this.searchTestCriteria(e.selectedRowKeys[0]);
      this.moduleTestCRIId = e.selectedRowKeys[0];
    }
  }

  // 04-02-2020 * Thêm mới xuất excel

  exportExcel() {
    this.testcriteriaService.excel(this.model).subscribe(d => {
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
