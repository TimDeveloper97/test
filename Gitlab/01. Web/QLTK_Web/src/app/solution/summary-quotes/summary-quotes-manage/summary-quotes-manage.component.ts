import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSetting, ComboboxService, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { SummaryQuotesService } from '../../service/summary-quotes.service';

@Component({
  selector: 'app-summary-quotes-manage',
  templateUrl: './summary-quotes-manage.component.html',
  styleUrls: ['./summary-quotes-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SummaryQuotesManageComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private service: SummaryQuotesService,
    private messageService: MessageService,
    private router: Router,
    public dateUtils: DateUtils,
    private config: Configuration,
  ) { }

  ModalInfo = {
    Title: '',
  };

  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: true,
    DateFrom: '',
    DateTo: '',
    DateFromV: '',
    DateToV: '',
    Status: -1,
  }

  listData: any[] = [];
  listShowPlan: any[] = [];
  startIndex = 0;
  Province: string;
  selectIndex = -1;
  DepartmentName: string;
  listDataPlan: any[] = [];
  listDataPlanbyQuotation: any[] = [];
  startIndexPlan = 1;

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên YCKH hoặc số YCKH...',
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
      },
      {
        Name: 'Phòng báo giá',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng báo giá',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Nhân viên báo giá',
        FieldName: 'EmployeeId',
        Placeholder: 'Nhân viên báo giá',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Employee,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian báo giá',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'Giá trị báo giá',
        FieldName: 'QuotationPrice',
        FieldNameType: 'QuotationPriceType',
        Placeholder: 'Nhập giá trị báo giá ...',
        Type: 'number'
      },
      {
        Name: 'Trạng thái báo giá',
        FieldName: 'Status',
        Placeholder: 'Trạng thái báo giá',
        Type: 'select',
        Data: this.constant.QuotationStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
   
    this.appSetting.PageTitle = "Tổng hợp báo giá";
    this.model.DateFromV = null;
    this.model.DateToV = null;
    this.searchQuotation();
  }

  searchQuotation() {
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
    if (this.model.Status == null)
    {
      this.model.status = -1;
    }
    this.service.getAllQuotationInfo(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.totalItems = data.TotalItem;
        this.selectIndex = -1;
        this.listDataPlan = [];
        this.ModalInfo.Title = ''
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showUpdate(Id: string) {
    this.router.navigate(['/giai-phap/tong-hop-bao-gia/chinh-sua/', Id]);
  }

  showConfirmDelete(QuotationId: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá báo giá này không?").then(
      data => {
        this.delete(QuotationId);
      },
      error => {

      }
    );
    
  }
  delete(QuotationId: string) {
    this.service.deleteQuotation(QuotationId).subscribe(
      data => {
        this.messageService.showSuccess('Xóa báo giá thành công!');
        this.searchQuotation();
      },
      error => {
        this.messageService.showError(error);
      }
    );
    this.selectIndex = -1;
    this.listDataPlan = [];
    this.ModalInfo.Title = '';
  }

  clear() {
    this.model = {
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'CreateDate',
      OrderType: true,
      DateFrom: '',
      DateTo: '',
      DateFromV: '',
      DateToV: '',
      Status: -1,
    }
    this.searchQuotation();
  }

  selectQuotes(QuotationId, index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.service.getQuotationById(QuotationId).subscribe(data => {
        this.ModalInfo.Title = ' - ' + data.data.Code;
        this.DepartmentName = data.departmentName.Name;
      });
      this.getPlan(QuotationId);
    }
    else {
      this.selectIndex = -1;
      this.listDataPlan = [];
      this.ModalInfo.Title = ''
    }
  }

  getPlan(QuotationId) {
    this.service.getQuotationPlan(QuotationId).subscribe(data => {
      if (data) {
        this.listDataPlan = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel(QuotationId) {
    this.service.exportExcel(this.model, QuotationId).subscribe(d => {
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
