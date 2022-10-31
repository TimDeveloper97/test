import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants, ComboboxService, Configuration, DateUtils } from 'src/app/shared';
import { ReportStatusProductService } from '../../service/report-status-product.service';

@Component({
  selector: 'app-report-status-product',
  templateUrl: './report-status-product.component.html',
  styleUrls: ['./report-status-product.component.scss']
})
export class ReportStatusProductComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public serviceStatusProduct: ReportStatusProductService,
    private config: Configuration,
    public dateUtils: DateUtils,
  ) { }

  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo tình trạng thiết bị";
    this.searchProduct();
  }

  StartIndex = 0;
  TotalProduct: number;
  listProductUse: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'ProductCode',
    OrderType: true,
    ProductName: '',
    ProductCode: '',
    ProjectId: '',
    DateFrom: null,
    DateTo: null,
  }

  searchOptions: any = {
    FieldContentName: 'ProductCode',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên thiết bị',
        FieldName: 'ProductName',
        Placeholder: 'Nhập tên thiết bị ...',
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


  searchProduct() {
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
    this.serviceStatusProduct.SearchProduct(this.model).subscribe(
      data => {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listProductUse = data.ListProductUse;
        this.model.TotalItems = data.TotalProduct;

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
      OrderBy: 'ProductCode',
      OrderType: true,
      ProductName: '',
      ProductCode: '',
      ProjectId: '',
      DateFrom: null,
      DateTo: null,
    }
    this.searchProduct();
  }

  exportExcel(){
    this.serviceStatusProduct.excel(this.model).subscribe(d => {
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

  exportExcelProduct(){
    this.serviceStatusProduct.exportExcelProduct(this.model).subscribe(d => {
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
