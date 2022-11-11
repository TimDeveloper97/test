import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { CandidateService } from '../../services/candidate.service';

@Component({
  selector: 'app-candidate-manage',
  templateUrl: './candidate-manage.component.html',
  styleUrls: ['./candidate-manage.component.scss']
})
export class CandidateManageComponent implements OnInit {

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,

    Name: '',
    PhoneNumber: "",
    WorkTypeId: '',
    Email: '',
    ApplyDateTo: null,
    ApplyDateFrom: null,
    ApplyDateToV: null,
    ApplyDateFromV: null,
  };

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo mã/tên ứng viên',
    Items: [
      {
        Name: 'Số điện thoại',
        FieldName: 'PhoneNumber',
        Placeholder: 'Nhập số điện thoại',
        Type: 'text'
      },
      {
        Name: 'Email',
        FieldName: 'Email',
        Placeholder: 'Email',
        Type: 'text'
      },
      {
        Name: 'Vị trí tuyển dụng',
        FieldName: 'WorkTypeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.WorkType,
        Columns: [{ Name: 'Name', Title: 'Vị trí tuyển dụng' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn vị trí tuyển dụng',
      },
      {
        Name: 'Thời gian ứng tuyển',
        FieldNameTo: 'ApplyDateToV',
        FieldNameFrom: 'ApplyDateFromV',
        Type: 'date'
      },     
    ]
  };

  candidates: any[] = []
  startIndex: number = 1;

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,
    private modalService: NgbModal,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private candidateService: CandidateService,
    private dateUtils: DateUtils) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Hồ sơ Ứng viên";
    this.searchCandidate();
  }

  searchCandidate() {
    this.searchModel.ApplyDateTo = null;
    if (this.searchModel.ApplyDateToV) {
      this.searchModel.ApplyDateTo = this.dateUtils.convertObjectToDate(this.searchModel.ApplyDateToV);
    }

    this.searchModel.ApplyDateFrom = null;
    if (this.searchModel.ApplyDateFromV) {
      this.searchModel.ApplyDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.ApplyDateFromV);
    }

    this.searchModel.InterviewDateTo = null;
    if (this.searchModel.InterviewDateToV) {
      this.searchModel.InterviewDateTo = this.dateUtils.convertObjectToDate(this.searchModel.InterviewDateToV);
    }

    this.searchModel.InterviewDateFrom = null;
    if (this.searchModel.InterviewDateFromV) {
      this.searchModel.InterviewDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.InterviewDateFromV);
    }

    this.candidateService.searchCandidate(this.searchModel).subscribe(
      data => {
        this.candidates = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Name: '',
      PhoneNumber: "",
      WorkTypeId: '',
      Email: '',
      ApplyDateTo: null,
      ApplyDateFrom: null,
      InterviewDateTo: null,
      InterviewDateFrom: null,
      ApplyDateToV: null,
      ApplyDateFromV: null,
      InterviewDateToV: null,
      InterviewDateFromV: null
    };
    this.searchCandidate();
  }

  showCreateUpdate() {
    this.router.navigate(['tuyen-dung/ho-so-ung-vien/them-moi']);
  }

  showConfirmDelete(id: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhân viên này không?").then(
      data => {
        this.delete(id);
      },
      error => {

      }
    );
  }

  delete(id) {
    this.candidateService.deleteCandidate({ Id: id }).subscribe(
      data => {
        this.searchCandidate();
        this.messageService.showSuccess('Xóa ứng viên thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
