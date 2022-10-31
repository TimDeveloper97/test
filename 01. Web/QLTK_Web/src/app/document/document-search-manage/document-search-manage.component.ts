import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { DocumentGroupService } from '../services/document-group.service';
import { DocumentService } from '../services/document.service';
import { DocumentViewComponent } from '../document-view/document-view.component';

@Component({
  selector: 'app-document-search-manage',
  templateUrl: './document-search-manage.component.html',
  styleUrls: ['./document-search-manage.component.scss']
})
export class DocumentSearchManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private router: Router,
    private documentGroupService: DocumentGroupService,
    private documentService: DocumentService,
    private dowloadservice: DownloadService,
    private config: Configuration,
    private dateUtils: DateUtils) { }

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
    Status: 1
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
    TotalItems: 8,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Name: '',
    Code: '',
    Keyword: '',
    Status: 1
  }

  startIndex: number = 1;

  documents: any[] = [];
  documentGroups: any[] = [];
  expandGroupKeys: any[] = [];
  selectGroupKeys: any[] = [];
  height = 0;
  selectedGroupId: any = '';


  ngOnInit(): void {
    this.appSetting.PageTitle = "Tra cứu tài liệu";
    this.height = window.innerHeight - 140;
    this.searchDocumentGroup();
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
    this.documentService.documentSearch(this.documentSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.documents = data.ListResult;
        this.documentSearchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
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
      Keyword: '',
      Status: 1
    };
    this.searchDocument();
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
