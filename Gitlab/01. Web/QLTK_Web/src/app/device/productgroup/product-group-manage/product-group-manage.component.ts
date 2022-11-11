import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Configuration, MessageService, AppSetting } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ProductGroupCreateComponent } from '../product-group-create/product-group-create.component';
import { ProductGroupService } from '../../services/product-group.service';
import { DxTreeListComponent } from 'devextreme-angular';

@Component({
  selector: 'app-product-group-manage',
  templateUrl: './product-group-manage.component.html',
  styleUrls: ['./product-group-manage.component.scss']
})
export class ProductGroupManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private appSetting: AppSetting,
    private serviceProductGroup: ProductGroupService
  ) {}
  
  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  listTPA: any[] = [];
  logUserId: string;
  productGroupId: '';
  selectedProductGroupId = '';
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
    ParentId: '',
    Description: '',
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm thiết bị";
    this.searchProductGroup();
  }
  ListProductGroupId = [];
  searchProductGroup() {
    this.serviceProductGroup.searchProductGroup(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
        if (this.selectedProductGroupId == null) {
          this.selectedProductGroupId = this.listDA[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedProductGroupId];
        for (var item of this.listDA) {
          this.ListProductGroupId.push(item.Id);
        }
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
      ParentId: '',
      Description: '',
    }
    this.searchProductGroup();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchProductGroup();
    }
  }

  //xoá nhóm module
  listDAById: any[] = [];
  searchProductGroupById(Id: string) {
    this.serviceProductGroup.searchProductGroupById(this.model).subscribe((data: any) => {
      if (data) {
        //this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDAById = data;
        //this.model.totalItems = data.TotalItem;
        if (this.listDAById.length == 1) {
          this.deleteProductGroup(Id);
        } else {
          this.messageService.showConfirm("Xóa nhóm thiết bị này sẽ xóa hết cả các nhóm thiết bị con thuộc nhóm. Bạn có chắc chắn muốn xóa không?").then(
            data => {
              this.deleteProductGroup(Id);
            }
          );
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });

  }

  showConfirmDeleteProductGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa nhóm thiết bị này không?").then(
      data => {
        this.model.Id = Id;
        this.searchProductGroupById(Id);
      }
    );
  }

  deleteProductGroup(Id: string) {
    this.serviceProductGroup.deleteProductGroup({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        //this.check=true;
        this.model.Id = '';
        this.searchProductGroup();
        this.messageService.showSuccess('Xóa nhóm thiết bị thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    this.selectedProductGroupId = e.selectedRowKeys[0];
    this.searchProductGroup();
    this.productGroupId = e.selectedRowKeys[0];
  }

  // popup thêm mới và chỉnh sửa
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ProductGroupCreateComponent, { container: 'body', windowClass: 'productgroupcreate-model', backdrop: 'static' })
    activeModal.componentInstance.idUpdate = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProductGroup();
      }
    }, (reason) => {
    });
  }

}
