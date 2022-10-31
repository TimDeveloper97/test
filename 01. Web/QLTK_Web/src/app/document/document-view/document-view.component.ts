import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, FileProcess, MessageService } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { CLIENT_RENEG_WINDOW } from 'tls';
import { DocumentService } from '../services/document.service';

@Component({
  selector: 'app-document-view',
  templateUrl: './document-view.component.html',
  styleUrls: ['./document-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DocumentViewComponent implements OnInit {
  @ViewChild('pdfViewer', { static: false }) public pdfViewer;

  constructor(private activeModal: NgbActiveModal,
    public constant: Constants,
    public appSetting: AppSetting,
    private sanitizer: DomSanitizer,
    private changeDetectorRef: ChangeDetectorRef,
    public fileProcess: FileProcess,
    private documentService: DocumentService,
    public messageService: MessageService,
    private downloadService: DownloadService) { }

  pdfSource: any = "";
  images: any[] = []

  id: number;
  documentCode: any;
  documentName: any;

  documentModel: any = {
    totalItems: 0,
    name: ''
  };

  documents: any[] = [];
  height: number = 0;
  isImg = false;
  fileViewIndex: number = -1;
  fileViewIndexView: number = 0;
  documentFiles: any[] = [];
  startIndex: number = 1;
  selectIndex: number = 0;

  ngOnInit(): void {
    this.height = window.innerHeight - 230;
    this.documentModel.DocumentId = this.id;
    this.searchDocument();
  }

  searchDocument() {
    this.documentService.searchDocumentFile(this.documentModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.documents = data.ListResult;

        if (this.documents.length > 0) {
          this.loadFiles(this.documents[0]);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  closeModal() {
    this.activeModal.close();
  }

  loadFiles(row: any) {
    var filePath = "";
    if (row.PDFPath != "" && row.PDFPath != null) {
      filePath = row.PDFPath;
    } else {
      filePath = row.Path;
    }

    this.downloadService.downloadFileBlob(filePath, row.FileName).subscribe(data => {
      var blob = new Blob([data], { type: 'octet/stream' });
      var url = window.URL.createObjectURL(blob);
      if (row.FileName.includes(".doc") || row.FileName.includes(".ppt") ||row.FileName.includes(".xls") || row.FileName.includes(".pdf")) {
        this.isImg = false;
        this.changeDetectorRef.detectChanges();
        this.pdfViewer.pdfSrc = blob;
        this.pdfViewer.refresh()
      } else if (row.FileName.includes(".jpg") || row.FileName.includes(".jpeg") || row.FileName.includes(".png")) {
        this.images = [];
        this.isImg = true;
        const blob = new Blob([data], { type: 'image/png' });
        var unsafeImg = window.URL.createObjectURL(blob);
        let img = this.sanitizer.bypassSecurityTrustUrl( unsafeImg);
        this.images.push(img);
      }
    }, error => {
      const blb = new Blob([error.error], { type: "text/plain" });
      const reader = new FileReader();

      reader.onload = () => {
        this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
      };
      // Start reading the blob as text.
      reader.readAsText(blb);
    });

  }

  downloadAFile(row: any) {
    this.fileProcess.downloadFileBlob(row.Path, row.FileName);
  }
}
