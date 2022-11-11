import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { MeetingService } from '../../service/meeting.service';
import { MeetingDetailComponent } from '../meeting-detail/meeting-detail.component';

@Component({
  selector: 'app-meeting-join-finish',
  templateUrl: './meeting-join-finish.component.html',
  styleUrls: ['./meeting-join-finish.component.scss']
})
export class MeetingJoinFinishComponent implements OnInit {

  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private meetingService: MeetingService,
    private modalService: NgbModal,
  ) {}

  height = 0;
  startIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    PageNumber: 1,
    UserId : JSON.parse(localStorage.getItem('qltkcurrentUser')).userid
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã hoặc tên meeting',
    Items: [
      {
        Name: 'Loại meeting',
        FieldName: 'Type',
        Placeholder: 'Loại meeting',
        Type: 'select',
        Data: this.constant.MeetingType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Chủng loại',
        FieldName: 'MeetingTypeId',
        Type: 'dropdowntree',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.MeetingType,
        ParentId: 'Exten',
        Columns: [{ Name: 'Code', Title: 'Mã chủng loại' }, { Name: 'Name', Title: 'Tên chủng loại' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn chủng loại meeting',
      },
    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Danh sách Meeting tham gia đã kết thúc";
    this.searchMeeting();
  }


  searchMeeting() {
    this.meetingService.searchMeetingFinish(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.totalItems = data.TotalItem;
        this.listData.forEach(item => {
          if(item.Type == 1)
          {
            item.TypeName = "Online";
          }
          else{
            item.TypeName = "Trực tiếp";
          }

          item.StartTime = item.StartTime.hour + ":" + (item.StartTime.minute < 10? '0':'') + item.StartTime.minute;
          item.EndTime = item.EndTime.hour + ":" + (item.EndTime.minute < 10? '0':'') + item.EndTime.minute;

          if(item.RealStartTime)
          {
            item.RealStartTime = item.RealStartTime.hour + ":" + (item.RealStartTime.minute < 10? '0':'') + item.RealStartTime.minute;
          }

          if(item.RealEndTime)
          {
            item.RealEndTime = item.RealEndTime.hour + ":" + (item.RealEndTime.minute < 10? '0':'') + item.RealEndTime.minute;
          }
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      PageNumber: 1,
    }
    this.searchMeeting();
  }

  showInfo(id:any)
  {
    let activeModal = this.modalService.open(MeetingDetailComponent, { container: 'body', windowClass: 'meeting-detail-model', backdrop: 'static' })
    activeModal.componentInstance.Id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchMeeting();
      }
    }, (reason) => {
    });
  }
}
