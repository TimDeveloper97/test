import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReasonChangeInsuranceService } from 'src/app/employee/service/reason-change-insurance.service';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ReasonChangeInsuranceCreateComponent } from '../reason-change-insurance-create/reason-change-insurance-create.component';

@Component({
  selector: 'app-reason-change-insurance-manage',
  templateUrl: './reason-change-insurance-manage.component.html',
  styleUrls: ['./reason-change-insurance-manage.component.scss']
})
export class ReasonChangeInsuranceManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private reasonChangeInsuranceService: ReasonChangeInsuranceService,
    public constant: Constants) { }

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

  reasonsChangeInsurance: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý lý do điều chỉnh mức đóng BHXH";
    this.search();
  }

  search() {
    this.reasonChangeInsuranceService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.reasonsChangeInsurance = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteReason(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá lý do điều chỉnh mức đóng BHXH này không?").then(
      data => {
        this.deleteReason(Id);
      },
      error => {

      }
    );
  }

  deleteReason(Id: string) {
    this.reasonChangeInsuranceService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa lý do điều chỉnh mức đóng BHXH thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ReasonChangeInsuranceCreateComponent, { container: 'body', windowClass: 'reasonchangeinsurance-create-model', backdrop: 'static' })
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
