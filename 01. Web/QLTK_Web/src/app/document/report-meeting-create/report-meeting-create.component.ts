import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { DocumentMeetingService } from '../services/document-meeting.service';

@Component({
  selector: 'app-report-meeting-create',
  templateUrl: './report-meeting-create.component.html',
  styleUrls: ['./report-meeting-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportMeetingCreateComponent implements OnInit {

  constructor(public fileProcess: FileProcess,
    private fileService: UploadfileService,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    private documentMeetingService:DocumentMeetingService) { }

  isAction: boolean = false;
  id: string = '';
  documentId: string;
  modalInfo = {
    Title: 'Thêm mới biên bản họp',
    SaveText: 'Lưu',
  };

  meetingDate: any = null;

  meetingModel: any = {
    Id: '',
    DocumentId: '',
    MeetingDate: null,
    MeetingFiles: []
  };

  fileModel: any = {
    FileName: '',
    FilePath: '',
    PDFPath: '',
    FileSize: null,
  };
  documentFilesUpload: any = [];

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm biên bản họp';
      this.modalInfo.SaveText = 'Lưu';

    }
    else {
      this.meetingModel.DocumentId = this.documentId;
      this.modalInfo.Title = 'Thêm mới nhóm biên bản họp';
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue: boolean) { }

  uploadFileClick($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var filesExist = "";

    for (var item of fileDataSheet) {
      var file = Object.assign({}, this.fileModel);
      file.FileName = item.name;
      file.FileSize = item.size;
      file.CreateDate = new Date();

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


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
