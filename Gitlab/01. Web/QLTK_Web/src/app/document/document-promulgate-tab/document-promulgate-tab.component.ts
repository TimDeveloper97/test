import { Component, Input, OnInit ,Output, EventEmitter} from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { DocumentService } from '../services/document.service';
import { DocumentPromulgateCreateComponent } from '../document-promulgate-create/document-promulgate-create.component';

@Component({
  selector: 'app-document-promulgate-tab',
  templateUrl: './document-promulgate-tab.component.html',
  styleUrls: ['./document-promulgate-tab.component.scss']
})
export class DocumentPromulgateTabComponent implements OnInit {

  @Output() onNewPromugate: EventEmitter<any> = new EventEmitter();
  @Input() documentId: string;

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private documentService: DocumentService,
    public constant: Constants,
    private dowloadservice: DownloadService,
    private config: Configuration,) { }

  searchOptions: any = {
    FieldContentName: 'Reason',
    Placeholder: 'Tìm kiếm theo lý do',
    Items: [
      {
        Name: 'Nội dung',
        FieldName: 'Content',
        Placeholder: 'Nhập nội dung',
        Type: 'text'
      },
    ]
  };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'PromulgateDate',
    OrderType: true,

    Reason: '',
    Content: '',
    DocumentId: '',
  };

  documentPromulgates: any[] = [];
  startIndex = 0;

  ngOnInit(): void {
    this.search();
  }

  search() {
    this.searchModel.DocumentId = this.documentId;
    this.documentService.searchPromulgate(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.documentPromulgates = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
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

    modelDowload.Name = "Ban hành tài liệu";
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

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá ban hành này không?").then(
      data => {
        this.delete(id);
      },
      error => {

      }
    );
  }

  delete(id: string) {
    this.documentService.deletePromulgate({ Id: id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa ban hành thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(DocumentPromulgateCreateComponent, { container: 'body', windowClass: 'promulgate-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.documentId = this.documentId;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
        this.onNewPromugate.emit();
      }
    }, (reason) => {
    });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'PromulgateDate',
      OrderType: false,

      Reason: '',
      Content: ''
    };
    this.search();
  }

}
