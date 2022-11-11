import { Component, OnInit } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { AppSetting, MessageService } from 'src/app/shared';
import { UnitService } from '../../services/unit-service';
import { UnitCreateComponent } from '../unit-create/unit-create.component';

@Component({
  selector: 'app-unit-manager',
  templateUrl: './unit-manager.component.html',
  styleUrls: ['./unit-manager.component.scss']
})
export class UnitManagerComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private unitService: UnitService,
  ) { this.pagination = Object.assign({}, appSetting.Pagination); }

  startIndex = 0;
  pagination: any;
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
    OrderBy: 'Index',
    OrderType: true,

    Id: '',
    Name: '',
    Index: '',
    Description: '',
  }
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tên đơn vị tính',
    Items: [
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Đơn vị tính";
    this.searchUnit();
  }

  searchUnit() {
    this.unitService.searchUnit(this.model).subscribe((data: any) => {
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
      OrderBy: 'Index',
      OrderType: true,

      Id: '',
      Name: '',
      Index: '',
      Description: '',
    }
    this.searchUnit();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchUnit();
    }
  }

  showConfirmUnit(Id: string, Index: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá đơn vị tính này không?").then(
      data => {
        this.deleteUnit(Id, Index);
      },
      error => {
        
      }
    );
  }

  deleteUnit(Id: string, Index: string) {
    this.unitService.deleteUnit({ Id: Id, Index: Index, LogUserId: this.logUserId }).subscribe(
      data => {
        this.searchUnit();
        this.messageService.showSuccess('Xóa đơn vị tính thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(UnitCreateComponent, { container: 'body', windowClass: 'unit-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchUnit();
      }
    }, (reason) => {
    });
  }

}
