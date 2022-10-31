import { Component, OnInit, ViewChild } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { WorkSkillCreateComponent } from '../work-skill-create/work-skill-create.component';
import { WorkSkillService } from '../../service/work-skill.service';
import { WorkSkillGroupCreateComponent } from '../work-skill-group-create/work-skill-group-create.component';
import { DxTreeListComponent } from 'devextreme-angular';

@Component({
  selector: 'app-work-skill-manage',
  templateUrl: './work-skill-manage.component.html',
  styleUrls: ['./work-skill-manage.component.scss']
})
export class WorkSkillManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private workSkillService: WorkSkillService,
    private titleservice: Title,
    public constant: Constants
  ) {
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }
  StartIndex = 0;
  listData: any[] = [];

  ListWorkSkillGroup: any = [];
  ListWordSkillGroupId: any = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Id: '',
    Name: '',
    Description: '',
    WorkSkillGroupId: ''
  }

  WorkSkillGroupSearchModel: any = {
    TotalItems: '',
    Code: '',
    Name: '',
  }
  items: any;
  WorkSkillGroupIdSelected: string;
  height = 0;
  AllWorkSkillGroup: any = {
    Id: '',
    Name: 'Tất cả',
    Code: '',
    ParentId: null,
  }
  workSkillGroupId: '';

  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Quản lý kỹ năng/kiến thức nhân viên";
    this.searchWorkSkill(this.workSkillGroupId);
    this.searchWorkSkillGroup();
  }

  searchWorkSkillGroup() {
    this.workSkillService.searchWorkSkillGroup(this.WorkSkillGroupSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListWorkSkillGroup = data.ListResult;
        this.ListWorkSkillGroup.unshift(this.model);
        this.WorkSkillGroupSearchModel.TotalItems = data.TotalItem;
        // this.ListWorkSkillGroup.forEach(element => {
        //   this.ListWordSkillGroupId.push(element.Id);
        // });

        if (this.workSkillGroupId == null || this.workSkillGroupId == "") {
          this.workSkillGroupId = this.ListWorkSkillGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.workSkillGroupId];
        for (var item of this.ListWorkSkillGroup) {
          this.ListWordSkillGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdateWordKillGroup(id, isUpdate) {
    let activeModal = this.modalService.open(WorkSkillGroupCreateComponent, { container: 'body', windowClass: 'workskillgroup-modal', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.Id = id;
    } else {
      activeModal.componentInstance.ParentId = id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchWorkSkillGroup();
      }
    }, (reason) => {
    });
  }
  showConfirmDeleteWorkSkillGroup(Id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm kỹ năng/kiến thức này không?").then(
      data => {
        this.deleteWorkSkillGroup(Id);
      },
      error => {
        
      }
    );
  }
  deleteWorkSkillGroup(Id: string) {
    this.workSkillService.deleteWorkSkillGroup({ Id: Id }).subscribe(
      data => {
        this.searchWorkSkillGroup();
        this.messageService.showSuccess('Xóa nhóm kỹ năng/kiến thức thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  itemClick(e) {
    if (this.workSkillGroupId == '' || this.workSkillGroupId == null) {
      this.messageService.showMessage("Bạn chưa chọn nhóm kỹ năng/kiến thức!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateWordKillGroup(this.workSkillGroupId, false);
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdateWordKillGroup(this.workSkillGroupId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteWorkSkillGroup(this.workSkillGroupId);
      }
    }
  }

  searchWorkSkill(WorkSkillGroupId: string) {
    this.model.WorkSkillGroupId = WorkSkillGroupId;
    this.workSkillService.searchWorkSkill(this.model).subscribe((data: any) => {
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

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên kỹ năng/kiến thức',
    Items: [
    ]
  };

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,

      Id: '',
      Name: '',
      Description: '',
    }
    this.searchWorkSkill(this.workSkillGroupId);
  }
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(WorkSkillCreateComponent, { container: 'body', windowClass: 'WorkSkill-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.WorkSkillGroupId = this.workSkillGroupId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchWorkSkill(this.workSkillGroupId);
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kỹ năng/kiến thức này không?").then(
      data => {
        this.delete(Id);
      },
      error => {
        
      }
    );
  }

  delete(Id: string) {
    this.workSkillService.deleteWorkSkill({ Id: Id }).subscribe(
      data => {
        this.searchWorkSkill(this.workSkillGroupId);
        this.messageService.showSuccess('Xóa kỹ năng/kiến thức thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  /* list cha con*/
  selectedWorkSkillId: '';
  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedWorkSkillId) {
      this.selectedWorkSkillId = e.selectedRowKeys[0];
      this.searchWorkSkill(e.selectedRowKeys[0]);
      this.workSkillGroupId = e.selectedRowKeys[0];

    }
  }


}
