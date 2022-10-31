import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Configuration, MessageService, AppSetting, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { MaterialgroupCreateComponent } from '../materialgroup-create/materialgroup-create.component';

@Component({
  selector: 'app-materialgroup-manage',
  templateUrl: './materialgroup-manage.component.html',
  styleUrls: ['./materialgroup-manage.component.scss']
})
export class MaterialgroupManageComponent implements OnInit {

  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private constant: Constants,
    private materialGroupService: MaterialGroupService,
    public appSetting: AppSetting,
  ) { this.pagination = Object.assign({}, appSetting.Pagination); }

  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
  listTPA: any[] = [];
  logUserId: string;

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    MaterialGroupTPAId: '',
    MaterialGroupTPAName: '',
    MaterialGroupTPACode: '',
    Name: '',
    Code: '',
    ParentId: '',
    Description: '',
    Type: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã nhóm vật tư',
    Items: [
      {
        Name: 'Tên nhóm vật tư',
        FieldName: 'Name',
        Placeholder: 'Nhập tên nhóm vật tư',
        Type: 'text'
      },
      {
        Name: 'Nhóm TPA',
        FieldName: 'MaterialGroupTPAId',
        Placeholder: 'Nhóm TPA',
        Type: 'select',
        DataType: this.constant.SearchDataType.GroupTPA,
        DisplayName: 'Name',
        ValueName: 'Id'
      }
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm vật tư theo chức năng";
    this.getCbbTPA();
    this.searchMaterialGroup();
  }

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      MaterialGroupTPAId: '',
      Name: '',
      Code: '',
      ParentId: '',
      Description: '',
      Type: '',
    }
    this.searchMaterialGroup();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchMaterialGroup();
    }
  }

  searchMaterialGroup() {
    this.materialGroupService.searchMaterialGroup(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }


  getCbbTPA() {
    this.materialGroupService.getCbbTPA().subscribe((data: any) => {
      this.listTPA = data;
    });
  }

  showConfirmDeleteMaterialGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm vật tư này không?").then(
      data => {
        this.deleteMaterialGroupTPA(Id);
      },
      error => {
        
      }
    );
  }

  deleteMaterialGroupTPA(Id: string) {
    this.materialGroupService.deleteMaterialGroup({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        this.check=true;
        this.searchMaterialGroup();
        this.messageService.showSuccess('Xóa nhóm vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(MaterialgroupCreateComponent, { container: 'body', windowClass: 'materialgroup-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    if(this.selectedMaterialGroupId != null){
      activeModal.componentInstance.selectMaterialGroupId = this.selectedMaterialGroupId;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchMaterialGroup();
        this.check=true;
      }
    }, (reason) => {
    });
  }
  selectedMaterialGroupId:'';
  onSelectionChanged(e) {
    this.selectedMaterialGroupId = e.selectedRowKeys[0];
  }

  check: boolean = true;
  showChild(Id: string, show: boolean) {

    if (show == true) {
      for (let i of this.listDA) {
        if (i.ParentId == Id) {
          i.IsShow = '1';
        }
      }
      for (let i of this.listDA) {
        if (i.Id == Id) {
          i.IsOpen = '1';
        }
      }
      this.check = false;
    } else {
      for (let i of this.listDA) {
        if (i.ParentId == Id) {
          i.IsShow = '0';
        }
      }
      for (let i of this.listDA) {
        if (i.Id == Id) {
          i.IsOpen = '0';
        }
      }
      this.check = true;
    }
  }
}
