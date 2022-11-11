import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AppSetting, Configuration, MessageService, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { ProjectServiceService } from '../../service/project-service.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowProjectComponent } from '../show-project/show-project.component'

import * as moment from 'moment';

@Component({
  selector: 'app-project-manage',
  templateUrl: './project-manage.component.html',
  styleUrls: ['./project-manage.component.scss']
})
export class ProjectManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    private combobox: ComboboxService,
    private messageService: MessageService,
    private projectService: ProjectServiceService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private route: ActivatedRoute,
    private modalService: NgbModal,
  ) { }

  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  SbuId: string;
  status1: number;
  status2: number;
  status3: number;
  sbuid = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;

  listPublications: any[] = [
    { Id: 1, Name: "Ảnh", Checked: false },
    { Id: 2, Name: "Video", Checked: false },
    { Id: 3, Name: "Catalog", Checked: false },
    { Id: 4, Name: "Web", Checked: false },
    { Id: 5, Name: "Kênh online khác", Checked: false },
  ]


  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    SBUId: '',
    DepartmentId: '',
    DateFrom: '',
    DateTo: '',
    Status: '',
    Note: '',
    search: '',
    CustomerTypeId: '',
    DateToV: '',
    DateFromV: '',
    CreateDate: '',
    Type: '',
    ProjectErrorStatus: null
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên dự án',
    Items: [
      {
        Name: 'Tên khách hàng',
        FieldName: 'CustomerName',
        Placeholder: 'Nhập tên khách hàng',
        Type: 'text'
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
        Permission: ['F060005'],
        RelationIndexTo: 2
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        Permission: ['F060005'],
        RelationIndexFrom: 1
      },
      {
        Name: 'Loại khách hàng',
        FieldName: 'CustomerTypeId',
        Placeholder: 'Loại khách hàng',
        Type: 'select',
        DataType: this.constant.SearchDataType.CustomerType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Trạng thái dự án',
        FieldName: 'Status',
        Placeholder: 'Trạng thái dự án',
        Type: 'select',
        Data: this.constant.ProjectStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng công nợ',
        FieldName: 'PaymentStatus',
        Placeholder: 'Tình trạng công nợ',
        Type: 'select',
        Data: this.constant.PaymentStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Trạng thái tài liệu',
        FieldName: 'DocumentStatus',
        Placeholder: 'Trạng thái tài liệu',
        Type: 'select',
        Data: this.constant.ProjectDocumentStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'Loại dự án',
        FieldName: 'Type',
        Placeholder: 'Loại dự án',
        Type: 'select',
        Data: this.constant.ProjectTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Vấn đề tồn đọng',
        FieldName: 'ErrorStatus',
        Placeholder: 'Vấn đề tồn đọng',
        Type: 'select',
        Data: this.constant.ProjectErrorStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      console.log(params);
      if (params.errorStatus) {
        this.model.ErrorStatus = parseInt(params.errorStatus);
        this.model.DateFromV = null;
        this.model.DateToV = null;
      }

      this.model.DepartmentId = params.departmentId;
      this.model.SBUId = params.sbuId;
      if (!this.model.SBUId) {
        this.model.SBUId = this.sbuid
      }
      this.searchProjects();
    });

    this.appSetting.PageTitle = "Quản lý dự án";
  }

  searchProjects() {
    if (this.model.DateFromV != null) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    if (this.model.DateFromV == null) {
      this.model.DateFrom = null;
    }
    if (this.model.DateToV != null) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    }
    if (this.model.DateToV == null) {
      this.model.DateTo = null;
    }
    this.projectService.searchProject(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.status1 = data.Status1;
        this.status2 = data.Status2;
        this.status3 = data.Status3;
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
      OrderBy: 'CreateDate',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      SBUId: '',
      DateFrom: '',
      DateTo: '',
      Status: '',
      Note: '',
      search: '',
      CustomerTypeId: '',
      DateToV: '',
      DateFromV: '',
      CreateDate: '',
      Type: null
    }
    this.searchProjects();
  }

  exportExcel() {
    this.projectService.excel(this.model).subscribe(d => {
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

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá dự án này không?").then(
      data => {
        this.deleteProject(Id);
      },
      error => {

      }
    );
  }

  deleteProject(Id: string) {
    this.projectService.Delete({ Id: Id }).subscribe(
      data => {
        this.searchProjects();
        this.messageService.showSuccess('Xóa dự án thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate() {
    this.router.navigate(['du-an/quan-ly-du-an/them-moi']);
  }

  showUpdate(Id: string) {
    this.router.navigate(['du-an/quan-ly-du-an/chinh-sua', Id]);
  }

  showProject(Id: string) {
    let activeModal = this.modalService.open(ShowProjectComponent, { container: 'body', windowClass: 'show-project-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjects();
      }
    }, (reason) => {
    });
  }

}
