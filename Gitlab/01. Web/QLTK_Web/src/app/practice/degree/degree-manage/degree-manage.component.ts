import { Component, OnInit } from '@angular/core';
import { MessageService } from 'src/app/shared/services/message.service';
import { AppSetting } from 'src/app/shared/config/appsetting';

import { DegreeService } from '../../degree/service/degree.service';
import { Constants } from 'src/app/shared/common/Constants';
import { DegreeCreateComponent } from '../degree-create/degree-create.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-degree-manage',
  templateUrl: './degree-manage.component.html',
  styleUrls: ['./degree-manage.component.scss']
})
export class DegreeManageComponent implements OnInit {

 

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private degreeService: DegreeService,
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

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm mã trình độ ...',
    Items: [
      {
        Name: 'Tên trình độ',
        FieldName: 'Name',
        Placeholder: 'Nhập tên trình độ',
        Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý Trình độ";
    this.searchDegree();
  }
  searchDegree() {
    this.degreeService.searchDegree(this.model).subscribe((data: any) => {
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
    this.searchDegree();
  }
  
  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(DegreeCreateComponent, { container: 'body', windowClass: 'degree-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchDegree();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteDegree(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá trình độ này không?").then(
      data => {
        this.deleteDegree(Id);
      },
      error => {
        
      }
    );
  }

  deleteDegree(Id: string) {
    this.degreeService.deleteDegree({ Id: Id }).subscribe(
      data => {
        this.searchDegree();
        this.messageService.showSuccess('Xóa trình độ thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
