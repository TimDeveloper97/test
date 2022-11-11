import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { WorkPlaceService } from '../service/work-place.service';
import { WorkPlaceCreateComponent } from '../work-place-create/work-place-create.component';

@Component({
  selector: 'app-work-place-manage',
  templateUrl: './work-place-manage.component.html',
  styleUrls: ['./work-place-manage.component.scss']
})
export class WorkPlaceManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private workPlaceService: WorkPlaceService,
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
    Code: ''
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm mã đơn vị công tác...',
    Items: [
      {
        Name: 'Tên đơn vị công tác',
        FieldName: 'Name',
        Placeholder: 'Nhập tên đơn vị công tác',
        Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý Đơn vị công tác";
    this.searchWorkPlace();
  }

  searchWorkPlace() {
    this.workPlaceService.searchWorkPlace(this.model).subscribe((data: any) => {
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
      Description: '',
    }
    this.searchWorkPlace();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(WorkPlaceCreateComponent, { container: 'body', windowClass: 'WorkPlace-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchWorkPlace();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteWorkPlace(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá đơn vị công tác này không?").then(
      data => {
        this.deleteWorkPlace(Id);
      },
      error => {
        
      }
    );
  }

  deleteWorkPlace(Id: string) {
    this.workPlaceService.deleteWorkPlace({ Id: Id }).subscribe(
      data => {
        this.searchWorkPlace();
        this.messageService.showSuccess('Xóa đơn vị công tác thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }
}
