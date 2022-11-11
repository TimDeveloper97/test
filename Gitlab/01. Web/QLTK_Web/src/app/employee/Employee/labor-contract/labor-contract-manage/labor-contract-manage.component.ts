import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LaborContractService } from 'src/app/employee/service/labor-contract.service';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { LaborContractCreateComponent } from '../labor-contract-create/labor-contract-create.component';

@Component({
  selector: 'app-labor-contract-manage',
  templateUrl: './labor-contract-manage.component.html',
  styleUrls: ['./labor-contract-manage.component.scss']
})
export class LaborContractManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private laborContractService: LaborContractService,
    public constant: Constants) { }


  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Loại',
        FieldName: 'Type',
        Placeholder: 'Loại',
        Type: 'select',
        Data: this.constant.LaborContract_Type,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Name: '',
    Type: null,
  };

  laborContracts: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý loại hợp đồng lao động";
    this.search();
  }

  search() {
    this.laborContractService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.laborContracts = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteReason(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá loại hợp đồng lao động này không?").then(
      data => {
        this.deleteReason(Id);
      },
      error => {

      }
    );
  }

  deleteReason(Id: string) {
    this.laborContractService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa loại hợp đồng lao động thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(LaborContractCreateComponent, { container: 'body', windowClass: 'laborcontract-create-model', backdrop: 'static' })
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
      Type: null,
    };
    this.search();
  }

}
