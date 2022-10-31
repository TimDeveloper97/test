import { Component, OnInit, Input } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { AppSetting, MessageService, Constants, DateUtils } from 'src/app/shared';

import { ModuleProjectService } from '../../services/module-project.service';
import { ModuleProjectTestCriteiaComponent } from '../module-project-test-criteia/module-project-test-criteia.component';

@Component({
  selector: 'app-module-project',
  templateUrl: './module-project.component.html',
  styleUrls: ['./module-project.component.scss']
})
export class ModuleProjectComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private projectService: ModuleProjectService,
    public constant: Constants,
    public dateUtils: DateUtils,
  ) { }

  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  logUserId: string;
  totalModule: number;
  totalProject: number;

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  model: any = {
    Id: '',
    Status: '',
    ProjectName: '',
    ProjectCode: '',
    ProjectProductId: '',
    DateToV: '',
    DateFromV: '',
  }

  searchOptions: any = {
    FieldContentName: 'ProjectCode',
    Placeholder: 'Tìm kiếm theo mã dự án',
    Items: [
      {
        Name: 'Tên dự án',
        FieldName: 'ProjectName',
        Placeholder: 'Nhập tên dự án',
        Type: 'text'
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
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ]
  };

  ngOnInit() {
    this.model.ModuleId = this.Id;
    this.searchProjectModel();

  }

  searchProjectModel() {
    if (this.model.DateFromV) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    if (this.model.DateToV) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    }
    this.projectService.SearchProjectModel(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listDA = data.ListResult;
        this.totalModule = data.TotalModule;
        this.totalProject = data.TotalProject;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  showCreateUpdate(Id) {
    let activeModal = this.modalService.open(ModuleProjectTestCriteiaComponent, { container: 'body', windowClass: 'ModuleProjectTestCriteia-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProjectModel();
      }
    }, (reason) => {
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

      ProjectName: '',
      ProjectCode: '',

    }
    this.model.ModuleId = this.Id;
    this.searchProjectModel();
  }


}
