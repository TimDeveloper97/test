import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DocumentTypeService } from '../../services/document-type.service';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { DocumentTypeCreateComponent } from '../document-type-create/document-type-create.component';

@Component({
  selector: 'app-document-type-manage',
  templateUrl: './document-type-manage.component.html',
  styleUrls: ['./document-type-manage.component.scss']
})
export class DocumentTypeManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private documentTypeService: DocumentTypeService,
    public constant: Constants) { }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên/mã loại tài liệu',
    Items: [
      // {
      //   Name: 'Mã loại tài liệu',
      //   FieldName: 'Code',
      //   Placeholder: 'Nhập mã loại tài liệu',
      //   Type: 'text'
      // },
    ]
  };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Name: '',
    Code: '',
  };

  documentTypes: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý loại tài liệu";
    this.search();
  }

  search() {
    this.documentTypeService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.documentTypes = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteReason(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá loại tài liệu này không?").then(
      data => {
        this.deleteReason(Id);
      },
      error => {

      }
    );
  }

  deleteReason(Id: string) {
    this.documentTypeService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa loại tài liệu thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(DocumentTypeCreateComponent, { container: 'body', windowClass: 'documenttype-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,

      Name: '',
      Code: '',
    };
    this.search();
  }

}
