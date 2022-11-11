import { Component, OnInit } from '@angular/core';
import { ClassIficationService } from '../../service/class-ification.service';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ClassIficationCreateComponent } from '../class-ification-create/class-ification-create.component';

@Component({
  selector: 'app-class-ification-manage',
  templateUrl: './class-ification-manage.component.html',
  styleUrls: ['./class-ification-manage.component.scss']
})
export class ClassIficationManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private ificationService: ClassIficationService,
    public constant: Constants
  ) { }

  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'des',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    Index: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm mã xếp loại hoặc tên xếp loại...',
    Items: [
      {
        // Name: 'Tên xếp loại',
        // FieldName: 'Name',
        // Placeholder: 'Nhập tên xếp loại',
        // Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.appSetting.PageTitle = "Quản lý xếp loại";
    this.searchClassIfication();
  }
  searchClassIfication() {
    this.ificationService.searchClassIfication(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        //this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
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
      OrderBy: 'des',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
      Note: '',
      Index: '',
    }
    this.searchClassIfication();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ClassIficationCreateComponent, { container: 'body', windowClass: 'ification-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchClassIfication();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteIfication(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá xếp loại này không?").then(
      data => {
        this.deleteIfication(Id);
      },
      error => {
        
      }
    );
  }

  deleteIfication(Id: string) {
    this.ificationService.deleteClassIfication(Id).subscribe(
      data => {
        this.searchClassIfication();
        this.messageService.showSuccess('Xóa xếp loại thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
