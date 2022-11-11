import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MessageService } from '../../services/message.service';
import { Configuration } from '../../config/configuration';
import { FileProcess } from '../../common/file-process';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-import-excel',
  templateUrl: './import-excel.component.html',
  styleUrls: ['./import-excel.component.scss']
})
export class ImportExcelComponent implements OnInit {

  modalInfo = {
    Title: 'Chọn file import dữ liệu'
  }

  importModel: any = {
    Path: ''
  }

  nameFile = "";
  fileToUpload: any = {};
  isData: boolean = false;
  getData: any;
  isGetData:boolean = false;
  @ViewChild('fileInput', { static: false })
  myInputVariable: ElementRef;
  templatePath: string = '';

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    public fileProcess: FileProcess,
  ) { }

  ngOnInit() {
  }

  handleFileInput(event) {
    this.fileProcess.readDataFile(event).subscribe((data: any) => {
      this.fileToUpload = data;
      this.nameFile = data.Name;
    });
  }

  import() {
    if (this.myInputVariable.nativeElement.value == "") {
      this.messageService.showMessage("Bạn chưa chọn file import!");
    }
    else {
      if (this.isData) {
        this.activeModal.close(this.fileToUpload.Data);
      }
      else {
        this.activeModal.close(this.fileToUpload.File);
      }

    }
  }

  closeModal() {
    this.activeModal.close(false);
  }

  download() {
    if (this.isGetData) {
      this.getData().subscribe(
        result => {
          this.downloadFile(this.config.ServerApi + result);
        }
      );
    }
    else {
      this.downloadFile(this.templatePath);
    }
  }

  downloadFile(path) {
    var link = document.createElement('a');
    link.setAttribute("type", "hidden");
    // link.href = this.templatePath;
    link.href = path;
    link.download = 'Download.zip';
    document.body.appendChild(link);
    // link.focus();
    link.click();
    document.body.removeChild(link);
  }

}
