import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReasonChangeIncomeService } from 'src/app/employee/service/reason-change-income.service';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ReasonChangeIncomeCreateComponent } from '../reason-change-income-create/reason-change-income-create.component';

@Component({
  selector: 'app-reason-change-income-manage',
  templateUrl: './reason-change-income-manage.component.html',
  styleUrls: ['./reason-change-income-manage.component.scss']
})
export class ReasonChangeIncomeManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private reasonChangeIncomeService: ReasonChangeIncomeService,
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

  reasonsChangeIncome: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý lý do thay đổi thu nhập";
    this.search();
  }

  search() {
    this.reasonChangeIncomeService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.reasonsChangeIncome = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteReason(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá lý do điều chỉnh thu nhập này không?").then(
      data => {
        this.deleteReason(Id);
      },
      error => {

      }
    );
  }

  deleteReason(Id: string) {
    this.reasonChangeIncomeService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa lý do điều chỉnh thu nhập thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ReasonChangeIncomeCreateComponent, { container: 'body', windowClass: 'reasonchangeincome-create-model', backdrop: 'static' })
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
