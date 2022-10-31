import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { element } from 'protractor';
import { AppSetting, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { DocumentGroupService } from '../services/document-group.service';
import { DocumentService } from '../services/document.service';
import { DocumentGroupCreateComponent } from '../document-group-create/document-group-create.component';
import { DocumentViewComponent } from '../document-view/document-view.component';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-document-manage',
  templateUrl: './document-manage.component.html',
  styleUrls: ['./document-manage.component.scss']
})
export class DocumentManageComponent implements OnInit {
  items: any;
  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private router: Router,
    private documentGroupService: DocumentGroupService,
    private documentService: DocumentService,
    private dowloadservice: DownloadService,
    private config: Configuration,
    private dateUtils: DateUtils) {
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }

  documentSearchModel: any = {
    Page: 1,
    PageSize: 10,
    TotalItems: 8,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Name: '',
    Code: '',
    Keyword: '',
    Status: null
  };

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo Tên/ Mã',
    Items: [
      {
        Name: 'Từ khóa',
        FieldName: 'Keyword',
        Placeholder: 'Nhập từ khóa',
        Type: 'text'
      },
      {
        Name: 'Trạng thái',
        FieldName: 'Status',
        Placeholder: 'Trạng thái',
        Type: 'select',
        Data: this.constant.DocumentStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Loại ban hành',
        FieldName: 'CompilationType',
        Placeholder: 'Loại ban hành',
        Type: 'select',
        Data: this.constant.CompilationType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      // {
      //   Name: 'SBU',
      //   FieldName: 'SBUId',
      //   Type: 'dropdown',
      //   SelectMode: 'single',
      //   DataType: this.constant.SearchDataType.SBU,
      //   Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
      //   DisplayName: 'Code',
      //   ValueName: 'Id',
      //   Placeholder: 'Chọn SBU',
      //   RelationIndexTo: 3,
      //   IsRelation: true,
      // },
      {
        Name: 'Nhà cung cấp ban hành',
        FieldName: 'CompilationSuppliserId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Supplier,
        Columns: [{ Name: 'Code', Title: 'Mã nhà cung cấp' }, { Name: 'Name', Title: 'Tên nhà cung cấp' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn nhà cung cấp',
        // RelationIndexFrom: 2,
        // IsRelation: true,
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn phòng ban',
        // RelationIndexFrom: 2,
        // IsRelation: true,
      },
      {
        Name: 'Loại tài liệu',
        FieldName: 'DocumentTypeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.DocumentType,
        Columns: [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn loại tài liệu',
      },
      {
        Name: 'Ngày ban hành',
        FieldNameFrom: 'PromulgateLastDateFromV',
        FieldNameTo: 'PromulgateLastDateToV',
        Type: 'date',
      },
      {
        Name: 'Ngày phê duyệt',
        FieldNameFrom: 'PromulgateDateFromV',
        FieldNameTo: 'PromulgateDateToV',
        Type: 'date',
      },
      {
        Name: 'Thời gian hiệu lực',
        FieldNameFrom: 'EffectiveDateFromV',
        FieldNameTo: 'EffectiveDateToV',
        Type: 'date',
      }
    ]
  };

  documentGroupSearchModel: any = {
    Page: 1,
    PageSize: 10,
    TotalItems: 5,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Name: '',
    Code: '',
  }

  startIndex: number = 1;

  documents: any[] = [];
  documentGroups: any[] = [];
  expandGroupKeys: any[] = [];
  selectGroupKeys: any[] = [];
  height = 0;
  selectedGroupId: any = '';
  toDay : any;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý tài liệu";
    this.height = window.innerHeight - 140;
    this.searchDocumentGroup();
    this.toDay = formatDate(new Date(),'yyyy-MM-dd','en_US');
  }

  searchDocumentGroup() {
    this.documentGroupService.search(this.documentGroupSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.documentGroups = data.ListResult;
        let modelAll: any = {
          Id: '',
          Name: 'Tất cả',
          Code: '',
        }
        this.documentGroups.unshift(modelAll);
        this.documentGroupSearchModel.TotalItems = data.TotalItem;

        this.searchDocument();
        this.selectGroupKeys = [this.selectedGroupId];
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchDocument() {

    if (this.documentSearchModel.PromulgateDateFromV) {
      this.documentSearchModel.PromulgateDateFrom = this.dateUtils.convertObjectToDate(this.documentSearchModel.PromulgateDateFromV);
    }

    if (this.documentSearchModel.PromulgateDateToV) {
      this.documentSearchModel.PromulgateDateTo = this.dateUtils.convertObjectToDate(this.documentSearchModel.PromulgateDateToV);
    }

    if (this.documentSearchModel.PromulgateLastDateFromV) {
      this.documentSearchModel.PromulgateLastDateFrom = this.dateUtils.convertObjectToDate(this.documentSearchModel.PromulgateLastDateFromV);
    }

    if (this.documentSearchModel.PromulgateLastDateToV) {
      this.documentSearchModel.PromulgateLastDateTo = this.dateUtils.convertObjectToDate(this.documentSearchModel.PromulgateLastDateToV);
    }

    if (this.documentSearchModel.EffectiveDateFromV) {
      this.documentSearchModel.EffectiveDateFrom = this.dateUtils.convertObjectToDate(this.documentSearchModel.EffectiveDateFromV);
    }

    if (this.documentSearchModel.EffectiveDateToV) {
      this.documentSearchModel.EffectiveDateTo = this.dateUtils.convertObjectToDate(this.documentSearchModel.EffectiveDateToV);
    }


    this.documentSearchModel.DocumentGroupId = this.selectedGroupId;
    this.documentService.manageSearch(this.documentSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.documents = data.ListResult;
        this.documents.forEach(element =>{
          if(element.ReviewDateTo !=null){
            if(this.toDay <= formatDate(element.ReviewDateTo,'yyyy-MM-dd','en_US')){
              var date = new Date();
              date.setDate(date.getDate()+30);
              if(formatDate(date,'yyyy-MM-dd','en_US') <=formatDate(element.ReviewDateTo,'yyyy-MM-dd','en_US')){
                element.checkDate =0;
              }else{
                element.checkDate =2;
              }
            }else{
              element.checkDate =1;
            }
          }else{
            element.checkDate =0;
          }
          
        });
        this.documentSearchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdateGroup(id: any, isUpdate: boolean) {
    let activeModal = this.modalService.open(DocumentGroupCreateComponent, { container: 'body', windowClass: 'document-group-create-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.id = this.selectedGroupId;
    } else {
      activeModal.componentInstance.parentId = id;
    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchDocumentGroup();
      }
    }, (reason) => {
    });
  }

  showCreateUpdate(id: any) {
    localStorage.setItem("selectedDocumentGroupId", this.selectedGroupId);
    if (id) {
      this.router.navigate(['tai-lieu/quan-ly-tai-lieu/chinh-sua', id]);
    } else {
      this.router.navigate(['tai-lieu/quan-ly-tai-lieu/them-moi']);
    }
  }

  clear() {
    this.documentSearchModel = {
      Page: 1,
      PageSize: 10,
      TotalItems: 8,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Name: '',
      Code: '',
      Status: null
    };
    this.searchDocument();
  }

  itemClick(e) {
    if (this.selectedGroupId == '' || this.selectedGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm tài liệu!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateGroup(this.selectedGroupId, false);
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdateGroup(this.selectedGroupId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteDocumentGroup(this.selectedGroupId);
      }
    }
  }

  showConfirmDeleteDocumentGroup(groupId: any) {
    this.messageService.showConfirm("Bạn có chắc muốn nhóm tài liệu này không?").then(
      data => {
        this.deleteDocumentGroup(groupId);
      },
      error => {

      }
    );
  }

  deleteDocumentGroup(groupId: any) {
    this.documentGroupService.delete({ Id: groupId }).subscribe(
      data => {
        this.searchDocumentGroup();
        this.messageService.showSuccess('Xóa nhóm tài liệu thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteDocument(id: any) {
    this.messageService.showConfirm("Bạn có chắc muốn tài liệu này không?").then(
      data => {
        this.deleteDocument(id);
      },
      error => {

      }
    );
  }

  deleteDocument(id: any) {
    this.documentService.delete({ Id: id }).subscribe(
      data => {
        this.searchDocument();
        this.messageService.showSuccess('Xóa tài liệu thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedGroupId) {
      this.selectedGroupId = e.selectedRowKeys[0];
      this.searchDocument();
      localStorage.setItem("selectedGroupId", this.selectedGroupId);
    }
  }

  downAllDocumentProcess(Datashet: any) {
    if (Datashet.length <= 0) {
      this.messageService.showMessage("Không có file để tải");
      return;
    }
    var listFilePath: any[] = [];
    Datashet.forEach(element => {
      listFilePath.push({
        Path: element.Path,
        FileName: element.FileName
      });
    });

    let modelDowload: any = {
      Name: '',
      ListDatashet: []
    }

    modelDowload.Name = "Tài liệu";
    modelDowload.ListDatashet = listFilePath;
    this.dowloadservice.downloadAll(modelDowload).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerFileApi + data.PathZip;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  viewDocument(document: any) {
    let activeModal = this.modalService.open(DocumentViewComponent, { container: 'body', windowClass: 'document-viewpdf-file-modal', backdrop: 'static' })
    activeModal.componentInstance.id = document.Id;
    activeModal.componentInstance.documentName = document.Name;
    activeModal.componentInstance.documentCode = document.Code;
    activeModal.result.then((result: any) => {
      if (result) {
      }
    });
  }

}
