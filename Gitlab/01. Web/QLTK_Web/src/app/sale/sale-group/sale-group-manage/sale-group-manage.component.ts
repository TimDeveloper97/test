import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { SaleGroupCreateComponent } from '../sale-group-create/sale-group-create.component';
import { SaleGroupService } from '../service/sale-group.service';

@Component({
  selector: 'app-sale-group-manage',
  templateUrl: './sale-group-manage.component.html',
  styleUrls: ['./sale-group-manage.component.scss']
})
export class SaleGroupManageComponent implements OnInit {


  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private saleGroupService: SaleGroupService,
    public config: Configuration
  ) { }

  listData = [];
  startIndex = 1;
  searchModel: any ={
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Name: '',
    Note: '',
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhóm',
    Items: [
      {
        Name: 'Ghi chú',
        FieldName: 'Note',
        Placeholder: 'Nhập nội dung ghi chú',
        Type: 'text'
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý nhóm kinh doanh";
    this.searchSaleGroup();
    // this.height = window.innerHeight - 140;
  }

  searchSaleGroup(){
    this.saleGroupService.searchSaleGroup(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.listData = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(id: string){
    let activeModal = this.modalService.open(SaleGroupCreateComponent, { container: 'body', windowClass: 'sale-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSaleGroup();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(id: string){
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm kinh doanh này không?").then(
      data => {
        this.delete(id);
      },
      error => {
        
      }
    );
  }

  delete(id: string){
    this.saleGroupService.deleteSaleGroup(id).subscribe(
      data => {
        this.searchSaleGroup();
        this.messageService.showSuccess('Xóa nhóm kinh doanh thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear(){
    this.searchModel={
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,
  
      Name: '',
      Note: '',
    }
    this.searchSaleGroup();
  }

}
