import { Component, OnInit } from '@angular/core';
import { StatusReportMaterialService } from '../service/status-report-material.service';
import { AppSetting, MessageService, Constants, Configuration, DateUtils } from 'src/app/shared';

@Component({
  selector: 'app-status-report-material',
  templateUrl: './status-report-material.component.html',
  styleUrls: ['./status-report-material.component.scss']
})
export class StatusReportMaterialComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    public constant: Constants,
    private service: StatusReportMaterialService,
    public dateUtils: DateUtils,
  ) { }

  model = {
    Code: '',
    DateFrom: null,
    DateTo: null,
    DateFromV : null,
    DateToV : null ,
    ProjectId:'',
    ModuleId:''
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã vật tư...',
    Items: [
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'Dự án',
        FieldName: 'ProjectId',
        Placeholder: 'Dự án',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Project,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Name',
        ValueName: 'Id',
      },

      {
        Name: 'Module',
        FieldName: 'ModuleId',
        Placeholder: 'Module',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Module,
        Columns: [{ Name: 'Code', Title: 'Mã module' }, { Name: 'Name', Title: 'Tên module' }],
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  totalModule: number = 0;
  totalMaterial : number = 0;
  listModule: any[] = []
  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo tình trạng vật tư trong Module";
  }

  getStatusReportMaterial() {
    
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

    if (!this.model.Code) {
      this.messageService.showMessage("Bạn chưa nhập mã vật tư");
      return;
    }

    this.service.getStatusReportMaterial(this.model).subscribe((data: any) => {
      this.listModule = data.ListResult;
      this.totalModule = data.TotalModule;
      this.totalMaterial = data.TotalMaterial;
    }, error => {
      this.messageService.showError(error);
    });
  }

  reportModuleMaterial() {
    this.service.reportModuleMaterial(this.model).subscribe((data: any) => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + data;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  moduleMaterialCheck3D() {
    this.service.moduleMaterialCheck3D().subscribe((data: any) => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + data;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  exportExcel(){
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
