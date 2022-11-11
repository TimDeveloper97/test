import { Component, OnInit } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { AppSetting, MessageService } from 'src/app/shared';
import { ProductStandardTpaTypeService } from '../../services/product-standard-tpa-type.service';

import { ProductStandardTPATypeCreateComponent } from '../product-standard-tpa-type-create/product-standard-tpa-type-create.component';

@Component({
  selector: 'app-product-standard-tpa-type-manager',
  templateUrl: './product-standard-tpa-type-manager.component.html',
  styleUrls: ['./product-standard-tpa-type-manager.component.scss']
})
export class ProductStandardTPATypeManagerComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private productStandardTpaTypeService: ProductStandardTpaTypeService
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
    Placeholder: 'Tên chủng loại',
    Items: [
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Chủng loại hàng hóa";
    this.searchType();
  }

  searchType() {
    this.productStandardTpaTypeService.searchType(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        // this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
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
    this.searchType();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchType();
    }
  }

  showConfirmType(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chủng loại này không?").then(
      data => {
        this.deleteType(row);
      },
      error => {
        
      }
    );
  }

  deleteType(row) {
    this.productStandardTpaTypeService.deleteType({ Id: row.Id, Index: row.Index, LogUserId: this.logUserId, ParentId: row.ParentId }).subscribe(
      data => {
        this.searchType();
        this.messageService.showSuccess('Xóa chủng loại thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ProductStandardTPATypeCreateComponent, { container: 'body', windowClass: 'product-standard-tpa-type-create-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchType();
      }
    }, (reason) => {
    });
  }

}
