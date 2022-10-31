import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { WorkLocationService } from 'src/app/employee/service/work-location.service';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { WorkLocationCreateComponent } from '../work-location-create/work-location-create.component';

@Component({
  selector: 'app-work-location-manage',
  templateUrl: './work-location-manage.component.html',
  styleUrls: ['./work-location-manage.component.scss']
})
export class WorkLocationManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private workLocationService: WorkLocationService,
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

  workLocations: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý nơi làm việc";
    this.search();
  }

  search() {
    this.workLocationService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.workLocations = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nơi làm việc này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.workLocationService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa nơi làm việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(WorkLocationCreateComponent, { container: 'body', windowClass: 'worklocation-create-model', backdrop: 'static' })
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
