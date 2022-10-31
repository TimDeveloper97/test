import { Component, OnInit, ViewChild } from '@angular/core';

import { AppSetting, MessageService, Configuration, Constants, ComboboxService } from 'src/app/shared';

import { ClassRoomCreateComponent } from '../class-room-create/class-room-create.component';
import { ClassRoomService } from '../../service/class-room.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RoomtypeCreateComponent } from '../../roomtype/room-type-create/roomtype-create.component';
import { RoomtypeService } from '../../roomtype/services/roomtype.service';
import { DxTreeListComponent } from 'devextreme-angular';
import { Router } from '@angular/router';



@Component({
  selector: 'app-class-room-manage',
  templateUrl: './class-room-manage.component.html',
  styleUrls: ['./class-room-manage.component.scss']
})
export class ClassRoomManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private modalService: NgbModal,
    private classRoomService: ClassRoomService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    private roomtypeService: RoomtypeService,
    public router: Router,
  ) {
    this.items = [
      //{ Id: 1, text: 'Thêm mới loại phòng học', icon: 'fas fa-plus' },
      { Id: 2, text: 'Chỉnh sửa loại phòng học', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }
  items: any;
  ListRoomType: any[] = [];
  StartIndex = 0;
  listData: any[] = [];
  Id: string;
  selectedRoomTypeId = '';

  ListRoomTypeId = [];
  RoomTypeId: '; ';

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    RoomTypeId: '',
    SkillName: '',
    Address: '',
    Description: '',
    ListRoomType: [],
    ListMaterial: [],
    ListSkill: [],
    SkillId: ''
  }

  modelAll: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  modelRoomType: any = {
    TotalItems: 0,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Description: ''
  }
  
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo mã hoặc tên phòng học',
    Items: [
      {
        Name: 'Địa chỉ phòng',
        FieldName: 'Address',
        Placeholder: 'Địa chỉ phòng',
        Type: 'text'
      },
    ]
  };
  height = 0;
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý Phòng học";
    this.height = window.innerHeight - 140;
    //this.getRoomType();
    this.searchRoomType();
    this.searchClassRoom("");
    this.getSkill();
    this.selectedRoomTypeId = localStorage.getItem("selectedRoomTypeId");
    localStorage.removeItem("selectedRoomTypeId");
  }
  searchClassRoom(RoomTypeId: string) {
    this.model.RoomTypeId = RoomTypeId;
    this.classRoomService.searchClassRoom(this.model).subscribe((data: any) => {
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
      RoomTypeId: '',
      SkillName: '',
      Address: '',
      Description: '',
      ListRoomType: [],
      ListMaterial: [],
      ListSkill: [],
    }
    this.searchClassRoom("");
  }
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ClassRoomCreateComponent, { container: 'body', windowClass: 'ClassRoom-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    if(this.selectedRoomTypeId){
      activeModal.componentInstance.RoomTypeId = this.selectedRoomTypeId;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchClassRoom(this.selectedRoomTypeId);
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteClassRoom(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá phòng học này không?").then(
      data => {
        this.deleteClassRoom(Id);
      },
      error => {
        
      }
    );
  }

  deleteClassRoom(Id: string) {
    this.classRoomService.deleteClassRoom({ Id: Id }).subscribe(
      data => {
        this.searchClassRoom(this.selectedRoomTypeId);
        this.messageService.showSuccess('Xóa phòng học thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ExportExcel() {
    this.classRoomService.ExportExcel(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }
  updatePriceClass() {
    this.classRoomService.updatePriceClass().subscribe(d => {
      this.searchClassRoom("");
      this.messageService.showSuccess('Cập nhật giá thành công!');
    }, e => {
      this.messageService.showError(e);
    });
  }

  getRoomType() {
    this.comboboxService.getListRoomType().subscribe(
      data => {
        this.ListRoomType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchRoomType() {
    this.classRoomService.searchRoomType(this.modelRoomType).subscribe((data: any) => {
      if (data.ListResult) {
        //this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.ListRoomType = data.ListResult;
        this.ListRoomType.unshift(this.modelAll); // chèn vào ptu đầu tiên của mảng
        this.modelRoomType.TotalItems = data.TotalItem;
        if (this.selectedRoomTypeId == null) {
          this.selectedRoomTypeId = this.ListRoomType[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedRoomTypeId];
        for (var item of this.ListRoomType) {
          this.ListRoomTypeId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdateRoomType(Id: String) {
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

  clearModelRoomType() {
    this.modelRoomType = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
    }
    this.searchRoomType();
  }

  onSelectionChanged(e) {
    // this.selectedRoomTypeId = e.selectedRowKeys[0];
    // this.RoomTypeId = e.selectedRowKeys[0];
    // this.searchClassRoom(e.selectedRowKeys[0]);

    if(e.selectedRowKeys[0] != null  && e.selectedRowKeys[0] != this.selectedRoomTypeId)
    {
      this.selectedRoomTypeId = e.selectedRowKeys[0];
      this.searchClassRoom(e.selectedRowKeys[0]);
      this.RoomTypeId = e.selectedRowKeys[0];
    }
  }

  ListSkill: any = [];
  getSkill() {
    this.comboboxService.getListSkill().subscribe(
      data => {
        this.ListSkill = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  typeId: number;
  /// Skien click chuột phải
  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.ShowCreateUpdateRoomType('');
    } else if (e.itemData.Id == 2) {
      this.ShowCreateUpdateRoomType(this.RoomTypeId);
    } else if (e.itemData.Id == 3) {
      this.showConfirmDeleteRoomType(this.RoomTypeId);
    }
  }

}
