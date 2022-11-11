import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportStatusModuleService } from '../service/report-status-module.service';

@Component({
  selector: 'app-report-status-module',
  templateUrl: './report-status-module.component.html',
  styleUrls: ['./report-status-module.component.scss']
})
export class ReportStatusModuleComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public serviceStatusModule: ReportStatusModuleService,
    private config: Configuration,
    public dateUtils: DateUtils,
  ) { }

  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo vấn đề tồn đọng";
    this.searchModule();
  }
  StartIndex = 0;
  TotalModule: number;
  listModuleUse: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'ModuleCode',
    OrderType: true,
    ModuleName: '',
    ModuleCode: '',
    ProjectId: '',
    DateFrom: null,
    DateTo: null,
  }

  searchOptions: any = {
    FieldContentName: 'ModuleCode',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên module',
        FieldName: 'ModuleName',
        Placeholder: 'Nhập tên module ...',
        Type: 'text'
      },
      {
        Placeholder: 'Chọn dự án',
        Name: 'Dự án',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.ProjectByUser,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ]
  };


  searchModule() {
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
    this.serviceStatusModule.SearchModule(this.model).subscribe(
      data => {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listModuleUse = data.ListModuleUse;
        this.model.TotalItems = data.TotalModule;

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'ModuleCode',
      OrderType: true,
      ModuleName: '',
      ModuleCode: '',
      ProjectId: '',
      DateFrom: null,
      DateTo: null,
    }
    this.searchModule();
  }

  exportExcel(){
    this.serviceStatusModule.excel(this.model).subscribe(d => {
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

  exportExcelModule(){
    this.serviceStatusModule.exportExcelModule(this.model).subscribe(d => {
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
