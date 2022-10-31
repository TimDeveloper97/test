import { Component, OnInit } from '@angular/core';
import { Constants, MessageService, AppSetting } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { JobGroupService } from '../../../education/service/job-group.service';
import { JobgroupCreateComponent } from '../jobgroup-create/jobgroup-create.component';

@Component({
  selector: 'app-jobgroup-mange',
  templateUrl: './jobgroup-mange.component.html',
  styleUrls: ['./jobgroup-mange.component.scss']
})
export class JobgroupMangeComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private jobGroupServiec: JobGroupService,
    public constant: Constants,
  ) {this.pagination = Object.assign({}, appSetting.Pagination); }

  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  logUserId: string;

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Description: '',
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm nghề";
    this.searchJobGroups();
  }

  searchJobGroups() {
    this.jobGroupServiec.SearchJobGroup(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }


  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      Note: '',
    }
    this.searchJobGroups();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchJobGroups();
    }
  }

  showConfirmDeleteJobGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm nghề này không?").then(
      data => {
        this.deleteJobGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteJobGroup(Id: string) {
    this.jobGroupServiec.DeleteJobGroup({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        this.searchJobGroups();
        this.messageService.showSuccess('Xóa nhóm nghề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(JobgroupCreateComponent, { container: 'body', windowClass: 'jobgroup-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchJobGroups();
      }
    }, (reason) => {
    });
  }

}
