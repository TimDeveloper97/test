import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { SbuService } from '../../service/sbu.service';
import { SBUCreateComponent } from '../sbucreate/sbucreate.component';

@Component({
  selector: 'app-sbumanage',
  templateUrl: './sbumanage.component.html',
  styleUrls: ['./sbumanage.component.scss']
})
export class SBUManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    private serviceSBU: SbuService
  ) { }

  startIndex = 0;
  listData: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Status: '',
    Location: '',
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý SBU";
    this.searchSBU();
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã SBU',
    Items: [
      {
        Name: 'Tên SBU',
        FieldName: 'Name',
        Placeholder: 'Nhập tên SBU',
        Type: 'text'
      },
      {
        Name: 'Trạng thái',
        FieldName: 'Status',
        Placeholder: 'Trạng thái',
        Type: 'select',
        Data: this.constant.StatusSBU,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  searchSBU() {
    this.serviceSBU.searchSBU(this.model).subscribe((data: any) => {
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
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
      Status: '',
      Location: '',
    }
    this.searchSBU();
  }

  showConfirmDeleteSBU(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá SBU này không?").then(
      data => {
        this.deleteSBU(Id);
      },
      error => {
        
      }
    );
  }

  deleteSBU(Id: string) {
    this.serviceSBU.deleteSBU({ Id: Id }).subscribe(
      data => {
        this.searchSBU();
        this.messageService.showSuccess('Xóa SBU thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SBUCreateComponent, { container: 'body', windowClass: 'sbu-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSBU();
      }
    }, (reason) => {
    });
  }

}
