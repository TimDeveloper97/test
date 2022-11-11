import { Component, OnInit } from '@angular/core';
import { AppSetting, Configuration, MessageService } from 'src/app/shared';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { RawMaterialService } from '../../services/rawmaterial-service';
import { RawmaterialCreateComponent } from '../rawmaterial-create/rawmaterial-create.component';

@Component({
  selector: 'app-rawmaterial-manage',
  templateUrl: './rawmaterial-manage.component.html',
  styleUrls: ['./rawmaterial-manage.component.scss']
})
export class RawmaterialManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private rawMaterialService: RawMaterialService
  ) { this.pagination = Object.assign({}, appSetting.Pagination); }

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
    OrderBy: 'Index',
    OrderType: true,

    Id: '',
    Name: '',
    Code:'',
    Index: '',
    Note: '',
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã vật liệu',
    Items: [
      {
        Name: 'Tên vật liệu',
        FieldName: 'Name',
        Placeholder: 'Nhập tên vật liệu',
        Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Vật liệu";
    this.searchRawMaterial();
  }

  searchRawMaterial() {
    this.rawMaterialService.searchRawMaterial(this.model).subscribe((data: any) => {
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
      OrderBy: 'Index',
      OrderType: true,

      Id: '',
      Name: '',
      Code:'',
      Index: '',
      Note: '',
    }
    this.searchRawMaterial();
  }

  loadPage(page: number) {
    if (page !== this.model.PageNumber) {
      this.model.PageNumber = page;
      this.searchRawMaterial();
    }
  }

  showConfirmDeleteRawMaterial(Id: string, Index: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật liệu này không?").then(
      data => {
        this.deleteRawMaterial(Id, Index);
      },
      error => {
        
      }
    );
  }

  deleteRawMaterial(Id: string, Index: string) {
    this.rawMaterialService.deleteRawMaterial({ Id: Id, Index: Index, LogUserId: this.logUserId }).subscribe(
      data => {
        this.searchRawMaterial();
        this.messageService.showSuccess('Xóa vật liệu thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(RawmaterialCreateComponent, { container: 'body', windowClass: 'rawmaterial-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchRawMaterial();
      }
    }, (reason) => {
    });
  }

}
