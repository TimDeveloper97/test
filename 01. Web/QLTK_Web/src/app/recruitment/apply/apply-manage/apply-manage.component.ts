import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { ApplyService } from '../../services/apply.service';
import { ApplyCheckCandidateComponent } from '../apply-check-candidate/apply-check-candidate.component';

@Component({
  selector: 'app-apply-manage',
  templateUrl: './apply-manage.component.html',
  styleUrls: ['./apply-manage.component.scss']
})
export class ApplyManageComponent implements OnInit {

  ListFollowStatus: any[] = [
    { Id: 1, Name: "Giữ liên hệ" },
    { Id: 2, Name: "Không giữ liên hệ" },
  ]

  idCheck = '0';

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
    Status: null,
    ProfileStatus: null,
    InterviewStatus: null,
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
        Name: 'Vị trí ứng tuyển',
        FieldName: 'WorkTypeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.WorkType,
        Columns: [{ Name: 'Name', Title: 'Tên vị trí ứng tuyển' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn Vị trí ứng tuyển',
      },
      {
        Name: 'Thời gian ứng tuyển',
        FieldNameTo: 'ApplyDateToV',
        FieldNameFrom: 'ApplyDateFromV',
        Type: 'date'
      },
      {
        Name: 'Tình trạng hồ sơ',
        FieldName: 'ProfileStatus',
        Placeholder: 'Tình trạng hồ sơ',
        Type: 'select',
        Data: this.constant.ProfileStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng phỏng vấn',
        FieldName: 'InterviewStatus',
        Placeholder: 'Tình trạng phỏng vấn',
        Type: 'select',
        Data: this.constant.InterviewStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng ứng tuyển',
        FieldName: 'Status',
        Placeholder: 'Tình trạng ứng tuyển',
        Type: 'select',
        Data: this.constant.ApplyStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Yêu cầu tuyển dụng',
        FieldName: 'RecruitmentRequestId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.RecruitmentRequest,
        Columns: [{ Name: 'Name', Title: 'Mã yêu cầu tuyển dụng' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn mã Yêu cầu tuyển dụng',
      },
      {
        Name: 'Tình trạng liên hệ',
        FieldName: 'FollowStatus',
        Placeholder: 'Tình trạng liên hệ',
        Type: 'select',
        Data: this.ListFollowStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },

      {
        Name: 'Ngoại ngữ',
        FieldName: 'LanguagesId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Language,
        Columns: [{ Name: 'Name', Title: 'Tên' }],
        DisplayName: 'Name',
        ValueName: 'Name',
        Placeholder: 'Chọn Ngoại ngữ',
      },

      {
        Name: 'Đơn vị đào tạo',
        FieldName: 'NameTraining',
        Placeholder: 'Nhập tên đơn vị đào tạo',
        Type: 'text'
      },

    ]
  };

  applys: any[] = []
  startIndex: number = 1;

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,
    private modalService: NgbModal,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private applyService: ApplyService,
    private dateUtils: DateUtils) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý hồ sơ ứng viên";
    this.searchApplys();
  }

  searchApplys() {
    this.checkeds = false;
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

    this.applyService.searchApplys(this.searchModel).subscribe(
      data => {
        this.applys = data.ListResult;
        if (this.checkeds) {
          this.applys.forEach(element => {
            element.Checked = true;
          });
        }
        if(this.listCheck){
          this.listCheck.forEach(element => {
            this.applys.forEach(a => { 
              if(element.Id == a.Id){
                a.Checked = true;
              }  
            });
          });
        }
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

      Name: null,
      PhoneNumber: null,
      WorkTypeId: null,
      Email: null,
      ApplyDateTo: null,
      ApplyDateFrom: null,
      InterviewDateTo: null,
      InterviewDateFrom: null,
      ApplyDateToV: null,
      ApplyDateFromV: null,
      InterviewDateToV: null,
      InterviewDateFromV: null
    };
    this.searchApplys();
  }

  showCreateUpdate() {
    // this.router.navigate(['tuyen-dung/ung-tuyen/them-moi']);

    let activeModal = this.modalService.open(ApplyCheckCandidateComponent, { container: 'body', windowClass: 'apply-check-candidate-model', backdrop: 'static' })
    activeModal.result.then((result) => {

    }, (reason) => {
    });
  }

  showConfirmDelete(id: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Ứng tuyển này không?").then(
      data => {
        this.delete(id);
      },
      error => {

      }
    );
  }

  delete(id) {
    this.applyService.deleteApply({ Id: id }).subscribe(
      data => {
        this.searchApplys();
        this.messageService.showSuccess('Xóa Ứng tuyển thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  checkeds: boolean = false;
  listCheck: any[] = [];
  selectAllFunction() {
    if(this.checkeds){
    this.idCheck = '1';
    }
    this.applys.forEach(element => {
      element.Checked = this.checkeds;
      if (this.checkeds) {
        this.listCheck.push(element)
      }
    });

  }

  pushChecker(row: any) {
    if (row.Checked) {
      this.listCheck.push(row);
    } else {
      this.idCheck = '0';
      this.checkeds = false;
      this.listCheck.splice(this.listCheck.indexOf(row), 1);
    }
  }

  ApplyExport() {
    if (this.listCheck.length >0) {
      this.applyService.ApplyExport(this.listCheck, this.idCheck).subscribe(d => {
        if (d) {
          // this.modelTest.PathFileMaterial = d;
          // this.modelTest.ApiUrl = this.config.ServerApi;
          // this.modelTest.FileName = "PATK-C.";
          // this.fileProcess.downloadFileBlob(this.config.ServerApi+d, this.modelTest.FileName);
          var link = document.createElement('a');
          link.setAttribute("type", "hidden");
          link.href = this.config.ServerApi + d;
          link.download = 'Download.docx';
          document.body.appendChild(link);
          // link.focus();
          link.click();
          document.body.removeChild(link);
          this.listCheck = [];
        }
      }, e => {
        this.messageService.showError(e);
      });
    }else{
      this.messageService.showMessage("Chưa chọn hồ sơ ứng viên không thể xuất")
    }
  }


}
