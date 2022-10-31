import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { ProductStandardGroupCreateComponent } from '../product-standard-group-create/product-standard-group-create.component';
import { ProductStandardGroupService } from '../../services/product-standard-group.service';

@Component({
  selector: 'app-product-standard-group-manage',
  templateUrl: './product-standard-group-manage.component.html',
  styleUrls: ['./product-standard-group-manage.component.scss']
})
export class ProductStandardGroupManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private ProductStandardGroupService: ProductStandardGroupService,
    public constant: Constants
  ) { }

  StartIndex = 0;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm tiêu chuẩn sản phẩm";
    this.searchProductStandardGroup();
  }

  searchProductStandardGroup() {
    this.ProductStandardGroupService.searchProductStandardGroup(this.model).subscribe((data: any) => {
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

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
    }
    this.searchProductStandardGroup();
  }

  showConfirmDeleteProductStandardGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm tiêu chuẩn này không?").then(
      data => {
        this.deleteProductStandardGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteProductStandardGroup(Id: string) {
    this.ProductStandardGroupService.deleteProductStandardGroup({ Id: Id }).subscribe(
      data => {
        this.searchProductStandardGroup();
        this.messageService.showSuccess('Xóa nhóm tiêu chuẩn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ProductStandardGroupCreateComponent, { container: 'body', windowClass: 'product-standard-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProductStandardGroup();
      }
    }, (reason) => {
    });
  }

}
