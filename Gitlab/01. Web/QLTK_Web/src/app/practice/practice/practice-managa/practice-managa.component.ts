import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, Constants, Configuration } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { PracticeService } from '../../service/practice.service';
import { Router } from '@angular/router';
import { DxTreeListComponent } from 'devextreme-angular/ui/tree-list';
import { PracticeGroupService } from '../../service/practice-group.service';
import { PracticeGroupCreateComponent } from '../practice-group-create/practice-group-create.component';
@Component({
  selector: 'app-practice-managa',
  templateUrl: './practice-managa.component.html',
  styleUrls: ['./practice-managa.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PracticeManagaComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private titleservice: Title,
    private servicePractice: PracticeService,
    private practiceGroupService: PracticeGroupService,
    public constant: Constants
  ) {
    this.items = [
      { Id: 1, text: 'Thêm mới nhóm bài thực hành/công đoạn', icon: 'fas fa-plus' },
      { Id: 2, text: 'Chỉnh sửa nhóm bài thực hành/công đoạn', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }
  items: any;
  StartIndex = 0;
  treeBoxValue: string;
  listData: any[] = [];
  listSkill: any[] = [];
  listSkillId: any[] = [];
  isDropDownBoxOpened = false;
  checked: boolean; // check tồn tại của SKill
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    SkillId: [],
    SkillName: '',
    ListIdSelect: [],
    PracticeGroupId: '',
    Index: '',
    MaterialConsumable: '',   // vật tư tiêu hao
    SupMaterial: '',          // thiết bị phụ trợ
    PracticeFile: false,   // tài liệu
    PracticeExist: '',
    PracaticeId: [],
    listSkillId: []
  }

  list_tickId = [];
  height = 0;

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên bài thực hành ...',
    Items: [
      {
        Name: 'Kĩ năng',
        FieldName: 'SkillId',
        Type: 'dropdown',
        SelectMode: 'multiple',
        DataType: this.constant.SearchDataType.Skill,
        Columns: [{ Name: 'Name', Title: 'Tên kĩ năng' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn kĩ năng'
      },
    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 150;
    this.appSetting.PageTitle = "Quản lý bài thực hành/công đoạn";
    this.searchPracticeGroup();

    this.getlistSkill();
    this.getListPracatice();
    this.selectedPracticeGroupId = localStorage.getItem("selectedPracticeGroupId");
    this.searchPractice(this.selectedPracticeGroupId);
    localStorage.removeItem("selectedPracticeGroupId");
  }

  listTempHeader: any = []
  searchPractice(practiceGroupId: string) {
    this.model.PracticeGroupId = practiceGroupId;
    this.servicePractice.searchPractice(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        this.listTempHeader = this.list_Skill_header;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getlistSkill() {
    this.servicePractice.searchSkill(this.model).subscribe((data: any) => {
      this.listSkill = data.ListResult;
      for (var item of this.listSkill) {
        this.listSkillId.push(item.Name);
      }
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
      SkillId: [],
      SkillName: '',
      ListIdSelect: [],
      PracticeGroupId: '',
      list_Skill_header: [],
    }
    this.listSkillId = [];
    this.treeBoxValue = '';
    this.model.SkillName = '';
    this.list_Skill_header = [];
    this.selectedRowKeys = [];
    this.searchPractice(this.practiceGroupId);
  }

  showConfirmDeletePractice(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá bài thực hành/công đoạn này không?").then(
      data => {
        localStorage.setItem("selectedPracticeGroupId", this.selectedPracticeGroupId);
        this.deletePractice(Id);
      },
      error => {
        
      }
    );
  }

  deletePractice(Id: string) {
    this.servicePractice.deletePractice({ Id: Id }).subscribe(
      data => {
        this.selectedPracticeGroupId = localStorage.getItem("selectedPracticeGroupId");
        this.searchPractice(this.selectedPracticeGroupId);
        this.messageService.showSuccess('Xóa bài thực hành/công đoạn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ExportExcel() {
    this.servicePractice.exportExcel(this.model).subscribe(d => {
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

  selectedRowKeys: any[] = [];
  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;
  }

  list_Skill_header = [];
  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys;
    this.model.SkillName = e.selectedRowKeys;
    // this.list_tickId = [];
    this.model.SkillId = [],
      this.list_Skill_header = [];
    e.selectedRowsData.forEach(item => {
      var isNew = true;
      var indexInHeader;

      this.list_Skill_header.forEach(element => {
        if (element.Id == item.Id) {
          isNew = false;
          indexInHeader = element.Index;
        }
      });
      if (isNew) {
        this.model.SkillId.push(item.Id);
        this.list_Skill_header.push(item)
      }
      else {
        this.list_Skill_header.slice(indexInHeader, 1);
      }
    });
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
    this.searchPractice(this.practiceGroupId);
  }

  showCreate() {
    localStorage.setItem("selectedPracticeGroupId", this.selectedPracticeGroupId);
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh/them-moi-bai-thuc-hanh']);
  }

  showUpdate(Id: string) {
    localStorage.setItem("selectedPracticeGroupId", this.selectedPracticeGroupId);
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh/chinh-sua-bai-thuc-hanh/', Id]);
  }

  //////// Practice Group

  ListPracticeGroupId = [];
  ListPracticeGroup: any[] = [];
  modelPracticeGroup: any = {
    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    ParentId: '',
    Description: '',
  }

  modelAll: any = {
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  practiceGroupId: '';
  selectedPracticeGroupId = '';
  searchPracticeGroup() {
    this.practiceGroupService.searchPracticeGroup(this.modelPracticeGroup).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListPracticeGroup = data.ListResult;
        this.ListPracticeGroup.unshift(this.modelAll);
        this.modelPracticeGroup.TotalItems = data.TotalItem;
        if (this.selectedPracticeGroupId == null) {
          this.selectedPracticeGroupId = this.ListPracticeGroup[0].Id;
        }

        this.treeView.selectedRowKeys = [this.selectedPracticeGroupId];
        for (var item of this.ListPracticeGroup) {
          this.ListPracticeGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    // this.selectedPracticeGroupId = e.selectedRowKeys[0];
    // this.searchPractice(e.selectedRowKeys[0]);
    // this.practiceGroupId = e.selectedRowKeys[0];

    if(e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedPracticeGroupId)
    {
      this.selectedPracticeGroupId = e.selectedRowKeys[0];
      this.searchPractice(e.selectedRowKeys[0]);
      this.practiceGroupId = e.selectedRowKeys[0];
    }
  }

  // popup thêm mới và chỉnh sửa
  showCreateUpdate(Id: string, isUpdate: boolean) {
    let activeModal = this.modalService.open(PracticeGroupCreateComponent, { container: 'body', windowClass: 'practicegroup-create-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.idUpdate = Id;
    } else {
      activeModal.componentInstance.parentId = Id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchPracticeGroup();
      }
    }, (reason) => {
    });
  }

  showConfirmDeletePracticeGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa nhóm bài thực hành/công đoạn này không?").then(
      data => {
        this.modelPracticeGroup.Id = Id;
        this.searchPracticeGroupById(Id);
      },
      error => {
        
      }
    );
  }

  //xoá nhóm bài thực hành
  listDAById: any[] = [];
  searchPracticeGroupById(Id: string) {
    this.practiceGroupService.searchPracticeGroup(this.modelPracticeGroup).subscribe((data: any) => {
      if (data) {
        this.listDAById = data;
        this.modelPracticeGroup.TotalItems = data.TotalItem;
        if (this.listDAById.length == 1) {
          this.deletePracticeGroup(Id);
        } else {
          this.messageService.showConfirm("Xóa nhóm bài thực hành/công đoạn này sẽ xóa hết cả các nhóm bài thực hành/công đoạn con thuộc nhóm, Bạn có chắc chắn muốn xóa không?").then(
            data => {
              this.deletePracticeGroup(Id);
            },
            error => {
              
            }
          );
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  deletePracticeGroup(Id: string) {
    this.practiceGroupService.deletePracticeGroup({ Id: Id }).subscribe(
      data => {
        //this.check=true;
        this.modelPracticeGroup.Id = '';
        this.searchPracticeGroup();
        this.messageService.showSuccess('Xóa nhóm nhóm bài thực hành/công đoạn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  /////////////////////////////////// multiselect bài thực hành
  //Combobox đa cấp
  treeBoxValue_Practice: string;
  isDropDownBoxOpened_Pracatice = false;
  selectedRowKeys_Pracatice: any[] = [];
  rowFocusIndex = -1;
  listPracaticeId = [];
  listPracatice = [];
  getListPracatice() {
    this.servicePractice.searchPractice(this.model).subscribe((data: any) => {
      this.listPracatice = data.ListResult;
      // lấy list id expandedRowKeys 
      for (var item of this.listPracatice) {
        this.listPracaticeId.push(item.Id);
      }
    });
  }

  syncTreeViewSelection_Pracatice() {
    if (!this.treeBoxValue_Practice) {
      this.selectedRowKeys_Pracatice = [];
    } else {
      this.selectedRowKeys_Pracatice = [this.treeBoxValue_Practice];
    }
  }

  treeView_itemSelectionChanged_Pracatice(e) {
    this.treeBoxValue_Practice = e.selectedRowKeys[0];
    this.model.PracaticeId = e.selectedRowKeys[0];
    this.model.PracaticeId = [];
    e.selectedRowsData.forEach(item => {
      this.model.PracaticeId.push(item.Id);
    });
  }

  closeDropDownBox_Pracatice() {
    this.isDropDownBoxOpened_Pracatice = false;
    this.searchPractice(this.practiceGroupId);
  }

  onRowDblClick_Pracatice() {
    this.isDropDownBoxOpened_Pracatice = false;
  }

  typeId: number;
  /// Skien click chuột phải
  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.showCreateUpdate(this.practiceGroupId, false);
    } else if (e.itemData.Id == 2) {
      this.showCreateUpdate(this.practiceGroupId, true);
    } else if (e.itemData.Id == 3) {
      this.showConfirmDeletePracticeGroup(this.practiceGroupId);
    }
  }
}
