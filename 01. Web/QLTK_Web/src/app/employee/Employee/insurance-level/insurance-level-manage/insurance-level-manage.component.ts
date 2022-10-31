import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { InsuranceLevelService } from 'src/app/employee/service/insurance-level.service';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { InsuranceLevelCreateComponent } from '../insurance-level-create/insurance-level-create.component';

@Component({
  selector: 'app-insurance-level-manage',
  templateUrl: './insurance-level-manage.component.html',
  styleUrls: ['./insurance-level-manage.component.scss']
})
export class InsuranceLevelManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private insuranceService: InsuranceLevelService,
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

  insuranceLevels: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý mức đóng bảo hiểm";
    this.search();
  }

  search() {
    this.insuranceService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.insuranceLevels = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteReason(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá mức đóng bảo hiểm này không?").then(
      data => {
        this.deleteReason(Id);
      },
      error => {

      }
    );
  }

  deleteReason(Id: string) {
    this.insuranceService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa mức đóng bảo hiểm thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(InsuranceLevelCreateComponent, { container: 'body', windowClass: 'insurancelevel-create-model', backdrop: 'static' })
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
