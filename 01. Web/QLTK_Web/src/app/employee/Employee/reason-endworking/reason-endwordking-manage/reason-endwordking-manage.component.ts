import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReasonEndworkingService } from 'src/app/employee/service/reason-endworking.service';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ReasonEndworkingCreateComponent } from '../reason-endworking-create/reason-endworking-create.component';

@Component({
  selector: 'app-reason-endwordking-manage',
  templateUrl: './reason-endwordking-manage.component.html',
  styleUrls: ['./reason-endwordking-manage.component.scss']
})
export class ReasonEndwordkingManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private reaonEndWorkingService: ReasonEndworkingService,
    public constant: Constants
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
  };

  reasonsEndWorking: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý lý do nghỉ việc";
    this.search();
  }

  search() {
    this.reaonEndWorkingService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.reasonsEndWorking = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteReason(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá lý do nghỉ việc này không?").then(
      data => {
        this.deleteReason(Id);
      },
      error => {

      }
    );
  }

  deleteReason(Id: string) {
    this.reaonEndWorkingService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa lý do nghỉ việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ReasonEndworkingCreateComponent, { container: 'body', windowClass: 'reason-create-model', backdrop: 'static' })
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
      OrderBy: 'Name',
      OrderType: true,

      Name: '',
    };
    this.search();
  }

}
