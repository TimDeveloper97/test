import { Component, OnInit } from '@angular/core';
import { ApplicationService } from '../../application/application.service';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApplicationCreateComponent } from '../application-create/application-create.component';

@Component({
  selector: 'app-application-manage',
  templateUrl: './application-manage.component.html',
  styleUrls: ['./application-manage.component.scss']
})
export class ApplicationManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private applicationService: ApplicationService,
    public constant: Constants
  ) { }

  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Index',
    OrderType: 'true',
    Name: '',
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        // Name: 'Tên ứng dụng',
        // FieldName: 'Name',
        // Placeholder: 'Nhập tên ứng dụng',
        // Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý ứng dụng";
    this.searchApplication();
  }
  searchApplication() {
    this.applicationService.searchApplication(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        //this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
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
      OrderBy: 'Index',
      OrderType: 'true',
      Name: '',
    }
    this.searchApplication();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ApplicationCreateComponent, { container: 'body', windowClass: 'application-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchApplication();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteApplication(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá ứng dụng này không?").then(
      data => {
        this.deleteApplication(Id);
      },
      error => {
        
      }
    );
  }

  deleteApplication(Id: string) {
    this.applicationService.deleteApplication(Id).subscribe(
      data => {
        this.searchApplication();
        this.messageService.showSuccess('Xóa ứng dụng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
