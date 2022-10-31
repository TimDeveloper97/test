import { Component, OnInit } from '@angular/core';
import { GroupUserService } from '../../service/group-user.service';
import { Constants, AppSetting, MessageService } from 'src/app/shared';
import { GroupUserCreateComponent } from '../group-user-create/group-user-create.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-group-user-manage',
  templateUrl: './group-user-manage.component.html',
  styleUrls: ['./group-user-manage.component.scss']
})
export class GroupUserManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private service: GroupUserService,
    public constant: Constants
  ) { }

  startIndex = 0;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Id: '',
    Name: '',
    SBUId: '',
    DepartmentId: '',
    IsDisable: '',
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhóm',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        Permission: ['F080905'],
        RelationIndexTo: 1
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        Permission: ['F080905'],
        RelationIndexFrom: 0
      },
      {
        Name: 'Trạng thái',
        FieldName: 'IsDisable',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.GroupUserStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý nhóm quyền";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.model.SBUId = currentUser.sbuId;
      this.model.DepartmentId = currentUser.departmentId;
    }
    this.searchGroupUser();
  }

  searchGroupUser() {
    this.service.searchGroupUser(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
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
      DepartmentId: '',
      IsDisable: '',
    }
    this.searchGroupUser();
  }

  showConfirmDeleteGroupUser(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm quyền này không?").then(
      data => {
        this.deleteGroupUser(Id);
      },
      error => {
        
      }
    );
  }

  deleteGroupUser(Id: string) {
    this.service.deleteGroupUser({ Id: Id }).subscribe(
      data => {
        this.searchGroupUser();
        this.messageService.showSuccess('Xóa nhóm quyền thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(GroupUserCreateComponent, { container: 'body', windowClass: 'group-user-create', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchGroupUser();
      }
    }, (reason) => {
    });
  }

}
