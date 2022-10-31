import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { WorktypeInterviewService } from '../../service/worktype-interview.service';
import { WorktypeInterviewCreateComponent } from '../worktype-interview-create/worktype-interview-create.component';

@Component({
  selector: 'app-worktype-interview-manage',
  templateUrl: './worktype-interview-manage.component.html',
  styleUrls: ['./worktype-interview-manage.component.scss']
})
export class WorktypeInterviewManageComponent implements OnInit {

  @Input() workTypeId: string;

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private workTypeInterviewService: WorktypeInterviewService,
    public constant: Constants,
 ) { }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
    ]
  };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Name: '',
    SBUId: '',
    DepartmentId: '',
    WorkTypeId: ''
  };

  workTypeInterviews: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.search();
  }

  search() {
    this.searchModel.WorkTypeId = this.workTypeId;
    this.workTypeInterviewService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.workTypeInterviews = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteWorkTypeInterview(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá lần phỏng vấn này không?").then(
      data => {
        this.deleteWorkTypeInterview(Id);
      },
      error => {

      }
    );
  }

  deleteWorkTypeInterview(Id: string) {
    this.workTypeInterviewService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa lần phỏng vấn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(WorktypeInterviewCreateComponent, { container: 'body', windowClass: 'worktypeinterview-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.componentInstance.workTypeId = this.workTypeId;
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
      OrderBy: 'Name',
      OrderType: true,

      Name: '',
      Code: '',
    };
    this.search();
  }



}
