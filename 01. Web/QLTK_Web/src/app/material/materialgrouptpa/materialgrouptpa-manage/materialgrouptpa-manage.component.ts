import { Component, OnInit } from '@angular/core';
import { AppSetting, Configuration, MessageService } from 'src/app/shared';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { MaterialGroupTPAService } from '../../services/materialgrouptpa-service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { MaterialgrouptpaCreateComponent } from '../materialgrouptpa-create/materialgrouptpa-create.component';

@Component({
  selector: 'app-materialgrouptpa-manage',
  templateUrl: './materialgrouptpa-manage.component.html',
  styleUrls: ['./materialgrouptpa-manage.component.scss']
})
export class MaterialgrouptpaManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private materialGroupTPAService: MaterialGroupTPAService) { this.pagination = Object.assign({}, appSetting.Pagination); }


  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];
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
    Description: '',
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm vật tư TPA";
    this.searchMaterialGroupTPA();
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã nhóm vật tư',
    Items: [
      {
        Name: 'Tên nhóm vật tư',
        FieldName: 'Name',
        Placeholder: 'Nhập tên nhóm vật tư',
        Type: 'text'
      },
    ]
  };

  searchMaterialGroupTPA() {
    this.materialGroupTPAService.searchMaterialGroupTPA(this.model).subscribe((data: any) => {
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
      Description: '',
    }
    this.searchMaterialGroupTPA();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchMaterialGroupTPA();
    }
  }

  showConfirmDeleteMaterialGroupTPA(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm vật tư TPA này không?").then(
      data => {
        this.deleteMaterialGroupTPA(Id);
      },
      error => {
        
      }
    );
  }

  deleteMaterialGroupTPA(Id: string) {
    this.materialGroupTPAService.deleteMaterialGroupTPA({ Id: Id, LogUserId: this.logUserId }).subscribe(
      data => {
        this.searchMaterialGroupTPA();
        this.messageService.showSuccess('Xóa nhóm vật tư TPA thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(MaterialgrouptpaCreateComponent, { container: 'body', windowClass: 'materialgrouptpa-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchMaterialGroupTPA();
      }
    }, (reason) => {
    });
  }

}
