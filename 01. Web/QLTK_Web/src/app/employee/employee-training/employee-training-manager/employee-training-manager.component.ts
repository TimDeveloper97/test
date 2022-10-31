import { Component, OnInit } from '@angular/core';
import { EmployeeTrainingService } from '../../service/employee-training.service';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { WorkTypeCreateComponent } from '../../worktype/work-type-create/work-type-create.component';
import { EmployeeTrainingCreateComponent } from '../employee-training-create/employee-training-create.component';

@Component({
  selector: 'app-employee-training-manager',
  templateUrl: './employee-training-manager.component.html',
  styleUrls: ['./employee-training-manager.component.scss']
})
export class EmployeeTrainingManagerComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private service: EmployeeTrainingService,
    private titleservice: Title,
    public constant: Constants
  ) { }

  startIndex = 0;
  startus1 = 0;
  startus2 = 0;
  startus3 = 0;
  startus4 = 0;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Id: '',
    Name: '',
    Description: ''
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Chương trình đào tạo";
    this.searchEmployeeTraining();
  }
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên chương trình đào tạo',
    Items: [
    ]
  };
  searchEmployeeTraining() {
    this.service.searchEmployeeTraining(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        this.startus1 = data.Status1;
        this.startus2 = data.Status2;
        this.startus3 = data.Status3;
        this.startus4 = data.Status4;
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
      OrderBy: 'Name',
      OrderType: true,

      Id: '',
      Name: '',
      Description: ''
    }
    this.searchEmployeeTraining();
  }

  showCreateUpdate(Id: string) {// s
    let activeModal = this.modalService.open(EmployeeTrainingCreateComponent, { container: 'body', windowClass: 'employee-trainning-create', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
      }
      this.searchEmployeeTraining();
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chương trình đào tạo này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.service.deleteEmployeeTraining({ Id: Id }).subscribe(
      data => {
        this.searchEmployeeTraining();
        this.messageService.showSuccess('Xóa chương trình đào tạo thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
