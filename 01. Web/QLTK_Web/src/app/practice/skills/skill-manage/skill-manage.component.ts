import { Component, OnInit, ViewChild } from '@angular/core';

import { AppSetting, Constants, MessageService, Configuration, ComboboxService} from 'src/app/shared';
import { Title } from '@angular/platform-browser';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SkillService } from '../service/skill.service';
import { SkillCreateComponent } from '../skill-create/skill-create.component';
import { SkillUpdateComponent } from '../skill-update/skill-update.component';
import { SkillGroupService } from '../../skillgroup/service/skill--group.service';
import { DxTreeListComponent } from 'devextreme-angular';
import { SkillGroupCreateComponent } from '../../skillgroup/skill-group-create/skill-group-create.component';

@Component({
  selector: 'app-skill-manage',
  templateUrl: './skill-manage.component.html',
  styleUrls: ['./skill-manage.component.scss']
})

export class SkillManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    private serviceSkill: SkillService,
    private combobox: ComboboxService,
    private skillGroupService: SkillGroupService
  ) {
    this.items = [
      { Id: 1, text: 'Thêm mới nhóm kỹ năng' ,icon: 'fas fa-plus'},
      { Id: 2, text: 'Chỉnh sửa nhóm kỹ năng', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }

  items: any;
  StartIndex = 0;
  listDA: any[] = [];
  listSkillGroup = [];
  ListSkillGroup: any[] = [];
  ListSkillGroupId = [];
  selectedSkillGroupId = '';
  SkillGroupId: '';

  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    SkillGroupId: '',
    Code: '',
    Name: '',
    CreateBy: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã kỹ năng',
    Items: [
      {
        Name: 'Tên kỹ năng',
        FieldName: 'Name',
        Placeholder: 'Nhập tên kỹ năng',
        Type: 'text'
      },
    ]
  };
  modelSkillGroup: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
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

  fileModel = {
    Id: '',
    SkillId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }

  height = 0;
  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Quản lý Kỹ năng";
    this.getCbSkillGroup();
    this.searchSkillGroup();
    this.searchSkill(this.SkillGroupId);
    this.selectedSkillGroupId = localStorage.getItem("selectedSkillGroupId");
    localStorage.removeItem("selectedSkillGroupId");
  }

  onSelectionChanged(e) {
    // this.selectedSkillGroupId = e.selectedRowKeys[0];
    // this.searchSkill(e.selectedRowKeys[0]);
    // this.SkillGroupId = e.selectedRowKeys[0];

    if(e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedSkillGroupId)
    {
      this.selectedSkillGroupId = e.selectedRowKeys[0];
      this.searchSkill(e.selectedRowKeys[0]);
      this.SkillGroupId = e.selectedRowKeys[0];
    }
  }

  searchSkillGroup() {
    this.skillGroupService.searchSkillGroup(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListSkillGroup = data.ListResult;
        this.ListSkillGroup.unshift(this.modelAll);
        this.modelSkillGroup.TotalItems = data.TotalItem;
        if (this.selectedSkillGroupId == null) {
          this.selectedSkillGroupId = this.ListSkillGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedSkillGroupId];
        for (var item of this.ListSkillGroup) {
          this.ListSkillGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchSkill(SkillGroupId: string) {
    this.model.SkillGroupId = SkillGroupId;
    this.serviceSkill.searchSkill(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCbSkillGroup() {
    this.combobox.getCbbSkillGroup().subscribe(
      data => {
        this.listSkillGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear(SkillGroupId: string) {
    this.model = {
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      SkillGroupId: '',
      Code: '',
      Name: '',
      CreateBy: '',
    }
    this.searchSkill(SkillGroupId);
  }

  showConfirmDeleteSkillGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm kỹ năng này không?").then(
      data => {
        this.deleteSkillGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteSkillGroup(Id: string) {
    this.skillGroupService.deleteSkillGroup({ Id: Id }).subscribe(
      data => {
        this.searchSkillGroup();
        this.searchSkill(this.SkillGroupId);
        this.messageService.showSuccess('Xóa nhóm kỹ năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteSkill(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá kỹ năng này không?").then(
      data => {
        this.deleteSkill(Id);
      },
      error => {
        
      }
    );
  }

  deleteSkill(Id: string) {
    this.serviceSkill.deleteSkill({ Id: Id }).subscribe(
      data => {
        this.searchSkill(this.SkillGroupId);
        this.messageService.showSuccess('Xóa kỹ năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreate(Id: string) {
    let activeModal = this.modalService.open(SkillCreateComponent, { container: 'body', windowClass: 'skillcreate-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.SkillGroupId = this.selectedSkillGroupId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSkill(this.SkillGroupId);
      }
    }, (reason) => {
    });
  }

  ShowUpdate(Id: string) {
    let activeModal = this.modalService.open(SkillUpdateComponent, { container: 'body', windowClass: 'skilldupdate-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSkill(this.SkillGroupId);
      }
    }, (reason) => {
    });
  }

  ShowCreateUpdate(Id: string, isUpdate: boolean) {
    let activeModal = this.modalService.open(SkillGroupCreateComponent, { container: 'body', windowClass: 'skill-group-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.idUpdate = Id;
    } else {
      activeModal.componentInstance.parentId = Id;
    }

    activeModal.result.then((result) => {
      if (result) {
        this.searchSkillGroup();
      }
    }, (reason) => {
    });
  }

  ExportExcel() {
    this.serviceSkill.exportExcel(this.model).subscribe(d => {
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

  typeId: number;
  /// Skien click chuột phải
  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.ShowCreateUpdate(this.SkillGroupId, false);
    } else if (e.itemData.Id == 2) {
      this.ShowCreateUpdate(this.SkillGroupId, true);
    } else if (e.itemData.Id == 3) {
      this.showConfirmDeleteSkillGroup(this.SkillGroupId);
    }
  }
}
