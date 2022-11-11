import { Component, OnInit } from '@angular/core';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SpecializeService } from '../service/specialize.service';
import { SpecializeCreateComponent } from '../specialize-create/specialize-create.component';


@Component({
  selector: 'app-specialize-manage',
  templateUrl: './specialize-manage.component.html',
  styleUrls: ['./specialize-manage.component.scss']
})
export class SpecializeManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private specializeService: SpecializeService,
    public constant: Constants
  ) { }

  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: ''
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý Chuyên môn";
    this.searchSpecialize();
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã chuyên môn',
    Items: [
      {
        Name: 'Tên chuyên môn',
        FieldName: 'Name',
        Placeholder: 'Nhập tên chuyên môn',
        Type: 'text'
      },
    ]
  };
  searchSpecialize() {
    this.specializeService.searchSpecialize(this.model).subscribe((data: any) => {
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
      Code: '',
      Description: '',
    }
    this.searchSpecialize();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SpecializeCreateComponent, { container: 'body', windowClass: 'specialize-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSpecialize();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteSpecialize(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chuyên môn này không?").then(
      data => {
        this.deleteSpecialize(Id);
      },
      error => {
        
      }
    );
  }

  deleteSpecialize(Id: string) {
    this.specializeService.deleteSpecialize({ Id: Id }).subscribe(
      data => {
        this.searchSpecialize();
        this.messageService.showSuccess('Xóa chuyên môn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
