import { Component, OnInit } from '@angular/core';
import { Constants, MessageService, AppSetting } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { JobPositionServiceService } from '../../service/job-position-service.service';
import { JobPositionCreateComponent } from '../job-position-create/job-position-create.component';

@Component({
  selector: 'app-job-position-manage',
  templateUrl: './job-position-manage.component.html',
  styleUrls: ['./job-position-manage.component.scss']
})
export class JobPositionManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private jobPosition: JobPositionServiceService,
    public constant: Constants
  ) { }
  StartIndex = 0;
  listData: any[] = [];
  listManager: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Index',
    OrderType: true,

    Id: '',
    Name: '',
    Index: '',
    Description: '',
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý chức vụ";
    this.searchJobPostiton();
  }
  searchJobPostiton() {
    this.jobPosition.SearchJobPostitons(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên chức vụ',
    Items: [
    ]
  };

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Index',
      OrderType: true,

      Name: '',
    }
    this.searchJobPostiton();
  }

  showConfirmDeleteJobPostiton(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chức vụ này không?").then(
      data => {
        this.deleteJobPostiton(Id);
      },
      error => {

      }
    );
  }

  deleteJobPostiton(Id: string) {
    this.jobPosition.DeleteJobPositions({ Id: Id }).subscribe(
      data => {
        this.searchJobPostiton();
        this.messageService.showSuccess('Xóa chức vụ thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(JobPositionCreateComponent, { container: 'body', windowClass: 'job-position-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchJobPostiton();
      }
    }, (reason) => {
    });
  }

}
