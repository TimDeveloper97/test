import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { DownloadService } from 'src/app/shared/services/download.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { DocumentMeetingService } from '../services/document-meeting.service';
import { DocumentService } from '../services/document.service';
import { DocumentReviewCreateComponent } from '../document-review-create/document-review-create.component';
import { forkJoin } from 'rxjs';
import { element } from 'protractor';

@Component({
  selector: 'app-document-review-tab',
  templateUrl: './document-review-tab.component.html',
  styleUrls: ['./document-review-tab.component.scss']
})
export class DocumentReviewTabComponent implements OnInit {
  @Input() documentId: string;
  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private documentService: DocumentService,
    public constants: Constants,
    private dowloadservice: DownloadService,
    private config: Configuration,
    private documentMeetingService: DocumentMeetingService,
    public fileProcess: FileProcess,
    private fileService: UploadfileService,
    public dateUtils: DateUtils,) { }

  searchModel: any = {
    Page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'ProblemDate',
    OrderType: false,

    Problem: '',
    ProblemDate: '',
    Status: null,
    DocumentId: ''
  };

  reviews: any[] = [];
  reportReviewFile: any[] = [];

  searchOptions: any = {
    FieldContentName: 'Problem',
    Placeholder: 'Tìm kiếm nội dung cần chỉnh sửa',
    Items: [
      {
        Name: 'Thời gian dự kiến hoàn thành',
        FieldNameTo: 'ProblemDateFromV',
        FieldNameFrom: 'ProblemDateToV',
        Type: 'date'
      },
    ]
  };
  startIndex = 1;
  documentMeetings: any[] = [];

  ngOnInit(): void {
    this.searchProblem();
    this.searchMeeting();
  }

  searchMeeting() {
    this.documentMeetingService.search({ DocumentId: this.documentId }).subscribe((data: any) => {
      if (data.ListResult) {
        this.meetingModel.MeetingFiles = data.ListResult;
        this.meetingModel.MeetingFiles.forEach(element => {
          if (element.MeetingDate != null) {
            element.MeetingDateV = this.dateUtils.convertDateToObject(element.MeetingDate);
          }
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchProblem() {
    this.searchModel.DocumentId = this.documentId;
    this.documentService.searchProblem(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.reviews = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(id: any) {
    let activeModal = this.modalService.open(DocumentReviewCreateComponent, { container: 'body', windowClass: 'document-review-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.documentId = this.documentId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchProblem();
      }
    }, (reason) => {
    });
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá review này không?").then(
      data => {
        this.delete(id);
      },
      error => {

      }
    );
  }

  delete(id: string) {
    this.documentService.deleteProblem({ Id: id }).subscribe(
      data => {
        this.searchProblem();
        this.messageService.showSuccess('Xóa review thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.searchModel = {
      Page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'ProblemDate',
      OrderType: false,

      Problem: '',
      ProblemDate: '',
      Status: null,
      DocumentId: ''
    };
    this.searchProblem();
  }


  fileModel: any = {
    FileName: '',
    Path: '',
    PDFPath: '',
    FileSize: null,
    MeetingDateV: null,
    MeetingDate: null,
  };
  documentFilesUpload: any[] = [];

  meetingModel: any = {
    DocumentId: '',
    MeetingFiles: [],
  }

  uploadFileClick($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var filesExist = "";

    for (var item of fileDataSheet) {
      var file = Object.assign({}, this.fileModel);
      file.FileName = item.name;
      file.FileSize = item.size;
      file.MeetingDateV = this.dateUtils.getDateNowToObject();

      var fileExist = this.meetingModel.MeetingFiles.find(a => a.FileName == item.name && a.FileSize == item.size);
      if (fileExist != null) {
        filesExist = fileExist.FileName + ", " + filesExist;
      } else {
        this.documentFilesUpload.push(item);
        this.meetingModel.MeetingFiles.push(file);
      }
    }
    if (filesExist != "") {
      this.messageService.showMessage("File: " + filesExist + " đã tồn tại");
    }
  }

  deleteFile(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file này không?").then(
      data => {
        if (this.meetingModel.MeetingFiles.length > 0) {
          this.meetingModel.MeetingFiles.splice(index, 1);
        }
      },
      error => {
      }
    );
  }



  saveMeetingFile() {
    this.meetingModel.DocumentId=this.documentId;
    let isOk = true;
    let meetingDateValid = this.meetingModel.MeetingFiles.filter(a => a.MeetingDateV == null);
    if (meetingDateValid.length > 0) {
      this.messageService.showMessage("Vui lòng nhập đẩy đủ thời gian họp");
      isOk = false;
    }

    if (isOk) {
      this.meetingModel.MeetingFiles.forEach(element => {
        if (element.MeetingDateV != null) {
          element.MeetingDate = this.dateUtils.convertObjectToDate(element.MeetingDateV);
        }
      });

      if (this.documentFilesUpload.length > 0) {
        let meetingFiles = this.fileService.uploadListFile(this.documentFilesUpload, 'MeetingFiles/');
        forkJoin([meetingFiles]).subscribe(results => {
          if (results[0].length > 0) {
            results[0].forEach(item => {
              var file = this.meetingModel.MeetingFiles.find(a => a.FileName == item.FileName && a.FileSize == item.FileSize);
              file.Path = item.FileUrl;
            });
          }
          this.updateMeetingFile();
        });
      } else {
        this.updateMeetingFile();
      }


    }
  }

  downloadAFile(row: any) {
    this.fileProcess.downloadFileBlob(row.Path, row.FileName);
  }

  updateMeetingFile() {
    this.documentMeetingService.update(this.meetingModel).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật review thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

}
