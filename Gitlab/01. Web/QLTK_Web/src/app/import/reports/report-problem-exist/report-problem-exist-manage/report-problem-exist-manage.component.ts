import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportProblemExistService } from 'src/app/import/services/report-problem-exist.service';
import { AppSetting, MessageService, Constants, DateUtils } from 'src/app/shared';
import { ReportProblemExistCreateComponent } from '../report-problem-exist-create/report-problem-exist-create.component';

@Component({
  selector: 'app-report-problem-exist-manage',
  templateUrl: './report-problem-exist-manage.component.html',
  styleUrls: ['./report-problem-exist-manage.component.scss']
})
export class ReportProblemExistManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private service: ReportProblemExistService,
    public constant: Constants,
    public dateUtils: DateUtils,
  ) { }

  startIndex = 0;
  listData: any[] = [
    { id: '1', time: '12/07/2021', employeeName: 'Nguyễn Văn A' },
    { id: '2', time: '11/07/2021', employeeName: 'Nguyễn Văn A' }
  ];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'CreateReportDate',
    OrderType: false,
    Id: '',
    Code: '',
    DateToV: null,
    DateFromV: null,
    DateTo: null,
    DateFrom: null
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm mã hồ sơ, mã dự án, mã PR  ...',
    Items: [
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý báo cáo vấn đề tồn đọng";
    //this.search();
    this.startIndex = 1;
    this.model.TotalItems = this.listData.length;
  }

  search() {
    if (this.model.DateFromV != null) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    } else {
      this.model.DateFrom = null;
    }
    if (this.model.DateToV != null) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    } else {
      this.model.DateTo = null;
    }

    this.service.searchReportProblemExist(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Code: '',
      DateToV: null,
      DateFromV: null,
      DateTo: null,
      DateFrom: null
    }
    this.search();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ReportProblemExistCreateComponent, { container: 'body', windowClass: 'report-proplem-exist-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá báo cáo vấn đề tồn đọng này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.service.deleteReportProblemExist({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa báo cáo vấn đề tồn đọng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
