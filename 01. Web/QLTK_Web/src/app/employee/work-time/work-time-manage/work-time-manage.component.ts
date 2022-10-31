import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { WorkTimeCreateComponent } from '../work-time-create/work-time-create.component';
import { WorkTimeService } from '../../service/work-time.service';

@Component({
  selector: 'app-work-time-manage',
  templateUrl: './work-time-manage.component.html',
  styleUrls: ['./work-time-manage.component.scss']
})
export class WorkTimeManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    public workTimeService: WorkTimeService,
  ) { }

  StartIndex = 0;
  listData: any[] = [];

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên thời gian làm việc',
    Items: [
    ]
  };
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Id: '',
    Name: '',
    StartTime: '',
    EndTime: '',
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Thời gian làm việc";
    this.searchWorkTime();
  }

  searchWorkTime() {
    this.workTimeService.searchWorkTime(this.model).subscribe((data: any) => {
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
      OrderBy: 'Name',
      OrderType: true,

      Id: '',
      Name: '',
      StartTime: '',
      EndTime: '',

    }
    this.searchWorkTime();
  }

  showCreateUpdate(Id: string) {// s
    let activeModal = this.modalService.open(WorkTimeCreateComponent, { container: 'body', windowClass: 'WorkSkill-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchWorkTime();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thời gian làm việc này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.workTimeService.deleteWorkTime({ Id: Id }).subscribe(
      data => {
        this.searchWorkTime();
        this.messageService.showSuccess('Xóa thời gian làm việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
