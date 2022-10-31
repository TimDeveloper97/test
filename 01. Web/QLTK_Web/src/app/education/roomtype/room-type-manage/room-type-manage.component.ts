import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { RoomtypeService } from '../services/roomtype.service';
import { RoomtypeCreateComponent } from '../room-type-create/roomtype-create.component';

@Component({
  selector: 'app-room-type-manage',
  templateUrl: './room-type-manage.component.html',
  styleUrls: ['./room-type-manage.component.scss']
})

export class RoomTypeManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    //title
    private titleservice: Title,
    private roomtypeService: RoomtypeService,
    public constant: Constants
  ) { }

  StartIndex = 0;
  listData: any[] = [];
  model: any = {//searchModel
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Description: ''
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Loại phòng học";
    this.searchRoomType();
  }

  searchRoomType() {
    this.roomtypeService.searchRoomType(this.model).subscribe((data: any) => {
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
    this.searchRoomType();
  }

  showCreateUpdate(Id: string) {// s
    let activeModal = this.modalService.open(RoomtypeCreateComponent, { container: 'body', windowClass: 'roomtype-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchRoomType();
      }
    }, (reason) => {
    });
  }
  
  showConfirmDeleteRoomType(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá loại phòng học này không?").then(
      data => {
        this.deleteRoomType(Id);
      },
      error => {
        
      }
    );
  }

  deleteRoomType(Id: string) {
    this.roomtypeService.deleteRoomType({ Id: Id }).subscribe(
      data => {
        this.searchRoomType();
        this.messageService.showSuccess('Xóa loại phòng học thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }
}
