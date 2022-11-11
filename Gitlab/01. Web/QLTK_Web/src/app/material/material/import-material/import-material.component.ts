import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MaterialService } from '../../services/material-service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { MessageService, Configuration, FileProcess } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-import-material',
  templateUrl: './import-material.component.html',
  styleUrls: ['./import-material.component.scss']
})
export class ImportMaterialComponent implements OnInit {

  modalInfo = {
    Title: 'Import vật tư'
  }

  importModel: any = {
    Path: ''
  }

  nameFile = "";
  fileToUpload: File = null;

  @ViewChild('fileInput',{static:false})
  myInputVariable: ElementRef;

  constructor(
    private messageService: MessageService,
    private uploadService: UploadfileService,
    private service: MaterialService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    public fileProcess: FileProcess,
  ) { }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.nameFile = files.item(0).name;
  }

  uploadFile() {
    this.uploadService.uploadFile(this.fileToUpload, 'Material/').subscribe(
      data => {
        if (data != null) {
          this.importModel.Link = data.FullFileUrl;
          if (this.importModel.Link != '') {
            this.service.importFileMaterial(this.importModel).subscribe((event: any) => {
              if (event.Message == null) {
                this.messageService.showSuccess("Import vật tư thành công!");
                this.activeModal.close(true);
              }
              else {
                this.messageService.showMessage(event.Message);
                this.nameFile = '';
              }

            }, error => {
              this.messageService.showMessage(error.error);
              this.nameFile = '';
            });
          }
        } else {
          this.messageService.showMessage(data.mess);
          this.nameFile = '';
        }

      },
      error => {
        this.messageService.showMessage(error);
        this.nameFile = '';
      }
    );
  }

  save() {
    if (this.myInputVariable.nativeElement.value == "") {
      this.messageService.showMessage("Bạn chưa chọn file import!");
    }
    else {
      var check = this.myInputVariable.nativeElement.value;
      var testcheck1 = ".xls";
      var testcheck2 = ".xlsx";
      if (check.lastIndexOf(testcheck1) == -1 && check.lastIndexOf(testcheck2) == -1) {
        this.messageService.showMessage("Bạn phải chọn file có định dạng .xls hoặc .xlsx");
      }
      else {
        this.uploadFile();
        this.myInputVariable.nativeElement.value = "";
      }
    }
  }

  closeModal() {
    this.activeModal.close(false);
  }

  downloadTemplate() {
    this.service.getGroupInTemplate(this.importModel).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

}
