import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants } from 'src/app/shared';

import { TestcriteriagroupService } from '../../services/testcriteriagroup-service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { TestCriteriaGroupCreateComponent } from '../test-criteria-group-create/test-criteria-group-create.component';

@Component({
  selector: 'app-test-criteria-group-manage',
  templateUrl: './test-criteria-group-manage.component.html',
  styleUrls: ['./test-criteria-group-manage.component.scss']
})
export class TestCriteriaGroupManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private testCriteriaGroupService: TestcriteriagroupService) { this.pagination = Object.assign({}, appSetting.Pagination); }


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
    Note: '',
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm tiêu chí kiểm tra";
    this.searchCriteriaGroup();
  }

  searchCriteriaGroup() {
    this.testCriteriaGroupService.searchTestCriterialGroup(this.model).subscribe((data: any) => {
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
    this.searchCriteriaGroup();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchCriteriaGroup();
    }
  }

  showConfirmDeleteTestCriterGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm tiêu chí này không?").then(
      data => {
        this.deleteTestCriterGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteTestCriterGroup(Id: string) {
    this.testCriteriaGroupService.deleteTestCriteralGroup({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        this.searchCriteriaGroup();
        this.messageService.showSuccess('Xóa nhóm tiêu chí thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(TestCriteriaGroupCreateComponent, { container: 'body', windowClass: 'test-criteria-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchCriteriaGroup();
      }
    }, (reason) => {
    });
  }

}
