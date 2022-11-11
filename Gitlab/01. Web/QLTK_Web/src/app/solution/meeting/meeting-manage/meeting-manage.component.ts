import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';

import { AppSetting, Configuration, MessageService, Constants, DateUtils } from 'src/app/shared';
import { Router } from '@angular/router';
import { MeetingCreateComponent } from '../meeting-create/meeting-create.component';
import { MeetingService } from '../../service/meeting.service';
import { MeetingTypeService } from '../../service/meeting-type.service';
import { MeetingTypeCreateComponent } from '../meeting-type-create/meeting-type-create.component';

@Component({
  selector: 'app-meeting-manage',
  templateUrl: './meeting-manage.component.html',
  styleUrls: ['./meeting-manage.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class MeetingManageComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    public dateUtils: DateUtils,
    private meetingService: MeetingService,
    private meetingTypeService: MeetingTypeService
  ) {
    this.items = [
      { Id: 1, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }

  height = 0;
  startIndex = 0;
  items: any;
  listData: any[] = [];
  listMeetingType: any[] = [];
  listMeetingTypeId = [];
  selectedMeetingTypeId = '';
  MeetingTypeId: '';
  sbuid = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
  departermentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId
  model: any = {
    PageSize: 10,
    PageNumber: 1,
  }

  solutionGroupModel: any = {
    TotalItems: 0,
    Id: '',
    Name: '',
    Code: '',
  }

  modelAll: any = {
    TotalItems: 0,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  fileModel = {
    Id: '',
    SolutionId: '',
    FileName: '',
    FileSize: '',
    Path: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã hoặc tên meeting',
    Items: [
      {
        Name: 'Trạng thái',
        FieldName: 'Status',
        Placeholder: 'Trạng thái',
        Type: 'select',
        Data: this.constant.MeetingStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
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
    this.appSetting.PageTitle = "Meeting";
    this.searchMeetingType();
    this.searchMeeting('');
    this.selectedMeetingTypeId = localStorage.getItem("selectedMeetingTypeId");
    localStorage.removeItem("selectedMeetingTypeId");
  }

  itemClick(e) {
    if (this.MeetingTypeId == '' || this.MeetingTypeId == null) {
      this.messageService.showMessage("Đây không phải chủng loại cuộc họp!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateMeetingType(this.MeetingTypeId);
      }
      else if (e.itemData.Id == 2) {
        this.showConfirmDeleteMeetingType(this.MeetingTypeId);
      }
    }
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedMeetingTypeId) {
      this.selectedMeetingTypeId = e.selectedRowKeys[0];
      this.searchMeeting(e.selectedRowKeys[0]);
      this.MeetingTypeId = e.selectedRowKeys[0];
    }
  }

  searchMeetingType() {
    this.meetingTypeService.searchMeetingType().subscribe((data: any) => {
      if (data.ListResult) {
        this.listMeetingType = data.ListResult;
        this.solutionGroupModel.TotalItems = data.TotalItem;
        this.listMeetingType.unshift(this.modelAll);
        for (var item of this.listMeetingType) {
          this.listMeetingTypeId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchMeeting(meetingTypeId: string) {
    this.model.MeetingTypeId = meetingTypeId;
    this.meetingService.searchMeeting(this.model).subscribe((data: any) => {
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

          if(item.StartTime)
          {
            item.StartTime = item.StartTime.hour + ":" + (item.StartTime.minute < 10? '0':'') + item.StartTime.minute;
          }

          if(item.EndTime)
          {
            item.EndTime = item.EndTime.hour + ":" + (item.EndTime.minute < 10? '0':'') + item.EndTime.minute;
          }

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

  clear(meetingTypeId: string) {
    this.model = {
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Code: '',
      Name: '',
      SolutionGroupId: '',
      ProjectId: '',
      DateToV: this.dateUtils.getFiscalYearEnd(),
      DateFromV: this.dateUtils.getFiscalYearStart(),
      SBUId: this.sbuid,
      DepartermentId: this.departermentId,
      Status: null,
    }
    this.searchMeeting(meetingTypeId);
  }

  showConfirmDeleteMeetingType(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chủng loại cuộc họp này không?").then(
      data => {
        this.deleteMeetingType(Id);
      },
      error => {

      }
    );
  }

  deleteMeetingType(Id: string) {
    this.meetingTypeService.delete(Id).subscribe(
      data => {
        this.selectedMeetingTypeId = '';
        this.searchMeetingType();
        this.searchMeeting("");
        this.messageService.showSuccess('Xóa chủng loại meeting thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá meeting này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.meetingService.delete(Id).subscribe(
      data => {
        this.searchMeeting(this.model.MeetingTypeId);
        this.searchMeetingType();
        this.searchMeeting("");
        this.messageService.showSuccess('Xóa yêu meeting thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdateMeetingType(Id:string)
  {
    let activeModal = this.modalService.open(MeetingTypeCreateComponent, { container: 'body', windowClass: 'meeting-type-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
      this.searchMeeting('');
      }
      this.searchMeetingType();
    }, (reason) => {
    });
  }

  showCreateUpdate(Id: string) {
    this.router.navigate(['giai-phap/meeting/them-moi']).then(re=>{
      if(re){

      } this.searchMeetingType();
      this.searchMeeting('');
    });

  }

  showAll() {
    this.MeetingTypeId = null;
    this.searchMeeting(this.MeetingTypeId);
  }

}
