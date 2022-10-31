import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService, PermissionService } from 'src/app/shared';
import { ApplyService } from '../../services/apply.service';
import { CandidateService } from '../../services/candidate.service';
import { InterviewService } from '../../services/interview.service';
import { RecruitmentRequestService } from '../../services/recruitment-request.service';
import { RecruitmentRequestCreateComponent } from '../recruitment-request-create/recruitment-request-create.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-recruitment-request-manage',
  templateUrl: './recruitment-request-manage.component.html',
  styleUrls: ['./recruitment-request-manage.component.scss']
})
export class RecruitmentRequestManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private recruitmentRequestService:RecruitmentRequestService,
    private applyService: ApplyService,
    private interviewService: InterviewService,
    private candidateService: CandidateService,
    public config: Configuration,
    private router: Router,
    public permissionService: PermissionService
  ) { }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      // {
      //   Name: 'Tình trạng',
      //   FieldName: 'Status',
      //   Placeholder: 'Tình trạng ',
      //   Type: 'select',
      //   Data: this.constant.RecruitmentRequestStatus,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      {
        Name: 'Phòng ban đề xuất',
        FieldName: 'DepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn phòng ban',
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
        Name: 'Tình trạng',
        FieldName: 'StatusRecruit',
        Placeholder: 'Tình trạng ',
        Type: 'select',
        Data: this.constant.RecruitStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  interviews: any =[];
  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Code: '',
    DepartmentId: '',
    WorkingTypeId:'',
    Status: null,
    StatusRecruit : null
  };
  candidatesModel: any = {
    Id: '',
    ApplyId: '',
  }

  listRequest: any[] = [];
  applys: any[]=[];
  candidates: any[]=[];
  startIndex = 0;
  selectIndex = -1;
  selectIndexs = -1;

  @ViewChild('scrollApply', { static: false }) scrollApply: ElementRef;
  @ViewChild('scrollApplyHeader', { static: false }) scrollApplyHeader: ElementRef;
  @ViewChild('scrollCandidate', { static: false }) scrollCandidate: ElementRef;
  @ViewChild('scrollCandidateHeader', { static: false }) scrollCandidateHeader: ElementRef;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý yêu cầu tuyển dụng";
    this.search();
  }

  ngAfterViewInit() {
    this.scrollApply.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollApplyHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollCandidate.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollCandidateHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollApply.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollCandidate.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  search() {
    this.recruitmentRequestService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.listRequest = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  select(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.applys = [];
      this.searchApplysByRecruitmentRequestId(this.listRequest[index].Id);
      this.searchCandidatesByRecruitmentRequestId(this.listRequest[index].Id);
      this.searchInterviewByRecruitmentRequestId(this.listRequest[index].Id);
    }
    else {
      this.selectIndex = -1;
      this.applys = [];
      this.candidates = [];
      this.interviews = [];
    }
  }

  searchApplysByRecruitmentRequestId(id: any) {
    this.applyService.searchApplysByRecruitmentRequestId(id).subscribe((data: any) => {
      if (data.ListResult) {
        this.applys = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchCandidatesByRecruitmentRequestId(id: any) {
    this.candidateService.searchCandidatesByRecruitmentRequestId(id).subscribe((data: any) => {
      if (data.ListResult) {
        this.candidates = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchInterviewByRecruitmentRequestId(id: any) {
    this.interviewService.getInterviewByRecruitmentRequestId(id).subscribe((data: any) => {
      if (data) {
        this.interviews = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá yêu cầu tuyển dụng này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.recruitmentRequestService.delete(Id).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa yêu cầu tuyển dụng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(RecruitmentRequestCreateComponent, { container: 'body', windowClass: 'recruitment-request-create-modal', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
  
      Code: '',
      Status: null
    };
    this.search();
    this.applys = [];
    this.candidates = [];
  }

  SolutionByDevice: any = [];
  // selectror(index) {
  //   if (this.selectIndex != index) {
  //     this.selectIndex = index;
  //     this.SolutionByDevice = [];
  //     var DeviceByPrice = this.SolutionModel.ListProductNeedSolution;
  //     var Id = DeviceByPrice[index].Id;
  //     var model = {
  //       Id: Id,
  //       Content: this.requirementModel.Id,
  //     }
  //     this.SolutionModel.ProductNeedSolutionId = Id;
  //     this.customerRequirementService.getCustomerRequirementProductSolutionById(model).subscribe(
  //       data => {
  //         this.SolutionByDevice = data;
  //       },
  //       error => {
  //         this.messageService.showError(error);
  //       }
  //     );
  //   }
  //   else {
  //     this.selectIndex = -1;
  //     this.SolutionByDevice = [];
  //   }
  // }

  selectError(index) {
    if (this.selectIndexs != index) {
      this.selectIndexs = index;
      this.SolutionByDevice = [];
      var DeviceByPrice = this.candidates;
      var ApplyId = DeviceByPrice[index].ApplyId;
      this.candidatesModel.ApplyId = ApplyId;
      this.showCreateUpdateCandidateProfile(ApplyId)
    }
    else {
      this.selectIndex = -1;
      this.SolutionByDevice = [];
    }
  }

  showCreateUpdateCandidateProfile(ApplyId : string) {
    // let activeModal = this.modalService.open(CandidateCreateComponent, { container: 'body', windowClass: 'candidate-create-model', backdrop: 'static' })
    // activeModal.componentInstance.id = Id;
    // activeModal.result.then((result) => {
    //   if (result == true) {
    //     // this.getById(3);
    //   }
    // }, (reason) => {
    // });
    
    this.router.navigate(['tuyen-dung/ho-so-ung-vien/chinh-sua/'+ApplyId]);
  }
}
