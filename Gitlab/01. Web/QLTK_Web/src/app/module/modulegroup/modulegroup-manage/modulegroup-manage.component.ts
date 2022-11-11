import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Configuration, MessageService, AppSetting } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { ModuleGroupService } from '../../services/module-group-service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ModuleGroupCreateComponent } from '../module-group-create/module-group-create.component';

@Component({
  selector: 'app-modulegroup-manage',
  templateUrl: './modulegroup-manage.component.html',
  styleUrls: ['./modulegroup-manage.component.scss']
})
export class ModulegroupManageComponent implements OnInit {

  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private appSetting: AppSetting,
    private moduleGroupService: ModuleGroupService
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
    Name: '',
    Code: '',
    ParentId: '',
    Description: '',
    Note: ''
  }

  ngOnInit() {
    
    this.appSetting.PageTitle = "Nhóm module";
    this.searchModuleGroup();
  }

  searchModuleGroup() {
    this.moduleGroupService.searchModuleGroup(this.model).subscribe((data: any) => {
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

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      ParentId: '',
      Description: '',
      Note: ''
    }
    this.searchModuleGroup();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchModuleGroup();
    }
  }

  //xoá nhóm module
  listDAById: any[] = [];
  searchModuleGroupById(Id: string) {
    this.moduleGroupService.searchModuleGroupById(this.model).subscribe((data: any) => {
      if (data) {
        //this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDAById = data;
        //this.model.totalItems = data.TotalItem;
        if (this.listDAById.length == 1) {
          this.deleteModuleGroup(Id);
        } else {
          this.messageService.showConfirm("Xóa nhóm module này sẽ xóa hết cả các nhóm module con thuộc nhóm, Bạn có chắc chắn muốn xóa không?").then(
            data => {
              this.deleteModuleGroup(Id);
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

  showConfirmDeleteModuleGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa nhóm module này không?").then(
      data => {
        this.model.Id = Id;
        this.searchModuleGroupById(Id);
      },
      error => {
        
      }
    );
  }

  deleteModuleGroup(Id: string) {
    this.moduleGroupService.deleteModuleGroup({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        //this.check=true;
        this.model.Id = '';
        this.searchModuleGroup();
        this.messageService.showSuccess('Xóa nhóm module thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  // popup thêm mới và chỉnh sửa
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ModuleGroupCreateComponent, { container: 'body', windowClass: 'modulegroupgreate-model', backdrop: 'static' })
    activeModal.componentInstance.idUpdate = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchModuleGroup();
      }
    }, (reason) => {
    });
  }

}
