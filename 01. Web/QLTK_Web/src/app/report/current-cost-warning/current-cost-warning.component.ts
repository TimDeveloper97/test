import { Component, OnInit, ViewChild, ElementRef, OnDestroy, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { CurrentCostWarningService } from '../service/current-cost-warning.service';
import { AppSetting, MessageService, Constants, ComboboxService, DateUtils, Configuration } from 'src/app/shared';

@Component({
  selector: 'app-current-cost-warning',
  templateUrl: './current-cost-warning.component.html',
  styleUrls: ['./current-cost-warning.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CurrentCostWarningComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('scrollHeaderProject',{static:false}) scrollHeaderProject: ElementRef;
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  //@ViewChild('scrollHeaderError',{static:false}) scrollHeaderError: ElementRef;
  //@ViewChild('scrollHeaderTwo',{static:false}) scrollHeaderTwo: ElementRef;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private service: CurrentCostWarningService,
    private combobox: ComboboxService
  ) { }

  totalProject: number;
  totalPriceProject: number;
  totalDesignPrice: number;
  totalProjectProcessing: number = 0;
  totalPriceProcessing: number = 0;
  totalDesignPriceProcessing: number = 0;
  //listPriceProjectproduct: any[] = [];
  //listPriceError: any[] = [];
  listProject: any[] = [];
  listData: any[] = [];
  totalModuleGroup: number;
  totalError: number;
  height: number;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];

  model: any = {
    ProjectId: '',
    DateFrom: '',
    DateTo: '',
    DateToV: null,
    DateFromV: null,
  }

  searchOptions: any = {
    FieldContentName: 'ModuleCode',
    Placeholder: 'Tìm kiếm',
    Items: [
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

  ngOnInit() {
    // this.scrollHeaderError.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
    //   this.scrollHeaderTwo.nativeElement.scrollLeft = event.target.scrollLeft;
    // }, true);
    this.appSetting.PageTitle = "Báo cáo chi phí hiện tại";
    this.height = window.innerHeight - 310;
    this.getListProject();
    this.getDataCurrentCostWarning();
  }

  ngAfterViewInit(){
    this.scrollHeaderProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);    
  }

  ngOnDestroy() {
    this.scrollHeaderProject.nativeElement.removeEventListener('ps-scroll-x', null);
    //this.scrollHeaderError.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  getListProject() {
    this.combobox.getListProject().subscribe((data: any) => {
      this.listProject = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getDataCurrentCostWarning() {
    if (this.model.DateFromV != null) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    } else {
      this.model.DateFrom = null
    }

    if (this.model.DateToV != null) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    } else {
      this.model.DateTo = null;
    }
    this.service.getDataCurrentCostWarning(this.model).subscribe((data: any) => {
      this.totalProject = data.TotalProject;
      this.totalPriceProject = data.TotalPriceProject;
      this.totalDesignPrice = data.TotalDesignPrice;
      this.totalProjectProcessing = data.TotalProjectProcessing;
      this.totalPriceProcessing = data.TotalPriceProcessing;
      this.totalDesignPriceProcessing = data.TotalDesignPriceProcessing;
      //this.listPriceProjectproduct = data.ListPriceProjectproduct;
      //this.listPriceError = data.ListPriceError;
      this.listData = data.ListData;
      this.totalModuleGroup = data.TotalModuleGroup;
      this.totalError = data.TotalError;
    }, error => {
      this.messageService.showError(error);
    });
  }

  exportExcel() {
    this.service.excelCurrentCostWarning(this.listData).subscribe(d => {
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
