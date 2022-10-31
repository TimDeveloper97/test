import { Component, OnInit } from '@angular/core';
import { Constants, MessageService, AppSetting, Configuration } from 'src/app/shared';
import { ProjectRoleService } from '../../service/project-role.service';
import { RoleCreateUpdateComponent } from '../role-create-update/role-create-update.component';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-project-role',
  templateUrl: './project-role.component.html',
  styleUrls: ['./project-role.component.scss']
})
export class ProjectRoleComponent implements OnInit {
  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    public roleService: ProjectRoleService,
    private modalService: NgbModal,
    private messageService: MessageService,
    private config: Configuration,


  ) { }
  logUserId: string;
  listData: any[] = [];
 
  model: any = {
    OrderBy: 'Index',
    OrderType: true,

    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Name: '',
    IsDisable: '',
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên vị trí ',
    Items: [
      {
        Name: 'Tình trạng',
        FieldName: 'IsDisable',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.ModuleIsDisable,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  listDA: any[] = [];

  totalRole: 0;
  startIndex: number = 1;
  Status: string = "";
  ngOnInit(): void {
    this.appSetting.PageTitle = "Vị trí công việc trong dự án";
    this.searchRoles();
  }

  searchRoles() {
    this.roleService.searchRole(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      OrderBy: 'Index',
      OrderType: true,
  
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Name: '',
      IsDisable: '',
    }
    this.searchRoles();
  }


  showCreateUpdate(Id) {
    let activeModal = this.modalService.open(RoleCreateUpdateComponent, { container: 'body', windowClass: 'role-create-update-modal', backdrop: 'static' });
    activeModal.componentInstance.model.Id = Id;
    activeModal.result.then(result => {
      if (result) {
      }
      this.searchRoles();
    })
  }

  showConfirmDeleteRole(Id: string, IsDisable: boolean, Index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vị trí này trong dự án không?").then(
      data => {
        this.deleteRole(Id, IsDisable, Index);
        
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  deleteRole(Id, IsDisable, Index: number) {
    this.roleService.deleteRole({Id: Id, IsDisable: IsDisable, Index: Index, LogUserId: this.logUserId} ).subscribe(result => {
      this.searchRoles();
      this.messageService.showConfirm("Xóa vị trí thành công!")
    },
    error => {
      this.messageService.showError(error);
    });
    
  }

  ExportExcel() {
    this.roleService.ExportExcel(this.model).subscribe(d => {
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

}

