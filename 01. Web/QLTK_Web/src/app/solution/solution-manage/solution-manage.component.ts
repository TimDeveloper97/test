import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';

import { AppSetting, Configuration, MessageService, Constants, DateUtils } from 'src/app/shared';
import { SolutionService } from '../service/solution.service';
import { SolutionGroupService } from '../service/solution-group.service';
import { SolutionCreateComponent } from '../solution-create/solution-create.component';
import { SolutionGroupCreateComponent } from '../solution-group-create/solution-group-create.component';
import { SolutionUpdateComponent } from '../solution-update/solution-update.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-solution-manage',
  templateUrl: './solution-manage.component.html',
  styleUrls: ['./solution-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SolutionManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private service: SolutionService,
    private serviceSolutionGroup: SolutionGroupService,
    public dateUtils: DateUtils,

  ) {
    this.items = [
      { Id: 1, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }
  height = 0;
  startIndex = 0;
  items: any;
  listData: any[] = [];
  listSolutionGroup: any[] = [];
  listSolutionGroupId = [];
  selectedSolutionGroupId = '';
  solutionGroupId: '';
  sbuid = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
  departermentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId
  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Code: '',
    Name: '',
    SolutionGroupId: '',
    ProjectId: '',
    DateToV: '',
    DateFromV: '',
    SBUId: this.sbuid,
    DepartmentId: this.departermentId,
    Status: null,
    totalenoughData: 0,
    totalStatus: 0,
    totalisenoughData: 0
  }

  solutionGroupModel: any = {
    TotalItems: 0,
    Id: '',
    Name: '',
    Code: '',
  }

  modelAll: any = {
    TotalItems: 0,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  fileModel = {
    Id: '',
    SolutionId: '',
    FileName: '',
    FileSize: '',
    Path: '',
  }

  listStatus = [
    { Id: 1, Name: "Đang triển khai" },
    { Id: 2, Name: "Thành dự án" },
    { Id: 3, Name: "Không thành dự án" },
    { Id: 4, Name: "Tạm dừng" },
    { Id: 5, Name: "Hủy" }
  ]

  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Giải pháp";
    this.searchSolutionGroup();
    this.searchSolution("");
    this.selectedSolutionGroupId = localStorage.getItem("selectedSolutionGroupId");
    localStorage.removeItem("selectedSolutionGroupId");
  }

  itemClick(e) {
    if (this.solutionGroupId == '' || this.solutionGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm giải pháp!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdate(this.solutionGroupId);
      }
      else if (e.itemData.Id == 2) {
        this.showConfirmDeleteSolutionGroup(this.solutionGroupId);
      }
    }
  }

  onSelectionChanged(e) {
    // this.selectedSolutionGroupId = e.selectedRowKeys[0];
    // this.searchSolution(e.selectedRowKeys[0]);
    // this.solutionGroupId = e.selectedRowKeys[0];

    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedSolutionGroupId) {
      this.selectedSolutionGroupId = e.selectedRowKeys[0];
      this.searchSolution(e.selectedRowKeys[0]);
      this.solutionGroupId = e.selectedRowKeys[0];
    }
  }

  searchSolutionGroup() {
    this.serviceSolutionGroup.searchSolutionGroup(this.solutionGroupModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listSolutionGroup = data.ListResult;
        this.listSolutionGroup.unshift(this.modelAll);
        this.solutionGroupModel.TotalItems = data.TotalItem;
        if (this.selectedSolutionGroupId == null) {
          this.selectedSolutionGroupId = this.listSolutionGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedSolutionGroupId];
        for (var item of this.listSolutionGroup) {
          this.listSolutionGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã hoặc tên giải pháp',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn SBU',
        RelationIndexTo: 1,
        Permission: ['F070105'],
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn phòng ban',
        RelationIndexFrom: 0,
        Permission: ['F070105'],
      },
      {
        Name: 'Khách hàng',
        FieldName: 'CustomerName',
        Placeholder: 'Nhập khách hàng',
        Type: 'text'
      },
      {
        Name: 'Khách hàng cuối',
        FieldName: 'EndCustomerName',
        Placeholder: 'Nhập khách hàng cuối',
        Type: 'text'
      },
      {
        Name: 'Trạng thái',
        FieldName: 'Status',
        Placeholder: 'Trạng thái',
        Type: 'select',
        Data: this.constant.StatusSolution,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian bắt đầu',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'Trạng thái hồ sơ',
        FieldName: 'Typedocuments',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.IsProduct,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Mảng kinh doanh',
        FieldName: 'BusinessDomain',
        Placeholder: 'Mảng KD',
        Type: 'select',
        Data: this.constant.BusinessDomainSolution,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  searchSolution(solutionGroupId: string) {
    this.model.SolutionGroupId = solutionGroupId;
    if (this.model.DateFromV) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    if (this.model.DateFromV == null) {
      this.model.DateFrom = null;
    }
    if (this.model.DateToV) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    }
    if (this.model.DateToV == null) {
      this.model.DateTo = null;
    }
    this.service.searchSolution(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.totalItems = data.TotalItem;
        this.model.totalenoughData = data.Status2;
        this.model.totalStatus = data.Status1;
        this.model.totalisenoughData = data.Status3
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear(solutionGroupId: string) {
    this.model = {
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Code: '',
      Name: '',
      SolutionGroupId: '',
      ProjectId: '',
      DepartmentId: '',
      // DateToV: this.dateUtils.getFiscalYearEnd(),
      // DateFromV: this.dateUtils.getFiscalYearStart(),
      SBUId: '',
      Status: null,
    }
    this.searchSolution(solutionGroupId);
  }

  showConfirmDeleteSolutionGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm giải pháp này không?").then(
      data => {
        this.deleteSolutionGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteSolutionGroup(Id: string) {
    this.serviceSolutionGroup.deleteSolutionGroup({ Id: Id }).subscribe(
      data => {
        this.selectedSolutionGroupId = '';
        this.searchSolutionGroup();
        this.searchSolution("");
        this.messageService.showSuccess('Xóa nhóm giải pháp thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteSolution(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá giải pháp này không?").then(
      data => {
        this.deleteSolution(Id);
      },
      error => {
        
      }
    );
  }

  deleteSolution(Id: string) {
    this.service.deleteSolution({ Id: Id }).subscribe(
      data => {
        this.searchSolution(this.solutionGroupId);
        this.messageService.showSuccess('Xóa giải pháp thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreate(Id: string) {
    this.router.navigate(['giai-phap/quan-ly-giai-phap/them-moi-giai-phap']);
    // let activeModal = this.modalService.open(SolutionCreateComponent, { container: 'body', windowClass: 'solution-create-model', backdrop: 'static' })
    // activeModal.componentInstance.Id = Id;
    // activeModal.result.then((result) => {
    //   if (result) {
    //     this.searchSolution("");
    //   }
    // }, (reason) => {
    // });
  }

  showUpdate(Id: string) {
    this.router.navigate(['giai-phap/quan-ly-giai-phap/chinh-sua-giai-phap/', Id]);
    // let activeModal = this.modalService.open(SolutionUpdateComponent, { container: 'body', windowClass: 'solution-update-model', backdrop: 'static' })
    // activeModal.componentInstance.Id = Id;
    // activeModal.result.then((result) => {
    //   if (result) {
    //     this.searchSolution("");
    //   }
    // }, (reason) => {
    // });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SolutionGroupCreateComponent, { container: 'body', windowClass: 'solution-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSolutionGroup();
        this.searchSolution(Id);
      }
    }, (reason) => {
    });
  }

  showAll() {
    this.solutionGroupId = null;
    this.searchSolution(this.solutionGroupId);
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

}
